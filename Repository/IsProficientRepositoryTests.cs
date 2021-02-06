using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Repository.Implementations;
using DnDProject.Backend.Repository.Interfaces;
using DnDProject.Entities.Character.DataModels;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDProject.UnitTests.Repository
{
    [TestFixture]
    public class IsProficientRepositoryTests
    {
        [Test]
        public void IsProficientRepository_AddIsProficient_ValidCall()
        {
            //Arrange
            List<IsProficient> proficiencyList = CreateTestData.GetListOfIsProficient();
            var mockSet = new Mock<DbSet<IsProficient>>()
                .SetupData(proficiencyList, o =>
                {
                    return proficiencyList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });


            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<IsProficient>()).Returns(mockSet.Object);
                IIsProficientRepository toTest = mockContext.Create<IsProficientRepository>();

                //Act
                var GrogProficiencies = CreateTestData.GetSampleIsProficient();
                GrogProficiencies.Character_id = Guid.Parse("c95a4b3e-340c-4ac4-86e0-784bb8c1b87c");

                toTest.Add(GrogProficiencies);

                var actual = toTest.Get(GrogProficiencies.Character_id);

                //Assert
                actual.Should().NotBeNull();
                actual.Should().BeOfType<IsProficient>();
                actual.Should().BeEquivalentTo(GrogProficiencies);

            }
        }

        [Test]
        public void IsProficientRepository_GetProficiencyRecord_ValidCall()
        {
            //Arrange
            List<IsProficient> proficiencyList = CreateTestData.GetListOfIsProficient();
            var mockSet = new Mock<DbSet<IsProficient>>()
                .SetupData(proficiencyList, o =>
                {
                    return proficiencyList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<IsProficient>()).Returns(mockSet.Object);
                IIsProficientRepository toTest = mockContext.Create<IsProficientRepository>();

                var id = Guid.Parse("11111111-2222-3333-4444-555555555555");
                var expected = CreateTestData.GetSampleIsProficient();

                //Act
                var actual = toTest.Get(id);

                //Assert
                actual.Should().NotBeNull();
                actual.Should().BeOfType<IsProficient>();
                actual.Should().BeEquivalentTo(expected);

            }
        }

        [Test]
        public void IsProficientRepository_UpdateProficiencyRecord_ValidCall()
        {
            //Arrange
            int saveChanges = 0;
            List<IsProficient> proficienciesList = CreateTestData.GetListOfIsProficient();
            var mockSet = new Mock<DbSet<IsProficient>>()
                .SetupData(proficienciesList, o =>
                {
                    return proficienciesList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleIsProficient();
                expected.StrengthSave = false;
                expected.DexteritySave = false;
                expected.ConstitutionSave = false;

                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<IsProficient>()).Returns(mockSet.Object);

                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.SaveChanges()).Callback(() => saveChanges = saveChanges + 1);

                //Act
                IIsProficientRepository toTest = mockContext.Create<IsProficientRepository>();
                toTest.Update(expected);

                //Assert
                expected.Should().NotBeNull();
                expected.Should().BeOfType<IsProficient>();
                //Verifies that the object I wished to update was attached to the DbSet.
                //Basically, that means EF confirms that the entity with expected's Primary key will be updated the next time Save is called.
                mockSet.Verify(x => x.Attach(expected), Times.Once());
            }

        }
    }
}
