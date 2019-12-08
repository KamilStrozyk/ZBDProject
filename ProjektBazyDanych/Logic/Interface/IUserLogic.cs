namespace ProjektBazyDanych.Logic.Interface
{
    internal interface IUserLogic
    {
        void AddAdmin();

        void ChangeAdminEmail(string mail);

        User GeneratePassword(User user);
        
    }
}