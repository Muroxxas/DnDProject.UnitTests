using DnDProject.Backend.Processors.Implementations;
using DnDProject.Backend.Processors.Interfaces;
using DnDProject.Backend.UserAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDProject.UnitTests.Processors
{
    public static class ProcessorFactory
    {
        public static ICreateCharacter getCreateCharacterProcessor(IBaseUserAccess access)
        {

            ICreateCharacter toTest = new CreateCharacter(access);
            return toTest;

        }
        public static ICharacterCommonFunctions GetCharacterCommonFunctions(IBaseUserAccess access)
        {
            ICharacterCommonFunctions toTest = new CharacterCommonFunctions(access);
            return toTest;
        }
    }
}
