using ProjektBazyDanych.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjektBazyDanych.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public connectionString _context = null;
        public DbSet<T> table = null;
        public GenericRepository()
        {
            this._context = new connectionString();
            table = _context.Set<T>();
        }
        public GenericRepository(connectionString _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }
        public T GetById(object id)
        {
            return table.Find(id);
        }
        public void Insert(T obj)
        {
            table.Add(obj);
        }
        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }
        public void Delete(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }

}