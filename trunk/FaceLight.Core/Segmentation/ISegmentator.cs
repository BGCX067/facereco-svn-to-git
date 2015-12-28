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

using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace FaceLight.Core
{
   /// <summary>
   /// Interface of an image segmentator.
   /// </summary>
   interface ISegmentator
   {
      IEnumerable<Segment> Process(WriteableBitmap input);
   }
}