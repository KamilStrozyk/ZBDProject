using ProjektBazyDanych.Logic.Interface;
using ProjektBazyDanych.Repository;
using ProjektBazyDanych.Repository.Interface;

namespace ProjektBazyDanych.Logic
{
    public class UserLogic : IUserLogic
    {
        private IUserRepository UserRepository = new UserRepository();

        public void AddAdmin()
        {
            var admin = new User
            {
                login = "admin",
                email = "admin@admin.com",
                passwordHash = "2137",
                passwordSalt = "69",

            };
            UserRepository.Insert(admin);
        }

        public void ChangeAdminEmail(string mail)
        {
            var admin = UserRepository.GetById("admin");
            admin.email = mail;
            UserRepository.Update(admin);
        }
    }
}