using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Unit_Of_Work.Implementations;
using DnDProject.Backend.Unit_Of_Work.Interfaces;
using DnDProject.Backend.UserAccess.Interfaces;
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

namespace DnDProject.UnitTests.UserAccess
{
    [TestFixture]
    class RaceManagerUserAccessTests
    {
        [Test]
        public void RaceManager_AddRace_ValidCall()
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
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IRaceManagerUserAccess toTest = UserAccessFactory.GetRaceManagerUserAccess(worker);
                toTest.AddRace(expected);

                //Assert
                races.Should().ContainEquivalentOf(expected);
            }
        }
        [Test]
        public void RaceManager_AddRaceAbility_ValidCall()
        {
            List<RaceAbility> raceAbilities = new List<RaceAbility>();
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
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IRaceManagerUserAccess toTest = UserAccessFactory.GetRaceManagerUserAccess(worker);
                toTest.AddRaceAbility(expected);

                //Assert
                raceAbilities.Should().ContainEquivalentOf(expected);

            }
        }
        [Test]
        public void RaceManager_RemoveRace_ValidCall()
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
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IRaceManagerUserAccess toTest = UserAccessFactory.GetRaceManagerUserAccess(worker);
                toTest.RemoveRace(toBeDeleted);

                //Assert
                races.Should().NotContain(toBeDeleted);

            }
        }
        [Test]
        public void RaceManager_RemoveRaceAbility_ValidCall()
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
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IRaceManagerUserAccess toTest = UserAccessFactory.GetRaceManagerUserAccess(worker);
                toTest.RemoveRaceAbility(toBeDeleted);

                //Assert
                raceAbilities.Should().NotContain(toBeDeleted);

            }
        }
    }
}
