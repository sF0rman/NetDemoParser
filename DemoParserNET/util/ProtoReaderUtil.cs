using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;

namespace DemoParserNET.util
{
    public static class ProtoReaderUtil
    {
        public static int ReadVarInt(BinaryReader buffer)
        {
            string binary = "";
            bool lastByte = false;
            while (!lastByte)
            {
                // Create binary representation of Byte
                string bin = Convert.ToString(buffer.ReadByte(), 2).PadLeft(8, '0');
                // Check the MSB. 0 means no more bytes are needed.
                lastByte = bin[0].Equals('0');
                // Add current byte to binary representation, removing the msb for each. 
                binary = binary.Insert(0, bin.Remove(0, 1));
            }

            // Reverse the bits
            char[] temp = binary.ToCharArray();
            Array.Reverse(temp);
            binary = new string(temp);

            // Create bit array
            BitArray bits = new BitArray(binary.Length);
            for (int i = 0; i < binary.Length; i++)
            {
                bits[i] = binary[i].Equals('1');
            }

            // Convert back to int
            int[] value = new int[1];
            bits.CopyTo(value, 0);

            return value[0];
        }

        public static string ReadNullTerminatedString(BinaryReader buffer)
        {
            StringBuilder str = new StringBuilder();
            char b; // set start value 
            do
            {
                b = buffer.ReadChar();
                str.Append(b);
            } while (b != 0x00);

            return str.ToString();
        }

        public static string ReadBytesAsBits(BinaryReader buffer, int amount = 1)
        {
            int bytes = amount / 8 + 1;
            string binary = "";
            for (int i = 0; i < bytes; i++)
            {
                string bin = Convert.ToString(buffer.ReadByte(), 2);
                binary += bin;
            }

            return binary;
        }
    }
}