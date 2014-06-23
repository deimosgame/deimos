using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class PlayerManager
    {
        public Dictionary<byte, Player> List = new Dictionary<byte, Player>();
        public Dictionary<byte, CollisionElement> CollisionsList = new Dictionary<byte, CollisionElement>();

        public void LoadModels()
        {
            foreach (KeyValuePair<byte, Player> p in List)
            {
                if (!GeneralFacade.SceneManager.ModelManager.LevelModelExists(p.Value.Name))
                {
                    GeneralFacade.SceneManager.ModelManager.LoadModel(
                        p.Value.Name,
                        p.Value.GetModelName(),
                        p.Value.Position,
                        p.Value.Rotation,
                        5,
                        LevelModel.CollisionType.None
                        );

                    CollisionElement PlayerCollision = new CollisionElement(
                        new Vector2(3, 5));
                    PlayerCollision.Nature = CollisionElement.ElementNature.Player;
                    PlayerCollision.Owner = p.Key;

                    CollisionsList.Add(p.Key, PlayerCollision);
                }
            }
        }

        public void Update()
        {
            for (int i = 0; i < List.Count; i++)
            {
                KeyValuePair<byte, Player> pair = List.ElementAt(i);

                if (pair.Value != null
                    && GeneralFacade.SceneManager.ModelManager.LevelModelExists(pair.Value.Name))
                {
                    if (pair.Value.Alive == 0x01 && (pair.Value.CurrentInstance == GameplayFacade.ThisPlayer.CurrentInstance))
                    {
                        GeneralFacade.SceneManager.ModelManager.GetLevelModel(pair.Value.Name).show = true;
                    }
                    else
                    {
                        GeneralFacade.SceneManager.ModelManager.GetLevelModel(pair.Value.Name).show = false;
                    }

                    GeneralFacade.SceneManager.ModelManager.GetLevelModel(pair.Value.Name).Position =
                        pair.Value.Position;
                    GeneralFacade.SceneManager.ModelManager.GetLevelModel(pair.Value.Name).Rotation =
                        pair.Value.Rotation;

                    CollisionsList[pair.Key].CheckCollision(pair.Value.Position);
                }
            }
        }
    }
}
