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
    public class SubclassesRepoTests
    {
        [Test]
        public void SubclassRepo_AddSubclass_ValidCall()
        {

            //Arrange
            List<Subclass> listofSubclass = new List<Subclass>();

            var mockSet = new Mock<DbSet<Subclass>>()
                .SetupData(listofSubclass, o =>
                {
                    return listofSubclass.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetSubclass();
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<Subclass>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<SubclassRepository>();
                toTest.Add(expected);
                //Assert
                listofSubclass.Should().ContainEquivalentOf(expected);
            }
        }
        [Test]
        public void SubclassRepo_AddSubclasses_ValidCall()
        {
            //Arrange
            List<Subclass> listofSubclass = new List<Subclass>();

            var mockSet = new Mock<DbSet<Subclass>>()
                .SetupData(listofSubclass, o =>
                {
                    return listofSubclass.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetListOfSubclass();
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<Subclass>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<SubclassRepository>();
                toTest.AddRange(expected);
                //Assert
                listofSubclass.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void SubclassRepo_CharacterOfClassLearnsSubclass_ValidCall()
        {
            //Arrange
            List<Character_Class_Subclass> knownClasses = new List<Character_Class_Subclass>();
            Character_Class_Subclass Caleb_Wizard_Blank = new Character_Class_Subclass
            {
                Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                Class_id = Guid.Parse("4e82620a-0496-4ecc-b6d4-05faa064310b"),
                RemainingHitDice = 12,
                ClassLevel = 12
            };
            knownClasses.Add(Caleb_Wizard_Blank);
            var mockSet = new Mock<DbSet<Character_Class_Subclass>>()
                .SetupData(knownClasses, o =>
                {
                    return knownClasses.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetCharacter_Class_Subclass();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.KnownClasses).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<SubclassRepository>();
                toTest.CharacterOfClassLearnsSubclass(
                    Caleb_Wizard_Blank.Character_id,
                    Caleb_Wizard_Blank.Class_id,
                    Guid.Parse("c8d2e23a-a193-4e06-8814-9180d4830732"));

                var actual = knownClasses.First();

                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Character_Class_Subclass>();
                expected.Should().BeOfType<Character_Class_Subclass>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void SubclassRepo_GetSubclass_ValidCall()
        {
            //Arrange
            List<Subclass> listofSubclass = CreateTestData.GetListOfSubclass();

            var mockSet = new Mock<DbSet<Subclass>>()
                .SetupData(listofSubclass, o =>
                {
                    return listofSubclass.Single(x => x.Subclass_id.CompareTo(o.First()) == 0);
                });
            var expected = CreateTestData.GetSubclass();
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<Subclass>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<SubclassRepository>();
                var actual = toTest.Get(expected.Subclass_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void SubclassRepo_GetAllSubclassesForClass_ValidCall()
        {
            //Arrange
            List<Subclass> subclasses = CreateTestData.GetListOfSubclass();
            var subclassesMockSet = new Mock<DbSet<Subclass>>()
                .SetupData(subclasses, o =>
                {
                    return subclasses.Single(x => x.Subclass_id.CompareTo(o.First()) == 0);
                });

            List<Subclass> expected = new List<Subclass>();
            Subclass gunslinger = CreateTestData.GetSubclass();
            expected.Add(gunslinger);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Subclasses).Returns(subclassesMockSet.Object);

                //Act
                var toTest = mockContext.Create<SubclassRepository>();
                var actual = toTest.GetAllSubclassesForClass(gunslinger.Class_id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<List<Subclass>>();
                expected.Should().BeOfType<List<Subclass>>();
                actual.Should().BeEquivalentTo(expected);

            }

        }

        [Test]
        public void SubclassRepo_CharacterOfClassForgetsSubclass_ValidCall()
        {
            //Arrange
            List<Character_Class_Subclass> knownClasses = CreateTestData.GetListOfCharacter_Class_Subclass();
            var mockSet = new Mock<DbSet<Character_Class_Subclass>>()
               .SetupData(knownClasses, o =>
               {
                   return knownClasses.Single(x => x.Character_id.CompareTo(o.First()) == 0);
               });
            Character_Class_Subclass expected= new Character_Class_Subclass
            {
                Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                Class_id = Guid.Parse("4e82620a-0496-4ecc-b6d4-05faa064310b"),
                RemainingHitDice = 12,
                ClassLevel = 12
            };

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.KnownClasses).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<SubclassRepository>();
                toTest.CharacterForgetsSubclassOfClass(
                    expected.Character_id,
                    expected.Class_id,
                    Guid.Parse("c8d2e23a-a193-4e06-8814-9180d4830732"));

                var actual = knownClasses.First();

                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Character_Class_Subclass>();
                expected.Should().BeOfType<Character_Class_Subclass>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void SubclassRepo_RemoveSubclass_ValidCall()
        {

            //Arrange
            List<Subclass> listofSubclass = CreateTestData.GetListOfSubclass();

            var mockSet = new Mock<DbSet<Subclass>>()
                .SetupData(listofSubclass, o =>
                {
                    return listofSubclass.Single(x => x.Subclass_id.CompareTo(o.First()) == 0);
                });
            var toBeDeleted = CreateTestData.GetSubclass();
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<PlayableClassContext>()
                    .Setup(x => x.Set<Subclass>()).Returns(mockSet.Object);

                //Act
                var toTest = mockContext.Create<SubclassRepository>();
                toTest.Remove(toBeDeleted);

                //Assert
                listofSubclass.Should().NotContain(toBeDeleted);
            }
        }


        //  //Arrange
        //List<TEntity> listofTEntity = CreateTestData.GetListOfTEntity();

        //var mockSet = new Mock<DbSet<TEntity>>()
        //    .SetupData(listofTEntity, o =>
        //    {
        //        return listofTEntity.Single(x => x.Subclass_id.CompareTo(o.First()) == 0);
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
