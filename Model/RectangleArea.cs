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
