using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Processors.Implementations.ItemsSearch;
using DnDProject.Backend.Processors.Implementations.ItemsSearch.Filters;
using DnDProject.Entities.Items.DataModels;
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
    public class ItemSearchTests
    {
        [Test]
        public void ItemSearch_NameContainsFilter_NameContainsKnuckles()
        {
            //Arrange
            List<Item> items = CreateTestData.GetListOfItems();
            var mockSet = new Mock<DbSet<Item>>()
                .SetupData(items, o =>
                {
                    return items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            List<Item> expected = new List<Item>();
            Item TitanstoneKnuckles = new Item
            {
                Item_id = Guid.Parse("026a7dff-5e85-4e6d-94c6-6613828e5df6"),
                Name = "Titanstone Knuckles",
                Description = "Gauntlets fashioned from the Titan of Stone, enhancing your strength to rival that of the gods.",
                isEquippable = true,
                isConsumable = false,
                requiresAttunement = true,
                Value = 999
            };
            expected.Add(TitanstoneKnuckles);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(mockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Item>()).Returns(mockSet.Object);


                ItemsContext context = mockContext.Create<ItemsContext>();

                //Act
                ItemSearchToDecorate baseObject = new ItemSearchToDecorate(context);
                NameContains toTest = new NameContains("Knuckles");
                toTest.setToBeDecorated(baseObject);
                var actual = toTest.GetItems().ToList();

                //Assert
                actual.Should().BeEquivalentTo(expected);
                
            }

        }

        [Test]
        public void ItemsSearch_DescriptionContainsFilter_DescriptionContainsGauntlets()
        {
            //Arrange
            List<Item> items = CreateTestData.GetListOfItems();
            var mockSet = new Mock<DbSet<Item>>()
                .SetupData(items, o =>
                {
                    return items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            List<Item> expected = new List<Item>();
            Item TitanstoneKnuckles = new Item
            {
                Item_id = Guid.Parse("026a7dff-5e85-4e6d-94c6-6613828e5df6"),
                Name = "Titanstone Knuckles",
                Description = "Gauntlets fashioned from the Titan of Stone, enhancing your strength to rival that of the gods.",
                isEquippable = true,
                isConsumable = false,
                requiresAttunement = true,
                Value = 999
            };
            expected.Add(TitanstoneKnuckles);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(mockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Item>()).Returns(mockSet.Object);


                ItemsContext context = mockContext.Create<ItemsContext>();

                //Act
                ItemSearchToDecorate baseObject = new ItemSearchToDecorate(context);
                DescriptionContains toTest = new DescriptionContains("Gauntlets");
                toTest.setToBeDecorated(baseObject);
                var actual = toTest.GetItems().ToList();

                //Assert
                actual.Should().BeEquivalentTo(expected);

            }
        }
    }
}
