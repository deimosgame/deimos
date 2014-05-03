using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    public class Data
    {
        // This class represents data received from the server
        // that will be treated and processed by the client

        public enum Nature
        {
            Player,
            Entity,
            Corrupt
        }

        public enum EntityType
        {
            Bullet,
            Weapon,
            Effect,
            Corrupt
        }

        public enum EntityState
        {
            Active,
            Inactive,
            Corrupt
        }

        public enum WorldDataType
        {
            Name,
            Score,
            Instance,
            Health,
            X,
            Y,
            Z,
            XRotation,
            YRotation,
            XVelocity,
            YVelocity,
            ZVelocity,
            AngularVelocityX,
            AngularVelocityY,
            ModelID,
            CurrentWeapon,
            Corrupt
        }

        public enum Contains
        {
            Byte,
            Int32,
            Float32,
            String
        }

        // ATTRIBUTES
        // Identity-related
        public Nature ID_Type;
        public EntityType Entity_Type;
        public EntityState Entity_State;
        public WorldDataType Data_Type;
        public Contains Contained;

        // Whose property is this shit?
        public byte PropertyOf;

        // Potential data
        public string Str_Data;
        public int Int32_Data;
        public float Float32_Data;
        public byte Byte_Data;

        // To help with handling
        public int end_index;

        // CONSTRUCTOR
        public Data(Nature nature)
        {
            ID_Type = nature;
        }

        public Data Clone()
        {
            Data new_data = new Data(ID_Type);
            new_data.Entity_Type = Entity_Type;
            new_data.Entity_State = Entity_State;
            new_data.Data_Type = Data_Type;
            new_data.Contained = Contained;

            new_data.PropertyOf = PropertyOf;

            new_data.Str_Data = Str_Data;
            new_data.Int32_Data = Int32_Data;
            new_data.Float32_Data = Float32_Data;
            new_data.Byte_Data = Byte_Data;

            new_data.end_index = end_index;

            return new_data;
        }
    }
}
