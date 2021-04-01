using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Preston Gilmore
// Alex Dale
//Hunter Wells

namespace SolsUnderground
{
    //the game states
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

        //games state
        private GameState currentState;
        bool godMode;

        //text
        private SpriteFont heading;
        private SpriteFont text;

        //keyboard and mouse
        private KeyboardState prevKB;
        private MouseState prevM;
        private ButtonState previousLeftBState;
        private ButtonState previousRightBState;


        //Player
        private Rectangle playerRect;
        private Player player;
        private Texture2D[] playerTextures;

        //weapon
        private Weapon stick;
        private Texture2D stickTexture;

        //enemy
        private Texture2D[] minionTextures;

        //Weapons
        private Weapon startWeapon;
        private Texture2D startWeaponTexture;

        // Managers
        private MapManager mapManager;
        private CombatManager combatManager;
        private EnemyManager enemyManager;
        private CollisionManager collisionManager;

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
        private int money = 0;
        private int enemyAmount;

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
            //sets game state to menu, and sets window size
            currentState = GameState.Menu;
            godMode = false;
            _graphics.PreferredBackBufferWidth = 1320;
            _graphics.PreferredBackBufferHeight = 1000;
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
            playerTextures = new Texture2D[] {
                Content.Load<Texture2D>("playerForward"),
                Content.Load<Texture2D>("playerBack"),
                Content.Load<Texture2D>("playerLeft"),
                Content.Load<Texture2D>("playerRight") };

            //weapon
            stickTexture = Content.Load<Texture2D>("stick");
            stick = new Weapon(stickTexture, new Rectangle(0, 0, 0, 0));

            //Player
            playerRect = new Rectangle(30, 440, playerTextures[0].Width, playerTextures[0].Height);
            player = new Player(playerTextures, playerRect, stick);

            // Managers
            collisionManager = new CollisionManager(player);
            combatManager = new CombatManager(player);
            enemyManager = new EnemyManager(player, collisionManager, combatManager);

            //enemy textures
            minionTextures = new Texture2D[] {
                Content.Load<Texture2D>("minionForward"),
                Content.Load<Texture2D>("minionBack"),
                Content.Load<Texture2D>("minionLeft"),
                Content.Load<Texture2D>("minionRight") };
            enemyManager.AddEnemyData(minionTextures);

            // Tiles
            List<Texture2D> tileTextures = new List<Texture2D>();
            tileTextures.Add(Content.Load<Texture2D>("BrickSprite"));
            tileTextures.Add(Content.Load<Texture2D>("BarrierSprite"));
            tileTextures.Add(Content.Load<Texture2D>("RedBrickSprite"));
            mapManager = new MapManager(tileTextures, 
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight);

            //menu items
            startGame = Content.Load<Texture2D>("startGame");
            button1 = new Rectangle(349, 345, 709, 153);
            loadGame = Content.Load<Texture2D>("LoadGame");
            button2 = new Rectangle(349, 512, 709, 153);
            controls = Content.Load<Texture2D>("Controls");
            button3 = new Rectangle(349, 679, 709, 153);
            instructions = Content.Load<Texture2D>("instructions");
            button4 = new Rectangle(349, 847, 709, 153);

            //options/controls buttons
            returnToMenu = Content.Load<Texture2D>("returnToMenu");
            button5 = new Rectangle(327, 827, 719, 145);

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
            button10 = new Rectangle(233, 643, 914, 139);
            button11 = new Rectangle(233, 782, 914, 139);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.End))
                Exit();
            
            //User Input
            KeyboardState kb = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            ButtonState leftBState = mouse.LeftButton;
            ButtonState rightBState = mouse.RightButton;


            switch (currentState)
            {
                // Main Menu State
                case GameState.Menu:
                    if (kb.IsKeyDown(Keys.Enter) || MouseClick(button1, mouse, prevM) == true)
                    {
                        currentState = GameState.Game;
                        StartGame();
                    }
                    if (SingleKeyPress(Keys.G, kb, prevKB)) // Toggle Godmode using G key
                    {
                        godMode = !godMode;
                    }
                    else if (kb.IsKeyDown(Keys.C) || MouseClick(button3, mouse, prevM) == true)
                        currentState = GameState.Controls;
                    else if (kb.IsKeyDown(Keys.I) || MouseClick(button4, mouse, prevM) == true)
                        currentState = GameState.Instructions;
                    else if (SingleKeyPress(Keys.Escape, kb, prevKB))
                        Exit();
                    break;


                // Controls Screen
                case GameState.Controls:
                    if(SingleKeyPress(Keys.Escape, kb, prevKB) || MouseClick(button5, mouse, prevM) == true)
                    {
                        currentState = GameState.Menu;
                    }
                    break;


                // Instructions Screen
                case GameState.Instructions:
                    if (kb.IsKeyDown(Keys.Escape) || MouseClick(button5, mouse, prevM) == true)
                        currentState = GameState.Menu;
                    break;


                // Main Game Loop
                case GameState.Game:

                    //Player
                    player.Input(kb, gameTime);
                    combatManager.PlayerAttack(
                        player.BasicAttack(leftBState, previousLeftBState),
                        player.Attack);

                    // Enemies
                    enemyManager.MoveEnemies();
                    combatManager.EnemyAttacks();
                    money += combatManager.CleanUp();

                    //Collisions
                    collisionManager.CheckCollisions();

                    // Move to next room
                    if(player.Width + player.X > _graphics.PreferredBackBufferWidth 
                        || (SingleKeyPress(Keys.N, kb, prevKB) && godMode))
                    {
                        player.X = 0;
                        mapManager.NextRoom();

                        enemyManager.ClearEnemies();
                        enemyManager.SpawnEnemies(
                            mapManager.CurrentRoom.EnemyCount,
                            mapManager.CurrentRoom.GetOpenTiles());

                        collisionManager.GetBarriers(mapManager.CurrentRoom.GetBarriers());
                    }

                    // State transitions
                    if (SingleKeyPress(Keys.Escape, kb, prevKB))
                    {
                        currentState = GameState.Pause;
                    }
                    if (player.Hp <= 0)
                    {
                        if (godMode)
                        {
                            player.Hp = player.MaxHp;
                        }
                        else
                        {
                            currentState = GameState.GameOver;
                        }
                    }
                    break;


                // Pause Menu
                case GameState.Pause:
                    if (SingleKeyPress(Keys.Escape, kb, prevKB) || MouseClick(button6, mouse, prevM) == true)
                    {
                        currentState = GameState.Game;
                    }
                        
                    if (kb.IsKeyDown(Keys.Q) || MouseClick(button9, mouse, prevM) == true)
                        currentState = GameState.Menu;
                    break;


                // Game Over Menu
                case GameState.GameOver:
                    if (kb.IsKeyDown(Keys.Enter) || MouseClick(button10, mouse, prevM) == true)
                    {
                        currentState = GameState.Game;
                        StartGame();
                    }
                    if (SingleKeyPress(Keys.Escape, kb, prevKB) || MouseClick(button11, mouse, prevM) == true)
                    {
                        currentState = GameState.Menu;
                    }
                    break;

            }

            //Keep track of the previous inputs
            prevKB = kb;
            prevM = mouse;
            previousLeftBState = leftBState;
            previousRightBState = rightBState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            switch (currentState)
            {
                // Main Menu State
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

                    // Draw godMode toggle
                    if (godMode)
                    {
                        _spriteBatch.DrawString(text, "GodMode", new Vector2(20, 150), Color.White);
                    }
                    break;


                // Controls Screen
                case GameState.Controls:
                     _spriteBatch.DrawString(
                        heading,
                        "Controls",
                        new Vector2(390, 0),
                        Color.White);
                      _spriteBatch.DrawString(
                        text,
                        "Forward - W",
                        new Vector2(550, 250),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        "Backwards - S",
                        new Vector2(550, 350),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        " Left - A ",
                        new Vector2(550, 450),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        "Right - D ",
                        new Vector2(550, 550),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        "Attack - Left Click",
                        new Vector2(550, 650),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        "Pause - ESC",
                        new Vector2(550, 750),
                        Color.White);
                    _spriteBatch.Draw(returnToMenu, button5, Color.White);
                    break;


                // Instructions Screen
                case GameState.Instructions:
                    _spriteBatch.DrawString(
                        heading,
                        "Instructions",
                        new Vector2(255, 0),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        "defeat all enemies to go on to the next room",
                        new Vector2(350, 250),
                        Color.White);
                    _spriteBatch.Draw(returnToMenu, button5, Color.White);
                    break;


                // Main Game Screen
                case GameState.Game:
                    mapManager.Draw(_spriteBatch);
                    player.Draw(_spriteBatch);
                    enemyManager.Draw(_spriteBatch);

                    _spriteBatch.DrawString(
                        text,
                        "health-" + player.Hp,
                        new Vector2(0, 0),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        "Tiger Bucks-" + money,
                        new Vector2(330, 0),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        "Floor-" + mapManager.CurrentFloor,
                        new Vector2(800, 0),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        "Room-" + mapManager.CurrentRoomNum,
                        new Vector2(1100, 0),
                        Color.White);
                    break;


                // Pause Screen
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
                    _spriteBatch.Draw(stickTexture, currentWeapon, Color.White);
                    
                    break;


                // Game Over Screen
                case GameState.GameOver:
                    _spriteBatch.DrawString(
                        heading,
                        "Game Over",
                        new Vector2(280, 60),
                        Color.White);
                    _spriteBatch.Draw(newGame, button10, Color.White);
                    _spriteBatch.Draw(exitToMenu, button11, Color.White);
                    break;

            }
            _spriteBatch.End();


            base.Draw(gameTime);
        }

        //allows the mouse clicks
        protected bool MouseClick(Rectangle button, MouseState currentMouse, MouseState previousMouse)
        {
            MouseState mouse = Mouse.GetState();
            if ((currentMouse.X >= button.Left && currentMouse.X <= button.Right) && (currentMouse.Y >= button.Top && currentMouse.Y <= button.Bottom) && currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            else
                return false;
        }

        //allows single key presses
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

        /// <summary>
        /// Resets all game-related variables to starting values.
        /// </summary>
        public void StartGame()
        {
            // Reset managers
            mapManager.Reset();

            enemyManager.ClearEnemies();
            enemyManager.SpawnEnemies(
                mapManager.CurrentRoom.EnemyCount,
                mapManager.CurrentRoom.GetOpenTiles());

            collisionManager.GetBarriers(mapManager.CurrentRoom.GetBarriers());

            // Reset player stats
            player.MaxHp = 100;
            player.Hp = player.MaxHp;
            player.EquipWeapon(stick);
            player.X = 30;
            player.Y = 440;
        }
    }
}
