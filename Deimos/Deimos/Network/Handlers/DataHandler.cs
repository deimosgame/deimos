using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class DataHandler
    {
        // These lists store the data according to their nature
        // up to treatment, and then they will be cleared

        List<Data> PlayerData = new List<Data>();
        List<Data> EntityData = new List<Data>();

        public void Handle(Data d)
        {
            if (d.ID_Type == Data.Nature.Player)
            {
                PlayerData.Add(d);
            }
            else if (d.ID_Type == Data.Nature.Entity)
            {
                EntityData.Add(d);
            }
        }

        public void InterpretPlayer(Data data)
        {
            if (data != null
                && NetworkFacade.Players.List.ContainsKey(data.PropertyOf))
            {
                switch (data.Data_Type)
                {
                    case Data.WorldDataType.X:
                        NetworkFacade.Players.List[data.PropertyOf].Position.X =
                            data.Float32_Data;
                        break;
                    case Data.WorldDataType.Y:
                        NetworkFacade.Players.List[data.PropertyOf].Position.Y =
                            data.Float32_Data - 6f;
                        break;
                    case Data.WorldDataType.Z:
                        NetworkFacade.Players.List[data.PropertyOf].Position.Z =
                            data.Float32_Data;
                        break;
                    case Data.WorldDataType.YRotation:
                        NetworkFacade.Players.List[data.PropertyOf].Rotation.Y =
                            data.Float32_Data;
                        break;
                    case Data.WorldDataType.Active:
                        if (data.Byte_Data == 0x00)
                        {
                            GameplayFacade.ScoresInterface.AddDeath(
                                "FFA", 
                                NetworkFacade.Players.List[data.PropertyOf].Name);
                        }
                        NetworkFacade.Players.List[data.PropertyOf].Alive = data.Byte_Data;
                        break;
                    case Data.WorldDataType.Score:
                        if (data.Byte_Data < NetworkFacade.Players.List[data.PropertyOf].Score)
                        {
                            GameplayFacade.ScoresInterface.AddDeath(
                                "FFA",
                                NetworkFacade.Players.List[data.PropertyOf].Name);
                        }
                        else if (data.Byte_Data > NetworkFacade.Players.List[data.PropertyOf].Score)
                        {
                            GameplayFacade.ScoresInterface.AddKill(
                                "FFA",
                                NetworkFacade.Players.List[data.PropertyOf].Name);
                        }
                        NetworkFacade.Players.List[data.PropertyOf].Score =
                            data.Byte_Data;
                        break;
                    case Data.WorldDataType.CurrentWeapon:
                        NetworkFacade.Players.List[data.PropertyOf].WeaponModel =
                            data.Byte_Data;
                        break;
                    case Data.WorldDataType.Name:
                        NetworkFacade.Players.List[data.PropertyOf].Name =
                            data.Str_Data;
                        break;
                    case Data.WorldDataType.Instance:
                        NetworkFacade.Players.List[data.PropertyOf].CurrentInstance =
                            data.Str_Data;
                        break;
                    case Data.WorldDataType.ModelID:
                        NetworkFacade.Players.List[data.PropertyOf].Model =
                            data.Byte_Data;
                        break;
                }
            }
        }

        public void InterpretEntity(Data data)
        {
            if (data != null)
            {

            }
        }

        public void Process()
        {
                // Allocating remove list
            List<Data> ToBeRemoved = new List<Data>();


                // We process all current player data and add to remove list
            for (int i = 0; i < PlayerData.Count; i++)
            {
                Data player = PlayerData.ElementAt(i);
                InterpretPlayer(player);
                ToBeRemoved.Add(player);
            }

            // We process all current entity data and add to remove list
            for (int i = 0; i < EntityData.Count; i++)
            {
                Data entity = EntityData.ElementAt(i);
                InterpretEntity(entity);
                ToBeRemoved.Add(entity);
            }


                // Removing data that has been treated
            foreach (Data d in ToBeRemoved)
            {
                if (d != null
                    && d.ID_Type == Data.Nature.Player)
                {
                    PlayerData.Remove(d);
                }
                else if (d != null)
                {
                    EntityData.Remove(d);
                }
            }
        }


    }
}
