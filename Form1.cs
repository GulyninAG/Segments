using Segments.Core;
using Segments.Model;
using Segments.Properties;
using System.Threading;

namespace Segments
{
  public partial class SegmentsForm : Form
  {
    private CancellationTokenSource? _cancellationTokenSource;

    private IReadOnlyList<Segment> segments;
    private RectangleArea rectangle;
    private IReadOnlyList<Segment> intersectingSegments;
    private readonly int numberOfPoints = Settings.Default.numberOfPoints; // ���������� �����, ���������� �������� = numberOfPoints - 1

    // ���������� ��������������
    private readonly int pointA_x = Settings.Default.pointA_x;
    private readonly int pointA_y = Settings.Default.pointA_y;
    private readonly int pointB_x = Settings.Default.pointB_x;
    private readonly int pointB_y = Settings.Default.pointB_y;

    // ������� ������� ������
    private int workingAreaWidth;
    private int workingAreaHeight;
    private readonly int marginLeft = 2;
    private readonly int marginTop = 36;

    // ���� �����
    private readonly Color color_Segment = Color.Green;
    private readonly Color color_RectangleArea = Color.Blue;
    private readonly Color color_intersectingSegment = Color.Red;

    Panel drawingPanel;

    // �������� �����
    private readonly string fileName = "segments.bin";
    public SegmentsForm()
    {
      InitializeComponent();

      this.Text = "������� � �������";
      this.WindowState = FormWindowState.Maximized;
      this.DoubleBuffered = true;

      // ������� ToolStrip � ��������� ������
      ToolStrip toolStrip = new ToolStrip();
      ToolStripButton btnStart = new ToolStripButton("������ ��������� ��������");
      ToolStripButton btnStop = new ToolStripButton("������ ��������� (��� ��������������� ��������)");

      btnStart.Click += btn_Start_Click;
      btnStop.Click += btn_Stop_Click;

      toolStrip.Items.Add(btnStart);
      toolStrip.Items.Add(btnStop);

      // ��������� ToolStrip �� �����
      this.Controls.Add(toolStrip);
      toolStrip.Dock = DockStyle.Top;

      // ������� Panel ��� ���������
      drawingPanel = new Panel();
      drawingPanel.Dock = DockStyle.Fill;
      drawingPanel.BackColor = Color.White;

      // ��������� Panel �� �����
      this.Controls.Add(drawingPanel);
    }

    private async void btn_Start_Click(object sender, EventArgs e)
    {
      _cancellationTokenSource = new CancellationTokenSource();

      drawingPanel.BackgroundImage = null;

      workingAreaWidth = drawingPanel.Width;
      workingAreaHeight = drawingPanel.Height;

      // ������������� ������
      InitializeData();

      try
      {
        // ��������� ���������� � ������� ������
        intersectingSegments = await Task.Run(() =>
            Intersection.GetSegmentsInOrIntersectingRectangle(segments, rectangle, _cancellationTokenSource.Token).ToList());

        // ����������� ���������
        await DrawSegmentsAsync();

        if (intersectingSegments.ToList().Count > 0)
        {
          SegmentSaver.SaveSegmentsToBinaryFile(intersectingSegments, fileName);
        }
      }
      catch (OperationCanceledException)
      {
        MessageBox.Show("�������� ���� ��������.");
      }
      catch (Exception ex)
      {
        MessageBox.Show($"�������� ������: {ex.Message}");
      }
      finally
      {
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = null;
      }
    }

    private void btn_Stop_Click(object sender, EventArgs e)
    {
        _cancellationTokenSource?.Cancel();

    }

    private void InitializeData()
    {
      var segmentGenerator = new SegmentGenerator(numberOfPoints, workingAreaWidth, workingAreaHeight, marginLeft, marginTop);
      segments = segmentGenerator.GetSegments();

      rectangle = new RectangleArea(new PointF(pointA_x, pointA_y), new PointF(pointB_x, pointB_y));
    }

    private async Task DrawSegmentsAsync()
    {
      var bitmap = new Bitmap(drawingPanel.Width, drawingPanel.Height);

      await Task.Run(() =>
      {
        using (var graphics = Graphics.FromImage(bitmap))
        {
          DrawingProvider.DrawSegments(graphics, segments, color_Segment, _cancellationTokenSource.Token);
          DrawingProvider.DrawRectangleArea(graphics, rectangle, color_RectangleArea);
          DrawingProvider.DrawSegments(graphics, intersectingSegments, color_intersectingSegment, _cancellationTokenSource.Token);
        }
      });

      drawingPanel.BackgroundImage = bitmap;
    }
  }
}
