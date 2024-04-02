using System;
using System.Collections.Generic;
using SkiaSharp;

public class Button
{
    private float X
    {
        get => Bounds.Left;
        set => Bounds = new(value, Bounds.Top, Bounds.Right, Bounds.Bottom);
    }

    private float Y
    {
        get => Bounds.Top;
        set => Bounds = new(Bounds.Left, value, Bounds.Right, Bounds.Bottom);
    }

    private float Width
    {
        get => Bounds.Right - Bounds.Left;
        set => Bounds = new(Bounds.Left, Bounds.Top, Bounds.Left + value, Bounds.Bottom);
    }

    private float Height
    {
        get => Bounds.Bottom - Bounds.Top;
        set => Bounds = new(Bounds.Left, Bounds.Top, Bounds.Right, Bounds.Top + value);
    }

    // private Dictionary<ButtonState, SKColor> StateColors = new()
    // {
    //     { ButtonState.None, new(40,40,40,255) },
    //     { ButtonState.Hovered, new(55,55,55,255) },
    //     { ButtonState.Clicked, new(75,75,75,255) }
    // };

    private Dictionary<ButtonState, SKColor> StateColors = new()
    {
        { ButtonState.None, new(255,255,255,255) },
        { ButtonState.Hovered, new(235,235,235,255) },
        { ButtonState.Clicked, new(245,245,245,255) }
    };

    public SKColor BackColor { get => StateColors[ButtonState.None]; set => StateColors[ButtonState.None] = value; }
    public SKColor HoveredBackColor { get => StateColors[ButtonState.Hovered]; set => StateColors[ButtonState.Hovered] = value; }
    public SKColor ClickedBackColor { get => StateColors[ButtonState.Clicked]; set => StateColors[ButtonState.Clicked] = value; }

    public SKColor TextColor = SKColors.Black;

    public ButtonState State = ButtonState.None;
    public string Text = null;

    public Action OnClicked;

    public static SKPaint ButtonPaint = new SKPaint
    {
        Color = SKColors.Black,
        TextSize = 18f,
        IsAntialias = true,
        Typeface = SKTypeface.FromFamilyName("Ubuntu Regular",
            new SKFontStyle(300, 2, SKFontStyleSlant.Upright)
        ),
    };

    #region Computed Variables

    private SKPoint TextPosition = new SKPoint(0, 0);
    public SKRect Bounds { get; set; }
    private SKRect TextBounds;
    private SKImageFilter ShadowFilter = SKImageFilter.CreateDropShadow(0, 2, 2, 2, new SKColor(0, 0, 0, 25), null, null);

    #endregion

    public Button(int x, int y, int width, int height)
    {
        Bounds = new SKRect(x, y, x + width, y + height);
    }

    public void Draw(SKCanvas targetCanvas)
    {
        DrawBase(targetCanvas);
        DrawText(targetCanvas);
    }

    private void DrawBase(SKCanvas targetCanvas)
    {
        ButtonPaint.ImageFilter = ShadowFilter;
        ButtonPaint.Color = StateColors[State];

        targetCanvas.DrawRoundRect(X, Y, Width, Height, 5, 5, ButtonPaint);
        ButtonPaint.ImageFilter = null;
    }

    private void DrawText(SKCanvas targetCanvas)
    {
        if (string.IsNullOrEmpty(Text)) return;

        CalculateText();

        ButtonPaint.Color = TextColor;
        targetCanvas.DrawText(Text, TextPosition, ButtonPaint);
    }


    private void CalculateText()
    {
        var cx = X;
        var cy = Y;
        var cw = Width;
        var ch = Height;

        CalculateTextBounds();

        TextPosition.X = cx + (cw / 2f) - TextBounds.MidX;
        TextPosition.Y = cy + (ch / 2f) - TextBounds.MidY;
    }

    private void CalculateTextBounds()
    {
        ButtonPaint.MeasureText(Text, ref TextBounds);
    }
}

public enum ButtonState
{
    None,
    Hovered,
    Clicked
}