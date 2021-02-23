using Autofac.Extras.Moq;
using DnDProject.Backend.Unit_Of_Work.Implementations;
using DnDProject.Backend.Unit_Of_Work.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDProject.UnitTests.Unit_Of_Work
{
    public static class UoW_Factory
    {
        public static IUnitOfWork getUnitofWork(AutoMock mockContext)
        {
            return mockContext.Create<UnitOfWork>();
        }
    }
}
