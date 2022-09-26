using System.Numerics;
using Raylib_cs;

namespace Flappy_Bird;

public class PipeRenderer
{
    private readonly Texture2D _texture;
    private readonly Sound _pointSound;
    private float _scroll = -500;
    private readonly Game _g;
    private readonly Queue<Pipe> _pipes = new ();
    private readonly Random _random = new();

    private List<int> _completedPipes = new();

    private const int PipeSpace = 120;

    public PipeRenderer(Game g)
    {
        _texture = Raylib.LoadTexture("./assets/sprites/pipe-green.png");
        _pointSound = Raylib.LoadSound("./assets/audio/point.ogg");
        _g = g;
        var initial = new Pipe
        {
            X = 0,
            Y = (int) (_random.NextSingle() * 270 + 30)
        };
        _pipes.Enqueue(initial);
    }

    public void Reset()
    {
        _completedPipes.Clear();
        _scroll = -500;
        _pipes.Clear();
        var initial = new Pipe
        {
            X = 0,
            Y = (int) (_random.NextSingle() * 270 + 30)
        };
        _pipes.Enqueue(initial);
    }

    public void Render()
    {
        foreach (var pipe in _pipes.ToArray())
        {
            DrawPipe((int) (pipe.X-_scroll), pipe.Y, PipeSpace);
        }
    }

    private void DrawPipe(int x, int y, int space)
    {
        Raylib.DrawTexture(_texture, x, y+space, Color.WHITE);
        Raylib.DrawTextureEx(_texture, new Vector2(x+52, y), 180, 1, Color.WHITE);
    }

    public void Update()
    {
        _scroll += 80 * Raylib.GetFrameTime();

        if (_pipes.Count < 4)
        {
            var p = new Pipe
            {
                X = _pipes.Last().X + 200,
                Y = (int) (_random.NextSingle() * 270 + 30)
            };
            _pipes.Enqueue(p);
        }
        else
        {
            if (_pipes.First().X < _scroll - 50)
            {
                _pipes.Dequeue();
            }
        }
        
        foreach (var pipe in _pipes.ToArray())
        {
            if (!IsAtPipe(pipe)) continue;
            if (CollidesWithPipe(pipe, _g.Bird.Position)) _g.Bird.Die();
            else
            {
                if (_completedPipes.Contains(pipe.X)) continue;
                _completedPipes.Add(pipe.X);
                _g.Score++;
                Raylib.PlaySound(_pointSound);
            }
        }
    }

    private bool IsAtPipe(Pipe p)
    {
        return _scroll + 144 > p.X && _scroll + 144 < p.X + 32;
    }

    private bool CollidesWithPipe(Pipe p, Vector2 b)
    {
        return Raylib.CheckCollisionRecs(new Rectangle(p.X - _scroll + 20, 0, 32, p.Y), new Rectangle(b.X, b.Y, 51, 36))
            || Raylib.CheckCollisionRecs(new Rectangle(p.X - _scroll + 20, p.Y + PipeSpace, 32, 500), new Rectangle(b.X, b.Y, 51, 36));
    } 
    
    private struct Pipe
    {
        public int X;
        public int Y;
    }
}