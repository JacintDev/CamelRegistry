using AutoMapper;
using CamelRegistry.Entities;
using CamelRegistry.Logic;
using CamelRegistry.Logic.Validators;
using CamelRegistry.Repository;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Linq.Expressions;

namespace CamelRegistry.Tests
{
    public class CamelLogicTests
    {

        private readonly Mock<ICamelRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidator<CamelCreateModel>> _mockCreateValidator;
        private readonly Mock<IValidator<CamelUpdateModel>> _mockUpdateValidator;
        private readonly CamelLogic _camelLogic;

        public CamelLogicTests()
        {
            _mockRepository = new Mock<ICamelRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockCreateValidator = new Mock<IValidator<CamelCreateModel>>();
            _mockUpdateValidator = new Mock<IValidator<CamelUpdateModel>>();
            _camelLogic = new CamelLogic(_mockRepository.Object, _mockMapper.Object, _mockCreateValidator.Object, _mockUpdateValidator.Object);
        }


        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCamels()
        {
            //ARRANGE
            var dummyCamels = new List<Camel>
            {
                new Camel { Id = Guid.NewGuid(), Name = "Camel1", Color="Brown", HumpCount=2},
                new Camel { Id = Guid.NewGuid(), Name = "Camel2", LastFed=DateTime.Now, HumpCount=1 }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Camel, object>>[]>()))
                .ReturnsAsync(dummyCamels);

            //ACT
            var result = await _camelLogic.GetAllAsync();

            //ASSERT
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Camel1", result[0].Name);

            _mockRepository.Verify(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Camel, object>>[]>()), Times.Once);
        }


        [Fact]
        public async Task GetById_Existing_ReturnCamel()
        {

            //ARRANGE
            var camelId = Guid.NewGuid();
            var dummyCamel = new Camel { Id = camelId, Name = "Camel1", Color = "Brown", HumpCount = 2 };

            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(dummyCamel);

            //ACT
            var result = await _camelLogic.GetByIdAsync(camelId);

            //ASSERT

            Assert.NotNull(result);
            Assert.Equal("Camel1", result.Name);

            _mockRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }


        [Fact]
        public async Task GetById_NonExistingId_ThrowsException()
        {

            //ARRANGE

            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Camel?)null);


            //ACT && ASSERT

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _camelLogic.GetByIdAsync(Guid.NewGuid()));


        }

        [Fact]
        public async Task AddAsync_ValidModel_CallsRepository()
        {
            //ARRANGE
            var createModel = new CamelCreateModel { Name = "New Camel", Color = "White", HumpCount = 3 };
            var mappedModel = new Camel { Name = "New Camel", Color = "White", HumpCount = 3 };
            var returnedCamel = new Camel { Id = Guid.NewGuid(), Name = "New Camel", Color = "White", HumpCount = 3 };
            _mockCreateValidator.Setup(v => v.ValidateAsync(createModel, default))
                .ReturnsAsync(new ValidationResult());

            _mockMapper.Setup(m => m.Map<Camel>(createModel))
                .Returns(mappedModel);

            _mockRepository.Setup(repo => repo.AddAsync(mappedModel))
                .ReturnsAsync(returnedCamel);

            //ACT
            var result = await _camelLogic.AddAsync(createModel);

            Assert.NotNull(result);
            Assert.Equal("New Camel", result.Name);

            _mockRepository.Verify(repo => repo.AddAsync(mappedModel), Times.Once);
        }



        [Fact]
        public async Task AddAsync_InvalidModel_ShouldThrowValidationException()
        {
            //ARRANGE
            var invalidModel = new CamelCreateModel { Color = "White" };

            var failures = new List<ValidationFailure> {
                new ValidationFailure("Name", "Name is required")
            };

            _mockCreateValidator.Setup(v => v.ValidateAsync(invalidModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(failures));


            //ACT && ASSERT

            await Assert.ThrowsAsync<ValidationException>(() => _camelLogic.AddAsync(invalidModel));
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Camel>()), Times.Never);
        }

        [Fact]

        public async Task DeleteAsync_ExistingId_DeletesCamel()
        {
            //ARRANGE
            var camelId = Guid.NewGuid();
            var existingCamel = new Camel { Id = camelId, Name = "Camel1", Color = "Brown", HumpCount = 2 };

            _mockRepository.Setup(repo => repo.GetByIdAsync(camelId))
                .ReturnsAsync(existingCamel);

            _mockRepository.Setup(repo => repo.DeleteAsync(existingCamel))
                .Returns(Task.CompletedTask);

            //ACT
            await _camelLogic.DeleteAsync(camelId);

            //ASSERT
            _mockRepository.Verify(repo => repo.DeleteAsync(existingCamel), Times.Once);
        }

        [Fact]

        public async Task DeleteAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            //ARRANGE
            var camelId = Guid.NewGuid();


            _mockRepository.Setup(repo => repo.GetByIdAsync(camelId))
                .ReturnsAsync((Camel?)null);

            //ACT && ASSERT
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _camelLogic.DeleteAsync(camelId));
        }

        [Fact]
        public async Task UpdateAsync_ValidModel_UpdatesCamel()
        {
            //ARRANGE
            var camelId = Guid.NewGuid();
            var updateModel = new CamelUpdateModel { Name = "Upgraded Camel", Color = "Yellow", HumpCount = 1 };
            var existingCamel = new Camel { Id = camelId, Name = "Camel1", Color = "Brown", HumpCount = 2 };

            _mockUpdateValidator.Setup(v => v.ValidateAsync(updateModel, default))
                                .ReturnsAsync(new ValidationResult());

            _mockRepository.Setup(r => r.GetByIdAsync(camelId))
                           .ReturnsAsync(existingCamel);

            _mockMapper.Setup(m => m.Map(updateModel, existingCamel))
                       .Callback(() => existingCamel.Name = updateModel.Name);

            _mockRepository.Setup(r => r.UpdateAsync(existingCamel))
                           .ReturnsAsync(existingCamel);

            // ACT
            var result = await _camelLogic.UpdateAsync(camelId, updateModel);

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal("Upgraded Camel", result.Name);

            _mockRepository.Verify(r => r.UpdateAsync(existingCamel), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_InvalidModel_ShouldThrowValidationException()
        {
            //ARRANGE
            var camelId = Guid.NewGuid();
            var invalidModel = new CamelUpdateModel { Color = "Yellow" };

            var failures = new List<ValidationFailure> {
                new ValidationFailure("Name", "Name is required")
            };

            _mockUpdateValidator.Setup(v => v.ValidateAsync(invalidModel, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(new ValidationResult(failures));

            //ACT && ASSERT
            await Assert.ThrowsAsync<ValidationException>(() => _camelLogic.UpdateAsync(camelId, invalidModel));
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Camel>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingId_ShouldThrowKeyNotFoundException()
        {
            //ARRANGE
            var camelId = Guid.NewGuid();
            var updateModel = new CamelUpdateModel { Name = "Upgraded Camel", Color = "Yellow", HumpCount = 1 };

            _mockUpdateValidator.Setup(v => v.ValidateAsync(updateModel, default))
                                .ReturnsAsync(new ValidationResult());

            _mockRepository.Setup(r => r.GetByIdAsync(camelId))
                           .ReturnsAsync((Camel?)null);
            //ACT && ASSERT
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _camelLogic.UpdateAsync(camelId, updateModel));
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Camel>()), Times.Never);
        }
    }
}