using Segments.Model;

namespace Segments.Core
{
  // В данном классе находится логика по пересечению отрезками прямоугольной области и нахождению отрезков внутри неё.
  internal static class Intersection
  {
    // Получаем отрезки, которые находятся в прям. области или пересекают её
    internal static List<Segment> GetSegmentsInOrIntersectingRectangle(IReadOnlyList<Segment> segments, RectangleArea rectangle)
    {
      List<Segment> segmentsInOrIntersectingRectangle = new List<Segment>();

      foreach (var segment in segments)
      {
        if (IsInside(segment, rectangle) || Intersects(segment, rectangle))
        {
          segmentsInOrIntersectingRectangle.Add(segment);
        }
      }

      return segmentsInOrIntersectingRectangle;
    }

    // Проверяем находится ли отрезок в прямоугольной области
    internal static bool IsInside(Segment segment, RectangleArea rectangle)
    {
      return segment.Start.X >= rectangle.BottomLeft.X && segment.Start.X <= rectangle.TopRight.X &&
              segment.Start.Y >= rectangle.BottomLeft.Y && segment.Start.Y <= rectangle.TopRight.Y &&
             (segment.End.X >= rectangle.BottomLeft.X && segment.End.X <= rectangle.TopRight.X &&
              segment.End.Y >= rectangle.BottomLeft.Y && segment.End.Y <= rectangle.TopRight.Y);
    }

    // Проверяем пересекает ли отрезок прямоугольную область
    internal static bool Intersects(Segment segment, RectangleArea rectangle)
    {
      PointF[] rectanglePoints = new PointF[]
      {
            rectangle.BottomLeft,
            new PointF(rectangle.BottomLeft.X, rectangle.TopRight.Y),
            rectangle.TopRight,
            new PointF(rectangle.TopRight.X, rectangle.BottomLeft.Y)
      };

      for (int i = 0; i < rectanglePoints.Length; i++)
      {
        PointF p1 = rectanglePoints[i];
        PointF p2 = rectanglePoints[(i + 1) % rectanglePoints.Length];

        Segment rectangleSide = new Segment(p1, p2);
        if (SegmentsIntersect(segment, rectangleSide))
        {
          return true;
        }
      }

      return false;
    }

    // Проверяем пересекаются ли отрезки
    private static bool SegmentsIntersect(Segment s1, Segment s2)
    {
      PointF p1 = s1.Start;
      PointF p2 = s1.End;
      PointF p3 = s2.Start;
      PointF p4 = s2.End;

      // Проверяем ориентацию трех точек
      int o1 = Orientation(p1, p2, p3);
      int o2 = Orientation(p1, p2, p4);
      int o3 = Orientation(p3, p4, p1);
      int o4 = Orientation(p3, p4, p2);

      // Общий случай
      if (o1 != o2 && o3 != o4)
      {
        return true;
      }

      // Специальные случаи
      // p1, p2 и p3 на одной прямой
      if (o1 == 0 && IsOnSegment(p1, p2, p3)) return true;
      // p1, p2 и p4 на одной прямой
      if (o2 == 0 && IsOnSegment(p1, p2, p4)) return true;
      // p3, p4 и p1 на одной прямой
      if (o3 == 0 && IsOnSegment(p3, p4, p1)) return true;
      // p3, p4 и p2 на одной прямой
      if (o4 == 0 && IsOnSegment(p3, p4, p2)) return true;

      return false; // Не пересекаются
    }

    private static int Orientation(PointF p, PointF q, PointF r)
    {
      double val = ((q.Y - p.Y) * (r.X - q.X)) - ((q.X - p.X) * (r.Y - q.Y));
      if (val == 0) return 0; // Коллинеарны
      return (val > 0) ? 1 : 2; // 1 - по часовой, 2 - против часовой
    }

    private static bool IsOnSegment(PointF p, PointF q, PointF r)
    {
      return q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
             q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y);
    }
  }
}
