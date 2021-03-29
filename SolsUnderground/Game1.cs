using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Preston Gilmore

namespace SolsUnderground
{
    public enum GameState
    {
        Menu,
        Controls,
        Instructions,
        Game,
        Pause,
        GameOver
            

    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameState currentState;
        private SpriteFont heading;
        private SpriteFont buttonText;
        private SpriteFont text;

        //Player
        private Texture2D playerTexture;
        private Rectangle playerRect;
        private Player player;

        //Weapons
        private Weapon startWeapon;
        private Texture2D startWeaponTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            currentState = GameState.Menu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            //Text
            heading = Content.Load<SpriteFont>("Roboto175");
            buttonText = Content.Load<SpriteFont>("Roboto100");
            text = Content.Load<SpriteFont>("Roboto40");

            //Player
            playerTexture = Content.Load<Texture2D>("tempPlayer");
            playerRect = new Rectangle(0, 0, playerTexture.Width, playerTexture.Height);
            startWeaponTexture = Content.Load<Texture2D>("stick");
            startWeapon = new Weapon(
                startWeaponTexture,
                new Rectangle(0, 0, startWeaponTexture.Width, startWeaponTexture.Height));
            player = new Player(playerTexture, playerRect, startWeapon);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState kb = Keyboard.GetState();

            switch(currentState)
            {
                case GameState.Menu:
                    if (kb.IsKeyDown(Keys.I))
                        currentState = GameState.Instructions;
                    if (kb.IsKeyDown(Keys.C))
                        currentState = GameState.Controls;
                    if (kb.IsKeyDown(Keys.Enter))
                        currentState = GameState.Game;
                        break;
                case GameState.Controls:
                    if(kb.IsKeyDown(Keys.Escape))
                        currentState = GameState.Menu;
                    break;
                case GameState.Instructions:
                    if (kb.IsKeyDown(Keys.Escape))
                        currentState = GameState.Menu;
                    break;
                case GameState.Game:
                    if (kb.IsKeyDown(Keys.Escape))
                        currentState = GameState.Pause;
                    break;
                case GameState.GameOver:
                    if (kb.IsKeyDown(Keys.Enter))
                        currentState = GameState.Game;
                    break;

            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            // TODO: Add your drawing code here


            base.Draw(gameTime);
        }
    }
}
