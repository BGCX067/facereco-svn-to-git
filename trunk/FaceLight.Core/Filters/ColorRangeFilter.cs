#region Header
//
//   Project:           FaceLight - Simple Silverlight Real Time Face Detection.
//
//   Changed by:        $Author$
//   Changed on:        $Date$
//   Changed in:        $Revision$
//   Project:           $URL$
//   Id:                $Id$
//
//
//   Copyright (c) 2010 Rene Schulte
//
//   This program is open source software. Please read the License.txt.
//
#endregion

using System;
using System.Windows.Media.Imaging;

namespace FaceLight.Core
{
   /// <summary>
   /// A filter that thresholds the color in the image between a lower and upper threshold.
   /// </summary>
   public class ColorRangeFilter : IFilter
   {
      public YCbCrColor LowerThreshold { get; set; }
      public YCbCrColor UpperThreshold { get; set; }
      public int ResultColor { get; set; }

      public ColorRangeFilter()
      {
         LowerThreshold = YCbCrColor.Min;
         UpperThreshold = YCbCrColor.Max;
         ResultColor = 0xFFFFFF;
      }

      public WriteableBitmap Process(WriteableBitmap input)
      {
         var p = input.Pixels;
         var result = new WriteableBitmap(input.PixelWidth, input.PixelHeight);
         var rp = result.Pixels;
         var r = this.ResultColor;
         YCbCrColor ycbcr;

         // Threshold every pixel
         for (int i = 0; i < p.Length; i++)
         {
            ycbcr = YCbCrColor.FromArgbColori(p[i]);
            if (ycbcr.Y >= LowerThreshold.Y && ycbcr.Y <= UpperThreshold.Y
             && ycbcr.Cb >= LowerThreshold.Cb && ycbcr.Cb <= UpperThreshold.Cb
             && ycbcr.Cr >= LowerThreshold.Cr && ycbcr.Cr <= UpperThreshold.Cr)
            {
               rp[i] = r;
            }
         }

         return result;
      }
   }
}
