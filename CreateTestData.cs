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
    }
}
