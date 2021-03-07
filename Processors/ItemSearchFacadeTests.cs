using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Processors.Implementations.ItemsSearch;
using DnDProject.Entities.Character.ViewModels.PartialViewModels.Components;
using DnDProject.Entities.Items.DataModels;
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
    public class ItemSearchFacadeTests
    {
        [Test]
        public void ItemsSearchFacade_GetByNameContainingKnuckes_ReturnPagedList()
        {
            //Arrange
            List<Item> items = CreateTestData.GetListOfItems();
            var mockSet = new Mock<DbSet<Item>>()
                .SetupData(items, o =>
                {
                    return items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            List<foundItemCM> expectedList = new List<foundItemCM>();
            foundItemCM Knuckles = new foundItemCM
            {
                Item_id = Guid.Parse("026a7dff-5e85-4e6d-94c6-6613828e5df6"),
                Name = "Titanstone Knuckles"
            };
            expectedList.Add(Knuckles);
            IPagedList<foundItemCM> expected = expectedList.ToPagedList(1, 20);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(mockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Item>()).Returns(mockSet.Object);

                ItemsContext context = mockContext.Create<ItemsContext>();

                //Act
                ItemSearchFacade toTest = new ItemSearchFacade(context);
                var actual = toTest.searchItemsToPagedList("Knuckles", "Name", 1);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }


        }

        [Test]
        public void ItemsSearchFacade_GetByNameContainingKNUCKLES_ReturnPagedList()
        {
            //Arrange
            List<Item> items = CreateTestData.GetListOfItems();
            var mockSet = new Mock<DbSet<Item>>()
                .SetupData(items, o =>
                {
                    return items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            List<foundItemCM> expectedList = new List<foundItemCM>();
            foundItemCM Knuckles = new foundItemCM
            {
                Item_id = Guid.Parse("026a7dff-5e85-4e6d-94c6-6613828e5df6"),
                Name = "Titanstone Knuckles"
            };
            expectedList.Add(Knuckles);
            IPagedList<foundItemCM> expected = expectedList.ToPagedList(1, 20);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(mockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Item>()).Returns(mockSet.Object);

                ItemsContext context = mockContext.Create<ItemsContext>();

                //Act
                ItemSearchFacade toTest = new ItemSearchFacade(context);
                var actual = toTest.searchItemsToPagedList("KNUCKLES", "Name", 1);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void ItemsSearchFacade_GetByDescriptionContainingGauntlets_ReturnPagedList()
        {
            //Arrange
            List<Item> items = CreateTestData.GetListOfItems();
            var mockSet = new Mock<DbSet<Item>>()
                .SetupData(items, o =>
                {
                    return items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            List<foundItemCM> expectedList = new List<foundItemCM>();
            foundItemCM Knuckles = new foundItemCM
            {
                Item_id = Guid.Parse("026a7dff-5e85-4e6d-94c6-6613828e5df6"),
                Name = "Titanstone Knuckles"
            };
            expectedList.Add(Knuckles);
            IPagedList<foundItemCM> expected = expectedList.ToPagedList(1, 20);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(mockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Item>()).Returns(mockSet.Object);

                ItemsContext context = mockContext.Create<ItemsContext>();

                //Act
                ItemSearchFacade toTest = new ItemSearchFacade(context);
                var actual = toTest.searchItemsToPagedList("Gauntlets", "Description", 1);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void ItemsSearchFacade_GetByDescriptionContainingGAUNTLETS_ReturnPagedList()
        {
            //Arrange
            List<Item> items = CreateTestData.GetListOfItems();
            var mockSet = new Mock<DbSet<Item>>()
                .SetupData(items, o =>
                {
                    return items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            List<foundItemCM> expectedList = new List<foundItemCM>();
            foundItemCM Knuckles = new foundItemCM
            {
                Item_id = Guid.Parse("026a7dff-5e85-4e6d-94c6-6613828e5df6"),
                Name = "Titanstone Knuckles"
            };
            expectedList.Add(Knuckles);
            IPagedList<foundItemCM> expected = expectedList.ToPagedList(1, 20);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(mockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Item>()).Returns(mockSet.Object);

                ItemsContext context = mockContext.Create<ItemsContext>();

                //Act
                ItemSearchFacade toTest = new ItemSearchFacade(context);
                var actual = toTest.searchItemsToPagedList("GAUNTLETS", "Description", 1);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void ItemsSearchFacade_GetByDefaultContainingKnuckles_ReturnPagedList()
        {
            //Arrange
            List<Item> items = CreateTestData.GetListOfItems();
            var mockSet = new Mock<DbSet<Item>>()
                .SetupData(items, o =>
                {
                    return items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            List<foundItemCM> expectedList = new List<foundItemCM>();
            foundItemCM Knuckles = new foundItemCM
            {
                Item_id = Guid.Parse("026a7dff-5e85-4e6d-94c6-6613828e5df6"),
                Name = "Titanstone Knuckles"
            };
            expectedList.Add(Knuckles);
            IPagedList<foundItemCM> expected = expectedList.ToPagedList(1, 20);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(mockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Item>()).Returns(mockSet.Object);

                ItemsContext context = mockContext.Create<ItemsContext>();

                //Act
                ItemSearchFacade toTest = new ItemSearchFacade(context);
                var actual = toTest.searchItemsToPagedList("Knuckles", "erijoipergoipeiorpgoi", 1);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }

        }

        [Test]
        public void ItemsSearchFacade_GetByDefaultContainingKNUCKLES_ReturnPagedList()
        {
            //Arrange
            List<Item> items = CreateTestData.GetListOfItems();
            var mockSet = new Mock<DbSet<Item>>()
                .SetupData(items, o =>
                {
                    return items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            List<foundItemCM> expectedList = new List<foundItemCM>();
            foundItemCM Knuckles = new foundItemCM
            {
                Item_id = Guid.Parse("026a7dff-5e85-4e6d-94c6-6613828e5df6"),
                Name = "Titanstone Knuckles"
            };
            expectedList.Add(Knuckles);
            IPagedList<foundItemCM> expected = expectedList.ToPagedList(1, 20);

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(mockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Item>()).Returns(mockSet.Object);

                ItemsContext context = mockContext.Create<ItemsContext>();

                //Act
                ItemSearchFacade toTest = new ItemSearchFacade(context);
                var actual = toTest.searchItemsToPagedList("KNUCKLES", "erijoipergoipeiorpgoi", 1);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }

    }
}
