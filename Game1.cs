using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Video_Game;

public class Game1 : Game
{
    Texture2D charaset;
    Vector2 charPosition;
    float charSpeed;
    float timer;
    int threshold;
    Rectangle [] sourceRectangles;
    byte currentAnimationIndex;
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

        // TODO: use this.Content to load your game content here
        charaset = Content.Load<Texture2D>("puppy-spritesheet");
        timer = 0;
        threshold = 250;

        sourceRectangles = new Rectangle[15];
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 3; j++) {
                sourceRectangles[i * 3 + j] = new Rectangle(j * 40, i * 40, 38, 38);
            }
        }

        currentAnimationIndex = 1;
    }

    // Enum to represent the direction
private enum Direction { Up, Down, Left, Right, None };
private Direction lastDirection = Direction.None;

protected override void Update(GameTime gameTime)
{
    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
        Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

    float updatedCharSpeed = charSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    var kstate = Keyboard.GetState();
    bool isMoving = false;
    Direction currentDirection = Direction.None;

    // Check movement input and set the current direction
    if (kstate.IsKeyDown(Keys.Up))
    {
        charPosition.Y -= updatedCharSpeed;
        currentDirection = Direction.Up;
        isMoving = true;
    }
    else if (kstate.IsKeyDown(Keys.Down))
    {
        charPosition.Y += updatedCharSpeed;
        currentDirection = Direction.Down;
        isMoving = true;
    }
    else if (kstate.IsKeyDown(Keys.Left))
    {
        charPosition.X -= updatedCharSpeed;
        currentDirection = Direction.Left;
        isMoving = true;
    }
    else if (kstate.IsKeyDown(Keys.Right))
    {
        charPosition.X += updatedCharSpeed;
        currentDirection = Direction.Right;
        isMoving = true;
    }

    // Boundary checks
    if (charPosition.X > _graphics.PreferredBackBufferWidth - charaset.Width / 2)
        charPosition.X = _graphics.PreferredBackBufferWidth - charaset.Width / 2;
    else if (charPosition.X < charaset.Width / 2)
        charPosition.X = charaset.Width / 2;

    if (charPosition.Y > _graphics.PreferredBackBufferHeight - charaset.Height / 2)
        charPosition.Y = _graphics.PreferredBackBufferHeight - charaset.Height / 2;
    else if (charPosition.Y < charaset.Height / 2)
        charPosition.Y = charaset.Height / 2;

    // Update animation only when moving and in the same direction
    if (isMoving)
    {
        // Reset the starting frame if direction changed
        if (currentDirection != lastDirection)
        {
            switch (currentDirection)
            {
                case Direction.Up: currentAnimationIndex = 2; break;
                case Direction.Down: currentAnimationIndex = 9; break;
                case Direction.Left: currentAnimationIndex = 12; break;
                case Direction.Right: currentAnimationIndex = 6; break;
            }
            lastDirection = currentDirection; // Update last direction
        }
        else
        {
            // Only advance animation if timer exceeds the threshold
            if (timer > threshold)
            {
                // Cycle through animation frames for each direction
                switch (currentAnimationIndex)
                {
                    case 2: currentAnimationIndex = 3; break; // Up animation
                    case 3: currentAnimationIndex = 4; break;
                    case 4: currentAnimationIndex = 5; break;
                    case 5: currentAnimationIndex = 2; break;

                    case 9: currentAnimationIndex = 10; break; // Down animation
                    case 10: currentAnimationIndex = 11; break;
                    case 11: currentAnimationIndex = 9; break;

                    case 12: currentAnimationIndex = 13; break; // Left animation
                    case 13: currentAnimationIndex = 12; break;
                    case 14: currentAnimationIndex = 14; break;

                    case 6: currentAnimationIndex = 7; break; // Right animation
                    case 7: currentAnimationIndex = 6; break;
                    case 8: currentAnimationIndex = 8; break;
                }
                timer = 0;
            }
            else
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }
    }
    else
    {
        // Reset to standing frame when not moving
        switch (currentDirection)
        {
            case Direction.Up: currentAnimationIndex = 2; break;
            case Direction.Down: currentAnimationIndex = 9; break;
            case Direction.Left: currentAnimationIndex = 14; break;
            case Direction.Right: currentAnimationIndex = 8; break;
        }
        lastDirection = Direction.None; // Reset last direction
        timer = 0; // Reset the timer when stationary
    }

    base.Update(gameTime);
}

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        Rectangle sourceRectangle = new Rectangle(0, 40, 40, 40);
        _spriteBatch.Draw(
            charaset,
            charPosition,
            sourceRectangles[currentAnimationIndex],
            Color.White,
            0f,
            new Vector2(charaset.Width / 2, charaset.Height / 2),
            Vector2.One,
            SpriteEffects.None,
            0f
        );
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
