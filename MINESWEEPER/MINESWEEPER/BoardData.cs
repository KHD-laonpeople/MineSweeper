using ButtonDatas;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BoardDatas
{
    class BoardData : ButtonData
    {
        private int x;
        private int y;
        private int mine;

        public List<List<ButtonData>> buttonDatas = new List<List<ButtonData>>();

        public BoardData()
        { }
        public BoardData(int x, int y, int mine)
        {
            this.x = x;
            this.y = y;
            this.mine = mine;
        }
        public void GameBoardCreate(int x, int y, int mine)
        {
            this.x = x;
            this.y = y;
            this.mine = mine;

            buttonDatas.Clear();

            for (int i = 0; i < y; i++)
            {
                List<ButtonData> button = new List<ButtonData>();
                for (int j = 0; j < x; j++)
                {
                    button.Add(new ButtonData());
                    button[j].SetdataName(j + i * x);
                    button[j].SetMine((j + i * x) < mine);
                }
                buttonDatas.Add(button);
            }
            Random random = new Random();
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y - 1; j++)
                {
                    int rand_1 = random.Next(j + 1, y);

                    bool temp = buttonDatas[j][i].GetMine();
                    buttonDatas[j][i].SetMine(buttonDatas[rand_1][i].GetMine());
                    buttonDatas[rand_1][i].SetMine(temp);
                }
            }
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x - 1; j++)
                {
                    int rand_1 = random.Next(j + 1, x);

                    bool temp = buttonDatas[i][j].GetMine();
                    buttonDatas[i][j].SetMine(buttonDatas[i][rand_1].GetMine());
                    buttonDatas[i][rand_1].SetMine(temp);
                }
            }
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    if (!buttonDatas[i][j].GetMine())
                    {
                        buttonDatas[i][j].SetRoundMineCount(CheckRound_MineCount(i, j));
                    }
                }
            }
        }

        private int CheckRound_MineCount(int row, int column)
        {
            int mineCount = 0;
            for (int i = 0; i < 9; i++)
            {
                int roundRow = row + (i / 3) - 1;
                int roundColumn = column + (i % 3) - 1;
                try
                {
                    if (i == 4) continue;
                    mineCount = buttonDatas[roundRow][roundColumn].GetMine() ? mineCount + 1 : mineCount;
                }
                catch
                {
                    continue;
                }
            }
            return mineCount;
        }

        public bool[,] CreateMineLocations(int x, int y, int mine)
        {
            int count = 0;
            if (x < 0 || y < 0 || mine < 0)
            {
                bool[,] error = new bool[0, 0];
                return error;
            }
            bool[,] mineLocation = new bool[y, x];

            bool[] tmp = new bool[mineLocation.Length];
            Random rnd = new Random();
            do
            {
                tmp[rnd.Next(0, tmp.Length)] = true;
                count++;
            } while (tmp.Count(g => g) < mine);
            Buffer.BlockCopy(tmp, 0, mineLocation, 0, tmp.Length);

            int test = count;
            return mineLocation;
        }

        public int[,] CreateMineMap(bool[,] mineLocations)
        {
            int row = mineLocations.Length / mineLocations.GetLength(0);
            int column =  mineLocations.GetLength(0);
            int[,] mineMap = new int[column, row];

            int count = 0;
            while (count != mineMap.Length)
            {
                int mineCount = 0;
                if (!mineLocations[count / row, count % row])
                {
                    for (int i = 0; i < 9; i++)
                    {
                        int roundRow = (i / 3) - 1;
                        int roundColumn = (i % 3) - 1;
                        try
                        {
                            if (i == 4) continue;
                            mineCount = mineLocations[roundRow, roundColumn] ? mineCount + 1 : mineCount;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                mineMap[count / row, count % row] = mineCount;
                count++;
            }
            return mineMap;
        }
    }
}
