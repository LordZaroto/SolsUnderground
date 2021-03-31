using System.Collections.Generic;
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

        //gamesstate
        private GameState currentState;

        //text
        private SpriteFont heading;
        private SpriteFont text;

        //keyboard and mouse
        KeyboardState prevKB;
        MouseState prevM;


        //Player
        private Texture2D playerForward;
        private Texture2D playerBack;
        private Texture2D playerLeft;
        private Texture2D playerRight;
        private Rectangle playerRect;
        private Player player;
        private Texture2D[] playerTextures;

        //enemy
        private Texture2D minionForward;
        private Texture2D minionBack;
        private Texture2D minionLeft;
        private Texture2D minionRight;
        private Texture2D[] minionTextures;

        //Weapons
        private Weapon startWeapon;
        private Texture2D startWeaponTexture;

        // Managers
        private MapManager mapManager;

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
            playerForward = Content.Load<Texture2D>("playerForward");
            playerBack = Content.Load<Texture2D>("playerBack");
            playerLeft = Content.Load<Texture2D>("playerLeft");
            playerRight = Content.Load<Texture2D>("playerRight");
            playerTextures = new Texture2D[]{playerForward, playerBack, playerLeft, playerRight};

            //enemy textures
            minionForward = Content.Load<Texture2D>("minionForward");
            minionBack = Content.Load<Texture2D>("minionBack");
            minionLeft = Content.Load<Texture2D>("minionLeft");
            minionRight = Content.Load<Texture2D>("minionRight");
            minionTextures = new Texture2D[] { minionForward, minionBack, minionLeft, minionRight };

            //Player
            playerRect = new Rectangle(30, 440, playerForward.Width, playerForward.Height);
            startWeaponTexture = Content.Load<Texture2D>("stick");
            startWeapon = new Weapon(
                startWeaponTexture,
                new Rectangle(0, 0, startWeaponTexture.Width, startWeaponTexture.Height));
            player = new Player(playerTextures, playerRect, startWeapon);

            // Tiles
            List<Texture2D> tileTextures = new List<Texture2D>();
            tileTextures.Add(Content.Load<Texture2D>("BrickSprite"));
            tileTextures.Add(Content.Load<Texture2D>("BarrierSprite"));
            tileTextures.Add(Content.Load<Texture2D>("RedBrickSprite"));
            mapManager = new MapManager(tileTextures, minionTextures, 
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
            KeyboardState kb = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            

            switch(currentState)
            {
                case GameState.Menu:
                    if (kb.IsKeyDown(Keys.Enter) || MouseClick(button1, mouse, prevM) == true)
                    {
                        currentState = GameState.Game;
                        mapManager.NewFloor();
                    }
                    else if (kb.IsKeyDown(Keys.C) || MouseClick(button3, mouse, prevM) == true)
                        currentState = GameState.Controls;
                    else if (kb.IsKeyDown(Keys.I) || MouseClick(button4, mouse, prevM) == true)
                        currentState = GameState.Instructions;
                    break;
                case GameState.Controls:
                    if(SingleKeyPress(Keys.Escape, kb, prevKB) || MouseClick(button5, mouse, prevM) == true)
                    {
                        currentState = GameState.Menu;
                    }
                    break;
                case GameState.Instructions:
                    if (kb.IsKeyDown(Keys.Escape) || MouseClick(button5, mouse, prevM) == true)
                        currentState = GameState.Menu;
                    break;
                case GameState.Game:
                    PlayerCollisions();
                    MinionCollisions();
                    if (SingleKeyPress(Keys.Escape,kb, prevKB))
                    {
                        currentState = GameState.Pause;
                    }

                    if(player.Width + player.X > _graphics.PreferredBackBufferWidth)
                    {
                        player.X = 0;
                        mapManager.NextRoom();
                    }
                        
                    if (player.Hp <= 0)
                        currentState = GameState.GameOver;
                    break;
                case GameState.Pause:
                    if (SingleKeyPress(Keys.Escape, kb, prevKB) || MouseClick(button6, mouse, prevM) == true)
                    {
                        currentState = GameState.Game;

                    }
                        
                    if (kb.IsKeyDown(Keys.Q) || MouseClick(button9, mouse, prevM) == true)
                        currentState = GameState.Menu;
                    break;
                case GameState.GameOver:
                    if (kb.IsKeyDown(Keys.Enter) || MouseClick(button10, mouse, prevM) == true)
                        currentState = GameState.Game;
                    if (SingleKeyPress(Keys.Escape, kb, prevKB) || MouseClick(button11, mouse, prevM) == true)
                    {
                        currentState = GameState.Menu;
                    }
                    break;

            }

            prevKB = kb;
            prevM = mouse;
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
                case GameState.Instructions:
                    _spriteBatch.DrawString(
                        heading,
                        "Instructions",
                        new Vector2(255, 0),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        "defeat all enemies to go on to the next room",
                        new Vector2(0, 250),
                        Color.White);
                    _spriteBatch.Draw(returnToMenu, button5, Color.White);
                    break;
                case GameState.Game:
                    mapManager.Draw(_spriteBatch);
                    player.Draw(_spriteBatch);

                    //Get the current enemies in the current room from the mapManager
                    //in order to draw and move them
                    List<Enemy> enemies = mapManager.Enemies;
                    foreach(Enemy e in enemies)
                    {
                        e.Draw(_spriteBatch);
                        e.EnemyMove(player);
                    }
                    player.PlayerMove(Keyboard.GetState());
                    _spriteBatch.DrawString(
                        text,
                        "health-" + player.Hp,
                        new Vector2(0, 0),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        "Tiger Bucks" + player.Hp,
                        new Vector2(330, 0),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        "Floor-" + mapManager.CurrentFloor,
                        new Vector2(800, 0),
                        Color.White);
                    _spriteBatch.DrawString(
                        text,
                        "Room-" + mapManager.CurrentRoom,
                        new Vector2(1100, 0),
                        Color.White);
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
        /// resolve collisions between the player and barriers
        /// </summary>
        public void PlayerCollisions()
        {
            List<Rectangle> barriers = mapManager.GetRoomBarriers();
            Rectangle temp = new Rectangle(player.X, player.Y, player.Width, player.Height);

            for(int i = 0; i < barriers.Count; i++)
            {
                //checks if the player intersects with a barrier
                if (barriers[i].Intersects(temp))
                {
                    //checks if the x or y needs to be adjusted
                    if (Rectangle.Intersect(temp, barriers[i]).Width <= Rectangle.Intersect(temp, barriers[i]).Height)
                    {
                        //adjusts the position
                        if (barriers[i].X > player.X)
                        {
                            temp.X -= Rectangle.Intersect(temp, barriers[i]).Width;

                        }
                        else
                        {
                            temp.X += Rectangle.Intersect(temp, barriers[i]).Width;
                        }
                    }
                    else
                    {
                        if (barriers[i].Y > temp.Y)
                        {
                            temp.Y -= Rectangle.Intersect(temp, barriers[i]).Height;

                        }
                        else
                        {
                            temp.Y += Rectangle.Intersect(temp, barriers[i]).Height;
                        }
                    }
                }
            }
            player.X = temp.X;
            player.Y = temp.Y;
        }

        //resolve collisions between the minions and barriers
        public void MinionCollisions()
        {
            List<Rectangle> barriers = mapManager.GetRoomBarriers();
            List<Enemy> enemies = mapManager.GetRoomEnemies();
            Rectangle temp = new Rectangle(0,0,0,0);

            //loops through all enemies in the room
            for (int j = 0; j < enemies.Count; j++)
            {
                temp = new Rectangle(enemies[j].X, enemies[j].Y, enemies[j].Width, enemies[j].Height);

                for (int i = 0; i < barriers.Count; i++)
                {
                    //checks if the enemies intersect with a barrier
                    if (barriers[i].Intersects(temp))
                    {
                        //checks if the x or y needs to be adjusted
                        if (Rectangle.Intersect(temp, barriers[i]).Width <= Rectangle.Intersect(temp, barriers[i]).Height)
                        {
                            //adjusts the position
                            if (barriers[i].X > player.X)
                            {
                                temp.X -= Rectangle.Intersect(temp, barriers[i]).Width;

                            }
                            else
                            {
                                temp.X += Rectangle.Intersect(temp, barriers[i]).Width;
                            }
                        }
                        else
                        {
                            if (barriers[i].Y > temp.Y)
                            {
                                temp.Y -= Rectangle.Intersect(temp, barriers[i]).Height;

                            }
                            else
                            {
                                temp.Y += Rectangle.Intersect(temp, barriers[i]).Height;
                            }
                        }
                    }
                }
                enemies[j].X = temp.X;
                enemies[j].Y = temp.Y;
            }
            
            
        }
    }
}
