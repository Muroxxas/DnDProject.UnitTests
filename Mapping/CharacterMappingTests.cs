using DnDProject.Backend.Mapping.Interfaces;
using DnDProject.Entities.Character.DataModels;
using DnDProject.Entities.Character.ViewModels;
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

        [Test]
        public void CharacterMapper_MapCharacterToCharacterVM_ItemsAreEquivalent()
        {
            //Arrange
            var expected = CreateTestData.getSampleCharacterVM();
            var m = CreateTestData.getSampleCharacter();
            ICharacterMapper toTest = mappingTestFactory.getCharacterMapper();

            //Act
            var actual = toTest.mapCharacterToCharacterVM(m);

            //Assert
            expected.Should().BeOfType<CharacterVM>();
            actual.Should().BeOfType<CharacterVM>();
            actual.Should().NotBeNull();
            expected.Should().NotBeNull();
            actual.Should().BeEquivalentTo(expected);

        }

        [Test]
        public void CharacterMapper_MapCharacterVMToNewEntity_ItemsAreEquivalent() 
        {
            //Arrance
            var expected = CreateTestData.getSampleCharacterVM();
            ICharacterMapper toTest = mappingTestFactory.getCharacterMapper();


            //Act
            var actual = toTest.mapCharacterVMToNewEntity(expected);

            //Assert
            actual.Should().BeOfType<Character>();
            expected.Should().BeOfType<CharacterVM>();
            actual.Should().NotBeNull();
            expected.Should().NotBeNull();
            actual.Should().BeEquivalentTo<CharacterVM>(expected);
        }

        [Test]
        public void CharacterMapper_MapCharacterVMToExistingEntity_ItemsAreEquivalent()
        {
            //Arrange
            var vm = CreateTestData.getSampleCharacterVM();
            var expected = CreateTestData.getSampleCharacter();
            var actual = new Character();
            ICharacterMapper toTest = mappingTestFactory.getCharacterMapper();

            //Act
            toTest.mapCharacterVMToExistingEntity(vm, actual);

            //Assert
            actual.Should().BeOfType<Character>();
            expected.Should().BeOfType<Character>();
            actual.Should().NotBeNull();
            expected.Should().NotBeNull();
            actual.Should().BeEquivalentTo(vm);            

            //VM doesn't have the user id, and we don't want it to, so it is excluded
            actual.Should().BeEquivalentTo(expected, options => options.Excluding(item => item.User_id));
            

        }

        [Test]
        public void CharacterMapper_MapUpdatedCharacterOverEntity_ItemsAreEquivalent()
        {
            //Arrange
            var expected = CreateTestData.getSampleCharacter();
            var actual = new Character();
            ICharacterMapper toTest = mappingTestFactory.getCharacterMapper();

            //Act
            toTest.mapUpdatedCharacterOverEntity(expected, actual);


            //Assert
            actual.Should().BeOfType<Character>();
            expected.Should().BeOfType<Character>();
            actual.Should().NotBeNull();
            expected.Should().NotBeNull();
            actual.Should().BeEquivalentTo(expected);


        }

        [Test]
        public void CharacterMapper_MapUpdatedProficiencyRecordOverEntity_ItemsAreEquivalent()
        {

            //Arrange
            var expected = CreateTestData.GetSampleIsProficient();
            var actual = new IsProficient();

            //Act
            ICharacterMapper toTest = mappingTestFactory.getCharacterMapper();
            toTest.mapUpdatedProficiencyRecordOverEntity(expected, actual);

            //Assert
            actual.Should().BeOfType<IsProficient>();
            actual.Should().NotBeNull();
            expected.Should().BeOfType<IsProficient>();
            expected.Should().NotBeNull();
            actual.Should().BeEquivalentTo(expected);


        }

        [Test]
        public void CharacterMapper_MapUpdatedHealthRecordOverEntity_ItemsAreEquivalent()
        {
            //Arrange
            var expected = CreateTestData.GetSampleHealth();
            var actual = new Health();

            ICharacterMapper toTest = mappingTestFactory.getCharacterMapper();
            toTest.mapUpdatedHealthRecordOverEntity(expected, actual);

            //Assert
            actual.Should().BeOfType<Health>();
            actual.Should().NotBeNull();
            expected.Should().BeOfType<Health>();
            expected.Should().NotBeNull();
            actual.Should().BeEquivalentTo(expected);
        }
        [Test]
        public void CharacterMapper_MapUpdatedStatsRecordOverEntity_ItemsAreEquivalent()
        {
            //Arrange
            var expected = CreateTestData.GetSampleStats();
            var actual =new Stats();
            //Act
            ICharacterMapper toTest = mappingTestFactory.getCharacterMapper();
            toTest.mapUpdatedStatsRecordOverEntity(expected, actual);

            //Assert
            actual.Should().BeOfType<Stats>();
            actual.Should().NotBeNull();
            expected.Should().BeOfType<Stats>();
            expected.Should().NotBeNull();
            actual.Should().BeEquivalentTo(expected);

        }

    }
}
