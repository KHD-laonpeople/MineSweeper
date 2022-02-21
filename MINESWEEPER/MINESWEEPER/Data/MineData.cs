using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MINESWEEPER.Data
{
    public class MineData: ReactiveObject,IMineData
    {        
        public bool IsCrashed()
        {
            return isOpend && IsMine;
        }
        public void Open()
        {
            IsOpend = true;
        }
        public void Check()
        {
            IsChecked = true;
        }
        public void Uncheck()
        {
            IsChecked = false;
        }
        public void ToggleCheck()
        {
            IsChecked = !IsChecked;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsMine { get; set; }
        public int NearMineCount { get; set; }
        bool isChecked = false;
        public bool IsChecked 
        {
            get { return this.isChecked; }
            set {
                this.RaiseAndSetIfChanged(ref this.isChecked, value);                
                this.RaisePropertyChanged(nameof(DisplayValue));
            }

        }
        bool isOpend = false;
        public bool IsOpend
        {
            get { return this.isOpend; }
            set
            {
                this.RaiseAndSetIfChanged(ref this.isOpend, value);
                this.RaisePropertyChanged(nameof(Background));
                this.RaisePropertyChanged(nameof(DisplayValue));
            }
        }
        public string Background
        {
            get
            {
                return IsOpend ? "Gray" : "LightGray";
            }
        }
        public string DisplayValue
        {
            get
            {

                if (!isOpend)
                {
                    if (IsChecked)
                        return "!";
                    else
                        return string.Empty;
                }
                else
                {
                    
                    if (IsMine)
                        return "*";
                    else
                        return NearMineCount > 0 ? NearMineCount.ToString() : string.Empty;
                    
                }
            }
        }

    }
}
