using Segments.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segments.Core
{
  // Класс для отрисовки отрезков и прямоугольной области. Представляет собой реализацию паттерна "Singleton".
  internal class DrawingProvider
  {
    private static readonly Lazy<DrawingProvider> lazy = new Lazy<DrawingProvider>(() => new DrawingProvider());

    private DrawingProvider()
    {
              
    }

    public static DrawingProvider GetInstance()
    {
      return lazy.Value;
    }

    internal void DrawSegments(Graphics g, List<Segment> segs, Color color)
    {
      using (Pen pen = new Pen(color))
      {
        foreach (var segment in segs)
        {
          g.DrawLine(pen, segment.Start, segment.End);
        }
      }
    }

    internal void DrawRectangleArea(Graphics g, RectangleArea rect, Color color)
    {
      using (Pen pen = new Pen(color))
      {
        g.DrawRectangle(pen, rect.BottomLeft.X, rect.BottomLeft.Y,
            rect.TopRight.X - rect.BottomLeft.X, rect.TopRight.Y - rect.BottomLeft.Y);
      }
    }
  }
}
