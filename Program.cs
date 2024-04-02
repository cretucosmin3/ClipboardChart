using System;
using System.Drawing;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using SkiaSharp;
using Silk.NET.Input;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using TextCopy;
using System.Globalization;
public class Program
{
    private static IWindow _window;
    private static GL _gl;

    private static uint _vao;
    private static uint _vbo;
    private static uint _ebo;

    private static uint _program;

    private static Texture MainTexture;

    private static readonly Texture[] TexturePool = new Texture[10];

    private static readonly int WindowHeight = (int)(1080f / 1.7f);
    private static readonly int WindowWidth = 1920 / 2;

    private static AppBase Application;

    public static void Main()
    {
        Clipboard clipboard = new();
        var clipboardText = clipboard.GetText();

        Console.WriteLine(clipboardText);

        WindowOptions options = WindowOptions.Default;
        options.Size = new Vector2D<int>(WindowWidth, WindowHeight);
        options.Title = "ClipChart";
        options.VSync = false;
        options.WindowState = WindowState.Normal;
        options.WindowBorder = WindowBorder.Fixed;
        options.TransparentFramebuffer = false;
        options.PreferredDepthBufferBits = null;
        options.IsEventDriven = true;
        options.FramesPerSecond = 15;

        Window.PrioritizeGlfw();

        _window = Window.Create(options);

        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;
        _window.FramebufferResize += OnResize;

        _window.FocusChanged += (x) => {
            Console.WriteLine($"Focus Changed {x}");
        };

        _window.Closing += () =>
        {

        };

        _window.Run();

        _window.Dispose();
    }

    private static void SetInput()
    {
        IInputContext _Input = _window.CreateInput();

        foreach (var mouse in _Input.Mice)
        {
            mouse.MouseMove += (mouse, position) => Application.MouseMove(mouse.Position);
            mouse.MouseDown += (mouse, button) => Application.MouseDown(mouse.Position, button);
            mouse.MouseUp += (mouse, button) => Application.MouseUp(mouse.Position, button);
        }
    }

    private static unsafe void OnLoad()
    {
        _window.WindowState = WindowState.Normal;
        _window.Center();

        SetInput();

        _gl = _window.CreateOpenGL();

        // _gl.ClearColor(Color.White);

        // Create the VAO.
        _vao = _gl.GenVertexArray();
        _gl.BindVertexArray(_vao);

        // The quad vertices data.
        float[] vertices =
        {
            // aPosition  -----  aTexCoords
             1f, -1f, 0.0f,      1.0f, 1.0f,
             1f,  1f, 0.0f,      1.0f, 0.0f,
            -1f,  1f, 0.0f,      0.0f, 0.0f,
            -1f, -1f, 0.0f,      0.0f, 1.0f
        };

        // Create the VBO.
        _vbo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);

        // Upload the vertices data to the VBO.
        fixed (float* buf = vertices)
            _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(vertices.Length * sizeof(float)), buf, BufferUsageARB.StaticDraw);

        // The quad indices data.
        uint[] indices =
        {
            0u, 1u, 3u,
            1u, 2u, 3u
        };

        // Create the EBO.
        _ebo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);

        // Upload the indices data to the EBO.
        fixed (uint* buf = indices)
            _gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)(indices.Length * sizeof(uint)), buf, BufferUsageARB.StaticDraw);

        Shader normalShader = new Shader(_gl, ShaderData.Fragment, ShaderData.Vertex);

        // Create our shader program, and attach the vertex & fragment shaders.
        _program = _gl.CreateProgram();

        normalShader.AttachToProgram(_program);

        // Attempt to "link" the program together.
        _gl.LinkProgram(_program);

        // Similar to shader compilation, check to make sure that the shader program has linked properly.
        _gl.GetProgram(_program, ProgramPropertyARB.LinkStatus, out int lStatus);

        if (lStatus != (int)GLEnum.True)
            throw new Exception("Program failed to link: " + _gl.GetProgramInfoLog(_program));

        normalShader.DetachFromProgram(_program);
        normalShader.EnableAttributes();

        // Unbind everything as we don't need it.
        _gl.BindVertexArray(0);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);

        MainTexture = new Texture(_gl, WindowWidth, WindowHeight, true);

        MainTexture.Draw(e => e.Clear());

        int location = _gl.GetUniformLocation(_program, "uTexture");
        _gl.Uniform1(location, 0);

        _gl.Enable(EnableCap.Blend);
        _gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        Application = new Application()
        {
            WindowWidth = WindowWidth,
            WindowHeight = WindowHeight
        };

        Application.Start();
    }

    private static void OnUpdate(double dt) { }

    private static unsafe void OnRender(double frameDelta)
    {
        _gl.Clear(ClearBufferMask.ColorBufferBit);

        _gl.BindVertexArray(_vao);
        _gl.UseProgram(_program);

        // The quad vertices data.
        float[] vertices =
        {
          // aPosition  --------   aTexCoords
             1f, -1f, 0.0f,      1.0f, 1.0f,
             1f,  1f, 0.0f,      1.0f, 0.0f,
            -1f,  1f, 0.0f,      0.0f, 0.0f,
            -1f, -1f, 0.0f,      0.0f, 1.0f
        };

        // Upload the vertices data to the VBO.
        fixed (float* buf = vertices)
            _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(vertices.Length * sizeof(float)), buf, BufferUsageARB.StaticDraw);

        MainTexture.Draw((canvas) =>
        {
            canvas.Clear(SKColors.White);

            foreach (var button in Application.Buttons)
            {
                button.Draw(canvas);
            }

            Application.OnDraw(canvas);
        });

        MainTexture.Render();
    }

    private static void OnResize(Vector2D<int> size)
    {
        _gl.Viewport(0, 0, (uint)size.X, (uint)size.Y);
    }
}