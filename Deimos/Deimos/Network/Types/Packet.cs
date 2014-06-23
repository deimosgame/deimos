using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Deimos
{
    public class Packet
    {
        public enum PacketType
        {
            Handshake,
            Connection,
            Disconnection,
            Chat,
            Broadcast,
            Move,
            PlayerList,
            UpdateInfo,
            Sound,
            Minigame,
            EntityList,
            BulletCreate,
            Corrupt
        }


        // ATTRIBUTES
        public PacketType Type;
        public bool Split = false; // this is the default value for this attribute

        public byte Checksum = 0x00;
        public byte Packet_ID;
        public byte Index = 0; // this is the default value for this attribute
        public byte Total_Packets = 1; // this is the default value for this attribute

        // Optional for world broadcast packets //
        public int Unique_ID;
        // Optional for world broadcast packets //

        // May be needed
        public byte Sum = 0x00;
        // May be needed

        MemoryStream Data = new MemoryStream();
        private int current_index = 4; // default value

        public byte[] Encoded_buffer = new byte[576];

        public Packet Next = null;
        public Packet Previous = null;

        // CONSTRUCTOR
        public Packet(PacketType type)
        {
            // Setting the type
            Type = type;

            // Allocating positions for first 4 bytes
            Data.WriteByte(0x00);
            Data.WriteByte(0x00);
            Data.WriteByte(0x00);
            Data.WriteByte(0x00);
        }

        // COMMON DATAGRAMS

        // This functions recursively returns the checksum as an int32
        // for split packets
        // WARNING: Needs to be initiated at first index dgram
        public int GetTotalSum()
        {
            return (int)Sum + (int)Next.GetTotalSum();
        }

        // Manually write a byte to the packet
        // WARNING! Risk of going out of bounds and not delimitated!
        // Use at your own risk
        public void Write(byte b)
        {
            Data.WriteByte(b);
            current_index++;
            Sum++;
        }

        // Manually write data to the packet
        // WARNING! Risk of going out of bounds and not delimitated!
        // Use at your own risk
        public void Write(byte[] buf)
        {
            for (int i = 0; i < buf.Length; i++)
            {
                Data.WriteByte(buf[i]);
                current_index++;
                Sum++;
            }
        }

        // These are normal and protocoled data adding method overloads:

        // This AddData overload adds a byte properly to the datagram
        public void AddData(byte b)
        {
            Data.WriteByte(b);
            current_index++;
            Sum++;
        }

        public void AddData(char c)
        {
            Data.WriteByte((byte)c);
            current_index++;
            Sum++;
        }

        // This AddData overload is the most generic and easy to use:
        // it adds a byte array to the datagram properly
        // and returns a boolean for succesfull fit in the package
        // or a split trigger
        public void AddData(byte[] buf)
        {
            if (buf.Length > 575 - current_index)
            {
                // The data triggers a splitting of the packet

                // We make the proper adjustments
                SetNext();

                if (Type == PacketType.Broadcast)
                {
                    Next.AddData(buf);
                }
                else
                {
                    int i = 0;
                    while (current_index <= 575
                        && i < buf.Length)
                    {
                        Data.WriteByte(buf[i]);
                        current_index++;
                        Sum++;
                        i++;
                    }

                    while (Next.current_index <= 575
                        && i < buf.Length)
                    {
                        Next.Data.WriteByte(buf[i]);
                        Next.current_index++;
                        Next.Sum++;
                        i++;
                    }
                }
            }
            else
            {
                // The specified data will fit in the packet

                for (int i = 0; i < buf.Length; i++)
                {
                    // Inscribing the bytes from the buffer to the stream
                    Data.WriteByte(buf[i]);
                    current_index++;
                    Sum++;
                }
            }
        }

        // This AddData overload adds a string to the MemoryStream
        // in an optimized way
        public void AddData(string s)
        {
            AddData(Encoding.UTF8.GetBytes(s));
            Write(0x00);
        }

        // This AddData overload adds a float to the Memory Stream
        // in an optimized way
        public void AddData(float f)
        {
            byte[] converted = System.BitConverter.GetBytes(f);

            if (!System.BitConverter.IsLittleEndian)
            {
                Array.Reverse(converted);
            }

            AddData(converted);
        }

        // This AddData overload adds an int32 to the Memory Stream
        // in an optimized way
        public void AddData(int x)
        {
            byte[] converted = System.BitConverter.GetBytes(x);

            if (!System.BitConverter.IsLittleEndian)
            {
                Array.Reverse(converted);
            }

            AddData(converted);
        }


        // These are ensemble handling methods:

        // This method handles a packet split
        // and sets packet properties for this and the next datagram
        public Packet SetNext()
        {
            // Adjusting properties
            Split = true;
            Total_Packets++;

            // Adjusting next packet
            Next = new Packet(Type);
            Next.Previous = this;

            Next.Total_Packets = Total_Packets;
            Next.Split = true;
            Next.Packet_ID = Packet_ID;
            Next.Index = Index++;

            // Returning the next packet
            return Next;
        }

        // To encode data to datagram buffer

        // This method generates the encoded buffer
        // with respect to the Louis-Paul Protocol
        public byte[] Encode()
        {
            ComputeChecksum();

            Data.Position = 0;
            Data.WriteByte(Checksum);
            Data.Position = 1;
            Data.WriteByte(Packet_ID);
            Data.Position = 2;
            Data.WriteByte(Index);
            Data.Position = 3;
            Data.WriteByte(Total_Packets);
            Data.Position = Data.Length - 1;

            Encoded_buffer = Data.ToArray();

            return Encoded_buffer;
        }

        public byte GetByteChecksum(byte b)
        {
            byte computed = 0;

            for (int j = 0; b > 0x00; j++)
            {
                computed += (byte)((j % 2 + 1) * (b % 2));
                b = (byte)(b >> 1);
            }

            return computed;
        }

        public void ComputeChecksum()
        {
            byte totalsum = GetByteChecksum(Packet_ID);
            totalsum += GetByteChecksum(Index);
            totalsum += GetByteChecksum(Total_Packets);

            Data.Position = 4;

            int next_read = Data.ReadByte();

            while (next_read != -1)
            {
                totalsum += GetByteChecksum((byte)(next_read));
                next_read = Data.ReadByte();
            }

            Checksum = totalsum;
        }
    }
}
