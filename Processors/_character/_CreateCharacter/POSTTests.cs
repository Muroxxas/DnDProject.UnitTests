using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Mapping.Implementations;
using DnDProject.Backend.Processors.Interfaces;
using DnDProject.Backend.Unit_Of_Work.Interfaces;
using DnDProject.Backend.UserAccess.Interfaces;
using DnDProject.Entities.Character.DataModels;
using DnDProject.Entities.Character.ViewModels;
using DnDProject.Entities.Character.ViewModels.PartialViewModels;
using DnDProject.Entities.Character.ViewModels.PartialViewModels.Components;
using DnDProject.Entities.Spells.DataModels;
using DnDProject.UnitTests.Processors;
using DnDProject.UnitTests.Unit_Of_Work;
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

namespace DnDProject.UnitTests.processors._character._CreateCharacter
{
    [TestFixture]
    public class POSTTests
    {

        [Test]
        public void CreateCharacterPOST_AllOfCharacterVMPosted()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void CreateCharacterPOST_CharacterDataModelPosted()
        {
            //Arrange
            List<CharacterDM> listOfCharacter = new List<CharacterDM>();
            var mockSet = new Mock<DbSet<CharacterDM>>()
                .SetupData(listOfCharacter, o =>
                {
                    return listOfCharacter.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            CharacterVM vm = new CharacterVM();
            var expected = CharacterMapper.mapCharacterVMToNewEntity(vm);
            Guid user_id = Guid.Parse("41f0d48d-8113-46c4-a059-a3595c538551");
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Characters).Returns(mockSet.Object);
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<CharacterDM>()).Returns(mockSet.Object);                

                //Act
                ICreateCharacter toTest = buildProcessor(mockContext);
                toTest.CreateCharacterPOST(user_id, vm);

                //Assert
                listOfCharacter.Should().ContainEquivalentOf(expected, options =>
                    options.Excluding(x => x.Character_id));
                listOfCharacter.Count.Should().Be(1);                
            }
        }

        [Test]
        public void CreateCharacterPOST_CharacterHealthRecordPosted()
        {
            //Arrange
            List<CharacterDM> listOfCharacter = new List<CharacterDM>();
            var characterMockSet = new Mock<DbSet<CharacterDM>>()
                .SetupData(listOfCharacter, o =>
                {
                    return listOfCharacter.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            List<Health> listOfHealthRecord = new List<Health>();
            var mockSet = new Mock<DbSet<Health>>()
                .SetupData(listOfHealthRecord, o =>
                {
                    return listOfHealthRecord.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            CombatCM vm = new CombatCM
            {
                MaxHP = 50,
                CurrentHP = 27,
                MovementSpeed = 25
            };

            CharacterVM CharVM = new CharacterVM();
            CharVM.PrimaryTab = new PrimaryTabVM();
            CharVM.PrimaryTab.Combat = vm;
            Guid user_id = Guid.Parse("41f0d48d-8113-46c4-a059-a3595c538551");

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Characters).Returns(characterMockSet.Object);
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<CharacterDM>()).Returns(characterMockSet.Object);

                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.HealthRecords).Returns(mockSet.Object);
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<Health>()).Returns(mockSet.Object);

                //Act
                ICreateCharacter toTest = buildProcessor(mockContext);
                toTest.CreateCharacterPOST(user_id, CharVM);

                //Assert
                listOfHealthRecord.First().MaxHP.Should().Be(vm.MaxHP);
                listOfHealthRecord.First().CurrentHP.Should().Be(vm.CurrentHP);

            }
        }
        [Test]
        public void CreateCharacterPOST_CharacterStatsPosted()
        {
            //Arrange
            //Character
            List<CharacterDM> listOfCharacter = new List<CharacterDM>();
            var characterMockSet = new Mock<DbSet<CharacterDM>>()
                .SetupData(listOfCharacter, o =>
                {
                    return listOfCharacter.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            //Health
            List<Health> listOfHealthRecord = new List<Health>();
            var healthMockSet = new Mock<DbSet<Health>>()
                .SetupData(listOfHealthRecord, o =>
                {
                    return listOfHealthRecord.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            //Stats
            List<Stats> listOfStatsRecords = new List<Stats>();
            var statsMockSet = new Mock<DbSet<Stats>>()
                .SetupData(listOfStatsRecords, o =>
                {
                    return listOfStatsRecords.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            StatsCM cm = new StatsCM();
            cm.Strength = 14;

            CharacterVM CharVM = new CharacterVM();
            CharVM.PrimaryTab = new PrimaryTabVM();
            CharVM.PrimaryTab.Stats = cm;
            Guid user_id = Guid.Parse("41f0d48d-8113-46c4-a059-a3595c538551");


            using (var mockContext = AutoMock.GetLoose())
            {
                //Character
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Characters).Returns(characterMockSet.Object);
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<CharacterDM>()).Returns(characterMockSet.Object);

                //Health
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.HealthRecords).Returns(healthMockSet.Object);
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<Health>()).Returns(healthMockSet.Object);

                //Stats
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.StatsRecords).Returns(statsMockSet.Object);
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<Stats>()).Returns(statsMockSet.Object);

                //---Act
                ICreateCharacter toTest = buildProcessor(mockContext);
                toTest.CreateCharacterPOST(user_id, CharVM);

                //assert
                listOfStatsRecords.First().Strength.Should().Be(cm.Strength);
            }
        }
        [Test]
        public void CreateCharacterPOST_CharacterKnownSpellsPosted()
        {
            //Arrange
            //Character
            List<CharacterDM> listOfCharacter = new List<CharacterDM>();
            var characterMockSet = new Mock<DbSet<CharacterDM>>()
                .SetupData(listOfCharacter, o =>
                {
                    return listOfCharacter.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            //Health
            List<Health> listOfHealthRecord = new List<Health>();
            var healthMockSet = new Mock<DbSet<Health>>()
                .SetupData(listOfHealthRecord, o =>
                {
                    return listOfHealthRecord.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            //Stats
            List<Stats> listOfStatsRecords = new List<Stats>();
            var statsMockSet = new Mock<DbSet<Stats>>()
                .SetupData(listOfStatsRecords, o =>
                {
                    return listOfStatsRecords.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            //Spells + Spell_Character+_SpellClass
            List<Spell> listOfSpells = CreateTestData.GetListOfSpells();
            List<Spell_Class> listofSpellClass = CreateTestData.GetListOfCastableByRecords();
            List<Spell_Character> listOfKnownSpells = new List<Spell_Character>();
            var spellsMockSet = new Mock<DbSet<Spell>>()
                .SetupData(listOfSpells, o =>
                {
                    return listOfSpells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });
            var spellClassMockSet = new Mock<DbSet<Spell_Class>>()
                .SetupData(listofSpellClass, o =>
                {
                    return listofSpellClass.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });
            var knownSpellsMockSet = new Mock<DbSet<Spell_Character>>()
                .SetupData(listOfKnownSpells, o =>
                {
                    return listOfKnownSpells.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });


            CharacterVM CharVM = new CharacterVM();
            SpellsTabVM spellsTab = new SpellsTabVM();
            spellsTab.KnownSpells = new KnownSpellRowCM[1];
            KnownSpellRowCM knownSpellCM = new KnownSpellRowCM()
            {
                Spell_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f")
            };
            spellsTab.KnownSpells[0] = knownSpellCM;
            CharVM.SpellsTab = spellsTab;
            Guid user_id = Guid.Parse("41f0d48d-8113-46c4-a059-a3595c538551");


            using (var mockContext = AutoMock.GetLoose())
            {
                //Character
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Characters).Returns(characterMockSet.Object);
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<CharacterDM>()).Returns(characterMockSet.Object);

                //Health
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.HealthRecords).Returns(healthMockSet.Object);
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<Health>()).Returns(healthMockSet.Object);

                //Stats
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.StatsRecords).Returns(statsMockSet.Object);
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<Stats>()).Returns(statsMockSet.Object);

                //Spells, KnownSpells, SpellClass
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(spellsMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(spellsMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.KnownSpells).Returns(knownSpellsMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell_Character>()).Returns(knownSpellsMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.CastableByRecords).Returns(spellClassMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell_Class>()).Returns(spellClassMockSet.Object);


                //---Act
                ICreateCharacter toTest = buildProcessor(mockContext);
                toTest.CreateCharacterPOST(user_id, CharVM);

                //assert
                listOfKnownSpells.First().Spell_id.Should().Be(knownSpellCM.Spell_id);
            }
        }
        [Test]
        public void CreateCharacterPOST_CharacterCombatRecordPosted()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void CreateCharacterPOST_CharacterMoneyRecordPosted()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void CreateCharacterPOST_CharacterHeldItemsPosted()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void CreateCharacterPOST_CharacterNotesPosted()
        {
            throw new NotImplementedException();
        }

        private static ICreateCharacter buildProcessor(AutoMock mockContext)
        {
            IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
            IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);
            ICreateCharacter processor = ProcessorFactory.getCreateCharacterProcessor(access);
            return processor;
        }


    }
}
