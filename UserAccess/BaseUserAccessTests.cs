using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Repository;
using DnDProject.Backend.Repository.Implementations;
using DnDProject.Backend.UserAccess.Implementations;
using DnDProject.Backend.UserAccess.Interfaces;
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

namespace DnDProject.UnitTests.UserAccess
{
    [TestFixture]
    public class BaseUserAccessTests
    {
        [Test]
        public void BaseUserAccess_GetCharacter_ValidCall()
        {
            //Arrange
            List<Character> charList = new List<Character>();
            charList.Add(CreateTestData.getSampleCharacter());

            var mockSet = new Mock<DbSet<Character>>()
                .SetupData(charList, o =>
                {
                    return charList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<CharacterContext>().Setup(x => x.Characters).Returns(mockSet.Object);
                IDataRepository dataRepository = mockContext.Create<MySqlDataRepository>();

                IBaseUserAccess toTest = new BaseUserAccess(dataRepository);

                //Act
                var expected = CreateTestData.getSampleCharacter();
                Guid id = expected.Character_id;
                var actual = toTest.GetCharacter(id);

                expected.Should().BeOfType<Character>();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Character>();
                actual.Should().NotBeNull();

                actual.Should().BeEquivalentTo(expected);

            }
        }

        [Test]
        public void BaseUserAccess_AddCharacter_ValidCall()
        {
            //Arrange
            List<Character> charList = new List<Character>();
            charList.Add(CreateTestData.getSampleCharacter());

            var mockSet = new Mock<DbSet<Character>>()
                .SetupData(charList, o =>
                {
                    return charList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<CharacterContext>().Setup(x => x.Characters).Returns(mockSet.Object);
                IDataRepository dataRepository = mockContext.Create<MySqlDataRepository>();

                IBaseUserAccess toTest = new BaseUserAccess(dataRepository);

                var expected = CreateTestData.getSampleCharacter();
                Guid id = Guid.Parse("757f4657-ff89-4a9d-a31b-aeda05a38615");
                expected.Character_id = id;

                //Act
                toTest.AddCharacter(expected);
                var actual = toTest.GetCharacter(id);

                expected.Should().BeOfType<Character>();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Character>();
                actual.Should().NotBeNull();

                actual.Should().BeEquivalentTo(expected);

            }

        }
    }
}
