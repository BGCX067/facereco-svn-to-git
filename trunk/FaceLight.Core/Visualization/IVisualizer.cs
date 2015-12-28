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
using System.Linq;
using System.Windows.Media.Imaging;

namespace FaceLight.Core
{
   /// <summary>
   /// An interface for a visualizer that draws something into a WriteableBitmap.
   /// </summary>
   interface IVisualizer
   {
      void Visualize(WriteableBitmap surface);
   }
}
