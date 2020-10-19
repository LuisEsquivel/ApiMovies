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
            bool Exist(object value);
            bool Add(T obj);
            bool Update(T obj);
            bool Delete(object id);
            bool Save();
        }

}
