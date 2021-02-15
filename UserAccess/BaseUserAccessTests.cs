﻿using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Repository;
using DnDProject.Backend.Repository.Implementations;
using DnDProject.Backend.Unit_Of_Work.Implementations;
using DnDProject.Backend.Unit_Of_Work.Interfaces;
using DnDProject.Backend.UserAccess.Implementations;
using DnDProject.Backend.UserAccess.Interfaces;
using DnDProject.Entities.Character.DataModels;
using DnDProject.Entities.Items.DataModels;
using DnDProject.Entities.Spells.DataModels;
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
            throw new NotImplementedException();
        }
        [Test]
        public void BaseUserAccess_CharacterLearnsClass_ValidCall()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void BaseUserAccess_CharacterForgetsClass_ValidCall()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void BaseUserAccess_GetClassesOfCharacter_ValidCall()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void Base_UserAccess_GetKnownClassRecordOfCharacterAndClass_ValidCall()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void BaseUserAccess_GetPlayableClass_ValidCall()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void BaseUserAccess_GetAbilitiesOfClass_ValidCall()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void BaseUserAccess_GetAbilitiesOfClassAtOrBelowLevel_ValidCall()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void BaseUserAccess_GetAbility_ValidCall()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void BaseUserAccess_GetAbilityOfSubclass_ValidCall()
        {
            throw new NotImplementedException();
        }



        [Test]
        public void BaseUserAccess_GetSubclass_ValidCall()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void BaseUserAccess_GetAllSubclassesForClass_ValidCall()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void BaseUserAccess_CharacterOfClass_LearnsSubclass_ValidCall()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void BaseUserAccess_CharacterOfClassForgetsSubclass_ValidCall()
        {
            throw new NotImplementedException();
        }



        [Test]
        public void BaseUserAccess_GetAllAbilitiesOfSubclass_ValidCall()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void BaseUserAccess_GetAbilitiesOfSubclassAtOrBelowLevel_ValidCall()
        {
            throw new NotImplementedException();
        }

    }
}
