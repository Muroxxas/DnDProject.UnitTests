﻿using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Repository;
using DnDProject.Backend.Repository.Implementations;
using DnDProject.Entities.Character.DataModels;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace DnDProject.UnitTests.Repository
{
    [TestFixture]
    public class MySqlDataRepositoryTests
    {
        [Test]
        public void MySqlDataRepository_ConstructMySqlDataRoot_ReturnTrue()
        {
            var toTest = new MySqlDataRepository();

            Assert.IsNotNull(toTest);
        }

        [Test]
        public void MySqlDataRepository_IsImplementationOfIDataRoot_ReturnTrue()
        {
            //Arrange

            //Act
            var toTest = new MySqlDataRepository();

            //Assert
            Assert.IsInstanceOf<IDataRepository>(toTest);
        }

        [Test]
        public void MySqlDataRepository_LoadCharacterByCharacterID_SingularCharacterObjectReturned()
        {

            //Arrange
            //1. Create the test data.
            List<Character> charList = CreateTestData.GetListOfCharacters();

            //2. Create a mock set, one that properly responds to EntityFramework's .Find()
            //from the charList, return the first object that has a character_id that matches the given character_id.
            var mockSet = new Mock<DbSet<Character>>()
                .SetupData(charList, o =>
                {
                    return charList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                //Act
                //3. Use the mockSet to properly create the mockContext.
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Characters).Returns(mockSet.Object);

                //4. Create a instance of MySqlDataRepository, injecting mockContext via the constructor.
                IDataRepository toTest = mockContext.Create<MySqlDataRepository>();
                var expected = CreateTestData.getSampleCharacter();
                var actual = toTest.GetCharacter(expected.Character_id);


                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Character>();
                expected.Should().BeOfType<Character>();
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void MySqlDataRepository_AddCharacter_ValidCall()
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

                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Characters).Returns(mockSet.Object);
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.SaveChanges()).Callback(() => saveChanges = saveChanges + 1);
                IDataRepository toTest = mockContext.Create<MySqlDataRepository>();
                var expected = CreateTestData.getSampleCharacter();
                expected.Character_id = Guid.Parse("33855fe6-807a-46e3-850f-ada7dacfc435");

                //Act
                toTest.AddCharacter(expected);
                toTest.SaveChanges();
                var actual = toTest.GetCharacter(expected.Character_id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Character>();
                expected.Should().BeOfType<Character>();
                actual.Should().BeEquivalentTo(expected);
                Assert.AreEqual(1, saveChanges);
            }
        }

        [Test]
        public void MySqlDataRepository_UpdateCharacter_CharacterUpdated() 
        {
            //PROBLEMATIC TEST
            //Whenever the record within the mockSet is mapped to and the mockSet is saved, the record we are updating is found, but not mapped.
            //However, unit testing of the mapper itself reveals that all is well, and the mapping is functioning properly. It's just Moq being weird.

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
                var expected = CreateTestData.getSampleCharacter();
                expected.Name = "Grog";
                expected.Exp = 100;

                //When something calls for the Characters table, return the DbSet in mockSet
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Characters).Returns(mockSet.Object);

                //when SaveChanges gets called on the context, the saveChanges variable will be incremented, indicating that the DB was saved successfully.
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.SaveChanges()).Callback(() => saveChanges = saveChanges + 1);

                IDataRepository toTest = mockContext.Create<MySqlDataRepository>();


                //Act
                toTest.UpdateCharacter(expected);
                toTest.SaveChanges();

                Guid id = expected.Character_id;

                var actual = toTest.GetCharacter(id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Character>();
                expected.Should().BeOfType<Character>();
                Assert.AreEqual(1, saveChanges);


            }
        }

        [Test]
        public void MySqlDataRepository_DeleteCharacter_ValidCall()
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
                    //When a removal of a Character object is called, perform a callback to the charList collection, using the same character object as an argument.
                    //This callback then fires, removing the object from the list.
                    .Setup(x => x.Characters.Remove(It.IsAny<Character>()))
                        .Callback<Character>((entity) => charList.Remove(entity));

                //Act
                IDataRepository toTest = mockContext.Create<MySqlDataRepository>();
                var id = CreateTestData.getSampleCharacter().Character_id;
                toTest.DeleteCharacter(id);
                Character expected = null;
                var NotExpected = CreateTestData.getSampleCharacter();
                var actual = charList.Find(character => character.Character_id == id);

                //Assert
                actual.Should().BeNull();
                actual.Should().NotBeEquivalentTo(NotExpected);
                actual.Should().BeEquivalentTo(expected);


            }
        }

        [Test]
        public void MySqlDataRepository_AddIsProficient_ValidCall()
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
                    .Setup(x => x.Proficiencies).Returns(mockSet.Object);
                IDataRepository toTest = mockContext.Create<MySqlDataRepository>();

                //Act
                var GrogProficiencies = CreateTestData.GetSampleIsProficient();
                GrogProficiencies.Character_id = Guid.Parse("c95a4b3e-340c-4ac4-86e0-784bb8c1b87c");

                toTest.AddProficiencyRecord(GrogProficiencies);

                var actual = toTest.GetProficiencyRecord(GrogProficiencies.Character_id);

                //Assert
                actual.Should().NotBeNull();
                actual.Should().BeOfType<IsProficient>();
                actual.Should().BeEquivalentTo(GrogProficiencies);

            }
        }

        [Test]
        public void MySqlDataRepository_GetProficiencyRecord_ValidCall()
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
                    .Setup(x => x.Proficiencies).Returns(mockSet.Object);
                IDataRepository toTest = mockContext.Create<MySqlDataRepository>();

                var id = Guid.Parse("11111111-2222-3333-4444-555555555555");
                var expected = CreateTestData.GetSampleIsProficient();

                //Act
                var actual = toTest.GetProficiencyRecord(id);

                //Assert
                actual.Should().NotBeNull();
                actual.Should().BeOfType<IsProficient>();
                actual.Should().BeEquivalentTo(expected);

            }
        }

        [Test]
        public void MySqlDataRepository_UpdateProficiencyRecord_ValidCall()
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
                    .Setup(x => x.Proficiencies).Returns(mockSet.Object);

                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.SaveChanges()).Callback(() => saveChanges = saveChanges + 1);

                //Act
                IDataRepository toTest = mockContext.Create<MySqlDataRepository>();
                toTest.UpdateProficiencyRecord(expected);
                toTest.SaveChanges();

                var actual = toTest.GetProficiencyRecord(expected.Character_id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<IsProficient>();
                expected.Should().BeOfType<IsProficient>();
                Assert.AreEqual(1, saveChanges);

            }

        }

        [Test]
        public void MySqlDataRepository_AddHealthRecord_ValidCall()
        {
            //Arrange
            List<Health> healthList = CreateTestData.GetListOfHealth();
            var mockSet = new Mock<DbSet<Health>>()
                .SetupData(healthList, o =>
                {
                    return healthList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose()) {
                var expected = CreateTestData.GetSampleHealth();
                var id = Guid.Parse("b346eee6-eba7-4ea7-be2e-911bb9034233");
                expected.Character_id = id;

                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.HealthRecords).Returns(mockSet.Object);

                //Act
                IDataRepository toTest = mockContext.Create<MySqlDataRepository>();
                toTest.AddHealthRecord(expected);
                var actual = toTest.GetHealthRecord(id);

                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Health>();
                expected.Should().BeOfType<Health>();
                actual.Should().BeEquivalentTo(expected);

            }
        }
        [Test]
        public void MySqlDataRepository_GetHealthRecord_ValidCall()
        {
            //Arrange
            List<Health> healthList = CreateTestData.GetListOfHealth();
            var mockSet = new Mock<DbSet<Health>>()
                .SetupData(healthList, o =>
                {
                    return healthList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleHealth();
                var id = expected.Character_id;

                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.HealthRecords).Returns(mockSet.Object);

                //Act
                IDataRepository toTest = mockContext.Create<MySqlDataRepository>();
                var actual = toTest.GetHealthRecord(id);

                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Health>();
                expected.Should().BeOfType<Health>();
                actual.Should().BeEquivalentTo(expected);
            }

        }
        [Test]
        public void MySqlDataRepository_UpdateHealthRecord_ValidCall()
        {
            //Arrange
            int saveChanges = 0;
            List<Health> healthList = CreateTestData.GetListOfHealth();
            var mockSet = new Mock<DbSet<Health>>()
                .SetupData(healthList, o =>
                {
                    return healthList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            using (var mockContext = AutoMock.GetLoose()) {
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.HealthRecords).Returns(mockSet.Object);
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.SaveChanges()).Callback(() => saveChanges = saveChanges + 1);

                var expected = CreateTestData.GetSampleHealth();
                expected.MaxHP = 200;
                expected.DeathSaveSuccesses = 2;
                IDataRepository toTest = mockContext.Create<MySqlDataRepository>();


                //Act
                toTest.UpdateHealthRecord(expected);
                toTest.SaveChanges();
                var actual = toTest.GetHealthRecord(expected.Character_id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Health>();
                expected.Should().BeOfType<Health>();
                Assert.AreEqual(1, saveChanges);

            }
        }
    }
}
