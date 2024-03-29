﻿using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Mapping.Implementations;
using DnDProject.Backend.Mapping.Implementations.Generic;
using DnDProject.Backend.Processors.Interfaces;
using DnDProject.Backend.Unit_Of_Work.Interfaces;
using DnDProject.Backend.UserAccess.Interfaces;
using DnDProject.Entities.Character.DataModels;
using DnDProject.Entities.Character.ViewModels.PartialViewModels.Components;
using DnDProject.Entities.Class.DataModels;
using DnDProject.Entities.Items.DataModels;
using DnDProject.Entities.Spells.DataModels;
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

namespace DnDProject.UnitTests.Processors
{
    [TestFixture]
    public class CMBuilderTests
    {
        [Test]
        public void CMBuilder_buildNewHeldItemRowCM_ValidCall()
        {
            List<Item> items = CreateTestData.GetListOfItems();
            var itemsMockSet = new Mock<DbSet<Item>>()
                .SetupData(items, o =>
                {
                    return items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });
            HeldItemRowCM expected = CharacterMapper.mapItemToHeldItemRowCM(CreateTestData.GetSampleItem());
            Item record = CreateTestData.GetSampleItem();
            expected.Count = 1;
            expected.isAttuned = false;
            expected.isEquipped = false;
            using (var mockContext = AutoMock.GetLoose())
            {

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Item>()).Returns(itemsMockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(itemsMockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //Act
                ICharacterCMBuilder toTest = ProcessorFactory.GetCharacterCMBuilder(access);
                var actual = toTest.buildNewHeldItemRowCM(record.Item_id);

                actual.Should().BeEquivalentTo(expected);

            }
        }
        [Test]
        public void CMBuilder_buildExistingHeldItemCM_ValidCall()
        {
            //Arrange
            List<Item> items = CreateTestData.GetListOfItems();
            var itemMockSet = new Mock<DbSet<Item>>()
                .SetupData(items, o =>
                {
                    return items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            List<Character_Item> heldItems = CreateTestData.GetListOfHeldItems();
            var heldItemsMockSet = new Mock<DbSet<Character_Item>>()
                .SetupData(heldItems, o =>
                {
                    return heldItems.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            Item item = CreateTestData.GetSampleItem();
            Character_Item heldItem = CreateTestData.GetSampleHeldItem();

            HeldItemRowCM expected = CharacterMapper.mapItemToHeldItemRowCM(item);
            CharacterMapper.mapHeldItemRecordToHeldItemRowCM(heldItem, expected);


            using (var mockContext = AutoMock.GetLoose())
            {

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Item>()).Returns(itemMockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(itemMockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Character_Item>()).Returns(heldItemsMockSet.Object);
                mockContext.Mock<ItemsContext>()
                   .Setup(x => x.HeldItems).Returns(heldItemsMockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //Act
                ICharacterCMBuilder toTest = ProcessorFactory.GetCharacterCMBuilder(access);
                var actual = toTest.buildExistingHeldItemRowCM(heldItem.Character_id, heldItem.Item_id);

                actual.Should().BeEquivalentTo(expected);

            }
        }
        [Test]
        public void CMBuilder_buildHeldItemRowCMsForCharacter_ValidCall()
        {
            List<Item> items = CreateTestData.GetListOfItems();
            var itemMockSet = new Mock<DbSet<Item>>()
                .SetupData(items, o =>
                {
                    return items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            List<Character_Item> heldItems = CreateTestData.GetListOfHeldItems();
            var heldItemsMockSet = new Mock<DbSet<Character_Item>>()
                .SetupData(heldItems, o =>
                {
                    return heldItems.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });
            List<HeldItemRowCM> expected = new List<HeldItemRowCM>();

            Item Whisper = CreateTestData.GetSampleItem();
            Character_Item VaxHoldsWhisper = CreateTestData.GetSampleHeldItem();
            HeldItemRowCM Vax_Whisper = CharacterMapper.mapItemToHeldItemRowCM(Whisper);
            CharacterMapper.mapHeldItemRecordToHeldItemRowCM(VaxHoldsWhisper, Vax_Whisper);
            Vax_Whisper.Index = 0;
            expected.Add(Vax_Whisper);

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
            Character_Item VaxHoldsPotions = new Character_Item
            {
                Character_id = Guid.Parse("e3a0faef-99da-4d15-bff1-b535a42b955c"),
                Item_id = Guid.Parse("2caa23dc-15e6-4a57-9bb6-62f6d8636ff7"),
                count = 3
            };
            HeldItemRowCM Vax_Potions = CharacterMapper.mapItemToHeldItemRowCM(HealingPotion);
            CharacterMapper.mapHeldItemRecordToHeldItemRowCM(VaxHoldsPotions, Vax_Potions);
            Vax_Potions.Index = 1;
            expected.Add(Vax_Potions);

            Guid Vax_id = Guid.Parse("e3a0faef-99da-4d15-bff1-b535a42b955c");


            using (var mockContext = AutoMock.GetLoose())
            {

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Item>()).Returns(itemMockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(itemMockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Character_Item>()).Returns(heldItemsMockSet.Object);
                mockContext.Mock<ItemsContext>()
                   .Setup(x => x.HeldItems).Returns(heldItemsMockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //Act
                ICharacterCMBuilder toTest = ProcessorFactory.GetCharacterCMBuilder(access);
                var actual = toTest.buildItemRowCMsForCharacter(Vax_id);

                actual.Should().BeEquivalentTo(expected);
            }
        }


        [Test]
        public void CMBuilder_buildKnownSpellRowCM_ValidCall()
        {
            //Arrange
            List<Spell> spells = CreateTestData.GetListOfSpells();
            var spellsMockSet = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });
            List<School> spellSchools = CreateTestData.GetListOfSchools();
            var schoolsMockSet = new Mock<DbSet<School>>()
                .SetupData(spellSchools, o =>
                {
                    return spellSchools.Single(x => x.School_id.CompareTo(o.First()) == 0);
                });

            Guid Tower_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f");
            var Tower = spells.Find(x => x.Spell_id == Tower_id);

            KnownSpellRowCM expected = CharacterMapper.mapSpellToKnownSpellRowCM(Tower);
            expected.School = "Conjuration";
            expected.Index = 0;


            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(spellsMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(spellsMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<School>()).Returns(schoolsMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Schools).Returns(schoolsMockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //Act
                ICharacterCMBuilder toTest = ProcessorFactory.GetCharacterCMBuilder(access);
                var actual = toTest.buildKnownSpellRowCM(Tower.Spell_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void CMBuilder_buildKnownSpellRowCMsForCharacter_ValidCall()
        {
            //Arrange
            List<Spell> spells = CreateTestData.GetListOfSpells();
            var spellsMockSet = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            List<Spell_Character> knownSpells = CreateTestData.GetListOfKnownSpells();
            var knownSpellsMockSet = new Mock<DbSet<Spell_Character>>()
                .SetupData(knownSpells, o =>
                {
                    return knownSpells.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            List<School> spellSchools = CreateTestData.GetListOfSchools();
            var schoolsMockSet = new Mock<DbSet<School>>()
                .SetupData(spellSchools, o =>
                {
                    return spellSchools.Single(x => x.School_id.CompareTo(o.First()) == 0);
                });


            //Caleb knows both his tower and Web of Fire, so we want to return CMs for both spells.
            Guid Caleb_id = Guid.Parse("11111111-2222-3333-4444-555555555555");

            List<KnownSpellRowCM> expected = new List<KnownSpellRowCM>();
            Guid Tower_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f");
            var Tower = spells.Find(x => x.Spell_id == Tower_id);
            KnownSpellRowCM TowerRow = CharacterMapper.mapSpellToKnownSpellRowCM(Tower);
            TowerRow.School = "Conjuration";
            TowerRow.Index = 0;
            expected.Add(TowerRow);

            Guid WoF_id = Guid.Parse("51b4c563-2040-4c7d-a23e-cab8d5d3c73b");
            var WebOfFire = spells.Find(x => x.Spell_id == WoF_id);
            KnownSpellRowCM WebOfFireRow = CharacterMapper.mapSpellToKnownSpellRowCM(WebOfFire);
            WebOfFireRow.School = "Evocation";
            WebOfFireRow.Index = 1;
            expected.Add(WebOfFireRow);




            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(spellsMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(spellsMockSet.Object);

                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.KnownSpells).Returns(knownSpellsMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell_Character>()).Returns(knownSpellsMockSet.Object);

                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<School>()).Returns(schoolsMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Schools).Returns(schoolsMockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //Act
                ICharacterCMBuilder toTest = ProcessorFactory.GetCharacterCMBuilder(access);
                var actual = toTest.buildKnownSpellRowCMsForCharacter(Caleb_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }

        }

        [Test]
        public void CMBuilder_buildNoteCMsForCharacter_ValidCall()
        {
            List<Note> notes = CreateTestData.GetListOfNotes();
            var mockSet = new Mock<DbSet<Note>>()
                .SetupData(notes, o =>
                {
                    return notes.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            Guid Grog_id = Guid.Parse("11111111-2222-3333-4444-555555555555");
            List<NoteCM> expected = new List<NoteCM>();


            Guid Spelling_id = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
            Note spellingNote = notes.Find(x => x.Note_id == Spelling_id);
            NoteCM spellingCM = CharacterMapper.mapNoteToNoteCM(spellingNote);
            spellingCM.Index = 0;
            expected.Add(spellingCM);

            Guid GreatAxe_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4");
            Note GreatAxeNote = notes.Find(x => x.Note_id == GreatAxe_id);
            NoteCM greatAxeCM = CharacterMapper.mapNoteToNoteCM(GreatAxeNote);
            greatAxeCM.Index = 1;
            expected.Add(greatAxeCM);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<Note>()).Returns(mockSet.Object);
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Notes).Returns(mockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //Act
                ICharacterCMBuilder toTest = ProcessorFactory.GetCharacterCMBuilder(access);
                var actual = toTest.buildNoteCMsFOrCharacter(Grog_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);

            }
        }

        [Test]
        public void CMBUilder_buildSpellDetailsCM_ValidCall()
        {
            //Arrange
            List<Spell> spells = CreateTestData.GetListOfSpells();
            var spellsMockSet = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });
            List<School> spellSchools = CreateTestData.GetListOfSchools();
            var schoolsMockSet = new Mock<DbSet<School>>()
                .SetupData(spellSchools, o =>
                {
                    return spellSchools.Single(x => x.School_id.CompareTo(o.First()) == 0);
                });
            List<Material> materials = CreateTestData.GetListOfMaterials();
            var materialsMockSet = new Mock<DbSet<Material>>()
                .SetupData(materials, o =>
                {
                    return materials.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });


            var VoltaicBolt_id = Guid.Parse("a9756f3d-55d0-40cd-8083-6b547e4932ab");

            //I expect to get the details CM for Brenatto's Voltaic Bolt
            var expected = new SpellDetailsCM
            {
                Spell_id = Guid.Parse("a9756f3d-55d0-40cd-8083-6b547e4932ab"),
                Name = "Brenatto's Voltaic Bolt",
                Description = "The caster's next ranged attack deals an additional 3d6 lightning damage",
                Level = 1,
                School = "Evocation",
                CastingTime = "1 Bonus Action",
                Duration = "1 round",
                Range = "30 feet",
                RequiresVerbal = false,
                RequiresSomantic = true,
                RequiresMaterial = true,
                RequiresConcentration = false,
                Material = "A bit of fleece"
            };
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(spellsMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(spellsMockSet.Object);

                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<School>()).Returns(schoolsMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Schools).Returns(schoolsMockSet.Object);

                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Materials).Returns(materialsMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Material>()).Returns(materialsMockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //Act
                ICharacterCMBuilder toTest = ProcessorFactory.GetCharacterCMBuilder(access);
                var actual = toTest.buildSpellDetailsCM(VoltaicBolt_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);

            }
        }
        [Test]
        public void CMBuilder_buildItemDetailsCM_ValidCall()
        {
            //Arrange
            List<Item> Items = CreateTestData.GetListOfItems();

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
            var itemsMockSet = new Mock<DbSet<Item>>()
                .SetupData(Items, o =>
                {
                    return Items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            Item Whisper = CreateTestData.GetSampleItem();
            ItemDetailsCM expected = new ItemDetailsCM
            {
                Item_id = Whisper.Item_id,
                Name = Whisper.Name,
                Description = Whisper.Description,
                Value = Whisper.Value,
                isEquippable = Whisper.isEquippable,
                isConsumable = Whisper.isConsumable,
                requiresAttunement = Whisper.requiresAttunement
            };
            String[] whisperTags = new string[2];
            whisperTags[0] = "Weapon";
            whisperTags[1] = "Wondorous Item";
            expected.Tags = whisperTags;

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Tags).Returns(tagsMockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Tag>()).Returns(tagsMockSet.Object);

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Item_Tags).Returns(ITmockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Item_Tag>()).Returns(ITmockSet.Object);

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(itemsMockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Item>()).Returns(itemsMockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //Act
                ICharacterCMBuilder toTest = ProcessorFactory.GetCharacterCMBuilder(access);
                var actual = toTest.buildItemDetailsCM(Whisper.Item_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);

            }
        }

        [Test]
        public void CMBuilder_buildNewClassRowCM()
        {

            //Arrange
            List<PlayableClass> playableClasses = CreateTestData.GetPlayableClasses();
            List<ClassesListModel> ClassesLM = new List<ClassesListModel>();
            foreach(PlayableClass playableClass in playableClasses)
            {
                ReadModelMapper<PlayableClass, ClassesListModel> mapper = new ReadModelMapper<PlayableClass, ClassesListModel>();
                ClassesListModel lm = mapper.mapDataModelToViewModel(playableClass);
                ClassesLM.Add(lm);
            }

            var expected = new ClassRowCM
            {
                Index = 5,
                Level = 1,
                RemainingHitDice = 1,
                playableClasses = ClassesLM.ToArray()

            };
            using (var mockAccess = AutoMock.GetLoose())
            {

                mockAccess.Mock<IBaseUserAccess>()
                    .Setup(x => x.GetAllPlayableClasses()).Returns(playableClasses);

                IBaseUserAccess access = mockAccess.Create<IBaseUserAccess>();
                //Act
                ICharacterCMBuilder toTest = ProcessorFactory.GetCharacterCMBuilder(access);
                var actual = toTest.buildNewClassRowCM(5);

                //Assert
                actual.Should().BeEquivalentTo(expected);

            }
        }
        [Test]
        public void CMBuilder_buildKnownClassRowCM()
        {

            //Arrange
            var Wizard_id = Guid.Parse("4e82620a-0496-4ecc-b6d4-05faa064310b");
            var Caleb_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4");
            var Transmutation_id = Guid.Parse("c8d2e23a-a193-4e06-8814-9180d4830732");

            List<PlayableClass> playableClasses = CreateTestData.GetPlayableClasses();
            List<ClassesListModel> ClassesLM = new List<ClassesListModel>();
            foreach (PlayableClass playableClass in playableClasses)
            {
                ReadModelMapper<PlayableClass, ClassesListModel> mapper = new ReadModelMapper<PlayableClass, ClassesListModel>();
                ClassesListModel lm = mapper.mapDataModelToViewModel(playableClass);
                ClassesLM.Add(lm);
            }

            List<Subclass> playableSubclasses = CreateTestData.GetListOfSubclass();
            List<SubclassesListModel> SubclassesLM = new List<SubclassesListModel>();
            foreach(Subclass subclass in playableSubclasses.Where(x => x.Class_id == Wizard_id))
            {
                ReadModelMapper<Subclass, SubclassesListModel> mapper = new ReadModelMapper<Subclass, SubclassesListModel>();
                SubclassesListModel lm = mapper.mapDataModelToViewModel(subclass);
                SubclassesLM.Add(lm);
            }

            Character_Class_Subclass Caleb_Wizard_Transmutation = new Character_Class_Subclass
            {
                Character_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                Class_id = Guid.Parse("4e82620a-0496-4ecc-b6d4-05faa064310b"),
                Subclass_id = Guid.Parse("c8d2e23a-a193-4e06-8814-9180d4830732"),
                RemainingHitDice = 12,
                ClassLevel = 12
            };

            ClassRowCM expected = new ClassRowCM
            {
                Index = 1,
                playableClasses = ClassesLM.ToArray(),
                SelectedClass_id = Wizard_id,

                Level = 12,
                RemainingHitDice = 12,

                subclasses = SubclassesLM.ToArray(),
                SelectedSubclass_id = Transmutation_id
            };

            using(var mockAccess = AutoMock.GetLoose())
            {
                mockAccess.Mock<IBaseUserAccess>()
                    .Setup(x => x.GetKnownClassRecordOfCharaterAndClass(Caleb_id, Wizard_id))
                    .Returns(Caleb_Wizard_Transmutation);
                mockAccess.Mock<IBaseUserAccess>()
                    .Setup(x => x.GetAllSubclassesForClass(Wizard_id))
                    .Returns(playableSubclasses.Where(x => x.Class_id == Wizard_id));

                IBaseUserAccess access = mockAccess.Create<IBaseUserAccess>();
                //Act
                ICharacterCMBuilder toTest = ProcessorFactory.GetCharacterCMBuilder(access);
                var actual = toTest.buildKnownClassRowCM(1, Caleb_Wizard_Transmutation, ClassesLM.ToArray());

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void CMBuilder_buildStatsCM()
        {
            //Arrange
            var expected = new StatsCM
            {
                Strength = 10,
                Dexterity = 12,
                Constitution = 16,
                Intelligence = 20,
                Wisdom = 16,
                Charisma = 16
            };
            var CalebStats = new Stats()
            {
                Character_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                Strength = 10,
                Dexterity = 12,
                Constitution = 16,
                Intelligence = 20,
                Wisdom = 16,
                Charisma = 16
            };
            var Caleb_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4");
            using (var mockAccess = AutoMock.GetLoose())
            {
                mockAccess.Mock<IBaseUserAccess>()
                    .Setup(x => x.GetStatsRecord(Caleb_id)).Returns(CalebStats);


                //Act
                var access = mockAccess.Create<IBaseUserAccess>();
                ICharacterCMBuilder toTest = ProcessorFactory.GetCharacterCMBuilder(access);
                var actual = toTest.buildStatsCM(Caleb_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void CMBuilter_buildStatBonusCM()
        {
            //Arrange
            var expected = new StatBonusCM
            {
                Strength = 0,
                Dexterity = 1,
                Constitution = 3,
                Intelligence = 5,
                Wisdom = -3,
                Charisma = 3
            };
            var CalebStatsCM = new StatsCM
            {
                Strength = 10,
                Dexterity = 12,
                Constitution = 16,
                Intelligence = 20,
                Wisdom = 5,
                Charisma = 16
            };
            var CalebStats = new Stats()
            {
                Character_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                Strength = 10,
                Dexterity = 12,
                Constitution = 16,
                Intelligence = 20,
                Wisdom = 5,
                Charisma = 16
            };

            using (var mockAccess = AutoMock.GetLoose())
            {
                var Caleb_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4");
                mockAccess.Mock<IBaseUserAccess>()
                    .Setup(x => x.GetStatsRecord(Caleb_id)).Returns(CalebStats);
                //Act
                var access = mockAccess.Create<IBaseUserAccess>();
                ICharacterCMBuilder toTest = ProcessorFactory.GetCharacterCMBuilder(access);
                var actual = toTest.buildStatBonusCM(CalebStatsCM);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void CMBuilder_buildProficiencyCM()
        {
            var skillBonus = new SkillBonusCM
            {
                Acrobatics = 1,
                AnimalHandling = -3,
                Arcana = 8,
                Athletics = 0,
                Deception = 6,

                History = 8,
                Insight = -3,
                Intimidation = 3,
                Investigation = 8,
                Medicine = -3,
                Nature = 5,

                Perception = -3,
                Performance = 3,
                Persuasion = 3,
                Religion = 8,
                SleightOfHand = 1,

                Stealth = 1,
                Survival = -3
            };
            var statBonus = new StatBonusCM
            {
                Strength = 0,
                Dexterity = 1,
                Constitution = 3,
                Intelligence = 5,
                Wisdom = -3,
                Charisma = 3
            };
            var proficiencies = new IsProficientCM
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
                Survival = false
            };
            var expected = new ProficiencyCM
            {
                ProficiencyBonus = 3,
                TotalBonus = skillBonus,
                isProficient = proficiencies
            };

            var proficiencyRecord = new IsProficient
            {
                Character_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                StrengthSave = false,
                DexteritySave = false,
                ConstitutionSave = true,
                IntelligenceSave = false,
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
                Survival = false
            };
            var Caleb_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4");
            using (var mockAccess = AutoMock.GetLoose())
            {
                mockAccess.Mock<IBaseUserAccess>()
                    .Setup(x => x.GetProficiencyRecord(Caleb_id)).Returns(proficiencyRecord);

                //Act
                var access = mockAccess.Create<IBaseUserAccess>();
                ICharacterCMBuilder toTest = ProcessorFactory.GetCharacterCMBuilder(access);
                var actual = toTest.buildProficiencyCM(Caleb_id, statBonus, 12);

                //Assert
                actual.Should().BeEquivalentTo(expected);

            }
        }

        [Test]
        public void CMBUilder_buildIsProficientCM()
        {
            Guid Caleb_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4");
            IsProficient CalebRecord = new IsProficient
            {
                Character_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                StrengthSave = false,
                DexteritySave = false,
                ConstitutionSave = true,
                IntelligenceSave = false,
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
                Survival = false
            };
            IsProficientCM Expected = new IsProficientCM
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
                Survival = false
            };

            using (var mockAccess = AutoMock.GetLoose())
            {
                mockAccess.Mock<IBaseUserAccess>()
                    .Setup(x => x.GetProficiencyRecord(Caleb_id)).Returns(CalebRecord);

                //Act
                var access = mockAccess.Create<IBaseUserAccess>();
                ICharacterCMBuilder toTest = ProcessorFactory.GetCharacterCMBuilder(access);
                var actual = toTest.buildIsProficientCM(Caleb_id);

                //Assert
                actual.Should().BeEquivalentTo(Expected);

            }
        }

        [Test]
        public void CMBUilder_buildSkillBonusCM()
        {
            //Arrange
            var statBonus = new StatBonusCM
            {
                Strength = 0,
                Dexterity = 1,
                Constitution = 3,
                Intelligence = 5,
                Wisdom = -3,
                Charisma = 3
            };
            var proficiencies = new IsProficientCM
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
                Survival = false
            };
            int totalLevel = 12;
            var expected = new SkillBonusCM
            {
                Acrobatics = 1,
                AnimalHandling = -3,
                Arcana = 8,
                Athletics = 0,
                Deception = 6,

                History = 8,
                Insight = -3,
                Intimidation = 3,
                Investigation = 8,
                Medicine = -3,
                Nature = 5,

                Perception = -3,
                Performance = 3,
                Persuasion = 3,
                Religion = 8,
                SleightOfHand = 1,

                Stealth = 1,
                Survival = -3
            };
            var CalebStats = new Stats()
            {
                Character_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                Strength = 10,
                Dexterity = 12,
                Constitution = 16,
                Intelligence = 20,
                Wisdom = 5,
                Charisma = 16
            };
            using (var mockAccess = AutoMock.GetLoose())
            {
                var Caleb_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4");
                mockAccess.Mock<IBaseUserAccess>()
                    .Setup(x => x.GetStatsRecord(Caleb_id)).Returns(CalebStats);

                //Act
                //Act
                var access = mockAccess.Create<IBaseUserAccess>();
                ICharacterCMBuilder toTest = ProcessorFactory.GetCharacterCMBuilder(access);
                var actual = toTest.buildSkillBonusCM(statBonus, totalLevel, proficiencies);

                //Assert
                actual.Should().BeEquivalentTo(expected);

            }

        }
    }
}
