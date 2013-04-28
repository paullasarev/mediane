using Mediane.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediane.DomainModel
{
    class ContentModelImpl : ContentModel
    {
        public ContentModelImpl(string id)
            : base(id)
        {
        }
    }


    public class ContentModelRepository : IContentModelRepository
    {
        Dictionary<string, ContentModel> Models = new Dictionary<string,ContentModel>();

        public Mediane.DomainModel.ContentModel Create(string id)
        {
            var model = new ContentModelImpl(id);
            model.Content = "New page template";
            return model;
        }

        public Mediane.DomainModel.ContentModel Load(string id)
        {
            string key = id.Trim();
            if (Models.ContainsKey(key))
            {
                return Models[id.Trim()];
            }
            else
            {
                return Create(id);
            }
        }

        public void Save(Mediane.DomainModel.ContentModel model)
        {
            Models[model.Id] = model;
        }
    }
}