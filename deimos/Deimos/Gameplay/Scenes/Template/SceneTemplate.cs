using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    abstract class SceneTemplate
    {
        // Template class
        abstract public void Load();

        abstract public void Initialize();

        abstract public void Update(float dt);

        virtual public float AmbiantLight
        {
            get
            {
                return 0.25f;
            }
        }

        public Vector3 PlayerSize;

        public ObjectManager Objects;

        public SpawnLocation[] SpawnLocations;
    }
}
