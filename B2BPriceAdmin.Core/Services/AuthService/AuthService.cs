using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using B2BPriceAdmin.Core.Common.PagedResponse;
using B2BPriceAdmin.Core.Interfaces;
using B2BPriceAdmin.Database;
using B2BPriceAdmin.Database.Entities;
using B2BPriceAdmin.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace B2BPriceAdmin.Core.Services
{
    public class AuthService : IAuthService
    {
        protected readonly B2BPriceDbContext _db;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private int _expiryHours = 1;

        public AuthService(B2BPriceDbContext db, RoleManager<Role> roleManager, UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager, IConfiguration configuration, IEmailService emailService)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailService = emailService;
        }
        public async Task<Response<AuthResponseDTO>> Authenticate(LoginDTO loginCredentials)
        {
            var user = await _userManager.FindByNameAsync(loginCredentials.Email);
            if (user == null || user.Deleted == true)
            {
                return Response<AuthResponseDTO>.Fail("Invalid Request");
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = 401;
                return Response<AuthResponseDTO>.Fail("Email is not confirmed");
            }

            if (!await _userManager.CheckPasswordAsync(user, loginCredentials.Password))
            {
                await _userManager.AccessFailedAsync(user);

                if (await _userManager.IsLockedOutAsync(user))
                {
                    var content = $@"Your account is locked out. To reset the password click this link: {_configuration["APP:ClientURL"]}";

                    try
                    {
                        await _emailService.SendEmailAsync(new List<string> { loginCredentials.Email }, "Locked out account information", content);
                    }
                    catch (Exception)
                    {
                        return Response<AuthResponseDTO>.Fail("Failed to send email.");
                    }
                    _httpContextAccessor.HttpContext.Response.StatusCode = 401;
                    return Response<AuthResponseDTO>.Fail("The account is locked out");
                }
                _httpContextAccessor.HttpContext.Response.StatusCode = 401;
                return Response<AuthResponseDTO>.Fail("Invalid Email or Password!");
            }

            if (await _userManager.GetTwoFactorEnabledAsync(user))
                return await GenerateOTPFor2StepVerification(user);

            var token = await GenerateJSONWebToken(user);

            await _userManager.ResetAccessFailedCountAsync(user);

            return Response<AuthResponseDTO>.Success(data: new AuthResponseDTO { IsAuthSuccessful = true, Token = token });
        }

        private async Task<Response<AuthResponseDTO>> GenerateOTPFor2StepVerification(User user)
        {
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
            if (!providers.Contains("Email"))
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = 401;
                return Response<AuthResponseDTO>.Fail("Invalid 2-Step Verification Provider.");
            }

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            var content = $@"Your OTP is: {token}";
            try
            {
                await _emailService.SendEmailAsync(new List<string> { user.Email }, "2FA Verification Code", content);
            }
            catch (Exception)
            {
                return Response<AuthResponseDTO>.Fail("Failed to send email.");
            }

            return Response<AuthResponseDTO>.Success(data: new AuthResponseDTO { Is2StepVerificationRequired = true, Provider = "Email" }
            , "A verification code hase been sent to your email. Check your email and enter the code to complete the login process.");
        }

        private async Task<IdentityResult> UpdatePassword(User user, string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            return result;
        }

        private async Task<string> GenerateJSONWebToken(User user)
        {
            var userRoleNames = await _userManager.GetRolesAsync(user);
            var token = CreateAccessToken(CreateJwtClaims(user, userRoleNames, _expiryHours));

            return token;
        }

        private static List<Claim> CreateJwtClaims(User user, IList<string> userRoles, int expiryHours)
        {
            var claims = new List<Claim>();
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, "B2BPriceAdmin"),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim("TenantId", user.TenantId.ToString())
            });
            foreach (var item in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }

            return claims;
        }

        private string CreateAccessToken(IEnumerable<Claim> claims)
        {
            var jwtSecret = _configuration["JwtBearer:SecurityKey"];
            var jwtIssuer = _configuration["JwtBearer:Issuer"];
            var jwtAudance = _configuration["JwtBearer:Audience"];
            var now = DateTime.Now;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudance,
                claims: claims,
                notBefore: now,
                //expires: DateTime.UtcNow.AddDays(60),
                expires: DateTime.Now.AddHours(_expiryHours),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
