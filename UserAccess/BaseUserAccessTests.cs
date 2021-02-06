using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Repository;
using DnDProject.Backend.Repository.Implementations;
using DnDProject.Backend.Unit_Of_Work.Implementations;
using DnDProject.Backend.Unit_Of_Work.Interfaces;
using DnDProject.Backend.UserAccess.Implementations;
using DnDProject.Backend.UserAccess.Interfaces;
using DnDProject.Entities.Character.DataModels;
using DnDProject.UnitTests.Unit_of_Work;
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
                mockContext.Mock<CharacterContext>().Setup(x => x.Set<Character>()).Returns(mockSet.Object);
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);

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
                mockContext.Mock<CharacterContext>().Setup(x => x.Set<Character>()).Returns(mockSet.Object);
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);

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
                mockContext.Mock<CharacterContext>().Setup(x => x.Set<Character>()).Returns(mockSet.Object);
                mockContext.Mock<CharacterContext>().Setup(x => x.SaveChanges()).Callback(() => saveChanges = saveChanges + 1);
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);

                var expected = CreateTestData.getSampleCharacter();
                expected.Name = "Grog";
                expected.Exp = 100;
                var id = expected.Character_id;
                //Act

                toTest.UpdateCharacter(expected);

                //Assert
                expected.Should().NotBeNull();
                expected.Should().BeOfType<Character>();
                //Verifies that the object I wished to update was attached to the DbSet.
                //Basically, that means EF confirms that the entity with expected's Primary key will be updated the next time Save is called.
                mockSet.Verify(x => x.Attach(expected), Times.Once());

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
                    .Setup(x => x.Set<Character>()).Returns(mockSet.Object);
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<Character>().Remove(It.IsAny<Character>()))
                        .Callback<Character>((entity) => charList.Remove(entity));



                var toDelete = CreateTestData.getSampleCharacter();
                Character expected = null;
                var NotExpected = CreateTestData.getSampleCharacter();

                var id = CreateTestData.getSampleCharacter().Character_id;
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);

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
                    .Setup(x => x.Set<IsProficient>()).Returns(mockSet.Object);
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<IsProficient>().Add(It.IsAny<IsProficient>()))
                        .Callback<IsProficient>((entity) => ProficienciesList.Add(entity));


                var toAdd = CreateTestData.GetSampleIsProficient();
                var id = Guid.Parse("ce798c73-638b-4c70-adea-9092615fbe01");
                toAdd.Character_id = id;
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);

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
                    .Setup(x => x.Set<IsProficient>()).Returns(mockSet.Object);

                var expected = CreateTestData.GetSampleIsProficient();
                var id = expected.Character_id;


                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);

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
            List<IsProficient> IsProficientList = CreateTestData.GetListOfIsProficient();
            var mockSet = new Mock<DbSet<IsProficient>>()
                .SetupData(IsProficientList, o =>
                {
                    return IsProficientList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleIsProficient();
                expected.Arcana = true;
                expected.DexteritySave = false;

                //When something calls for the Characters table, return the DbSet in mockSet
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<IsProficient>()).Returns(mockSet.Object);

                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);


                //Act
                toTest.UpdateProficiencyRecord(expected);

                //Assert
                expected.Should().NotBeNull();
                expected.Should().BeOfType<IsProficient>();
                //Verifies that the object I wished to update was attached to the DbSet.
                //Basically, that means EF confirms that the entity with expected's Primary key will be updated the next time Save is called.
                mockSet.Verify(x => x.Attach(expected), Times.Once());
            }
        }
        [Test]
        public void BaseUserAcces_AddHealthRecord_ValidCall()
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
                var id = Guid.Parse("b346eee6-eba7-4ea7-be2e-911bb9034233");
                expected.Character_id = id;

                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<Health>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);

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
        public void BaseUserAccess_GetHealthRecord_ValidCall()
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
                    .Setup(x => x.Set<Health>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetHealthRecord(id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Health>();
                expected.Should().BeOfType<Health>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_UpdateHealthRecord_ValidCall()
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
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<Health>()).Returns(mockSet.Object);

                var expected = CreateTestData.GetSampleHealth();
                expected.MaxHP = 200;
                expected.DeathSaveSuccesses = 2;
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);

                //Act
                toTest.UpdateHealthRecord(expected);


                //Assert
                expected.Should().NotBeNull();
                expected.Should().BeOfType<Health>();
                //Verifies that the object I wished to update was attached to the DbSet.
                //Basically, that means EF confirms that the entity with expected's Primary key will be updated the next time Save is called.
                mockSet.Verify(x => x.Attach(expected), Times.Once());
            }
        }
        [Test]
        public void BaseUserAccess_AddStatsRecord_ValidCall()
        {
            List<Stats> statsList = CreateTestData.GetListOfStats();
            var mockSet = new Mock<DbSet<Stats>>()
                .SetupData(statsList, o =>
                {
                    return statsList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleStats();
                var id = Guid.Parse("b346eee6-eba7-4ea7-be2e-911bb9034233");
                expected.Character_id = id;

                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<Stats>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                toTest.AddStatsRecord(expected);
                var actual = toTest.GetStatsRecord(id);

                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Stats>();
                expected.Should().BeOfType<Stats>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_GetStatsRecord_ValidCall()
        {

            List<Stats> statsList = CreateTestData.GetListOfStats();
            var mockSet = new Mock<DbSet<Stats>>()
                .SetupData(statsList, o =>
                {
                    return statsList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleStats();
                mockContext.Mock<CharacterContext>()
                   .Setup(x => x.Set<Stats>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetStatsRecord(expected.Character_id);

                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Stats>();
                expected.Should().BeOfType<Stats>();
                actual.Should().BeEquivalentTo(expected);

            }

        }
        [Test]
        public void BaseUserAccess_UpdateStatsRecord_ValidCall()
        {
            //Arrange

            List<Stats> statsList = CreateTestData.GetListOfStats();
            var mockSet = new Mock<DbSet<Stats>>()
                .SetupData(statsList, o =>
                {
                    return statsList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleStats();
                expected.Strength = 20;
                expected.Dexterity = 20;
                expected.Constitution = 24;
                mockContext.Mock<CharacterContext>()
                   .Setup(x => x.Set<Stats>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                toTest.UpdateStatsRecord(expected);

                //Assert

                expected.Should().NotBeNull();

                expected.Should().BeOfType<Stats>();
                //Verifies that the object I wished to update was attached to the DbSet.
                //Basically, that means EF confirms that the entity with expected's Primary key will be updated the next time Save is called.
                mockSet.Verify(x => x.Attach(expected), Times.Once());

            }
        }

        [Test]
        public void BaseUserAccess_AddCurrencyRecord_ValidCall()
        {

            //Arrange
            List<Currency> currencyList = CreateTestData.GetListOfCurrency();
            var mockSet = new Mock<DbSet<Currency>>()
                .SetupData(currencyList, o =>
                {
                    return currencyList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleCurrency();
                var id = Guid.Parse("b346eee6-eba7-4ea7-be2e-911bb9034233");
                expected.Character_id = id;

                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<Currency>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                toTest.AddCurrencyRecord(expected);
                var actual = toTest.GetCurrencyRecord(id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Currency>();
                expected.Should().BeOfType<Currency>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_GetCurrencyRecord_ValidCall()
        {
            //Arrange
            List<Currency> currencyList = CreateTestData.GetListOfCurrency();
            var mockSet = new Mock<DbSet<Currency>>()
                .SetupData(currencyList, o =>
                {
                    return currencyList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleCurrency();
                mockContext.Mock<CharacterContext>()
                   .Setup(x => x.Set<Currency>()).Returns(mockSet.Object);


                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetCurrencyRecord(expected.Character_id);


                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Currency>();
                expected.Should().BeOfType<Currency>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_UpdateCurrencyRecord_ValidCall()
        {
            //Arrange
            List<Currency> currencyList = CreateTestData.GetListOfCurrency();
            var mockSet = new Mock<DbSet<Currency>>()
                .SetupData(currencyList, o =>
                {
                    return currencyList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleCurrency();
                expected.GoldPieces = 427;

                mockContext.Mock<CharacterContext>()
                  .Setup(x => x.Set<Currency>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                toTest.UpdateCurrencyRecord(expected);

                //Assert
                expected.Should().NotBeNull();
                expected.Should().BeOfType<Currency>();
                //Verifies that the object I wished to update was attached to the DbSet.
                //Basically, that means EF confirms that the entity with expected's Primary key will be updated the next time Save is called.
                mockSet.Verify(x => x.Attach(expected), Times.Once());
            }
        }

        [Test]
        public void BaseUserAccess_AddNote_ValidCall()
        {
            List<Note> listOfNotes = CreateTestData.GetListOfNotes();
            var mockSet = new Mock<DbSet<Note>>()
                .SetupData(listOfNotes, o =>
                {
                    return listOfNotes.Single(x => x.Note_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleNote();
                var id = Guid.Parse("b346eee6-eba7-4ea7-be2e-911bb9034233");
                expected.Note_id = id;

                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<Note>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                toTest.AddNote(expected);
                var actual = toTest.GetNote(id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Note>();
                expected.Should().BeOfType<Note>();
                actual.Should().BeEquivalentTo(expected);

            }
        }
        [Test]
        public void BaseUserAccess_GetNote_ValidCall()
        {
        //Arrange
        List<Note> listOfNotes = CreateTestData.GetListOfNotes();
        var mockSet = new Mock<DbSet<Note>>()
            .SetupData(listOfNotes, o =>
            {
                return listOfNotes.Single(x => x.Note_id.CompareTo(o.First()) == 0);
            });

        using (var mockContext = AutoMock.GetLoose())
        {

            var expected = CreateTestData.GetSampleNote();
            mockContext.Mock<CharacterContext>()
               .Setup(x => x.Set<Note>()).Returns(mockSet.Object);

                //Act
            IUnitOfWork worker = mockContext.Create<UnitOfWork>();
            IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
            var actual = toTest.GetNote(expected.Note_id);

            //Assert
            actual.Should().NotBeNull();
            expected.Should().NotBeNull();
            actual.Should().BeOfType<Note>();
            expected.Should().BeOfType<Note>();
            actual.Should().BeEquivalentTo(expected);
        }
    }
        [Test]
        public void BaseUserAcess_GetNotesOwnedBy_ValidCall()
        {
            //Arrange
            List<Note> listOfNotes = CreateTestData.GetListOfNotes();
            var mockSet = new Mock<DbSet<Note>>()
                .SetupData(listOfNotes, o =>
                {
                    return listOfNotes.Single(x => x.Note_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {

                var expected = new List<Note>();
                expected.Add(CreateTestData.GetSampleNote());
                var GreatAxe = new Note()
                {
                    Note_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                    Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                    Name = "How To Use a Great Axe Ft. Grog Strongjaw",
                    Contents = "Lorem Ipsum"
                };

                expected.Add(GreatAxe);

                mockContext.Mock<CharacterContext>()
                   .Setup(x => x.Set<Note>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetNotesOwnedBy(Guid.Parse("11111111-2222-3333-4444-555555555555"));

                actual.Should().NotBeEmpty();
                expected.Should().NotBeEmpty();
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<List<Note>>();
                expected.Should().BeOfType<List<Note>>();
                actual.Should().BeEquivalentTo(expected);

            }
        }
        [Test]
        public void BaseUserAccess_UpdateNote_ValidCall()
        {
            //Arrange
            List<Note> listOfNotes = CreateTestData.GetListOfNotes();
            var mockSet = new Mock<DbSet<Note>>()
                .SetupData(listOfNotes, o =>
                {
                    return listOfNotes.Single(x => x.Note_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleNote();
                expected.Contents = "This is where I post the entire Bee Movie script, right?";

                mockContext.Mock<CharacterContext>()
                  .Setup(x => x.Set<Note>()).Returns(mockSet.Object);


                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                toTest.UpdateNote(expected);

                //Assert
                expected.Should().NotBeNull();
                expected.Should().BeOfType<Note>();
                //Verifies that the object I wished to update was attached to the DbSet.
                //Basically, that means EF confirms that the entity with expected's Primary key will be updated the next time Save is called.
                mockSet.Verify(x => x.Attach(expected), Times.Once());
            }
        }
        [Test]
        public void BaseUserAccess_DeleteNote_ValidCall()
        {
            //Arrange
            List<Note> listOfNotes = CreateTestData.GetListOfNotes();
            var mockSet = new Mock<DbSet<Note>>()
                .SetupData(listOfNotes, o =>
                {
                    return listOfNotes.Single(x => x.Note_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var ToBeDeleted = CreateTestData.GetSampleNote();

                mockContext.Mock<CharacterContext>()
                   .Setup(x => x.Set<Note>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                toTest.DeleteNote(ToBeDeleted.Note_id);

                //Assert
                listOfNotes.Should().NotBeEmpty();
                listOfNotes.Should().NotContain(ToBeDeleted);
                listOfNotes.Should().BeOfType<List<Note>>();
            }
        }
    }
}
