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

        private List<Bullet> BulletTab = new List<Bullet>(); // the tab of bullets that calls functions for them

        // Constructor
        public BulletManager(DeimosGame game)
        {
            Game = game;
        }

        // Destructor
        public void DestroyBullet(Bullet bullet)
        {
            // And call the Bullet destructor??
            BulletTab.Remove(bullet);
        }

        // Methods
        public Bullet SpawnBullet(Weapon weapon)
        {
            Bullet FiredBullet = new Bullet(Game);
            BulletTab.Add(FiredBullet);

            return FiredBullet;
        }

        // These next two functions are purely first build test phase, they WILL be changed.
        public void Propagate(Bullet bulletToFire, float dt)
        {

            bulletToFire.Position = bulletToFire.Position * bulletToFire.Direction * bulletToFire.speed * dt;

            // Collision or Hit check
        }

        public void Update(Bullet bulletToFire, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;


            Propagate(bulletToFire, dt);
        }
    }
}
