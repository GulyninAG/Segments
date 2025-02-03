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
    private readonly int numberOfPoints = Settings.Default.numberOfPoints; // количество точек, количество отрезков = numberOfPoints - 1

    // Координаты прямоугольника
    private readonly int pointA_x = Settings.Default.pointA_x;
    private readonly int pointA_y = Settings.Default.pointA_y;
    private readonly int pointB_x = Settings.Default.pointB_x;
    private readonly int pointB_y = Settings.Default.pointB_y;

    // Рабочая область экрана
    private int workingAreaWidth;
    private int workingAreaHeight;
    private readonly int marginLeft = 2;
    private readonly int marginTop = 36;

    // Цвет линий
    private readonly Color color_Segment = Color.Green;
    private readonly Color color_RectangleArea = Color.Blue;
    private readonly Color color_intersectingSegment = Color.Red;

    Panel drawingPanel;

    // Название файла
    private readonly string fileName = "segments.bin";
    public SegmentsForm()
    {
      InitializeComponent();

      this.Text = "Отрезки и область";
      this.WindowState = FormWindowState.Maximized;
      this.DoubleBuffered = true;

      // Создаем ToolStrip и добавляем кнопки
      ToolStrip toolStrip = new ToolStrip();
      ToolStripButton btnStart = new ToolStripButton("Запуск отрисовки отрезков");
      ToolStripButton btnStop = new ToolStripButton("Отмена отрисовки (при продолжительной операции)");

      btnStart.Click += btn_Start_Click;
      btnStop.Click += btn_Stop_Click;

      toolStrip.Items.Add(btnStart);
      toolStrip.Items.Add(btnStop);

      // Добавляем ToolStrip на форму
      this.Controls.Add(toolStrip);
      toolStrip.Dock = DockStyle.Top;

      // Создаем Panel для отрисовки
      drawingPanel = new Panel();
      drawingPanel.Dock = DockStyle.Fill;
      drawingPanel.BackColor = Color.White;

      // Добавляем Panel на форму
      this.Controls.Add(drawingPanel);
    }

    private async void btn_Start_Click(object sender, EventArgs e)
    {
      _cancellationTokenSource = new CancellationTokenSource();

      drawingPanel.BackgroundImage = null;

      workingAreaWidth = drawingPanel.Width;
      workingAreaHeight = drawingPanel.Height;

      // Инициализация данных
      InitializeData();

      try
      {
        // Выполняем вычисления в фоновом потоке
        intersectingSegments = await Task.Run(() =>
            Intersection.GetSegmentsInOrIntersectingRectangle(segments, rectangle, _cancellationTokenSource.Token).ToList());

        // Асинхронная отрисовка
        await DrawSegmentsAsync();

        if (intersectingSegments.ToList().Count > 0)
        {
          SegmentSaver.SaveSegmentsToBinaryFile(intersectingSegments, fileName);
        }
      }
      catch (OperationCanceledException)
      {
        MessageBox.Show("Операция была отменена.");
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Возникла ошибка: {ex.Message}");
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
