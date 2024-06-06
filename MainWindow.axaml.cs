using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace AlgoImages;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Modulo.ValueChanged += (_, _) => UpdateBitmap();
        Scale.ValueChanged += (_, _) => UpdateBitmap();

        UpdateBitmap();
    }

    public RenderTargetBitmap Bitmap { get; set; } = new(new PixelSize(256, 256));

    private void UpdateBitmap()
    {
        var modulo = (int)(Modulo.Value ?? Modulo.Minimum);
        var scale = (int)(Scale.Value ?? Scale.Minimum);
        var dc = Bitmap.CreateDrawingContext();
        var brush = new SolidColorBrush();

        dc.FillRectangle(Brushes.White, new Rect(Bitmap.Size));

        for (var x = 0; x < Bitmap.Size.Width; x++)
        for (var y = 0; y < Bitmap.Size.Height; y++)
        {
            var result = x * y % modulo;
            var scaled = uint.MaxValue * result / (modulo - 1);
            brush.Color = Color.FromUInt32((uint)scaled);
            dc.FillRectangle(brush, new Rect(x * scale, y * scale, scale, scale));
        }

        dc.Dispose();

        // Forces rendering of image
        Image.Source = Bitmap;
    }
}