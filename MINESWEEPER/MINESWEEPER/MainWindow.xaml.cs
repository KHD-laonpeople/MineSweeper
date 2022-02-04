﻿using System;
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
        //지뢰, 깃발 이미지 한번만 불러오도록 구성. 윈도우 생성자에서 한 번만 이미지 저장(상대경로)
        //파일 내부에 이미지 업로드하여 어느곳에서든 사용가능하게 구성
        Image mineImg = new Image();
        Image findMineImg = new Image();
        Image flagImg = new Image();

        public MainWindow()
        {
            InitializeComponent();
            mineImg.Source = new BitmapImage(new Uri(@"\bin\Debug\mine.png", UriKind.Relative));
            findMineImg.Source = new BitmapImage(new Uri(@"\bin\Debug\findMine.png", UriKind.Relative));
            flagImg.Source = new BitmapImage(new Uri(@"\bin\Debug\findMine.png", UriKind.Relative));

            mineImg.Stretch = Stretch.Uniform;
            findMineImg.Stretch = Stretch.Uniform;
            flagImg.Stretch = Stretch.Uniform;
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
            //텍스트가 비어있을 경우 오류메세지
            if(boardX.Text == "" || boardY.Text == "" || mineEA.Text == "")
            {
                MessageBox.Show("입력값을 확인해주세요.");
                return;
            }
            int x = int.Parse(boardX.Text);
            int y = int.Parse(boardY.Text);
            int mine = int.Parse(mineEA.Text);

            //지뢰 개수가 보드 크기와 같거나 많으면 오류메세지
            if(x * y <= mine)
            {
                MessageBox.Show("지뢰개수를 확인해주세요..");
                return;
            }

            int sizeX = x * 20;
            int sizeY = y * 20;

            //기본 보드 크기가 240x240이고 버튼하나당 20x20이므로 12개가 초과하면 창크기 및 버튼이 생성되는 grid 영역 조절
            //수량이 적을 경우 grid 크기를 조절하여 창의 중앙에 위치하도록 설정
            mainWindow.Width = y > 12 ? sizeX + 60 : 300;
            mainWindow.Height = x > 12 ? sizeY + 120 : 360;
            grid1.Height = sizeX;
            grid1.Width = sizeY;
            grid1.Margin = new Thickness(0, 0, 0, y <= 12 ? (Convert.ToInt32(mainWindow.Height) - sizeY - 80) / 2 : 20);

            //동적 버튼 생성, 버튼의 크기 20x20 지정, 버튼 이름을 button_번호로 입력하여 해당 번호 사용
            //토글 버튼을 사용하여 시각화 높임, 2중 for문을 사용하여 행렬 방식 표시
            //PreviewMousecDown을 사용하여 좌클릭시 기본으로 사용되는 Click이벤트 충돌 방지
            for (int i = 0; i < y; i++)
            {
                for(int j = 0; j < x; j++)
                {
                    Button button = new Button();
                    button.Name = $"button_{j * x + i + 1}";
                    button.Height = 20;
                    button.Width = 20;
                    button.HorizontalAlignment = HorizontalAlignment.Left;
                    button.VerticalAlignment = VerticalAlignment.Top;
                    button.Margin = new Thickness(button.Width * i, button.Height * j, 0, 0);
                    button.PreviewMouseDown += new MouseButtonEventHandler(GameButton_Click);

                    grid1.Children.Add(button);
                }
            }
        }
        private void FindMine(object sender, string right_left)
        {
            Button button = sender as Button;
            string[] buttonNumber = button.Name.Split('_');
            int buttonNum = int.Parse(buttonNumber[1]);

            if (right_left == "Same")
            {

            }
            else if(right_left == "Left")
            {
                //button.IsEnabled = false;
            }
            else if(right_left == "Right")
            {
                button.Content = "1";
                button.Content = flagImg;
            }
        }

        //게임버튼 클릭시 좌클릭, 우클릭, 좌우 동시 클릭 동작 구분 및 동작에 따른 함수 연동
        private void GameButton_Click(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed && e.RightButton == MouseButtonState.Pressed)
            {
                FindMine(sender,"Same");
            }
            else if(e.RightButton == MouseButtonState.Pressed)
            {
                FindMine(sender,"Right");
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                FindMine(sender, "Left");
            }
        }
    }
}
