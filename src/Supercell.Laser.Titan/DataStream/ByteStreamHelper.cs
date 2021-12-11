namespace Supercell.Laser.Titan.DataStream
{
    using Supercell.Laser.Titan.Files.CsvData;
    using Supercell.Laser.Titan.Logic.Math;
    using System;
    using System.Linq;

    public static class ByteStreamHelper
    {
        /// <summary>
        /// Converts the specified buffer to a short.
        /// </summary>
        internal static short ToInt16(this byte[] buffer)
        {
            return BitConverter.ToInt16(buffer.Reverse().ToArray(), 0);
        }

        /// <summary>
        /// Converts the specified buffer to an int.
        /// </summary>
        internal static int ToInt24(this byte[] buffer)
        {
            byte[] int24 = new byte[4];

            int24[0] = 0;
            int24[1] = buffer[0];
            int24[2] = buffer[1];
            int24[3] = buffer[2];

            return int24.ToInt32();
        }

        /// <summary>
        /// Converts the specified buffer to an int.
        /// </summary>
        internal static int ToInt32Endian(this byte[] buffer)
        {
            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Converts the specified buffer to an int.
        /// </summary>
        internal static int ToInt32(this byte[] buffer)
        {
            return BitConverter.ToInt32(buffer.Reverse().ToArray(), 0);
        }

        /// <summary>
        /// Converts the specified buffer to a long.
        /// </summary>
        internal static long ToInt64(this byte[] buffer)
        {
            return BitConverter.ToInt64(buffer.Reverse().ToArray(), 0);
        }

        /// <summary>
        /// Converts a hexadecimal string to a byte array.
        /// </summary>
        public static byte[] HexaToBytes(this string value)
        {
            string hexa = value.Replace("-", string.Empty).Replace(" ", string.Empty).Replace("\t", string.Empty);
            return Enumerable.Range(0, hexa.Length / 2).Select(x => Convert.ToByte(hexa.Substring(x * 2, 2), 16)).ToArray();
        }

        public static void EncodeLogicLong(this ByteStream stream, LogicLong value)
        {
            stream.WriteVInt(value.High);
            stream.WriteVInt(value.Low);
        }

        public static void WriteDataReference(this ByteStream stream, LogicData data)
        {
            stream.WriteVInt(data.ClassId);
            if (data.ClassId != 0)
            {
                stream.WriteVInt(data.InstanceId);
            }
        }

        public static LogicData ReadDataReference(this ByteStream stream)
        {
            int classId = stream.ReadVInt();

            if (classId != 0)
            {
                return new LogicData(classId, stream.ReadVInt());
            }
            return null;
        }
    }
}
