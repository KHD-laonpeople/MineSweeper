using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINESWEEPER
{
    class ButtonData
    {
        private bool mine = false;
        private bool flag = false;
        private int dataName = 0;
        private int roundMineCount = 0;

        public ButtonData()
        {
        }
        public ButtonData(int i)
        {
            mine = false;
            flag = false;
            dataName = i;
            roundMineCount = 0;
        }
        public void SetMine(bool mine)
        {
            this.mine = mine;
        }
        public void SetFlag(bool flag)
        {
            this.flag = flag;
        }
        public void SetdataName(int dataName)
        {
            this.dataName = dataName;
        }
        public void SetRountMineCount(int roundMineCount)
        {
            this.roundMineCount = roundMineCount;
        }
        public bool GetMine()
        {
            return mine;
        }
        public bool GetFlag()
        {
            return flag;
        }
        public int GetdataName()
        {
            return dataName;
        }
        public int GetRountMineCount()
        {
            return roundMineCount;
        }

    }
}
