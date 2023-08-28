using AuthServer.Core.Entities.Common;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitofWork;
using AuthServer.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class ServiceGeneric<TEntity, TDto> : IServiceGeneric<TEntity, TDto> where TEntity : BaseEntity<Guid>
                                                                 where TDto : class
    {
        private readonly IUnitofWork _unitofWork;
        private IRepository<TEntity> _repository;

        public ServiceGeneric(IUnitofWork unitofWork, IRepository<TEntity> repository)
        {
            _unitofWork = unitofWork;
            _repository = repository;
        }

        public async Task<Response<TDto>> AddAsync(TDto dto)
        {
            var entity = ObjectMapper.Mapper.Map<TEntity>(dto);

            if (entity is null)
                return Response<TDto>.Error($"{typeof(TEntity).Name} nesnesi bulunamadı!", (int)HttpStatusCode.NotFound);

            await _repository.AddAsync(entity);
            var result = await _unitofWork.SaveChangesAsync();

            return result > 0 ? Response<TDto>.Success((int)HttpStatusCode.NoContent)
                              : Response<TDto>.Error($"{typeof(TDto).Name} kaydetme işlemi başarısız!", (int)HttpStatusCode.BadRequest);
        }

        public async Task<Response<List<TDto>>> GetAllAsync()
        {
            var entities = await _repository.GetAll()
                                            .ToListAsync();

            if (!entities.Any())
                return Response<List<TDto>>.Error($"{typeof(TEntity).Name}'ler bulunamadı!", (int)HttpStatusCode.NotFound);

            var allDtos = ObjectMapper.Mapper.Map<List<TDto>>(entities);

            return allDtos.Any() ? Response<List<TDto>>.Success(allDtos, (int)HttpStatusCode.OK)
                                 : Response<List<TDto>>.Error($"{typeof(TDto).Name}'lar bulunamadı!", (int)HttpStatusCode.NotFound);
        }

        public async Task<Response<TDto>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = await _repository.GetAsync(predicate);

            if (entity is null)
                return Response<TDto>.Error($"{typeof(TEntity).Name} nesnesi bulunamadı!", (int)HttpStatusCode.NotFound);

            var dto = ObjectMapper.Mapper.Map<TDto>(entity);

            return dto is not null ? Response<TDto>.Success(dto, (int)HttpStatusCode.OK)
                                   : Response<TDto>.Error($"{typeof(TDto).Name} nesnesi bulunamadı!", (int)HttpStatusCode.NotFound);
        }

        public async Task<Response<TDto>> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity is null)
                return Response<TDto>.Error($"{typeof(TEntity).Name} nesnesi bulunamadı!", (int)HttpStatusCode.NotFound);

            var dto = ObjectMapper.Mapper.Map<TDto>(entity);

            return dto is not null ? Response<TDto>.Success(dto, (int)HttpStatusCode.OK)
                                   : Response<TDto>.Error($"{typeof(TDto).Name} nesnesi bulunamadı!", (int)HttpStatusCode.NotFound);
        }

        public async Task<Response<NoDataDto>> Remove(Guid id)
        {
            var isExistEntity = await _repository.GetByIdAsync(id);

            if(isExistEntity is null)
                return Response<NoDataDto>.Error($"{typeof(TEntity).Name} nesnesi bulunamadı!", (int)HttpStatusCode.NotFound);

            var entity = ObjectMapper.Mapper.Map<TEntity>(isExistEntity);

            _repository.Remove(entity);
            var result = await _unitofWork.SaveChangesAsync();

            return result > 0 ? Response<NoDataDto>.Success((int)HttpStatusCode.NoContent)
                              : Response<NoDataDto>.Error($"{typeof(TDto).Name} nesnesi silinemedi!", (int)HttpStatusCode.NotFound);
        }

        public async Task<Response<NoDataDto>> Update(Guid id, TDto dto)
        {
            var isExistEntity = await _repository.GetByIdAsync(id);

            if (isExistEntity is null)
                return Response<NoDataDto>.Error($"{typeof(TEntity).Name} nesnesi bulunamadı!", (int)HttpStatusCode.NotFound);

            var entity = ObjectMapper.Mapper.Map<TEntity>(dto);

            _repository.Update(entity);
            var result = await _unitofWork.SaveChangesAsync();

            return result > 0 ? Response<NoDataDto>.Success((int)HttpStatusCode.NoContent)
                              : Response<NoDataDto>.Error($"{typeof(TDto).Name} nesnesi güncellenemedi!", (int)HttpStatusCode.NotFound);
        }
    }
}
