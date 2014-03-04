
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace TetrisBetaWP
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D backGroundTexture;
        Texture2D playAreaTexture;
        Texture2D spriteTexture;
        Texture2D menuScreenTexture;
        Texture2D startgameButtonTexture;
        Texture2D startnewgameButtonTexture;
        Texture2D quitButtonTexture;
        Texture2D resumeButtonTexture;
        Rectangle startgameButtonBounds;
        Rectangle startnewgameButtonBounds;
        Rectangle resumeButtonBounds;
        Rectangle quitButtonBounds;
        Point centerOfScreen;
        Point currentBlockStartingPos;
        Point nextBlockPos;
        Point playAreaPos;
        Block currentBlock;
        Block nextBlock;
        HUD hud;


        Dictionary<Point, Sprite> gridMap = new Dictionary<Point, Sprite>();
        Random random = new Random();
        GameStates gameState = GameStates.StartScreen;

        FlashINFO flashINFO;
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);

            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            centerOfScreen = new Point(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
            playAreaTexture = Content.Load<Texture2D>(@"Images\Tetris_PlayerArea_v2");
            playAreaPos = new Point(centerOfScreen.X - playAreaTexture.Width, centerOfScreen.Y - (playAreaTexture.Height / 2));

            hud = new HUD(new Vector2(playAreaPos.X, playAreaPos.Y));
            hud.Font = Content.Load<SpriteFont>(@"Fonts\Arial");
            hud.NextBoxTexture = Content.Load<Texture2D>(@"Images\TetrisBetaWP_NextButton");
            hud.LevelBoxTexture = Content.Load<Texture2D>(@"Images\TetrisBetaWP_LevelButton_orange");
            hud.ScoreBoxTexture = Content.Load<Texture2D>(@"Images\TetrisBetaWP_ScoreButton_orange_v2");
            hud.DownControlTexture = Content.Load<Texture2D>(@"Images\red_sliderDown");
            hud.UpControlTexture = Content.Load<Texture2D>(@"Images\red_sliderUp");
            hud.LeftControlTexture = Content.Load<Texture2D>(@"Images\red_sliderLeft");
            hud.RightControlTexture = Content.Load<Texture2D>(@"Images\red_sliderRight");
            hud.PausedBoxTexture = Content.Load<Texture2D>(@"Images\red_boxCross");
            hud.LoadControlPoints();

            backGroundTexture = Content.Load<Texture2D>(@"Images\TetrisBackGround_480_800");
            menuScreenTexture = Content.Load<Texture2D>(@"Images\TetrisBetaWP_StartScreen_400_400");

            startgameButtonTexture = Content.Load<Texture2D>(@"Images\TetrisBetaWP_red_button_startgame");
            startnewgameButtonTexture = Content.Load<Texture2D>(@"Images\TetrisBetaWP_red_button_startnewgame");
            quitButtonTexture = Content.Load<Texture2D>(@"Images\TetrisBetaWP_red_button_quit");
            resumeButtonTexture = Content.Load<Texture2D>(@"Images\TetrisBetaWP_red_button_resumegame");

            spriteTexture = Content.Load<Texture2D>(@"Images\TetrisSheet7");

            startgameButtonBounds = new Rectangle(centerOfScreen.X - (startgameButtonTexture.Width / 2), centerOfScreen.Y - 85, startgameButtonTexture.Width, startgameButtonTexture.Height);
            startnewgameButtonBounds = startgameButtonBounds;
            quitButtonBounds = new Rectangle(centerOfScreen.X - (quitButtonTexture.Width / 2), centerOfScreen.Y + 50, quitButtonTexture.Width, quitButtonTexture.Height);
            resumeButtonBounds = startgameButtonBounds;
            // TODO: Fix values below
            nextBlockPos = new Point(playAreaPos.X + Constants.GAMESIZEWIDTH + Constants.PLAYAREAOFFSET - (Constants.TITLESIZE * 3), playAreaPos.Y + Constants.TITLESIZE);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            TouchCollection touchState = TouchPanel.GetState();
            switch (gameState)
            {
                case GameStates.StartScreen:
                    // Touch Control Logic
                    foreach (TouchLocation location in touchState)
                    {
                        if (location.State == TouchLocationState.Pressed)
                        {
                            if (startgameButtonBounds.Contains(new Point((int)location.Position.X, (int)location.Position.Y)))
                            {
                                GetBlock();
                                GetNextBlock();
                                gameState = GameStates.Playing;
                                break;
                            }
                            else if (quitButtonBounds.Contains(new Point((int)location.Position.X, (int)location.Position.Y)))
                            {
                                this.Exit();
                            }
                        }
                    }
                    // End Touch Control Logic
                    break;
                case GameStates.Playing:
                    // refactor with Min()
                    if ((Constants.STARTINGFPS - (hud.Level * Constants.FPSREDUCTIONPERLEVEL)) < Constants.MINFPS)
                    {
                        millisecondsPerFrame = Constants.MINFPS;
                    }
                    else
                    {
                        millisecondsPerFrame = Constants.STARTINGFPS - (hud.Level * Constants.FPSREDUCTIONPERLEVEL);
                    }
                    // Touch Control Logic
                    foreach (TouchLocation location in touchState)
                    {
                        if (location.State == TouchLocationState.Pressed)
                        {
                            if (new Rectangle(hud.PausedBoxPoint.X, hud.PausedBoxPoint.Y, hud.PausedBoxTexture.Width, hud.PausedBoxTexture.Height).Contains(new Point((int)location.Position.X, (int)location.Position.Y)))
                            {
                                gameState = GameStates.Paused;
                                break;
                            }
                            else if (new Rectangle(hud.UpControlPoint.X, hud.UpControlPoint.Y, hud.UpControlTexture.Width, hud.UpControlTexture.Height).Contains(new Point((int)location.Position.X, (int)location.Position.Y)))
                            {
                                currentBlock.Rotate(gridMap);
                                break;
                            }
                            else if (new Rectangle(hud.LeftControlPoint.X, hud.LeftControlPoint.Y, hud.LeftControlTexture.Width, hud.LeftControlTexture.Height).Contains(new Point((int)location.Position.X, (int)location.Position.Y)))
                            {
                                if (!CheckForCollision(Direction.Left))
                                {
                                    currentBlock.Move(Direction.Left);
                                }
                                break;
                            }
                            else if (new Rectangle(hud.RightControlPoint.X, hud.RightControlPoint.Y, hud.RightControlTexture.Width, hud.RightControlTexture.Height).Contains(new Point((int)location.Position.X, (int)location.Position.Y)))
                            {
                                if (!CheckForCollision(Direction.Right))
                                {
                                    currentBlock.Move(Direction.Right);
                                }
                                break;
                            }
                            else if (new Rectangle(hud.DownControlPoint.X, hud.DownControlPoint.Y, hud.DownControlTexture.Width, hud.DownControlTexture.Height).Contains(new Point((int)location.Position.X, (int)location.Position.Y)))
                            {
                                if (!CheckForCollision(Direction.Down))
                                {
                                    currentBlock.Move(Direction.Down);
                                }
                                else
                                {
                                    AddSpritesToMap();
                                    if (CheckForClearedLines())
                                    {
                                        gameState = GameStates.Flashing;
                                        break;
                                    }
                                    else
                                    {
                                        currentBlock = nextBlock;
                                        currentBlock.playarea = new Point(playAreaPos.X, playAreaPos.Y);
                                        GetNextBlock();
                                    }
                                }
                                break;
                            }
                        }
                    }
                    // End Touch Control Logic

                    timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                    if (timeSinceLastFrame > millisecondsPerFrame)
                    {
                        timeSinceLastFrame -= millisecondsPerFrame;
                        if (!CheckForCollision(Direction.Down))
                        {
                            currentBlock.Move(Direction.Down);
                        }
                        else
                        {
                            AddSpritesToMap();
                            if (CheckForClearedLines())
                            {
                                gameState = GameStates.Flashing;
                                break;
                            }
                            else
                            {
                                currentBlock = nextBlock;
                                currentBlock.playarea = new Point(playAreaPos.X, playAreaPos.Y);
                                GetNextBlock();
                            }
                        }
                    }
                    break;
                case GameStates.Flashing:
                    // Touch Control Logic
                    foreach (TouchLocation location in touchState)
                    {
                        if (location.State == TouchLocationState.Pressed)
                        {
                            if (new Rectangle(hud.PausedBoxPoint.X, hud.PausedBoxPoint.Y, hud.PausedBoxTexture.Width, hud.PausedBoxTexture.Height).Contains(new Point((int)location.Position.X, (int)location.Position.Y)))
                            {
                                gameState = GameStates.Paused;
                                break;
                            }
                        }
                    }
                    // End Touch Control Logic
                    timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                    if (timeSinceLastFrame > Constants.FLASHFPS)
                    {
                        timeSinceLastFrame -= Constants.FLASHFPS;
                        flashINFO.flashcount++;
                        if (flashINFO.flashing)
                        {
                            flashINFO.flashing = false;
                        }
                        else
                        {
                            flashINFO.flashing = true;
                        }
                        if (flashINFO.flashcount > Constants.FLASHCOUNT)
                        {
                            gameState = GameStates.Playing;
                            RemoveClearedLines();
                            currentBlock = nextBlock;
                            currentBlock.playarea = new Point(playAreaPos.X, playAreaPos.Y);
                            GetNextBlock();
                        }
                    }
                    break;
                case GameStates.Paused:
                    // Touch Control Logic
                    foreach (TouchLocation location in touchState)
                    {
                        if (location.State == TouchLocationState.Pressed)
                        {
                            if (resumeButtonBounds.Contains(new Point((int)location.Position.X, (int)location.Position.Y)))
                            {
                                gameState = GameStates.Playing;
                                break;
                            }
                            else if (quitButtonBounds.Contains(new Point((int)location.Position.X, (int)location.Position.Y)))
                            {
                                this.Exit();
                            }
                        }
                    }
                    // End Touch Control Logic
                    break;
                case GameStates.GameOver:
                    // Touch Control Logic
                    foreach (TouchLocation location in touchState)
                    {
                        if (location.State == TouchLocationState.Pressed)
                        {
                            if (startnewgameButtonBounds.Contains(new Point((int)location.Position.X, (int)location.Position.Y)))
                            {
                                gridMap.Clear();
                                gameState = GameStates.Playing;
                                hud.Restart();
                                GetBlock();
                                GetNextBlock();
                                break;
                            }
                            else if (quitButtonBounds.Contains(new Point((int)location.Position.X, (int)location.Position.Y)))
                            {
                                this.Exit();
                            }
                        }
                    }
                    // End Touch Control Logic
                    break;
                default:
                    break;
            }

            // TODO: Fix input below
            /*
            switch (gameState)
            {
                case GameStates.StartScreen:
                    if (input.WasKeyPressed(Keys.Enter))
                    {
                        GetBlock();
                        GetNextBlock();
                        gameState = GameStates.Playing;
                    }
                    break;
                case GameStates.Playing:
                    // refactor with Min()
                    if ((Constants.STARTINGFPS - (hud.Level * Constants.FPSREDUCTIONPERLEVEL)) < Constants.MINFPS)
                    {
                        millisecondsPerFrame = Constants.MINFPS;
                    }
                    else
                    {
                        millisecondsPerFrame = Constants.STARTINGFPS - (hud.Level * Constants.FPSREDUCTIONPERLEVEL);
                    }
                    // TODO: Fix Input
                    if (input.WasKeyPressed(Keys.Escape))
                    {
                        gameState = GameStates.Paused;
                    }
                    // add in holding down/left/right later
                    if (input.WasKeyPressed(Keys.Up))
                    {
                        currentBlock.Rotate(gridMap);
                    }
                    if (input.WasKeyPressed(Keys.Left))
                    {
                        //check blocks & walls
                        //      if no collision, move left
                        if (!CheckForCollision(Direction.Left))
                        {
                            currentBlock.Move(Direction.Left);
                        }
                    }
                    if (input.WasKeyPressed(Keys.Right))
                    {
                        //check blocks & walls
                        //      if no collision, move right
                        if (!CheckForCollision(Direction.Right))
                        {
                            currentBlock.Move(Direction.Right);
                        }
                    }
                    if (input.WasKeyPressed(Keys.Down))
                    {
                        //check blocks & walls
                        //      if collision
                        //          check for cleared lines, and add sprites to Map and drop new block
                        //      if no collision, move down
                        if (!CheckForCollision(Direction.Down))
                        {
                            currentBlock.Move(Direction.Down);
                        }
                        else
                        {
                            AddSpritesToMap();
                            if (CheckForClearedLines())
                            {
                                gameState = GameStates.Flashing;
                                break;
                            }
                            else
                            {
                                currentBlock = nextBlock;
                                currentBlock.playarea = new Point(playAreaPos.X, playAreaPos.Y);
                                GetNextBlock();
                            }
                        }
                    }

                    timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                    if (timeSinceLastFrame > millisecondsPerFrame)
                    {
                        timeSinceLastFrame -= millisecondsPerFrame;
                        //check blocks & walls
                        //      if collision
                        //          check for cleared lines, and add sprites to Map and drop new block
                        //      if no collision, move down
                        if (!CheckForCollision(Direction.Down))
                        {
                            currentBlock.Move(Direction.Down);
                        }
                        else
                        {
                            AddSpritesToMap();
                            if (CheckForClearedLines())
                            {
                                gameState = GameStates.Flashing;
                                break;
                            }
                            else
                            {
                                currentBlock = nextBlock;
                                currentBlock.playarea = new Point(playAreaPos.X, playAreaPos.Y);
                                GetNextBlock();
                            }
                        }
                    }
                    break;
                case GameStates.Flashing:
                    timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                    if (timeSinceLastFrame > Constants.FLASHFPS)
                    {
                        timeSinceLastFrame -= Constants.FLASHFPS;
                        flashINFO.flashcount++;
                        if (flashINFO.flashing)
                        {
                            flashINFO.flashing = false;
                        }
                        else
                        {
                            flashINFO.flashing = true;
                        }
                        if (flashINFO.flashcount > Constants.FLASHCOUNT)
                        {
                            gameState = GameStates.Playing;
                            RemoveClearedLines();
                            currentBlock = nextBlock;
                            currentBlock.playarea = new Point(playAreaPos.X, playAreaPos.Y);
                            GetNextBlock();
                        }
                    }
                    break;
                case GameStates.Paused:
                    if (input.WasKeyPressed(Keys.Enter))
                    {
                        gameState = GameStates.Playing;
                    }
                    break;
                case GameStates.GameOver:
                    if (input.WasKeyPressed(Keys.Enter))
                    {
                        gridMap.Clear();
                        gameState = GameStates.Playing;
                        hud.Restart();
                        GetBlock();
                        GetNextBlock();
                    }
                    break;
                default:
                    break;
            }
            */
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(backGroundTexture, new Rectangle(0, 0, backGroundTexture.Width, backGroundTexture.Height), Color.White);
            switch (gameState)
            {
                case GameStates.StartScreen:
                    spriteBatch.Draw(menuScreenTexture, new Rectangle(centerOfScreen.X - (menuScreenTexture.Width / 2), centerOfScreen.Y - (menuScreenTexture.Height / 2), menuScreenTexture.Width, menuScreenTexture.Height), Color.White);
                    spriteBatch.Draw(startgameButtonTexture, startgameButtonBounds, Color.White);
                    spriteBatch.Draw(quitButtonTexture, quitButtonBounds, Color.White);
                    break;
                case GameStates.Playing:
                    spriteBatch.Draw(playAreaTexture, new Rectangle(playAreaPos.X, playAreaPos.Y, playAreaTexture.Width, playAreaTexture.Height), Color.White);
                    currentBlock.Draw(spriteBatch);
                    nextBlock.Draw(spriteBatch);
                    foreach (Point key in gridMap.Keys)
                    {
                        gridMap[key].Draw(spriteBatch, (key.X * Constants.TITLESIZE) + playAreaPos.X, (key.Y * Constants.TITLESIZE) + playAreaPos.Y);
                    }
                    hud.Draw(spriteBatch);
                    break;
                case GameStates.Flashing:
                    spriteBatch.Draw(playAreaTexture, new Rectangle(playAreaPos.X, playAreaPos.Y, playAreaTexture.Width, playAreaTexture.Height), Color.White);
                    nextBlock.Draw(spriteBatch);
                    if (flashINFO.flashing)
                    {
                        foreach (Point key in gridMap.Keys)
                        {
                            if (!flashINFO.flashGrid.ContainsKey(key))
                            {
                                gridMap[key].Draw(spriteBatch, (key.X * Constants.TITLESIZE) + playAreaPos.X, (key.Y * Constants.TITLESIZE) + playAreaPos.Y);
                            }
                        }
                        foreach (Point key in flashINFO.flashGrid.Keys)
                        {
                            flashINFO.flashGrid[key].Draw(spriteBatch, (key.X * Constants.TITLESIZE) + playAreaPos.X, (key.Y * Constants.TITLESIZE) + playAreaPos.Y);
                        }
                    }
                    else
                    {
                        foreach (Point key in gridMap.Keys)
                        {
                            gridMap[key].Draw(spriteBatch, (key.X * Constants.TITLESIZE) + playAreaPos.X, (key.Y * Constants.TITLESIZE) + playAreaPos.Y);
                        }
                    }
                    hud.Draw(spriteBatch);
                    break;
                case GameStates.Paused:
                    spriteBatch.Draw(playAreaTexture, new Rectangle(playAreaPos.X, playAreaPos.Y, playAreaTexture.Width, playAreaTexture.Height), Color.White);
                    currentBlock.Draw(spriteBatch);
                    nextBlock.Draw(spriteBatch);
                    foreach (Point key in gridMap.Keys)
                    {
                        gridMap[key].Draw(spriteBatch, (key.X * Constants.TITLESIZE) + playAreaPos.X, (key.Y * Constants.TITLESIZE) + playAreaPos.Y);
                    }
                    hud.Draw(spriteBatch);
                    spriteBatch.Draw(menuScreenTexture, new Rectangle(centerOfScreen.X - (menuScreenTexture.Width / 2), centerOfScreen.Y - (menuScreenTexture.Height / 2), menuScreenTexture.Width, menuScreenTexture.Height), Color.White);
                    spriteBatch.Draw(resumeButtonTexture, resumeButtonBounds, Color.White);
                    spriteBatch.Draw(quitButtonTexture, quitButtonBounds, Color.White);
                    break;
                case GameStates.GameOver:
                    spriteBatch.Draw(playAreaTexture, new Rectangle(playAreaPos.X, playAreaPos.Y, playAreaTexture.Width, playAreaTexture.Height), Color.White);
                    nextBlock.Draw(spriteBatch);
                    foreach (Point key in gridMap.Keys)
                    {
                        gridMap[key].Draw(spriteBatch, (key.X * Constants.TITLESIZE) + playAreaPos.X, (key.Y * Constants.TITLESIZE) + playAreaPos.Y);
                    }
                    hud.Draw(spriteBatch);
                    spriteBatch.Draw(menuScreenTexture, new Rectangle(centerOfScreen.X - (menuScreenTexture.Width / 2), centerOfScreen.Y - (menuScreenTexture.Height / 2), menuScreenTexture.Width, menuScreenTexture.Height), Color.White);
                    spriteBatch.Draw(startnewgameButtonTexture, startnewgameButtonBounds, Color.White);
                    spriteBatch.Draw(quitButtonTexture, quitButtonBounds, Color.White);
                    break;
                default:
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void GetBlock()
        {
            currentBlock = new Block(spriteTexture, 0, 0, Constants.TITLESIZE, Constants.TITLESIZE, random, playAreaPos);
        }

        private void GetNextBlock()
        {
            nextBlock = new Block(spriteTexture, 0, 0, Constants.TITLESIZE, Constants.TITLESIZE, random, nextBlockPos);
        }

        private void AddSpritesToMap()
        {
            // voodoo below
            var tempDC = new Dictionary<Point, Sprite>[1];
            tempDC[0] = new Dictionary<Point, Sprite>(currentBlock.ReturnGridAndSprites());
            bool gameover = false;
            foreach (Dictionary<Point,Sprite> result in tempDC)
            {
                foreach (var item in result)
                {
                    if (!gridMap.ContainsKey(item.Key))
                    {
                        gridMap.Add(item.Key, item.Value);
                    }
                    else
                    {
                        gameover = true;
                    }
                }
            }
            if (gameover)
            {
                gameState = GameStates.GameOver;
            }
        }

        private bool CheckForCollision(Direction direction)
        {
            bool collision = false;
            List<Point> gridLocations = new List<Point>(currentBlock.ReturnGridLocations());
            switch (direction)
            {
                case Direction.Left:
                    foreach (Point gridLocation in gridLocations)
                    {
                        Point newGridLocation = new Point(gridLocation.X - 1, gridLocation.Y);
                        if (gridLocation.X == 0 || gridMap.ContainsKey(newGridLocation))
                        {
                            collision = true;
                        }
                    }
                    break;
                case Direction.Right:
                    foreach (Point gridLocation in gridLocations)
                    {
                        Point newGridLocation = new Point(gridLocation.X + 1, gridLocation.Y);
                        if (gridLocation.X == Constants.GAMEWIDTH - 1 || gridMap.ContainsKey(newGridLocation))
                        {
                            collision = true;
                        }
                    }
                    break;
                case Direction.Down:
                    foreach (Point gridLocation in gridLocations)
                    {
                        Point newGridLocation = new Point(gridLocation.X, gridLocation.Y + 1);
                        if (gridLocation.Y == Constants.GAMEHEIGHT - 1 || gridMap.ContainsKey(newGridLocation))
                        {
                            collision = true;
                        }
                    }
                    break;
                default:
                    break;
            }
            return collision;
        }

        private bool CheckForClearedLines()
        {
            bool linescleared = false;
            int numlinescleared = 0;
            List<Point> gridLocations = new List<Point>(currentBlock.ReturnGridLocations());
            List<int> removeSpritesYLocation = new List<int>();
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            int[] lines = new int[4];
            // each block within the falling piece could be different y locations, thus we check if the rows at the y locations are filled.
            for (int i = 0; i < Constants.GAMEWIDTH; i++)
            {
                if (gridMap.ContainsKey(new Point(i, gridLocations[0].Y)))
                {
                    lines[0]++;
                }
                if (gridMap.ContainsKey(new Point(i, gridLocations[1].Y)))
                {
                    lines[1]++;
                }
                if (gridMap.ContainsKey(new Point(i, gridLocations[2].Y)))
                {
                    lines[2]++;
                }
                if (gridMap.ContainsKey(new Point(i, gridLocations[3].Y)))
                {
                    lines[3]++;
                }
            }
            // if the row is full, we need to clear it. set linescleared to true and add the ylocation to the sorteddictionary.
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                if (lines[i] == Constants.GAMEWIDTH)
                {
                    linescleared = true;
                    if (!dictionary.ContainsKey(gridLocations[i].Y))
                    {
                        dictionary.Add(gridLocations[i].Y, gridLocations[i].Y);
                        numlinescleared++;
                        removeSpritesYLocation.Add(gridLocations[i].Y);
                    }
                }
            }
            flashINFO = CreateFlashINFO(removeSpritesYLocation);
            hud.CalcLines(numlinescleared); // Move?
            return linescleared;
        }

        private void RemoveClearedLines()
        {
            // iterate over the dictionary starting at the bottom/top? of the playarea, remove the block from the line that was cleared, then move every block above it down 1.
            Dictionary<int, int> yList = new Dictionary<int, int>();
            foreach (var item in flashINFO.flashGrid.OrderBy(i => i.Key.Y))
            {
                if (!yList.ContainsKey(item.Key.Y))
                {
                    yList.Add(item.Key.Y, item.Key.Y);
                }
            }
            foreach (var item in yList.OrderBy(i => i.Key))
            {
                for (int x = Constants.GAMEWIDTH - 1; x >= 0; x--)
                {
                    int ypoint = item.Key;
                    gridMap.Remove(new Point(x, ypoint));
                    for (int k = ypoint - 1; k >= 0; k--)
                    {
                        Point oldPoint = new Point(x, k);
                        if (gridMap.ContainsKey(oldPoint))
                        {
                            Point newPoint = new Point(oldPoint.X, k + 1);
                            Dictionary<Point, Sprite> temp = new Dictionary<Point, Sprite>();
                            temp.Add(newPoint, gridMap[oldPoint]);
                            gridMap.Remove(oldPoint);
                            gridMap.Add(newPoint, temp[newPoint]);
                        }
                    }
                }
            }
        }

        private FlashINFO CreateFlashINFO(List<int> list)
        {
            Dictionary<Point, Sprite> temp = new Dictionary<Point, Sprite>();
            foreach (int ylocation in list)
            {
		        for (int i = 0; i < Constants.GAMEWIDTH; i++)
                {
                    temp.Add(new Point(i, ylocation), new Sprite(spriteTexture, 3 * Constants.TITLESIZE, Constants.TITLESIZE, Constants.TITLESIZE, Constants.TITLESIZE));
                }
            }
            return new FlashINFO(temp);
            
        }

        private enum GameStates
        {
            StartScreen,
            Playing,
            Flashing,
            Paused,
            GameOver
        }

        private struct FlashINFO
        {
            public int flashcount;
            public bool flashing;
            public Dictionary<Point,Sprite> flashGrid;

            public FlashINFO(Dictionary<Point, Sprite> list)
            {
                flashcount = 0;
                flashing = true;
                flashGrid = new Dictionary<Point, Sprite>(list);
            }
        }
    }
}
