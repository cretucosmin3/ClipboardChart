using SkiaSharp;

public static class PaintsLibrary
{
    public static SKPaint SimpleBlack = new SKPaint
    {
        Color = SKColors.Black,
        TextSize = 22,
        IsAntialias = true,
        Typeface = SKTypeface.FromFamilyName("DejaVu Sans",
            new SKFontStyle(300, 2, SKFontStyleSlant.Upright)
        )
    };

    public static SKPaint SimpleWhite = new SKPaint
    {
        Color = SKColors.White,
        TextSize = 22,
        IsAntialias = true,
        Typeface = SKTypeface.FromFamilyName("DejaVu Sans",
            new SKFontStyle(300, 2, SKFontStyleSlant.Upright)
        )
    };

    public static SKPaint SoftRed = new SKPaint
    {
        Color = SKColors.IndianRed,
        IsAntialias = true,
    };

    public static SKPaint SoftGray = new SKPaint
    {
        Color = SKColors.DimGray,
        IsAntialias = true,
    };

    public static SKPaint HighlightGray = new SKPaint
    {
        Color = SKColors.DimGray,
        IsAntialias = true,
    };
}