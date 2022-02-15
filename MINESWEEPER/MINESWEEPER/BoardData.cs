using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ButtonDatas;


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
            for(int i = -1; i < 2; i++)
            {
                for(int j = -1; j < 2; j++)
                {
                    if (row > 0 && column > 0 && row < (y - 1) && column < (x - 1))
                    {
                        if (i == 0 && j == 0)
                        {
                            continue;
                        }
                    }
                    //좌측상단 클릭시
                    else if (row == 0 && column == 0)
                    {
                        if (i < 0 || j < 0) continue;
                    }
                    //우측하단 클릭시
                    else if (row == (y - 1) && column == (x - 1))
                    {
                        if (i > 0 || j > 0) continue;
                    }
                    //우측상단 클릭시
                    else if (row == 0 && column == (x - 1))
                    {
                        if (i < 0 || j > 0) continue;
                    }
                    //좌측하단 클릭시
                    else if (row == (y - 1) && column == 0)
                    {
                        if (i > 0 || j < 0) continue;
                    }
                    //상단면 클릭시
                    else if (row == 0 && column > 0)
                    {
                        if (i < 0) continue;
                    }
                    //하단면 클릭시
                    else if (row == (y - 1) && column > 0)
                    {
                        if (i > 0) continue;
                    }
                    //우측면 클릭시
                    else if (row > 0 && column == 0)
                    {
                        if (j < 0) continue;
                    }
                    //좌측면 클릭시
                    else if (row > 0 && column == (x - 1))
                    {
                        if (j > 0) continue;
                    }
                    else
                    {
                        continue;
                    }

                    mineCount = buttonDatas[row + i][column + j].GetMine() ? mineCount + 1 : mineCount;
                }
            }
            return mineCount;
        }

        public bool[,] CreateMineLocations(int x, int y, int mine)
        {
            bool[,] mineLocation = new bool[y, x];
            for(int i = 0; i < y; i++)
            {
                for(int j = 0; j < x; j++)
                {
                    mineLocation[i, j] = buttonDatas[i][j].GetMine();
                }
            }
            return mineLocation;
        }

        public int[,] CreateMineMap(bool[,] mineLocations)
        {
            int[,] mineMap = new int[mineLocations.GetLength(0), mineLocations.Length / mineLocations.GetLength(0)];
            for(int i = 0; i < y; i++)
            {
                for(int j = 0; j < x; j++)
                {
                    mineMap[i, j] = mineLocations[i,j] ? 0 : CheckRound_MineCount(i, j);
                }
            }
            return mineMap;
        }
    }
}
