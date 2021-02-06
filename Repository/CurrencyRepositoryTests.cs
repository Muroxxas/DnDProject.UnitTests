using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Repository.Interfaces;
using DnDProject.Backend.Repository.Implementations;
using DnDProject.Entities.Character.DataModels;
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
    class CurrencyRepositoryTests
    {
        [Test]
        public void CurrencyRepository_AddCurrencyRecord_ValidCall()
        {
            //Arrange
            List<Currency> currencyList = CreateTestData.GetListOfCurrency();
            var mockSet = new Mock<DbSet<Currency>>()
                .SetupData(currencyList, o =>
                {
                    return currencyList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleCurrency();
                var id = Guid.Parse("b346eee6-eba7-4ea7-be2e-911bb9034233");
                expected.Character_id = id;

                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<Currency>()).Returns(mockSet.Object);

                //Act
                ICurrencyRepository toTest = mockContext.Create<CurrencyRepository>();
                toTest.Add(expected);
                var actual = toTest.Get(id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Currency>();
                expected.Should().BeOfType<Currency>();
                actual.Should().BeEquivalentTo(expected);
            }
        }


        [Test]
        public void CurrencyContext_GetCurrencyRecord_ValidCall()
        {
            //Arrange
            List<Currency> currencyList = CreateTestData.GetListOfCurrency();
            var mockSet = new Mock<DbSet<Currency>>()
                .SetupData(currencyList, o =>
                {
                    return currencyList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleCurrency();
                mockContext.Mock<CharacterContext>()
                   .Setup(x => x.Set<Currency>()).Returns(mockSet.Object);


                //Act
                ICurrencyRepository toTest = mockContext.Create<CurrencyRepository>();
                var actual = toTest.Get(expected.Character_id);


                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Currency>();
                expected.Should().BeOfType<Currency>();
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void EFRepository_UpdateCurrencyRecord_ValidCall()
        {
            //Arrange

            List<Currency> currencyList = CreateTestData.GetListOfCurrency();
            var mockSet = new Mock<DbSet<Currency>>()
                .SetupData(currencyList, o =>
                {
                    return currencyList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleCurrency();
                expected.GoldPieces = 427;

                mockContext.Mock<CharacterContext>()
                  .Setup(x => x.Set<Currency>()).Returns(mockSet.Object);

                //Act
                ICurrencyRepository toTest = mockContext.Create<CurrencyRepository>();
                toTest.Update(expected);


                //Assert
                expected.Should().NotBeNull();
                expected.Should().BeOfType<Currency>();
                //Verifies that the object I wished to update was attached to the DbSet.
                //Basically, that means EF confirms that the entity with expected's Primary key will be updated the next time Save is called.
                mockSet.Verify(x => x.Attach(expected), Times.Once());

            }
        }
    }
}
