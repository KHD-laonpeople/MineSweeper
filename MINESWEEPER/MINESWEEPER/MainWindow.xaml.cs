using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Text.RegularExpressions;


namespace MINESWEEPER
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //텍스트박스 입력전 이벤트
        //입력한 텍스트가 숫자일 경우 regex(0~9)의 값과 일치하여 숫자 입력, 이외의 값은 차단
        private void textBox_PreviewTextInput(Object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //보드 크기, 지뢰 개수를 입력받아 해당 게임보드 생성
        private void Button_GameStart(object sender, RoutedEventArgs e)
        {
            if(boardX.Text == "" || boardY.Text == "" || mineEA.Text == "")
            {
                MessageBox.Show("입력값을 확인해주세요.");
                return;
            }
            int x = int.Parse(boardX.Text);
            int y = int.Parse(boardY.Text);
            int mine = int.Parse(mineEA.Text);

            if(x * y <= mine)
            {
                MessageBox.Show("지뢰개수를 확인해주세요..");
                return;
            }

            int sizeX = x * 20;
            int sizeY = y * 20;

            mainWindow.Width = x > 12 ? sizeX + 60 : 300;
            mainWindow.Height = y > 12 ? sizeY + 120 : 360;
            grid1.Width = sizeX;
            grid1.Height = sizeY;
            grid1.Margin = new Thickness(0, 0, 0, y <= 12 ? (Convert.ToInt32(mainWindow.Height) - sizeY - 80) / 2 : 20);

            for (int i = 0; i < x; i++)
            {
                for(int j = 0; j < y; j++)
                {

                    ToggleButton button = new ToggleButton();
                    button.Name = $"button_{j + i * j + 1}";
                    button.Height = 20;
                    button.Width = 20;
                    button.HorizontalAlignment = HorizontalAlignment.Left;
                    button.VerticalAlignment = VerticalAlignment.Top;
                    button.Margin = new Thickness(button.Width * i, button.Height * j, 0, 0);
                    button.PreviewMouseDown += new MouseButtonEventHandler(GameButton_LeftClick);

                    grid1.Children.Add(button);
                }
            }
        }


        private void GameButton_LeftClick(object sender, MouseEventArgs e)
        {
            int a = 0;
            if(e.LeftButton == MouseButtonState.Pressed && e.RightButton == MouseButtonState.Pressed)
            {
                a = 1;
            }
            else if(e.RightButton == MouseButtonState.Pressed)
            {
                a = 2;
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                a = 3;
            }
            mineEA.Text = a.ToString();
        }
        private void GameButton_RightClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
