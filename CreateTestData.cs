using DnDProject.Entities.Character.DataModels;
using DnDProject.Entities.Character.ViewModels;
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
    }
}
