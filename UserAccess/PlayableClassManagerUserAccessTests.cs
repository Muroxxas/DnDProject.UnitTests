using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Unit_Of_Work.Implementations;
using DnDProject.Backend.Unit_Of_Work.Interfaces;
using DnDProject.Backend.UserAccess.Interfaces;
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

namespace DnDProject.UnitTests.UserAccess
{
    [TestFixture]
    public class PlayableClassManagerUserAccessTests
    {
        [Test]
        public void PlayableClassManager_AddPlayableClass_ValidCall()
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
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IClassManagerUserAccess toTest = UserAccessFactory.GetClassManagerUserAccess(worker);
                toTest.AddPlayableClass(expected);
                var actual = toTest.GetPlayableClass(expected.Class_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void PlayableClassManager_RemoveClass_ValidCall()
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
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IClassManagerUserAccess toTest = UserAccessFactory.GetClassManagerUserAccess(worker);
                toTest.RemovePlayableClass(toBeDeleted);

                //Assert
                listofPlayableClass.Should().NotContain(toBeDeleted);

            }
        }

        [Test]
        public void PlayableClassManager_AddAbility_ValidCall()
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
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IClassManagerUserAccess toTest = UserAccessFactory.GetClassManagerUserAccess(worker);
                toTest.AddAbility(expected);

                //Assert
                listofClassAbility.Should().ContainEquivalentOf(expected);
            }
        }
        [Test]
        public void PlayableClassManager_AddAbilities_ValidCall()
        {
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
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IClassManagerUserAccess toTest = UserAccessFactory.GetClassManagerUserAccess(worker);
                toTest.AddAbilities(expected);


                //Assert
                listofClassAbility.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void PlayableClassManager_GetAllClassAbilities_ValidCall()
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
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IClassManagerUserAccess toTest = UserAccessFactory.GetClassManagerUserAccess(worker);
                var actual = toTest.GetAllClassAbilities();

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void PlayableClassManager_RemoveClassAbility_ValidCall()
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
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IClassManagerUserAccess toTest = UserAccessFactory.GetClassManagerUserAccess(worker);
                toTest.RemoveClassAbility(toBeDeleted);

                //Assert
                listofClassAbility.Should().NotContain(toBeDeleted);
            }
        }

        [Test]
        public void PlayableClassManager_AddSubclass_ValidCall()
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
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IClassManagerUserAccess toTest = UserAccessFactory.GetClassManagerUserAccess(worker);
                toTest.AddSubclass(expected);
                //Assert
                listofSubclass.Should().ContainEquivalentOf(expected);
            }
        }
        [Test]
        public void PlayableClassManager_AddSubclasses_ValidCall()
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
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IClassManagerUserAccess toTest = UserAccessFactory.GetClassManagerUserAccess(worker);
                toTest.AddSubclasses(expected);

                //Assert
                listofSubclass.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void PlayableClassManager_RemoveSubclass_ValidCall()
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
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IClassManagerUserAccess toTest = UserAccessFactory.GetClassManagerUserAccess(worker);
                toTest.RemoveSubclass(toBeDeleted);

                //Assert
                listofSubclass.Should().NotContain(toBeDeleted);
            }
        }

        [Test]
        public void PlayableClassManager_AddAbilityForSubclass_ValidCall()
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
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IClassManagerUserAccess toTest = UserAccessFactory.GetClassManagerUserAccess(worker);
                toTest.AddSubclassAbility(expected);

                //Assert
                listofSubclassAbility.Should().ContainEquivalentOf(expected);
            }
        }
        [Test]
        public void PlayableClassManager_AddAbilitiesForSubclass_ValidCall()
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
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IClassManagerUserAccess toTest = UserAccessFactory.GetClassManagerUserAccess(worker);
                toTest.AddSubclassAbilities(expected);

                //Assert
                listofSubclassAbility.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void PlayableClassManager_RemoveSubclassAbility_ValidCall()
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
                IUnitOfWork worker = mockContext.Create<UnitOfWork>();
                IClassManagerUserAccess toTest = UserAccessFactory.GetClassManagerUserAccess(worker);

                toTest.RemoveSubclassAbility(notExpected);

                //Assert
                listofSubclassAbility.Should().NotContain(notExpected);
            }
        }
    }
}
