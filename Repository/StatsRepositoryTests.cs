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
    class StatsRepositoryTests
    {
        [Test]
        public void EFRepository_AddStatsRecord_ValidCall()
        {
            List<Stats> statsList = CreateTestData.GetListOfStats();
            var mockSet = new Mock<DbSet<Stats>>()
                .SetupData(statsList, o =>
                {
                    return statsList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleStats();
                var id = Guid.Parse("b346eee6-eba7-4ea7-be2e-911bb9034233");
                expected.Character_id = id;

                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<Stats>()).Returns(mockSet.Object);

                //Act
                IStatsRepository toTest = mockContext.Create<StatsRepository>();
                toTest.Add(expected);
                var actual = toTest.Get(id);

                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Stats>();
                expected.Should().BeOfType<Stats>();
                actual.Should().BeEquivalentTo(expected);

            }
        }

        [Test]
        public void StatsRepository_GetStatsRecord_ValidCall()
        {

            List<Stats> statsList = CreateTestData.GetListOfStats();
            var mockSet = new Mock<DbSet<Stats>>()
                .SetupData(statsList, o =>
                {
                    return statsList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleStats();
                mockContext.Mock<CharacterContext>()
                   .Setup(x => x.Set<Stats>()).Returns(mockSet.Object);

                //Act
                IStatsRepository toTest = mockContext.Create<StatsRepository>();
                var actual = toTest.Get(expected.Character_id);

                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Stats>();
                expected.Should().BeOfType<Stats>();
                actual.Should().BeEquivalentTo(expected);

            }
        }

        [Test]
        public void EFRepository_UpdateStatsRecord_ValidCall()
        {
            //Arrange

            List<Stats> statsList = CreateTestData.GetListOfStats();
            var mockSet = new Mock<DbSet<Stats>>()
                .SetupData(statsList, o =>
                {
                    return statsList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleStats();
                expected.Strength = 20;
                expected.Dexterity = 20;
                expected.Constitution = 24;
                mockContext.Mock<CharacterContext>()
                   .Setup(x => x.Set<Stats>()).Returns(mockSet.Object);


                //Act
                IStatsRepository toTest = mockContext.Create<StatsRepository>();
                toTest.Update(expected);

                //Assert
                expected.Should().NotBeNull();
                expected.Should().BeOfType<Stats>();
                //Verifies that the object I wished to update was attached to the DbSet.
                //Basically, that means EF confirms that the entity with expected's Primary key will be updated the next time Save is called.
                mockSet.Verify(x => x.Attach(expected), Times.Once());

            }
        }
    }
}
