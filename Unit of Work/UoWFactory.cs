using DnDProject.Backend.Contexts;
using DnDProject.Backend.Unit_Of_Work.Implementations;
using DnDProject.Backend.Unit_Of_Work.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDProject.UnitTests.Unit_of_Work
{
    public static class UoWFactory
    {
        public static IUnitOfWork GetUnitOfWork(CharacterContext context)
        {
            return new UnitOfWork(context);
        }
    }
}
