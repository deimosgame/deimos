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
        public Dictionary<byte, byte> PlayerWeapons = new Dictionary<byte, byte>();

        public void LoadModels()
        {
            foreach (KeyValuePair<byte, Player> p in List)
            {
                if (!GeneralFacade.SceneManager.ModelManager.LevelModelExists(p.Value.Name))
                {
                    PlayerCollision PlayerCollision = new PlayerCollision(5.5f, 3);
                    PlayerCollision.Owner = p.Key;

                    PlayerCollision.Model = GeneralFacade.SceneManager.ModelManager.LoadModelReturn(
                        p.Value.Name,
                        p.Value.GetModelName(),
                        p.Value.Position,
                        p.Value.Rotation,
                        4,
                        LevelModel.CollisionType.Sphere,
                        PlayerCollision
                        );

                    if (!CollisionsList.ContainsKey(p.Key))
                    {
                        CollisionsList.Add(p.Key, PlayerCollision);
                    }

                    //if (!PlayerWeapons.ContainsKey(p.Key))
                    //{
                    //    PlayerWeapons.Add(p.Key, 0xFF);
                    //}
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
                        GeneralFacade.SceneManager.ModelManager.GetLevelModel(pair.Value.Name).CollisionDetection = LevelModel.CollisionType.Sphere;
                    }
                    else
                    {
                        GeneralFacade.SceneManager.ModelManager.GetLevelModel(pair.Value.Name).show = false;
                        GeneralFacade.SceneManager.ModelManager.GetLevelModel(pair.Value.Name).CollisionDetection = LevelModel.CollisionType.None;
                    }

                    GeneralFacade.SceneManager.ModelManager.GetLevelModel(pair.Value.Name).Position =
                        pair.Value.Position;
                    GeneralFacade.SceneManager.ModelManager.GetLevelModel(pair.Value.Name).Rotation =
                        pair.Value.Rotation;

                    if (CollisionsList.ContainsKey(pair.Key))
                    {
                        CollisionsList[pair.Key].CheckCollision(pair.Value.Position);
                    }

                    //if (PlayerWeapons[pair.Key] != pair.Value.WeaponModel)
                    //{
                    //    if (GeneralFacade.SceneManager.ModelManager.LevelModelExists("w" + pair.Key))
                    //    {
                    //        GeneralFacade.SceneManager.ModelManager.GetLevelModel("w" + pair.Key).show = false;
                    //    }
                    //    else
                    //    {
                    //        GeneralFacade.SceneManager.ModelManager.LoadModel(
                    //            "w" + pair.Key,
                    //            GetModelFromByte(pair.Value.WeaponModel),
                    //            pair.Value.Position,
                    //            pair.Value.Rotation,
                    //            GetScaleFromByte(pair.Value.WeaponModel),
                    //            LevelModel.CollisionType.None
                    //        );
                    //    }
                    //}

                    //PlayerWeapons[pair.Key] = pair.Value.WeaponModel;
                    //DisplayEnemyWeapons();
                }
            }
        }

        public void DisplayEnemyWeapons()
        {
            for (int i = 0; i < PlayerWeapons.Count; i++)
            {
                KeyValuePair<byte, byte> pair = PlayerWeapons.ElementAt(i);

                if (GeneralFacade.SceneManager.ModelManager.LevelModelExists("w" + pair.Key))
                {
                    GeneralFacade.SceneManager.ModelManager.GetLevelModel(
                        "w" + pair.Key).Position = List[pair.Key].Position;
                    GeneralFacade.SceneManager.ModelManager.GetLevelModel(
                        "w" + pair.Key).Rotation = List[pair.Key].Rotation;
                }
            }
        }

        private string GetModelFromByte(byte b)
        {
            switch (b)
            {
                case 0x00:
                    return "Models/Weapons/Knife/knife";
                case 0x01:
                    return "Models/Weapons/M9/Handgun";
                case 0x02:
                    return "Models/Weapons/PP19/PP19Model";
                case 0x03:
                    return "Models/Weapons/Arbiter/arbiter";
                case 0x04:
                    return "Models/Weapons/RPG/RPG";
                case 0x05:
                    return "Models/Weapons/PP19/PP19Model";
                default:
                    return "";
            }
        }

        private float GetScaleFromByte(byte b)
        {
            switch (b)
            {
                case 0x00:
                    return 0.1f;
                case 0x01:
                    return 0.001f;
                case 0x02:
                    return 0.05f;
                case 0x03:
                    return 1;
                case 0x04:
                    return 0.01f;
                case 0x05:
                    return 0.05f;
                default:
                    return 0;
            }
        }
    }
}
