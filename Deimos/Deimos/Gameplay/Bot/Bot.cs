using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class Bot : Player
    {
        // Attributes
        public DeimosGame Game;
        public BotPhysics Physics;
        public BotMovement Movement;

        // Constructor
        public Bot(DeimosGame game)
        {
            Game = game;

            Physics = new BotPhysics(Game, this);
            Movement = new BotMovement(Game, this);
        }

        // Methods
        public void Update(float dt)
        {
            if (IsAlive())
            {
                //Physics.ApplyGravity(dt);
                //Physics.Jump(false);
                Movement.HandleMovement(dt);
            }
        }
    }
}
