using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class ResourceManager
    {
        Dictionary<string, CollidableModel.CollidableModel> ModelList =
            new Dictionary<string,CollidableModel.CollidableModel>();
        ContentManager Content;

        public ResourceManager(ContentManager content)
        {
            Content = content;
        }

        ~ResourceManager()
        {
            Content.Unload();
        }

        public CollidableModel.CollidableModel LoadModel(string name)
        {
            if (ModelList.ContainsKey(name))
            {
                return ModelList[name];
            }
            return Content.Load<CollidableModel.CollidableModel>(name);
        }
    }
}
