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
        ClearSave,
        SaveCleared,
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
        bool isRoomCleared;

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
        private Texture2D[] wandererTextures;
        private Texture2D[] shooterTextures;
        private Texture2D[] vmBossTextures;

        //Bosses
        private Texture2D[] weebTextures;

        // Items
        private List<Texture2D> chestTextures;

        //Weapons
        private wStick stick;
        private Texture2D stickTexture;
        private wRITchieClaw ritchieClaw;
        private Texture2D ritchieClawTexture;
        //private wHockeyStick hockeyStick;
        private Texture2D hockeyStickTexture;
        //private wBrickBreaker brickBreaker;
        private Texture2D brickBreakerTexture;
        //private wHotDog hotDog;
        private Texture2D hotDogTexture;
        //private wThePrecipice thePrecipice;
        private Texture2D thePrecipiceTexture;

        // Armor
        private aHoodie hoodie;
        private Texture2D hoodieTexture;
        //private aMask mask;
        private Texture2D maskTexture;
        //private aSkates skates;
        private Texture2D skatesTexture;
        //private aWinterCoat winterCoat;
        private Texture2D winterCoatTexture;
        //private aBandana bandana;
        private Texture2D bandanaTexture;

        // Managers
        private MapManager mapManager;
        private CombatManager combatManager;
        private EnemyManager enemyManager;
        private CollisionManager collisionManager;
        private ItemManager itemManager;

        //menu items
        private Texture2D startGame;
        private Texture2D startGameClicked;
        private Rectangle button1;
        private Texture2D loadGame;
        private Texture2D loadGameClicked;
        private Rectangle button2;
        private Texture2D controls;
        private Texture2D controlsClicked;
        private Rectangle button3;
        private Texture2D instructions;
        private Texture2D instructionsClicked;
        private Rectangle button4;

        //options/instruction items
        private Texture2D returnToMenu;
        private Texture2D returnToMenuClicked;
        private Rectangle button5;

        //HUD items
        private SpriteFont uiText;
        private Rectangle hudWeapon;
        private int enemyAmount;
        private Texture2D heart0;
        private Texture2D heart10;
        private Texture2D heart20;
        private Texture2D heart30;
        private Texture2D heart40;
        private Texture2D heart50;
        private Texture2D heart60;
        private Texture2D heart70;
        private Texture2D heart80;
        private Texture2D heart90;
        private Texture2D heart100;
        private Rectangle hearts;
        private Texture2D tigerBucks;
        private Rectangle money;


        //pause items
        private Texture2D returnToGame;
        private Texture2D returnToGameClicked;
        private Rectangle button6;
        private Texture2D saveGame;
        private Texture2D saveGameClicked;
        private Rectangle button8;
        private Texture2D exitToMenu;
        private Texture2D exitToMenuClicked;
        private Rectangle button9;
        private Rectangle weaponIcon;
        private Rectangle armorIcon;
        private Rectangle infoRect;
        private Rectangle currentWeapon;

        //save/load items
        private Texture2D file1;
        private Texture2D file1Clicked;
        private Rectangle file1Rect;
        private Texture2D file2;
        private Texture2D file2Clicked;
        private Rectangle file2Rect;
        private Texture2D file3;
        private Texture2D file3Clicked;
        private Rectangle file3Rect;

        //gameover items
        private Texture2D newGame;
        private Texture2D newGameClicked;
        private Rectangle button10;
        private Rectangle button11;
        private Texture2D grave;
        private Rectangle tombstone;


        //game time
        private GameTime gameTime;

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
            isRoomCleared = false;
            _graphics.PreferredBackBufferWidth = 1320;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Text
            heading = Content.Load<SpriteFont>("Roboto175");
            text = Content.Load<SpriteFont>("Roboto40");

            // Player Animations
            Dictionary<string, Animation> animations = new Dictionary<string, Animation>()
            {
                {"playerMoveForward", new Animation(Content.Load<Texture2D>("playerMovingUp2"), 4) },
                {"playerMoveBack", new Animation(Content.Load<Texture2D>("playerMovingDown2"), 4) },
                {"playerMoveLeft", new Animation(Content.Load<Texture2D>("playerMovingLeft"), 4) },
                {"playerMoveRight", new Animation(Content.Load<Texture2D>("playerMovingRight"), 4) },
                {"heroDeath", new Animation(Content.Load<Texture2D>("heroDeath"), 4) }
            };

            // Player Textures
            playerTextures = new Texture2D[] {
                Content.Load<Texture2D>("heroForward2"),
                Content.Load<Texture2D>("heroBack2"),
                Content.Load<Texture2D>("heroLeft"),
                Content.Load<Texture2D>("heroRight") };

            /// NOTE: When adding new weapons/armor, item needs to be registered in
            /// itemTextures list and Chest class to be added in chest drops.

            // Starting weapon
            stickTexture = Content.Load<Texture2D>("stick");
            ritchieClawTexture = Content.Load<Texture2D>("ritchieClaw");
            brickBreakerTexture = Content.Load<Texture2D>("BrickBreaker2");
            
            stick = new wStick(stickTexture, new Rectangle(0, 0, 0, 0));

            //Testing Weapons
            ritchieClaw = new wRITchieClaw(ritchieClawTexture, new Rectangle(0, 0, 0, 0));

            // Armor
            hoodieTexture = Content.Load<Texture2D>("Hoodie");
            hoodie = new aHoodie(hoodieTexture, new Rectangle(0, 0, 0, 0));

            // Player
            playerRect = new Rectangle(30, 440, playerTextures[0].Width, playerTextures[0].Height);
            player = new Player(playerTextures, playerRect, stick, hoodie, animations);

            // Managers
            collisionManager = new CollisionManager(player,
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight);
            combatManager = new CombatManager(player);
            enemyManager = new EnemyManager(player, collisionManager, combatManager);

            // Items
            chestTextures = new List<Texture2D>();
            chestTextures.Add(Content.Load<Texture2D>("ChestClosed"));
            chestTextures.Add(Content.Load<Texture2D>("ChestOpen"));
            itemManager = new ItemManager(player, collisionManager, chestTextures);
            itemManager.AddMoneySprite(Content.Load<Texture2D>("TigerBuck"));
            itemManager.AddHealthSprite(Content.Load<Texture2D>("Cola"));

            // Weapons
            itemManager.AddWeaponSprite(stickTexture);
            itemManager.AddWeaponSprite(ritchieClawTexture);
            hockeyStickTexture = Content.Load<Texture2D>("HockeyStick");
            hotDogTexture = Content.Load<Texture2D>("HotDog");
            thePrecipiceTexture = Content.Load<Texture2D>("thePrecipice");
            itemManager.AddWeaponSprite(brickBreakerTexture);
            itemManager.AddWeaponSprite(hockeyStickTexture);
            itemManager.AddWeaponSprite(hotDogTexture);
            itemManager.AddWeaponSprite(thePrecipiceTexture);

            // Armor
            winterCoatTexture = Content.Load<Texture2D>("WinterCoat2");
            bandanaTexture = Content.Load<Texture2D>("Bandana2");
            skatesTexture = Content.Load<Texture2D>("Skates");
            maskTexture = Content.Load<Texture2D>("Mask");
            itemManager.AddArmorSprite(hoodieTexture);
            itemManager.AddArmorSprite(winterCoatTexture);
            itemManager.AddArmorSprite(bandanaTexture);
            itemManager.AddArmorSprite(skatesTexture);
            itemManager.AddArmorSprite(maskTexture);

            // Enemy Textures
            minionTextures = new Texture2D[] {
                Content.Load<Texture2D>("minionForward"),
                Content.Load<Texture2D>("minionBack"),
                Content.Load<Texture2D>("minionLeft"),
                Content.Load<Texture2D>("minionRight") };
            enemyManager.AddEnemyData(minionTextures);

            wandererTextures = new Texture2D[] {
                Content.Load<Texture2D>("wandererForward"),
                Content.Load<Texture2D>("wandererBack"),
                Content.Load<Texture2D>("wandererLeft"),
                Content.Load<Texture2D>("wandererRight") };
            enemyManager.AddEnemyData(wandererTextures);

            shooterTextures = new Texture2D[]
            {
                Content.Load<Texture2D>("ShooterFront"),
                Content.Load<Texture2D>("ShooterBack"),
                Content.Load<Texture2D>("ShooterLeft"),
                Content.Load<Texture2D>("ShooterRight") };
            enemyManager.AddEnemyData(shooterTextures);
            // Boss Textures
            weebTextures = new Texture2D[] {
                Content.Load<Texture2D>("weeb_Forward"),
                Content.Load<Texture2D>("weeb_Back"),
                Content.Load<Texture2D>("weeb_Left"),
                Content.Load<Texture2D>("weeb_Right"),
                Content.Load<Texture2D>("thePrecipice")}; // Boss Attack Texture
            enemyManager.AddBossData(weebTextures);

            vmBossTextures = new Texture2D[]
            {
                Content.Load<Texture2D>("vmBossFront"),
                Content.Load<Texture2D>("vmBossBack"),
                Content.Load<Texture2D>("vmBossLeft"),
                Content.Load<Texture2D>("vmBossRight"),
                Content.Load<Texture2D>("HotDog")}; // Boss Attack Texture
            enemyManager.AddBossData(vmBossTextures);

            // Tiles
            List<Texture2D> tileTextures = new List<Texture2D>();
            tileTextures.Add(Content.Load<Texture2D>("BrickSprite"));
            tileTextures.Add(Content.Load<Texture2D>("BarrierSprite"));
            tileTextures.Add(Content.Load<Texture2D>("RedBrickSprite"));
            mapManager = new MapManager(tileTextures,
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight);

            //menu items
            startGame = Content.Load<Texture2D>("StartGame1");
            startGameClicked = Content.Load<Texture2D>("StartGameClicked");
            button1 = new Rectangle(305, 345, 709, 153);
            loadGame = Content.Load<Texture2D>("Load1");
            loadGameClicked = Content.Load<Texture2D>("Load1Clicked");
            button2 = new Rectangle(305, 512, 709, 153);
            controls = Content.Load<Texture2D>("Controls");
            controlsClicked = Content.Load<Texture2D>("ControlsClicked");
            button3 = new Rectangle(305, 679, 709, 153);
            instructions = Content.Load<Texture2D>("Instructions");
            instructionsClicked = Content.Load<Texture2D>("InstructionsClicked");
            button4 = new Rectangle(305, 847, 709, 153);

            //options/controls buttons
            uiText = Content.Load<SpriteFont>("Roberto20a");
            Program.drawSquare = Content.Load<Texture2D>("BlankRect2");
            returnToMenu = Content.Load<Texture2D>("ReturnToMenu");
            returnToMenuClicked = Content.Load<Texture2D>("ReturnToMenuClicked");
            button5 = new Rectangle(305, 827, 709, 153);

            //health and money for game
            heart0 = Content.Load<Texture2D>("heart0");
            heart10 = Content.Load<Texture2D>("heart10");
            heart20 = Content.Load<Texture2D>("heart20");
            heart30 = Content.Load<Texture2D>("heart30");
            heart40 = Content.Load<Texture2D>("heart40");
            heart50 = Content.Load<Texture2D>("heart50");
            heart60 = Content.Load<Texture2D>("heart60");
            heart70 = Content.Load<Texture2D>("heart70");
            heart80 = Content.Load<Texture2D>("heart80");
            heart90 = Content.Load<Texture2D>("heart90");
            heart100 = Content.Load<Texture2D>("heart100");
            hearts = new Rectangle(0, 0, 400, 40);
            tigerBucks = Content.Load<Texture2D>("TigerBuck");
            money = new Rectangle(550, -15, 40, 50);

            //paused buttons
            returnToGame = Content.Load<Texture2D>("ReturnToGame");
            returnToGameClicked = Content.Load<Texture2D>("ReturnToGameClicked");
            button6 = new Rectangle( 0, 413, 709, 153);
            saveGame = Content.Load<Texture2D>("Save");
            weaponIcon = new Rectangle(961, 398, 139, 113);
            armorIcon = new Rectangle(961, 515, 139, 113);
            infoRect = new Rectangle(0, 0, 0, 0);
            saveGameClicked = Content.Load<Texture2D>("SaveClicked");
            button8 = new Rectangle( 0, 576, 709, 153);
            exitToMenu = Content.Load<Texture2D>("ExitToMenu");
            exitToMenuClicked = Content.Load<Texture2D>("ExitToMenuClicked");
            button9 = new Rectangle( 0, 739, 709, 153);

            //save/load buttons
            file1 = Content.Load<Texture2D>("file1");
            file1Clicked = Content.Load<Texture2D>("file1Clicked");
            file1Rect = new Rectangle(112, 500, 290, 300);
            file2 = Content.Load<Texture2D>("file2");
            file2Clicked = Content.Load<Texture2D>("file2Clicked");
            file2Rect = new Rectangle(515, 500, 290, 300);
            file3 = Content.Load<Texture2D>("file1");
            file3Clicked = Content.Load<Texture2D>("file1Clicked");
            file3Rect = new Rectangle(918, 500, 290, 300);


            //Game Over Buttons
            newGame = Content.Load<Texture2D>("NewGame");
            newGameClicked = Content.Load<Texture2D>("NewGameCLicked");
            button10 = new Rectangle(305, 623, 709, 153);
            button11 = new Rectangle(305, 782, 709, 153);
            grave = Content.Load<Texture2D>("grave");
            tombstone = new Rectangle(460, 255, 400, 400);

            
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
                        Program.godMode = !Program.godMode;
                    }
                    else if (kb.IsKeyDown(Keys.C) || MouseClick(button3, mouse, prevM) == true)
                        currentState = GameState.Controls;
                    else if (kb.IsKeyDown(Keys.I) || MouseClick(button4, mouse, prevM) == true)
                        currentState = GameState.Instructions;
                    else if (SingleKeyPress(Keys.Escape, kb, prevKB))
                        Exit();
                    else if (SingleKeyPress(Keys.L, kb, prevKB) || MouseClick(button2, mouse, prevM) == true)
                    {
                        currentState = GameState.LoadChoice;
                    }
                    else if (SingleKeyPress(Keys.K, kb, prevKB))
                    {
                        currentState = GameState.ClearSave;
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
                    combatManager.PlayerAttack(player.BasicAttack(leftBState, previousLeftBState));
                    combatManager.PlayerAttack(player.Special(rightBState, previousRightBState));

                    // Enemies

                    enemyManager.MoveEnemies(gameTime);
                    combatManager.EnemyAttacks(player);
                    combatManager.CleanUp(itemManager);

                    //Collisions
                    itemManager.ActivateItems(SingleKeyPress(Keys.E, kb, prevKB));
                    collisionManager.CheckCollisions();

                    // Check if room is cleared
                    if (combatManager.EnemyCount == 0 && !isRoomCleared)
                    {
                        itemManager.OpenChests();
                        collisionManager.OpenNextRoom();
                        isRoomCleared = true;
                    }

                    // Move to next room
                    if(player.X > _graphics.PreferredBackBufferWidth 
                        || (SingleKeyPress(Keys.N, kb, prevKB) && Program.godMode))
                    {
                        player.X = 0;
                        mapManager.NextRoom();
                        if(mapManager.CurrentFloor > 6)
                        {
                            currentState = GameState.Win;
                        }

                        itemManager.NextRoom(mapManager.CurrentRoom.ChestSpawns);
                        isRoomCleared = false;

                        enemyManager.ClearEnemies();

                        // Check if boss room
                        if (mapManager.IsBossRoom)
                        {
                            enemyManager.SpawnBoss(mapManager.CurrentRoom.GetOpenTiles());
                        }
                        else
                        {
                            enemyManager.SpawnEnemies(
                                mapManager.CurrentRoom.EnemyCount,
                                mapManager.CurrentRoom.GetOpenTiles());
                        }

                        collisionManager.SetBarrierList(mapManager.CurrentRoom.GetBarriers());
                    }

                    // State transitions
                    if (SingleKeyPress(Keys.Escape, kb, prevKB))
                    {
                        currentState = GameState.Pause;
                    }

                    if(player.Hp <= 0)
                    {
                        if (Program.godMode)
                        {
                            player.Hp = player.MaxHp;
                        }
                        else
                        {
                            enemyManager.ClearEnemies();
                            player.Die();
                            player.UpdateTimer(gameTime);
                            //player.State = PlayerState.dead;
                        }
                        
                    }

                    if (player.State == PlayerState.dead)
                    {
                        currentState = GameState.GameOver;
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

                    if (SingleKeyPress(Keys.S, kb, prevKB) || MouseClick(button8, mouse, prevM) == true)
                    {
                        currentState = GameState.SaveChoice;
                    }
                    break;


                // Game Over Menu
                case GameState.GameOver:
                    if (kb.IsKeyDown(Keys.Enter) || MouseClick(button10, mouse, prevM) == true)
                    {
                        player.State = PlayerState.faceForward;
                        currentState = GameState.Game;
                        StartGame();
                    }
                    if (SingleKeyPress(Keys.Escape, kb, prevKB) || MouseClick(button11, mouse, prevM) == true)
                    {
                        player.State = PlayerState.faceForward;
                        currentState = GameState.Menu;
                    }
                    break;
                case GameState.SaveChoice:
                    //Saves player stats to the SaveFiles folder in Content
                    //The user chooses one of three available save files
                    if (SingleKeyPress(Keys.NumPad2, kb, prevKB) || MouseClick(file1Rect, mouse, prevM) == true)
                    {
                        SaveFile("saveFile2");
                    }
                    else if(SingleKeyPress(Keys.NumPad3, kb, prevKB) || MouseClick(file2Rect, mouse, prevM) == true)
                    {
                        SaveFile("saveFile3");
                    }
                    else if(SingleKeyPress(Keys.NumPad1, kb, prevKB) || MouseClick(file3Rect, mouse, prevM) == true)
                    {
                        SaveFile("saveFile1");
                    }
                    else if (SingleKeyPress(Keys.Escape, kb, prevKB))
                    {
                        currentState = GameState.Pause;
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
                    if (SingleKeyPress(Keys.NumPad2, kb, prevKB) || MouseClick(file1Rect, mouse, prevM) == true)
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
                    else if (SingleKeyPress(Keys.NumPad3, kb, prevKB) || MouseClick(file2Rect, mouse, prevM) == true)
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
                    else if (SingleKeyPress(Keys.NumPad1, kb, prevKB) || MouseClick(file3Rect, mouse, prevM) == true)
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
                case GameState.ClearSave:
                    if (SingleKeyPress(Keys.Escape, kb, prevKB))
                    {
                        currentState = GameState.Menu;
                    }
                    //The user chooses one of three available save files
                    if (SingleKeyPress(Keys.NumPad2, kb, prevKB) || MouseClick(file1Rect, mouse, prevM) == true)
                    {
                        ClearFile("saveFile2");
                    }
                    else if (SingleKeyPress(Keys.NumPad3, kb, prevKB) || MouseClick(file2Rect, mouse, prevM) == true)
                    {
                        ClearFile("saveFile3");
                    }
                    else if (SingleKeyPress(Keys.NumPad1, kb, prevKB) || MouseClick(file3Rect, mouse, prevM) == true)
                    {
                        ClearFile("saveFile1");
                    }
                    break;
                case GameState.SaveCleared:
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
            MouseState mouse = Mouse.GetState();
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
                    if(MouseOver(button1, mouse) == true)
                        _spriteBatch.Draw(startGameClicked, button1, Color.White);
                    else
                        _spriteBatch.Draw(startGame, button1, Color.White);

                    if (MouseOver(button2, mouse) == true)
                        _spriteBatch.Draw(loadGameClicked, button2, Color.White);
                    else
                        _spriteBatch.Draw(loadGame, button2, Color.White);

                    if (MouseOver(button3, mouse) == true)
                        _spriteBatch.Draw(controlsClicked, button3, Color.White);
                    else
                        _spriteBatch.Draw(controls, button3, Color.White);

                    if (MouseOver(button4, mouse) == true)
                        _spriteBatch.Draw(instructionsClicked, button4, Color.White);
                    else
                         _spriteBatch.Draw(instructions, button4, Color.White);

                    // Draw godMode toggle
                    if (Program.godMode)
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
                        new Vector2(500, 250),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        "Backwards - S",
                        new Vector2(500, 350),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        " Left - A ",
                        new Vector2(500, 450),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        "Right - D ",
                        new Vector2(500, 550),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        "Attack - Left Click",
                        new Vector2(500, 650),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        "Pause - ESC",
                        new Vector2(500, 750),
                        Color.White);
                    if (MouseOver(button5, mouse) == true)
                        _spriteBatch.Draw(returnToMenuClicked, button5, Color.White);
                    else
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
                    if (MouseOver(button5, mouse) == true)
                        _spriteBatch.Draw(returnToMenuClicked, button5, Color.White);
                    else
                        _spriteBatch.Draw(returnToMenu, button5, Color.White);
                    break;


                // Main Game Screen
                case GameState.Game:
                    mapManager.Draw(_spriteBatch);
                    itemManager.Draw(_spriteBatch);
                    player.Draw(_spriteBatch);
                    enemyManager.Draw(_spriteBatch);

                    Health(_spriteBatch, player.Hp);
                    _spriteBatch.Draw(tigerBucks, money, Color.White);
                    _spriteBatch.DrawString(
                        text,
                        "-" + player.TigerBucks,
                        new Vector2(590, -10),
                        Color.White);

                    // Draw HP bar
                    _spriteBatch.Draw(Program.drawSquare, new Rectangle(0, 0, player.MaxHp * 3 + 10, 40), Color.Black);
                    _spriteBatch.Draw(Program.drawSquare, new Rectangle(5, 5, player.Hp * 3, 30), Color.DarkRed);

                    // Draw cooldown bars
                    if (player.BasicCounter < player.CurrentWeapon.BasicCooldown)
                    {
                        _spriteBatch.Draw(Program.drawSquare, 
                            new Rectangle(player.MaxHp * 3 + 10, 0, 
                            (int)(player.CurrentWeapon.BasicCooldown * 50 - player.BasicCounter * 50), 20), 
                            Color.Green);
                    }
                    if (player.SpecialCounter < player.CurrentWeapon.SpecialCooldown)
                    {
                        _spriteBatch.Draw(Program.drawSquare,
                            new Rectangle(player.MaxHp * 3 + 10, 20,
                            (int)(player.CurrentWeapon.SpecialCooldown * 20 - player.SpecialCounter * 20), 20),
                            Color.Blue);
                    }

                    _spriteBatch.DrawString(
                        uiText,
                        mapManager.CurrentFloor + "-" + mapManager.CurrentRoomNum,
                        new Vector2(1275, 5),
                        Color.White);
                    break;


                // Pause Screen
                case GameState.Pause:
                    _spriteBatch.DrawString(
                        heading,
                        "Paused",
                        new Vector2(0, 60),
                        Color.White);
                    if (MouseOver(button6, mouse) == true)
                        _spriteBatch.Draw(returnToGameClicked, button6, Color.White);
                    else
                        _spriteBatch.Draw(returnToGame, button6, Color.White);
                    if (MouseOver(button8, mouse) == true)
                        _spriteBatch.Draw(saveGameClicked, button8, Color.White);
                    else
                        _spriteBatch.Draw(saveGame, button8, Color.White);
                    if (MouseOver(button9, mouse) == true)
                        _spriteBatch.Draw(exitToMenuClicked, button9, Color.White);
                    else
                        _spriteBatch.Draw(exitToMenu, button9, Color.White);


                    // Draw weapon info if mouse hovers over weapon icon
                    //_spriteBatch.Draw()
                    if (prevM.X > weaponIcon.X && prevM.X < weaponIcon.X + weaponIcon.Width
                        && prevM.Y > weaponIcon.Y && prevM.Y < weaponIcon.Y + weaponIcon.Height)
                    {
                        infoRect = new Rectangle(prevM.X, prevM.Y, 270, 120);

                        _spriteBatch.Draw(Program.drawSquare, infoRect, Color.DarkGray);
                        _spriteBatch.DrawString(uiText, "Damage: " + player.CurrentWeapon.Attack,
                            new Vector2(infoRect.X + 10, infoRect.Y + 3), Color.White);
                        _spriteBatch.DrawString(uiText, "Knockback: " + player.CurrentWeapon.Knockback,
                            new Vector2(infoRect.X + 10, infoRect.Y + 30), Color.White);
                        _spriteBatch.DrawString(uiText, "Basic Cooldown: " + player.CurrentWeapon.BasicCooldown,
                            new Vector2(infoRect.X + 10, infoRect.Y + 55), Color.White);
                        _spriteBatch.DrawString(uiText, "Special Cooldown: " + player.CurrentWeapon.SpecialCooldown,
                            new Vector2(infoRect.X + 10, infoRect.Y + 80), Color.White);
                    }

                    // Draw armor info if mouse hovers over armor icon
                    if (prevM.X > armorIcon.X && prevM.X < armorIcon.X + armorIcon.Width
                        && prevM.Y > armorIcon.Y && prevM.Y < armorIcon.Y + armorIcon.Height)
                    {
                        infoRect = new Rectangle(prevM.X, prevM.Y, 150, 90);

                        _spriteBatch.Draw(Program.drawSquare, infoRect, Color.DarkGray);
                        _spriteBatch.DrawString(uiText, "Defense: " + player.CurrentArmor.Defense,
                            new Vector2(infoRect.X + 10, infoRect.Y + 3), Color.White);
                        _spriteBatch.DrawString(uiText, "Speed: " + player.CurrentArmor.Speed,
                            new Vector2(infoRect.X + 10, infoRect.Y + 30), Color.White);
                        _spriteBatch.DrawString(uiText, "HP: " + player.CurrentArmor.HP,
                            new Vector2(infoRect.X + 10, infoRect.Y + 55), Color.White);
                    }

                    break;


                // Game Over Screen
                case GameState.GameOver:
                    _spriteBatch.DrawString(
                        heading,
                        "Game Over",
                        new Vector2(280, 60),
                        Color.White);
                    if (MouseOver(button10, mouse) == true)
                        _spriteBatch.Draw(newGameClicked, button10, Color.White);
                    else
                        _spriteBatch.Draw(newGame, button10, Color.White);

                    if (MouseOver(button11, mouse) == true)
                        _spriteBatch.Draw(exitToMenuClicked, button11, Color.White);
                    else
                        _spriteBatch.Draw(exitToMenu, button11, Color.White);
                    _spriteBatch.Draw(grave, tombstone, Color.White);
                    break;

                //Save file choice screen
                case GameState.SaveChoice:
                    if(MouseOver(file1Rect, mouse) == true)
                        _spriteBatch.Draw(file1Clicked, file1Rect, Color.White);
                    else
                        _spriteBatch.Draw(file1, file1Rect, Color.White);

                    if (MouseOver(file2Rect, mouse) == true)
                        _spriteBatch.Draw(file2Clicked, file2Rect, Color.White);
                    else
                        _spriteBatch.Draw(file2, file2Rect, Color.White);

                    if (MouseOver(file3Rect, mouse) == true)
                        _spriteBatch.Draw(file3Clicked, file3Rect, Color.White);
                    else
                        _spriteBatch.Draw(file3, file3Rect, Color.White);

                    _spriteBatch.DrawString(
                        heading,
                        "Save File" +
                        "Press esc to return",
                        new Vector2(550, 0),
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
                    if (MouseOver(file1Rect, mouse) == true)
                        _spriteBatch.Draw(file1Clicked, file1Rect, Color.White);
                    else
                        _spriteBatch.Draw(file1, file1Rect, Color.White);

                    if (MouseOver(file2Rect, mouse) == true)
                        _spriteBatch.Draw(file2Clicked, file2Rect, Color.White);
                    else
                        _spriteBatch.Draw(file2, file2Rect, Color.White);

                    if (MouseOver(file3Rect, mouse) == true)
                        _spriteBatch.Draw(file3Clicked, file3Rect, Color.White);
                    else
                        _spriteBatch.Draw(file3, file3Rect, Color.White);
                    _spriteBatch.DrawString(
                        heading,
                        "Load File",
                        new Vector2(350, 0),
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
                case GameState.ClearSave:
                    if (MouseOver(file1Rect, mouse) == true)
                        _spriteBatch.Draw(file1Clicked, file1Rect, Color.White);
                    else
                        _spriteBatch.Draw(file1, file1Rect, Color.White);

                    if (MouseOver(file2Rect, mouse) == true)
                        _spriteBatch.Draw(file2Clicked, file2Rect, Color.White);
                    else
                        _spriteBatch.Draw(file2, file2Rect, Color.White);

                    if (MouseOver(file3Rect, mouse) == true)
                        _spriteBatch.Draw(file3Clicked, file3Rect, Color.White);
                    else
                        _spriteBatch.Draw(file3, file3Rect, Color.White);
                    _spriteBatch.DrawString(
                       text,
                       "Would you like to clear file 1,   2,   or 3?\n" +
                       "Press esc to return",
                       new Vector2(0, 0),
                       Color.White);
                    break;
                case GameState.SaveCleared:
                    _spriteBatch.DrawString(
                       text,
                       "Save file has been cleared!\n" +
                       "Press esc to return",
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

        protected bool MouseOver(Rectangle button, MouseState currentMouse)
        {
            MouseState mouse = Mouse.GetState();
            if ((currentMouse.X >= button.Left && currentMouse.X <= button.Right) && (currentMouse.Y >= button.Top && currentMouse.Y <= button.Bottom))
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
            itemManager.NextRoom(mapManager.CurrentRoom.ChestSpawns);
            isRoomCleared = false;
            enemyManager.SpawnEnemies(
                mapManager.CurrentRoom.EnemyCount,
                mapManager.CurrentRoom.GetOpenTiles());

            collisionManager.SetBarrierList(mapManager.CurrentRoom.GetBarriers());

            // Reset player stats
            player.EquipWeapon(stick);
            player.EquipArmor(hoodie);
            player.MaxHp = 100;
            player.Hp = player.MaxHp;
            player.X = 30;
            player.Y = 440;
            player.TigerBucks = 0;

            if (Program.godMode)
            {
                itemManager.FullAccess();
            }
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
            player.TigerBucks = int.Parse(fileData[1]);

            int numFloors = int.Parse(fileData[2]);
            for(int i = 0; i < numFloors; i++)
            {
                mapManager.NewFloor();
            }

            int roomNum = int.Parse(fileData[3]);
            for(int j = 0; j < roomNum; j++)
            {
                player.X = 0;
                mapManager.NextRoom();
                if (mapManager.CurrentFloor > 6)
                {
                    currentState = GameState.Win;
                }

                itemManager.NextRoom(mapManager.CurrentRoom.ChestSpawns);
                isRoomCleared = false;

                enemyManager.ClearEnemies();

                // Check if boss room
                if (mapManager.IsBossRoom)
                {
                    enemyManager.SpawnBoss(mapManager.CurrentRoom.GetOpenTiles());
                }
                else
                {
                    enemyManager.SpawnEnemies(
                        mapManager.CurrentRoom.EnemyCount,
                        mapManager.CurrentRoom.GetOpenTiles());
                }

                collisionManager.SetBarrierList(mapManager.CurrentRoom.GetBarriers());
            }

            string currentWeaponName = fileData[4];
            switch (currentWeaponName)
            {
                case "Stick":
                    player.EquipWeapon(stick);
                    break;
                case "Ritchie Claw":
                    player.EquipWeapon(ritchieClaw);
                    break;
                case "Brick Breaker":
                    player.EquipWeapon(new wBrickBreaker(brickBreakerTexture, new Rectangle(0,0,0,0)));
                    break;
                case "Hockey Stick":
                    player.EquipWeapon(new wHockeyStick(hockeyStickTexture, new Rectangle(0, 0, 0, 0)));
                    break;
                case "Hot Dog":
                    player.EquipWeapon(new wHotDog(hotDogTexture, new Rectangle(0, 0, 0, 0)));
                    break;
                case "The Precipice":
                    player.EquipWeapon(new wThePrecipice(thePrecipiceTexture, new Rectangle(0, 0, 0, 0)));
                    break;
            }

            string currentArmorName = fileData[5];
            switch (currentArmorName)
            {
                case "Hoodie":
                    player.EquipArmor(hoodie);
                    break;
                case "Mask":
                    player.EquipArmor(new aMask(maskTexture, new Rectangle(0, 0, 0, 0)));
                    break;
                case "Bandana":
                    player.EquipArmor(new aBandana(bandanaTexture, new Rectangle(0, 0, 0, 0)));
                    break;
                case "Skates":
                    player.EquipArmor(new aSkates(skatesTexture, new Rectangle(0, 0, 0, 0)));
                    break;
                case "Winter Coat":
                    player.EquipArmor(new aWinterCoat(winterCoatTexture, new Rectangle(0, 0, 0, 0)));
                    break;
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
            writer.WriteLine($"{player.Hp}|{player.TigerBucks}|{mapManager.CurrentFloor}|" +
                $"{mapManager.CurrentRoomNum}|{player.CurrentWeapon.Name}|{player.CurrentArmor.Name}");
            writer.Close();
            currentState = GameState.Saved;
        }

        /// <summary>
        /// Noah Flanders
        /// 
        /// Helper method that deletes the save file the user chooses from 
        /// the game's content folder
        /// </summary>
        /// <param name="fileName"></param>
        private void ClearFile(string fileName)
        {
            File.Delete($"../../../Content//SaveFiles//{fileName}");
            currentState = GameState.SaveCleared;
        }

        private void Health(SpriteBatch spriteBatch, int health)
        {
            if (health > 90)
                spriteBatch.Draw(heart100, hearts, Color.White);
            else if(health <= 90 && health > 80)
                spriteBatch.Draw(heart90, hearts, Color.White);
            else if (health <= 80 && health > 70)
                spriteBatch.Draw(heart80, hearts, Color.White);
            else if (health <= 70 && health > 60)
                spriteBatch.Draw(heart70, hearts, Color.White);
            else if (health <= 60 && health > 50)
                spriteBatch.Draw(heart60, hearts, Color.White);
            else if (health <= 50 && health > 40)
                spriteBatch.Draw(heart50, hearts, Color.White);
            else if (health <= 40 && health > 30)
                spriteBatch.Draw(heart40, hearts, Color.White);
            else if (health <= 30 && health > 20)
                spriteBatch.Draw(heart30, hearts, Color.White);
            else if (health <= 20 && health > 10)
                spriteBatch.Draw(heart20, hearts, Color.White);
            else if (health <= 10 && health > 0)
                spriteBatch.Draw(heart10, hearts, Color.White);
            else if (health <= 0)
               spriteBatch.Draw(heart0, hearts, Color.White);

        }
    }
}
