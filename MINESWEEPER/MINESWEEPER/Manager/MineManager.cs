using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINESWEEPER.Manager
{
    public static class MineManager
    {
        public static bool[,] CreateMineLocations(int x, int y, int mine)
        {
            bool[,] mines = new bool[y, x];
            if (x > 0 && y > 0 && mine > 0 && mine < x*y)
            {
                bool[] tmp = new bool[mines.Length];
                Random rnd = new Random();
                do
                {
                    tmp[rnd.Next(0, tmp.Length)] = true;
                } while (tmp.Count(g => g) < mine);
                Buffer.BlockCopy(tmp, 0, mines, 0, tmp.Length);
            }
            return mines;
        }
        public static int[,] CreateMineMap(bool[,] mineLocations)
        {
            var width = mineLocations.GetLength(0);
            var height = mineLocations.GetLength(1);
            int[,] mineMap = new int[width,height];

            for (int x=0;x<width;x++)
            {
                for(int y=0;y<height;y++)
                {
                    var mineCount = 0;
                    // -1 <= x <= 1 & -1 <= y <= 1
                    
                    for(int x1 = (x-1 < 0 ? 0 : x-1); x1 <= (width-1  <= x + 1 ? width-1 : x + 1) ; x1++)
                    {
                        for (int y1 = (y - 1 < 0 ? 0 : y - 1); y1 <= (height-1 <= y +1 ? height-1 : y + 1); y1++)
                        {
                            mineCount += mineLocations[x1, y1] ? 1 : 0;
                        }
                    }
                    mineMap[x, y] = mineCount;
                }
            }

            return mineMap;
        }
    }
}
