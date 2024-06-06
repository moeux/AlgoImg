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

        Z.ValueChanged += (_, _) => Update();
        Scale.ValueChanged += (_, _) => Update();

        Update();
    }

    public RenderTargetBitmap Bitmap { get; set; } = new(new PixelSize(256, 256));

    private void Update()
    {
        UpdateBitmap((int)(Z.Value ?? Z.Minimum), (int)(Scale.Value ?? Scale.Minimum));
    }

    private void UpdateBitmap(int z, int scale)
    {
        var dc = Bitmap.CreateDrawingContext();
        var brush = new SolidColorBrush(Colors.Black);

        dc.FillRectangle(Brushes.White, new Rect(Bitmap.Size));

        for (var x = 0; x < Bitmap.Size.Width; x++)
        for (var y = 0; y < Bitmap.Size.Height; y++)
        {
            brush.Opacity = 1D * Calculate(x, y, z, 1) / (z - 1);
            dc.FillRectangle(brush, new Rect(x * scale, y * scale, scale, scale));
        }

        dc.Dispose();

        // Forces re-rendering of image
        Image.Source = Bitmap;
    }

    private static long Calculate(int x, int y, int z, int algorithm)
    {
        return algorithm switch
        {
            1 => x * y % z,
            _ => x * y
        };
    }
}