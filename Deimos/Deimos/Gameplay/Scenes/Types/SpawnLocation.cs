using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class SpawnLocation
    {
        public Vector3 Location;
        public Vector3 Rotation;

        public SpawnLocation(Vector3 location, Vector3 rotation)
        {
            Location = location;
            Rotation = rotation;
        }
    }
}
