using DnDProject.Backend.Mapping.Implementations;
using DnDProject.Backend.Mapping.Interfaces;
using DnDProject.Entities.Character.DataModels;
using DnDProject.Entities.Character.ViewModels;
using DnDProject.Entities.Character.ViewModels.PartialViewModels.Components;
using DnDProject.Entities.Races.ViewModels.PartialViewModels.ComponentModels;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDProject.UnitTests.Mapping
{
    [TestFixture]
    public class CharacterMappingTests
    {
        //Create
        [Test]
        public void CharacterMapper_MapCharacterVmToCharacter()
        {
            CharacterVM vm = new CharacterVM();
            vm.Name = "Bradley Norwood";

            CharacterDM actual = CharacterMapper.mapCharacterVMToNewEntity(vm);

            actual.Name.Should().Be(vm.Name);
        }
        [Test]
        public void CharacterMapper_MapCombatCMToHealthRecord()
        {
            CombatCM cm = new CombatCM();
            cm.MaxHP = 50;

            Health actual = CharacterMapper.mapCombatCMToNewHealthEntity(cm);

            actual.MaxHP.Should().Be(cm.MaxHP);
        }
        [Test]
        public void CharacterMapper_MapStatsCMToNewEntity()
        {
            StatsCM cm = new StatsCM();
            cm.Strength = 14;

            Stats actual = CharacterMapper.mapStatsCMToNewEntity(cm);

            actual.Strength.Should().Be(cm.Strength);
        }

        [Test]
        public void CharacterMapper_MapMoneyCMToNewEntity()
        {
            MoneyCM cm = new MoneyCM();
            cm.GoldPieces = 50;

            Currency actual = CharacterMapper.mapCurrencyCMToNewEntity(cm);

            actual.GoldPieces.Should().Be(cm.GoldPieces);
        }
        [Test]
        public void CharacterMapper_MapNoteCMToNewEntity()
        {
            NoteCM cm = new NoteCM();
            cm.Name = "Backstory";

            Note actual = CharacterMapper.mapNoteCMToNewEntity(cm);

            actual.Name.Should().Be(cm.Name);
        }



        //update
        [Test]
        public void CharacterMapper_MapNoteCMToNote_ValidCall()
        {
            //Arrance
            NoteCM updated = new NoteCM
            {
                Note_id = Guid.Parse("f0a03fe1-4d70-4e4f-8b91-5f34494bdccb"),
                Name = "Background",
                Contents = "Test"
            };
            var overwritten = new Note { 
                Note_id = Guid.Parse("e29ca3aa-867e-467a-a4fe-8235e621548e")
            };

            //Act
            CharacterMapper.mapNoteCMOverNote(updated, overwritten);

            //Assert
            //I want to map all properties except for the Note id - I don't want that accidentally being changed
            overwritten.Should().BeEquivalentTo(updated,
                options => options.Excluding(o => o.Note_id));
            overwritten.Note_id.Should().NotBe(updated.Note_id);
        }

        //Read

     
        [Test]
        public void CharacterMapper_MapRaceToRaceListModel_ValidCall()
        {
            //Arrance
            var expected = CreateTestData.GetSampleRace();
            var actual = new RaceListModel();

            //Act            
            actual = CharacterMapper.mapRaceToRaceListModel(expected);

            //Arrange
            expected.Should().NotBeNull();
            actual.Should().NotBeNull();
            actual.Race_id.Should().Be(expected.Race_id);
            actual.Name.Should().Be(expected.Name);


        }
        [Test]
        public void CharacterMapper_MapIsProficientToIsProficientCM_ValidCall()
        {
            //Arrange
            var record = CreateTestData.GetSampleIsProficient();
            var actual = new IsProficientCM();

            //Act
            actual = CharacterMapper.mapIsProficientToIsProficientCM(record);

            //Assert
            actual.Should().BeEquivalentTo(record,
                options => options.Excluding(o => o.Character_id));

        }
        [Test]
        public void CharacterMapper_MapNoteToNoteCM_ValidCall()
        {
            //Arrange
            var record = CreateTestData.GetSampleNote();
            var actual = new NoteCM();

            //Act
            actual = CharacterMapper.mapNoteToNoteCM(record);

            actual.Should().BeEquivalentTo(record,
                options => options.Excluding(o => o.Character_id));
        }
        [Test]
        public void CharacterMapper_mapItemToHeldItemRowCM()
        {
            var record = CreateTestData.GetSampleItem();
            var expected = new HeldItemRowCM();
            expected.Item_id = record.Item_id;
            expected.Name = record.Name;
            expected.Value = record.Value;
            expected.isEquippable = record.isEquippable;
            expected.isEquipped = false;
            expected.requiresAttunement = record.requiresAttunement;
            expected.isAttuned = false;

            //act
            var actual = CharacterMapper.mapItemToHeldItemRowCM(record);

            //Assert
            actual.Should().BeEquivalentTo(expected);            
        }
        [Test]
        public void CharacterMapper_mapHeldItemRecordToHeldItemRowCM()
        {
            var record = CreateTestData.GetSampleHeldItem();
            var expected = new HeldItemRowCM();
            expected.Item_id = record.Item_id;
            expected.isEquipped = record.isEquipped;
            expected.isAttuned = record.IsAttuned;
            expected.Count = record.count;

            //Act
            var actual = CharacterMapper.mapHeldItemRecordToHeldItemRowCM(record);

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CharacterMapper_mapSpellToKnownSpellRowCM_ValidCall()
        {
            var record = CreateTestData.GetSampleSpell();
            var expected = new KnownSpellRowCM();
            expected.Spell_id = record.Spell_id;
            expected.Name = record.Name;
            expected.School = null;
            expected.Level = record.Level;
            expected.isPrepared = false;

            var actual = CharacterMapper.mapSpellToKnownSpellRowCM(record);

            actual.Should().BeEquivalentTo(expected);

        }
    }
}
