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
            if (NetworkFacade.Players.ContainsKey(data.PropertyOf))
            {
                switch (data.Data_Type)
                {
                    case Data.WorldDataType.X:
                        NetworkFacade.Players[data.PropertyOf].Position.X =
                            data.Float32_Data;
                        break;
                    case Data.WorldDataType.Y:
                        NetworkFacade.Players[data.PropertyOf].Position.Y =
                            data.Float32_Data;
                        break;
                    case Data.WorldDataType.Z:
                        NetworkFacade.Players[data.PropertyOf].Position.Z =
                            data.Float32_Data;
                        break;
                    case Data.WorldDataType.Score:
                        NetworkFacade.Players[data.PropertyOf].Score =
                            data.Byte_Data;
                        break;
                    case Data.WorldDataType.CurrentWeapon:
                        NetworkFacade.Players[data.PropertyOf].CurrentWeapon.Path =
                            data.Str_Data;
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
                            data.Str_Data;
                        break;
                }
            }
        }

        public void InterpretEntity(Data data)
        {

        }

        public void Process()
        {
                // Making copies of the two lists to avoid thread mixup
            List<Data> PlayerCopy = PlayerData;
            List<Data> EntityCopy = EntityData;

                // Allocating remove list
            List<Data> ToBeRemoved = new List<Data>();


                // We process all current player data and add to remove list
            foreach (Data d in PlayerCopy)
            {
                InterpretPlayer(d);

                ToBeRemoved.Add(d);
            }

                // We process all current entity data and add to remove list
            foreach (Data d in EntityCopy)
            {
                InterpretEntity(d);

                ToBeRemoved.Add(d);
            }


                // Removing data that has been treated
            foreach (Data d in ToBeRemoved)
            {
                if (d.ID_Type == Data.Nature.Player)
                {
                    PlayerData.Remove(d);
                }
                else
                {
                    EntityData.Remove(d);
                }
            }
        }


    }
}
