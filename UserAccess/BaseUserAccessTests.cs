using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Repository;
using DnDProject.Backend.Repository.Implementations;
using DnDProject.Backend.UserAccess.Implementations;
using DnDProject.Backend.UserAccess.Interfaces;
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

namespace DnDProject.UnitTests.UserAccess
{
    [TestFixture]
    public class BaseUserAccessTests
    {
        [Test]
        public void BaseUserAccess_AddCharacter_ValidCall()
        {
            //Arrange
            List<Character> charList = new List<Character>();
            charList.Add(CreateTestData.getSampleCharacter());

            var mockSet = new Mock<DbSet<Character>>()
                .SetupData(charList, o =>
                {
                    return charList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<CharacterContext>().Setup(x => x.Characters).Returns(mockSet.Object);
                IDataRepository dataRepository = mockContext.Create<MySqlDataRepository>();

                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(dataRepository);

                var expected = CreateTestData.getSampleCharacter();
                Guid id = Guid.Parse("757f4657-ff89-4a9d-a31b-aeda05a38615");
                expected.Character_id = id;

                //Act
                toTest.AddCharacter(expected);
                var actual = toTest.GetCharacter(id);

                expected.Should().BeOfType<Character>();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Character>();
                actual.Should().NotBeNull();

                actual.Should().BeEquivalentTo(expected);

            }

        }
        [Test]
        public void BaseUserAccess_GetCharacter_ValidCall()
        {
            //Arrange
            List<Character> charList = new List<Character>();
            charList.Add(CreateTestData.getSampleCharacter());

            var mockSet = new Mock<DbSet<Character>>()
                .SetupData(charList, o =>
                {
                    return charList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<CharacterContext>().Setup(x => x.Characters).Returns(mockSet.Object);
                IDataRepository dataRepository = mockContext.Create<MySqlDataRepository>();

                IBaseUserAccess toTest = new BaseUserAccess(dataRepository);

                //Act
                var expected = CreateTestData.getSampleCharacter();
                Guid id = expected.Character_id;
                var actual = toTest.GetCharacter(id);

                expected.Should().BeOfType<Character>();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Character>();
                actual.Should().NotBeNull();

                actual.Should().BeEquivalentTo(expected);

            }
        }
        [Test]
        public void BaseUserAccess_UpdateCharacter_ValidCall()
        {
            //Arrange
            int saveChanges = 0;
            List<Character> charList = CreateTestData.GetListOfCharacters();

            var mockSet = new Mock<DbSet<Character>>()
                .SetupData(charList, o =>
                {
                    return charList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<CharacterContext>().Setup(x => x.Characters).Returns(mockSet.Object);
                mockContext.Mock<CharacterContext>().Setup(x => x.SaveChanges()).Callback(() => saveChanges = saveChanges + 1);
                IDataRepository dataRepository = mockContext.Create<MySqlDataRepository>();


                var expected = CreateTestData.getSampleCharacter();
                expected.Name = "Grog";
                expected.Exp = 100;
                var id = expected.Character_id;
                //Act
                IBaseUserAccess toTest = new BaseUserAccess(dataRepository);
                toTest.UpdateCharacter(expected);
                toTest.SaveChanges();

                var actual = toTest.GetCharacter(id);

                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Character>();
                expected.Should().BeOfType<Character>();
                Assert.AreEqual(1, saveChanges);

            }

        }
        [Test]
        public void BaseUserAccess_DeleteCharacter_ValidCall()
        {
            //Arrange
            List<Character> charList = CreateTestData.GetListOfCharacters();
            var mockSet = new Mock<DbSet<Character>>()
                .SetupData(charList, o =>
                {
                    return charList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Characters).Returns(mockSet.Object);
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Characters.Remove(It.IsAny<Character>()))
                        .Callback<Character>((entity) => charList.Remove(entity));

                IDataRepository dataRepository = mockContext.Create<MySqlDataRepository>();

                var toDelete = CreateTestData.getSampleCharacter();
                Character expected = null;
                var NotExpected = CreateTestData.getSampleCharacter();

                var id = CreateTestData.getSampleCharacter().Character_id;
                IDataRepository repository = mockContext.Create<MySqlDataRepository>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(repository);

                //Act
                toTest.DeleteCharacter(id);
                var actual = charList.Find(character => character.Character_id == id);

                //Assert
                actual.Should().BeNull();
                actual.Should().NotBeEquivalentTo(NotExpected);
                actual.Should().BeEquivalentTo(expected);


            }

        }
        [Test]
        public void BaseUserAccess_AddProficiencyRecord_ValidCall()
        {
            //Arrange
            List<IsProficient> ProficienciesList = CreateTestData.GetListOfIsProficient();
            var mockSet = new Mock<DbSet<IsProficient>>()
                .SetupData(ProficienciesList, o =>
                {
                    return ProficienciesList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Proficiencies).Returns(mockSet.Object);
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Proficiencies.Add(It.IsAny<IsProficient>()))
                        .Callback<IsProficient>((entity) => ProficienciesList.Add(entity));

                IDataRepository dataRepository = mockContext.Create<MySqlDataRepository>();

                var toAdd = CreateTestData.GetSampleIsProficient();
                var id = Guid.Parse("ce798c73-638b-4c70-adea-9092615fbe01");
                toAdd.Character_id = id;

                IDataRepository repository = mockContext.Create<MySqlDataRepository>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(repository);

                //Act
                toTest.AddProficiencyRecord(toAdd);
                var expected = toAdd;
                var actual = ProficienciesList.Find(proficiencies => proficiencies.Character_id == id);

                //Assert

                expected.Should().BeOfType<IsProficient>();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<IsProficient>();
                actual.Should().NotBeNull();
                actual.Should().BeEquivalentTo(expected);

            }
        }
        [Test]
        public void BaseUserAccess_GetProficiencyRecord_ValidCall()
        {

            //Arrange
            List<IsProficient> ProficienciesList = CreateTestData.GetListOfIsProficient();
            var mockSet = new Mock<DbSet<IsProficient>>()
                .SetupData(ProficienciesList, o =>
                {
                    return ProficienciesList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Proficiencies).Returns(mockSet.Object);

                var expected = CreateTestData.GetSampleIsProficient();
                var id = expected.Character_id;


                IDataRepository repository = mockContext.Create<MySqlDataRepository>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(repository);

                //Act
                var actual = toTest.GetProficiencyRecord(id);

                //Assert
                expected.Should().BeOfType<IsProficient>();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<IsProficient>();
                actual.Should().NotBeNull();
                actual.Should().BeEquivalentTo(expected);

            }
        }
        [Test]
        public void BaseUserAccess_UpdateProficiencyRecord_ValidCall()
        {
            //Arrange
            int saveChanges = 0;
            List<IsProficient> ProficienciesList = CreateTestData.GetListOfIsProficient();
            var mockSet = new Mock<DbSet<IsProficient>>()
                .SetupData(ProficienciesList, o =>
                {
                    return ProficienciesList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Proficiencies).Returns(mockSet.Object);
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.SaveChanges()).Callback(() => saveChanges = saveChanges + 1);

                var expected = CreateTestData.GetSampleIsProficient();
                expected.StrengthSave = false;
                expected.DexteritySave = false;
                expected.CharismaSave = true;

                IDataRepository repository = mockContext.Create<MySqlDataRepository>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(repository);

                //Act
                toTest.UpdateProficiencyRecord(expected);
                toTest.SaveChanges();
                var actual = toTest.GetProficiencyRecord(expected.Character_id);

                //Assert
                expected.Should().BeOfType<IsProficient>();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<IsProficient>();
                actual.Should().NotBeNull();
                Assert.AreEqual(1, saveChanges);

            
            }
        }
    }
}
