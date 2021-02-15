using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Repository.Implementations;
using DnDProject.Entities.Class.DataModels;
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
    class SubclassAbilitiesRepoTests
    {
        [Test]
        public void SubclassAbilitiesRepo_AddSubclassAbility_ValidCall()
        {
            //Arrange
            List<SubclassAbility> listofSubclassAbility = new List<SubclassAbility>();
            SubclassAbility expected = CreateTestData.GetSubclassAbility();
            var mockSet = new Mock<DbSet<SubclassAbility>>()
                .SetupData(listofSubclassAbility, o =>
                {
                    return listofSubclassAbility.Single(x => x.Subclass_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<SubclassAbility>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<SubclassAbilityRepository>();
                toTest.Add(expected);

                //Assert
                listofSubclassAbility.Should().ContainEquivalentOf(expected);
            }
        }
        [Test]
        public void SubclassAbilitiesRepo_AddManySubclassAbilities_ValidCall()
        {
            //Arrange
            List<SubclassAbility> listofSubclassAbility = new List<SubclassAbility>();
            List<SubclassAbility> expected = CreateTestData.GetListOfSubclassAbility();
            var mockSet = new Mock<DbSet<SubclassAbility>>()
                .SetupData(listofSubclassAbility, o =>
                {
                    return listofSubclassAbility.Single(x => x.Subclass_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<SubclassAbility>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<SubclassAbilityRepository>();
                toTest.AddRange(expected);

                //Assert
                listofSubclassAbility.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void SubclassAbilitiesRepo_GetSubclassAbility_ValidCall()
        {
            //Arrange
            List<SubclassAbility> listofSubclassAbility = CreateTestData.GetListOfSubclassAbility();
            SubclassAbility expected = CreateTestData.GetSubclassAbility();
            var mockSet = new Mock<DbSet<SubclassAbility>>()
                .SetupData(listofSubclassAbility, o =>
                {
                    return listofSubclassAbility.Single(x => x.SubclassAbility_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<SubclassAbility>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<SubclassAbilityRepository>();
                var actual = toTest.Get(expected.SubclassAbility_id);

                //Assert
                actual.Should().NotBeNull();
                actual.Should().BeOfType<SubclassAbility>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void SubclassAbilitiesRepo_GetAllAbilitiesOfSubclass_ValidCall()
        {
            //Arrange
            List<SubclassAbility> listofSubclassAbility = CreateTestData.GetListOfSubclassAbility();
            List<SubclassAbility> expected = new List<SubclassAbility>();
            SubclassAbility Gunslinger = CreateTestData.GetSubclassAbility();
            expected.Add(Gunslinger);
            var gunslinger_id = Gunslinger.Subclass_id;
            var mockSet = new Mock<DbSet<SubclassAbility>>()
                .SetupData(listofSubclassAbility, o =>
                {
                    return listofSubclassAbility.Single(x => x.Subclass_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.SubclassAbilities).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<SubclassAbilityRepository>();
                var actual = toTest.GetAbilitiesOfSubclass(gunslinger_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void SubclassAbilitiesRepo_GetAbilitiesOfSubclassAtOrBelowLevel_ValidCall()
        {
            //Arrange
            List<SubclassAbility> listofSubclassAbility = CreateTestData.GetListOfSubclassAbility();

            List<SubclassAbility> expected = new List<SubclassAbility>();
            SubclassAbility Gunslinger = CreateTestData.GetSubclassAbility();
            SubclassAbility Quickdraw = new SubclassAbility
            {
                Subclass_id = Gunslinger.Subclass_id,
                SubclassAbility_id = Guid.Parse("eb852e1e-39a6-47af-86e2-5dfb3fc8bdee"),
                Name = "Quickdraw",
                Description = "You add your proficiency bonus to your initiative. You can also stow a firearm, then draw another firearm as a single object interaction on your turn.",
                LevelLearned = 7
            };
            listofSubclassAbility.Add(Quickdraw);
            expected.Add(Gunslinger);
            var gunslinger_id = Gunslinger.Subclass_id;
            var mockSet = new Mock<DbSet<SubclassAbility>>()
                .SetupData(listofSubclassAbility, o =>
                {
                    return listofSubclassAbility.Single(x => x.Subclass_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.SubclassAbilities).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<SubclassAbilityRepository>();
                var actual = toTest.GetAbilitiesOfSubclassAtOrBelowLevel(gunslinger_id, 5);

                //Assert
                actual.Should().BeEquivalentTo(expected);
                actual.Should().NotContain(Quickdraw);
            }
        }
        [Test]
        public void SubclassAbilitiesRepo_RemoveSubclassAbilities_ValidCall()
        {
            //Arrange
            List<SubclassAbility> listofSubclassAbility = CreateTestData.GetListOfSubclassAbility();
            SubclassAbility notExpected = CreateTestData.GetSubclassAbility();

            var mockSet = new Mock<DbSet<SubclassAbility>>()
                .SetupData(listofSubclassAbility, o =>
                {
                    return listofSubclassAbility.Single(x => x.Subclass_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<SubclassAbility>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<SubclassAbilityRepository>();
                toTest.Remove(notExpected);

                //Assert
                listofSubclassAbility.Should().NotContain(notExpected);
            }
        }

//        //Arrange
//        List<SubclassAbility> listofSubclassAbility = new List<SubclassAbility>();

//        var mockSet = new Mock<DbSet<SubclassAbility>>()
//            .SetupData(listofSubclassAbility, o =>
//            {
//                return listofSubclassAbility.Single(x => x.Subclass_id.CompareTo(o.First()) == 0);
//            });

//            using (var mockContext = AutoMock.GetLoose())
//            {
//                mockContext.Mock<PlayableClassContext>()
//                    .Setup(x => x.Set<SubclassAbility>()).Returns(mockSet.Object);

//    //Act
//    var toTest = mockContext.Create<SubclassAbilityRepository>();

//    //Assert
//}
    }
}
