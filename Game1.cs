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

        sourceRectangles = new Rectangle[12];
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 3; j++) {
                sourceRectangles[i * 3 + j] = new Rectangle(j * 40, i * 40, 25, 25);
            }
        }

        currentAnimationIndex = 1;
    }

    protected override void Update(GameTime gameTime)
    {if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                             Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        // The time since Update was called last.
        float updatedCharSpeed = charSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        var kstate = Keyboard.GetState();
        
        if (kstate.IsKeyDown(Keys.Up))
        {
            charPosition.Y -= updatedCharSpeed;
        }
        
        if (kstate.IsKeyDown(Keys.Down))
        {
            charPosition.Y += updatedCharSpeed;
        }
        
        if (kstate.IsKeyDown(Keys.Left))
        {
            charPosition.X -= updatedCharSpeed;
        }
        
        if (kstate.IsKeyDown(Keys.Right))
        {
            charPosition.X += updatedCharSpeed;
        }

        if (charPosition.X > _graphics.PreferredBackBufferWidth - charaset.Width / 2)
        {
            charPosition.X = _graphics.PreferredBackBufferWidth - charaset.Width / 2;
        }
        else if (charPosition.X < charaset.Width / 2)
        {
            charPosition.X = charaset.Width / 2;
        }

        if (charPosition.Y > _graphics.PreferredBackBufferHeight - charaset.Height / 2)
        {
            charPosition.Y = _graphics.PreferredBackBufferHeight - charaset.Height / 2;
        }
        else if (charPosition.Y < charaset.Height / 2)
        {
            charPosition.Y = charaset.Height / 2;
        }

        if (timer > threshold){
            if (currentAnimationIndex == 0){
                currentAnimationIndex = 1;
            }
            else{
                currentAnimationIndex = 0;
            }
            timer=0;
        }
        else
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
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
