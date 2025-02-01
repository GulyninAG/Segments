using Segments.Model;
using Serilog;

namespace Segments.Core
{
  // Класс по сохранению координат отрезков в бинарный файл
  internal static class SegmentSaver
  {
    private static readonly ILogger Logger = CreateLogger();

    public static void SaveSegmentsToBinaryFile(IReadOnlyList<Segment> segments, string filePath)
    {
      if (segments == null || segments.Count == 0)
        throw new ArgumentNullException(nameof(segments), "Список с отрезками не может быть пустым.");
      if (string.IsNullOrWhiteSpace(filePath))
        throw new ArgumentException("Путь к файлу не может быть пустым.", nameof(filePath));

      try
      {
        using (var writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
        {
          writer.Write(segments.Count);

          foreach (var segment in segments)
          {
            writer.Write(segment.Start.X);
            writer.Write(segment.Start.Y);
            writer.Write(segment.End.X);
            writer.Write(segment.End.Y);
          }
        }

        Logger.Information($"Отрезки были успешно записаны в файл {filePath}.");
      }
      catch (FileNotFoundException ex)
      {
        Logger.Error(ex, $"Файл не найден.");
        throw;
      }
      catch (Exception ex)
      {
        Logger.Error(ex, $"Не удалось записать отрезки в файл {filePath}.");
        throw;
      }
    }

    private static ILogger CreateLogger()
    {
      Log.Logger = new LoggerConfiguration()
          .WriteTo.File("log.txt")
          .CreateLogger();

      return Log.Logger;
    }
  }
}
