using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Processors.Implementations.SpellsSearch;
using DnDProject.Backend.Processors.Implementations.SpellsSearch.Filters;
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

namespace DnDProject.UnitTests.Processors._character
{
    [TestFixture]
    public class SpellSearchTests
    {
        [Test]
        public void SpellsSearch_NameContainsFIlter_NameContainsTower()
        {
            //Arrange
            List<Spell> spells = CreateTestData.GetListOfSpells();
            var mockSet = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            List<Spell> expected = new List<Spell>();
            Spell Tower = new Spell
            {
                Spell_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f"),
                Name = "Widogast's Nascent Nine-sided Tower",
                Description = "A flavored Magnificent Mansion",
                Level = 7,
                School_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                CastingTime = "1 minute",
                Range = "100 feet",
                Duration = "24 hours",
                RequiresVerbal = true,
                RequiresSomantic = true,
                RequiresMaterial = false,
                RequiresConcentration = true
            };
            expected.Add(Tower);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(mockSet.Object);

                SpellsContext context = mockContext.Create<SpellsContext>();

                //Act
                SpellSearchToDecorate baseObject = new SpellSearchToDecorate(context);
                NameContains toTest = new NameContains("Tower");
                toTest.setToBeDecorated(baseObject);
                var actual = toTest.GetSpells().ToList();

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }

        }
        [Test]
        public void SpellsSearch_DescriptionContainsFilter_DescriptionContainsMansion()
        {
            //Arrange
            List<Spell> spells = CreateTestData.GetListOfSpells();
            var mockSet = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            List<Spell> expected = new List<Spell>();
            Spell Tower = new Spell
            {
                Spell_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f"),
                Name = "Widogast's Nascent Nine-sided Tower",
                Description = "A flavored Magnificent Mansion",
                Level = 7,
                School_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                CastingTime = "1 minute",
                Range = "100 feet",
                Duration = "24 hours",
                RequiresVerbal = true,
                RequiresSomantic = true,
                RequiresMaterial = false,
                RequiresConcentration = true
            };
            expected.Add(Tower);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(mockSet.Object);

                SpellsContext context = mockContext.Create<SpellsContext>();

                //Act
                SpellSearchToDecorate baseObject = new SpellSearchToDecorate(context);
                DescriptionContains toTest = new DescriptionContains("Mansion");
                toTest.setToBeDecorated(baseObject);
                var actual = toTest.GetSpells().ToList();

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
    }
}
