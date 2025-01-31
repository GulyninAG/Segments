using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segments.Model
{
  // Класс для представления прямоугольной области
  internal class RectangleArea
  {
    public PointF BottomLeft { get; }
    public PointF TopRight { get; }

    public RectangleArea(PointF bottomLeft, PointF topRight)
    {
      BottomLeft = bottomLeft;
      TopRight = topRight;
    }
  }
}
