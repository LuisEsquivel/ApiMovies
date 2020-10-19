using ApiMovies.Data;
using ApiMovies.Interface.IGenericRepository;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMovies.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private ApplicationDbContext _context;
        private DbSet<T> table = null;

        
        public GenericRepository(ApplicationDbContext context)
        {
            this._context = context;
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

        public void Add(T obj)
        {
            table.Add(obj);
            Save();
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
            Save();
        }

        public void Delete(object id)
        {
            T row = table.Find(id);

            if (row != null)
            {
                table.Remove(row);
                Save();
            }

        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
