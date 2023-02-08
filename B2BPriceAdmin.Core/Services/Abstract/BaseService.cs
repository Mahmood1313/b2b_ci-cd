using AutoMapper;
using B2BPriceAdmin.Common.Extensions;
using B2BPriceAdmin.Core.Common.PagedResponse;
using B2BPriceAdmin.Core.Helper.Pagination;
using B2BPriceAdmin.Core.Interfaces;
using B2BPriceAdmin.Database;
using B2BPriceAdmin.Database.Entities;
using B2BPriceAdmin.DTO;
using Microsoft.EntityFrameworkCore;

namespace B2BPriceAdmin.Core.Services.Abstract
{
    public abstract class BaseService<E, CDTO, UDTO, GADTO, GIDTO> : IBaseService<CDTO, UDTO, GADTO, GIDTO>
        where E : BaseEntity where CDTO : BaseDTO where UDTO : BaseDTO where GADTO : BaseDTO where GIDTO : BaseDTO
    {
        protected readonly B2BPriceDbContext _db;
        protected readonly IMapper _mapper;

        public BaseService(B2BPriceDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds record to the entity.
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns>Returns the newly created record in the entity.</returns>
        public virtual async Task<Response<CDTO>> CreateAsync(CDTO Dto)
        {
            try
            {
                var entity = _mapper.Map<E>(Dto);
                //_mapper.Map(_userInfoDetailDTO, entity);
                await _db.Set<E>().AddAsync(entity);
                await _db.SaveChangesAsync();
                return Response<CDTO>.Success(_mapper.Map<CDTO>(entity), "Saved Successfully");
            }
            catch (Exception ex)
            {
                if (ex.ErrorExceptionMessage().Contains("IX_Unique_"))
                {
                    return Response<CDTO>.Fail("Already exist");
                }
                throw;
            }
        }

        /// <summary>
        /// Updates the Record
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Dto"></param>
        /// <returns>Returns the updated record.</returns>
        public virtual async Task<Response<UDTO>> UpdateAsync(int Id, CDTO Dto)
        {
            if (Dto == null)
                return null;
            try
            {
                var entity = await _db.Set<E>().FirstOrDefaultAsync(x => x.Id == Id);
                if (entity != null)
                {
                    _mapper.Map(Dto, entity);
                    //_mapper.Map(_userInfoDetailDTO, entity);
                    _db.Set<E>().Update(entity);
                    await _db.SaveChangesAsync();
                    return Response<UDTO>.Success(_mapper.Map<UDTO>(entity), "Updated Successfully");
                }
                return Response<UDTO>.Fail("Can not find entity to update");
            }
            catch (Exception ex)
            {
                if (ex.ErrorExceptionMessage().Contains("IX_Unique_"))
                {
                    return Response<UDTO>.Fail("Already exist");
                }
                throw;
            }
        }

        /// <summary>
        /// UpdateRange the list of records. 
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns>Returns range of records.</returns>
        public virtual async Task<Response<List<CDTO>>> CreateUpdateAsync(List<CDTO> Dto)
        {
            try
            {
                //Dto.ForEach(x =>
                //{
                //    _mapper.Map(_userInfoDetailDTO, x);
                //});
                var entity = _mapper.Map<List<E>>(Dto);
                // _db.Entry(entity.Where(x=>x.Id != null)).State = EntityState.Modified;

                //_mapper.Map(_userInfoDetailDTO, entity);
                _db.Set<E>().UpdateRange(entity);
                await _db.SaveChangesAsync();
                return Response<List<CDTO>>.Success(_mapper.Map<List<CDTO>>(entity), "Saved Successfully");
            }
            catch (Exception ex)
            {
                if (ex.ErrorExceptionMessage().Contains("IX_Unique_"))
                {
                    return Response<List<CDTO>>.Fail("Already exist");
                }
                throw;
            }
        }

        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns>Single Record</returns>
        public async Task<Response<GIDTO>> GetByIdAsync(BaseDTO Dto)
        {
            var entity = await GetSingleEntity().FirstOrDefaultAsync(x => x.Id == Dto.Id);
            if (entity != null)
            {
                return Response<GIDTO>.Success(_mapper.Map<GIDTO>(entity));
            }
            return Response<GIDTO>.Fail("Can not find entity");
        }

        /// <summary>
        /// Used in GetByIdAsync override it to create custom query
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<E> GetSingleEntity()
        {
            return _db.Set<E>();
        }

        /// <summary>
        /// GetAllAsync
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<PagedResponse<List<GADTO>>> GetAllAsync(PaginationFilterInputDTO filter)
        {
            var data = CreateQuery(filter);
            data = data.AsNoTracking();
            var totalRecords = data.Count();
            if ((filter.PageSize == null || filter.PageSize == 0))
            {
                filter.PageSize = totalRecords;
            }
            if (totalRecords == 0)
            {
                return PaginationHelper.CreatePagedReponse<GADTO>(new List<GADTO>(), filter, totalRecords);
            }
            data = data.DynamicSort(filter.SortColumn, filter.SortOrder);
            var pagedData = _mapper.Map<List<GADTO>>(await data.Skip((filter.PageNumber - 1) * filter.PageSize ?? 0)
                .Take(filter.PageSize ?? 0).ToListAsync());

            return PaginationHelper.CreatePagedReponse<GADTO>(pagedData, filter, totalRecords);
        }

        /// <summary>
        /// Used in GetAllAsync. overide it to create custom query.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        protected virtual IQueryable<E> CreateQuery(PaginationFilterInputDTO filter)
        {
            return _db.Set<E>();
        }

        /// <summary>
        /// Soft Delete a record. Override it to add custom queries.
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns>Soft deletes a record.</returns>
        public virtual async Task<Response<bool>> DeleteAsync(BaseDTO Dto)
        {
            var entity = await _db.Set<E>().FirstOrDefaultAsync(x => x.Id == Dto.Id);
            if (entity != null)
            {
                entity.Deleted = true;
                await _db.SaveChangesAsync();
                return Response<bool>.Success("Deleted Successfully"); ;
            }
            return Response<bool>.Fail("Could not find the record to delete.");
        }

    }
}
