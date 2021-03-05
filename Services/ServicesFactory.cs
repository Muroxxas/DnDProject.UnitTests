using DnDProject.Backend.Processors.Interfaces;
using DnDProject.Backend.Services.Implementations;
using DnDProject.Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDProject.UnitTests.Services
{
    public static class ServicesFactory
    {

        public static ICharacterServices GetCharacterService(ICreateCharacter creator, IUpdateCharacter updater, ICharacterCMBuilder builder) 
        { 
            return new CharacterServices(creator, updater, builder);
        }
    }
}
