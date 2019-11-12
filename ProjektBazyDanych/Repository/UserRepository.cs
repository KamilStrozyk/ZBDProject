using ProjektBazyDanych.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjektBazyDanych.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public User GetByEmail(string mail)
        {
            return _context.Users.Where(x => x.email == mail).SingleOrDefault();
        }
    }
}