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

        public void Unload()
        {
            Content.Unload();
        }

        public CollidableModel.CollidableModel LoadModel(string name)
        {
            if (ModelList.ContainsKey(name))
            {
                return ModelList[name];
            }

            CollidableModel.CollidableModel thisContent = null;
            try
            {
                thisContent = Content.Load<CollidableModel.CollidableModel>(name);

                ModelList.Add(name, thisContent);
            }
            catch
            {
                // error
            }
            return thisContent;
        }
    }
}
