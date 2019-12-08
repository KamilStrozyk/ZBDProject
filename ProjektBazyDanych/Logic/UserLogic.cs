using ProjektBazyDanych.Logic.Interface;
using ProjektBazyDanych.Repository;
using ProjektBazyDanych.Repository.Interface;
using System.Security.Cryptography;
using SimpleHashing;
using System;

namespace ProjektBazyDanych.Logic
{
    public class UserLogic : IUserLogic
    {
        private IUserRepository UserRepository = new UserRepository();
        private PasswordLogic passwordLogic = new PasswordLogic();

        public void AddAdmin()
        {
            Tuple<string, string> password = passwordLogic.HashWithSalt("admin");
            var admin = new User
            {
                login = "admin",
                email = "admin@admin.com",
                firstName = "admin",
                lastName = "admin",
                passwordSalt = password.Item1,
                passwordHash = password.Item2,

            };
            UserRepository.Insert(admin);
        }

        public void ChangeAdminEmail(string mail)
        {
            var admin = UserRepository.GetById("admin");
            admin.email = mail;
            UserRepository.Update(admin);
        }

        public User GeneratePassword(User user)
        {
            var password = user.passwordSalt;
            var passwordTuple = passwordLogic.HashWithSalt(password);
            user.passwordSalt = passwordTuple.Item1;
            user.passwordHash = passwordTuple.Item2;
            return user;
        }
    }
}