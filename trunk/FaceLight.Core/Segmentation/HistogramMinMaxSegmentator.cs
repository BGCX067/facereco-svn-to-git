﻿#region Header
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
using System.Linq;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace FaceLight.Core
{
   /// <summary>
   /// Segmentator that uses a histogram to find connected pixels and determines one region.
   /// </summary>
   public class HistogramMinMaxSegmentator : ISegmentator
   {
      private int jitteringCount;
      private Segment lastSegment;
      private int jitteringAmountThreshold;
      private int jitteringAmountThresholdSq;

      public Histogram Histogram { get; set; }
      public Vector ThresholdLuminance { get; set; }
      public int JitteringCountThreshold { get; set; }
      public int JitteringAmountThreshold 
      {
         get { return jitteringAmountThreshold; }
         set
         {
            jitteringAmountThreshold = value;
            jitteringAmountThresholdSq = value * value;
         }
      }

      public HistogramMinMaxSegmentator()
      {
         this.JitteringCountThreshold = 5;
         this.JitteringAmountThreshold = 40;
         Reset();
      }

      public IEnumerable<Segment> Process(WriteableBitmap input)
      {
         if (Histogram == null)
         {
            throw new InvalidOperationException("The histogram must be provided");
         }

         var hx = Histogram.X;
         var hy = Histogram.Y;
         var histUpperThreshold = Histogram.Max * 0.5f;

         // Find seeds for the segmentation:
         // All the x and y histogram indices where the value is above the half maximum and have a min distance
         const int step = 10;
         // x
         var ix = GetIndicesAboveThreshold(Histogram.MaxIndex.X, -step, hx, histUpperThreshold.X);
         ix.AddRange(GetIndicesAboveThreshold(Histogram.MaxIndex.X + step, step, hx, histUpperThreshold.X));
         // y
         var iy = GetIndicesAboveThreshold(Histogram.MaxIndex.Y, -step, hy, histUpperThreshold.Y);
         iy.AddRange(GetIndicesAboveThreshold(Histogram.MaxIndex.Y + step, step, hy, histUpperThreshold.Y));


         // Find the boundaries for the segments defined by the seeds
         var segments = new List<Segment>();
         foreach (var y0 in iy)
         {
            foreach (var x0 in ix)
            {
               var segment = new Segment(0, 0, 0, 0);
               segment.Min.X = GetIndexBelowThreshold(x0, -1, hx, ThresholdLuminance.X);
               segment.Max.X = GetIndexBelowThreshold(x0, 1, hx, ThresholdLuminance.X);
               segment.Min.Y = GetIndexBelowThreshold(y0, -1, hy, ThresholdLuminance.Y);
               segment.Max.Y = GetIndexBelowThreshold(y0, 1, hy, ThresholdLuminance.Y);
               segments.Add(segment);
            }
         }

         if (segments.Count > 0)
         {
            // Sort (only pick the largest for now)
            var foundSegment = segments.OrderByDescending(s => s.DiagonalSq).FirstOrDefault();


            // Prevent jittering: 
            // If the position doesn't change too much over a certain timespan, the last segment is always returned.
            
            // Check for jittering position.
            bool isJittering = true;
            if ((lastSegment.Min - foundSegment.Min).LengthSq < jitteringAmountThresholdSq)
            {
               // Check for jittering size
               if (Math.Abs(foundSegment.DiagonalSq - lastSegment.DiagonalSq) < jitteringAmountThresholdSq)
               {
                  jitteringCount++;
                  isJittering = false;
               }
            }

            // Freeze or not
            if (isJittering)
            {
               jitteringCount = 0;
            }
            if (jitteringCount < JitteringCountThreshold)
            {
               lastSegment = foundSegment;
            }
         }

         return new List<Segment> { lastSegment };
      }

      private int GetIndexBelowThreshold(int start, int step, int[] hist, int threshold)
      {
         int result = start, hi;
         for (int i = start; i < hist.Length && i > 0; i += step)
         {
            hi = hist[i];
            result = i;
            if (hi < threshold)
            {
               break;
            }
         }
         return result;
      }

      private List<int> GetIndicesAboveThreshold(int start, int step, int[] hist, int threshold)
      {
         var result = new List<int>();
         int hi;
         for (int i = start; i < hist.Length && i > 0; i += step)
         {
            hi = hist[i];
            if (hi > threshold)
            {
               result.Add(i);
            }
         }
         return result;
      }

      public void Reset()
      {
         jitteringCount = 0;
         lastSegment = new Segment(-100, -100, -200, -200);
      }
   }
}
