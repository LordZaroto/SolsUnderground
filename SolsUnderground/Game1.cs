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
        private SpriteFont text;
        KeyboardState prevKB;
        //Player
        private Texture2D playerForward;
        private Texture2D playerBack;
        private Texture2D playerLeft;
        private Texture2D playerRight;
        private Rectangle playerRect;
        private Player player;
        private Texture2D[] playerTextures;

        //Weapons
        private Weapon startWeapon;
        private Texture2D startWeaponTexture;


        //menu items
        private Texture2D startGame;
        private Rectangle button1;
        private Texture2D loadGame;
        private Rectangle button2;
        private Texture2D controls;
        private Rectangle button3;
        private Texture2D instructions;
        private Rectangle button4;

        //options/instruction items
        private Texture2D returnToMenu;
        private Rectangle button5;

        //HUD items
        private Rectangle hudWeapon;

        //pause items
        private Texture2D returnToGame;
        private Rectangle button6;
        private Texture2D saveGame;
        private Rectangle button7;
        private Texture2D loadGame2;
        private Rectangle button8;
        private Texture2D exitToMenu;
        private Rectangle button9;
        private Rectangle currentWeapon;

        //gameover items
        private Texture2D newGame;
        private Rectangle button10;
        private Rectangle button11;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            currentState = GameState.Menu;
            _graphics.PreferredBackBufferWidth = 1440;
            _graphics.PreferredBackBufferHeight = 1024;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Text
            heading = Content.Load<SpriteFont>("Roboto175");
            text = Content.Load<SpriteFont>("Roboto40");

            //character textures
            playerForward = Content.Load<Texture2D>("playerForward");
            playerBack = Content.Load<Texture2D>("playerBack");
            playerLeft = Content.Load<Texture2D>("playerLeft");
            playerRight = Content.Load<Texture2D>("playerRight");
            playerTextures = new Texture2D[]{playerForward, playerBack, playerLeft, playerRight};

            //Player
            playerRect = new Rectangle(0, 0, playerForward.Width, playerForward.Height);
            startWeaponTexture = Content.Load<Texture2D>("stick");
            startWeapon = new Weapon(
                startWeaponTexture,
                new Rectangle(0, 0, startWeaponTexture.Width, startWeaponTexture.Height));
            player = new Player(playerTextures, playerRect, startWeapon);

            //menu items
            startGame = Content.Load<Texture2D>("startGame");
            button1 = new Rectangle(379, 345, 709, 153);
            loadGame = Content.Load<Texture2D>("LoadGame");
            button2 = new Rectangle(379, 512, 709, 153);
            controls = Content.Load<Texture2D>("Controls");
            button3 = new Rectangle(379, 679, 709, 153);
            instructions = Content.Load<Texture2D>("instructions");
            button4 = new Rectangle(379, 847, 709, 153);

            //options/controls buttons
            returnToMenu = Content.Load<Texture2D>("returnToMenu");
            button5 = new Rectangle(347, 827, 719, 145);

            //paused buttons
            returnToGame = Content.Load<Texture2D>("return");
            button6 = new Rectangle( 0, 372, 914, 141);
            loadGame2 = Content.Load<Texture2D>("load");
            button7 = new Rectangle( 0, 513, 914, 141);
            saveGame = Content.Load<Texture2D>("Save");
            button8 = new Rectangle( 0, 653, 914, 141);
            exitToMenu = Content.Load<Texture2D>("Exit");
            button9 = new Rectangle( 0, 792, 914, 141);
            currentWeapon = new Rectangle(1161, 398, 139, 113);

            //Game Over Buttons
            newGame = Content.Load<Texture2D>("newGameGO");
            button10 = new Rectangle(263, 643, 914, 139);
            button11 = new Rectangle(263, 782, 914, 139);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.End))
                Exit();
            KeyboardState kb = Keyboard.GetState();
            

            switch(currentState)
            {
                case GameState.Menu:
                    if (kb.IsKeyDown(Keys.Enter) || MouseClick(button1.X, button1.Y, button1.Width, button1.Height) == true)
                        currentState = GameState.Game;
                    if (kb.IsKeyDown(Keys.C) || MouseClick(button3.X, button3.Y, button3.Width, button3.Height) == true)
                        currentState = GameState.Controls;
                    if (kb.IsKeyDown(Keys.I) || MouseClick(button4.X, button4.Y, button4.Width, button4.Height) == true)
                        currentState = GameState.Instructions;
                    break;
                case GameState.Controls:
                    if(SingleKeyPress(Keys.Escape, kb, prevKB) || MouseClick(button5.X, button5.Y, button5.Width, button5.Height) == true)
                    {
                        currentState = GameState.Menu;
                    }
                    break;
                case GameState.Instructions:
                    if (kb.IsKeyDown(Keys.Escape) || MouseClick(button5.X, button5.Y, button5.Width, button5.Height) == true)
                        currentState = GameState.Menu;
                    break;
                case GameState.Game:
                    if (SingleKeyPress(Keys.Escape,kb, prevKB))
                    {
                        currentState = GameState.Pause;
                    }
                        
                    if (player.Hp <= 0)
                        currentState = GameState.GameOver;
                    break;
                case GameState.Pause:
                    if (SingleKeyPress(Keys.Escape, kb, prevKB) || MouseClick(button6.X, button6.Y, button6.Width, button6.Height) == true)
                    {
                        currentState = GameState.Game;

                    }
                        
                    if (kb.IsKeyDown(Keys.Q) || MouseClick(button9.X, button9.Y, button9.Width, button9.Height) == true)
                        currentState = GameState.Menu;
                    break;
                case GameState.GameOver:
                    if (kb.IsKeyDown(Keys.Enter) || MouseClick(button10.X, button10.Y, button10.Width, button10.Height) == true)
                        currentState = GameState.Game;
                    if (SingleKeyPress(Keys.Escape, kb, prevKB) || MouseClick(button11.X, button11.Y, button11.Width, button11.Height) == true)
                    {
                        currentState = GameState.Menu;
                    }
                    break;

            }

            prevKB = kb;
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            switch (currentState)
            {
                case GameState.Menu:
                    _spriteBatch.DrawString(
                        heading,
                        "Sols UnderGround",
                        new Vector2(0, 0),
                        Color.White);
                    _spriteBatch.Draw(startGame, button1, Color.White);
                    _spriteBatch.Draw(loadGame, button2, Color.White);
                    _spriteBatch.Draw(controls, button3, Color.White);
                    _spriteBatch.Draw(instructions, button4, Color.White);
                    break;
                case GameState.Controls:
                     _spriteBatch.DrawString(
                        heading,
                        "Controls",
                        new Vector2(0, 0),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        "*insert Controls*",
                        new Vector2(0, 250),
                        Color.White);
                    _spriteBatch.Draw(returnToMenu, button5, Color.White);
                    break;
                case GameState.Instructions:
                    _spriteBatch.DrawString(
                        heading,
                        "Instructions",
                        new Vector2(0, 0),
                        Color.White);
                    _spriteBatch.DrawString(
                        heading,
                        "*insert Instructions*",
                        new Vector2(0, 250),
                        Color.White);
                    _spriteBatch.Draw(returnToMenu, button5, Color.White);
                    break;
                case GameState.Game:
                    player.Draw(_spriteBatch);
                    player.PlayerMove(Keyboard.GetState());
                    break;
                case GameState.Pause:
                    _spriteBatch.DrawString(
                        heading,
                        "Paused",
                        new Vector2(0, 60),
                        Color.White);
                    _spriteBatch.Draw(returnToGame, button6, Color.White);
                    _spriteBatch.Draw(saveGame, button7, Color.White);
                    _spriteBatch.Draw(loadGame2, button8, Color.White);
                    _spriteBatch.Draw(exitToMenu, button9, Color.White);
                    _spriteBatch.Draw(startWeaponTexture, currentWeapon, Color.White);

                    break;
                case GameState.GameOver:
                    _spriteBatch.DrawString(
                        heading,
                        "Game Over",
                        new Vector2(0, 60),
                        Color.White);
                    _spriteBatch.Draw(newGame, button10, Color.White);
                    _spriteBatch.Draw(exitToMenu, button11, Color.White);
                    break;

            }
            _spriteBatch.End();


            base.Draw(gameTime);
        }
        protected bool MouseClick(int x, int y, int width, int height)
        {
            MouseState mouse = Mouse.GetState();
            if ((mouse.X >= x && mouse.X <= x + height) && (mouse.Y >= y && mouse.Y <= y + width) && mouse.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public bool SingleKeyPress(Keys key, KeyboardState kbState, KeyboardState previousKbState)
        {
            if (previousKbState.IsKeyDown(key) && kbState.IsKeyDown(key))
            {
                return false;
            }
            else if (kbState.IsKeyDown(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
