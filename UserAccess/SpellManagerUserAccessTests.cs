using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Repository.Implementations;
using DnDProject.Backend.Repository.Interfaces;
using DnDProject.Backend.Unit_Of_Work.Implementations;
using DnDProject.Backend.Unit_Of_Work.Interfaces;
using DnDProject.Backend.UserAccess.Interfaces;
using DnDProject.Entities.Spells.DataModels;
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
    public class SpellManagerUserAccessTests
    {
        [Test]
        public void SpellManagerUserAccess_AddSpell_ValidCall()
        {
            //Arrange
            List<Spell> spells = new List<Spell>();
            var mockSet = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleSpell();
                var id = expected.Spell_id;

                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork UoW = mockContext.Create<UnitOfWork>();
                ISpellManagerUserAccess toTest = UserAccessFactory.getSpellManagerUserAccess(UoW);
                toTest.AddSpell(expected);
                var actual = toTest.GetSpell(id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Spell>();
                expected.Should().BeOfType<Spell>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void SpellManagerUserAccess_RemoveSpell_ValidCall()
        {
            //Arrange

            List<Spell> spells = CreateTestData.GetListOfSpells();
            var mockSet = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                   .Setup(x => x.Set<Spell>()).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    //When a removal of a spell object is called, perform a callback to the charList collection, using the same spell object as an argument.
                    //This callback then fires, removing the object from the list.
                    .Setup(x => x.Set<Spell>().Remove(It.IsAny<Spell>()))
                        .Callback<Spell>((entity) => spells.Remove(entity));

                //Act
                IUnitOfWork UoW = mockContext.Create<UnitOfWork>();
                ISpellManagerUserAccess toTest = UserAccessFactory.getSpellManagerUserAccess(UoW);
                var toBeDeleted = CreateTestData.GetSampleSpell();
                toTest.RemoveSpell(toBeDeleted);
                var NotExpected = CreateTestData.GetSampleSpell();

                //Assert
                spells.Should().NotContain(toBeDeleted);
            }
        }

        [Test]
        public void SPellManagerUserAccess_AddSpellMaterials_ValidCall()
        {
            //Arrange
            List<Material> Materials = new List<Material>();
            var mockSet = new Mock<DbSet<Material>>()
                .SetupData(Materials, o =>
                {
                    return Materials.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleMaterial();
                var id = expected.Spell_id;

                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Materials).Returns(mockSet.Object);

                //Act
                IUnitOfWork UoW = mockContext.Create<UnitOfWork>();
                ISpellManagerUserAccess toTest = UserAccessFactory.getSpellManagerUserAccess(UoW);
                toTest.AddSpellMaterials(expected);
                var actual = toTest.GetSpellMaterials(expected.Spell_id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Material>();
                expected.Should().BeOfType<Material>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void SpellManagerUserAccess_DeleteSpellMaterials_ValidCall()
        {
            //Arrange
            List<Material> Materials = CreateTestData.GetListOfMaterials();
            var mockSet = new Mock<DbSet<Material>>()
                .SetupData(Materials, o =>
                {
                    return Materials.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Materials).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Materials.Remove(It.IsAny<Material>()))
                        .Callback<Material>((entity) => Materials.Remove(entity));

                var toBeDeleted = CreateTestData.GetSampleMaterial();

                //act
                IUnitOfWork UoW = mockContext.Create<UnitOfWork>();
                ISpellManagerUserAccess toTest = UserAccessFactory.getSpellManagerUserAccess(UoW);
                toTest.DeleteSpellMaterials(toBeDeleted);

                //Assert
                Materials.Should().NotContain(toBeDeleted);
            }
        }
        [Test]
        public void SpellManagerUserAccess_DeleteSpellMaterialsById_ValidCall()
        {
            //Arrange
            List<Material> Materials = CreateTestData.GetListOfMaterials();
            var mockSet = new Mock<DbSet<Material>>()
                .SetupData(Materials, o =>
                {
                    return Materials.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Materials).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Materials.Remove(It.IsAny<Material>()))
                        .Callback<Material>((entity) => Materials.Remove(entity));

                var toBeDeleted = CreateTestData.GetSampleMaterial();

                //act
                IUnitOfWork UoW = mockContext.Create<UnitOfWork>();
                ISpellManagerUserAccess toTest = UserAccessFactory.getSpellManagerUserAccess(UoW);
                toTest.DeleteSpellMaterialsById(toBeDeleted.Spell_id);

                //Assert
                Materials.Should().NotContain(toBeDeleted);
            }
        }
    }
}
