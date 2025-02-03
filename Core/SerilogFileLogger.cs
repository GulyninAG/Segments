using Serilog;

namespace Segments.Core
{
  internal static class SerilogFileLogger
  {
    internal static ILogger CreateLogger()
    {
      Log.Logger = new LoggerConfiguration()
          .WriteTo.File("log.txt")
          .CreateLogger();

      return Log.Logger;
    }
  }
}
