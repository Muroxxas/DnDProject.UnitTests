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

            actual.Should().BeOfType<Character>();
            actual.Should().BeEquivalentTo(vm);

            //VM doesn't have the user id, and we don't want it to, so it is excluded
            actual.Should().BeEquivalentTo(expected, options => options.Excluding(item => item.User_id));
            

        }
    }
}
