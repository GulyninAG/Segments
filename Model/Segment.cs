namespace Segments.Model
{
  // Класс для представления отрезка
  internal class Segment
  {
    public PointF Start { get; }
    public PointF End { get; }

    public Segment(PointF start, PointF end)
    {
      Start = start;
      End = end;
    }
  }
}
