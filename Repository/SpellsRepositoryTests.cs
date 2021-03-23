using Autofac.Extras.Moq;
using DnDProject.Backend.Contexts;
using DnDProject.Backend.Repository.Implementations;
using DnDProject.Backend.Repository.Interfaces;
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

namespace DnDProject.UnitTests.Repository
{
    [TestFixture]
    public class SpellsRepositoryTests
    {
        [Test]
        public void SpellsRepository_AddSpell_ValidCall()
        {
            //Arrange
            List<Spell> spells = new List<Spell>();
            var mockSet = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleSpell();
                var id = expected.Spell_id;

                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(mockSet.Object);

                //Act
                ISpellsRepository toTest = mockContext.Create<SpellsRepository>();
                toTest.Add(expected);
                var actual = toTest.Get(id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Spell>();
                expected.Should().BeOfType<Spell>();
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void SpellsRepository_GetSpell_ValidCall()
        {
            //Arrange
            List<Spell> spells =  CreateTestData.GetListOfSpells();
            var mockSet = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleSpell();
                var id = expected.Spell_id;

                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell>()).Returns(mockSet.Object);

                //Act
                ISpellsRepository toTest = mockContext.Create<SpellsRepository>();
                var actual = toTest.Get(id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Spell>();
                expected.Should().BeOfType<Spell>();
                actual.Should().BeEquivalentTo(expected);
            };
        }
        [Test]
        public void SpellsRepository_GetKnownSpellRecordsForCharacter_ValidCall()
        {
            List<Spell_Character> knownSpells = CreateTestData.GetListOfKnownSpells();
            var mockSet = new Mock<DbSet<Spell_Character>>()
                .SetupData(knownSpells, o =>
                {
                    return knownSpells.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            List<Spell_Character> expected = new List<Spell_Character>();
            Spell_Character Caleb_Tower = CreateTestData.GetSampleKnownSpell();
            expected.Add(Caleb_Tower);

            Spell_Character Caleb_WebOfFire = new Spell_Character()
            {
                Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                Spell_id = Guid.Parse("51b4c563-2040-4c7d-a23e-cab8d5d3c73b")
            };
            expected.Add(Caleb_WebOfFire);
  
            Guid Caleb_id = Guid.Parse("11111111-2222-3333-4444-555555555555");

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.KnownSpells).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell_Character>()).Returns(mockSet.Object);

                //Act
                ISpellsRepository toTest = mockContext.Create<SpellsRepository>();
                var actual = toTest.GetKnownSpellRecordsForCharacter(Caleb_id);

                //Arrange
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void SpellsRepository_GetKnownSpellRecord_ValidCall()
        {
            //Arrange
            //Create list of Character_Spell
            List<Spell_Character> knownSpells = CreateTestData.GetListOfKnownSpells();
            var mockSet = new Mock<DbSet<Spell_Character>>()
                .SetupData(knownSpells, o =>
                {
                    return knownSpells.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            //We want to find the record that indicates Caleb can create his tower.
            var expected = CreateTestData.GetSampleKnownSpell();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.KnownSpells).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Set<Spell_Character>()).Returns(mockSet.Object);

                //Act
                ISpellsRepository toTest = mockContext.Create<SpellsRepository>();
                var actual = toTest.GetKnownSpellRecord(expected.Character_id, expected.Spell_id);

                //Arrange
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void SpellsRepository_GetSpellsKnownBy_ValidCall()
        {
            //Arrange

            //Create list of spells
            List<Spell> spells = CreateTestData.GetListOfSpells();
            //Create list of Character_Spell
            List<Spell_Character> knownSpells = CreateTestData.GetListOfKnownSpells();

            //We'll be looking for spells that Caleb knows!
            List<Spell> expected = new List<Spell>();
            Spell WebOfFire = new Spell
            {
                Spell_id = Guid.Parse("51b4c563-2040-4c7d-a23e-cab8d5d3c73b"),
                Name = "Widogast's Web Of Fire",
                Description = "The caster deals a shitton of fire damage to the target.",
                Level = 4,
                School_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                CastingTime = "1 Action",
                Duration = "Instant",
                Range = "120 feet",
                RequiresVerbal = true,
                RequiresSomantic = true,
                RequiresMaterial = true,
                RequiresConcentration = false
            };
            expected.Add(WebOfFire);
            Spell nineSidedTower = new Spell
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
            expected.Add(nineSidedTower);

            var spellMockSet = new Mock<DbSet<Spell>>()
               .SetupData(spells, o =>
               {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
               });

            var knownSpellsMockSet = new Mock<DbSet<Spell_Character>>()
                .SetupData(knownSpells, o =>
                {
                    return knownSpells.Single(x => x.Character_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(spellMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.KnownSpells).Returns(knownSpellsMockSet.Object);

                //Act
                ISpellsRepository toTest = mockContext.Create<SpellsRepository>();
                var Caleb_id = Guid.Parse("11111111-2222-3333-4444-555555555555");
                var actual = toTest.GetSpellsKnownBy(Caleb_id).ToList();

                //Assert
                actual.Should().NotBeEmpty();
                actual.Should().NotBeNull();
                actual.Should().BeOfType<List<Spell>>();
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void SpellsRepository_GetSpellsOfSchool_ValidCall()
        {
            //Arrange
            List<Spell> spells = CreateTestData.GetListOfSpells();

            List <School> schools = CreateTestData.GetListOfSchools();

            //We're expecting the only Conjutation spell contained within the spells list - Caleb's tower.
            Spell NineSidedTower = new Spell
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
            List<Spell> expected = new List<Spell>();
            expected.Add(NineSidedTower);

            var spellMockSet = new Mock<DbSet<Spell>>()
               .SetupData(spells, o =>
               {
                   return spells.Single(x => x.School_id.CompareTo(o.First()) == 0);
               });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(spellMockSet.Object);
                //Act
                ISpellsRepository toTest = mockContext.Create<SpellsRepository>();
                var conjuration_id = NineSidedTower.School_id;
                var actual = toTest.GetSpellsOfSchool(conjuration_id);

                //Assert
                actual.Should().NotBeEmpty();
                actual.Should().NotBeNull();
                actual.Should().BeOfType<List<Spell>>();
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void SpellsRepository_GetSpellsCastableBy_ValidCall()
        {
            //Arrange
            List<Spell> spells = CreateTestData.GetListOfSpells();

            List<Spell_Class> CastableByRecords = CreateTestData.GetListOfCastableByRecords();

            //I expect three spells - Web of Fire, Voltaic Bolt, and the Tower, all of which can be cast by a wizard.
            List<Spell> expected = new List<Spell>();
            Spell NineSidedTower = new Spell
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
            expected.Add(NineSidedTower);
            Spell WebOfFire = new Spell
            {
                Spell_id = Guid.Parse("51b4c563-2040-4c7d-a23e-cab8d5d3c73b"),
                Name = "Widogast's Web Of Fire",
                Description = "The caster deals a shitton of fire damage to the target.",
                Level = 4,
                School_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                CastingTime = "1 Action",
                Duration = "Instant",
                Range = "120 feet",
                RequiresVerbal = true,
                RequiresSomantic = true,
                RequiresMaterial = true,
                RequiresConcentration = false
            };
            expected.Add(WebOfFire);
            Spell VoltaicBolt = new Spell
            {
                Spell_id = Guid.Parse("a9756f3d-55d0-40cd-8083-6b547e4932ab"),
                Name = "Brenatto's Voltaic Bolt",
                Description = "The caster's next ranged attack deals an additional 3d6 lightning damage",
                Level = 1,
                School_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                CastingTime = "1 Bonus Action",
                Duration = "1 round",
                Range = "30 feet",
                RequiresVerbal = false,
                RequiresSomantic = true,
                RequiresMaterial = true,
                RequiresConcentration = false
            };
            expected.Add(VoltaicBolt);

            var spellMockSet = new Mock<DbSet<Spell>>()
               .SetupData(spells, o =>
               {
                   return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
               });

            var SpellCastableByMockSet = new Mock<DbSet<Spell_Class>>()
                .SetupData(CastableByRecords, o =>
                {
                    return CastableByRecords.Single(x => x.Class_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(spellMockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.CastableByRecords).Returns(SpellCastableByMockSet.Object);


                //Act
                ISpellsRepository toTest = mockContext.Create<SpellsRepository>();
                Guid WIzard_id = Guid.Parse("b74e228f-015d-45b4-af0f-a6781976535a");
                var actual = toTest.GetSpellsCastableBy(WIzard_id).ToList();

                //Assert
                actual.Should().NotBeEmpty();
                actual.Should().NotBeNull();
                actual.Should().BeOfType<List<Spell>>();
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Test]
        public void SpellsRepository_GetIdsOfClassesThatCanCastSpell_ValidCall()
        {
            //Arrange
            List<Spell_Class> castableByList = CreateTestData.GetListOfCastableByRecords();
            var mockSet = new Mock<DbSet<Spell_Class>>()
                .SetupData(castableByList, o =>
                {
                    return castableByList.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });
            var expected = new Spell_Class
            {
                Spell_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f"),
                Class_id = Guid.Parse("b74e228f-015d-45b4-af0f-a6781976535a")
            };
            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.CastableByRecords).Returns(mockSet.Object);

                //Act
                ISpellsRepository toTest = mockContext.Create<SpellsRepository>();
                var actual = toTest.GetIdsOfClassesThatCanCastSpell(expected.Spell_id);

                //Assert
                actual.Should().Contain(expected.Class_id);
            }
        }

        [Test]
        public void SpellsRepository_GetSchool_ValidCall()
        {
            //Arrange
            List<School> spellSchools = CreateTestData.GetListOfSchools();
            var schoolsMockSet = new Mock<DbSet<School>>()
                .SetupData(spellSchools, o =>
                {
                    return spellSchools.Single(x => x.School_id.CompareTo(o.First()) == 0);
                });

            var expected = CreateTestData.GetSampleSchool();

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Schools).Returns(schoolsMockSet.Object);

                //Act
                ISpellsRepository toTest = mockContext.Create<SpellsRepository>();
                var actual = toTest.GetSchool(expected.School_id);

                //Assert
                actual.Should().BeEquivalentTo(expected);

            }
        }

        [Test]
        public void SpellsRepository_RemoveSpell_ValidCall()
        {
            //Arrange

            List<Spell> spells = CreateTestData.GetListOfSpells();
            var mockSet = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0); 
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                   .Setup(x => x.Set<Spell>()).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    //When a removal of a spell object is called, perform a callback to the charList collection, using the same spell object as an argument.
                    //This callback then fires, removing the object from the list.
                    .Setup(x => x.Set<Spell>().Remove(It.IsAny<Spell>()))
                        .Callback<Spell>((entity) => spells.Remove(entity));

                //Act
                ISpellsRepository toTest = mockContext.Create<SpellsRepository>();
                var toBeDeleted = CreateTestData.GetSampleSpell();
                toTest.Remove(toBeDeleted); 
                var NotExpected = CreateTestData.GetSampleSpell();

                //Assert
                spells.Should().NotContain(toBeDeleted);
            }
        }

        [Test]
        public void SpellsRepository_AddSpellMaterials_ValidCall()
        {
            //Arrange
            List<Material> Materials = new List<Material>();
            var mockSet = new Mock<DbSet<Material>>()
                .SetupData(Materials, o =>
                {
                    return Materials.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                var expected = CreateTestData.GetSampleMaterial();
                var id = expected.Spell_id;

                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Materials).Returns(mockSet.Object);

                //Act
                ISpellsRepository toTest = mockContext.Create<SpellsRepository>();
                toTest.AddSpellMaterials(expected);
                var actual = toTest.GetSpellMaterials(expected.Spell_id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Material>();
                expected.Should().BeOfType<Material>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void SpellsRepository_GetSpellMaterials_ValidCall()
        {
            //Arrange
            List<Material> Materials = CreateTestData.GetListOfMaterials();
            var mockSet = new Mock<DbSet<Material>>()
                .SetupData(Materials, o =>
                {
                    return Materials.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Materials).Returns(mockSet.Object);

                Material expected = new Material()
                {
                    Spell_id = Guid.Parse("caf8b2d1-7903-493c-bc3a-ec2fc554d533"),
                    materials = "Diamonds worth 300 gp, which the spell consumes."
                };

                //act
                ISpellsRepository toTest = mockContext.Create<SpellsRepository>();
                var actual = toTest.GetSpellMaterials(expected.Spell_id);

                //Assert
                actual.Should().NotBeNull();
                expected.Should().NotBeNull();
                actual.Should().BeOfType<Material>();
                expected.Should().BeOfType<Material>();
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void SpellsRepository_DeleteSpellMaterials_ValidCall()
        {
            //Arrange
            List<Material> Materials = CreateTestData.GetListOfMaterials();
            var mockSet = new Mock<DbSet<Material>>()
                .SetupData(Materials, o =>
                {
                    return Materials.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Materials).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Materials.Remove(It.IsAny<Material>()))
                        .Callback<Material>((entity) => Materials.Remove(entity));

                var toBeDeleted = CreateTestData.GetSampleMaterial();

                //act
                ISpellsRepository toTest = mockContext.Create<SpellsRepository>();
                toTest.DeleteSpellMaterials(toBeDeleted);

                //Assert
                Materials.Should().NotContain(toBeDeleted);
            }
        }
        [Test]
        public void SpellsRepository_DeleteSpellMaterialsById_ValidCall()
        {
            //Arrange
            List<Material> Materials = CreateTestData.GetListOfMaterials();
            var mockSet = new Mock<DbSet<Material>>()
                .SetupData(Materials, o =>
                {
                    return Materials.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Materials).Returns(mockSet.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Materials.Remove(It.IsAny<Material>()))
                        .Callback<Material>((entity) => Materials.Remove(entity));

                var toBeDeleted = CreateTestData.GetSampleMaterial();

                //act
                ISpellsRepository toTest = mockContext.Create<SpellsRepository>();
                toTest.DeleteSpellMaterialsById(toBeDeleted.Spell_id);

                //Assert
                Materials.Should().NotContain(toBeDeleted);
            }
        }

        [Test]
        public void SpellsRepository_CharacterLearnsSpell_ByIDs_ValidCall()
        {
            //Arrange
            List<Spell_Character> KnownSpells = CreateTestData.GetListOfKnownSpells();
            List<Spell> spells = CreateTestData.GetListOfSpells();
            
            var mockKnownSpells = new Mock<DbSet<Spell_Character>>()
                .SetupData(KnownSpells, o =>
                {
                    return KnownSpells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });
            var mockSpells = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            //Caleb learns Eldritch blast, somehow!
            Guid Caleb_id = Guid.Parse("11111111-2222-3333-4444-555555555555");
            Guid EldritchBlast_id = Guid.Parse("45c1a8cc-2e3e-4e29-8eeb-f9fa0cc9e27e");

            Spell EldritchBlast = new Spell
            {
                Spell_id = Guid.Parse("45c1a8cc-2e3e-4e29-8eeb-f9fa0cc9e27e"),
                Name = "Eldritch Blast",
                Description = "Cast eldritch blast",
                Level = 0,
                School_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                CastingTime = "1 Action",
                Duration = "Instant",
                Range = "60 feet",
                RequiresVerbal = true,
                RequiresSomantic = true,
                RequiresMaterial = true,
                RequiresConcentration = false
            };

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(mockSpells.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.KnownSpells).Returns(mockKnownSpells.Object);

                //Act
                ISpellsRepository toTest = mockContext.Create<SpellsRepository>();
                toTest.CharacterLearnsSpell(Caleb_id, EldritchBlast_id);
                var actual = toTest.GetSpellsKnownBy(Caleb_id);


                //Assert
                actual.Should().ContainEquivalentOf(EldritchBlast);
            }
        }

        [Test]
        public void SpellsRepository_CharacterLearnsSpell_ByRecord_ValidCall()
        {
            //Arrange
            List<Spell_Character> KnownSpells = new List<Spell_Character>();
            var mockKnownSpells = new Mock<DbSet<Spell_Character>>()
                .SetupData(KnownSpells, o =>
                {
                    return KnownSpells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            Spell_Character expected = new Spell_Character
            {
                Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                Spell_id = Guid.Parse("45c1a8cc-2e3e-4e29-8eeb-f9fa0cc9e27e"),
                isPrepared = true
                
            };

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.KnownSpells).Returns(mockKnownSpells.Object);

                //Act
                ISpellsRepository toTest = mockContext.Create<SpellsRepository>();
                toTest.CharacterLearnsSpell(expected);

                var actual = KnownSpells.First();

                //assert
                actual.Should().BeEquivalentTo(expected);
            }
        }
        [Test]
        public void SpellsRepository_CharacterForgetsSpell_ValidCall()
        {
            //Arrange
            List<Spell_Character> KnownSpells = CreateTestData.GetListOfKnownSpells();
            List<Spell> spells = CreateTestData.GetListOfSpells();

            var mockKnownSpells = new Mock<DbSet<Spell_Character>>()
                .SetupData(KnownSpells, o =>
                {
                    return KnownSpells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });
            var mockSpells = new Mock<DbSet<Spell>>()
                .SetupData(spells, o =>
                {
                    return spells.Single(x => x.Spell_id.CompareTo(o.First()) == 0);
                });

            //Caleb forgets Web of Fire!
            Guid Caleb_id = Guid.Parse("11111111-2222-3333-4444-555555555555");
            Spell WebOfFire = new Spell
            {
                Spell_id = Guid.Parse("51b4c563-2040-4c7d-a23e-cab8d5d3c73b"),
                Name = "Widogast's Web Of Fire",
                Description = "The caster deals a shitton of fire damage to the target.",
                Level = 4,
                School_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                CastingTime = "1 Action",
                Duration = "Instant",
                Range = "120 feet",
                RequiresVerbal = true,
                RequiresSomantic = true,
                RequiresMaterial = true,
                RequiresConcentration = false
            };

            using (var mockContext = AutoMock.GetLoose())
            {
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.Spells).Returns(mockSpells.Object);
                mockContext.Mock<SpellsContext>()
                    .Setup(x => x.KnownSpells).Returns(mockKnownSpells.Object);

                //Act
                ISpellsRepository toTest = mockContext.Create<SpellsRepository>();
                toTest.CharacterForgetsSpell(Caleb_id, WebOfFire.Spell_id);
                var actual = toTest.GetSpellsKnownBy(Caleb_id);

                actual.Should().NotContain(WebOfFire);
            }
        }


    }
}
