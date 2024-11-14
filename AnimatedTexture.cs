using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class AnimatedTexture
{
    // Number of frames in the animation.
    private int frameCount;
    
    // The animation spritesheet.
    private Texture2D myTexture;
    
    // The number of frames to draw per second.
    private float timePerFrame;
    
    // The current frame being drawn.
    private int frame;
    
    // Total amount of time the animation has been running.
    private float totalElapsed;
    
    // Is the animation currently running?
    private bool isPaused;

    // The current rotation, scale and draw depth for the animation.
    public float Rotation, Scale, Depth;
    public int Height;
    
    // The origin point of the animated texture.
    public Vector2 Origin, Offset;

    public AnimatedTexture(Vector2 offset, float rotation, float scale, float depth, int height)
    {
        this.Offset = offset;
        this.Origin = Vector2.Zero;
        this.Rotation = rotation;
        this.Scale = scale;
        this.Depth = depth;
        this.Height = height;
    }

    public void Load(ContentManager content, string asset, int frameCount, int framesPerSec)
    {
        this.frameCount = frameCount;
        myTexture = content.Load<Texture2D>(asset);
        timePerFrame = (float)1 / framesPerSec;
        frame = 0;
        totalElapsed = 0;
        isPaused = false;
    }

    public void UpdateFrame(float elapsed)
    {
        if (isPaused)
            return;
        totalElapsed += elapsed;
        if (totalElapsed > timePerFrame)
        {
            frame++;
            // Keep the Frame between 0 and the total frames, minus one.
            frame %= frameCount;
            totalElapsed -= timePerFrame;
        }
    }

    public void DrawFrame(SpriteBatch batch, Vector2 screenPos)
    {
        DrawFrame(batch, frame, screenPos);
    }

    public void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos)
    {
        int FrameWidth = Height;
        int FrameHeight = Height;
        int XOffset = (int)Offset.X;
        int YOffset = (int)Offset.Y;
        Rectangle sourcerect = new Rectangle(FrameWidth * frame + XOffset, YOffset,
            FrameWidth - 2, FrameHeight -  1);
        batch.Draw(myTexture, screenPos, sourcerect, Color.White,
            Rotation, Origin, Scale, SpriteEffects.None, Depth);
    }

    public bool IsPaused
    {
        get { return isPaused; }
    }

    public void Reset()
    {
        frame = 0;
        totalElapsed = 0f;
    }

    public void Stop()
    {
        Pause();
        Reset();
    }

    public void Play()
    {
        isPaused = false;
    }

    public void Pause()
    {
        isPaused = true;
    }
}
