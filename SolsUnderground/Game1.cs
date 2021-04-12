using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;

// Noah Flanders
// Preston Gilmore
// Alex Dale
// Hunter Wells
// Braden Flanders

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
        GameOver,
        SaveChoice,
        Saved,
        LoadChoice,
        LoadFailed,
        Win

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

        //enemy
        private Texture2D[] minionTextures;

        // Items
        private List<Texture2D> itemTextures;

        //Weapons
        private Stick stick;
        private RITchieClaw ritchieClaw;
        private Texture2D stickTexture;

        // Managers
        private MapManager mapManager;
        private CombatManager combatManager;
        private EnemyManager enemyManager;
        private CollisionManager collisionManager;
        private ItemManager itemManager;

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

            //character animations
            Dictionary<string, Animation> animations = new Dictionary<string, Animation>()
            {
                {"playerMoveForward", new Animation(Content.Load<Texture2D>("playerMovingUp2"), 4) },
                {"playerMoveBack", new Animation(Content.Load<Texture2D>("playerMovingDown2"), 4) },
                {"playerMoveLeft", new Animation(Content.Load<Texture2D>("playerMovingLeft"), 4) },
                {"playerMoveRight", new Animation(Content.Load<Texture2D>("playerMovingRight"), 4) }
            };

            //character textures
            playerTextures = new Texture2D[] {
                Content.Load<Texture2D>("heroForward2"),
                Content.Load<Texture2D>("heroBack2"),
                Content.Load<Texture2D>("heroLeft"),
                Content.Load<Texture2D>("heroRight") };

            // Items
            itemTextures = new List<Texture2D>();
            itemTextures.Add(Content.Load<Texture2D>("TigerBuck"));

            // Add Potion Texture

            // Weapons
            stickTexture = Content.Load<Texture2D>("stick");
            stick = new Stick(stickTexture, new Rectangle(0, 0, 0, 0));

            //Testing Weapons
            ritchieClaw = new RITchieClaw(stickTexture, new Rectangle(0, 0, 0, 0));

            //Player
            playerRect = new Rectangle(30, 440, playerTextures[0].Width, playerTextures[0].Height);
            player = new Player(playerTextures, playerRect, stick, animations);

            // Managers
            collisionManager = new CollisionManager(player);
            combatManager = new CombatManager(player);
            enemyManager = new EnemyManager(player, collisionManager, combatManager);
            itemManager = new ItemManager(player, collisionManager, itemTextures);

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
                    else if (SingleKeyPress(Keys.L, kb, prevKB))
                    {
                        currentState = GameState.LoadChoice;
                    }
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
                        player.Attack,
                        player.Knockback);

                    // Enemies
                    enemyManager.MoveEnemies();
                    combatManager.EnemyAttacks();
                    combatManager.CleanUp(itemManager);

                    //Collisions
                    itemManager.ActivateItems();
                    collisionManager.CheckCollisions();

                    // Move to next room
                    if(player.Width + player.X > _graphics.PreferredBackBufferWidth 
                        || (SingleKeyPress(Keys.N, kb, prevKB) && godMode))
                    {
                        player.X = 0;
                        mapManager.NextRoom();
                        if(mapManager.CurrentFloor > 6)
                        {
                            currentState = GameState.Win;
                        }
                        itemManager.NextRoom();

                        enemyManager.ClearEnemies();
                        enemyManager.SpawnEnemies(
                            mapManager.CurrentRoom.EnemyCount,
                            mapManager.CurrentRoom.GetOpenTiles());

                        collisionManager.GetBarrierList(mapManager.CurrentRoom.GetBarriers());
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

                    if (SingleKeyPress(Keys.S, kb, prevKB) || MouseClick(button7, mouse, prevM) == true)
                    {
                        currentState = GameState.SaveChoice;
                    }
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
                case GameState.SaveChoice:
                    //Saves player stats to the SaveFiles folder in Content
                    //The user chooses one of three available save files
                    if (SingleKeyPress(Keys.NumPad2, kb, prevKB))
                    {
                        SaveFile("saveFile2");
                    }
                    else if(SingleKeyPress(Keys.NumPad3, kb, prevKB))
                    {
                        SaveFile("saveFile3");
                    }
                    else if(SingleKeyPress(Keys.NumPad1, kb, prevKB))
                    {
                        SaveFile("saveFile1");
                    }
                    
                    break;
                case GameState.Saved:
                    //Pressing escape takes you back to the pause menu to either continue exit
                    if (SingleKeyPress(Keys.Escape, kb, prevKB))
                    {
                        currentState = GameState.Pause;
                    }
                    break;
                case GameState.LoadChoice:
                    //The user chooses one of three available save files
                    if (SingleKeyPress(Keys.NumPad2, kb, prevKB))
                    {
                        try
                        {
                            LoadGameFile("saveFile2");
                        }
                        catch
                        {
                            currentState = GameState.LoadFailed;
                        }
                    }
                    else if (SingleKeyPress(Keys.NumPad3, kb, prevKB))
                    {
                        try
                        {
                            LoadGameFile("saveFile3");
                        }
                        catch
                        {
                            currentState = GameState.LoadFailed;
                        }
                    }
                    else if (SingleKeyPress(Keys.NumPad1, kb, prevKB))
                    {
                        try
                        {
                            LoadGameFile("saveFile1");
                        }
                        catch
                        {
                            currentState = GameState.LoadFailed;
                        }
                    }
                    else if(SingleKeyPress(Keys.Escape, kb, prevKB))
                        {
                            currentState = GameState.Menu;
                        }
                    break;
                case GameState.LoadFailed:
                    if (SingleKeyPress(Keys.Escape, kb, prevKB))
                    {
                        currentState = GameState.Menu;
                    }
                        break;
                case GameState.Win:
                    if (SingleKeyPress(Keys.Escape, kb, prevKB))
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
                        new Vector2(150, 250),
                        Color.White);
                    _spriteBatch.Draw(returnToMenu, button5, Color.White);
                    break;


                // Main Game Screen
                case GameState.Game:
                    mapManager.Draw(_spriteBatch);
                    itemManager.Draw(_spriteBatch);
                    player.Draw(_spriteBatch);
                    enemyManager.Draw(_spriteBatch);
                    if(Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        player.CurrentWeapon.Draw(_spriteBatch);
                    }
                    

                    _spriteBatch.DrawString(
                        text,
                        "health-" + player.Hp,
                        new Vector2(0, 0),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        "Tiger Bucks-" + player.TigerBucks,
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

                //Save file choice screen
                case GameState.SaveChoice:
                    _spriteBatch.DrawString(
                        text,
                        "Save File 1,   2,  or 3",
                        new Vector2(0, 0),
                        Color.White);
                    break;

                //File saved confirmation screen    
                case GameState.Saved:
                    _spriteBatch.DrawString(
                        text,
                        "File Saved!\n" +
                        "Press esc to return to the pause menu",
                        new Vector2(0, 0),
                        Color.White);
                    break;
                case GameState.LoadChoice:
                    _spriteBatch.DrawString(
                        text,
                        "Load File 1,   2,  or 3",
                        new Vector2(0, 0),
                        Color.White);
                    break;
                case GameState.LoadFailed:
                    _spriteBatch.DrawString(
                        text,
                        "No save data available!\n" +
                        "Press esc to return to the menu",
                        new Vector2(0, 0),
                        Color.White);
                    break;
                case GameState.Win:
                    _spriteBatch.DrawString(
                       text,
                       "Congratulations! You have escaped with your panini!\n" +
                       "Press esc to return to the menu",
                       new Vector2(0, 0),
                       Color.White);
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
            itemManager.NextRoom();
            enemyManager.SpawnEnemies(
                mapManager.CurrentRoom.EnemyCount,
                mapManager.CurrentRoom.GetOpenTiles());

            collisionManager.GetBarrierList(mapManager.CurrentRoom.GetBarriers());

            // Reset player stats
            player.MaxHp = 100;
            player.Hp = player.MaxHp;
            player.EquipWeapon(stick);
            player.X = 30;
            player.Y = 440;
            player.TigerBucks = 0;
        }

        /// <summary>
        /// Noah Flanders
        /// 
        /// Helper method that takes in the data from the save file chosen 
        /// and adjusts the player's stats and location accordingly
        /// </summary>
        /// <param name="fileName"></param>
        private void LoadGameFile(string fileName)
        {
            StreamReader reader = new StreamReader($"../../../Content//SaveFiles//{fileName}");
            string fileLine = reader.ReadLine();
            string[] fileData = fileLine.Split('|');
            reader.Close();

            //Reset the current game
            StartGame();

            //Adjust the players stats based on the file data
            player.Hp = int.Parse(fileData[0]);
            money = int.Parse(fileData[1]);

            int numFloors = int.Parse(fileData[2]);
            for(int i = 0; i <= numFloors; i++)
            {
                mapManager.NewFloor();
            }

            int roomNum = int.Parse(fileData[3]);
            for(int j = 0; j <= roomNum; j++)
            {
                mapManager.NextRoom();
            }

            //Change the GameState
            currentState = GameState.Game;
        }

        /// <summary>
        /// Noah Flanders
        /// 
        /// Helper method that writes all of the player data to a text file
        /// within the SaveFiles folder in the game's Content folder
        /// </summary>
        /// <param name="fileName"></param>
        private void SaveFile(string fileName)
        {
            StreamWriter writer = new StreamWriter($"../../../Content//SaveFiles//{fileName}");
            writer.WriteLine($"{player.Hp}|{money}|{mapManager.CurrentFloor}|{mapManager.CurrentRoomNum}");
            writer.Close();
            currentState = GameState.Saved;
        }
    }
}
