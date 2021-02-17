using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Mapping.Interfaces;
using DnDProject.Backend.Services.Implementations;
using DnDProject.Backend.Services.Interfaces;
using DnDProject.Backend.Unit_Of_Work.Implementations;
using DnDProject.Backend.Unit_Of_Work.Interfaces;
using DnDProject.Backend.UserAccess.Implementations;
using DnDProject.Backend.UserAccess.Interfaces;
using DnDProject.Entities.Character.DataModels;
using DnDProject.Entities.Character.ViewModels;
using DnDProject.Entities.Races.DataModels;
using DnDProject.UnitTests.Mapping;
using DnDProject.UnitTests.UserAccess;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDProject.UnitTests.Services.Character
{
    [TestFixture]
    public class CreateCharacterTests
    {
        [Test]
        public void CreateCharacter_CreateCharacterGET_racesIsPopulated()
        {
            //Arrange
            List<Race> races = CreateTestData.GetListOfRace();
            using (var mockContext = getMockContext())
            {
                //Act
                ICreateCharacter toTest = getCharacterCreator(mockContext);
                var primaryTab = toTest.CreateCharacterGET().PrimaryTab;
                var actual = primaryTab.Races;

                //Assert
                actual.Count.Should().BeGreaterThan(0);
                actual.Count.Should().Be(races.Count);
                races.Count.Should().BeGreaterThan(0);
            }
        }

        [Test]
        public void CreateCharacter_CreateCharacterGET_proficiencyCMNotNull()
        {
            //Arrange
            using (var mockContext = getMockContext())
            {
                //Act
                ICreateCharacter toTest = getCharacterCreator(mockContext);
                var primaryTab = toTest.CreateCharacterGET().PrimaryTab;
                var actual = primaryTab.IsProficient;

                //Assert
                actual.Should().NotBeNull();               
            }
        }

        [Test]
        public void CreateCharacter_CreateCharacterGET_statsCMNotNull()
        {
            
            using (var mockContext = getMockContext())
            {
                ICreateCharacter toTest = getCharacterCreator(mockContext);
                var primaryTab = toTest.CreateCharacterGET().PrimaryTab;
                var actual = primaryTab.Stats;

                //Assert
                actual.Should().NotBeNull();
            }
        }

        [Test]
        public void CreateCharacter_CreateCharacterGET_CombatCMNotNull()
        {
            using (var mockContext = getMockContext())
            {
                ICreateCharacter toTest = getCharacterCreator(mockContext);
                var primaryTab = toTest.CreateCharacterGET().PrimaryTab;
                var actual = primaryTab.Combat;

                //Assert
                actual.Should().NotBeNull();
            }
        }
        private static AutoMock getMockContext()
        {
            //------mockSets-------
            //Races
            List<Race> races = CreateTestData.GetListOfRace();
            var racesMockSet = new Mock<DbSet<Race>>()
                 .SetupData(races, o =>
                 {
                     return races.Single(x => x.Race_id.CompareTo(o.First()) == 0);
                 });
            //IsProficient
            List<IsProficient> listofIsProficient = CreateTestData.GetListOfIsProficient();
            var isProficientMockSet = new Mock<DbSet<IsProficient>>()
                 .SetupData(listofIsProficient, o =>
                 {
                     return listofIsProficient.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                 });


            var mockContext = AutoMock.GetLoose();
            //-----Call & Return------

            //Races
            mockContext.Mock<RaceContext>()
                .Setup(x => x.Races).Returns(racesMockSet.Object);
            mockContext.Mock<RaceContext>()
                .Setup(x => x.Set<Race>()).Returns(racesMockSet.Object);

            //IsProficient
            mockContext.Mock<CharacterContext>()
                .Setup(x => x.Proficiencies).Returns(isProficientMockSet.Object);
            mockContext.Mock<CharacterContext>()
                .Setup(x => x.Set<IsProficient>()).Returns(isProficientMockSet.Object);

            return mockContext;


        }

        private static ICreateCharacter getCharacterCreator(AutoMock mockContext)
        {
            IUnitOfWork uow = mockContext.Create<UnitOfWork>();
            IBaseUserAccess access = UserAccessFactory.getBaseUserAccess(uow);
            ICreateCharacter toTest = new CreateCharacter(access);
            return toTest;

        }


    }
}
