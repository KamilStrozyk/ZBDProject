using ProjektBazyDanych.Logic.Interface;
using SimpleHashing.Net;
using System;

namespace ProjektBazyDanych.Logic
{
    public class PasswordLogic : IPasswordLogic
    {
        public Tuple<string, string> HashWithSalt(string password)
        {
            ISimpleHash simpleHash = new SimpleHash();
            string hashedPassword = simpleHash.Compute(password);
            return new Tuple<string, string>(hashedPassword.Substring(0,hashedPassword.Length/2), hashedPassword.Substring(hashedPassword.Length / 2));
        }

        public bool TestPasswordHasher(string password, string dbSalt, string dbHash)
        {
            ISimpleHash simpleHash = new SimpleHash();
            return simpleHash.Verify(password, String.Concat(dbSalt, dbHash));
        }
    }
}