using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Repository.Implementations;
using DnDProject.Backend.Repository.Interfaces;
using DnDProject.Entities.Character.DataModels;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDProject.UnitTests.Repository
{
    [TestFixture]
    class CharacterRepositoryTests
    {
        [Test]
        public void CharacterRepository_GetCharacter_ValidCall()
        {

            //Arrange
            //1. Create the test data.
            List<Character> charList = CreateTestData.GetListOfCharacters();

            //2. Create a mock set, one that properly responds to EntityFramework's .Find()
            //from the charList, return the first object that has a character_id that matches the given character_id.
            var mockSet = new Mock<DbSet<Character>>()
                .SetupData(charList, o =>
                {
                    return charList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                //Act
                //3. Use the mockSet to properly create the mockContext.
                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<Character>()).Returns(mockSet.Object);

                //4. Create a instance of the Character repository, injecting mockContext via the constructor.
                ICharacterRepository toTest = mockContext.Create<CharacterRepository>();
                var expected = CreateTestData.getSampleCharacter();
                var actual = toTest.Get(expected.Character_id);


                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Character>();
                expected.Should().BeOfType<Character>();
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void CharacterRepository_AddCharacter_ValidCall()
        {
            //Arrange
            int saveChanges = 0;
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
                    .Setup(x => x.SaveChanges()).Callback(() => saveChanges = saveChanges + 1);
                ICharacterRepository toTest = mockContext.Create<CharacterRepository>();
                var expected = CreateTestData.getSampleCharacter();
                expected.Character_id = Guid.Parse("33855fe6-807a-46e3-850f-ada7dacfc435");

                //Act
                toTest.Add(expected);
                var actual = toTest.Get(expected.Character_id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Character>();
                expected.Should().BeOfType<Character>();
                actual.Should().BeEquivalentTo(expected);
            }
        }

        
        [Test]
        public void CharacterRepository_DeleteCharacter_ValidCall()
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
                    //When a removal of a Character object is called, perform a callback to the charList collection, using the same character object as an argument.
                    //This callback then fires, removing the object from the list.
                    .Setup(x => x.Set<Character>().Remove(It.IsAny<Character>()))
                        .Callback<Character>((entity) => charList.Remove(entity));

                //Act
                ICharacterRepository toTest = mockContext.Create<CharacterRepository>();
                var id = CreateTestData.getSampleCharacter().Character_id;
                toTest.Remove(id);
                Character expected = null;
                var NotExpected = CreateTestData.getSampleCharacter();
                var actual = charList.Find(character => character.Character_id == id);

                //Assert
                actual.Should().BeNull();
                actual.Should().NotBeEquivalentTo(NotExpected);
                actual.Should().BeEquivalentTo(expected);


            }
        }


    }
}
