using DnDProject.Backend.Repository;
using DnDProject.Backend.UserAccess.Implementations;
using DnDProject.Backend.UserAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDProject.UnitTests.UserAccess
{
    public static class UserAccessFactory
    {
        public static IBaseUserAccess getBaseUserAccess(IDataRepository dataRepository)
        {
            return new BaseUserAccess(dataRepository);
        }
    }
}
