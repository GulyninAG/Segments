using Segments.Core;
using Segments.Model;

namespace Segments
{
  public partial class SegmentsForm : Form
  {
    private readonly IReadOnlyList<Segment> segments;
    private readonly RectangleArea rectangle;
    private readonly IReadOnlyList<Segment> intersectingSegments;
    private const int numberOfPoints = 100; // количество точек, количество отрезков = numberOfPoints - 1

    // Координаты прямоугольника
    private readonly int pointA_x = Properties.Settings.Default.pointA_x;
    private const int pointA_y = 100;
    private const int pointB_x = 750;
    private const int pointB_y = 750;

    // Рабочая область экрана
    private readonly int workingAreaWidth = (Screen.PrimaryScreen is not null) ? Screen.PrimaryScreen.WorkingArea.Width : 1920;
    private readonly int workingAreaHeight = (Screen.PrimaryScreen is not null) ? Screen.PrimaryScreen.WorkingArea.Height : 1080;

    // Цвет линий
    private readonly Color color_Segment = Color.Green;
    private readonly Color color_RectangleArea = Color.Blue;
    private readonly Color color_intersectingSegment = Color.Red;

    // Название файла
    private readonly string fileName = "segments.bin";
    public SegmentsForm()
    {
      InitializeComponent();

      this.Text = "Отрезки и область";
      this.WindowState = FormWindowState.Maximized;
      this.DoubleBuffered = true;

      SegmentGenerator segmentGenerator = new SegmentGenerator(numberOfPoints, workingAreaWidth, workingAreaHeight);
      segments = segmentGenerator.GetSegments();

      rectangle = new RectangleArea(new PointF(pointA_x, pointA_y), new PointF(pointB_x, pointB_y));

      intersectingSegments = Intersection.GetSegmentsInOrIntersectingRectangle(segments, rectangle);

      if (intersectingSegments.Count > 0)
      {
        SegmentSaver.SaveSegmentsToBinaryFile(intersectingSegments, fileName);
      }

      this.Paint += SegmentsForm_Paint;
    }

    protected void SegmentsForm_Paint(object sender, PaintEventArgs e)
    {
      DrawingProvider.DrawSegments(e.Graphics, segments, color_Segment);
      DrawingProvider.DrawRectangleArea(e.Graphics, rectangle, color_RectangleArea);
      DrawingProvider.DrawSegments(e.Graphics, intersectingSegments, color_intersectingSegment);
    }
  }
}
