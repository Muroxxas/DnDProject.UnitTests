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

            ICreateCharacter toTest = new CreateCharacter(access, GetCharacterCommonFunctions(access));
            return toTest;

        }
        public static IUpdateCharacter getUpdateCharacterProcessor(IBaseUserAccess access)
        {
            return null;
        }
        public static ICharacterCommonFunctions GetCharacterCommonFunctions(IBaseUserAccess access)
        {
            ICharacterCommonFunctions toTest = new CharacterCommonFunctions(access);
            return toTest;
        }

        public static ICharacterCMBuilder GetCharacterCMBuilder(IBaseUserAccess access)
        {
            ICharacterCMBuilder toTest = new CharacterCMBuilder(access);
            return toTest;
        }
    }
}
