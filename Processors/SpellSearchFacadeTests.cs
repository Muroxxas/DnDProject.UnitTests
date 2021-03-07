using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Processors.Implementations.SpellsSearch;
using DnDProject.Entities.Character.ViewModels.PartialViewModels.Components;
using DnDProject.Entities.Spells.DataModels;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDProject.UnitTests.Processors
{
    [TestFixture]
    public class SpellSearchFacadeTests
    {
        [Test]
        public void SpellSearchFacade_GetByNameContainingTower_ReturnPagedList()
        {
            //Arrange
            List<Spell> spells = CreateTestData.GetListOfSpells();
            var mockSet = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            List<foundSpellCM> expectedList = new List<foundSpellCM>();
            foundSpellCM tower = new foundSpellCM
            {
                Spell_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f"),
                Name = "Widogast's Nascent Nine-sided Tower"
            };
            expectedList.Add(tower);
            IPagedList<foundSpellCM> expected = expectedList.ToPagedList(1, 20);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(mockSet.Object);

                SpellsContext context = mockContext.Create<SpellsContext>();

                //Act
                var toTest = new SpellSearchFacade(context);
                var actual = toTest.searchSpellsToPagedList("Tower", "Name", 1);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void SpellSearchFacade_GetByNameContainingTOWER_ReturnPagedList()
        {
            //ensure that capitalization does not effect the search result.
            //Arrange
            List<Spell> spells = CreateTestData.GetListOfSpells();
            var mockSet = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            List<foundSpellCM> expectedList = new List<foundSpellCM>();
            foundSpellCM tower = new foundSpellCM
            {
                Spell_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f"),
                Name = "Widogast's Nascent Nine-sided Tower"
            };
            expectedList.Add(tower);
            IPagedList<foundSpellCM> expected = expectedList.ToPagedList(1, 20);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(mockSet.Object);

                SpellsContext context = mockContext.Create<SpellsContext>();

                //Act
                var toTest = new SpellSearchFacade(context);
                var actual = toTest.searchSpellsToPagedList("TOWER", "Name", 1);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void SpellSearchFacade_GetByDescriptionContainingMansion_ReturnPagedList()
        {
            //Arrange
            List<Spell> spells = CreateTestData.GetListOfSpells();
            var mockSet = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            List<foundSpellCM> expectedList = new List<foundSpellCM>();
            foundSpellCM tower = new foundSpellCM
            {
                Spell_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f"),
                Name = "Widogast's Nascent Nine-sided Tower"
            };
            expectedList.Add(tower);
            IPagedList<foundSpellCM> expected = expectedList.ToPagedList(1, 20);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(mockSet.Object);

                SpellsContext context = mockContext.Create<SpellsContext>();

                //Act
                var toTest = new SpellSearchFacade(context);
                var actual = toTest.searchSpellsToPagedList("Mansion", "Description", 1);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void SpellSearchFacade_GetByDescriptionContainingMANSION_ReturnPagedList()
        {
            //Arrange
            List<Spell> spells = CreateTestData.GetListOfSpells();
            var mockSet = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            List<foundSpellCM> expectedList = new List<foundSpellCM>();
            foundSpellCM tower = new foundSpellCM
            {
                Spell_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f"),
                Name = "Widogast's Nascent Nine-sided Tower"
            };
            expectedList.Add(tower);
            IPagedList<foundSpellCM> expected = expectedList.ToPagedList(1, 20);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(mockSet.Object);

                SpellsContext context = mockContext.Create<SpellsContext>();

                //Act
                var toTest = new SpellSearchFacade(context);
                var actual = toTest.searchSpellsToPagedList("MANSION", "Description", 1);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void SpellSearchFacade_GetByDefaultContainingTower_ReturnPagedList()
        {
            //Arrange
            List<Spell> spells = CreateTestData.GetListOfSpells();
            var mockSet = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            List<foundSpellCM> expectedList = new List<foundSpellCM>();
            foundSpellCM tower = new foundSpellCM
            {
                Spell_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f"),
                Name = "Widogast's Nascent Nine-sided Tower"
            };
            expectedList.Add(tower);
            IPagedList<foundSpellCM> expected = expectedList.ToPagedList(1, 20);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(mockSet.Object);

                SpellsContext context = mockContext.Create<SpellsContext>();

                //Act
                var toTest = new SpellSearchFacade(context);
                var actual = toTest.searchSpellsToPagedList("Tower", "kjokihpjoiehjkietkjlhth", 1);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void SpellSearchFacade_GetByDefaultContainingTOWER_ReturnPagedList()
        {
            //Arrange
            List<Spell> spells = CreateTestData.GetListOfSpells();
            var mockSet = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            List<foundSpellCM> expectedList = new List<foundSpellCM>();
            foundSpellCM tower = new foundSpellCM
            {
                Spell_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f"),
                Name = "Widogast's Nascent Nine-sided Tower"
            };
            expectedList.Add(tower);
            IPagedList<foundSpellCM> expected = expectedList.ToPagedList(1, 20);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(mockSet.Object);

                SpellsContext context = mockContext.Create<SpellsContext>();

                //Act
                var toTest = new SpellSearchFacade(context);
                var actual = toTest.searchSpellsToPagedList("TOWER", "kjokihpjoiehjkietkjlhth", 1);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
    }
}
