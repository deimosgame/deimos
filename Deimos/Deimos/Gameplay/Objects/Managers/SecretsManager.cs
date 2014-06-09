using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class SecretsManager
    {
        private Dictionary<string, SecretWall> Walls =
            new Dictionary<string, SecretWall>();

        int n_walls = 0;

        public SecretsManager()
        {

        }

        // Methods
        public string GenerateWallToken()
        {
            string t = "sw";
            t += n_walls.ToString();
            return t;
        }

        public string AddWall(string texture, string path, Vector3 position,
            SecretObject.State state,
            float respawn, Vector3 dimensions, float scale,
            Vector3 rotation = default(Vector3))
        {
            SecretWall wall = new SecretWall(dimensions, path, scale);
            wall.Manager = this;
            wall.Name = texture;
            wall.Position = position;
            wall.T_Respawn = respawn;
            wall.Token = GenerateWallToken();
            n_walls++;
            wall.Status = state;

            Walls.Add(wall.Token, wall);

            GeneralFacade.SceneManager.ModelManager.LoadModel(wall.Token,
                path, position, rotation, scale);

            return wall.Token;
        }

        public void RemoveWall(string token)
        {
            if (Walls.ContainsKey(token))
            {
                Walls.Remove(token);
                n_walls--;

                // unloading the models
                GeneralFacade.SceneManager.ModelManager.RemoveLevelModel(token);
            }
        }

        public void HandleWallDiscovery(string token)
        {
            if (Walls.ContainsKey(token))
            {
                GeneralFacade.SceneManager.ModelManager.GetLevelModel(
                    token).CollisionDetection = LevelModel.CollisionType.None;

                GeneralFacade.SceneManager.ModelManager.GetLevelModel(
                    token).show = false;

                Walls[token].Status = SecretObject.State.Discovered;
            }
            else
            {
                return;
            }
        }

        public void Update(float dt)
        {
            foreach (KeyValuePair<string, SecretWall> wall in Walls)
            {
                if (wall.Value.Status == SecretObject.State.Inactive)
                {
                    return;
                }
                else if (wall.Value.Status == SecretObject.State.Undiscovered)
                {
                    wall.Value.CheckCollision(wall.Value.Position);
                }
                else
                {
                    if (wall.Value.respawn_timer >= wall.Value.T_Respawn)
                    {
                        // activating the respawned object
                        wall.Value.Status = SecretObject.State.Undiscovered;

                        // showing once again the now active object
                        GeneralFacade.SceneManager.ModelManager.GetLevelModel(
                            wall.Key).show = true;

                        // Putting collision back
                        GeneralFacade.SceneManager.ModelManager.GetLevelModel(
                            wall.Key).CollisionDetection =
                            LevelModel.CollisionType.Accurate;

                        // resetting its respawn timer
                        wall.Value.respawn_timer = 0;
                    }
                    else
                    {
                        // incrementing the respawn timer
                        wall.Value.respawn_timer += dt;
                    }
                }
            }
        }
    }
}
