namespace Segments
{
  internal static class Program
  {
    [STAThread]
    static void Main()
    {
      ApplicationConfiguration.Initialize();
      Application.EnableVisualStyles();
      Application.Run(new SegmentsForm());
    }
  }
}