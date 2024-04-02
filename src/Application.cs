
using System;
using SkiaSharp;

public class Application : AppBase
{
    private Button CloseButton;

    public override void Start()
    {
        InitButtons();

        Buttons.Add(CloseButton);
    }

    private void InitButtons()
    {
        CloseButton = new Button(WindowWidth - 100, 10, 90, 40)
        {
            BackColor = new(40,40,40),
            HoveredBackColor = new(55,55,55),
            ClickedBackColor = new(70,70,70),
            TextColor = SKColors.White,
            Text = "Close",
            OnClicked = OnCloseClicked
        };
    }

    public override void OnDraw(SKCanvas canvas)
    {

    }

    private int Counter = 0;
    private void OnCloseClicked()
    {
        Console.WriteLine("Clicked!!");
    }
}