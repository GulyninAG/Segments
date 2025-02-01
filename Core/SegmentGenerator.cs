using System;
using System.Collections.Generic;
using System.Drawing;
using Segments.Model;

namespace Segments.Core
{
  // Класс по генерации отрезков на основе точек
  internal class SegmentGenerator
  {
    public int MaxWidth { get; }
    public int MaxHeight { get; }
    public int NumberOfPoints { get; }

    private readonly Random _random;
    public SegmentGenerator(int numberOfPoints, int maxWidth, int maxHeight)
    {
      if (numberOfPoints < 2)
        throw new ArgumentException("Количество точек должно быть больше 2.", nameof(numberOfPoints));
      if (maxWidth <= 0)
        throw new ArgumentException("Максимальная ширина должна быть больше 0.", nameof(maxWidth));
      if (maxHeight <= 0)
        throw new ArgumentException("Максимальная ширина должна быть больше 0.", nameof(maxHeight));

      NumberOfPoints = numberOfPoints;
      MaxWidth = maxWidth;
      MaxHeight = maxHeight;
      _random = new Random();
    }

    public IReadOnlyList<Segment> GetSegments()
    {
      var points = GenerateRandomPoints();
      return CreateSegments(points);
    }

    private PointF[] GenerateRandomPoints()
    {
      var points = new PointF[NumberOfPoints];
      for (int i = 0; i < NumberOfPoints; i++)
      {
        points[i] = new PointF(
            _random.Next(0, MaxWidth),
            _random.Next(0, MaxHeight)
        );
      }
      return points;
    }

    private IReadOnlyList<Segment> CreateSegments(PointF[] points)
    {
      var segments = new List<Segment>();
      for (int i = 0; i < points.Length - 1; i++)
      {
        segments.Add(new Segment(points[i], points[i + 1]));
      }
      return segments;
    }
  }
}
