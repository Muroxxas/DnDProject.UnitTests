﻿using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Repository.Implementations;
using DnDProject.Backend.Repository.Interfaces;
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
    class HealthRepositoryTests
    {
        [Test]
        public void HealthRepository_AddHealthRecord_ValidCall()
        {
            //Arrange
            List<Health> healthList = CreateTestData.GetListOfHealth();
            var mockSet = new Mock<DbSet<Health>>()
                .SetupData(healthList, o =>
                {
                    return healthList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleHealth();
                var id = Guid.Parse("b346eee6-eba7-4ea7-be2e-911bb9034233");
                expected.Character_id = id;

                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<Health>()).Returns(mockSet.Object);

                //Act
                IHealthRepository toTest = mockContext.Create<HealthRepository>();
                toTest.Add(expected);
                var actual = toTest.Get(id);

                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Health>();
                expected.Should().BeOfType<Health>();
                actual.Should().BeEquivalentTo(expected);

            }
        }


        [Test]
        public void HealthRepository_GetHealthRecord_ValidCall()
        {
            //Arrange
            List<Health> healthList = CreateTestData.GetListOfHealth();
            var mockSet = new Mock<DbSet<Health>>()
                .SetupData(healthList, o =>
                {
                    return healthList.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleHealth();
                var id = expected.Character_id;

                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<Health>()).Returns(mockSet.Object);

                //Act
                IHealthRepository toTest = mockContext.Create<HealthRepository>();
                var actual = toTest.Get(id);

                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Health>();
                expected.Should().BeOfType<Health>();
                actual.Should().BeEquivalentTo(expected);
            }

        }

      

    }
}
