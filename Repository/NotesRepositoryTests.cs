using Autofac.Extras.Moq;
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
    class NotesRepositoryTests
    {

        [Test]
        public void EFRepository_AddNote_ValidCall()
        {
            List<Note> listOfNotes = CreateTestData.GetListOfNotes();
            var mockSet = new Mock<DbSet<Note>>()
                .SetupData(listOfNotes, o =>
                {
                    return listOfNotes.Single(x => x.Note_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleNote();
                var id = Guid.Parse("b346eee6-eba7-4ea7-be2e-911bb9034233");
                expected.Note_id = id;

                mockContext.Mock<CharacterContext>()
                    .Setup(x => x.Set<Note>()).Returns(mockSet.Object);

                //Act
                INotesRepository toTest = mockContext.Create<NotesRepository>();
                toTest.Add(expected);
                var actual = toTest.Get(id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Note>();
                expected.Should().BeOfType<Note>();
                actual.Should().BeEquivalentTo(expected);

            }
        }


        [Test]
        public void EFRepository__GetNote_ValidCall()
        {
            //Arrange
            List<Note> listOfNotes = CreateTestData.GetListOfNotes();
            var mockSet = new Mock<DbSet<Note>>()
                .SetupData(listOfNotes, o =>
                {
                    return listOfNotes.Single(x => x.Note_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {

                var expected = CreateTestData.GetSampleNote();
                mockContext.Mock<CharacterContext>()
                   .Setup(x => x.Set<Note>()).Returns(mockSet.Object);

                //Act
                INotesRepository toTest = mockContext.Create<NotesRepository>();
                var actual = toTest.Get(expected.Note_id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Note>();
                expected.Should().BeOfType<Note>();
                actual.Should().BeEquivalentTo(expected);
            }
        }


        [Test]
        public void EFRepository_GetNotesOwnedByCharacter_ValidCall()
        {
            //Arrange
            List<Note> listOfNotes = CreateTestData.GetListOfNotes();
            var mockSet = new Mock<DbSet<Note>>()
                .SetupData(listOfNotes, o =>
                {
                    return listOfNotes.Single(x => x.Note_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {

                var expected = new List<Note>();
                expected.Add(CreateTestData.GetSampleNote());
                var GreatAxe = new Note()
                {
                    Note_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                    Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                    Name = "How To Use a Great Axe Ft. Grog Strongjaw",
                    Contents = "Lorem Ipsum"
                };

                expected.Add(GreatAxe);

                //1. When the 
                mockContext.Mock<CharacterContext>()
                   .Setup(x => x.Set<Note>()).Returns(mockSet.Object);

                //Act
                INotesRepository toTest = mockContext.Create<NotesRepository>();
                var actual = toTest.GetNotesOwnedBy(Guid.Parse("11111111-2222-3333-4444-555555555555"));

                actual.Should().NotBeEmpty();
                expected.Should().NotBeEmpty();
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<List<Note>>();
                expected.Should().BeOfType<List<Note>>();
                actual.Should().BeEquivalentTo(expected);

            }
        }


        

        [Test]
        public void EFRepository__DeleteNote_ValidCall()
        {
            //Arrange
            List<Note> listOfNotes = CreateTestData.GetListOfNotes();
            var mockSet = new Mock<DbSet<Note>>()
                .SetupData(listOfNotes, o =>
                {
                    return listOfNotes.Single(x => x.Note_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var ToBeDeleted = CreateTestData.GetSampleNote();

                mockContext.Mock<CharacterContext>()
                   .Setup(x => x.Set<Note>()).Returns(mockSet.Object);

                //Act
                INotesRepository toTest = mockContext.Create<NotesRepository>();
                toTest.Remove(ToBeDeleted.Note_id);

                //Assert
                listOfNotes.Should().NotBeEmpty();
                listOfNotes.Should().NotContain(ToBeDeleted);
                listOfNotes.Should().BeOfType<List<Note>>();
            }
        }
    }
}
