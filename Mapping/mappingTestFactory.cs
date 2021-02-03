using DnDProject.Backend.Mapping.Implementations;
using DnDProject.Backend.Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDProject.UnitTests.Mapping
{
    public static class mappingTestFactory
    { 
        public static ICharacterMapper getCharacterMapper()
        {
            return new CharacterMapper();
        }
    }
}
