using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Mapping.Interfaces;
using DnDProject.Backend.Services.Implementations;
using DnDProject.Backend.Services.Interfaces;
using DnDProject.Backend.Unit_Of_Work.Implementations;
using DnDProject.Backend.Unit_Of_Work.Interfaces;
using DnDProject.Backend.UserAccess.Implementations;
using DnDProject.Backend.UserAccess.Interfaces;
using DnDProject.Entities.Character.DataModels;
using DnDProject.Entities.Character.ViewModels;
using DnDProject.Entities.Races.DataModels;
using DnDProject.UnitTests.Mapping;
using DnDProject.UnitTests.UserAccess;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDProject.UnitTests.Services.Character._CreateCharacter
{
    [TestFixture]
    public class GETTests
    {

        [Test]
        public void CreateCharacterGET_proficiencyCMNotNull()
        {
            //Arrange
            using (var mockContext = mockMaker.getMockContext())
            {
                //Act
                ICreateCharacter toTest = mockMaker.getCharacterCreator(mockContext);
                var primaryTab = toTest.CreateCharacterGET().PrimaryTab;
                var actual = primaryTab.IsProficient;

                //Assert
                actual.Should().NotBeNull();               
            }
        }

        [Test]
        public void CreateCharacterGET_statsCMNotNull()
        {
            
            using (var mockContext = mockMaker.getMockContext())
            {
                ICreateCharacter toTest = mockMaker.getCharacterCreator(mockContext);
                var primaryTab = toTest.CreateCharacterGET().PrimaryTab;
                var actual = primaryTab.Stats;

                //Assert
                actual.Should().NotBeNull();
            }
        }

        [Test]
        public void CreateCharacterGET_CombatCMNotNull()
        {
            using (var mockContext = mockMaker.getMockContext())
            {
                ICreateCharacter toTest = mockMaker.getCharacterCreator(mockContext);
                var primaryTab = toTest.CreateCharacterGET().PrimaryTab;
                var actual = primaryTab.Combat;

                //Assert
                actual.Should().NotBeNull();
            }
        }
        [Test]
        public void CreateCharacterGET_NotesTabNotNull()
        {
            using (var mockContext = mockMaker.getMockContext())
            {
                ICreateCharacter toTest = mockMaker.getCharacterCreator(mockContext);
                var notesTab = toTest.CreateCharacterGET().NotesTab;

                notesTab.Should().NotBeNull();
            }
        }

        [Test]
        public void CreateCharacterGET_InventoryTabNotNull()
        {
            using (var mockContext = mockMaker.getMockContext())
            {
                ICreateCharacter toTest = mockMaker.getCharacterCreator(mockContext);
                var inventoryTab = toTest.CreateCharacterGET().InventoryTab;

                inventoryTab.Should().NotBeNull();
            }
        }
        [Test]
        public void CreateCharacterGET_MoneyCMNotNull()
        {
            using (var mockContext = mockMaker.getMockContext())
            {
                ICreateCharacter toTest = mockMaker.getCharacterCreator(mockContext);
                var inventoryTab = toTest.CreateCharacterGET().InventoryTab;

                inventoryTab.Money.Should().NotBeNull();
            }
        }

        [Test]
        public void CreateCharacterGET_NotesCollectionIsEmpty()
        {
            using (var mockContext = mockMaker.getMockContext())
            {
                ICreateCharacter toTest = mockMaker.getCharacterCreator(mockContext);
                var notesTab = toTest.CreateCharacterGET().NotesTab;

                notesTab.Notes.Length.Should().Be(0);
            }
        }

        [Test]
        public void CreateCharacterGET_HeldItemsCollectionIsEmpty()
        {
            using (var mockContext = mockMaker.getMockContext())
            {
                ICreateCharacter toTest = mockMaker.getCharacterCreator(mockContext);
                var inventoryTab = toTest.CreateCharacterGET().InventoryTab;

                inventoryTab.Items.Length.Should().Be(0);
            }
        }

        [Test]
        public void CreateCharacterGET_racesIsPopulated()
        {
            //Arrange
            List<Race> races = CreateTestData.GetListOfRace();
            using (var mockContext = mockMaker.getMockContext())
            {
                //Act
                ICreateCharacter toTest = mockMaker.getCharacterCreator(mockContext);
                var primaryTab = toTest.CreateCharacterGET().PrimaryTab;
                var actual = primaryTab.Races;

                //Assert
                actual.Count.Should().BeGreaterThan(0);
                actual.Count.Should().Be(races.Count);
                races.Count.Should().BeGreaterThan(0);
            }
        }

        
        



        


    }
}
