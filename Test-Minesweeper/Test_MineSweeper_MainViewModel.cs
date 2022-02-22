using MINESWEEPER;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test_Minesweeper
{
    public class Test_MineSweeper_MainViewModel
    {

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(0, 0, 1, 0)]
        [InlineData(0, 0, 10, 0)]
        [InlineData(10, 10, 10, 100)]
        public void CreateMineMap(int width, int height, int mineCount, int assertMineCount)
        {
            //Arrange
            var vm = new MainViewModel();

            //Action
            vm.Width = width;
            vm.Height = height;
            vm.MineCount = mineCount;
            vm.StartStopCommand.Execute().Subscribe();            

            //Assert
            
            Assert.True(vm.Mines.Count() == assertMineCount);
        }
    }
}
