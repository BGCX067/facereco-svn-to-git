using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ImageHash;
namespace ImageHash
{
    public class Utility
    {
        public static bool IsSimilar(string sourceImagePath, string tragetImagePath, ulong targetSimilarity)
        {
            Bitmap theImage;
            Bitmap theOtherImage;

            theImage = new Bitmap(sourceImagePath, true);
            theOtherImage = new Bitmap(tragetImagePath, true);

            ulong hash1 = Imghash.AverageHash(theImage);
            ulong hash2 = Imghash.AverageHash(theOtherImage);

            ulong similarity = ((64UL - Imghash.BitCount(hash1 ^ hash2)) * 100UL) / 64UL;

            return similarity >= targetSimilarity;
        }
    }
}
