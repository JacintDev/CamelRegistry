using CamelRegistry.Entities;
using CamelRegistry.Logic.Validators;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelRegistry.Tests
{
    public class CamelValidationTests
    {
        private readonly CamelCreateModelValidator _createValidator;
        private readonly CamelUpdateModelValidator _updateValidator;
        public CamelValidationTests()
        {
            _createValidator = new CamelCreateModelValidator();
            _updateValidator = new CamelUpdateModelValidator();
        }

        [Trait("Category", "CreateValidation")]
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("This name is way too long to be valid because it exceeds the maximum length of fifty characters")]
        public void Create_NameIsEmpty_ShouldHaveValidationError(string badname)
        {
            //ARRANGE
            var model = new CamelCreateModel
            {
                Name = "",
                Color = "Brown",
                HumpCount = 2
            };

            //ACT
            var result = _createValidator.TestValidate(model);

            //ASSERT
            Assert.False(result.IsValid);
            result.ShouldHaveValidationErrorFor(x => x.Name);

        }
        [Trait("Category", "CreateValidation")]
        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        public void Create_HumpCountOutOfRange_ShouldHaveValidationError(int badHumpCount)
        {
            //ARRANGE
            var model = new CamelCreateModel
            {
                Name = "Valid Name",
                Color = "Brown",
                HumpCount = badHumpCount
            };

            //ACT
            var result = _createValidator.TestValidate(model);

            //ASSERT
            Assert.False(result.IsValid);
            result.ShouldHaveValidationErrorFor(x => x.HumpCount);
        }

        [Trait("Category", "CreateValidation")]
        [Fact]
        public void Create_LastFedInFuture_ShouldHaveValidationError()
        {
            //ARRANGE
            var model = new CamelCreateModel
            {
                Name = "Valid Name",
                Color = "Brown",
                HumpCount = 2,
                LastFed = DateTime.Now.AddDays(1) // Future date
            };

            //ACT
            var result = _createValidator.TestValidate(model);

            //ASSERT
            Assert.False(result.IsValid);
            result.ShouldHaveValidationErrorFor(x => x.LastFed);
        }

        [Trait("Category", "CreateValidation")]
        [Fact]
        public void Create_ValidModel_ShouldNotHaveValidationErrors()
        {
            //ARRANGE
            var model = new CamelCreateModel
            {
                Name = "Valid Name",
                Color = "Brown",
                HumpCount = 2,
                LastFed = DateTime.Now.AddDays(-1) // Past date
            };

            //ACT
            var result = _createValidator.TestValidate(model);

            //ASSERT
            Assert.True(result.IsValid);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
            result.ShouldNotHaveValidationErrorFor(x => x.HumpCount);
            result.ShouldNotHaveValidationErrorFor(x => x.LastFed);

        }

        [Trait("Category", "UpdateValidation")]
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("This name is way too long to be valid because it exceeds the maximum length of fifty characters")]
        public void Update_NameIsEmpty_ShouldHaveValidationError(string badname)
        {
            //ARRANGE
            var model = new CamelUpdateModel
            {
                Name = "",
                Color = "Brown",
                HumpCount = 2
            };

            //ACT
            var result = _updateValidator.TestValidate(model);

            //ASSERT
            Assert.False(result.IsValid);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Trait("Category", "UpdateValidation")]
        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        public void Update_HumpCountOutOfRange_ShouldHaveValidationError(int badHumpCount)
        {
            //ARRANGE
            var model = new CamelUpdateModel
            {
                Name = "Valid Name",
                Color = "Brown",
                HumpCount = badHumpCount
            };

            //ACT
            var result = _updateValidator.TestValidate(model);

            //ASSERT
            Assert.False(result.IsValid);
            result.ShouldHaveValidationErrorFor(x => x.HumpCount);
        }

        [Trait("Category", "UpdateValidation")]
        [Fact]
        public void Update_LastFedInFuture_ShouldHaveValidationError()
        {
            //ARRANGE
            var model = new CamelUpdateModel
            {
                Name = "Valid Name",
                Color = "Brown",
                HumpCount = 2,
                LastFed = DateTime.Now.AddDays(1) // Future date
            };

            //ACT
            var result = _updateValidator.TestValidate(model);

            //ASSERT
            Assert.False(result.IsValid);
            result.ShouldHaveValidationErrorFor(x => x.LastFed);
        }

        [Trait("Category", "UpdateValidation")]
        [Fact]
        public void Update_ValidModel_ShouldNotHaveValidationErrors()
        {
            //ARRANGE
            var model = new CamelUpdateModel
            {
                Name = "Valid Name",
                Color = "Brown",
                HumpCount = 2,
                LastFed = DateTime.Now.AddDays(-1) // Past date
            };

            //ACT
            var result = _updateValidator.TestValidate(model);

            //ASSERT
            Assert.True(result.IsValid);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
            result.ShouldNotHaveValidationErrorFor(x => x.HumpCount);
            result.ShouldNotHaveValidationErrorFor(x => x.LastFed);
        }
    }
}