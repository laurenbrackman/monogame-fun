using System;
using System.Collections;
using System.Net;
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
    private AnimatedTexture slimeTexture, dogDownTexture, dogSittingTexture, dogLeftTexture, dogRightTexture, dogUpTexture;
    AnimatedTexture dogTexture;
    private const float rotation = 0;
    private const float scale = 1;
    private const float depth = 0.5f;
    private Viewport viewport;
    private const int framesPerSec = 3;
    float dogSpeed;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        slimeTexture = new AnimatedTexture(Vector2.Zero, rotation, scale, depth, 50);
        dogDownTexture = new AnimatedTexture(new Vector2(40, 120), rotation,scale, depth, 40);
        dogSittingTexture = new AnimatedTexture(Vector2.Zero, rotation,scale,depth,40);
        dogUpTexture = new AnimatedTexture(new Vector2(0, 40), rotation,scale, depth, 40);
        dogLeftTexture = new AnimatedTexture(new Vector2(0, 160), rotation,scale, depth, 40);
        dogRightTexture = new AnimatedTexture(new Vector2(0, 80), rotation,scale, depth, 40);
    }

    protected override void Initialize()
    {
        slimePosition = new Vector2(100,100);
        dogPosition = new Vector2(200,200);
        dogSpeed = 100f;
        dogTexture = dogSittingTexture;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        slimeTexture.Load(Content, "slime-sheet", 3, framesPerSec);
        dogSittingTexture.Load(Content, "puppy-spritesheet", 2, framesPerSec);
        dogDownTexture.Load(Content, "puppy-spritesheet", 2, framesPerSec);
        dogUpTexture.Load(Content, "puppy-spritesheet", 3, framesPerSec);
        dogLeftTexture.Load(Content, "puppy-spritesheet", 2, framesPerSec);
        dogRightTexture.Load(Content, "puppy-spritesheet", 2, framesPerSec);
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
    Direction currentDirection = Direction.None;

    float updatedDogSpeed = dogSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

    var kstate = Keyboard.GetState();
    
    if (kstate.IsKeyDown(Keys.Up))
    {
        dogPosition.Y -= updatedDogSpeed;
        currentDirection = Direction.Up;
    }
    
    if (kstate.IsKeyDown(Keys.Down))
    {
        dogPosition.Y += updatedDogSpeed;
        currentDirection = Direction.Down;
    }
    
    if (kstate.IsKeyDown(Keys.Left))
    {
        dogPosition.X -= updatedDogSpeed;
        currentDirection = Direction.Left;
    }
    
    if (kstate.IsKeyDown(Keys.Right))
    {
        dogPosition.X += updatedDogSpeed;
        currentDirection = Direction.Right;
    }

    switch(currentDirection){
        case Direction.Down:
            dogTexture = dogDownTexture; break;
        case Direction.Up:
            dogTexture = dogUpTexture; break;
        case Direction.Left:
            dogTexture = dogLeftTexture; break;
        case Direction.Right:
            dogTexture = dogRightTexture; break;
        default:
            dogTexture = dogSittingTexture; break;
    }
        

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
