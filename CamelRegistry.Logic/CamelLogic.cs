using AutoMapper;
using CamelRegistry.Entities;
using CamelRegistry.Repository;
using FluentValidation;
using System.Linq.Expressions;

namespace CamelRegistry.Logic
{
    public class CamelLogic : ICamelLogic
    {
        private readonly ICamelRepository _repo;
        private readonly IMapper _mapper;
        private readonly IValidator<CamelCreateModel> _createValidator;
        private readonly IValidator<CamelUpdateModel> _updateValidator;

        public CamelLogic(ICamelRepository repo, IMapper mapper, IValidator<CamelCreateModel> createValidator, IValidator<CamelUpdateModel> updateValidator)
        {
            _repo = repo;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<Camel> AddAsync(CamelCreateModel createModel)
        {
            var validationResult = await _createValidator.ValidateAsync(createModel);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Camel>(createModel);
            await _repo.AddAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Camel with id {id} not found");
            }
            await _repo.DeleteAsync(entity);
        }


        public async Task<List<Camel>> GetAllAsync(params Expression<Func<Camel, object>>[] includes)
            => await _repo.GetAllAsync(includes);

        public async Task<Camel> GetByIdAsync(Guid id)
        {
            var entity= await _repo.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Camel with id {id} not found");
            }
            return entity;
        }

        public async Task<Camel> UpdateAsync(Guid id, CamelUpdateModel updateModel)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateModel);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingEntity = await GetByIdAsync(id);

            _mapper.Map(updateModel, existingEntity);
            await _repo.UpdateAsync(existingEntity!);
            return existingEntity!;

        }
    }
}
