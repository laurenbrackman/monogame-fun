using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Video_Game;

public class Game1 : Game
{
    Texture2D puppy;
    Texture2D slime;
    Vector2 charPosition;
    float charSpeed;
    float timer;
    int threshold;
    Rectangle [] puppyRectangles;
    Rectangle [] slimeRectangles;
    byte currentAnimationIndex;
    private enum Direction { Up, Down, Left, Right, None };
    private Direction lastDirection;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        charPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
        charSpeed = 100f;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        slime = Content.Load<Texture2D>("slime-sheet");
        puppy = Content.Load<Texture2D>("puppy-spritesheet");
        timer = 0;
        threshold = 200;
        lastDirection = Direction.None;

        puppyRectangles = new Rectangle[15];
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 3; j++) {
                puppyRectangles[i * 3 + j] = new Rectangle(j * 40, i * 40, 38, 38);
            }
        }

        slimeRectangles = new Rectangle[15];
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 3; j++) {
                slimeRectangles[i * 3 + j] = new Rectangle(j * 50, i * 50, 50, 50);
            }
        }

        currentAnimationIndex = 0;
    }

protected override void Update(GameTime gameTime)
{
    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
        Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

    float updatedCharSpeed = charSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    var kstate = Keyboard.GetState();
    Direction currentDirection;

    // Check movement input and set the current direction
    if (kstate.IsKeyDown(Keys.Up))
    {
        charPosition.Y -= updatedCharSpeed;
        currentDirection = Direction.Up;
    }
    else if (kstate.IsKeyDown(Keys.Down))
    {
        charPosition.Y += updatedCharSpeed;
        currentDirection = Direction.Down;
    }
    else if (kstate.IsKeyDown(Keys.Left))
    {
        charPosition.X -= updatedCharSpeed;
        currentDirection = Direction.Left;
    }
    else if (kstate.IsKeyDown(Keys.Right))
    {
        charPosition.X += updatedCharSpeed;
        currentDirection = Direction.Right;
    }
    else{
        currentDirection = Direction.None;
    }

    // Boundary checks
    if (charPosition.X > _graphics.PreferredBackBufferWidth - puppy.Width / 2)
        charPosition.X = _graphics.PreferredBackBufferWidth - puppy.Width / 2;
    else if (charPosition.X < puppy.Width / 2)
        charPosition.X = puppy.Width / 2;

    if (charPosition.Y > _graphics.PreferredBackBufferHeight - (puppy.Height/2))
        charPosition.Y = _graphics.PreferredBackBufferHeight - (puppy.Height/2);
    else if (charPosition.Y < (puppy.Height / 2))
        charPosition.Y = puppy.Height / 2;

    // Update animation only when moving and in the same direction
    if (currentDirection != lastDirection)
        {
            switch (currentDirection)
            {
                case Direction.Up: currentAnimationIndex = 3; break;
                case Direction.Down: currentAnimationIndex = 10; break;
                case Direction.Left: currentAnimationIndex = 12; break;
                case Direction.Right: currentAnimationIndex = 6; break;
                case Direction.None: currentAnimationIndex = 0; break;
            }
            lastDirection = currentDirection; // Update last direction
        }
            // Only advance animation if timer exceeds the threshold
            if (timer > threshold)
            {
                // Cycle through animation frames for each direction
                switch (currentAnimationIndex)
                {
                    case 0: currentAnimationIndex = 1; break;
                    case 1: currentAnimationIndex = 0; break;

                    case 2: currentAnimationIndex = 4; break; // Up animation
                    case 3: currentAnimationIndex = 5; break;
                    case 4: currentAnimationIndex = 2; break;
                    case 5: currentAnimationIndex = 3; break;

                    case 9: currentAnimationIndex = 9; break; // Down animation
                    case 10: currentAnimationIndex = 11; break;
                    case 11: currentAnimationIndex = 10; break;

                    case 12: currentAnimationIndex = 13; break; // Left animation
                    case 13: currentAnimationIndex = 12; break;
                    case 14: currentAnimationIndex = 14; break;

                    case 6: currentAnimationIndex = 7; break; // Right animation
                    case 7: currentAnimationIndex = 6; break;
                    case 8: currentAnimationIndex = 8; break;
                }
                timer = 0;
            }
    
    timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

    base.Update(gameTime);
}

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.SeaGreen);

        _spriteBatch.Begin();
        _spriteBatch.Draw(
            puppy,
            charPosition,
            puppyRectangles[currentAnimationIndex],
            Color.White,
            0f,
            new Vector2(puppy.Width / 2, puppy.Height / 2),
            Vector2.One,
            SpriteEffects.None,
            0f
        );
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
