using Raylib_cs;

namespace Flappy_Bird;

public class TextRenderer
{
    private readonly Texture2D[] _digits;

    public TextRenderer()
    {
        _digits = new Texture2D[10];

        for (var i = 0; i < 10; i++)
        {
            _digits[i] = Raylib.LoadTexture($"./assets/sprites/{i}.png");
        }
    }

    public void DrawText(int x, int y, string txt)
    {
        var i = 0;
        foreach (var c in txt)
        {
            Raylib.DrawTexture(_digits[Int32.Parse(c.ToString())] ,x + i, y, Color.WHITE);
            i += 26;
        }
    }

    public void DrawCenteredText(int y, string txt)
    {
        DrawText(Raylib.GetScreenWidth()/2-MeasureText(txt)/2, y, txt);
    }

    private static int MeasureText(string txt)
    {
        return txt.Length * 24;
    }
}