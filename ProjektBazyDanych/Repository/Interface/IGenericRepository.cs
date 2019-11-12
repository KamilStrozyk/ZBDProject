using System.Collections.Generic;

namespace ProjektBazyDanych.Repository.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        T GetById(object id);

        void Insert(T obj);

        void Update(T obj);

        void Delete(object id);

        void Save();
    }