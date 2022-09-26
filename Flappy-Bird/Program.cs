
using System.Numerics;
using Raylib_cs;

namespace Flappy_Bird
{
    public class Game
    {
        private readonly Background _background;
        public Bird Bird;
        private readonly TextRenderer _textRenderer;
        private PipeRenderer _pipeRenderer;

        private bool _started;
        private readonly Texture2D _startTexture;
        //Örs ez így nem lesz jó
        private readonly Texture2D _gameOver;
        private float _restartCountdown = 0.5f;

        private int _highscore;

        public int Score;
        
        public static void Main(String[] args)
        {
            new Game().Run();
        }

        private Game()
        {
            Raylib.InitWindow(288, 512, "Flappy Bird attempt");
            Raylib.SetWindowState(ConfigFlags.FLAG_VSYNC_HINT);
            Raylib.InitAudioDevice();

            _background = new Background();
            Bird = new Bird(new Vector2(144, 200));
            _textRenderer = new TextRenderer();
            _pipeRenderer = new PipeRenderer(this);
            _started = false;
            _startTexture = Raylib.LoadTexture("./assets/sprites/message.png");
            _gameOver = Raylib.LoadTexture("./assets/sprites/gameover.png");
            _highscore = Raylib.LoadStorageValue(0);
        }

        private void Run()
        {
            while (!Raylib.WindowShouldClose())
            {
                Loop();
            }
            
            Raylib.CloseAudioDevice();
            Raylib.CloseWindow();
        }

        private void Loop()
        {
            Raylib.ClearBackground(Color.RAYWHITE);
            Raylib.BeginDrawing();
            
            _background.Render();
            if (_started)
            {
                _pipeRenderer.Render();
                Bird.Render();
                if (Bird.IsDead)
                {
                    Raylib.DrawTexture(_gameOver, 52, 30, Color.WHITE);
                    _textRenderer.DrawCenteredText(80, Score.ToString());
                    if (Score > _highscore) _highscore = Score;
                    Raylib.DrawText($"Highscore: {_highscore}", 144-Raylib.MeasureText($"Highscore: {_highscore}", 26)/2, 120, 26, Color.BLACK);
                }
                else _textRenderer.DrawCenteredText(40, Score.ToString());
            }
            else Raylib.DrawTextureEx(_startTexture, new Vector2(7), 0, 1.5f, Color.WHITE);
            
            _background.RenderGround();
            
            Raylib.EndDrawing();

            if (!Bird.IsDead) _background.Update();
            if (_started)
            {
                Bird.Update();
                if (Bird.IsDead)
                {
                    if (_restartCountdown > 0)
                    {
                        _restartCountdown -= Raylib.GetFrameTime();
                        return;
                    }
                    if (!Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) && !Raylib.IsMouseButtonPressed(0)) return;
                    //Save highscore and reset everything
                    if (Score > _highscore)
                    {
                        Raylib.SaveStorageValue(0, Score);
                        _highscore = Score;
                    }
                    
                    Bird.Reset();
                    _pipeRenderer.Reset();
                    Score = 0;
                    _restartCountdown = 0.5f;
                }
                else _pipeRenderer.Update();
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) || Raylib.IsMouseButtonPressed(0)) _started = true;
        }
    }
}

