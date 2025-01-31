using Segments.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segments.Core
{
  // Класс по генерации отрезков на основе точек
  internal class SegmentGenerator
  {
    public int MaxWidth { get; set; }
    public int MaxHeight { get; set; }
    public int NumberOfPoints { get; set; }

    internal List<Segment> segments = new List<Segment>();

    public SegmentGenerator(int numberOfPoints, int maxWidth, int maxHeight)
    {
      NumberOfPoints = numberOfPoints;
      MaxWidth = maxWidth;
      MaxHeight = maxHeight; 

    }

    internal List<Segment> GetSegments()
    {
      PointF[] points = new PointF[NumberOfPoints];
      List<Segment> segments = new List<Segment>();
      Random random = new Random();

      for (int i = 0; i < NumberOfPoints; i++)
      {
        points[i] = new PointF(random.Next(0, MaxWidth), random.Next(0, MaxHeight));
      }

      for (int i = 0; i < NumberOfPoints - 1; i++)
      {
        segments.Add(new Segment(points[i], points[i + 1]));
      }

      return segments;
    }
  }
}
