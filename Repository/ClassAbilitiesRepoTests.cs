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
    class ClassAbilitiesRepoTests
    {
        [Test]
        public void ClassAbilitiesRepo_AddAbility_ValidCall()
        {
            //Arrange
            List<ClassAbility> listofClassAbility = new List<ClassAbility>();
            var mockSet = new Mock<DbSet<ClassAbility>>()
                .SetupData(listofClassAbility, o =>
                {
                    return listofClassAbility.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetClassAbility();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    //The argument for setup *MUST* be an exact match for the syntax we use in the implementation. Otherwise, the test will fail!
                    .Setup(x => x.Set<ClassAbility>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<ClassAbilityRepository>();
                toTest.Add(expected);
                var actual = toTest.GetAll();

                //Assert
                actual.Should().ContainEquivalentOf(expected);
            }
        } 
    
        [Test]
        public void ClassAbilitiesRepo_AddAbilities_ValidCall()
        {
            //Arrange
            List<ClassAbility> listofClassAbility = new List<ClassAbility>();
            var mockSet = new Mock<DbSet<ClassAbility>>()
                .SetupData(listofClassAbility, o =>
                {
                    return listofClassAbility.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetListOfClassAbility();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    //The argument for setup *MUST* be an exact match for the syntax we use in the implementation. Otherwise, the test will fail!
                    .Setup(x => x.Set<ClassAbility>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<ClassAbilityRepository>();
                toTest.AddRange(expected);
                var actual = toTest.GetAll();

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void ClassAbilitiesRepo_GetAbility_ValidCall()
        {
            //Arrange
            List<ClassAbility> listofClassAbility = CreateTestData.GetListOfClassAbility();

            var mockSet = new Mock<DbSet<ClassAbility>>()
                .SetupData(listofClassAbility, o =>
                {
                    return listofClassAbility.Single(x => x.ClassAbility_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetClassAbility();
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<ClassAbility>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<ClassAbilityRepository>();
                var actual = toTest.Get(expected.ClassAbility_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void ClassAbilitiesRepo_GetAllClassAbilities_ValidCall()
        {
            //Arrange
            List<ClassAbility> listofClassAbility = CreateTestData.GetListOfClassAbility();

            var mockSet = new Mock<DbSet<ClassAbility>>()
                .SetupData(listofClassAbility, o =>
                {
                    return listofClassAbility.Single(x => x.ClassAbility_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetListOfClassAbility();
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<ClassAbility>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<ClassAbilityRepository>();
                var actual = toTest.GetAll();

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void ClassAbilitiesRepo_GetAbilitiesOfClass_ValidCall()
        {
            //Arrange
            List<ClassAbility> listofClassAbility = CreateTestData.GetListOfClassAbility();

            var mockSet = new Mock<DbSet<ClassAbility>>()
                .SetupData(listofClassAbility, o =>
                {
                    return listofClassAbility.Single(x => x.ClassAbility_id.CompareTo(o.First()) == 0);
                });
            ClassAbility expected = CreateTestData.GetClassAbility();
            var notExpected1 = new ClassAbility
            {
                ClassAbility_id = Guid.Parse("19e51104-8590-4199-b7e2-079993bb8ccb"),
                Class_id = Guid.Parse("4e82620a-0496-4ecc-b6d4-05faa064310b"),
                Name = "Spell Master",
                Description = "Choose a 1st level spell and a 2nd level spell in your spellbook. As long as you have them prepared, you can cast them without consuming a spell slot.",
                LevelLearned = 18
            };
            var notExpected2 = new ClassAbility
            {
                ClassAbility_id = Guid.Parse("97bd8231-a001-4228-824f-7606202913b0"),
                Class_id = Guid.Parse("969c08ca-f983-4ddd-b351-31962f2429cd"),
                Name = "Vanish",
                Description = "You can Hide as a bonus action.",
                LevelLearned = 14
            };
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.ClassAbilities).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<ClassAbilityRepository>();
                var actual = toTest.GetAbilitiesOfClass(expected.Class_id);

                //Assert
                actual.Should().ContainEquivalentOf(expected);
                actual.Should().NotContain(notExpected1);
                actual.Should().NotContain(notExpected2);
                actual.Should().NotContainNulls();
                actual.Should().NotBeEmpty();
            }
        }
        [Test]
        public void ClassAbilitiesRepo_GetAbilitiesOfClassAtOrBelowLevel_ValidCall()
        {
            //Arrange
            List<ClassAbility> listofClassAbility = CreateTestData.GetListOfClassAbility();

            var mockSet = new Mock<DbSet<ClassAbility>>()
                .SetupData(listofClassAbility, o =>
                {
                    return listofClassAbility.Single(x => x.ClassAbility_id.CompareTo(o.First()) == 0);
                });
            ClassAbility expected = new ClassAbility
            {
                ClassAbility_id = Guid.Parse("97bd8231-a001-4228-824f-7606202913b0"),
                Class_id = Guid.Parse("969c08ca-f983-4ddd-b351-31962f2429cd"),
                Name = "Vanish",
                Description = "You can Hide as a bonus action.",
                LevelLearned = 14
            };
            var notExpected1 = CreateTestData.GetClassAbility();
            var notExpected2 = new ClassAbility
            {
                ClassAbility_id = Guid.Parse("19e51104-8590-4199-b7e2-079993bb8ccb"),
                Class_id = Guid.Parse("4e82620a-0496-4ecc-b6d4-05faa064310b"),
                Name = "Spell Master",
                Description = "Choose a 1st level spell and a 2nd level spell in your spellbook. As long as you have them prepared, you can cast them without consuming a spell slot.",
                LevelLearned = 18
            };

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.ClassAbilities).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<ClassAbilityRepository>();
                var actual = toTest.GetAbilitiesOfClassAtOrBelowLevel(expected.Class_id, 14);

                //Assert
                actual.Should().ContainEquivalentOf(expected);
                actual.Should().NotContain(notExpected1);
                actual.Should().NotContain(notExpected2);
                actual.Should().NotContainNulls();
                actual.Should().NotBeEmpty();
            }
        }
        [Test]
        public void ClassAbilitiesRepo_RemoveClassAbility_ValidCall()
        {
              //Arrange
        List<ClassAbility> listofClassAbility = CreateTestData.GetListOfClassAbility();

        var mockSet = new Mock<DbSet<ClassAbility>>()
            .SetupData(listofClassAbility, o =>
            {
                return listofClassAbility.Single(x => x.ClassAbility_id.CompareTo(o.First()) == 0);
            });
            var toBeDeleted = CreateTestData.GetClassAbility();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    //The argument for setup *MUST* be an exact match for the syntax we use in the implementation. Otherwise, the test will fail!
                    .Setup(x => x.Set<ClassAbility>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<ClassAbilityRepository>();
                toTest.Remove(toBeDeleted);
                var actual = toTest.GetAll();
                //Assert
                actual.Should().NotContain(toBeDeleted);
            }
        }

        //  //Arrange
        //List<TEntity> listofTEntity = CreateTestData.GetListOfTEntity();

        //var mockSet = new Mock<DbSet<TEntity>>()
        //    .SetupData(listofTEntity, o =>
        //    {
        //        return listofTEntity.Single(x => x.Class_id.CompareTo(o.First()) == 0);
        //    });

        //    using (var mockContext = AutoMock.GetLoose())
        //    {
        //         mockContext.Mock<TContext>()
        //             //The argument for setup *MUST* be an exact match for the syntax we use in the implementation. Otherwise, the test will fail!
        //             .Setup(x => x.Set<TEntity>()).Returns(mockSet.Object);

        //        //Act
        //        var toTest = mockContext.Create<TRepository>();

        //        //Assert
        //    }
    }
}
