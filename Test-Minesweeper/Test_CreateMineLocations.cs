using MineKit;
using System;
using Xunit;

namespace Test_Minesweeper
{    
    public class Test_CreateMineLocations
    {
        int GetMineCount(bool[,] mines)
        {
            int result = 0;
            foreach (var item in mines)
                result = result + (item ? 1 : 0);
            return result;
        }
        [Fact(DisplayName = "1. BoardAndMineZero")]
        public void BoardAndMineZero()
        {
            //Arrange
            int resultMineCount = 0;
            int testMineCount = 0;

            //Action
            var mines = MineManager.CreateMineLocations(0, 0, testMineCount);
            resultMineCount = GetMineCount(mines);

            //Assert
            Assert.True(resultMineCount == testMineCount);
        }
        [Fact]
        public void Board0x0Mine1()
        {
            //Arrange
            int resultMineCount = 0;
            int testMineCount = 1;

            //Action
            var mines = MineManager.CreateMineLocations(0, 0, testMineCount);
            resultMineCount = GetMineCount(mines);

            //Assert
            Assert.True(resultMineCount == 0);
        }
        [Fact]
        public void Board0x0Mine10()
        {
            //Arrange
            int resultMineCount = 0;
            int testMineCount = 10;

            //Action
            var mines = MineManager.CreateMineLocations(0, 0, testMineCount);
            resultMineCount = GetMineCount(mines);

            //Assert
            Assert.True(resultMineCount == 0);
        }
        [Fact]
        public void Board0x0Mine1000000()
        {
            //Arrange
            int resultMineCount = 0;
            int testMineCount = 100000;

            //Action
            var mines = MineManager.CreateMineLocations(0, 0, testMineCount);
            resultMineCount = GetMineCount(mines);

            //Assert
            Assert.True(resultMineCount == 0);
        }
        [Fact]
        public void Board1x1MineZero()
        {
            //Arrange
            int resultMineCount = 0;
            int testMineCount = 0;

            //Action
            var mines = MineManager.CreateMineLocations(1, 1, testMineCount);
            resultMineCount = GetMineCount(mines);

            //Assert
            Assert.True(resultMineCount == 0);
        }
        [Fact]
        public void Board1x1Mine1()
        {
            //Arrange
            int resultMineCount = 0;
            int testMineCount = 1;

            //Action
            var mines = MineManager.CreateMineLocations(1, 1, testMineCount);
            resultMineCount = GetMineCount(mines);

            //Assert
            Assert.True(resultMineCount == 0);
        }
        [Fact]
        public void Board1x1Mine10()
        {
            //Arrange
            int resultMineCount = 0;
            int testMineCount = 10;

            //Action
            var mines = MineManager.CreateMineLocations(1, 1, testMineCount);
            resultMineCount = GetMineCount(mines);

            //Assert
            Assert.True(resultMineCount == 0);
        }
        [Fact]
        public void Board10x10Mine0()
        {
            //Arrange
            int resultMineCount = 0;
            int testMineCount = 0;

            //Action
            var mines = MineManager.CreateMineLocations(10, 10, testMineCount);
            resultMineCount = GetMineCount(mines);

            //Assert
            Assert.True(resultMineCount == 0);
        }
        [Fact]
        public void Board10x10Mine1()
        {
            //Arrange
            int resultMineCount = 0;
            int testMineCount = 1;

            //Action
            var mines = MineManager.CreateMineLocations(10, 10, testMineCount);
            resultMineCount = GetMineCount(mines);

            //Assert
            Assert.True(resultMineCount == testMineCount);
        }
        [Fact]
        public void Board10x10Mine5()
        {
            //Arrange
            int resultMineCount = 0;
            int testMineCount = 5;

            //Action
            var mines = MineManager.CreateMineLocations(10, 10, testMineCount);
            resultMineCount = GetMineCount(mines);

            //Assert
            Assert.True(resultMineCount == testMineCount);
        }
        [Fact]
        public void Board10x10Mine100()
        {
            //Arrange
            int resultMineCount = 0;
            int testMineCount = 100;

            //Action
            var mines = MineManager.CreateMineLocations(10, 10, testMineCount);
            resultMineCount = GetMineCount(mines);

            //Assert
            Assert.True(resultMineCount == 0);
        }
        [Fact]
        public void Board1000x1000Mine100()
        {
            //Arrange
            int resultMineCount = 0;
            int testMineCount = 100;

            //Action
            var mines = MineManager.CreateMineLocations(100, 100, testMineCount);
            resultMineCount = GetMineCount(mines);

            //Assert
            Assert.True(resultMineCount == testMineCount);
        }
        //[Theory]
        //[InlineData(0, 0, 0, 0)]
        //[InlineData(0, 0, 1, 0)]
        //[InlineData(0, 0, 10, 0)]
        //public void BoardAndMine(int width, int height, int mineCount, int assertMineCount)
        //{
        //    //Arrange
        //    int resultMineCount = 0;
        //    int testMineCount = mineCount;

        //    //Action
        //    var mines = MineManager.CreateMineLocations(width, height, testMineCount);
        //    resultMineCount = GetMineCount(mines);

        //    //Assert
        //    Assert.True(resultMineCount == assertMineCount);
        //}
    }
}
