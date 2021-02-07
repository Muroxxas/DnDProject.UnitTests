using DnDProject.Entities.Character.DataModels;
using DnDProject.Entities.Character.ViewModels;
using DnDProject.Entities.Spells.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDProject.UnitTests
{
    public static class CreateTestData
    {
        public static Character getSampleCharacter()
        {
            Character character = new Character
            {
                Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                User_id = Guid.Parse("4878cf31-c247-4d8f-b55c-b7ebdcd673be"),
                Name = "Vax'ildan",
                Race_id = Guid.Parse("0c56bb15-ea0b-46a6-9454-1533045574b6"),
                Alignment = "Chaotic Good",
                Exp = 0,
                Background = "Criminal",
                Inspiration = false
            };

            return character;
        }

        public static CharacterVM getSampleCharacterVM() 
        {
            CharacterVM character = new CharacterVM
            {
                Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                Name = "Vax'ildan",
                Race_id = Guid.Parse("0c56bb15-ea0b-46a6-9454-1533045574b6"),
                Alignment = "Chaotic Good",
                Exp = 0,
                Background = "Criminal",
                Inspiration = false
            };

            return character;
        }

        public static List<Character> GetListOfCharacters()
        {
            List<Character> listOfCharacters = new List<Character>();

            Character Vax = getSampleCharacter();
            listOfCharacters.Add(Vax);

            Character Percy = new Character
            {
                Character_id = Guid.Parse("55555555-4444-3333-2222-111111111111"),
                User_id = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
                Name = "Percy de Rolo III",
                Race_id = Guid.Parse("00000000-9999-8888-7777-666666666666"),
                Alignment = "Chaotic Good",
                Exp = 0,
                Background = "Noble",
                Inspiration = false
            };
            listOfCharacters.Add(Percy);

            Character Gilmore = new Character
            {
                Character_id = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
                User_id = Guid.Parse("00000000-9999-8888-7777-666666666666"),
                Name = "Percy de Rolo III",
                Race_id = Guid.Parse("55555555-4444-3333-2222-111111111111"),
                Alignment = "Chaotic Good",
                Exp = 0,
                Background = "Merchant",
                Inspiration = false
            };
            listOfCharacters.Add(Gilmore);

            return listOfCharacters;

        }

        public static IsProficient GetSampleIsProficient()
        {
            IsProficient isProficient = new IsProficient
            {

                Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                StrengthSave = true,
                DexteritySave = true,
                ConstitutionSave = true,
                IntelligenceSave = false,
                WisdomSave = false,
                CharismaSave = false,

                Acrobatics = true,
                AnimalHandling = true,
                Arcana = true,
                Athletics = true,
                Deception = true,

                History = false,
                Intimidation = false,
                Investigation = false,
                Medicine = false,
                Nature = false,

                Perception = true,
                Performance = true,
                Persuasion = true,
                Religion = true,
                SleightOfHand = true,

                Stealth = false,
                Survival = false
            };

            return isProficient;
        }
        public static List<IsProficient> GetListOfIsProficient()
        {
            List<IsProficient> proficiencyList = new List<IsProficient>();

            var Vax = GetSampleIsProficient();
            var Percy = GetSampleIsProficient();
            Percy.Character_id = Guid.Parse("55555555-4444-3333-2222-111111111111");
            Percy.AnimalHandling = false;

            var Gilmore = GetSampleIsProficient();
            Gilmore.Character_id = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
            Gilmore.Survival = true;

            proficiencyList.Add(Vax);
            proficiencyList.Add(Percy);
            proficiencyList.Add(Gilmore);

            return proficiencyList;
        }

        public static Health GetSampleHealth()
        {
            Health health = new Health
            {
                Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                MaxHP = 50,
                CurrentHP = 25,
                TempHP = 0,
                DeathSaveSuccesses = 0,
                DeathSaveFails = 0
            };
            return health;
        }

        public static List<Health> GetListOfHealth()
        {
            List<Health> listOfHealth = new List<Health>();
            listOfHealth.Add(GetSampleHealth());

            var Percy = GetSampleHealth();
            Percy.Character_id = Guid.Parse("55555555-4444-3333-2222-111111111111");
            Percy.MaxHP = 40;
            Percy.CurrentHP = 35;
            listOfHealth.Add(Percy);

            var Grog = GetSampleHealth();
            Grog.Character_id = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
            Grog.MaxHP = 100;
            Grog.CurrentHP = 100;
            listOfHealth.Add(Grog);

            return listOfHealth;
        }

        public static Stats GetSampleStats()
        {
            Stats stats = new Stats
            {
                Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                Strength = 10,
                Dexterity = 10,
                Constitution = 10,
                Intelligence = 10,
                Wisdom = 10,
                Charisma = 10
            };
            return stats;
        }

        public static List<Stats> GetListOfStats()
        {
            List<Stats> listOfStats = new List<Stats>();
            listOfStats.Add(GetSampleStats());

            var Vex = new Stats()
            {
                Character_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f"),
                Strength = 7,
                Dexterity = 20,
                Constitution = 10,
                Intelligence = 14,
                Wisdom = 16,
                Charisma = 17
            };
            listOfStats.Add(Vex);

            var Caleb = new Stats()
            {
                Character_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                Strength = 10,
                Dexterity = 12,
                Constitution = 16,
                Intelligence = 20,
                Wisdom = 16,
                Charisma = 16
            };
            listOfStats.Add(Caleb);

            return listOfStats;
        }

        public static Currency GetSampleCurrency()
        {
            Currency currency = new Currency
            {
                Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                PlatinumPieces = 50,
                GoldPieces = 4000,
                ElectrumPieces = 0,
                SilverPieces = 99,
                CopperPieces = 500
            };
            return currency;
        }

        public static List<Currency> GetListOfCurrency()
        {
            List<Currency> listOfCurrencies = new List<Currency>();

            listOfCurrencies.Add(GetSampleCurrency());

            Currency Vex = GetSampleCurrency();
            Vex.Character_id = Guid.Parse("55555555-4444-3333-2222-111111111111");
            Vex.PlatinumPieces = 1000000;
            listOfCurrencies.Add(Vex);

            Currency Veth = GetSampleCurrency();
            Veth.Character_id = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
            Veth.PlatinumPieces = 0;
            Veth.GoldPieces = 0;
            Veth.ElectrumPieces = 0;
            Veth.SilverPieces = 0;
            Veth.CopperPieces = 1;
            listOfCurrencies.Add(Veth);

            return listOfCurrencies;
        }

        public static Note GetSampleNote()
        {
            Note note = new Note
            {
                Note_id = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
                Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                Name = "Learning to Spell Ft. Grog Strongjaw",
                Contents = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            };
            return note;
        }

        public static List<Note> GetListOfNotes()
        {
            List<Note> notes = new List<Note>();
            notes.Add(GetSampleNote());

            var GreatAxe = new Note()
            {
                Note_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                Name = "How To Use a Great Axe Ft. Grog Strongjaw",
                Contents = "Lorem Ipsum"
            };
            notes.Add(GreatAxe);

            var Tary = new Note()
            {
                Note_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                Character_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f"),
                Name = "The Daring Trials and Tribulations of Taryon Darington",
                Contents = "The quick brown fox jumped over the lazy dog."
            };
            notes.Add(Tary);

            return notes;
        }


        public static Spell GetSampleSpell()
        {
            Spell spell = new Spell
            {
                Spell_id = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
                Name = "Firebolt",
                Description = "Launch a bolt of fire for 1d6 fire damage.",
                Level = 0,
                School_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                CastingTime = "1 Action",
                Range = "30 feet",
                Duration = "instant",
                RequiresVerbal = true,
                RequiresSomantic = true,
                RequiresMaterial = false,
                RequiresConcentration = false
            };
            return spell;
        }

        public static List<Spell> GetListOfSpells()
        {
            List<Spell> spells = new List<Spell>();

            spells.Add(GetSampleSpell());

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

            spells.Add(nineSidedTower);

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
            spells.Add(VoltaicBolt);

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
            spells.Add(WebOfFire);
            return spells;

        }

        public static Material GetSampleMaterial()
        {
            Material material = new Material()
            {
                Spell_id = Guid.Parse("a9756f3d-55d0-40cd-8083-6b547e4932ab"),
                materials = "A bit of fleece"
        };
        return material;
        }

        public static List<Material> GetListOfMaterials()
        {
            List<Material> materials = new List<Material>();
            materials.Add(GetSampleMaterial());

            Material fireball = new Material()
            {
                Spell_id = Guid.Parse("8a179717-960b-42a2-bda7-13914a9daed4"),
                materials = "A tiny ball of bat guano and sulfur."
            };
            materials.Add(fireball);

            Material revivify = new Material()
            {
                Spell_id = Guid.Parse("caf8b2d1-7903-493c-bc3a-ec2fc554d533"),
                materials = "Diamonds worth 300 gp, which the spell consumes."
            };
            materials.Add(revivify);
            return materials;
        }

        public static Spell_Character GetSampleKnownSpell()
        {
            Spell_Character knownSpellRecord = new Spell_Character()
            {
                Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                Spell_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f")
            };
            return knownSpellRecord;
        }
        public static List<Spell_Character> GetListOfKnownSpells()
        {
            List<Spell_Character> knownSpells = new List<Spell_Character>();

            var Caleb_Tower = GetSampleKnownSpell();

            knownSpells.Add(Caleb_Tower);

            Spell_Character Caleb_WebOfFire = new Spell_Character()
            {
                Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                Spell_id = Guid.Parse("51b4c563-2040-4c7d-a23e-cab8d5d3c73b")
            };

            knownSpells.Add(Caleb_WebOfFire);

            Spell_Character Veth_VoltaicBolt = new Spell_Character()
            {
                Character_id = Guid.Parse("9aa6cd47-d784-46e4-9b66-7b1ea10d3386"),
                Spell_id = Guid.Parse("a9756f3d-55d0-40cd-8083-6b547e4932ab")

            };
            knownSpells.Add(Veth_VoltaicBolt);

            Spell_Character Veth_MageHand = new Spell_Character()
            {
                Character_id = Guid.Parse("9aa6cd47-d784-46e4-9b66-7b1ea10d3386"),
                Spell_id = Guid.Parse("a678034d-730c-4b86-a952-c975c04c6291")
            };
            knownSpells.Add(Veth_MageHand);

            return knownSpells;

        }
    }
}
