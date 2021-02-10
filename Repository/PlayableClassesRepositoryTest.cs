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
    public class PlayableClassesRepositoryTest
    {
        [Test]
        public void PlayableClassRepository_AddPlayableClass_ValidCall()
        {
            //Arrange
            List<PlayableClass> listofPlayableClass = new List<PlayableClass>();

            var mockSet = new Mock<DbSet<PlayableClass>>()
                .SetupData(listofPlayableClass, o =>
                {
                    return listofPlayableClass.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetSampleClass();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<PlayableClass>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<PlayableClassRepository>();
                toTest.Add(expected);
                var actual = toTest.Get(expected.Class_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void PlayableClassRepository_CharacterLearnsClass_ValidCall()
        {
            //Arrange
            List<Character_Class_Subclass> listofCharacter_Class_Subclass = new List<Character_Class_Subclass>();

            var mockSet = new Mock<DbSet<Character_Class_Subclass>>()
                .SetupData(listofCharacter_Class_Subclass, o =>
                {
                    return listofCharacter_Class_Subclass.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var Percy_id = Guid.Parse("6983e8dc-3e3c-4853-ac49-ba33f236723a");
            var Fighter_id = Guid.Parse("15478d70-f96e-4c14-aeaf-4a1e35605874");
            var expected = new Character_Class_Subclass
            {
                Character_id = Percy_id,
                Class_id = Fighter_id,
                ClassLevel = 1,
                RemainingHitDice = 1
            };

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.KnownClasses).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<PlayableClassRepository>();
                toTest.CharacterLearnsClass(Percy_id, Fighter_id);
                //Assert
                listofCharacter_Class_Subclass.Should().ContainEquivalentOf(expected);
            }
        }
        [Test]
        public void PlayableClassRepository_CharacterLearnsClasses_ValidCall()
        {
            //Arrange
            List<Character_Class_Subclass> listofCharacter_Class_Subclass = new List<Character_Class_Subclass>();

            var mockSet = new Mock<DbSet<Character_Class_Subclass>>()
                .SetupData(listofCharacter_Class_Subclass, o =>
                {
                    return listofCharacter_Class_Subclass.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var Percy_id = Guid.Parse("6983e8dc-3e3c-4853-ac49-ba33f236723a");
            var learnedClasses = new List<Guid>();
            var Fighter_id = Guid.Parse("15478d70-f96e-4c14-aeaf-4a1e35605874");
            learnedClasses.Add(Fighter_id);
            var Ranger_id = Guid.Parse("da7d6227-d330-44ab-8001-880dbf52110a");
            learnedClasses.Add(Ranger_id);

            var expected = new List<Character_Class_Subclass>();
            var Percy_Fighter = new Character_Class_Subclass
            {
                Character_id = Percy_id,
                Class_id = Fighter_id,
                ClassLevel = 1,
                RemainingHitDice = 1
            };
            expected.Add(Percy_Fighter);
            var Percy_Ranger = new Character_Class_Subclass
            {
                Character_id = Percy_id,
                Class_id = Ranger_id,
                ClassLevel = 1,
                RemainingHitDice = 1
            };
            expected.Add(Percy_Ranger);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.KnownClasses).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<PlayableClassRepository>();
                toTest.CharacterLearnsClasses(Percy_id, learnedClasses);
                //Assert
                listofCharacter_Class_Subclass.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void PlayableClassRepository_GetAllPlayableClasses_ValidCall()
        {
            //Arrange
            List<PlayableClass> listofPlayableClass = CreateTestData.GetPlayableClasses();

            var mockSet = new Mock<DbSet<PlayableClass>>()
                .SetupData(listofPlayableClass, o =>
                {
                    return listofPlayableClass.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetPlayableClasses();
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<PlayableClass>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<PlayableClassRepository>();

                var actual = toTest.GetAll();

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void PlayableClassRepository_GetPlayableClass_ValidCall()
        {
            //Arrange

            List<PlayableClass> playableClasses = CreateTestData.GetPlayableClasses();

            var mockSet = new Mock<DbSet<PlayableClass>>()
                .SetupData(playableClasses, o =>
                {
                    return playableClasses.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetSampleClass();
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<PlayableClass>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<PlayableClassRepository>();
                var actual = toTest.Get(expected.Class_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void PlayableClassRepository_GetClassesOfCharacter_ValidCall()
        {
            //Arrange
            List<PlayableClass> playableClasses = CreateTestData.GetPlayableClasses();
            List<Character_Class_Subclass> knownclasses = CreateTestData.GetListOfCharacter_Class_Subclass();

            var classMockSet = new Mock<DbSet<PlayableClass>>()
                .SetupData(playableClasses, o =>
                {
                    return playableClasses.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var knownClassesMockSet = new Mock<DbSet<Character_Class_Subclass>>()
                .SetupData(knownclasses, o =>
                {
                    return knownclasses.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            var Percy_id = Guid.Parse("6983e8dc-3e3c-4853-ac49-ba33f236723a");
            var expected = CreateTestData.GetSampleClass();
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Classes).Returns(classMockSet.Object);
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.KnownClasses).Returns(knownClassesMockSet.Object);

                //Act
                var toTest = mockContext.Create<PlayableClassRepository>();
                var actual = toTest.GetClassesOfCharacter(Percy_id);

                //Assert
                actual.Should().ContainEquivalentOf(expected);

            }
        }
        [Test]
        public void PlayableClassRepository_GetKnownClassRecordOfCharacterAndClass_ValidCall()
        {
            //Arrange
            List<Character_Class_Subclass> listofCharacter_Class_Subclass = CreateTestData.GetListOfCharacter_Class_Subclass();

            var mockSet = new Mock<DbSet<Character_Class_Subclass>>()
                .SetupData(listofCharacter_Class_Subclass, o =>
                {
                    return listofCharacter_Class_Subclass.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var expected = new Character_Class_Subclass
            {
                Character_id = Guid.Parse("da7d6227-d330-44ab-8001-880dbf52110a"),
                Class_id = Guid.Parse("969c08ca-f983-4ddd-b351-31962f2429cd"),
                Subclass_id = Guid.Parse("c7de67ae-3a65-4261-9c09-05a7b0c527bb"),
                RemainingHitDice = 20,
                ClassLevel = 20
            };
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.KnownClasses).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<PlayableClassRepository>();
                var actual = toTest.GetKnownClassRecordOfCharacterAndClass(expected.Character_id, expected.Class_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void PlayableClassRepository_GetAllKnownClassRecordsOfCharacter_ValidCall()
        {
            //Arrange
            List<Character_Class_Subclass> listofCharacter_Class_Subclass = CreateTestData.GetListOfCharacter_Class_Subclass();

            var mockSet = new Mock<DbSet<Character_Class_Subclass>>()
                .SetupData(listofCharacter_Class_Subclass, o =>
                {
                    return listofCharacter_Class_Subclass.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var expected = new Character_Class_Subclass
            {
                Character_id = Guid.Parse("da7d6227-d330-44ab-8001-880dbf52110a"),
                Class_id = Guid.Parse("969c08ca-f983-4ddd-b351-31962f2429cd"),
                Subclass_id = Guid.Parse("c7de67ae-3a65-4261-9c09-05a7b0c527bb"),
                RemainingHitDice = 20,
                ClassLevel = 20
            };
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.KnownClasses).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<PlayableClassRepository>();
                var actual = toTest.GetAllKnownClassRecordsOfCharacter(expected.Character_id);

                //Assert
                actual.Should().ContainEquivalentOf(expected);
            }
        }
        [Test]
        public void PlayableClassRepository_CharacterForgetsClass_ValidCall()
        {
            //Arrange
            List<Character_Class_Subclass> listofCharacter_Class_Subclass = CreateTestData.GetListOfCharacter_Class_Subclass();

            var mockSet = new Mock<DbSet<Character_Class_Subclass>>()
                .SetupData(listofCharacter_Class_Subclass, o =>
                {
                    return listofCharacter_Class_Subclass.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var toBeDeleted = new Character_Class_Subclass
            {
                Character_id = Guid.Parse("6983e8dc-3e3c-4853-ac49-ba33f236723a"),
                Class_id = Guid.Parse("15478d70-f96e-4c14-aeaf-4a1e35605874")
            };
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.KnownClasses).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<PlayableClassRepository>();
                toTest.CharacterForgetsClass(toBeDeleted.Character_id, toBeDeleted.Class_id);

                //Assert
                listofCharacter_Class_Subclass.Should().NotContain(toBeDeleted);
            }
        }
        [Test]
        public void PlayableClassRepository_RemoveClass_ValidCall()
        {
            //Arrange
            List<PlayableClass> listofPlayableClass = CreateTestData.GetPlayableClasses();

            var mockSet = new Mock<DbSet<PlayableClass>>()
                .SetupData(listofPlayableClass, o =>
                {
                    return listofPlayableClass.Single(x => x.Class_id.CompareTo(o.First()) == 0);
               });
            var toBeDeleted = CreateTestData.GetSampleClass();
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<PlayableClass>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<PlayableClassRepository>();
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
        //      mockContext.Mock<TContext>()
        //          .Setup(x => x.Set<TEntity>()).Returns(mockSet.Object);

        //      //Act
        //      var toTest = mockContext.Create<TRepository>();

        //      //Assert
        //    }
    }
}
