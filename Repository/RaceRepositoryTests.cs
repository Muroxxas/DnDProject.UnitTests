using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Repository.Implementations;
using DnDProject.Entities.Races.DataModels;
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
    class RaceRepositoryTests
    {
        [Test]
        public void RaceRepository_AddRace_ValidCall()
        {
            //Arrange
            List<Race> races = new List<Race>();
            var mockSet = new Mock<DbSet<Race>>()
                .SetupData(races, o =>
                {
                    return races.Single(x => x.Race_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetSampleRace();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<RaceContext>()
                    .Setup(x => x.Set<Race>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<RaceRepository>();
                toTest.Add(expected);

                //Assert
                races.Should().ContainEquivalentOf(expected);
            }
        }
        [Test]
        public void RaceRepository_AddRaceAbility_ValidCall()
        {
            List < RaceAbility > raceAbilities = new List<RaceAbility>();
            var mockSet = new Mock<DbSet<RaceAbility>>()
                .SetupData(raceAbilities, o =>
                {
                    return raceAbilities.Single(x => x.RaceAbility_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetSampleRaceAbility();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<RaceContext>()
                    .Setup(x => x.RaceAbilities).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<RaceRepository>();
                toTest.AddRaceAbility(expected);

                //Assert
                raceAbilities.Should().ContainEquivalentOf(expected);

            }
        }
        [Test]
        public void RaceRepository_GetRace_ValidCall()
        {
            //Arrange
            List<Race> races = CreateTestData.GetListOfRace();
            var mockSet = new Mock<DbSet<Race>>()
                .SetupData(races, o =>
                {
                    return races.Single(x => x.Race_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetSampleRace();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<RaceContext>()
                    .Setup(x => x.Set<Race>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<RaceRepository>();
                var actual = toTest.Get(expected.Race_id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Race>();
                expected.Should().BeOfType<Race>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void RaceRepository_GetAllRaces_ValidCall()
        {
            //Arrange
            List<Race> races = CreateTestData.GetListOfRace();
            var mockSet = new Mock<DbSet<Race>>()
                .SetupData(races, o =>
                {
                    return races.Single(x => x.Race_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetListOfRace();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<RaceContext>()
                    .Setup(x => x.Set<Race>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<RaceRepository>();
                var actual = toTest.GetAll().ToList();

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<List<Race>>();
                expected.Should().BeOfType<List<Race>>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void RaceRepository_GetRaceAbility_ValidCall()
        {
            //Arrange
            List<RaceAbility> raceAbilities = CreateTestData.GetListOfRaceAbility();
            var mockSet = new Mock<DbSet<RaceAbility>>()
                .SetupData(raceAbilities, o =>
                {
                    return raceAbilities.Single(x => x.RaceAbility_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetSampleRaceAbility();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<RaceContext>()
                    .Setup(x => x.RaceAbilities).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<RaceRepository>();
                var actual = toTest.GetRaceAbility(expected.RaceAbility_id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<RaceAbility>();
                expected.Should().BeOfType<RaceAbility>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void RaceRepository_GetAbilitiesOfRace_ValidCall()
        {
            //Arrange
            List<RaceAbility> raceAbilities = CreateTestData.GetListOfRaceAbility();
            var mockSet = new Mock<DbSet<RaceAbility>>()
                .SetupData(raceAbilities, o =>
                {
                    return raceAbilities.Single(x => x.RaceAbility_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetSampleRaceAbility();
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<RaceContext>()
                   .Setup(x => x.RaceAbilities).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<RaceRepository>();
                var actual = toTest.GetAbilitiesOfRace(expected.Race_id).ToList();

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<List<RaceAbility>>();
                expected.Should().BeOfType<RaceAbility>();
                actual.Should().ContainEquivalentOf(expected);
            }
        }
        [Test]
        public void RaceRepository_RemoveRace_ValidCall()
        {
            //Arrange
            List<Race> races = CreateTestData.GetListOfRace();
            var mockSet = new Mock<DbSet<Race>>()
                .SetupData(races, o =>
                {
                    return races.Single(x => x.Race_id.CompareTo(o.First()) == 0);
                });
            var toBeDeleted = CreateTestData.GetSampleRace();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<RaceContext>()
                    .Setup(x => x.Set<Race>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<RaceRepository>();
                toTest.Remove(toBeDeleted);

                //Assert
                races.Should().NotContain(toBeDeleted);

            }
        }
        [Test]
        public void RaceRepository_RemoveRaceAbility_ValidCall()
        {
            //Arrange
            List<RaceAbility> raceAbilities = CreateTestData.GetListOfRaceAbility();
            var mockSet = new Mock<DbSet<RaceAbility>>()
                .SetupData(raceAbilities, o =>
                {
                    return raceAbilities.Single(x => x.RaceAbility_id.CompareTo(o.First()) == 0);
                });
            var toBeDeleted = CreateTestData.GetSampleRaceAbility();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<RaceContext>()
                    .Setup(x => x.RaceAbilities).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<RaceRepository>();
                toTest.RemoveRaceAbility(toBeDeleted);

                //Assert
                raceAbilities.Should().NotContain(toBeDeleted);

            }
        }
    }
}
