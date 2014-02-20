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
        private Dictionary<string, Bullet> BulletTab = new Dictionary<string, Bullet>();

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
            Vector3 bulletPosition = Game.ThisPlayer.Position - Game.Camera.ViewVector * 10;
            Bullet FiredBullet = new Bullet(Game, bulletPosition, - Game.Camera.ViewVector);
            string id = "Bullet" + General.Uniqid();
            BulletTab.Add(id, FiredBullet);

            Game.SceneManager.ModelManager.LoadPrivateModel(
                id,
                "Models/Weapons/PP19/PP19Model", // Model
                 bulletPosition, // Location
                 Game.ThisPlayer.Rotation,
                 0.1f,
                 LevelModel.CollisionType.None
            );
        }

        /// <summary>
        /// Destroys the corresponding bullet
        /// </summary>
        /// <param name="bullet"></param>
        private void DestroyBullet(string key)
        {
            Game.SceneManager.ModelManager.RemovePrivateModel(
                key
            );
            BulletTab.Remove(key);
        }

        /// <summary>
        /// Propagate our bullets, with its direction.
        /// </summary>
        /// <param name="dt"></param>
        public void Propagate(string key, Bullet bullet, float dt)
        {
            bullet.Position += bullet.Direction * bullet.speed * dt;
            Game.SceneManager.ModelManager.GetPrivateModel(key).Position = 
                bullet.Position;
        }

        /// <summary>
        /// Updates the age of the bullets and if they are too old, destroy them.
        /// </summary>
        /// <param name="dt"></param>
        public void Age(string key, Bullet bullet, float dt)
        {
            bullet.lifeSpan -= dt;
            if (bullet.lifeSpan <= 0)
            {
                DestroyBullet(key);
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

            // We can't iterate through the dictionnary and edit it at the same
            // time: this is why we need to store the keys in another list.
            List<string> BulletTabKeys = new List<string>();
            foreach (string key in BulletTab.Keys)
            {
                BulletTabKeys.Add(key);
            }
            foreach (string key in BulletTabKeys)
            {
                Bullet bullet = BulletTab[key];
                Propagate(key, bullet, dt);
                Age(key, bullet, dt);
            }
        }
    }
}
