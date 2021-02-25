using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Mapping.Implementations;
using DnDProject.Backend.Services.Implementations;
using DnDProject.Backend.Services.Interfaces;
using DnDProject.Backend.Unit_Of_Work.Interfaces;
using DnDProject.Backend.UserAccess.Interfaces;
using DnDProject.Entities.Character.ViewModels.PartialViewModels.Components;
using DnDProject.Entities.Class.DataModels;
using DnDProject.Entities.Items.DataModels;
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

namespace DnDProject.UnitTests.Services
{
    [TestFixture]
    public class CharacterServiceTests
    {
        //------Create------
        [Test]
        public void CharacterServices_GetBlankKnownClassComponent_ValidCall()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void CharacterServices_CharacterObtainsItem_ValidCall()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void CharacterServices_GetBlankNoteComponent_ValidCall()
        {
            var expected = new NoteCM();
            expected.Index = 1;

            using (var mockContext = AutoMock.GetLoose())
            {

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);
                var creator = ProcessorFactory.getCreateCharacterProcessor(access);
                var updater = ProcessorFactory.getUpdateCharacterProcessor(access);
                var builder = ProcessorFactory.GetCharacterCMBuilder(access);

                //Act                
                var toTest = ServicesFactory.GetCharacterService(creator, updater, builder);
                var actual = toTest.GetBlankNoteComponent(1);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void CharacterSerivces_buildHeldItemRowCM_ValidCall()
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
            expected.Index = 1;
            using (var mockContext = AutoMock.GetLoose())
            {

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Item>()).Returns(itemsMockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(itemsMockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);
                var creator = ProcessorFactory.getCreateCharacterProcessor(access);
                var updater = ProcessorFactory.getUpdateCharacterProcessor(access);
                var builder = ProcessorFactory.GetCharacterCMBuilder(access);

                //Act                
                var toTest = ServicesFactory.GetCharacterService(creator, updater, builder);
                var actual = toTest.buildHeldItemRowCM(1, record.Item_id);

                actual.Should().BeEquivalentTo(expected);

            }
        }
        [Test]
        public void CharacterServices_buildKnownSpellRowCM_ValidCall()
        {
            //Arrange
            List<Spell> spells = CreateTestData.GetListOfSpells();
            var mockSet = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });
            var record = CreateTestData.GetSampleSpell();
            KnownSpellRowCM expected = CharacterMapper.mapSpellToKnownSpellRowCM(record);
            expected.Index = 1;

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(mockSet.Object);

                IUnitOfWork uow = UoW_Factory.getUnitofWork(mockContext);
                IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);
                var creator = ProcessorFactory.getCreateCharacterProcessor(access);
                var updater = ProcessorFactory.getUpdateCharacterProcessor(access);
                var builder = ProcessorFactory.GetCharacterCMBuilder(access);

                //Act                
                var toTest = ServicesFactory.GetCharacterService(creator, updater, builder);
                var actual = toTest.buildKnownSpellRowCM(1, record.Spell_id);
            }
        }

        [Test]
        public void CharacterServices_buildItemDetailsCM_ValidCall()
        {

            throw new NotImplementedException();
        }
        [Test]
        public void CharacterServices_buildSpellDetailsCM_ValidCall()
        {
            throw new NotImplementedException();
        }






        [Test]
        public void CharacterServices_ExistingCharacterLearnsSpell_ValidCall()
        {
            //Arrange
            List<Spell> listOfSpells = CreateTestData.GetListOfSpells();
            List<Spell_Class> listOfCastableBy = CreateTestData.GetListOfCastableByRecords();
            List<Character_Class_Subclass> listofKnownClasses = CreateTestData.GetListOfCharacter_Class_Subclass();
            List<Spell_Character> listOfKnownSpells = new List<Spell_Character>();

            //Caleb should learn how to cast his tower spell.
            Spell_Character expected = CreateTestData.GetSampleKnownSpell();
            Guid Caleb_id = expected.Character_id;
            Guid User_id = Guid.Parse("5f0d6374-fe3e-4337-9a0a-787db1f7b628");
            Guid Tower_id = expected.Spell_id;

            var spellMockSet = new Mock<DbSet<Spell>>()
                .SetupData(listOfSpells, o =>
                {
                    return listOfSpells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });
            var castableByMockSet = new Mock<DbSet<Spell_Class>>()
                .SetupData(listOfCastableBy, o =>
                {
                    return listOfCastableBy.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var knownClassesMockSet = new Mock<DbSet<Character_Class_Subclass>>()
                .SetupData(listofKnownClasses, o =>
                {
                    return listofKnownClasses.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            var knownSpellsMockSet = new Mock<DbSet<Spell_Character>>()
                .SetupData(listOfKnownSpells, o =>
                {
                    return listOfKnownSpells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

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

                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.KnownSpells).Returns(knownSpellsMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell_Character>()).Returns(knownSpellsMockSet.Object);

                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.KnownClasses).Returns(knownClassesMockSet.Object);
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<Character_Class_Subclass>()).Returns(knownClassesMockSet.Object);

                //Act
                ICharacterServices toTest = mockContext.Create<CharacterServices>();
                toTest.ExistingCharacterLearnsSpell(User_id, Caleb_id, Tower_id);

                //Assert
                listOfKnownSpells.Should().ContainEquivalentOf(expected);
                listOfKnownSpells.Where(x => x.Spell_id == expected.Spell_id && x.Character_id == expected.Character_id).Count().Should().Be(1);
            }
        }

        [Test]
        public void CharacterServices_CharacterForgetsClass_ValidCall()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void CharacterServices_CharacterLosesItem_ValidCall()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void CharacterServices_DeleteNote_ValidCall()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void CharacterServices_CharacterForgetsSpell_ValidCall()
        {
            throw new NotImplementedException();
        }

    }
}
