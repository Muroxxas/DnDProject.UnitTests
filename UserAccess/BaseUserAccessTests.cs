using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Repository;
using DnDProject.Backend.Repository.Implementations;
using DnDProject.Backend.Unit_Of_Work.Implementations;
using DnDProject.Backend.Unit_Of_Work.Interfaces;
using DnDProject.Backend.UserAccess.Implementations;
using DnDProject.Backend.UserAccess.Interfaces;
using DnDProject.Entities.Character.DataModels;
using DnDProject.Entities.Class.DataModels;
using DnDProject.Entities.Items.DataModels;
using DnDProject.Entities.Races.DataModels;
using DnDProject.Entities.Spells.DataModels;
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
            List<CharacterDM> charList = new List<CharacterDM>();
            charList.Add(CreateTestData.getSampleCharacter());

            var mockSet = new Mock<DbSet<CharacterDM>>()
                .SetupData(charList, o =>
                {
                    return charList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<CharacterContext>().Setup(x => x.Set<CharacterDM>()).Returns(mockSet.Object);
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);

                var expected = CreateTestData.getSampleCharacter();
                Guid id = Guid.Parse("757f4657-ff89-4a9d-a31b-aeda05a38615");
                expected.Character_id = id;

                //Act
                toTest.AddCharacter(expected);
                var actual = toTest.GetCharacter(id);

                expected.Should().BeOfType<CharacterDM>();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<CharacterDM>();
                actual.Should().NotBeNull();

                actual.Should().BeEquivalentTo(expected);

            }

        }
        [Test]
        public void BaseUserAccess_GetCharacter_ValidCall()
        {
            //Arrange
            List<CharacterDM> charList = new List<CharacterDM>();
            charList.Add(CreateTestData.getSampleCharacter());

            var mockSet = new Mock<DbSet<CharacterDM>>()
                .SetupData(charList, o =>
                {
                    return charList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<CharacterContext>().Setup(x => x.Set<CharacterDM>()).Returns(mockSet.Object);
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);

                //Act
                var expected = CreateTestData.getSampleCharacter();
                Guid id = expected.Character_id;
                var actual = toTest.GetCharacter(id);

                expected.Should().BeOfType<CharacterDM>();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<CharacterDM>();
                actual.Should().NotBeNull();

                actual.Should().BeEquivalentTo(expected);

            }
        }

        [Test]
        public void BaseUserAccess_DeleteCharacter_ValidCall()
        {
            //Arrange
            List<CharacterDM> charList = CreateTestData.GetListOfCharacters();
            var mockSet = new Mock<DbSet<CharacterDM>>()
                .SetupData(charList, o =>
                {
                    return charList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<CharacterDM>()).Returns(mockSet.Object);
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<CharacterDM>().Remove(It.IsAny<CharacterDM>()))
                        .Callback<CharacterDM>((entity) => charList.Remove(entity));



                var toDelete = CreateTestData.getSampleCharacter();
                CharacterDM expected = null;
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

        [Test]
        public void BaseUserAccess_GetSpell_ValidCall()
        {
            List<Spell> spells = CreateTestData.GetListOfSpells();
            var mockSet = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleSpell();
                var id = expected.Spell_id;

                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetSpell(id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Spell>();
                expected.Should().BeOfType<Spell>();
                actual.Should().BeEquivalentTo(expected);
            };
        }
        [Test]
        public void BaseUserAccess_GetSpellsKnownBy_ValidCall()
        {
            //Arrange

            //Create list of spells
            List<Spell> spells = CreateTestData.GetListOfSpells();
            //Create list of Character_Spell
            List<Spell_Character> knownSpells = CreateTestData.GetListOfKnownSpells();

            //We'll be looking for spells that Caleb knows!
            List<Spell> expected = new List<Spell>();
            Spell WebOfFire = new Spell
            {
                Spell_id = Guid.Parse("51b4c563-2040-4c7d-a23e-cab8d5d3c73b"),
                Name = "Widogast's Web Of Fire",
                Description = "The caster deals a shitton of fire damage to the target.",
                Level = 4,
                School_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                CastingTime = "1 Action",
                Duration = "Instant",
                Range = "120 feet",
                RequiresVerbal = true,
                RequiresSomantic = true,
                RequiresMaterial = true,
                RequiresConcentration = false
            };
            expected.Add(WebOfFire);
            Spell nineSidedTower = new Spell
            {
                Spell_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f"),
                Name = "Widogast's Nascent Nine-sided Tower",
                Description = "A flavored Magnificent Mansion",
                Level = 7,
                School_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                CastingTime = "1 minute",
                Range = "100 feet",
                Duration = "24 hours",
                RequiresVerbal = true,
                RequiresSomantic = true,
                RequiresMaterial = false,
                RequiresConcentration = true
            };
            expected.Add(nineSidedTower);

            var spellMockSet = new Mock<DbSet<Spell>>()
               .SetupData(spells, o =>
               {
                   return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
               });

            var knownSpellsMockSet = new Mock<DbSet<Spell_Character>>()
                .SetupData(knownSpells, o =>
                {
                    return knownSpells.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {

                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(spellMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.KnownSpells).Returns(knownSpellsMockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var Caleb_id = Guid.Parse("11111111-2222-3333-4444-555555555555");
                var actual = toTest.GetSpellsKnownBy(Caleb_id).ToList();

                //Assert
                actual.Should().NotBeEmpty();
                actual.Should().NotBeNull();
                actual.Should().BeOfType<List<Spell>>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_GetSpellsOfSchool_ValidCall()
        {
            //Arrange
            List<Spell> spells = CreateTestData.GetListOfSpells();

            List<School> schools = CreateTestData.GetListOfSchools();

            //We're expecting the only Conjutation spell contained within the spells list - Caleb's tower.
            Spell NineSidedTower = new Spell
            {
                Spell_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f"),
                Name = "Widogast's Nascent Nine-sided Tower",
                Description = "A flavored Magnificent Mansion",
                Level = 7,
                School_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                CastingTime = "1 minute",
                Range = "100 feet",
                Duration = "24 hours",
                RequiresVerbal = true,
                RequiresSomantic = true,
                RequiresMaterial = false,
                RequiresConcentration = true
            };
            List<Spell> expected = new List<Spell>();
            expected.Add(NineSidedTower);

            var spellMockSet = new Mock<DbSet<Spell>>()
               .SetupData(spells, o =>
               {
                   return spells.Single(x => x.School_id.CompareTo(o.First()) == 0);
               });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(spellMockSet.Object);
                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var conjuration_id = NineSidedTower.School_id;
                var actual = toTest.GetSpellsOfSchool(conjuration_id);

                //Assert
                actual.Should().NotBeEmpty();
                actual.Should().NotBeNull();
                actual.Should().BeOfType<List<Spell>>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_GetSpellsCastableBy_ValidCall()
        {
            //Arrange
            List<Spell> spells = CreateTestData.GetListOfSpells();

            List<Spell_Class> CastableByRecords = CreateTestData.GetListOfCastableByRecords();

            //I expect three spells - Web of Fire, Voltaic Bolt, and the Tower, all of which can be cast by a wizard.
            List<Spell> expected = new List<Spell>();
            Spell NineSidedTower = new Spell
            {
                Spell_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f"),
                Name = "Widogast's Nascent Nine-sided Tower",
                Description = "A flavored Magnificent Mansion",
                Level = 7,
                School_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                CastingTime = "1 minute",
                Range = "100 feet",
                Duration = "24 hours",
                RequiresVerbal = true,
                RequiresSomantic = true,
                RequiresMaterial = false,
                RequiresConcentration = true
            };
            expected.Add(NineSidedTower);
            Spell WebOfFire = new Spell
            {
                Spell_id = Guid.Parse("51b4c563-2040-4c7d-a23e-cab8d5d3c73b"),
                Name = "Widogast's Web Of Fire",
                Description = "The caster deals a shitton of fire damage to the target.",
                Level = 4,
                School_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                CastingTime = "1 Action",
                Duration = "Instant",
                Range = "120 feet",
                RequiresVerbal = true,
                RequiresSomantic = true,
                RequiresMaterial = true,
                RequiresConcentration = false
            };
            expected.Add(WebOfFire);
            Spell VoltaicBolt = new Spell
            {
                Spell_id = Guid.Parse("a9756f3d-55d0-40cd-8083-6b547e4932ab"),
                Name = "Brenatto's Voltaic Bolt",
                Description = "The caster's next ranged attack deals an additional 3d6 lightning damage",
                Level = 1,
                School_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                CastingTime = "1 Bonus Action",
                Duration = "1 round",
                Range = "30 feet",
                RequiresVerbal = false,
                RequiresSomantic = true,
                RequiresMaterial = true,
                RequiresConcentration = false
            };
            expected.Add(VoltaicBolt);

            var spellMockSet = new Mock<DbSet<Spell>>()
               .SetupData(spells, o =>
               {
                   return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
               });

            var SpellCastableByMockSet = new Mock<DbSet<Spell_Class>>()
                .SetupData(CastableByRecords, o =>
                {
                    return CastableByRecords.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(spellMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.CastableByRecords).Returns(SpellCastableByMockSet.Object);


                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                Guid WIzard_id = Guid.Parse("b74e228f-015d-45b4-af0f-a6781976535a");
                var actual = toTest.GetSpellsCastableBy(WIzard_id).ToList();

                //Assert
                actual.Should().NotBeEmpty();
                actual.Should().NotBeNull();
                actual.Should().BeOfType<List<Spell>>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_GetSpellMaterials_ValidCall()
        {
            //Arrange
            List<Material> Materials = CreateTestData.GetListOfMaterials();
            var mockSet = new Mock<DbSet<Material>>()
                .SetupData(Materials, o =>
                {
                    return Materials.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Materials).Returns(mockSet.Object);

                Material expected = new Material()
                {
                    Spell_id = Guid.Parse("caf8b2d1-7903-493c-bc3a-ec2fc554d533"),
                    materials = "Diamonds worth 300 gp, which the spell consumes."
                };

                //act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetSpellMaterials(expected.Spell_id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Material>();
                expected.Should().BeOfType<Material>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_GetIdsOfClassesThatCanCastSpell_ValidCall()
        {
            //Arrange
            List<Spell_Class> castableByList = CreateTestData.GetListOfCastableByRecords();
            var mockSet = new Mock<DbSet<Spell_Class>>()
                .SetupData(castableByList, o =>
                {
                    return castableByList.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });
            var expected = new Spell_Class
            {
                Spell_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f"),
                Class_id = Guid.Parse("b74e228f-015d-45b4-af0f-a6781976535a")
            };
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.CastableByRecords).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetIdsOfClassesThatCanCastSpell(expected.Spell_id);

                //Assert
                actual.Should().Contain(expected.Class_id);
            }
        }
        [Test]
        public void BaseUserAccess_GetKnownSpellRecord_ValidCall()
        {
            //Arrange
            //Create list of Character_Spell
            List<Spell_Character> knownSpells = CreateTestData.GetListOfKnownSpells();
            var mockSet = new Mock<DbSet<Spell_Character>>()
                .SetupData(knownSpells, o =>
                {
                    return knownSpells.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            //We want to find the record that indicates Caleb can create his tower.
            var expected = CreateTestData.GetSampleKnownSpell();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.KnownSpells).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell_Character>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetKnownSpellRecord(expected.Character_id, expected.Spell_id);

                //Arrange
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_GetKnownSpellRecordsForCharacter_ValidCall()
        {
            List<Spell_Character> knownSpells = CreateTestData.GetListOfKnownSpells();
            var mockSet = new Mock<DbSet<Spell_Character>>()
                .SetupData(knownSpells, o =>
                {
                    return knownSpells.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            List<Spell_Character> expected = new List<Spell_Character>();
            Spell_Character Caleb_Tower = CreateTestData.GetSampleKnownSpell();
            expected.Add(Caleb_Tower);

            Spell_Character Caleb_WebOfFire = new Spell_Character()
            {
                Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                Spell_id = Guid.Parse("51b4c563-2040-4c7d-a23e-cab8d5d3c73b")
            };
            expected.Add(Caleb_WebOfFire);

            Guid Caleb_id = Guid.Parse("11111111-2222-3333-4444-555555555555");

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.KnownSpells).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell_Character>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetKnownSpellRecordsForCharacter(Caleb_id);

                //Arrange
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_GetSchool_ValidCall()
        {
            //Arrange
            List<School> spellSchools = CreateTestData.GetListOfSchools();
            var schoolsMockSet = new Mock<DbSet<School>>()
                .SetupData(spellSchools, o =>
                {
                    return spellSchools.Single(x => x.School_id.CompareTo(o.First()) == 0);
                });

            var expected = CreateTestData.GetSampleSchool();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Schools).Returns(schoolsMockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetSchool(expected.School_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);

            }
        }
        [Test]
        public void BaseUserAccess_CharacterLearnsSpell_ValidCall()
        {
            //Arrange
            List<Spell_Character> KnownSpells = CreateTestData.GetListOfKnownSpells();
            List<Spell> spells = CreateTestData.GetListOfSpells();

            var mockKnownSpells = new Mock<DbSet<Spell_Character>>()
                .SetupData(KnownSpells, o =>
                {
                    return KnownSpells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });
            var mockSpells = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            //Caleb learns Eldritch blast, somehow!
            Guid Caleb_id = Guid.Parse("11111111-2222-3333-4444-555555555555");
            Guid EldritchBlast_id = Guid.Parse("45c1a8cc-2e3e-4e29-8eeb-f9fa0cc9e27e");

            Spell EldritchBlast = new Spell
            {
                Spell_id = Guid.Parse("45c1a8cc-2e3e-4e29-8eeb-f9fa0cc9e27e"),
                Name = "Eldritch Blast",
                Description = "Cast eldritch blast",
                Level = 0,
                School_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                CastingTime = "1 Action",
                Duration = "Instant",
                Range = "60 feet",
                RequiresVerbal = true,
                RequiresSomantic = true,
                RequiresMaterial = true,
                RequiresConcentration = false
            };

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(mockSpells.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.KnownSpells).Returns(mockKnownSpells.Object);

                //act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                toTest.CharacterLearnsSpell(Caleb_id, EldritchBlast_id);
                var actual = toTest.GetSpellsKnownBy(Caleb_id);


                //Assert
                actual.Should().ContainEquivalentOf(EldritchBlast);
            }
        }
        [Test]
        public void BaseUserAccess_CharacterForgetsSpell_ValidCall()
        {
            //Arrange
            List<Spell_Character> KnownSpells = CreateTestData.GetListOfKnownSpells();
            List<Spell> spells = CreateTestData.GetListOfSpells();

            var mockKnownSpells = new Mock<DbSet<Spell_Character>>()
                .SetupData(KnownSpells, o =>
                {
                    return KnownSpells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });
            var mockSpells = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            //Caleb forgets Web of Fire!
            Guid Caleb_id = Guid.Parse("11111111-2222-3333-4444-555555555555");
            Spell WebOfFire = new Spell
            {
                Spell_id = Guid.Parse("51b4c563-2040-4c7d-a23e-cab8d5d3c73b"),
                Name = "Widogast's Web Of Fire",
                Description = "The caster deals a shitton of fire damage to the target.",
                Level = 4,
                School_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                CastingTime = "1 Action",
                Duration = "Instant",
                Range = "120 feet",
                RequiresVerbal = true,
                RequiresSomantic = true,
                RequiresMaterial = true,
                RequiresConcentration = false
            };

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(mockSpells.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.KnownSpells).Returns(mockKnownSpells.Object);

                //act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                toTest.CharacterForgetsSpell(Caleb_id, WebOfFire.Spell_id);
                var actual = toTest.GetSpellsKnownBy(Caleb_id);

                actual.Should().NotContain(WebOfFire);
            }
        }


        [Test]
        public void BaseUserAccess_GetItem_ValidCall()
        {
            //Arrange
            List<Item> Items = CreateTestData.GetListOfItems();
            var mockSet = new Mock<DbSet<Item>>()
                .SetupData(Items, o =>
                {
                    return Items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleItem();
                var id = expected.Item_id;

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Item>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetItem(id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Item>();
                expected.Should().BeOfType<Item>();
                actual.Should().BeEquivalentTo(expected);
            };
        }

        [Test]
        public void BaseUserAccess_GetHeldItemRecord_ValidCall()
        {
            //Arrange
            List<Character_Item> heldItems = CreateTestData.GetListOfHeldItems();


            var heldItemsMockSet = new Mock<DbSet<Character_Item>>()
              .SetupData(heldItems, o =>
              {
                  return heldItems.Single(x => x.Character_id.CompareTo(o.First()) == 0);
              });


            var expected = CreateTestData.GetSampleHeldItem();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.HeldItems).Returns(heldItemsMockSet.Object);

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Character_Item>()).Returns(heldItemsMockSet.Object);

                //act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetHeldItemRecord(expected.Character_id, expected.Item_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);

            }
        }

        [Test]
        public void BaseUserAccess_GetHeldItemRecordsForCharacter_ValidCall()
        {
            //Arrange
            List<Character_Item> heldItems = CreateTestData.GetListOfHeldItems();


            var heldItemsMockSet = new Mock<DbSet<Character_Item>>()
              .SetupData(heldItems, o =>
              {
                  return heldItems.Single(x => x.Character_id.CompareTo(o.First()) == 0);
              });

            List<Character_Item> expected = new List<Character_Item>();
            Character_Item Vax_Whisper = CreateTestData.GetSampleHeldItem();
            expected.Add(Vax_Whisper);

            Character_Item Vax_Potion = new Character_Item
            {
                Character_id = Guid.Parse("e3a0faef-99da-4d15-bff1-b535a42b955c"),
                Item_id = Guid.Parse("2caa23dc-15e6-4a57-9bb6-62f6d8636ff7"),
                count = 3
            };
            expected.Add(Vax_Potion);

            Guid Vax_id = Guid.Parse("e3a0faef-99da-4d15-bff1-b535a42b955c");

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.HeldItems).Returns(heldItemsMockSet.Object);

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Character_Item>()).Returns(heldItemsMockSet.Object);

                //act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetHeldItemRecordsForCharacter(Vax_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);

            }
        }
        [Test]
        public void BaseUserAccess_GetItemsHeldBy_ValidCall()
        {
            //Arrange
            List<Character_Item> heldItems = CreateTestData.GetListOfHeldItems();
            List<Item> itemset = CreateTestData.GetListOfItems();
            var heldItemsMockSet = new Mock<DbSet<Character_Item>>()
              .SetupData(heldItems, o =>
              {
                  return heldItems.Single(x => x.Character_id.CompareTo(o.First()) == 0);
              });

            var ItemsMockSet = new Mock<DbSet<Item>>()
                .SetupData(itemset, o =>
                {
                    return itemset.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            var Vax_id = Guid.Parse("e3a0faef-99da-4d15-bff1-b535a42b955c");
            var expected = new List<Item>();
            Item Whisper = new Item()
            {
                Item_id = Guid.Parse("709135c3-6f89-46cb-80ae-4097b621e3b0"),
                Name = "Whisper",
                Description = "A Legendary dagger that allows you to teleport to wherever it strikes",
                isEquippable = true,
                isConsumable = false,
                requiresAttunement = true,
                Value = 999
            };
            expected.Add(Whisper);
            Item HealingPotion = new Item
            {
                Item_id = Guid.Parse("2caa23dc-15e6-4a57-9bb6-62f6d8636ff7"),
                Name = "Healing potion",
                Description = "Upon consumption of the whole potion, the imbiber heals for 2d4+2 health.",
                isEquippable = false,
                isConsumable = true,
                requiresAttunement = false,
                Value = 50
            };
            expected.Add(HealingPotion);
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(ItemsMockSet.Object);

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.HeldItems).Returns(heldItemsMockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetItemsHeldBy(Vax_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_GetAllTags_ValidCall()
        {
            List<Tag> tags = CreateTestData.GetListOfTags();
            var mockSet = new Mock<DbSet<Tag>>()
                .SetupData(tags, o =>
                {
                    return tags.Single(x => x.Tag_id.CompareTo(o.First()) == 0);
                });

            var expected = CreateTestData.GetListOfTags();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Tags).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetAllTags();

                //Assert
                actual.Should().BeEquivalentTo(expected);

            }
        }

        [Test]
        public void BaseUserAccess_GetTagsForItem_ValidCall()
        {
            List<Item_Tag> itemTags = CreateTestData.GetListOfItemTags();
            List<Tag> tags = CreateTestData.GetListOfTags();
            var ITmockSet = new Mock<DbSet<Item_Tag>>()
                .SetupData(itemTags, o =>
                {
                    return itemTags.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });
            var tagsMockSet = new Mock<DbSet<Tag>>()
                .SetupData(tags, o =>
                {
                    return tags.Single(x => x.Tag_id.CompareTo(o.First()) == 0);
                });
            var expected = new List<Tag>();
            Tag Wondorous = new Tag
            {
                Tag_id = Guid.Parse("e2c7f8a3-52ba-4dc2-baaf-4026718b1f03"),
                TagName = "Wondorous Item"
            };
            expected.Add(Wondorous);
            Tag Weapon = new Tag
            {
                Tag_id = Guid.Parse("172e8478-e1bd-49ba-a7a7-6455d5a58c6e"),
                TagName = "Weapon"
            };
            expected.Add(Weapon);
            Guid whisper_id = Guid.Parse("709135c3-6f89-46cb-80ae-4097b621e3b0");
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Tags).Returns(tagsMockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Item_Tags).Returns(ITmockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetTagsForItem(whisper_id);


                //Assert
                actual.Should().BeEquivalentTo(expected);

            }
        }

        [Test]
        public void BaseUserAccess_CharacterObtainsItem_ValidCall()
        {
            //Arrange
            List<Character_Item> heldItems = new List<Character_Item>();
            List<Item> items = CreateTestData.GetListOfItems();
            var heldItemsMockSet = new Mock<DbSet<Character_Item>>()
                .SetupData(heldItems, o =>
                {
                    return heldItems.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            var ItemsMockSet = new Mock<DbSet<Item>>()
                .SetupData(items, o =>
                {
                    return items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var Whisper = CreateTestData.GetSampleItem();
                var Whisper_id = Whisper.Item_id;
                var Vax_id = Guid.Parse("e3a0faef-99da-4d15-bff1-b535a42b955c");

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(ItemsMockSet.Object);

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.HeldItems).Returns(heldItemsMockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                toTest.CharacterObtainsItem(Vax_id, Whisper_id);

                var actual = toTest.GetItemsHeldBy(Vax_id);

                //Assert
                actual.Should().ContainEquivalentOf(Whisper);

            }
        }
        [Test]
        public void BaseUserAccess_CharacterLosesItem_ValidCall()
        {
            //Arrange - Vax loses Whisper
            List<Character_Item> heldItems = CreateTestData.GetListOfHeldItems();
            List<Item> itemset = CreateTestData.GetListOfItems();
            var heldItemsMockSet = new Mock<DbSet<Character_Item>>()
              .SetupData(heldItems, o =>
              {
                  return heldItems.Single(x => x.Character_id.CompareTo(o.First()) == 0);
              });

            var ItemsMockSet = new Mock<DbSet<Item>>()
                .SetupData(itemset, o =>
                {
                    return itemset.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var Whisper = CreateTestData.GetSampleItem();
                var Whisper_id = Whisper.Item_id;
                var Vax_id = Guid.Parse("e3a0faef-99da-4d15-bff1-b535a42b955c");

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(ItemsMockSet.Object);

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.HeldItems).Returns(heldItemsMockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                toTest.CharacterLosesItem(Vax_id, Whisper_id);
                var actual = toTest.GetItemsHeldBy(Vax_id);

                //Assert
                actual.Should().NotContain(Whisper);
            }
        }

        [Test]
        public void BaseUserAccess_GetAllPlayableClasses_ValidCall()
        {
            //Arrange
            List<PlayableClass> listofPlayableClasses = CreateTestData.GetPlayableClasses();

            var mockSet = new Mock<DbSet<PlayableClass>>()
                .SetupData(listofPlayableClasses, o =>
                {
                    return listofPlayableClasses.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });

            List<PlayableClass> expected = CreateTestData.GetPlayableClasses();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Classes).Returns(mockSet.Object);


                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<PlayableClass>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetAllPlayableClasses();

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_CharacterLearnsClass_ValidCall()
        {
            //Arrange
            List<Character_Class_Subclass> listofCharacter_Class_Subclass = new List<Character_Class_Subclass>();

            var mockSet = new Mock<DbSet<Character_Class_Subclass>>()
                .SetupData(listofCharacter_Class_Subclass, o =>
                {
                    return listofCharacter_Class_Subclass.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetCharacter_Class_Subclass();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.KnownClasses).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                toTest.CharacterLearnsClass(expected);
                //Assert
                listofCharacter_Class_Subclass.Should().ContainEquivalentOf(expected);
            }
        }
        [Test]
        public void BaseUserAccess_CharacterLearnsClasses_ValidCall()
        {
            //Arrange
            List<Character_Class_Subclass> listofCharacter_Class_Subclass = new List<Character_Class_Subclass>();

            var mockSet = new Mock<DbSet<Character_Class_Subclass>>()
                .SetupData(listofCharacter_Class_Subclass, o =>
                {
                    return listofCharacter_Class_Subclass.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var Percy_id = Guid.Parse("6983e8dc-3e3c-4853-ac49-ba33f236723a");
            var learnedClasses = new List<Guid>();
            var Fighter_id = Guid.Parse("15478d70-f96e-4c14-aeaf-4a1e35605874");
            learnedClasses.Add(Fighter_id);
            var Ranger_id = Guid.Parse("da7d6227-d330-44ab-8001-880dbf52110a");
            learnedClasses.Add(Ranger_id);

            var expected = new List<Character_Class_Subclass>();
            var Percy_Fighter = new Character_Class_Subclass
            {
                Character_id = Percy_id,
                Class_id = Fighter_id,
                ClassLevel = 1,
                RemainingHitDice = 1
            };
            expected.Add(Percy_Fighter);
            var Percy_Ranger = new Character_Class_Subclass
            {
                Character_id = Percy_id,
                Class_id = Ranger_id,
                ClassLevel = 1,
                RemainingHitDice = 1
            };
            expected.Add(Percy_Ranger);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.KnownClasses).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                toTest.CharacterLearnsClasses(Percy_id, learnedClasses);
                //Assert
                listofCharacter_Class_Subclass.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_CharacterForgetsClass_ValidCall()
        {
            //Arrange
            List<Character_Class_Subclass> listofCharacter_Class_Subclass = CreateTestData.GetListOfCharacter_Class_Subclass();

            var mockSet = new Mock<DbSet<Character_Class_Subclass>>()
                .SetupData(listofCharacter_Class_Subclass, o =>
                {
                    return listofCharacter_Class_Subclass.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var toBeDeleted = new Character_Class_Subclass
            {
                Character_id = Guid.Parse("6983e8dc-3e3c-4853-ac49-ba33f236723a"),
                Class_id = Guid.Parse("15478d70-f96e-4c14-aeaf-4a1e35605874")
            };
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.KnownClasses).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                toTest.CharacterForgetsClass(toBeDeleted.Character_id, toBeDeleted.Class_id);

                //Assert
                listofCharacter_Class_Subclass.Should().NotContain(toBeDeleted);
            }
        }

        [Test]
        public void BaseUserAccess_GetClassesOfCharacter_ValidCall()
        {
            //Arrange
            List<PlayableClass> playableClasses = CreateTestData.GetPlayableClasses();
            List<Character_Class_Subclass> knownclasses = CreateTestData.GetListOfCharacter_Class_Subclass();

            var classMockSet = new Mock<DbSet<PlayableClass>>()
                .SetupData(playableClasses, o =>
                {
                    return playableClasses.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var knownClassesMockSet = new Mock<DbSet<Character_Class_Subclass>>()
                .SetupData(knownclasses, o =>
                {
                    return knownclasses.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            var Percy_id = Guid.Parse("6983e8dc-3e3c-4853-ac49-ba33f236723a");
            var expected = CreateTestData.GetSampleClass();
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Classes).Returns(classMockSet.Object);
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.KnownClasses).Returns(knownClassesMockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetClassesOfCharacter(Percy_id);

                //Assert
                actual.Should().ContainEquivalentOf(expected);

            }
        }
        [Test]
        public void Base_UserAccess_GetKnownClassRecordOfCharacterAndClass_ValidCall()
        {
            //Arrange
            List<Character_Class_Subclass> listofCharacter_Class_Subclass = CreateTestData.GetListOfCharacter_Class_Subclass();

            var mockSet = new Mock<DbSet<Character_Class_Subclass>>()
                .SetupData(listofCharacter_Class_Subclass, o =>
                {
                    return listofCharacter_Class_Subclass.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var expected = new Character_Class_Subclass
            {
                Character_id = Guid.Parse("da7d6227-d330-44ab-8001-880dbf52110a"),
                Class_id = Guid.Parse("969c08ca-f983-4ddd-b351-31962f2429cd"),
                Subclass_id = Guid.Parse("c7de67ae-3a65-4261-9c09-05a7b0c527bb"),
                RemainingHitDice = 20,
                ClassLevel = 20
            };
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.KnownClasses).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetKnownClassRecordOfCharaterAndClass(expected.Character_id, expected.Class_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_GetPlayableClass_ValidCall()
        {
            //Arrange

            List<PlayableClass> playableClasses = CreateTestData.GetPlayableClasses();

            var mockSet = new Mock<DbSet<PlayableClass>>()
                .SetupData(playableClasses, o =>
                {
                    return playableClasses.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetSampleClass();
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<PlayableClass>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetPlayableClass(expected.Class_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_GetAbilitiesOfClass_ValidCall()
        {
            //Arrange
            List<ClassAbility> listofClassAbility = CreateTestData.GetListOfClassAbility();

            var mockSet = new Mock<DbSet<ClassAbility>>()
                .SetupData(listofClassAbility, o =>
                {
                    return listofClassAbility.Single(x => x.ClassAbility_id.CompareTo(o.First()) == 0);
                });
            ClassAbility expected = CreateTestData.GetClassAbility();
            var notExpected1 = new ClassAbility
            {
                ClassAbility_id = Guid.Parse("19e51104-8590-4199-b7e2-079993bb8ccb"),
                Class_id = Guid.Parse("4e82620a-0496-4ecc-b6d4-05faa064310b"),
                Name = "Spell Master",
                Description = "Choose a 1st level spell and a 2nd level spell in your spellbook. As long as you have them prepared, you can cast them without consuming a spell slot.",
                LevelLearned = 18
            };
            var notExpected2 = new ClassAbility
            {
                ClassAbility_id = Guid.Parse("97bd8231-a001-4228-824f-7606202913b0"),
                Class_id = Guid.Parse("969c08ca-f983-4ddd-b351-31962f2429cd"),
                Name = "Vanish",
                Description = "You can Hide as a bonus action.",
                LevelLearned = 14
            };
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.ClassAbilities).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetAbilitiesOfClass(expected.Class_id);

                //Assert
                actual.Should().ContainEquivalentOf(expected);
                actual.Should().NotContain(notExpected1);
                actual.Should().NotContain(notExpected2);
                actual.Should().NotContainNulls();
                actual.Should().NotBeEmpty();
            }
        }
        [Test]
        public void BaseUserAccess_GetAbilitiesOfClassAtOrBelowLevel_ValidCall()
        {
            //Arrange
            List<ClassAbility> listofClassAbility = CreateTestData.GetListOfClassAbility();

            var mockSet = new Mock<DbSet<ClassAbility>>()
                .SetupData(listofClassAbility, o =>
                {
                    return listofClassAbility.Single(x => x.ClassAbility_id.CompareTo(o.First()) == 0);
                });
            ClassAbility expected = new ClassAbility
            {
                ClassAbility_id = Guid.Parse("97bd8231-a001-4228-824f-7606202913b0"),
                Class_id = Guid.Parse("969c08ca-f983-4ddd-b351-31962f2429cd"),
                Name = "Vanish",
                Description = "You can Hide as a bonus action.",
                LevelLearned = 14
            };
            var notExpected1 = CreateTestData.GetClassAbility();
            var notExpected2 = new ClassAbility
            {
                ClassAbility_id = Guid.Parse("19e51104-8590-4199-b7e2-079993bb8ccb"),
                Class_id = Guid.Parse("4e82620a-0496-4ecc-b6d4-05faa064310b"),
                Name = "Spell Master",
                Description = "Choose a 1st level spell and a 2nd level spell in your spellbook. As long as you have them prepared, you can cast them without consuming a spell slot.",
                LevelLearned = 18
            };

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.ClassAbilities).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetAbilitiesOfClassAtOrBelowLevel(expected.Class_id, 14);

                //Assert
                actual.Should().ContainEquivalentOf(expected);
                actual.Should().NotContain(notExpected1);
                actual.Should().NotContain(notExpected2);
                actual.Should().NotContainNulls();
                actual.Should().NotBeEmpty();
            }
        }
        [Test]
        public void BaseUserAccess_GetAbility_ValidCall()
        {
            //Arrange
            List<ClassAbility> listofClassAbility = CreateTestData.GetListOfClassAbility();

            var mockSet = new Mock<DbSet<ClassAbility>>()
                .SetupData(listofClassAbility, o =>
                {
                    return listofClassAbility.Single(x => x.ClassAbility_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetClassAbility();
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<ClassAbility>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetAbility(expected.ClassAbility_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }




        [Test]
        public void BaseUserAccess_GetSubclass_ValidCall()
        {
            //Arrange
            List<Subclass> listofSubclass = CreateTestData.GetListOfSubclass();

            var mockSet = new Mock<DbSet<Subclass>>()
                .SetupData(listofSubclass, o =>
                {
                    return listofSubclass.Single(x => x.Subclass_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetSubclass();
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<Subclass>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetSubclass(expected.Subclass_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_GetAllSubclassesForClass_ValidCall()
        {
            //Arrange
            List<Subclass> subclasses = CreateTestData.GetListOfSubclass();
            var subclassesMockSet = new Mock<DbSet<Subclass>>()
                .SetupData(subclasses, o =>
                {
                    return subclasses.Single(x => x.Subclass_id.CompareTo(o.First()) == 0);
                });

            List<Subclass> expected = new List<Subclass>();
            Subclass gunslinger = CreateTestData.GetSubclass();
            expected.Add(gunslinger);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Subclasses).Returns(subclassesMockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetAllSubclassesForClass(gunslinger.Class_id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<List<Subclass>>();
                expected.Should().BeOfType<List<Subclass>>();
                actual.Should().BeEquivalentTo(expected);

            }
        }
        [Test]
        public void BaseUserAccess_CharacterOfClass_LearnsSubclass_ValidCall()
        {
            //Arrange
            List<Character_Class_Subclass> knownClasses = new List<Character_Class_Subclass>();
            Character_Class_Subclass Caleb_Wizard_Blank = new Character_Class_Subclass
            {
                Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                Class_id = Guid.Parse("4e82620a-0496-4ecc-b6d4-05faa064310b"),
                RemainingHitDice = 12,
                ClassLevel = 12
            };
            knownClasses.Add(Caleb_Wizard_Blank);
            var mockSet = new Mock<DbSet<Character_Class_Subclass>>()
                .SetupData(knownClasses, o =>
                {
                    return knownClasses.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetCharacter_Class_Subclass();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.KnownClasses).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                toTest.CharacterOfClassLearnsSubclass(
                    Caleb_Wizard_Blank.Character_id,
                    Caleb_Wizard_Blank.Class_id,
                    Guid.Parse("c8d2e23a-a193-4e06-8814-9180d4830732"));

                var actual = knownClasses.First();

                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Character_Class_Subclass>();
                expected.Should().BeOfType<Character_Class_Subclass>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_CharacterOfClassForgetsSubclass_ValidCall()
        {
            //Arrange
            List<Character_Class_Subclass> knownClasses = CreateTestData.GetListOfCharacter_Class_Subclass();
            var mockSet = new Mock<DbSet<Character_Class_Subclass>>()
               .SetupData(knownClasses, o =>
               {
                   return knownClasses.Single(x => x.Character_id.CompareTo(o.First()) == 0);
               });
            Character_Class_Subclass expected = new Character_Class_Subclass
            {
                Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                Class_id = Guid.Parse("4e82620a-0496-4ecc-b6d4-05faa064310b"),
                RemainingHitDice = 12,
                ClassLevel = 12
            };

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.KnownClasses).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                toTest.CharacterOfClassForgetsSubclass(
                    expected.Character_id,
                    expected.Class_id,
                    Guid.Parse("c8d2e23a-a193-4e06-8814-9180d4830732"));

                var actual = knownClasses.First();

                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Character_Class_Subclass>();
                expected.Should().BeOfType<Character_Class_Subclass>();
                actual.Should().BeEquivalentTo(expected);
            }
        }


        [Test]
        public void BaseUserAccess_GetSubclassAbility_ValidCall()
        {
            //Arrange
            List<SubclassAbility> listofSubclassAbility = CreateTestData.GetListOfSubclassAbility();
            SubclassAbility expected = CreateTestData.GetSubclassAbility();
            var mockSet = new Mock<DbSet<SubclassAbility>>()
                .SetupData(listofSubclassAbility, o =>
                {
                    return listofSubclassAbility.Single(x => x.SubclassAbility_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<SubclassAbility>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetSubclassAbility(expected.SubclassAbility_id);

                //Assert
                actual.Should().NotBeNull();
                actual.Should().BeOfType<SubclassAbility>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_GetAllAbilitiesOfSubclass_ValidCall()
        {
            //Arrange
            List<SubclassAbility> listofSubclassAbility = CreateTestData.GetListOfSubclassAbility();
            List<SubclassAbility> expected = new List<SubclassAbility>();
            SubclassAbility Gunslinger = CreateTestData.GetSubclassAbility();
            expected.Add(Gunslinger);
            var gunslinger_id = Gunslinger.Subclass_id;
            var mockSet = new Mock<DbSet<SubclassAbility>>()
                .SetupData(listofSubclassAbility, o =>
                {
                    return listofSubclassAbility.Single(x => x.Subclass_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.SubclassAbilities).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetAllAbilitiesOfSubclass(gunslinger_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_GetAbilitiesOfSubclassAtOrBelowLevel_ValidCall()
        {
            //Arrange
            List<SubclassAbility> listofSubclassAbility = CreateTestData.GetListOfSubclassAbility();

            List<SubclassAbility> expected = new List<SubclassAbility>();
            SubclassAbility Gunslinger = CreateTestData.GetSubclassAbility();
            SubclassAbility Quickdraw = new SubclassAbility
            {
                Subclass_id = Gunslinger.Subclass_id,
                SubclassAbility_id = Guid.Parse("eb852e1e-39a6-47af-86e2-5dfb3fc8bdee"),
                Name = "Quickdraw",
                Description = "You add your proficiency bonus to your initiative. You can also stow a firearm, then draw another firearm as a single object interaction on your turn.",
                LevelLearned = 7
            };
            listofSubclassAbility.Add(Quickdraw);
            expected.Add(Gunslinger);
            var gunslinger_id = Gunslinger.Subclass_id;
            var mockSet = new Mock<DbSet<SubclassAbility>>()
                .SetupData(listofSubclassAbility, o =>
                {
                    return listofSubclassAbility.Single(x => x.Subclass_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.SubclassAbilities).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetAbilitiesOfSubclassAtOrBelowLevel(gunslinger_id, 5);

                //Assert
                actual.Should().BeEquivalentTo(expected);
                actual.Should().NotContain(Quickdraw);
            }
        }

        [Test]
        public void BaseUserAccess_GetRace_ValidCall()
        {
            //Arrange
            List<Race> races = CreateTestData.GetListOfRace();
            var mockSet = new Mock<DbSet<Race>>()
                .SetupData(races, o =>
                {
                    return races.Single(x => x.Race_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetSampleRace();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<RaceContext>()
                    .Setup(x => x.Set<Race>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetRace(expected.Race_id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Race>();
                expected.Should().BeOfType<Race>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void BaseUserAccess_GetAllRaces_ValidCall()
        {
            //Arrange
            List<Race> races = CreateTestData.GetListOfRace();
            var mockSet = new Mock<DbSet<Race>>()
                .SetupData(races, o =>
                {
                    return races.Single(x => x.Race_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetListOfRace();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<RaceContext>()
                    .Setup(x => x.Set<Race>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetAllRaces().ToList();

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<List<Race>>();
                expected.Should().BeOfType<List<Race>>();
                actual.Should().BeEquivalentTo(expected);

            }
        }
        [Test]
        public void BaseUserAccess_GetAbilitiesOfRace_ValidCall()
        {
            //Arrange
            List<RaceAbility> raceAbilities = CreateTestData.GetListOfRaceAbility();
            var mockSet = new Mock<DbSet<RaceAbility>>()
                .SetupData(raceAbilities, o =>
                {
                    return raceAbilities.Single(x => x.RaceAbility_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetSampleRaceAbility();
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<RaceContext>()
                   .Setup(x => x.RaceAbilities).Returns(mockSet.Object);

                //Act
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IBaseUserAccess toTest = UserAccessFactory.getBaseUserAccess(worker);
                var actual = toTest.GetAbilitiesOfRace(expected.Race_id).ToList();

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<List<RaceAbility>>();
                expected.Should().BeOfType<RaceAbility>();
                actual.Should().ContainEquivalentOf(expected);
            }
        }
    }
}
