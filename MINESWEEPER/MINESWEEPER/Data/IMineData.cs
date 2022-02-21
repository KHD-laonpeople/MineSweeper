using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINESWEEPER.Data
{
    public interface IMineData
    {
        bool IsCrashed();
        void Open();
        void Check();
        void Uncheck();
        void ToggleCheck();
        int X { get; set; }
        int Y { get; set; }
        bool IsMine { get; set; }
        int NearMineCount { get; set; }        
        bool IsChecked { get; set; }
        bool IsOpend { get; set; }
        string Background { get;  }
        string DisplayValue { get;  }
    }
}
