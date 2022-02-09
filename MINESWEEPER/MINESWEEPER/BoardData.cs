using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MINESWEEPER
{
    class BoardData
    {
        private int x;
        private int y;
        private int mine;
        //다중 List로 변경, 행렬 이해에 편리
        public List<ButtonData> buttonDatas = new List<ButtonData>();

        public BoardData(int x, int y, int mine)
        {
            this.x = x;
            this.y = y;
            this.mine = mine;

            for (int i = 0; i < x * y; i++)
            {
                buttonDatas.Add(new ButtonData(i));
            }
        }
        public void gameBoardCreate()
        {

            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    buttonDatas[i * x + j].SetdataName(i * x + j);
                    buttonDatas[j + i * x].SetMine(((j + i * x) < mine) ? true : false);
                }
            }
            MineSufle();
        }
        private void MineSufle()
        {
            Random random = new Random();
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y - 1; j++)
                {
                    int rand_1 = random.Next(j + 1, y);

                    bool temp = buttonDatas[j *  x + i].GetMine();
                    buttonDatas[j * x + i].SetMine(buttonDatas[rand_1 * x + i].GetMine());
                    buttonDatas[rand_1 * x + i].SetMine(temp);
                }
            }
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x - 1; j++)
                {
                    int rand_1 = random.Next(j + 1, y);

                    bool temp = buttonDatas[j * x + i].GetMine();
                    buttonDatas[j * x + i].SetMine(buttonDatas[rand_1 * x + i].GetMine());
                    buttonDatas[rand_1 * x + i].SetMine(temp);
                }
            }
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    if (!buttonDatas[i * x + j].GetMine())
                    {
                        buttonDatas[i * x + j].SetRountMineCount(CheckRound_MineCount(i, j));
                    }
                }
            }
            return;
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

                    mineCount = buttonDatas[(row + i) * x + column + j].GetMine() ? mineCount + 1 : mineCount;
                }
            }
            return mineCount;
        }
    }
}
