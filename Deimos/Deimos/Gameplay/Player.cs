using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    public class Player
    {
        Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        Vector3 rotation;
        public Vector3 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        Vector3 lookAt;
        public Vector3 LookAt
        {
            get { return lookAt; }
            set { lookAt = value; }
        }

        public enum Teams
        {
            Humans,
            Aliens
        }
        Teams team;
        internal Teams Team
        {
            get { return team; }
            set { team = value; }
        }

        float speed = 70;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        
    }
}
