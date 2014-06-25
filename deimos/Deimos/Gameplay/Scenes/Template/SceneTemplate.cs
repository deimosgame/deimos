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

        public Vector2 PlayerSize;

        public float TimeLimit;
        public float Elapsed = 0;

        public ObjectManager Objects;
        public SecretsManager Secrets;

        public Dictionary<byte, SpawnLocation> Spawns;

        public SpawnLocation GetRandomSpawn()
        {
            Random rng = new Random();

            byte b = (byte)(rng.Next(0, Spawns.Count));

            return Spawns[b];
        }
    }
}
