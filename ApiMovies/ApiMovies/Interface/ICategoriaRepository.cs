using ApiMovies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApiMovies.Interface.IGenericRepository
{
  
        public interface IGenericRepository<T> where T : class
        {
            IEnumerable<T> GetAll();
            T GetById(object id);
            IEnumerable<T> GetByValues(Func<T, bool> values);
            bool Exist(Func<T, bool> values);
            bool Add(T obj);
            bool Update(T obj, int id = 0);
            bool Delete(object id);
            bool Save();
        }

}
