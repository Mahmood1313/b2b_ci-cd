namespace B2BPriceAdmin.DTO
{
    public class AuthResponseDTO
    {
        public bool IsAuthSuccessful { get; set; } = false;
        public string Token { get; set; }
        public bool Is2StepVerificationRequired { get; set; } = false;
        public string Provider { get; set; }
    }
}
