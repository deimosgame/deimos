using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deimos.Facades;

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
                && NetworkFacade.Players.ContainsKey(data.PropertyOf))
            {
                switch (data.Data_Type)
                {
                    case Data.WorldDataType.X:
                        DisplayFacade.DebugScreen.Debug("x? " + data.Float32_Data.ToString());
                        NetworkFacade.Players[data.PropertyOf].Position.X =
                            data.Float32_Data;
                        break;
                    case Data.WorldDataType.Y:
                        DisplayFacade.DebugScreen.Debug("y?" + data.Float32_Data.ToString());
                        NetworkFacade.Players[data.PropertyOf].Position.Y =
                            data.Float32_Data - 6;
                        break;
                    case Data.WorldDataType.Z:
                        DisplayFacade.DebugScreen.Debug("z?" + data.Float32_Data.ToString());
                        NetworkFacade.Players[data.PropertyOf].Position.Z =
                            data.Float32_Data;
                        break;
                    case Data.WorldDataType.YRotation:
                        NetworkFacade.Players[data.PropertyOf].Rotation.Y =
                            data.Float32_Data;
                        break;
                    case Data.WorldDataType.Score:
                        NetworkFacade.Players[data.PropertyOf].Score =
                            data.Byte_Data;
                        break;
                    case Data.WorldDataType.CurrentWeapon:
                        NetworkFacade.Players[data.PropertyOf].WeaponModel =
                            data.Byte_Data;
                        break;
                    case Data.WorldDataType.Name:
                        NetworkFacade.Players[data.PropertyOf].Name =
                            data.Str_Data;
                        break;
                    case Data.WorldDataType.Instance:
                        NetworkFacade.Players[data.PropertyOf].Instance =
                            data.Str_Data;
                        break;
                    case Data.WorldDataType.ModelID:
                        NetworkFacade.Players[data.PropertyOf].Model =
                            data.Byte_Data;
                        break;
                }
            }
        }

        public void InterpretEntity(Data data)
        {

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
