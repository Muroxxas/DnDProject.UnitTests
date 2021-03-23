using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Mapping.Implementations.Generic;
using DnDProject.Backend.Processors.Interfaces;
using DnDProject.Backend.Unit_Of_Work.Implementations;
using DnDProject.Backend.Unit_Of_Work.Interfaces;
using DnDProject.Backend.UserAccess.Interfaces;
using DnDProject.Entities.Character.DataModels;
using DnDProject.Entities.Character.ViewModels;
using DnDProject.Entities.Character.ViewModels.PartialViewModels;
using DnDProject.Entities.Character.ViewModels.PartialViewModels.Components;
using DnDProject.Entities.Class.DataModels;
using DnDProject.Entities.Items.DataModels;
using DnDProject.Entities.Races.DataModels;
using DnDProject.Entities.Races.ViewModels.PartialViewModels.ComponentModels;
using DnDProject.Entities.Spells.DataModels;
using DnDProject.UnitTests.UserAccess;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDProject.UnitTests.Processors._character
{
    [TestFixture]
    public class CreateCharacterTests
    {
        [Test]
        public void CreateCharacter_GET_ReturnNonNullCharacterVM()
        {
            //Arrange
            PrimaryTabVM primaryTab = new PrimaryTabVM
            {
                Races = CreateTestData.GetRacesListModels(),
                Classes = CreateTestData.GetNonNullClassesCM(),
                Stats = new StatsCM
                {
                    Bonus = new StatBonusCM()
                },
                Saves = new SavesCM(),
                Combat = new CombatCM(),
                Skills = new ProficiencyCM
                {
                    isProficient = new IsProficientCM(),
                    TotalBonus = new SkillBonusCM()
                }
            };

            NoteCM blankNote = new NoteCM();
            NoteCM[] noteArray = new NoteCM[1];
            noteArray[0] = blankNote;
            NoteTabVM notesTab = new NoteTabVM
            {
                Notes = noteArray
            };

            InventoryTabVM inventory = new InventoryTabVM
            {
                Money = new MoneyCM(),
                Items = new HeldItemRowCM[0]
            };
            SpellsTabVM spells = new SpellsTabVM
            {
                KnownSpells = new KnownSpellRowCM[0]
            };

            CharacterVM expected = new CharacterVM
            {
                PrimaryTab = primaryTab,
                NotesTab = notesTab,
                InventoryTab = inventory,
                SpellsTab = spells
            };

            using (var mocks = AutoMock.GetLoose())
            {
                mocks.Mock<IBaseUserAccess>()
                    .Setup(x => x.GetAllRaces()).Returns(CreateTestData.GetListOfRace());
                mocks.Mock<IBaseUserAccess>()
                    .Setup(x => x.GetAllPlayableClasses()).Returns(CreateTestData.GetPlayableClasses());

                mocks.Mock<ICharacterCommonFunctions>()
                    .Setup(x => x.itemExists(Guid.NewGuid())).Returns(false);

                var access = mocks.Create<IBaseUserAccess>();
                var commons = mocks.Create<ICharacterCommonFunctions>();

                //Act
                var toTest = ProcessorFactory.getCreateCharacterProcessor(access, commons);
                var actual = toTest.CreateCharacterGET();

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void CreateCharacter_POST_DataAddedToSets()
        {
            //---------- Arrange ----------
            List<CharacterDM> characters = new List<CharacterDM>();
            var characterMockSet = new Mock<DbSet<CharacterDM>>()
                .SetupData(characters, o =>
                {
                    return characters.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            List<Currency> heldMoney = new List<Currency>();
            var moneyMockSet = new Mock<DbSet<Currency>>()
                .SetupData(heldMoney, o =>
                {
                    return heldMoney.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            List<Health> healthRecords = new List<Health>();
            var healthMockSet = new Mock<DbSet<Health>>()
                .SetupData(healthRecords, o =>
                {
                    return healthRecords.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            List<IsProficient> skills = new List<IsProficient>();
            var skillsMockSet = new Mock<DbSet<IsProficient>>()
                .SetupData(skills, o =>
                {
                    return skills.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            List<Note> notes = new List<Note>();
            var notesMockSet = new Mock<DbSet<Note>>()
                .SetupData(notes, o =>
                {
                    return notes.Single(x => x.Note_id.CompareTo(o.First()) == 0);
                });
            List<Stats> stats = new List<Stats>();
            var statsMockSet = new Mock<DbSet<Stats>>()
                .SetupData(stats, o =>
                {
                    return stats.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            List<Character_Class_Subclass> CCSC_Records = new List<Character_Class_Subclass>();
            var ccscMockSet = new Mock<DbSet<Character_Class_Subclass>>()
                .SetupData(CCSC_Records, o =>
                {
                    return CCSC_Records.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            List<Character_Item> inventories = new List<Character_Item>();
            var inventoryMockSet = new Mock<DbSet<Character_Item>>()
                .SetupData(inventories, o =>
                {
                    return inventories.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            List<Spell_Character> knownSpells = new List<Spell_Character>();
            var knownSpellsMockSet = new Mock<DbSet<Spell_Character>>()
                .SetupData(knownSpells, o =>
                {
                    return knownSpells.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            //---------- Expected ----------
            
            CharacterDM expectedCharacterDM = new CharacterDM
            {
                Character_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                User_id = Guid.Parse("7d210de0-58f7-48ec-8065-4f4304725933"),
                Name = "Caleb Widowgast",
                Alignment = "Chaotic Good",
                ArmorClass = 14,
                Background = "blah",
                Exp = 0,
                Inspiration = false,
                MovementSpeed = 30,
                Race_id = Guid.Parse("14f91515-0107-4c79-a3da-be3cf48d7a26")
            };
            Currency expectedCurrency = new Currency
            {
                Character_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                GoldPieces = 3,
                SilverPieces = 50
            };
            Health expectedHealth = new Health
            {
                Character_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                MaxHP = 15,
                CurrentHP = 15,
                TempHP = 0,
                DeathSaveFails = 0,
                DeathSaveSuccesses = 0
            };
            IsProficient expectedSkills = new IsProficient
            {
                Character_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),

                StrengthSave = false,
                DexteritySave = false,
                ConstitutionSave = true,
                IntelligenceSave = true,
                WisdomSave = false,
                CharismaSave = false,

                Acrobatics = false,
                AnimalHandling = false,
                Arcana = true,
                Athletics = false,
                Deception = true,
                History = true,
                Intimidation = false,
                Investigation = true,
                Medicine = false,
                Nature = false,
                Perception = false,
                Performance = false,
                Persuasion = false,
                Religion = true,
                SleightOfHand = false,
                Stealth = false,
                Survival = true
            };
            List<Note> expectedNotes = new List<Note>();
            expectedNotes.Add(new Note
            {
                Character_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                Note_id = Guid.Parse("b5c49d62-737e-4357-903b-e17cb30daf07"),
                Name = "The Cat Prince",
                Contents = "A children's picture book. The text is in Zemnian"
            });

            Stats expectedStats = new Stats
            {
                Character_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                Strength = 10,
                Dexterity = 12,
                Constitution = 14,
                Intelligence = 20,
                Wisdom = 16,
                Charisma = 16
            };

            List<Character_Class_Subclass> expected_CCSC = new List<Character_Class_Subclass>();
            expected_CCSC.Add(new Character_Class_Subclass
            {
                Character_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                Class_id = Guid.Parse("4e82620a-0496-4ecc-b6d4-05faa064310b"),
                Subclass_id = Guid.Parse("c8d2e23a-a193-4e06-8814-9180d4830732"),
                RemainingHitDice = 12,
                ClassLevel = 12
            });

            List<Character_Item> expected_inventory = new List<Character_Item>();
            expected_inventory.Add(new Character_Item
            {
                //Has a healing potion
                Character_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                Item_id = Guid.Parse("2caa23dc-15e6-4a57-9bb6-62f6d8636ff7"),
                isEquipped = false,
                IsAttuned = false,
                count = 1
            });

            List<Spell_Character> expected_knownSpells = new List<Spell_Character>();
            expected_knownSpells.Add(new Spell_Character
            {
                //Caleb knows firebolt
                Character_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                Spell_id = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
                isPrepared = false
            });

            bool saveCalled = false;

            //---------- Argument ----------
            var Argument = buildPOSTArgument();



            using (var mocks = AutoMock.GetLoose())
            {

                mocks.Mock<CharacterContext>()
                    .Setup(x => x.Set<CharacterDM>()).Returns(characterMockSet.Object);
                mocks.Mock<CharacterContext>()
                    .Setup(x => x.Set<Currency>()).Returns(moneyMockSet.Object);
                mocks.Mock<CharacterContext>()
                    .Setup(x => x.Set<Health>()).Returns(healthMockSet.Object);
                mocks.Mock<CharacterContext>()
                    .Setup(x => x.Set<IsProficient>()).Returns(skillsMockSet.Object);
                mocks.Mock<CharacterContext>()
                    .Setup(x => x.Set<Note>()).Returns(notesMockSet.Object);
                mocks.Mock<CharacterContext>()
                    .Setup(x => x.Set<Stats>()).Returns(statsMockSet.Object);

                mocks.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<Character_Class_Subclass>()).Returns(ccscMockSet.Object);
                mocks.Mock<ItemsContext>()
                    .Setup(x => x.Set<Character_Item>()).Returns(inventoryMockSet.Object);
                mocks.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell_Character>()).Returns(knownSpellsMockSet.Object);

                mocks.Mock<IBaseUserAccess>()
                    .Setup(x => x.AddCharacter(It.IsAny<CharacterDM>()))
                        .Callback<CharacterDM>((characterRecord) => characters.Add(characterRecord));
                mocks.Mock<IBaseUserAccess>()
                    .Setup(x => x.AddCurrencyRecord(It.IsAny<Currency>()))
                    .Callback<Currency>((currency) => heldMoney.Add(currency));
                mocks.Mock<IBaseUserAccess>()
                    .Setup(x => x.AddHealthRecord(It.IsAny<Health>()))
                    .Callback<Health>((health) => healthRecords.Add(health));
                mocks.Mock<IBaseUserAccess>()
                    .Setup(x => x.AddProficiencyRecord(It.IsAny<IsProficient>()))
                    .Callback<IsProficient>((IsProficient) => skills.Add(IsProficient));
                mocks.Mock<IBaseUserAccess>()
                    .Setup(x => x.AddStatsRecord(It.IsAny<Stats>()))
                    .Callback<Stats>((stat) => stats.Add(stat));
                mocks.Mock<IBaseUserAccess>()
                    .Setup(x => x.AddNote(It.IsAny<Note>()))
                    .Callback<Note>((theNote) => notes.Add(theNote));

                mocks.Mock<IBaseUserAccess>()
                    .Setup(x => x.CharacterLearnsSpell(It.IsAny<Spell_Character>()))
                    .Callback<Spell_Character>((sc) => knownSpells.Add(sc));

                mocks.Mock<IBaseUserAccess>()
                    .Setup(x => x.CharacterObtainsItem(It.IsAny<Character_Item>()))
                    .Callback<Character_Item>((ci) => inventories.Add(ci));

                mocks.Mock<IBaseUserAccess>()
                    .Setup(x => x.CharacterLearnsClass(It.IsAny<Character_Class_Subclass>()))
                    .Callback<Character_Class_Subclass>((record) => CCSC_Records.Add(record));

                mocks.Mock<IBaseUserAccess>()
                    .Setup(x => x.SaveChanges())
                    .Callback( ()=> saveCalled = true);

                mocks.Mock<ICharacterCommonFunctions>()
                    .Setup(x => x.raceExists(Guid.Parse("14f91515-0107-4c79-a3da-be3cf48d7a26"))).Returns(true);

                //---------- act ----------
                var access = mocks.Create<IBaseUserAccess>();
                var commons = mocks.Create<ICharacterCommonFunctions>();
                var toTest = ProcessorFactory.getCreateCharacterProcessor(access, commons);
                toTest.CreateCharacterPOST(Guid.Parse("7d210de0-58f7-48ec-8065-4f4304725933"), Argument);

                //Assert
                characters.Should().ContainEquivalentOf(expectedCharacterDM, options =>
                    options.Excluding(x => x.Character_id));
                heldMoney.Should().ContainEquivalentOf(expectedCurrency, options =>
                    options.Excluding(x => x.Character_id));
                healthRecords.Should().ContainEquivalentOf(expectedHealth, options =>
                    options.Excluding(x => x.Character_id));
                stats.Should().ContainEquivalentOf(expectedStats, options =>
                    options.Excluding(x => x.Character_id));
                skills.Should().ContainEquivalentOf(expectedSkills, options =>
                    options.Excluding(x => x.Character_id));
                notes.Should().BeEquivalentTo(expectedNotes, options =>
                    options.Excluding(x => x.Note_id)
                    .Excluding(x => x.Character_id));
                saveCalled.Should().BeTrue();
                
                
            }

        }

        [Test]
        public void CreateCharacter_INVALID_ReferenceDataReset()
        {
            
            //Arrange
            var argument = buildPOSTArgument();            
            var Wizard_id = Guid.Parse("4e82620a-0496-4ecc-b6d4-05faa064310b");
            argument.PrimaryTab.Classes.SelectedClasses[0].SelectedClass_id = Wizard_id;

            var expected = argument;


            var selectableClasses = new List<ClassesListModel>();

            //set classes
            ReadModelMapper<PlayableClass, ClassesListModel> classesMapper = new ReadModelMapper<PlayableClass, ClassesListModel>();
            foreach(PlayableClass pc in CreateTestData.GetPlayableClasses())
            {
                ClassesListModel lm = classesMapper.mapDataModelToViewModel(pc);
                selectableClasses.Add(lm);
            }
            foreach(ClassRowCM cm in expected.PrimaryTab.Classes.SelectedClasses)
            {
                cm.playableClasses = selectableClasses.ToArray();

                //set subclasses
                List<SubclassesListModel> subclasses = new List<SubclassesListModel>();
                ReadModelMapper<Subclass, SubclassesListModel> subclassesMapper = new ReadModelMapper<Subclass, SubclassesListModel>();
                foreach(Subclass sc in CreateTestData.GetListOfSubclass().Where(x => x.Class_id == cm.SelectedClass_id))
                {
                    SubclassesListModel lm = subclassesMapper.mapDataModelToViewModel(sc);
                    subclasses.Add(lm);
                }
                cm.subclasses = subclasses.ToArray();
            }

            //set races
            ReadModelMapper<Race, RaceListModel> racesMapper = new ReadModelMapper<Race, RaceListModel>();
            List<RaceListModel> raceList = new List<RaceListModel>();
            foreach(Race r in CreateTestData.GetListOfRace())
            {
                RaceListModel lm = racesMapper.mapDataModelToViewModel(r);
            }

            //I need to obtain and replace the reference data for available races, classes, and subclasses.

            using (var mocks = AutoMock.GetLoose())
            {
                mocks.Mock<IBaseUserAccess>()
                    .Setup(x => x.GetAllRaces()).Returns(CreateTestData.GetListOfRace());
                mocks.Mock<IBaseUserAccess>()
                    .Setup(x => x.GetAllPlayableClasses()).Returns(CreateTestData.GetPlayableClasses());
                mocks.Mock<IBaseUserAccess>()
                    .Setup(x => x.GetAllSubclassesForClass(Wizard_id))
                    .Returns(CreateTestData.GetListOfSubclass().Where(x => x.Class_id == Wizard_id));

                mocks.Mock<ICharacterCommonFunctions>()
                    .Setup(x => x.itemExists(Guid.NewGuid())).Returns(false);

                
                var access = mocks.Create<IBaseUserAccess>();
                var commons = mocks.Create<ICharacterCommonFunctions>();

                //Act
                var toTest = ProcessorFactory.getCreateCharacterProcessor(access, commons);
                var actual = toTest.CreateCharacterINVALID(argument);

                actual.Should().BeEquivalentTo(expected);
            }
        }
        private CharacterVM buildPOSTArgument()
        {
            ClassesCM postedClasses = new ClassesCM
            {
                SelectedClasses = new ClassRowCM[1]
            };
            postedClasses.SelectedClasses[0] = new ClassRowCM
            {
                Index = 1,
                Level = 12,
                RemainingHitDice = 12,
                SelectedClass_id = Guid.Parse("4e82620a-0496-4ecc-b6d4-05faa064310b"),
                SelectedSubclass_id = Guid.Parse("c8d2e23a-a193-4e06-8814-9180d4830732")
            };
            PrimaryTabVM primaryTab = new PrimaryTabVM
            {
                Name = "Caleb Widowgast",
                Alignment = "Chaotic Good",
                Background = "blah",
                isInspired = false,
                Exp = 0,
                Race = Guid.Parse("14f91515-0107-4c79-a3da-be3cf48d7a26"),
                Classes = postedClasses,
                Stats = new StatsCM
                {
                    Strength = 10,
                    Dexterity = 12,
                    Constitution = 14,
                    Intelligence = 20,
                    Wisdom = 16,
                    Charisma = 16
                },
                Combat = new CombatCM
                {
                    MaxHP = 15,
                    CurrentHP = 15,
                    TempHP = 0,
                    DeathSaveFails = 0,
                    DeathSaveSuccesses = 0,
                    ArmorClass = 14,
                    MovementSpeed = 30
                },
                Skills = new ProficiencyCM
                {
                    isProficient = new IsProficientCM
                    {
                        Acrobatics = false,
                        AnimalHandling = false,
                        Arcana = true,
                        Athletics = false,
                        Deception = true,
                        History = true,
                        Intimidation = false,
                        Investigation = true,
                        Medicine = false,
                        Nature = false,
                        Perception = false,
                        Performance = false,
                        Persuasion = false,
                        Religion = true,
                        SleightOfHand = false,
                        Stealth = false,
                        Survival = true
                    }
                },
                Saves = new SavesCM
                {
                    StrengthSave = false,
                    DexteritySave = false,
                    ConstitutionSave = true,
                    IntelligenceSave = true,
                    WisdomSave = false,
                    CharismaSave = false,
                }

            };

            NoteTabVM noteTab = new NoteTabVM
            {
                Notes = new NoteCM[1]
            };
            noteTab.Notes[0] = new NoteCM
            {
                Name = "The Cat Prince",
                Contents = "A children's picture book. The text is in Zemnian"
            };

            InventoryTabVM inventoryTab = new InventoryTabVM
            {
                Money = new MoneyCM
                {
                    GoldPieces = 3,
                    SilverPieces = 50
                },
                Items = new HeldItemRowCM[1]
            };
            inventoryTab.Items[0] = new HeldItemRowCM
            {
                Item_id = Guid.Parse("2caa23dc-15e6-4a57-9bb6-62f6d8636ff7"),
                isAttuned = false,
                isEquipped = false,
                Count = 1
            };

            SpellsTabVM spellsTab = new SpellsTabVM
            {
                KnownSpells = new KnownSpellRowCM[1]
            };
            spellsTab.KnownSpells[0] = new KnownSpellRowCM
            {
                Spell_id = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
                isPrepared = true
            };

            CharacterVM Argument = new CharacterVM
            {
                PrimaryTab = primaryTab,
                NotesTab = noteTab,
                InventoryTab = inventoryTab,
                SpellsTab = spellsTab
            };

            return Argument;
        }
        
    }

   
}
