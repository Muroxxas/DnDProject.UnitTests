using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Repository.Implementations;
using DnDProject.Backend.Repository.Interfaces;
using DnDProject.Backend.Unit_Of_Work.Implementations;
using DnDProject.Backend.Unit_Of_Work.Interfaces;
using DnDProject.Backend.UserAccess.Interfaces;
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

namespace DnDProject.UnitTests.UserAccess
{
    [TestFixture]
    public class ItemManagerUserAccessTests
    {
        [Test]
        public void ItemManagerUserAccess_AddItem_ValidCall()
        {
            //Arrange
            List<Item> items = new List<Item>();
            var mockSet = new Mock<DbSet<Item>>()
                .SetupData(items, o =>
                {
                    return items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleItem();
                var id = expected.Item_id;

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Item>()).Returns(mockSet.Object);

                //Act
                IUnitOfWork UoW = mockContext.Create<UnitOfWork>();
                IItemsManagerUserAccess toTest = UserAccessFactory.getItemsManagerUserAccess(UoW);
                toTest.AddItem(expected);
                var actual = toTest.GetItem(id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Item>();
                expected.Should().BeOfType<Item>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void ItemManagerUserAccess_SettAllTagsForItem_ValidCall()
        {
            //Arrange
            List<Item_Tag> itemTags = new List<Item_Tag>();
            List<Tag> tags = CreateTestData.GetListOfTags();
            var ITmockSet = new Mock<DbSet<Item_Tag>>()
               .SetupData(itemTags, o =>
               {
                   return itemTags.Single(x => x.Item_id.CompareTo(o.First()) == 0);
               });
            var tagsMockSet = new Mock<DbSet<Tag>>()
                .SetupData(tags, o =>
                {
                    return tags.Single(x => x.Tag_id.CompareTo(o.First()) == 0);
                });
            //Whisper is assigned all tags!
            List<Tag> expected = CreateTestData.GetListOfTags();
            List<Guid> tag_ids = new List<Guid>();
            foreach (Tag tag in expected)
            {
                tag_ids.Add(tag.Tag_id);
            }

            Guid whisper_id = Guid.Parse("709135c3-6f89-46cb-80ae-4097b621e3b0");

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Tags).Returns(tagsMockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Item_Tags).Returns(ITmockSet.Object);

                //Act
                //Act
                IUnitOfWork UoW = mockContext.Create<UnitOfWork>();
                IItemsManagerUserAccess toTest = UserAccessFactory.getItemsManagerUserAccess(UoW);
                toTest.SetAllTagsForItem(whisper_id, tag_ids);
                var actual = toTest.GetTagsForItem(whisper_id);


                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void ItemManagerUserAccess_SetTagForItem_ValidCall()
        {
            //Arrange
            List<Item_Tag> itemTags = new List<Item_Tag>();
            List<Tag> tags = CreateTestData.GetListOfTags();
            var ITmockSet = new Mock<DbSet<Item_Tag>>()
                .SetupData(itemTags, o =>
                {
                    return itemTags.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });
            var tagsMockSet = new Mock<DbSet<Tag>>()
                .SetupData(tags, o =>
                {
                    return tags.Single(x => x.Tag_id.CompareTo(o.First()) == 0);
                });

            List<Tag> expected = new List<Tag>();
            Tag weapon = new Tag
            {
                Tag_id = Guid.Parse("172e8478-e1bd-49ba-a7a7-6455d5a58c6e"),
                TagName = "Weapon"
            };
            expected.Add(weapon);
            Guid whisper_id = Guid.Parse("709135c3-6f89-46cb-80ae-4097b621e3b0");
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Tags).Returns(tagsMockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Item_Tags).Returns(ITmockSet.Object);

                //Act
                IUnitOfWork UoW = mockContext.Create<UnitOfWork>();
                IItemsManagerUserAccess toTest = UserAccessFactory.getItemsManagerUserAccess(UoW);
                toTest.SetTagForItem(whisper_id, weapon.Tag_id);
                var actual = toTest.GetTagsForItem(whisper_id);


                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void ItemManagerUserAccess_RemoveItem_ValidCall()
        {
            //Arrange

            List<Item> Items = CreateTestData.GetListOfItems();
            var mockSet = new Mock<DbSet<Item>>()
                .SetupData(Items, o =>
                {
                    return Items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                   .Setup(x => x.Set<Item>()).Returns(mockSet.Object);
                mockContext.Mock<ItemsContext>()
                    //When a removal of a Item object is called, perform a callback to the charList collection, using the same Item object as an argument.
                    //This callback then fires, removing the object from the list.
                    .Setup(x => x.Set<Item>().Remove(It.IsAny<Item>()))
                        .Callback<Item>((entity) => Items.Remove(entity));

                //Act
                IUnitOfWork UoW = mockContext.Create<UnitOfWork>();
                IItemsManagerUserAccess toTest = UserAccessFactory.getItemsManagerUserAccess(UoW);
                var toBeDeleted = CreateTestData.GetSampleItem();
                toTest.RemoveItem(toBeDeleted);
                var NotExpected = CreateTestData.GetSampleItem();

                //Assert
                Items.Should().NotContain(toBeDeleted);
            }
        }

        [Test]
        public void ItemManagerUserAccess_RemoveItemByID_ValidCall()
        {
            //Arrange

            List<Item> Items = CreateTestData.GetListOfItems();
            var mockSet = new Mock<DbSet<Item>>()
                .SetupData(Items, o =>
                {
                    return Items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                   .Setup(x => x.Set<Item>()).Returns(mockSet.Object);
                mockContext.Mock<ItemsContext>()
                    //When a removal of a Item object is called, perform a callback to the charList collection, using the same Item object as an argument.
                    //This callback then fires, removing the object from the list.
                    .Setup(x => x.Set<Item>().Remove(It.IsAny<Item>()))
                        .Callback<Item>((entity) => Items.Remove(entity));

                //Act
                IUnitOfWork UoW = mockContext.Create<UnitOfWork>();
                IItemsManagerUserAccess toTest = UserAccessFactory.getItemsManagerUserAccess(UoW);
                var toBeDeleted = CreateTestData.GetSampleItem();
                toTest.RemoveItemById(toBeDeleted.Item_id);
                var NotExpected = CreateTestData.GetSampleItem();

                //Assert
                Items.Should().NotContain(toBeDeleted);
            }
        }
    }
}
