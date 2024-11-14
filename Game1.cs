using System;
using System.Reflection.Emit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Video_Game;

public class Game1 : Game
{
    Vector2 slimePosition, dogPosition;
    private enum Direction { Up, Down, Left, Right, None };
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private AnimatedTexture slimeTexture, dogTexture;
    private const float rotation = 0;
    private const float scale = 1;
    private const float depth = 0.5f;
    private Viewport viewport;
    private const int framesPerSec = 3;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        slimeTexture = new AnimatedTexture(Vector2.Zero, rotation, scale, depth, 50);
        dogTexture = new AnimatedTexture(Vector2.Zero, rotation, scale, depth, 40);
    }

    protected override void Initialize()
    {
        slimePosition = new Vector2(100,100);
        dogPosition = new Vector2(200,200);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        slimeTexture.Load(Content, "slime-sheet", 3, framesPerSec);
        dogTexture.Load(Content, "puppy-spritesheet", 2, framesPerSec);
        viewport = _graphics.GraphicsDevice.Viewport;
    }

protected override void Update(GameTime gameTime)
{
    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
        Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

    float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
    slimeTexture.UpdateFrame(elapsed);
    dogTexture.UpdateFrame(elapsed);

    base.Update(gameTime);
}

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.SeaGreen);

        _spriteBatch.Begin();
       
        slimeTexture.DrawFrame(_spriteBatch, slimePosition);

        dogTexture.DrawFrame(_spriteBatch, dogPosition);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
