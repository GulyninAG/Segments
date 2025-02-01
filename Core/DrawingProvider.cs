using Segments.Model;

namespace Segments.Core
{
  // Класс для отрисовки отрезков и прямоугольной области.
  internal static class DrawingProvider
  {
    internal static void DrawSegments(Graphics g, IReadOnlyList<Segment> segs, Color color)
    {
      using (Pen pen = new Pen(color))
      {
        foreach (var segment in segs)
        {
          g.DrawLine(pen, segment.Start, segment.End);
        }
      }
    }

    internal static void DrawRectangleArea(Graphics g, RectangleArea rect, Color color)
    {
      using (Pen pen = new Pen(color))
      {
        g.DrawRectangle(pen, rect.BottomLeft.X, rect.BottomLeft.Y,
            rect.TopRight.X - rect.BottomLeft.X, rect.TopRight.Y - rect.BottomLeft.Y);
      }
    }
  }
}
