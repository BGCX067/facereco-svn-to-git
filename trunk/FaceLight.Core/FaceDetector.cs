using System;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace FaceLight.Core
{
    public class FaceDetector
    {
        IFilter _erodeFilter;
        IFilter _dilateFilter;
        HistogramMinMaxSegmentator _segmentator;
        HistogramVisualizer _histogramViz;
        CbCrColorSpaceVisualizer _colorSpaceViz;
        ColorRangeFilter _skinColorFilter;

        public FaceDetector()
        {
            try
            {
                // Init filters and segmentation
                _skinColorFilter = new ColorRangeFilter
                {
                    LowerThreshold = new YCbCrColor(0.10f, -0.15f, 0.05f),
                    UpperThreshold = new YCbCrColor(1.00f, 0.05f, 0.20f),
                };
                _erodeFilter = new Erode5x5Filter();
                _dilateFilter = new Dilate5x5Filter();
                _segmentator = new HistogramMinMaxSegmentator();

                // Initialize Visualizers
                _histogramViz = new HistogramVisualizer { Scale = 20 };
                _colorSpaceViz = new CbCrColorSpaceVisualizer();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public WriteableBitmap Process(WriteableBitmap bmp)
        {
            if (bmp == null)
            {
                return null;
            }

            // Filter
            var skin = _skinColorFilter.Process(bmp);
            var erode = _erodeFilter.Process(skin);
            var dilate = _dilateFilter.Process(erode);
            dilate = _dilateFilter.Process(dilate);
            dilate = _dilateFilter.Process(dilate);

            // Segment
            var histogram = Histogram.FromWriteabelBitmap(dilate);
            _segmentator.Histogram = histogram;
            _segmentator.ThresholdLuminance = histogram.Max * 0.1f;
            var foundSegments = _segmentator.Process(dilate);

            // Visualize
            _histogramViz.Histogram = histogram;
            _histogramViz.Visualize(dilate);

            var result = new WriteableBitmap(bmp.PixelWidth, bmp.PixelHeight);
            foreach (var s in foundSegments)
            {
                // Uses the segment's center and half width, height
                var c = s.Center;
                result.DrawEllipseCentered(c.X, c.Y, s.Width >> 1, s.Height >> 1, Colors.Red);
            }
            return result;
        }

        public WriteableBitmap Corp(WriteableBitmap bmp)
        {
            if (bmp == null)
            {
                return null;
            }

            // Filter
            var skin = _skinColorFilter.Process(bmp);
            var erode = _erodeFilter.Process(skin);
            var dilate = _dilateFilter.Process(erode);
            dilate = _dilateFilter.Process(dilate);
            dilate = _dilateFilter.Process(dilate);

            // Segment
            var histogram = Histogram.FromWriteabelBitmap(dilate);
            _segmentator.Histogram = histogram;
            _segmentator.ThresholdLuminance = histogram.Max * 0.1f;
            var foundSegments = _segmentator.Process(dilate);

            // Visualize
            _histogramViz.Histogram = histogram;
            _histogramViz.Visualize(dilate);

            WriteableBitmap result = null;
            foreach (var s in foundSegments)
            {
                // Uses the segment's center and half width, height
                //var c = s.Center;
                //result.DrawEllipseCentered(c.X, c.Y, s.Width >> 1, s.Height >> 1, Colors.Red);


                var x = (s.Center.X - s.Width >> 1) > 0 ? (s.Center.X - s.Width >> 1) : 0;
                var y = (s.Center.Y - s.Height >> 1) > 0 ? (s.Center.Y - s.Height >> 1) : 0;
                result = bmp.Crop(x, y, s.Width, s.Height);
                break;
            }

            return result;
        }
    }
}
