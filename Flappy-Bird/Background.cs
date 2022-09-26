using Raylib_cs;

namespace Flappy_Bird;

public class Background
{
    private const float Speed = 80;
    private readonly Texture2D _bgDay;
    private readonly Texture2D _bgNight;
    private readonly Texture2D _base;

    private float _scroll;
    private bool IsNight { get; set; }
    
    public Background()
    {
        _bgDay = Raylib.LoadTexture("./assets/sprites/background-day.png");
        _bgNight = Raylib.LoadTexture("./assets/sprites/background-night.png");
        _base = Raylib.LoadTexture("./assets/sprites/base.png");
    }

    public void Render()
    {
        Raylib.DrawTexture(IsNight ? _bgNight : _bgDay, 0, 0, Color.WHITE);
    }

    public void RenderGround()
    {
        Raylib.DrawTexture(_base, (int)_scroll%288, 400, Color.WHITE);
        Raylib.DrawTexture(_base, (int) (_scroll % 288) + 288, 400, Color.WHITE);
    }

    public void Update()
    {
        _scroll -= Speed * Raylib.GetFrameTime();
    }
}