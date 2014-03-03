using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisBetaWP
{
    class Block
    {
        private Sprite[] sprites;
        private BlockTypes blocktype;
        private Point[] gridLocationsOnMap; // x,y point for the grid
        private Point colorPointOnSheet; // x,y point for spritesheet
        public Point playarea { get; set; }
        private int blockstate;

        public Block(Texture2D texture, int source_x, int source_y, int width, int height, Random random, Point playarea)
        {
            sprites = new Sprite[4];
            gridLocationsOnMap = new Point[4];
            this.playarea = playarea;
            GetRandomBlock(random);
            //blocktype = BlockTypes.S; //temp for testing
            //colorPointOnSheet = new Point(Constants.TITLESIZE, Constants.TITLESIZE); //temp for testing
            blockstate = 0;
            for (int i = 0; i < sprites.Count(); i++)
            {
                sprites[i] = new Sprite(texture, colorPointOnSheet.X, colorPointOnSheet.Y, width, height);
            }
            SetupTheBlock(blocktype);
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < sprites.Count(); i++)
            {
                if (gridLocationsOnMap[i].Y >= 0) // check to not draw above playarea
                {
                    int x = (gridLocationsOnMap[i].X * Constants.TITLESIZE) + playarea.X;
                    int y = (gridLocationsOnMap[i].Y * Constants.TITLESIZE) + playarea.Y;
                    sprites[i].Draw(spriteBatch, x, y);
                }
            }
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    for (int i = 0; i < sprites.Count(); i++)
                    {
                        gridLocationsOnMap[i] = new Point(gridLocationsOnMap[i].X - 1, gridLocationsOnMap[i].Y);
                    }
                    break;
                case Direction.Right:
                    for (int i = 0; i < sprites.Count(); i++)
                    {
                        gridLocationsOnMap[i] = new Point(gridLocationsOnMap[i].X + 1, gridLocationsOnMap[i].Y);
                    }
                    break;
                case Direction.Down:
                    for (int i = 0; i < sprites.Count(); i++)
                    {
                        gridLocationsOnMap[i] = new Point(gridLocationsOnMap[i].X, gridLocationsOnMap[i].Y + 1);
                    }
                    break;
                default:
                    break;
            }
        }

        public List<Sprite> ReturnSprites()
        {
            List<Sprite> tempList = new List<Sprite>();
            foreach (Sprite sprite in sprites)
            {
                tempList.Add(sprite);
            }
            return tempList;
        }
        public List<Point> ReturnGridLocations()
        {
            List<Point> tempList = new List<Point>();
            foreach (Point location in gridLocationsOnMap)
            {
                tempList.Add(location);
            }
            return tempList;
        }
        public Dictionary<Point, Sprite> ReturnGridAndSprites()
        {
            Dictionary<Point, Sprite> tempD = new Dictionary<Point, Sprite>();
            for (int i = 0; i < sprites.Count(); i++)
            {
                tempD.Add(new Point(gridLocationsOnMap[i].X, gridLocationsOnMap[i].Y), sprites[i]);
            }
            return tempD;
        }

        public void Rotate(Dictionary<Point, Sprite> gridMap)
        {
            Point[] newGridLocation = new Point[4];
            bool[] canrotate = new bool[4];
            switch (blocktype)
            {
                case BlockTypes.O:
                    break;
                case BlockTypes.S:
                    for (int i = 0; i < gridLocationsOnMap.Length; i++)
                    {
                        int x = gridLocationsOnMap[i].X;
                        int y = gridLocationsOnMap[i].Y;
                        newGridLocation[i] = new Point(x, y);
                    }
                    if (blockstate == 0)
                    {
                        newGridLocation[2] = new Point(gridLocationsOnMap[0].X + 1, gridLocationsOnMap[0].Y + 1);
                        newGridLocation[3] = new Point(gridLocationsOnMap[0].X, gridLocationsOnMap[0].Y - 1);
                        if (CheckToRotate(newGridLocation, gridMap))
                        {
                            gridLocationsOnMap = newGridLocation;
                            blockstate = 1;
                        }
                    }
                    else
                    {
                        newGridLocation[2] = new Point(gridLocationsOnMap[0].X, gridLocationsOnMap[0].Y + 1);
                        newGridLocation[3] = new Point(gridLocationsOnMap[0].X - 1, gridLocationsOnMap[0].Y + 1);
                        if (CheckToRotate(newGridLocation, gridMap))
                        {
                            gridLocationsOnMap = newGridLocation;
                            blockstate = 0;
                        }
                    }
                    break;
                case BlockTypes.Z:
                    for (int i = 0; i < gridLocationsOnMap.Length; i++)
                    {
                        int x = gridLocationsOnMap[i].X;
                        int y = gridLocationsOnMap[i].Y;
                        newGridLocation[i] = new Point(x, y);
                    }
                    if (blockstate == 0)
                    {
                        newGridLocation[1] = new Point(gridLocationsOnMap[0].X + 1, gridLocationsOnMap[0].Y);
                        newGridLocation[3] = new Point(gridLocationsOnMap[0].X + 1, gridLocationsOnMap[0].Y - 1);
                        if (CheckToRotate(newGridLocation, gridMap))
                        {
                            gridLocationsOnMap = newGridLocation;
                            blockstate = 1;
                        }
                    }
                    else
                    {
                        newGridLocation[1] = new Point(gridLocationsOnMap[0].X + 1, gridLocationsOnMap[0].Y + 1);
                        newGridLocation[3] = new Point(gridLocationsOnMap[0].X - 1, gridLocationsOnMap[0].Y);
                        if (CheckToRotate(newGridLocation, gridMap))
                        {
                            gridLocationsOnMap = newGridLocation;
                            blockstate = 0;
                        }
                    }
                    break;
                case BlockTypes.T:
                    for (int i = 0; i < gridLocationsOnMap.Length; i++)
                    {
                        int x = gridLocationsOnMap[i].X;
                        int y = gridLocationsOnMap[i].Y;
                        newGridLocation[i] = new Point(x, y);
                    }
                    if (blockstate == 0)
                    {
                        newGridLocation[1] = new Point(gridLocationsOnMap[0].X + 1, gridLocationsOnMap[0].Y);
                        newGridLocation[2] = new Point(gridLocationsOnMap[0].X, gridLocationsOnMap[0].Y + 1);
                        newGridLocation[3] = new Point(gridLocationsOnMap[0].X, gridLocationsOnMap[0].Y - 1);
                        if (CheckToRotate(newGridLocation, gridMap))
                        {
                            gridLocationsOnMap = newGridLocation;
                            blockstate = 1;
                        }
                    }
                    else if (blockstate == 1)
                    {
                        newGridLocation[1] = new Point(gridLocationsOnMap[0].X + 1, gridLocationsOnMap[0].Y);
                        newGridLocation[2] = new Point(gridLocationsOnMap[0].X, gridLocationsOnMap[0].Y - 1);
                        newGridLocation[3] = new Point(gridLocationsOnMap[0].X - 1, gridLocationsOnMap[0].Y);
                        if (CheckToRotate(newGridLocation, gridMap))
                        {
                            gridLocationsOnMap = newGridLocation;
                            blockstate = 2;
                        }
                    }
                    else if (blockstate == 2)
                    {
                        newGridLocation[1] = new Point(gridLocationsOnMap[0].X, gridLocationsOnMap[0].Y - 1);
                        newGridLocation[2] = new Point(gridLocationsOnMap[0].X, gridLocationsOnMap[0].Y + 1);
                        newGridLocation[3] = new Point(gridLocationsOnMap[0].X - 1, gridLocationsOnMap[0].Y);
                        if (CheckToRotate(newGridLocation, gridMap))
                        {
                            gridLocationsOnMap = newGridLocation;
                            blockstate = 3;
                        }
                    }
                    else if (blockstate == 3)
                    {
                        newGridLocation[1] = new Point(gridLocationsOnMap[0].X + 1, gridLocationsOnMap[0].Y);
                        newGridLocation[2] = new Point(gridLocationsOnMap[0].X, gridLocationsOnMap[0].Y + 1);
                        newGridLocation[3] = new Point(gridLocationsOnMap[0].X - 1, gridLocationsOnMap[0].Y);
                        if (CheckToRotate(newGridLocation, gridMap))
                        {
                            gridLocationsOnMap = newGridLocation;
                            blockstate = 0;
                        }
                    }
                    break;
                case BlockTypes.L:
                    for (int i = 0; i < gridLocationsOnMap.Length; i++)
                    {
                        int x = gridLocationsOnMap[i].X;
                        int y = gridLocationsOnMap[i].Y;
                        newGridLocation[i] = new Point(x, y);
                    }
                    if (blockstate == 0)
                    {
                        newGridLocation[1] = new Point(gridLocationsOnMap[0].X, gridLocationsOnMap[0].Y - 1);
                        newGridLocation[2] = new Point(gridLocationsOnMap[0].X, gridLocationsOnMap[0].Y + 1);
                        newGridLocation[3] = new Point(gridLocationsOnMap[0].X + 1, gridLocationsOnMap[0].Y + 1);
                        if (CheckToRotate(newGridLocation, gridMap))
                        {
                            gridLocationsOnMap = newGridLocation;
                            blockstate = 1;
                        }
                    }
                    else if (blockstate == 1)
                    {
                        newGridLocation[1] = new Point(gridLocationsOnMap[0].X + 1, gridLocationsOnMap[0].Y);
                        newGridLocation[2] = new Point(gridLocationsOnMap[0].X - 1, gridLocationsOnMap[0].Y);
                        newGridLocation[3] = new Point(gridLocationsOnMap[0].X + 1, gridLocationsOnMap[0].Y - 1);
                        if (CheckToRotate(newGridLocation, gridMap))
                        {
                            gridLocationsOnMap = newGridLocation;
                            blockstate = 2;
                        }
                    }
                    else if (blockstate == 2)
                    {
                        newGridLocation[1] = new Point(gridLocationsOnMap[0].X, gridLocationsOnMap[0].Y - 1);
                        newGridLocation[2] = new Point(gridLocationsOnMap[0].X - 1, gridLocationsOnMap[0].Y - 1);
                        newGridLocation[3] = new Point(gridLocationsOnMap[0].X, gridLocationsOnMap[0].Y + 1);
                        if (CheckToRotate(newGridLocation, gridMap))
                        {
                            gridLocationsOnMap = newGridLocation;
                            blockstate = 3;
                        }
                    }
                    else if (blockstate == 3)
                    {
                        newGridLocation[1] = new Point(gridLocationsOnMap[0].X + 1, gridLocationsOnMap[0].Y);
                        newGridLocation[2] = new Point(gridLocationsOnMap[0].X - 1, gridLocationsOnMap[0].Y);
                        newGridLocation[3] = new Point(gridLocationsOnMap[0].X - 1, gridLocationsOnMap[0].Y + 1);
                        if (CheckToRotate(newGridLocation, gridMap))
                        {
                            gridLocationsOnMap = newGridLocation;
                            blockstate = 0;
                        }
                    }
                    break;
                case BlockTypes.I:
                    for (int i = 0; i < gridLocationsOnMap.Length; i++)
                    {
                        int x = gridLocationsOnMap[i].X;
                        int y = gridLocationsOnMap[i].Y;
                        newGridLocation[i] = new Point(x, y);
                    }
                    if (blockstate == 0)
                    {
                        newGridLocation[1] = new Point(gridLocationsOnMap[0].X, gridLocationsOnMap[0].Y + 2);
                        newGridLocation[2] = new Point(gridLocationsOnMap[0].X, gridLocationsOnMap[0].Y + 1);
                        newGridLocation[3] = new Point(gridLocationsOnMap[0].X, gridLocationsOnMap[0].Y - 1);
                        if (CheckToRotate(newGridLocation, gridMap))
                        {
                            gridLocationsOnMap = newGridLocation;
                            blockstate = 1;
                        }
                    }
                    else
                    {
                        newGridLocation[1] = new Point(gridLocationsOnMap[0].X + 1, gridLocationsOnMap[0].Y);
                        newGridLocation[2] = new Point(gridLocationsOnMap[0].X - 1, gridLocationsOnMap[0].Y);
                        newGridLocation[3] = new Point(gridLocationsOnMap[0].X - 2, gridLocationsOnMap[0].Y);
                        if (CheckToRotate(newGridLocation, gridMap))
                        {
                            gridLocationsOnMap = newGridLocation;
                            blockstate = 0;
                        }
                    }
                    break;
                case BlockTypes.J:
                    for (int i = 0; i < gridLocationsOnMap.Length; i++)
                    {
                        int x = gridLocationsOnMap[i].X;
                        int y = gridLocationsOnMap[i].Y;
                        newGridLocation[i] = new Point(x, y);
                    }
                    if (blockstate == 0)
                    {
                        newGridLocation[1] = new Point(gridLocationsOnMap[0].X, gridLocationsOnMap[0].Y - 1);
                        newGridLocation[2] = new Point(gridLocationsOnMap[0].X, gridLocationsOnMap[0].Y + 1);
                        newGridLocation[3] = new Point(gridLocationsOnMap[0].X + 1, gridLocationsOnMap[0].Y - 1);
                        if (CheckToRotate(newGridLocation, gridMap))
                        {
                            gridLocationsOnMap = newGridLocation;
                            blockstate = 1;
                        }
                    }
                    else if (blockstate == 1)
                    {
                        newGridLocation[1] = new Point(gridLocationsOnMap[0].X + 1, gridLocationsOnMap[0].Y);
                        newGridLocation[2] = new Point(gridLocationsOnMap[0].X - 1, gridLocationsOnMap[0].Y);
                        newGridLocation[3] = new Point(gridLocationsOnMap[0].X - 1, gridLocationsOnMap[0].Y - 1);
                        if (CheckToRotate(newGridLocation, gridMap))
                        {
                            gridLocationsOnMap = newGridLocation;
                            blockstate = 2;
                        }
                    }
                    else if (blockstate == 2)
                    {
                        newGridLocation[1] = new Point(gridLocationsOnMap[0].X, gridLocationsOnMap[0].Y + 1);
                        newGridLocation[2] = new Point(gridLocationsOnMap[0].X - 1, gridLocationsOnMap[0].Y + 1);
                        newGridLocation[3] = new Point(gridLocationsOnMap[0].X, gridLocationsOnMap[0].Y - 1);
                        if (CheckToRotate(newGridLocation, gridMap))
                        {
                            gridLocationsOnMap = newGridLocation;
                            blockstate = 3;
                        }
                    }
                    else if (blockstate == 3)
                    {
                        newGridLocation[1] = new Point(gridLocationsOnMap[0].X + 1, gridLocationsOnMap[0].Y);
                        newGridLocation[2] = new Point(gridLocationsOnMap[0].X + 1, gridLocationsOnMap[0].Y + 1);
                        newGridLocation[3] = new Point(gridLocationsOnMap[0].X - 1, gridLocationsOnMap[0].Y);
                        if (CheckToRotate(newGridLocation, gridMap))
                        {
                            gridLocationsOnMap = newGridLocation;
                            blockstate = 0;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private bool CheckToRotate(Point[] gridLocations, Dictionary<Point, Sprite> gridMap)
        {
            bool[] canrotate = new bool[4];
            for (int i = 0; i < gridLocations.Count(); i++)
            {
                if (gridLocations[i].X >= 0 && gridLocations[i].X < Constants.GAMEWIDTH && gridLocations[i].Y < Constants.GAMEHEIGHT)
                {
                    if (!gridMap.ContainsKey(gridLocations[i]))
                    {
                        canrotate[i] = true;
                    }
                }
            }
            if (canrotate[0] && canrotate[1] && canrotate[2] && canrotate[3])
            {
                return true;
            }
            return false;
        }

        private void GetRandomBlock(Random random)
        {
            int randomColor = random.Next(7);
            switch (randomColor)
            {
                case 0:
                    blocktype = BlockTypes.I;
                    colorPointOnSheet = new Point(Constants.TITLESIZE, Constants.TITLESIZE);
                    break;
                case 1:
                    blocktype = BlockTypes.J;
                    colorPointOnSheet = new Point(Constants.TITLESIZE * 3, 0);
                    break;
                case 2:
                    blocktype = BlockTypes.L;
                    colorPointOnSheet = new Point(0, 0);
                    break;
                case 3:
                    blocktype = BlockTypes.S;
                    colorPointOnSheet = new Point(0, Constants.TITLESIZE);
                    break;
                case 4:
                    blocktype = BlockTypes.Z;
                    colorPointOnSheet = new Point(Constants.TITLESIZE * 2, 0);
                    break;
                case 5:
                    blocktype = BlockTypes.O;
                    colorPointOnSheet = new Point(Constants.TITLESIZE, 0);
                    break;
                case 6:
                    blocktype = BlockTypes.T;
                    colorPointOnSheet = new Point(Constants.TITLESIZE * 2, Constants.TITLESIZE);
                    break;
                default:
                    blocktype = BlockTypes.I;
                    colorPointOnSheet = new Point(Constants.TITLESIZE, Constants.TITLESIZE);
                    break;
            }
        }

        private void SetupTheBlock(BlockTypes blocktype)
        {
            switch (blocktype)
            {
                case BlockTypes.O:
                    gridLocationsOnMap[0] = new Point(5, 0);

                    gridLocationsOnMap[1] = new Point(5, 1);

                    gridLocationsOnMap[2] = new Point(4, 1);

                    gridLocationsOnMap[3] = new Point(4, 0);

                    break;
                case BlockTypes.S:
                    gridLocationsOnMap[0] = new Point(5, 0);

                    gridLocationsOnMap[1] = new Point(6, 0);

                    gridLocationsOnMap[2] = new Point(5, 1);

                    gridLocationsOnMap[3] = new Point(4, 1);
                    break;
                case BlockTypes.Z:
                    gridLocationsOnMap[0] = new Point(5, 0);

                    gridLocationsOnMap[1] = new Point(6, 1);

                    gridLocationsOnMap[2] = new Point(5, 1);

                    gridLocationsOnMap[3] = new Point(4, 0);
                    break;
                case BlockTypes.T:
                    gridLocationsOnMap[0] = new Point(5, 0);

                    gridLocationsOnMap[1] = new Point(6, 0);

                    gridLocationsOnMap[2] = new Point(5, 1);

                    gridLocationsOnMap[3] = new Point(4, 0);
                    break;
                case BlockTypes.L:
                    gridLocationsOnMap[0] = new Point(5, 0);

                    gridLocationsOnMap[1] = new Point(6, 0);

                    gridLocationsOnMap[2] = new Point(4, 0);

                    gridLocationsOnMap[3] = new Point(4, 1);
                    break;
                case BlockTypes.I:
                    gridLocationsOnMap[0] = new Point(5, 0);

                    gridLocationsOnMap[1] = new Point(6, 0);

                    gridLocationsOnMap[2] = new Point(4, 0);

                    gridLocationsOnMap[3] = new Point(3, 0);
                    break;
                case BlockTypes.J:
                    gridLocationsOnMap[0] = new Point(5, 0);

                    gridLocationsOnMap[1] = new Point(6, 0);

                    gridLocationsOnMap[2] = new Point(6, 1);

                    gridLocationsOnMap[3] = new Point(4, 0);
                    break;
                default:
                    break;
            }
        }

        private enum BlockTypes
        {
            O,
            S,
            Z,
            T,
            L,
            I,
            J
        }
    }
}
