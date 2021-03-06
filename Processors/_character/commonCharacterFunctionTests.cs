﻿using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Processors.Implementations;
using DnDProject.Backend.Processors.Interfaces;
using DnDProject.Backend.Unit_Of_Work.Implementations;
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
    public class commonCharacterFunctionTests
    {
        //Create
        [Test]
        public void CharacterCommons_CharacterObtainsItem_ValidCall()
        {
            //Arrange
            List<Character_Item> heldItems = new List<Character_Item>();
            var mockSet = new Mock<DbSet<Character_Item>>()
                .SetupData(heldItems, o =>
                {
                    return heldItems.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            Character_Item expected = new Character_Item
            {
                Character_id = CreateTestData.getSampleCharacter().Character_id,
                Item_id = CreateTestData.GetSampleItem().Item_id,
                count = 1
            };
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.HeldItems).Returns(mockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Character_Item>()).Returns(mockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //act
                ICharacterCommonFunctions toTest = ProcessorFactory.GetCharacterCommonFunctions(access);
                toTest.addHeldItemToDb(expected.Character_id, expected.Item_id);

                //Assert
                heldItems.Should().ContainEquivalentOf(expected);

            }
        }
        [Test]
        public void CharacterCommons_CharacterLearnsClassSubclass_ValidCall()
        {
            List<Character_Class_Subclass> knownClasses = new List<Character_Class_Subclass>();
            var mockSet = new Mock<DbSet<Character_Class_Subclass>>()
                .SetupData(knownClasses, o =>
                {
                    return knownClasses.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            Character_Class_Subclass expected = CreateTestData.GetCharacter_Class_Subclass();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.KnownClasses).Returns(mockSet.Object);
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<Character_Class_Subclass>()).Returns(mockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //act
                ICharacterCommonFunctions toTest = ProcessorFactory.GetCharacterCommonFunctions(access);
                toTest.characterLearnsClass(expected);

                //Assert
                knownClasses.Should().ContainEquivalentOf(expected);
            }
        }
        [Test]
        public void CharacterCommons_AddNoteToDb_ValidCall()
        {
            //Arrange
            List<Note> notes = new List<Note>();
            var mockSet = new Mock<DbSet<Note>>()
                .SetupData(notes, o =>
                {
                    return notes.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            Note expected = CreateTestData.GetSampleNote();

            using( var mockContext = AutoMock.GetLoose()){
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Notes).Returns(mockSet.Object);
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<Note>()).Returns(mockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //act
                ICharacterCommonFunctions toTest = ProcessorFactory.GetCharacterCommonFunctions(access);
                toTest.addNote(expected);

                notes.Should().ContainEquivalentOf(expected);

            }

        }
        [Test]
        public void CharacterCommons_CharacterLearnsSpell_ValidCall()
        {
            //Arrange
            List<Spell_Character> knownSpells = new List<Spell_Character>();
            var mockSet = new Mock<DbSet<Spell_Character>>()
                .SetupData(knownSpells, o =>
                {
                    return knownSpells.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            Spell_Character expected = CreateTestData.GetSampleKnownSpell();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.KnownSpells).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell_Character>()).Returns(mockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //act
                ICharacterCommonFunctions toTest = ProcessorFactory.GetCharacterCommonFunctions(access);
                toTest.characterLearnsSpell(expected.Character_id, expected.Spell_id);

                //Assert
                knownSpells.Should().ContainEquivalentOf(expected);
            }
        }




        //READ
        [Test]
        public void CharacterCommons_spellExists_returnTrue()
        {
            //Arrange
            List<Spell> listOfSpells = CreateTestData.GetListOfSpells();
            var mockSet = new Mock<DbSet<Spell>>()
                .SetupData(listOfSpells, o =>
                {
                    return listOfSpells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });
            var realSpell = CreateTestData.GetSampleSpell();
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(mockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //act
                ICharacterCommonFunctions toTest = ProcessorFactory.GetCharacterCommonFunctions(access);
                var actual = toTest.spellExists(realSpell.Spell_id);

                //Assert
                actual.Should().BeTrue();
            }
        }
        [Test]
        public void CharacterCommons_spellExists_returnFalse()
        {
            //Arrange
            List<Spell> listOfSpells = CreateTestData.GetListOfSpells();
            var mockSet = new Mock<DbSet<Spell>>()
                .SetupData(listOfSpells, o =>
                {
                    return listOfSpells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });
            var false_id = Guid.Parse("720f467c-7621-4dcf-a82f-7af50f253068");
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(mockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //act
                ICharacterCommonFunctions toTest = ProcessorFactory.GetCharacterCommonFunctions(access);
                Action act = () => toTest.spellExists(false_id);

                //Assert
                act.Should().Throw<InvalidOperationException>().WithMessage("Sequence contains no matching element");
            }
        }

        [Test]
        public void CharacterCommons_playableClassExists_returnTrue()
        {
            List<PlayableClass> playableClasses = CreateTestData.GetPlayableClasses();
            var mockSet = new Mock<DbSet<PlayableClass>>()
                .SetupData(playableClasses, o =>
                {
                    return playableClasses.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var realClass = CreateTestData.GetSampleClass();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Classes).Returns(mockSet.Object);
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<PlayableClass>()).Returns(mockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //act
                ICharacterCommonFunctions toTest = ProcessorFactory.GetCharacterCommonFunctions(access);
                var actual = toTest.playableClassExists(realClass.Class_id);

                //Assert
                actual.Should().BeTrue();
            }
        }
        [Test]
        public void CharacterCommons_playableClassExists_returnFalse()
        {
            List<PlayableClass> playableClasses = CreateTestData.GetPlayableClasses();
            var mockSet = new Mock<DbSet<PlayableClass>>()
                .SetupData(playableClasses, o =>
                {
                    return playableClasses.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var false_id = Guid.Parse("720f467c-7621-4dcf-a82f-7af50f253068");

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Classes).Returns(mockSet.Object);
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<PlayableClass>()).Returns(mockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //act
                ICharacterCommonFunctions toTest = ProcessorFactory.GetCharacterCommonFunctions(access);
                Action act = () => toTest.playableClassExists(false_id);

                //Assert
                act.Should().Throw<InvalidOperationException>().WithMessage("Sequence contains no matching element");
            }
        }
        [Test]
        public void CharacterCommons_subclassExists_returnTrue()
        {
            //Arrange
            List<Subclass> subclasses = CreateTestData.GetListOfSubclass();
            var mockSet = new Mock<DbSet<Subclass>>()
                .SetupData(subclasses, o =>
                {
                    return subclasses.Single(x => x.Subclass_id.CompareTo(o.First()) == 0);
                });
            var realSubclass = CreateTestData.GetSubclass();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Subclasses).Returns(mockSet.Object);
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<Subclass>()).Returns(mockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //act
                ICharacterCommonFunctions toTest = ProcessorFactory.GetCharacterCommonFunctions(access);
                var actual = toTest.subclassExists(realSubclass.Subclass_id);

                //Assert
                actual.Should().BeTrue();
            }
        }
        [Test]
        public void Charactercommons_subclassExists_returnFalse()
        {
            //Arrange
            List<Subclass> subclasses = CreateTestData.GetListOfSubclass();
            var mockSet = new Mock<DbSet<Subclass>>()
                .SetupData(subclasses, o =>
                {
                    return subclasses.Single(x => x.Subclass_id.CompareTo(o.First()) == 0);
                });
            var false_id = Guid.Parse("720f467c-7621-4dcf-a82f-7af50f253068");

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Subclasses).Returns(mockSet.Object);
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<Subclass>()).Returns(mockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //act
                ICharacterCommonFunctions toTest = ProcessorFactory.GetCharacterCommonFunctions(access);
                Action act = () => toTest.subclassExists(false_id);

                //Assert
                act.Should().Throw<InvalidOperationException>().WithMessage("Sequence contains no matching element");
            }
        }

        [Test]
        public void CharacterCommons_itemExists_returnTrue()
        {
            //Arrange
            List<Item> items = CreateTestData.GetListOfItems();
            var mockSet = new Mock<DbSet<Item>>()
                .SetupData(items, o =>
                {
                    return items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            var realItem = CreateTestData.GetSampleItem();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(mockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Item>()).Returns(mockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //Act
                ICharacterCommonFunctions toTest = ProcessorFactory.GetCharacterCommonFunctions(access);
                var actual = toTest.itemExists(realItem.Item_id);

                actual.Should().BeTrue();
            }
        }
        [Test]
        public void CharacterCommons_itemExists_returnFalse()
        {
            //Arrange
            List<Item> items = CreateTestData.GetListOfItems();
            var mockSet = new Mock<DbSet<Item>>()
                .SetupData(items, o =>
                {
                    return items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            var false_id = Guid.Parse("720f467c-7621-4dcf-a82f-7af50f253068");

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(mockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Item>()).Returns(mockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //Act
                ICharacterCommonFunctions toTest = ProcessorFactory.GetCharacterCommonFunctions(access);
                Action act = () => toTest.itemExists(false_id);

                //Assert
                act.Should().Throw<InvalidOperationException>().WithMessage("Sequence contains no matching element");

            }
        }
    
        [Test]
        public void CharacterCommons_spellCanBeCastByClass_returnTrue()
        {
            List<Spell_Class> spell_Classes = CreateTestData.GetListOfCastableByRecords();
            List<Spell> spells = CreateTestData.GetListOfSpells();
            var spellMockSet = new Mock<DbSet<Spell>>()
               .SetupData(spells, o =>
               {
                   return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
               });
            var castableByMockSet = new Mock<DbSet<Spell_Class>>()
                .SetupData(spell_Classes, o =>
                {
                    return spell_Classes.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });
            var realSpell_Class = CreateTestData.GetSampleCastableBy();
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(spellMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(spellMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.CastableByRecords).Returns(castableByMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell_Class>()).Returns(castableByMockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //act
                ICharacterCommonFunctions toTest = ProcessorFactory.GetCharacterCommonFunctions(access);
                var actual = toTest.spellCanBeCastByClass(realSpell_Class.Spell_id, realSpell_Class.Class_id);

                //Assert
                actual.Should().BeTrue();

            }
        }
        [Test]
        public void CharacterCommons_spellCanBeCastByClass_returnFalse()
        {
            List<Spell_Class> spell_Classes = CreateTestData.GetListOfCastableByRecords();
            List<Spell> spells = CreateTestData.GetListOfSpells();
            var spellMockSet = new Mock<DbSet<Spell>>()
               .SetupData(spells, o =>
               {
                   return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
               });
            var castableByMockSet = new Mock<DbSet<Spell_Class>>()
                .SetupData(spell_Classes, o =>
                {
                    return spell_Classes.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });
            var realSpellID = CreateTestData.GetSampleSpell().Spell_id;
            var falseClassID = Guid.Parse("16310468-2345-460f-a408-d9f7c908ad2a");
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(spellMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(spellMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.CastableByRecords).Returns(castableByMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell_Class>()).Returns(castableByMockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //act
                ICharacterCommonFunctions toTest = ProcessorFactory.GetCharacterCommonFunctions(access);
                var actual = toTest.spellCanBeCastByClass(realSpellID, falseClassID);

                //Assert
                actual.Should().BeFalse();
            }
        }



        [Test]
        public void CharacterCommons_RemoveNonExistantSpellFromKnownSpells_ValidCall()
        {
            //Arrange
            List<KnownSpellRowCM> KnownSpellRowCMs = new List<KnownSpellRowCM>();
            List<KnownSpellRowCM> expected = new List<KnownSpellRowCM>();

            KnownSpellRowCM realSpell = new KnownSpellRowCM()
            {
                Spell_id = CreateTestData.GetSampleSpell().Spell_id
            };
            expected.Add(realSpell);
            KnownSpellRowCMs.Add(realSpell);
            KnownSpellRowCM fakeSpell = new KnownSpellRowCM()
            {
                Spell_id = Guid.Parse("96bd962c-5283-4f28-8a39-e82dbe01ff1a")
            };
            KnownSpellRowCMs.Add(fakeSpell);
            KnownSpellRowCM[] knownSpellArray = KnownSpellRowCMs.ToArray();

            using (var mockContext = AutoMock.GetLoose())
            {
                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //act
                ICharacterCommonFunctions toTest = ProcessorFactory.GetCharacterCommonFunctions(access);
                var actual = toTest.removeNonExistantSpellCMFromKnownSpells(knownSpellArray, fakeSpell.Spell_id);

                //Assert
                actual.Should().BeEquivalentTo(expected.ToArray());
            }
        }
        [Test]
        public void CharacterCommons_RemoveNonExistantClassIDFromSelectedClasses_ValidCall()
        {
            //Arrange
            Guid[] selectedClasses = new Guid[2];
            Guid[] expected = new Guid[1];
            var falseClass_id = Guid.Parse("96bd962c-5283-4f28-8a39-e82dbe01ff1a");

            selectedClasses[0] = CreateTestData.GetSampleClass().Class_id;
            expected[0] = CreateTestData.GetSampleClass().Class_id;
            selectedClasses[1] = falseClass_id;

            using (var mockContext = AutoMock.GetLoose())
            {
                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //act
                ICharacterCommonFunctions toTest = ProcessorFactory.GetCharacterCommonFunctions(access);
                var actual = toTest.removeNonExistantClassIdFromSelectedClasses(selectedClasses, falseClass_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);
                actual.Should().NotBeEquivalentTo(selectedClasses);
            }
        }
        [Test]
        public void CharacterCommons_RemoveNonExistantItemFromHeldItems_ValidCall()
        {
            HeldItemRowCM[] heldItems = new HeldItemRowCM[2];
            HeldItemRowCM[] expected = new HeldItemRowCM[1];
            HeldItemRowCM realItem = new HeldItemRowCM
            {
                Item_id = CreateTestData.GetSampleItem().Item_id
            };
            heldItems[0] = realItem;
            expected[0] = realItem;
            Guid false_id = Guid.Parse("96bd962c-5283-4f28-8a39-e82dbe01ff1a");
            HeldItemRowCM fakeItem = new HeldItemRowCM
            {
                Item_id = false_id
            };
            heldItems[1] = fakeItem;

            using (var mockContext = AutoMock.GetLoose())
            {
                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);

                //act
                ICharacterCommonFunctions toTest = ProcessorFactory.GetCharacterCommonFunctions(access);
                var actual = toTest.removeNonExistantItemFromHeldItems(heldItems, false_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);
                actual.Should().NotBeEquivalentTo(heldItems);

            }
        }
    }
}
