using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Repository.Implementations;
using DnDProject.Backend.Repository.Interfaces;
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

namespace DnDProject.UnitTests.Repository
{
    [TestFixture]
    class ItemsRepositoryTests
    {
        [Test]
        public void ItemsRepository_AddItem_ValidCall()
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
                IItemsRepository toTest = mockContext.Create<ItemsRepository>();
                toTest.Add(expected);
                var actual = toTest.Get(id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Item>();
                expected.Should().BeOfType<Item>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void ItemsRepository_SettAllTagsForItem_ValidCall()
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
            foreach(Tag tag in expected)
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
                IItemsRepository toTest = mockContext.Create<ItemsRepository>();
                toTest.SetAllTagsForItem(whisper_id, tag_ids);
                var actual = toTest.GetTagsForItem(whisper_id);


                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void ItemsRepository_SetTagForItem_ValidCall()
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
                IItemsRepository toTest = mockContext.Create<ItemsRepository>();
                toTest.SetTagForItem(whisper_id, weapon.Tag_id);
                var actual = toTest.GetTagsForItem(whisper_id);


                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }


        [Test]
        public void ItemsRepository_GetItem_ValidCall()
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
                var expected = CreateTestData.GetSampleItem();
                var id = expected.Item_id;

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Item>()).Returns(mockSet.Object);

                //Act
                IItemsRepository toTest = mockContext.Create<ItemsRepository>();
                var actual = toTest.Get(id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Item>();
                expected.Should().BeOfType<Item>();
                actual.Should().BeEquivalentTo(expected);
            };
        }
        [Test]
        public void ItemsRepository_GetItemsHeldByCharacter_ValidCall()
        {
            //Arrange
            List<Character_Item> heldItems = CreateTestData.GetListOfHeldItems();
            List<Item> itemset = CreateTestData.GetListOfItems();
            var heldItemsMockSet = new Mock<DbSet<Character_Item>>()
              .SetupData(heldItems, o =>
              {
                  return heldItems.Single(x => x.Character_id.CompareTo(o.First()) == 0);
              });

            var ItemsMockSet = new Mock<DbSet<Item>>()
                .SetupData(itemset, o =>
                {
                    return itemset.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            var Vax_id = Guid.Parse("e3a0faef-99da-4d15-bff1-b535a42b955c");
            var expected = new List<Item>();
            Item Whisper = new Item()
            {
                Item_id = Guid.Parse("709135c3-6f89-46cb-80ae-4097b621e3b0"),
                Name = "Whisper",
                Description = "A Legendary dagger that allows you to teleport to wherever it strikes",
                isEquippable = true,
                isConsumable = false,
                requiresAttunement = true,
                Value = 999
            };
            expected.Add(Whisper);
            Item HealingPotion = new Item
            {
                Item_id = Guid.Parse("2caa23dc-15e6-4a57-9bb6-62f6d8636ff7"),
                Name = "Healing potion",
                Description = "Upon consumption of the whole potion, the imbiber heals for 2d4+2 health.",
                isEquippable = false,
                isConsumable = true,
                requiresAttunement = false,
                Value = 50
            };
            expected.Add(HealingPotion);
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(ItemsMockSet.Object);

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.HeldItems).Returns(heldItemsMockSet.Object);

                //Act
                IItemsRepository toTest = mockContext.Create<ItemsRepository>();
                var actual = toTest.GetItemsHeldBy(Vax_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void ItemsRepository_GetHeldItemRecord_ValidCall()
        {
            //Arrange
            List<Character_Item> heldItems = CreateTestData.GetListOfHeldItems();


            var heldItemsMockSet = new Mock<DbSet<Character_Item>>()
              .SetupData(heldItems, o =>
              {
                  return heldItems.Single(x => x.Character_id.CompareTo(o.First()) == 0);
              });


            var expected = CreateTestData.GetSampleHeldItem();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.HeldItems).Returns(heldItemsMockSet.Object);

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Character_Item>()).Returns(heldItemsMockSet.Object);

                //Act
                IItemsRepository toTest = mockContext.Create<ItemsRepository>();
                var actual = toTest.GetHeldItemRecord(expected.Character_id, expected.Item_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);

            }
        }
        [Test]
        public void ItemsRepository_GetHeldItemRecordsForCharacter_ValidCall()
        {
            //Arrange
            List<Character_Item> heldItems = CreateTestData.GetListOfHeldItems();


            var heldItemsMockSet = new Mock<DbSet<Character_Item>>()
              .SetupData(heldItems, o =>
              {
                  return heldItems.Single(x => x.Character_id.CompareTo(o.First()) == 0);
              });

            List<Character_Item> expected = new List<Character_Item>();
            Character_Item Vax_Whisper = CreateTestData.GetSampleHeldItem();
            expected.Add(Vax_Whisper);

            Character_Item Vax_Potion = new Character_Item
            {
                Character_id = Guid.Parse("e3a0faef-99da-4d15-bff1-b535a42b955c"),
                Item_id = Guid.Parse("2caa23dc-15e6-4a57-9bb6-62f6d8636ff7"),
                count = 3
            };
            expected.Add(Vax_Potion);

            Guid Vax_id = Guid.Parse("e3a0faef-99da-4d15-bff1-b535a42b955c");

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.HeldItems).Returns(heldItemsMockSet.Object);

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Set<Character_Item>()).Returns(heldItemsMockSet.Object);

                //Act
                IItemsRepository toTest = mockContext.Create<ItemsRepository>();
                var actual = toTest.GetHeldItemRecordsForCharacter(Vax_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);

            }
        }

        [Test]
        public void ItemsRepository_GetAllTags_ValidCall()
        {
            List<Tag> tags = CreateTestData.GetListOfTags();
            var mockSet = new Mock<DbSet<Tag>>()
                .SetupData(tags, o =>
                {
                    return tags.Single(x => x.Tag_id.CompareTo(o.First()) == 0);
                });

            var expected = CreateTestData.GetListOfTags();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Tags).Returns(mockSet.Object);

                //Act
                IItemsRepository toTest = mockContext.Create<ItemsRepository>();
                var actual = toTest.GetAllTags();

                //Assert
                actual.Should().BeEquivalentTo(expected);

            }
        }

        [Test]
        public void ItemsRepository_GetTagsForItem_ValidCall()
        {
            List<Item_Tag> itemTags = CreateTestData.GetListOfItemTags();
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
            var expected = new List<Tag>();
            Tag Wondorous = new Tag
            {
                Tag_id = Guid.Parse("e2c7f8a3-52ba-4dc2-baaf-4026718b1f03"),
                TagName = "Wondorous Item"
            };
            expected.Add(Wondorous);
            Tag Weapon = new Tag
            {
                Tag_id = Guid.Parse("172e8478-e1bd-49ba-a7a7-6455d5a58c6e"),
                TagName = "Weapon"
            };
            expected.Add(Weapon);
            Guid whisper_id = Guid.Parse("709135c3-6f89-46cb-80ae-4097b621e3b0");
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Tags).Returns(tagsMockSet.Object);
                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Item_Tags).Returns(ITmockSet.Object);

                //Act
                IItemsRepository toTest = mockContext.Create<ItemsRepository>();
                var actual = toTest.GetTagsForItem(whisper_id);


                //Assert
                actual.Should().BeEquivalentTo(expected);

            }
        }
        [Test]
        public void ItemsRepository_RemoveItem_ValidCall()
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
                IItemsRepository toTest = mockContext.Create<ItemsRepository>();
                var toBeDeleted = CreateTestData.GetSampleItem();
                toTest.Remove(toBeDeleted);
                var NotExpected = CreateTestData.GetSampleItem();

                //Assert
                Items.Should().NotContain(toBeDeleted);
            }
        }
        [Test]
        public void ItemsRepository_RemoveItemByID_ValidCall()
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
                IItemsRepository toTest = mockContext.Create<ItemsRepository>();
                var toBeDeleted = CreateTestData.GetSampleItem();
                toTest.Remove(toBeDeleted.Item_id);
                var NotExpected = CreateTestData.GetSampleItem();

                //Assert
                Items.Should().NotContain(toBeDeleted);
            }
        }

        [Test]
        public void ItemsRepository_CharacterObtainsItem_ByIDs_ValidCall()
        {
            //Arrange
            List<Character_Item> heldItems = new List<Character_Item>();
            List<Item> items = CreateTestData.GetListOfItems();
            var heldItemsMockSet = new Mock<DbSet<Character_Item>>()
                .SetupData(heldItems, o =>
                {
                    return heldItems.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            var ItemsMockSet = new Mock<DbSet<Item>>()
                .SetupData(items, o =>
                {
                    return items.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var Whisper = CreateTestData.GetSampleItem();
                var Whisper_id = Whisper.Item_id;
                var Vax_id = Guid.Parse("e3a0faef-99da-4d15-bff1-b535a42b955c");

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(ItemsMockSet.Object);

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.HeldItems).Returns(heldItemsMockSet.Object);

                //Act
                IItemsRepository toTest = mockContext.Create<ItemsRepository>();
                toTest.CharacterObtainsItem(Vax_id, Whisper_id);

                var actual = toTest.GetItemsHeldBy(Vax_id);

                //Assert
                actual.Should().ContainEquivalentOf(Whisper);

            }
        }

        [Test]
        public void ItemsRepository_CharacterObtainsItem_ByRecord_ValidCall()
        {
            //Arrange
            List<Character_Item> heldItems = new List<Character_Item>();
            var mockSet = new Mock<DbSet<Character_Item>>()
                .SetupData(heldItems, o =>
                {
                    return heldItems.Single(x => x.Item_id.CompareTo(o.First())==0);
                });
                 
            var expected = new Character_Item
            {
                Character_id = Guid.Parse("e3a0faef-99da-4d15-bff1-b535a42b955c"),
                Item_id = Guid.Parse("709135c3-6f89-46cb-80ae-4097b621e3b0"),
                isEquipped = true,
                IsAttuned = true
            };

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<ItemsContext>()
                    .Setup(X => X.HeldItems).Returns(mockSet.Object);

                //Act
                IItemsRepository toTest = mockContext.Create<ItemsRepository>();
                toTest.CharacterObtainsItem(expected);

                var actual = heldItems.First();
                //assert
                actual.Should().BeEquivalentTo(expected);

            }
        }
        [Test]
        public void ItemsRepository_CharacterLosesItem_ValidCall()
        {
            //Arrange - Vax loses Whisper
            List<Character_Item> heldItems = CreateTestData.GetListOfHeldItems();
            List<Item> itemset = CreateTestData.GetListOfItems();
            var heldItemsMockSet = new Mock<DbSet<Character_Item>>()
              .SetupData(heldItems, o =>
              {
                  return heldItems.Single(x => x.Character_id.CompareTo(o.First()) == 0);
              });

            var ItemsMockSet = new Mock<DbSet<Item>>()
                .SetupData(itemset, o =>
                {
                    return itemset.Single(x => x.Item_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var Whisper = CreateTestData.GetSampleItem();
                var Whisper_id = Whisper.Item_id;
                var Vax_id = Guid.Parse("e3a0faef-99da-4d15-bff1-b535a42b955c");

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.Items).Returns(ItemsMockSet.Object);

                mockContext.Mock<ItemsContext>()
                    .Setup(x => x.HeldItems).Returns(heldItemsMockSet.Object);
                
                //Act
                IItemsRepository toTest = mockContext.Create<ItemsRepository>();
                toTest.CharacterLosesItem(Vax_id, Whisper_id);
                var actual = toTest.GetItemsHeldBy(Vax_id);

                //Assert
                actual.Should().NotContain(Whisper);
            }
        }
    }
}
