using DnDProject.Backend.Repository;
using DnDProject.Backend.Unit_Of_Work.Interfaces;
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
        public static IBaseUserAccess getBaseUserAccess(IUnitOfWork worker)
        {
            return new BaseUserAccess(worker);
        }
        public static ISpellManagerUserAccess getSpellManagerUserAccess(IUnitOfWork worker)
        {
            return new SpellManagerUserAccess(worker);
        }
        public static IItemsManagerUserAccess getItemsManagerUserAccess(IUnitOfWork worker)
        {
            return new ItemsManagerUserAccess(worker);
        }
        public static IClassManagerUserAccess GetClassManagerUserAccess(IUnitOfWork worker)
        {
            return new ClassManagerUserAccess(worker);
        }
    }
}
