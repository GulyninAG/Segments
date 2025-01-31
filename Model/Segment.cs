using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
