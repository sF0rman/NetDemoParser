using System.Collections;
using System.IO;
using System.Text;

namespace DemoParserNET.util
{
    class BitReader
    {
        int _bit;
        byte _currentByte;
        readonly Stream _stream;
        public BitReader(Stream stream)
        { _stream = stream; }

        public bool? ReadBit(bool bigEndian = false)
        {
            if (_bit == 8 ) 
            {

                var r = _stream.ReadByte();
                if (r== -1) return null;
                _bit = 0; 
                _currentByte  = (byte)r;
            }
            bool value;
            if (!bigEndian)
                value = (_currentByte & (1 << _bit)) > 0;
            else
                value = (_currentByte & (1 << (7-_bit))) > 0;

            _bit++;
            return value;
        }

        public int ReadBits(int num, bool bigEndian = false)
        {
            BitArray bits = new BitArray(num);
            for (int i = 0; i < num; i++)
            {
                bits[i] = ReadBit(bigEndian) ?? false;
            }

            int[] x = new int[1];
            bits.CopyTo(x, 0);
            return x[0];
        }
    }
}