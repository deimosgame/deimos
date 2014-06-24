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
        // the tab of bullets that calls functions for them
        private Dictionary<string, Bullet> BulletTab = new Dictionary<string, Bullet>();

        // Constructor
        public BulletManager()
        {
            //
        }

        // Methods
        /// <summary>
        /// Add a new bullet to our world.
        /// </summary>
        public void SpawnBullet(char rep)
        {
            Vector3 bulletPosition = GameplayFacade.ThisPlayer.Position - DisplayFacade.Camera.ViewVector * 1;
            Bullet FiredBullet = new Bullet(bulletPosition, -DisplayFacade.Camera.ViewVector, rep);
            string id = "Bullet" + GeneralFacade.Uniqid();
            BulletTab.Add(id, FiredBullet);

            GeneralFacade.SceneManager.ModelManager.LoadPrivateModel(
                id,
                "Models/Weapons/PP19/PP19Model", // Model
                 bulletPosition, // Location
                 GameplayFacade.ThisPlayer.Rotation,
                 0.025f,
                 LevelModel.CollisionType.None
            );

            if (!NetworkFacade.Local)
            {
                NetworkFacade.MainHandling.Bullets.Send(FiredBullet);
            }
        }

        public void SpawnOtherBullet(Vector3 pos, Vector3 director, string rep, Vector3 rotation, float velo, float span)
        {
            Vector3 bulletPosition = pos;
            Bullet FiredBullet = new Bullet(bulletPosition, director, rep[0]);
            FiredBullet.Own = false;
            FiredBullet.speed = velo;
            FiredBullet.lifeSpan = span;
            string id = "Bullet" + GeneralFacade.Uniqid();
            BulletTab.Add(id, FiredBullet);

            GeneralFacade.SceneManager.ModelManager.LoadPrivateModel(
                id,
                "Models/Weapons/PP19/PP19Model", // Model
                 bulletPosition, // Location
                 rotation,
                 0.025f,
                 LevelModel.CollisionType.None
            );
        }

        public void SpawnMelee(char rep)
        {
            Vector3 bulletPosition = GameplayFacade.ThisPlayer.Position - DisplayFacade.Camera.ViewVector * 1;
            Bullet FiredBullet = new Bullet(bulletPosition, -DisplayFacade.Camera.ViewVector, rep);
            FiredBullet.lifeSpan = 0.5f;
            string id = "Bullet" + GeneralFacade.Uniqid();
            BulletTab.Add(id, FiredBullet);

            GeneralFacade.SceneManager.ModelManager.LoadPrivateModel(
                id,
                "Models/Weapons/PP19/PP19Model", // Model
                 bulletPosition, // Location
                 GameplayFacade.ThisPlayer.Rotation,
                 0.025f,
                 LevelModel.CollisionType.None
            );

            GeneralFacade.SceneManager.ModelManager.GetPrivateModel(id).show = false;
        }

        /// <summary>
        /// Destroys the corresponding bullet
        /// </summary>
        /// <param name="bullet"></param>
        private void DestroyBullet(string key)
        {
            GeneralFacade.SceneManager.ModelManager.RemovePrivateModel(
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
            if (bullet.Collided)
            {
                return;
            }
            Vector3 oldPos = bullet.Position;
            for (float i = dt / 10; i < dt; i += dt / 10)
            {
                bullet.Position = oldPos + (bullet.Direction * bullet.speed * i);
                if (bullet.CheckCollision(bullet.Position))
                {
                    break;
                }
            }
            GeneralFacade.SceneManager.ModelManager.GetPrivateModel(key).Position = 
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
