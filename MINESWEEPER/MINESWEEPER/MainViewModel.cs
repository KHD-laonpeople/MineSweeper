using MINESWEEPER.Data;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Reactive.Linq;
using MineKit;

namespace MINESWEEPER
{
    public class MainViewModel: ReactiveObject
    {
        public ObservableCollection<MineData> Mines { get; set; }
        private IObservable<bool> canExecuteEditMode;
        public MainViewModel()
        {
            Mines = new ObservableCollection<MineData>();

            Subscribes();
            Commands();
            Events();
            
        }
        void Subscribes()
        {
            this.canExecuteEditMode = this.WhenAnyValue(x => x.EditMode);
        }
        void Events()
        {

        }
        void SetEditMode(bool editMode)
        {
            EditMode = editMode;
            StartStopString = editMode ? ToggleStatus.Start.ToString() : ToggleStatus.Stop.ToString();
        }
        void Commands()
        {
            this.StartStopCommand = ReactiveCommand.Create(() =>
            {
                if (EditMode)
                    CreateMineMap();

                SetEditMode(!EditMode);
            });
            this.OnMineLeftDoubleCommand = ReactiveCommand.Create<MineData>(e =>
            {
                //open not mine?               
                var x = e.X;
                var y = e.Y;
                List<MineData> nearMines = new List<MineData>();

                //for (int x1 = (x - 1 < 0 ? 0 : x - 1); x1 <= (width - 1 <= x + 1 ? width - 1 : x + 1); x1++)
                //{
                //    for (int y1 = (y - 1 < 0 ? 0 : y - 1); y1 <= (height - 1 <= y + 1 ? height - 1 : y + 1); y1++)
                //    {
                //        var mine = Mines.Where(g => g.X == x1 && g.Y == y1).FirstOrDefault();
                //        nearMines.Add(mine);
                //    }
                //}

                //Action<int, int> act = (a,b)=>{
                //    var mine = Mines.Where(g => g.X == a && g.Y == b).FirstOrDefault();
                //    nearMines.Add(mine);
                //};
                MineManager.DoWorkNearMine(x, y, width, height, (a, b) => {
                    var mine = Mines.Where(g => g.X == a && g.Y == b).FirstOrDefault();
                    nearMines.Add(mine);
                });

                //near mine count == 0 ? open other tile
                if (e.NearMineCount == 0)
                {
                    foreach (var item in nearMines)
                        item.Open();
                }

                //near mine count == isChecked count ? open other tile
                if (nearMines.Count(g => g.IsChecked) == e.NearMineCount)
                {
                    foreach (var item in nearMines.Where(g => !g.IsChecked))
                        item.Open();

                }
                CheckMineCrash();
            }, canExecuteEditMode.Select(x => !x));
            this.OnMineLeftCommand = ReactiveCommand.Create<MineData>(e =>
            {
                //avoid first mine
                var mine = e;                
                if (Mines.All(g => !g.IsOpend) && mine.IsMine)
                {                     
                    while (true)
                    {                
                        var mines = MineManager.CreateMineLocations(width, height, mineCount);
                        if (!mines[e.X, e.Y])
                        {
                            CreateMineMap(mines);
                            mine = Mines.Where(g => g.X == e.X && g.Y == e.Y).FirstOrDefault();
                            break;
                        }
                    }
                }
                //opened     
                mine.Open();
                mine.Uncheck();
                CheckMineCrash();
            }, canExecuteEditMode.Select(x => !x));
            this.OnMineRightCommand = ReactiveCommand.Create<MineData>(e =>
            {
                //checked
                e.ToggleCheck();
                CheckMineCrash();
            }, canExecuteEditMode.Select(x => !x));
        }

        void CheckMineCrash()
        {
            if (Mines.Any(g => g.IsCrashed()))
            {
                MessageBox.Show("Booooom");
                SetEditMode(true);
            }
            if (Mines.All(g => g.IsChecked || g.IsOpend))
            {
                MessageBox.Show("Goooood");
                SetEditMode(true);
            }
        }
        void CreateMineMap()
        {
            //create mine
            var mines = MineManager.CreateMineLocations(Width, Height, MineCount);
            CreateMineMap(mines);
        }
        void CreateMineMap(bool[,] mines)
        {
            //create min map
            var mineMap = MineManager.CreateMineMap(mines);
            //binding mine data
            Mines.Clear();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Mines.Add(new MineData()
                    {
                        X = x,
                        Y = y,
                        IsMine = mines[x, y],
                        NearMineCount = mineMap[x, y],
                    });
                }
            }
        }

        //Properties
        private int width = 5;
        public int Width
        {
            get { return this.width; }
            set { this.RaiseAndSetIfChanged(ref this.width, value); }
        }
        private int height = 5;
        public int Height
        {
            get { return this.height; }
            set { this.RaiseAndSetIfChanged(ref this.height, value); }
        }
        private int mineCount = 5;
        public int MineCount
        {
            get { return this.mineCount; }
            set { this.RaiseAndSetIfChanged(ref this.mineCount, value); }
        }
        private bool editMode = true;
        public bool EditMode
        {
            get { return this.editMode; }
            set { this.RaiseAndSetIfChanged(ref this.editMode, value);}
        }

        private string startStopString = ToggleStatus.Start.ToString();
        public string StartStopString
        {
            get { return this.startStopString; }
            set { this.RaiseAndSetIfChanged(ref this.startStopString, value); }
        }

        //Commamds
        public ReactiveCommand<Unit, Unit> StartStopCommand { get; set; }
        public ReactiveCommand<MineData, Unit> OnMineLeftDoubleCommand { get; set; }
        public ReactiveCommand<MineData, Unit> OnMineLeftCommand { get; set; }
        public ReactiveCommand<MineData, Unit> OnMineRightCommand { get; set; }
    }
    public enum ToggleStatus { Start=0, Stop }
}
