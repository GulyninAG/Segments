using Segments.Core;
using Segments.Model;

namespace Segments
{
  public partial class SegmentsForm : Form
  {
    private List<Segment> segments;
    private RectangleArea rectangle;
    private List<Segment> intersectingSegments;
    private const int numberOfPoints = 50; // количество точек, количество отрезков = numberOfPoints - 1

    // Координаты прямоугольника
    private const int pointA_x = 100;
    private const int pointA_y = 100;
    private const int pointB_x = 750;
    private const int pointB_y = 750;

    // Рабочая область экрана
    private readonly int workingAreaWidth = (Screen.PrimaryScreen is not null) ? Screen.PrimaryScreen.WorkingArea.Width : 1920;
    private readonly int workingAreaHeight = (Screen.PrimaryScreen is not null) ? Screen.PrimaryScreen.WorkingArea.Height : 1080;

    // Цвет линий
    private Color color_Segment = Color.Green;
    private Color color_RectangleArea = Color.Blue;
    private Color color_intersectingSegment = Color.Red;

    public SegmentsForm()
    {
      InitializeComponent();

      this.Text = "Отрезки и область";
      this.WindowState = FormWindowState.Maximized;


      SegmentGenerator segmentGenerator = new SegmentGenerator(numberOfPoints, workingAreaWidth, workingAreaHeight);
      segments = segmentGenerator.GetSegments();

      rectangle = new RectangleArea(new PointF(pointA_x, pointA_y), new PointF(pointB_x, pointB_y));

      Intersection intersection = new Intersection();
      intersectingSegments = intersection.GetSegmentsInOrIntersectingRectangle(segments, rectangle);

      this.Paint += new PaintEventHandler(SegmentsForm_Paint);
    }

    protected void SegmentsForm_Paint(object sender, PaintEventArgs e)
    {
      DrawingProvider drawingProvider = DrawingProvider.GetInstance();
      drawingProvider.DrawSegments(e.Graphics, segments, color_Segment);
      drawingProvider.DrawRectangleArea(e.Graphics, rectangle, color_RectangleArea);
      drawingProvider.DrawSegments(e.Graphics, intersectingSegments, color_intersectingSegment);
    }
  }
}
