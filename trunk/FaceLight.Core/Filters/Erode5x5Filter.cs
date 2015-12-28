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
   /// Erosion filter with a fixed 5x5 kernel.
   /// </summary>
   public class Erode5x5Filter : IFilter
   {
      public int ResultColor { get; set; }
      public int CompareEmptyColor { get; set; }

      public Erode5x5Filter()
      {
         ResultColor = 0xFFFFFF;
         CompareEmptyColor = 0;
      }

      public WriteableBitmap Process(WriteableBitmap input)
      {
         var p = input.Pixels;
         var w = input.PixelWidth;
         var h = input.PixelHeight;
         var result = new WriteableBitmap(w, h);
         var rp = result.Pixels;
         var empty = CompareEmptyColor;
         int c, cm;
         int i = 0;

         // Erode every pixel
         for (int y = 0; y < h; y++)
         {
            for (int x = 0; x < w; x++, i++)
            {
               // Middle pixel
               cm = p[y * w + x];
               if (cm == empty) { continue; }

               // Row 0
               // Left pixel
               if (x - 2 > 0 && y - 2 > 0)
               {
                  c = p[(y - 2) * w + (x - 2)];
                  if (c == empty) { continue; }
               }
               // Middle left pixel
               if (x - 1 > 0 && y - 2 > 0)
               {
                  c = p[(y - 2) * w + (x - 1)];
                  if (c == empty) { continue; }
               }
               if (y - 2 > 0)
               {
                  c = p[(y - 2) * w + x];
                  if (c == empty) { continue; }
               }
               if (x + 1 < w && y - 2 > 0)
               {
                  c = p[(y - 2) * w + (x + 1)];
                  if (c == empty) { continue; }
               }
               if (x + 2 < w && y - 2 > 0)
               {
                  c = p[(y - 2) * w + (x + 2)];
                  if (c == empty) { continue; }
               }

               // Row 1
               // Left pixel
               if (x - 2 > 0 && y - 1 > 0)
               {
                  c = p[(y - 1) * w + (x - 2)];
                  if (c == empty) { continue; }
               }
               if (x - 1 > 0 && y - 1 > 0)
               {
                  c = p[(y - 1) * w + (x - 1)];
                  if (c == empty) { continue; }
               }
               if (y - 1 > 0)
               {
                  c = p[(y - 1) * w + x];
                  if (c == empty) { continue; }
               }
               if (x + 1 < w && y - 1 > 0)
               {
                  c = p[(y - 1) * w + (x + 1)];
                  if (c == empty) { continue; }
               }
               if (x + 2 < w && y - 1 > 0)
               {
                  c = p[(y - 1) * w + (x + 2)];
                  if (c == empty) { continue; }
               }

               // Row 2
               if (x - 2 > 0)
               {
                  c = p[y * w + (x - 2)];
                  if (c == empty) { continue; }
               }
               if (x - 1 > 0)
               {
                  c = p[y * w + (x - 1)];
                  if (c == empty) { continue; }
               }
               if (x + 1 < w)
               {
                  c = p[y * w + (x + 1)];
                  if (c == empty) { continue; }
               }
               if (x + 2 < w)
               {
                  c = p[y * w + (x + 2)];
                  if (c == empty) { continue; }
               }

               // Row 3
               if (x - 2 > 0 && y + 1 < h)
               {
                  c = p[(y + 1) * w + (x - 2)];
                  if (c == empty) { continue; }
               }
               if (x - 1 > 0 && y + 1 < h)
               {
                  c = p[(y + 1) * w + (x - 1)];
                  if (c == empty) { continue; }
               }
               if (y + 1 < h)
               {
                  c = p[(y + 1) * w + x];
                  if (c == empty) { continue; }
               }
               if (x + 1 < w && y + 1 < h)
               {
                  c = p[(y + 1) * w + (x + 1)];
                  if (c == empty) { continue; }
               }
               if (x + 2 < w && y + 1 < h)
               {
                  c = p[(y + 1) * w + (x + 2)];
                  if (c == empty) { continue; }
               }

               // Row 4
               if (x - 2 > 0 && y + 2 < h)
               {
                  c = p[(y + 2) * w + (x - 2)];
                  if (c == empty) { continue; }
               }
               if (x - 1 > 0 && y + 2 < h)
               {
                  c = p[(y + 2) * w + (x - 1)];
                  if (c == empty) { continue; }
               }
               if (y + 2 < h)
               {
                  c = p[(y + 2) * w + x];
                  if (c == empty) { continue; }
               }
               if (x + 1 < w && y + 2 < h)
               {
                  c = p[(y + 2) * w + (x + 1)];
                  if (c == empty) { continue; }
               }
               if (x + 2 < w && y + 2 < h)
               {
                  c = p[(y + 2) * w + (x + 2)];
                  if (c == empty) { continue; }
               }

               // If all neighboring pixels are processed 
               // it's clear that the current pixel is not a boundary pixel.
               rp[i] = cm;
            }
         }
         
         return result;
      }
   }
}
