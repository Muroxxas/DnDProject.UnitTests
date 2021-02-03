using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Repository;
using DnDProject.Backend.Repository.Implementations;
using DnDProject.Entities.Character.DataModels;
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
    public class MySqlDataRepositoryTests
    {
        [Test]
        public void MySqlDataRepository_ConstructMySqlDataRoot_ReturnTrue()
        {
            var toTest = new MySqlDataRepository();

            Assert.IsNotNull(toTest);
        }

        [Test]
        public void MySqlDataRepository_IsImplementationOfIDataRoot_ReturnTrue()
        {
            //Arrange

            //Act
            var toTest = new MySqlDataRepository();

            //Assert
            Assert.IsInstanceOf<IDataRepository>(toTest);
        }

        [Test]
        public void MySqlDataRepository_LoadCharacterByCharacterID_SingularCharacterObjectReturned()
        {

            //Arrange
            //1. Create the test data.
            List<Character> charList = new List<Character>();
            charList.Add(CreateTestData.getSampleCharacter());

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
                mockContext.Mock<CharacterContext>().Setup(x => x.Characters).Returns(mockSet.Object);

                //4. Create a instance of MySqlDataRepository, injecting mockContext via the constructor.
                IDataRepository toTest = mockContext.Create<MySqlDataRepository>();
                var expected = CreateTestData.getSampleCharacter();
                var Actual = toTest.GetCharacterBy_CharacterID(expected.Character_id);


                //Assert
                Assert.IsNotNull(expected);
                Assert.IsNotNull(Actual);
                Assert.AreEqual(Actual.Character_id, expected.Character_id);
            }
        }

        [Test]
        public void MySqlDataRepository_AddCharacterToCharacterID_CharacterAdded()
        {
            //Arrange
            List<Character> charList = new List<Character>();
            var mockSet = new Mock<DbSet<Character>>()
                .SetupData(charList, o =>
                {
                    return charList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                //Act
                mockContext.Mock<CharacterContext>().Setup(x => x.Characters).Returns(mockSet.Object);

                IDataRepository toTest = mockContext.Create<MySqlDataRepository>();
                var expected = CreateTestData.getSampleCharacter();

                toTest.InsertCharacterIntoDb(expected);
                var actual = toTest.GetCharacterBy_CharacterID(expected.Character_id);

                //Assert
                Assert.IsNotNull(expected);
                Assert.IsNotNull(actual);
                Assert.AreEqual(expected.Character_id, actual.Character_id);

            }
        }




    }
}
