using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisBetaWP
{
    class HUD
    {
        private Vector2 scorePos;
        private Vector2 levelPos;
        private Vector2 startingPoint;
        public Texture2D NextBoxTexture { get; set; }
        public Texture2D LevelBoxTexture { get; set; }
        public Texture2D ScoreBoxTexture { get; set; }
        public Texture2D DownControlTexture { get; set; }
        public Texture2D UpControlTexture { get; set; }
        public Texture2D LeftControlTexture { get; set; }
        public Texture2D RightControlTexture { get; set; }
        public Texture2D PausedBoxTexture { get; set; }
        public SpriteFont Font { get; set; }
        public Point PausedBoxPoint { get; set; }
        public Point DownControlPoint { get; set; }
        public Point UpControlPoint { get; set; }
        public Point LeftControlPoint { get; set; }
        public Point RightControlPoint { get; set; }
        public int Score { get; set; }
        public int Level { get; set; }

        public HUD(Vector2 startingVector)
        {
            // Using playAreaPos location
            startingPoint = startingVector;
            scorePos.X = startingVector.X + Constants.GAMESIZEWIDTH + Constants.PLAYAREAOFFSET;
            scorePos.Y = startingVector.Y + Constants.GAMESIZEHEIGHT;
            levelPos.X = scorePos.X;
            levelPos.Y = startingVector.Y + Constants.GAMESIZEHEIGHT - (Constants.TITLESIZE * 6);
            Score = 0;
            Level = 1;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 scoreHeight = Font.MeasureString(Score.ToString());
            Vector2 levelHeight = Font.MeasureString(Level.ToString());
            //spriteBatch.DrawString(Font, "Score:", scorePos, Color.White);
            spriteBatch.Draw(NextBoxTexture, new Rectangle((int)scorePos.X, (int)startingPoint.Y, NextBoxTexture.Width, NextBoxTexture.Height), Color.White);
            spriteBatch.Draw(ScoreBoxTexture, new Rectangle((int)scorePos.X, (int)scorePos.Y - ScoreBoxTexture.Height, ScoreBoxTexture.Width, ScoreBoxTexture.Height), Color.White);
            spriteBatch.DrawString(Font, Score.ToString(), new Vector2(scorePos.X + 50, scorePos.Y - (ScoreBoxTexture.Height / 2) + scoreHeight.Y), Color.Black);
            //spriteBatch.DrawString(Font, "Level:", levelPos, Color.White);
            spriteBatch.Draw(LevelBoxTexture, new Rectangle((int)levelPos.X, (int)levelPos.Y - LevelBoxTexture.Height, LevelBoxTexture.Width, LevelBoxTexture.Height), Color.White);
            spriteBatch.DrawString(Font, Level.ToString(), new Vector2(levelPos.X + 50, levelPos.Y - (LevelBoxTexture.Height / 2) + levelHeight.Y), Color.Black);
            spriteBatch.Draw(PausedBoxTexture, new Rectangle(PausedBoxPoint.X, PausedBoxPoint.Y, PausedBoxTexture.Width, PausedBoxTexture.Height), Color.White);
            spriteBatch.Draw(LeftControlTexture, new Rectangle(LeftControlPoint.X, LeftControlPoint.Y, LeftControlTexture.Width, LeftControlTexture.Height), Color.White);
            spriteBatch.Draw(RightControlTexture, new Rectangle(RightControlPoint.X, RightControlPoint.Y, RightControlTexture.Width, RightControlTexture.Height), Color.White);
            spriteBatch.Draw(DownControlTexture, new Rectangle(DownControlPoint.X, DownControlPoint.Y, DownControlTexture.Width, DownControlTexture.Height), Color.White);
            spriteBatch.Draw(UpControlTexture, new Rectangle(UpControlPoint.X, UpControlPoint.Y, UpControlTexture.Width, UpControlTexture.Height), Color.White);
        }

        public void Restart()
        {
            Score = 0;
            Level = 1;
        }

        public void CalcLines(int numlinescleared)
        {
            if (numlinescleared == 4)
            {
                Score += numlinescleared * Constants.SCOREPERLINE;
                Score += 250;
            }
            else
            {
                Score += numlinescleared * Constants.SCOREPERLINE;
            }
            if (Score > Level * Constants.BASESCOREPERLEVEL)
            {
                Level++;
            }
        }

        public void LoadControlPoints()
        {
            PausedBoxPoint = new Point((int)startingPoint.X + (Constants.GAMESIZEWIDTH * 2) - 45, (int)startingPoint.Y - 135);
            DownControlPoint = new Point((int)startingPoint.X + RightControlTexture.Width + LeftControlTexture.Width + (50 * 2), (int)startingPoint.Y + Constants.GAMESIZEHEIGHT + 75);
            UpControlPoint = new Point((int)startingPoint.X + RightControlTexture.Width + LeftControlTexture.Width + DownControlTexture.Width + (50 * 3), (int)startingPoint.Y + Constants.GAMESIZEHEIGHT + 75);
            LeftControlPoint = new Point((int)startingPoint.X, (int)startingPoint.Y + Constants.GAMESIZEHEIGHT + 75);
            RightControlPoint = new Point((int)startingPoint.X + RightControlTexture.Width + 50, (int)startingPoint.Y + Constants.GAMESIZEHEIGHT + 75);
        }
    }
}
