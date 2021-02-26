using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archiver
{
    static class Tools
    {
        public static byte[] GetSubArray(this byte[] source, int count)
        {
            if (count == 0 || count > source.Length) throw new IndexOutOfRangeException("Count has not to be zero of bigger lan array length.");
            var outArray = new byte[count];

            for (int i = 0; i < count; i++)
            {
                outArray[i] = source[i];
            }
            return outArray;
        }
    }
}
