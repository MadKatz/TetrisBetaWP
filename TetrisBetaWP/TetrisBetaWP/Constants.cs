using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TetrisBetaWP
{
    static class Constants
    {
        public const int TITLESIZE = 20;
        public const int GAMEWIDTH = 10;
        public const int GAMEHEIGHT = 20;
        public const int PLAYAREAOFFSET = (TITLESIZE * 2);
        public const int GAMESIZEWIDTH = GAMEWIDTH * TITLESIZE;
        public const int GAMESIZEHEIGHT = GAMEHEIGHT * TITLESIZE;
        public const int SCOREPERLINE = 100;
        public const int BASESCOREPERLEVEL = 400;
        public const int STARTINGFPS = 600; // in milliseconds: 1000ms = 1 second
        public const int MINFPS = 30; // in milliseconds: 1000ms = 1 second
        public const int FPSREDUCTIONPERLEVEL = 42; // in milliseconds: 1000ms = 1 second
        public const int FLASHCOUNT = 4;
        public const int FLASHFPS = 100;


        //public const int PLAYAREA_MAXY = GAMESIZEHEIGHT + BUFFER;
        //public const int PLAYAREA_MINY = BUFFER;
        //public const int PLAYAREA_MAXX = (GAMESIZEWIDTH + (TITLESIZE * 4)) + GAMESIZEWIDTH;
        //public const int PLAYAREA_MINX = (GAMESIZEWIDTH + (TITLESIZE * 4));
        //public const int PLAYERSTARTINGX = PLAYAREA_MINX + (TITLESIZE * 5); // starting in the 6th Cell of the GridMap (0-9) width
        //public const int PLAYERSTARTINGY = PLAYAREA_MINY; //starting in the 0th cell of the GridMap (0-19) height
    }

    public enum Direction
    {
        Left,
        Right,
        Down
    }
}
