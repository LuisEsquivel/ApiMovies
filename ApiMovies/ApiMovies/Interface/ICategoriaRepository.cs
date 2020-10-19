using ApiMovies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMovies.Interface.IGenericRepository
{
  
        public interface IGenericRepository<T> where T : class
        {
            IEnumerable<T> GetAll();
            T GetById(object id);
            void Add(T obj);
            void Update(T obj);
            void Delete(object id);
            void Save();
        }

}
