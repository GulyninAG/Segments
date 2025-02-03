using Segments.Model;
using Serilog;
using System.Threading;

namespace Segments.Core
{
  // Класс для отрисовки отрезков и прямоугольной области.
  internal static class DrawingProvider
  {
    private static readonly ILogger Logger = SerilogFileLogger.CreateLogger();
    internal static void DrawSegments(Graphics g, IReadOnlyList<Segment> segments, Color color, CancellationToken cancellationToken)
    {
      if (g == null)
        throw new ArgumentNullException(nameof(g), "Объект графики не может быть пустым.");
      if (segments == null)
        throw new ArgumentNullException(nameof(segments), "Список отрезков не был передан.");

      try
      {
        using (Pen pen = new Pen(color))
        {
          foreach (var segment in segments)
          {
            cancellationToken.ThrowIfCancellationRequested();
            g.DrawLine(pen, segment.Start, segment.End);
          }
        }

        Logger.Information("Отрезки были успешно отрисованы.");
      }
      catch (Exception ex)
      {
        Logger.Error(ex, "Произошла ошибка при отрисовке отрезков.");
        throw;
      }
    }

    internal static void DrawRectangleArea(Graphics g, RectangleArea rect, Color color)
    {
      if (g == null)
        throw new ArgumentNullException(nameof(g), "Объект графики не может быть пустым.");
      if (rect == null)
        throw new ArgumentNullException(nameof(rect), "Прямоугольник не был передан.");

      try
      {
        using (Pen pen = new Pen(color))
        {
          g.DrawRectangle(pen, rect.BottomLeft.X, rect.BottomLeft.Y,
              rect.TopRight.X - rect.BottomLeft.X, rect.TopRight.Y - rect.BottomLeft.Y);
        }

        Logger.Information("Прямоугольник был отрисован успешно.");
      }
      catch (Exception ex)
      {
        Logger.Error(ex, "Произошла ошибка при отрисовке прямоугольника.");
        throw;
      }
    }
  }
}
