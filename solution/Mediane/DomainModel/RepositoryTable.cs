using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediane.DomainModel
{
    public class RepositoryTable
    {
        private static RepositoryTable RepositoriesValue = new RepositoryTable();
        public static RepositoryTable Repositories
        { 
            get 
            {
                return RepositoriesValue;
            } 
        }

        Dictionary<System.Int32, object> Repos = new Dictionary<System.Int32, object>();

        public T1 Locate<T1>()
        {
            System.Int32 key = typeof(T1).GetHashCode();
            return (T1)Repos[key];
        }

        public void Register<T1>(T1 repository)
        {
            System.Int32 key = typeof(T1).GetHashCode();
            Repos[key] = repository;
        }

        public void Clear()
        {
            Repos.Clear();
        }

        public void Clear<T1>()
        {
            System.Int32 key = typeof(T1).GetHashCode();
            Repos.Remove(key);
        }
    }
}