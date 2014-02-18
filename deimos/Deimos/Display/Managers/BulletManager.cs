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

        // Methods
        /// <summary>
        /// Add a new bullet to our world.
        /// </summary>
        public void SpawnBullet()
        {
            Bullet FiredBullet = new Bullet(Game, Game.ThisPlayer.Position, Game.Camera.ViewVector);
            BulletTab.Add(FiredBullet);

            Game.SceneManager.ModelManager.LoadPrivateModel(
                "Bullet" + BulletTab.FindIndex(delegate(Bullet bullet) { return bullet == FiredBullet; }).ToString(),
                "Models/Weapons/PP19/PP19Model", // Model
                 new Vector3(10, 0, 0), // Location
                 Vector3.Zero,
                 0.1f,
                 LevelModel.CollisionType.Accurate
            );
        }

        /// <summary>
        /// Destroys the corresponding bullet
        /// </summary>
        /// <param name="bullet"></param>
        private void DestroyBullet(Bullet bullet)
        {
            Game.SceneManager.ModelManager.RemoveLevelModel(
                "Bullet" + BulletTab.FindIndex(delegate(Bullet b) { return b == bullet; }).ToString()
            );
            BulletTab.Remove(bullet);
        }

        /// <summary>
        /// Propagate our bullets, with its direction.
        /// </summary>
        /// <param name="dt"></param>
        public void Propagate(Bullet bullet, float dt)
        {
            bullet.Position += bullet.Direction * bullet.speed * dt;
        }

        /// <summary>
        /// Updates the age of the bullets and if they are too old, destroy them.
        /// </summary>
        /// <param name="dt"></param>
        public void Age(Bullet bullet, float dt)
        {
            bullet.lifeSpan -= dt;
            if (bullet.lifeSpan <= 0)
            {
                DestroyBullet(bullet);
            }
        }

        public void CheckCollision(Bullet bullet)
        {
            
        }

        /// <summary>
        /// Method to be called at each update of the game. Updates every bullets.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            for (int i = 0; i < BulletTab.Count; i++)
            {
                Bullet bullet = BulletTab[i];
                Propagate(bullet, dt);
                Age(bullet, dt);
            }
        }
    }
}
