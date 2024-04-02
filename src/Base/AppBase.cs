using System.Collections.Generic;
using System.Numerics;
using Silk.NET.Input;
using SkiaSharp;
using System;
using System.Threading;

public abstract class AppBase
{
    public int WindowWidth = 0;
    public int WindowHeight = 0;

    public List<Button> Buttons { get; set; } = new List<Button>();
    public abstract void Start();
    public abstract void OnDraw(SKCanvas canvas);

    public Button? ActiveButton = null;

    #region Public Methods

    public void MouseMove(Vector2 position)
    {
        CheckMouseInteraction(position);

        if (ActiveButton != null)
            ActiveButton.State = ButtonState.Hovered;
    }

    public void MouseDown(Vector2 position, MouseButton button)
    {
        Console.WriteLine("MouseDown");

        Thread.Sleep(5);

        if (button != MouseButton.Left) return;

        CheckMouseInteraction(position);

        if (ActiveButton != null)
            ActiveButton.State = ButtonState.Clicked;

        Program.ReRender();
    }

    public void MouseUp(Vector2 position, MouseButton button)
    {
        Console.WriteLine("MouseUp");
        if (button != MouseButton.Left) return;

        CheckMouseInteraction(position);

        if (ActiveButton != null)
        {
            ActiveButton.State = ButtonState.Hovered;
            ActiveButton.OnClicked?.Invoke();
        }

        Program.ReRender();
    }

    #endregion

    #region Private Methods

    private void CheckMouseInteraction(Vector2 position)
    {
        Button? hovered = FindHoveredButton(position);

        if (hovered != ActiveButton && ActiveButton != null)
            ActiveButton.State = ButtonState.None;

        ActiveButton = hovered;
    }

    private Button? FindHoveredButton(Vector2 position)
    {
        var intersectionRect = new SKRect(position.X, position.Y, position.X + 2, position.Y + 2);

        foreach (var button in Buttons)
        {
            if (button.Bounds.IntersectsWith(intersectionRect))
                return button;
        }

        return null;
    }

    #endregion
}