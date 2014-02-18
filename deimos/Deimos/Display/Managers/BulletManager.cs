using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class BulletManager
    {

        // Attributes
        DeimosGame Game;

        // the tab of bullets that calls functions for them
        private List<Bullet> BulletTab = new List<Bullet>(); 

        // Constructor
        public BulletManager(DeimosGame game)
        {
            Game = game;
        }

        private void DestroyBullet(Bullet bullet)
        {
            // And call the Bullet destructor??
            BulletTab.Remove(bullet);
        }

        // Methods
        public Bullet SpawnBullet()
        {
            Bullet FiredBullet = new Bullet(Game);
            BulletTab.Add(FiredBullet);

            return FiredBullet;
        }

        // These next two functions are purely first build test phase, 
        // they WILL be changed.
        public void Propagate(float dt)
        {
            foreach (Bullet bullet in BulletTab)
            {
                bullet.Position = bullet.Position * bullet.Direction * bullet.speed * dt;
            }  
        }

        public void Age(float dt)
        {
            foreach (var bullet in BulletTab)
            {
                bullet.lifeSpan -= dt;
                if (bullet.lifeSpan <= 0)
                {
                    BulletTab.Remove(bullet);
                }
            }
        }

        public void Update(Bullet bulletToFire, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;


            Propagate(dt);
        }
    }
}
