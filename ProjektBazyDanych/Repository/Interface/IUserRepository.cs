using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektBazyDanych.Repository.Interface
{
    interface IUserRepository : IGenericRepository<User>
    {
        User GetByEmail(string mail);
    }
}
