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
        //Create/Update
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
        public void CharacterMapper_MapCharacterToCharacterVM_ItemsAreEquivalent()
        {
            //Arrange
            var expected = CreateTestData.getSampleCharacterVM();
            var m = CreateTestData.getSampleCharacter();

            //Act
            var actual = CharacterMapper.mapCharacterToCharacterVM(m);

            //Assert
            expected.Should().BeOfType<CharacterVM>();
            actual.Should().BeOfType<CharacterVM>();
            actual.Should().NotBeNull();
            expected.Should().NotBeNull();
            actual.Should().BeEquivalentTo(expected);

        }      
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
    }
}
