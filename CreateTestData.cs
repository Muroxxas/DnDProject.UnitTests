using DnDProject.Entities.Character.DataModels;
using DnDProject.Entities.Character.ViewModels;
using DnDProject.Entities.Class.DataModels;
using DnDProject.Entities.Items.DataModels;
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

            spells.Add(WebOfFire);
            spells.Add(EldritchBlast);
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
        public static School GetSampleSchool()
        {
            School evocation = new School()
            {
                School_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                Name = "Evocation"
            };
            return evocation;
        }
        public static List<School> GetListOfSchools()
        {
            List<School> Schools = new List<School>();

            Schools.Add(GetSampleSchool());

            School Conjuration = new School()
            {
                School_id = Guid.Parse("361bd911-0702-437f-ab59-a29da0f9fba4"),
                Name = "Conjuration"
            };
            Schools.Add(Conjuration);

            School Abjuration = new School()
            {
                School_id = Guid.Parse("c73d220e-e0fa-41c7-9f8a-241030620927"),
                Name = "Abjuration"
            };

            Schools.Add(Abjuration);

            return Schools;
        }
        public static Spell_Class GetSampleCastableBy()
        {
            Spell_Class Tower_Wizard = new Spell_Class
            {
                Spell_id = Guid.Parse("46d10bb8-84d2-408d-a928-5847ff99461f"),
                Class_id = Guid.Parse("b74e228f-015d-45b4-af0f-a6781976535a")
            };
            return Tower_Wizard;
        }
        public static List<Spell_Class> GetListOfCastableByRecords()
        {
            List<Spell_Class> castableByList = new List<Spell_Class>();

            Spell_Class Tower_Wizard = GetSampleCastableBy();

            castableByList.Add(Tower_Wizard);

            Spell_Class WebOfFire_Wizard = new Spell_Class()
            {
                Spell_id = Guid.Parse("51b4c563-2040-4c7d-a23e-cab8d5d3c73b"),
                Class_id = Guid.Parse("b74e228f-015d-45b4-af0f-a6781976535a")
            };
            castableByList.Add(WebOfFire_Wizard);

            Spell_Class VoltaicBolt_Wizard = new Spell_Class()
            {
                Spell_id = Guid.Parse("a9756f3d-55d0-40cd-8083-6b547e4932ab"),
                Class_id = Guid.Parse("b74e228f-015d-45b4-af0f-a6781976535a")
            };

            castableByList.Add(VoltaicBolt_Wizard);

            Spell_Class EldritchBlast_Warlock = new Spell_Class()
            {
                Spell_id = Guid.Parse("45c1a8cc-2e3e-4e29-8eeb-f9fa0cc9e27e"),
                Class_id = Guid.Parse("d29f1c68-1d94-462a-bc1e-3f61a7904983")
            };

            castableByList.Add(EldritchBlast_Warlock);
            return castableByList;
        }

        public static Item GetSampleItem()
        {
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
            return Whisper;
        }
        public static List<Item> GetListOfItems()
        {
            List <Item> items = new List<Item>();
            Item whisper = GetSampleItem();
            items.Add(whisper);

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

            items.Add(TitanstoneKnuckles);

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
            items.Add(HealingPotion);

            return items;
        }
        public static Character_Item GetSampleHeldItem()
        {
            Character_Item Vax_Whisper = new Character_Item()
            {
                Item_id = Guid.Parse("709135c3-6f89-46cb-80ae-4097b621e3b0"),
                Character_id = Guid.Parse("e3a0faef-99da-4d15-bff1-b535a42b955c")
            };
            return Vax_Whisper;
        }
        public static List<Character_Item> GetListOfHeldItems()
        {
            List<Character_Item> heldItems = new List<Character_Item>();

            Character_Item Vax_Whisper = GetSampleHeldItem();
            heldItems.Add(Vax_Whisper);

            Character_Item Grog_Knuckles = new Character_Item
            {
                Character_id = Guid.Parse("96992cfa-9e3b-480f-ab06-46539d3666f6"),
                Item_id = Guid.Parse("026a7dff-5e85-4e6d-94c6-6613828e5df6")
            };
            heldItems.Add(Grog_Knuckles);

            Character_Item Vax_Potion = new Character_Item
            {
                Character_id = Guid.Parse("e3a0faef-99da-4d15-bff1-b535a42b955c"),
                Item_id = Guid.Parse("2caa23dc-15e6-4a57-9bb6-62f6d8636ff7")
            };

            heldItems.Add(Vax_Potion);

            return heldItems;

        }
        public static Tag GetSampleTag()
        {
            Tag Weapon = new Tag
            {
                Tag_id = Guid.Parse("172e8478-e1bd-49ba-a7a7-6455d5a58c6e"),
                TagName = "Weapon"
            };
            return Weapon;
        }
        public static List<Tag> GetListOfTags()
        {
            List<Tag> tags = new List<Tag>();
            Tag weapon = GetSampleTag();
            tags.Add(weapon);

            Tag HeavyArmor = new Tag
            {
                Tag_id = Guid.Parse("35d27332-ccc3-40b2-b2d1-a91715ad0917"),
                TagName = "Heavy Armor"
            };
            tags.Add(HeavyArmor);

            Tag Wondorous = new Tag
            {
                Tag_id = Guid.Parse("e2c7f8a3-52ba-4dc2-baaf-4026718b1f03"),
                TagName = "Wondorous Item"
            };
            tags.Add(Wondorous);
            return tags;
        }
        public static Item_Tag GetSampleItemTag()
        {
            Item_Tag Whisper_Weapon = new Item_Tag
            {
                Item_id = Guid.Parse("709135c3-6f89-46cb-80ae-4097b621e3b0"),
                Tag_id = Guid.Parse("172e8478-e1bd-49ba-a7a7-6455d5a58c6e")
            };
            return Whisper_Weapon;
        }
        public static List<Item_Tag> GetListOfItemTags()
        {
            List<Item_Tag> ITRecords = new List<Item_Tag>();

            Item_Tag Whisper_Weapon = GetSampleItemTag();
            ITRecords.Add(Whisper_Weapon);

            Item_Tag Whisper_Wondorous = new Item_Tag
            {
                Item_id = Guid.Parse("709135c3-6f89-46cb-80ae-4097b621e3b0"),
                Tag_id = Guid.Parse("e2c7f8a3-52ba-4dc2-baaf-4026718b1f03")
            };
            ITRecords.Add(Whisper_Wondorous);

            Item_Tag Knuckles_Wondorous = new Item_Tag
            {
                Item_id = Guid.Parse("026a7dff-5e85-4e6d-94c6-6613828e5df6"),
                Tag_id = Guid.Parse("e2c7f8a3-52ba-4dc2-baaf-4026718b1f03")
            };

            ITRecords.Add(Knuckles_Wondorous);

            Item_Tag Knuckles_HeavyArmor = new Item_Tag
            {
                Item_id = Guid.Parse("026a7dff-5e85-4e6d-94c6-6613828e5df6"),
                Tag_id = Guid.Parse("35d27332-ccc3-40b2-b2d1-a91715ad0917")
            };
            ITRecords.Add(Knuckles_HeavyArmor);

            return ITRecords;
        }


        public static PlayableClass GetSampleClass()
        {
            PlayableClass Fighter = new PlayableClass
            {
                Class_id = Guid.Parse("15478d70-f96e-4c14-aeaf-4a1e35605874"),
                Name = "Fighter",
                Description = "Swing swords n stuff!",
                IsCaster = false,
                HitDiceSize = 10,
                casterCapability = 0
            };
            return Fighter;
        }
        public static List<PlayableClass> GetPlayableClasses()
        {
            List<PlayableClass> playableClasses = new List<PlayableClass>();

            PlayableClass Fighter = GetSampleClass();
            playableClasses.Add(Fighter);

            PlayableClass Wizard = new PlayableClass
            {
                Class_id = Guid.Parse("4e82620a-0496-4ecc-b6d4-05faa064310b"),
                Name = "Wizard",
                Description = "Book nerd!",
                IsCaster = true,
                HitDiceSize = 6,
                casterCapability = 1
            };
            playableClasses.Add(Wizard);

            PlayableClass Ranger = new PlayableClass
            {
                Class_id = Guid.Parse("969c08ca-f983-4ddd-b351-31962f2429cd"),
                Name = "Ranger",
                Description = "Bows!",
                IsCaster = true,
                HitDiceSize = 8,
                casterCapability = .5
            };
            playableClasses.Add(Ranger);
            return playableClasses;
            
        }
        public static ClassAbility GetClassAbility()
        {
            ClassAbility SecondWind = new ClassAbility
            {
                ClassAbility_id = Guid.Parse("500f04e7-d080-4ba8-9a22-57a64d5f6a25"),
                Class_id = Guid.Parse("15478d70-f96e-4c14-aeaf-4a1e35605874"),
                Name = "Second Wind",
                Description = "You have a limited well of stamina you can draw upon to protect yourself from harm.",
                LevelLearned = 1
            };
            return SecondWind;
        }
        public static List<ClassAbility>GetListOfClassAbility()
        {
            List<ClassAbility> listofClassAbilities = new List<ClassAbility>();
            ClassAbility SecondWind = GetClassAbility();
            listofClassAbilities.Add(SecondWind);

            ClassAbility SpellMastery = new ClassAbility
            {
                ClassAbility_id = Guid.Parse("19e51104-8590-4199-b7e2-079993bb8ccb"),
                Class_id = Guid.Parse("4e82620a-0496-4ecc-b6d4-05faa064310b"),
                Name = "Spell Master",
                Description = "Choose a 1st level spell and a 2nd level spell in your spellbook. As long as you have them prepared, you can cast them without consuming a spell slot.",
                LevelLearned = 18
            };
            listofClassAbilities.Add(SpellMastery);

            ClassAbility Vanish = new ClassAbility
            {
                ClassAbility_id = Guid.Parse("97bd8231-a001-4228-824f-7606202913b0"),
                Class_id = Guid.Parse("969c08ca-f983-4ddd-b351-31962f2429cd"),
                Name = "Vanish",
                Description = "You can Hide as a bonus action.",
                LevelLearned = 14
            };
            listofClassAbilities.Add(Vanish);
            return listofClassAbilities;           
        }
        public static Character_Class_Subclass GetCharacter_Class_Subclass()
        {
            Character_Class_Subclass Caleb_Wizard_Transmutation = new Character_Class_Subclass
            {
                Character_id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                Class_id = Guid.Parse("4e82620a-0496-4ecc-b6d4-05faa064310b"),
                Subclass_id = Guid.Parse("c8d2e23a-a193-4e06-8814-9180d4830732"),
                RemainingHitDice = 12,
                ClassLevel = 12
            };
            return Caleb_Wizard_Transmutation;
        }
        public static List<Character_Class_Subclass> GetListOfCharacter_Class_Subclass()
        {
            List<Character_Class_Subclass> knownClasses = new List<Character_Class_Subclass>();

            var Caleb_Wizard_Transmutation = GetCharacter_Class_Subclass();
            knownClasses.Add(Caleb_Wizard_Transmutation);

            Character_Class_Subclass Percy_Fighter_Gunslinger = new Character_Class_Subclass
            {
                Character_id = Guid.Parse("6983e8dc-3e3c-4853-ac49-ba33f236723a"),
                Class_id = Guid.Parse("15478d70-f96e-4c14-aeaf-4a1e35605874"),
                Subclass_id = Guid.Parse("a8e9e19f-b04f-4d6c-baf8-ada5cd40c30b"),
                RemainingHitDice = 20,
                ClassLevel = 20
            };
            knownClasses.Add(Percy_Fighter_Gunslinger);
            Character_Class_Subclass Vex_Ranger_BeastMaster = new Character_Class_Subclass
            {
                Character_id = Guid.Parse("da7d6227-d330-44ab-8001-880dbf52110a"),
                Class_id = Guid.Parse("969c08ca-f983-4ddd-b351-31962f2429cd"),
                Subclass_id = Guid.Parse("c7de67ae-3a65-4261-9c09-05a7b0c527bb"),
                RemainingHitDice = 20,
                ClassLevel = 20
            };
            knownClasses.Add(Vex_Ranger_BeastMaster);
            return knownClasses;
        }
    }

}
