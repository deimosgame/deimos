using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deimos.Facades;

namespace Deimos
{
    public class Subhandler
    {
        // Lists to store packets in processing
        public List<Packet> Ongoing = new List<Packet>();
        public List<Packet> ToBeAssembled = new List<Packet>();

        // COMMON METHODS FOR ALL SUBHANDLERS

        // This method checks the sum of the packet and returns a boolean
        public bool CheckSum(Packet pack)
        {
            return true;
        }
        // This is the general method to handle a received packet
        // and add it to its corresponding list
        public void Handle(byte[] buf)
        {
            // Converting the byte buffer to a new packet of its type
            Packet Received = new Packet(GetTypeFromID(buf[1]));

            Received.Checksum = buf[0];
            Received.Packet_ID = buf[1];
            Received.Index = buf[2];
            Received.Total_Packets = buf[3];

            if (Received.Type == Packet.PacketType.Broadcast)
            {
                Received.Unique_ID = ExtractInt32(buf, 4);
                NetworkFacade.MainHandling.Broadcasts.ConfirmReceipt(Received.Unique_ID);
            }

            // Copying the buffer into the packet
            Received.Encoded_buffer = buf;

            if (Received.Total_Packets > 1)
            {
                Received.Split = true;
                ToBeAssembled.Add(Received);
                return;
            }

            Ongoing.Add(Received);
        }

        // This method return the PacketType from a byte
        public Packet.PacketType GetTypeFromID(byte b)
        {
            switch (b)
            {
                case 0x00:
                    return Packet.PacketType.Handshake;
                case 0x01:
                    return Packet.PacketType.Connection;
                case 0x02:
                    return Packet.PacketType.Disconnection;
                case 0x03:
                    return Packet.PacketType.Chat;
                case 0x04:
                    return Packet.PacketType.Broadcast;
                case 0x05:
                    return Packet.PacketType.Move;
                default:
                    return Packet.PacketType.Corrupt;
            }
        }

        // This method extracts the string whose first char is
        // located at the init index
        public string ExtractString(byte[] buf, int init)
        {
            // allocating our string
            string data;

            // allocating a non-optimized buffer
            // strings should not exceed 571 bytes meaning 571 characters
            byte[] tmp = new byte[571];

            // traverser int
            int i = init;

            while (i < buf.Length
                && buf[i] != 0x00)
            {
                tmp[i - init] = buf[i];
                i++;
            }

            // creating our actual buffer
            byte[] new_buf = new byte[i - init];

            // copying the old buffer inside the actual one
            for (int j = 0; j < new_buf.Length; j++)
            {
                new_buf[j] = tmp[j];
            }

            data = Encoding.UTF8.GetString(new_buf);

            return data;
        }

        // This method extracts a float from a given initial index
        public float ExtractFloat32(byte[] buf, int init)
        {
            byte[] extracted = new byte[4];

            for (int i = init; i - init < 4; i++)
            {
                extracted[i - init] = buf[i];
            }

            if (!System.BitConverter.IsLittleEndian)
            {
                Array.Reverse(extracted);
            }

            return System.BitConverter.ToSingle(extracted, 0);
        }

        // This next one extracts an int32 from a given initial index
        public int ExtractInt32(byte[] buf, int init)
        {
            byte[] extracted = new byte[4];

            for (int i = init; i - init < 4; i++)
            {
                extracted[i - init] = buf[i];
            }

            if (!System.BitConverter.IsLittleEndian)
            {
                Array.Reverse(extracted);
            }

            return System.BitConverter.ToInt32(extracted, 0);
        }

        // DATA RELATED METHODS
        ///////////////////////

        // This one extracts a char from a byte which is the prefix
        public string ExtractPrefix(byte[] buf, int i)
        {
            return Encoding.UTF8.GetString(
                buf, i, 1);
        }

        // This method meticulously extracts prefixed data
        // and returns a data object
        public Data ExtractData(byte[] buf, int init)
        {
            Data data = new Data(GetNatureFromPrefix(
                ExtractPrefix(buf, init))
                );

            if (data.ID_Type == Data.Nature.Corrupt)
            {
                return null;
            }

            data.PropertyOf = buf[init + 1];

            data.Data_Type = IdentifyDataTypeFromPrefix(
                ExtractPrefix(buf, init + 2), data
                );

            if (data.ID_Type == Data.Nature.Entity)
            {
                // Entity type identification

                // Entity state identification
            }

            InjectData(buf, init + 3, data);

            return data;
        }

        // This method identifies the data's nature
        public Data.Nature GetNatureFromPrefix(string prefix)
        {
            switch (prefix)
            {
                case "A":
                    return Data.Nature.Player;
                case "B":
                    return Data.Nature.Entity;
                default:
                    return Data.Nature.Corrupt;
            }
        }

        // This method identifies the data's type
        // and sets the 'contained' filed appropriately
        public Data.WorldDataType IdentifyDataTypeFromPrefix(string prefix, Data d)
        {
            switch (prefix)
            {
                case "N":
                    d.Contained = Data.Contains.String;
                    return Data.WorldDataType.Name;
                case "L":
                    d.Contained = Data.Contains.Byte;
                    return Data.WorldDataType.Score;
                case "X":
                    d.Contained = Data.Contains.Float32;
                    return Data.WorldDataType.X;
                case "Y":
                    d.Contained = Data.Contains.Float32;
                    return Data.WorldDataType.Y;
                case "Z":
                    d.Contained = Data.Contains.Float32;
                    return Data.WorldDataType.Z;
                case "P":
                    d.Contained = Data.Contains.Float32;
                    return Data.WorldDataType.XRotation;
                case "Q":
                    d.Contained = Data.Contains.Float32;
                    return Data.WorldDataType.YRotation;
                case "R":
                    d.Contained = Data.Contains.Float32;
                    return Data.WorldDataType.XVelocity;
                case "S":
                    d.Contained = Data.Contains.Float32;
                    return Data.WorldDataType.YVelocity;
                case "T":
                    d.Contained = Data.Contains.Float32;
                    return Data.WorldDataType.ZVelocity;
                case "U":
                    d.Contained = Data.Contains.Float32;
                    return Data.WorldDataType.AngularVelocityX;
                case "V":
                    d.Contained = Data.Contains.Float32;
                    return Data.WorldDataType.AngularVelocityY;
                case "M":
                    d.Contained = Data.Contains.String;
                    return Data.WorldDataType.ModelID;
                case "W":
                    d.Contained = Data.Contains.Byte;
                    return Data.WorldDataType.CurrentWeapon;
                default:
                    return Data.WorldDataType.Corrupt;
            }
        }

        // This method is to inject data into the appropriate
        // data store in the object
        public void InjectData(byte[] buf, int init, Data d)
        {
            switch (d.Contained)
            {
                case Data.Contains.Byte:
                    d.Byte_Data = buf[init];
                    d.end_index = init;
                    break;
                case Data.Contains.Int32:
                    d.Int32_Data = ExtractInt32(buf, init);
                    d.end_index = init + 4;
                    break;
                case Data.Contains.Float32:
                    d.Float32_Data = ExtractFloat32(buf, init);
                    d.end_index = init + 4;
                    break;
                case Data.Contains.String:
                    d.Str_Data = ExtractString(buf, init);
                    d.end_index = init + d.Str_Data.Length;
                    break;
                default:
                    break;
            }
        }
    }
}
