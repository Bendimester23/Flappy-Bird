using System.Numerics;
using Raylib_cs;

namespace Flappy_Bird;

public class Bird
{
    private readonly Texture2D _texture;
    private readonly Sound _wingSound;
    private readonly Sound _dieSound;

    public Vector2 Position { get; set; }
    private Vector2 initial;
    private Vector2 _velocity;

    //TODO clean this up
    private readonly Vector2 _rotationOriginStuff = new (25.5f, 18);
    private readonly Rectangle _sourceRectangleStuff = new (0, 0, 34, 24);
    
    public bool IsDead { get; private set; }

    public Bird(Vector2 initialPosition)
    {
        //TODO do animations and stuff
        _texture = Raylib.LoadTexture("./assets/sprites/yellowbird-midflap.png");
        _wingSound = Raylib.LoadSound("./assets/audio/wing.ogg");
        _dieSound = Raylib.LoadSound("./assets/audio/die.ogg");
        Position = initialPosition;
        initial = initialPosition;
        _velocity = new Vector2(0, 0);
    }

    public void Render()
    {
        Raylib.DrawTexturePro(
            _texture, 
            _sourceRectangleStuff, 
            new Rectangle(Position.X, Position.Y, 51, 36), 
            _rotationOriginStuff, 
            _velocity.Y / 8, 
            Color.WHITE
            );
    }

    public void Reset()
    {
        Position = initial;
        _velocity = Vector2.Zero;
        IsDead = false;
    }

    public void Die()
    {
        IsDead = true;
        Raylib.PlaySound(_dieSound);
    }

    public void Update()
    {
        if (IsDead)
        {
            if (!(Position.Y + 24 <= 400)) return;
            Position += _velocity * Raylib.GetFrameTime();
            if (_velocity.Y < 800) _velocity.Y += 500 * Raylib.GetFrameTime();

            return;
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) || Raylib.IsMouseButtonPressed(0))
        {
            _velocity.Y = -250;
            Raylib.PlaySound(_wingSound);
        }

        if (Position.Y < 0)
        {
            Die();
            return;
        }
        
        //Don't fall through the ground
        if (Position.Y + 24 >= 400 && _velocity.Y > 0)
        {
            Die();
            return;
        }
        
        //Physics go brrrrrr
        Position += _velocity * Raylib.GetFrameTime();
        
        //Terminal velocity
        if (_velocity.Y < 800) _velocity.Y += 500 * Raylib.GetFrameTime();
    }
}