using Mediane.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediane
{
    public static class RepositoryConfig
    {
        public static void RegisterRepositories(RepositoryTable repository)
        {
            repository.Register<IContentModelRepository>(new ContentModelRepository());
        }
    }
}