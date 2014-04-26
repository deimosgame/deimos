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

        abstract public void Update();

        public Vector3 PlayerSize;

        public ObjectManager Objects;
    }
}
