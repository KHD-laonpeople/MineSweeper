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
        //지뢰, 깃발 이미지 한번만 불러오도록 구성. 윈도우 생성자에서 한 번만 이미지 저장(상대경로)
        //파일 내부에 이미지 업로드하여 어느곳에서든 사용가능하게 구성
        Image mineImg = new Image();
        Image findMineImg = new Image();
        Image flagImg = new Image();

        int x = 0;
        int y = 0;
        int mine = 0;

        int leftButtonCount = 0;
        int rightButtonCount = 0;

        List<List<int>> mineBackground = new List<List<int>>();
        List<Button> buttons = new List<Button>();

        public MainWindow()
        {
            InitializeComponent();
            mineImg.Source = new BitmapImage(new Uri(@"\bin\Debug\mine.png", UriKind.Relative));
            findMineImg.Source = new BitmapImage(new Uri(@"\bin\Debug\findMine.png", UriKind.Relative));
            flagImg.Source = new BitmapImage(new Uri(@"\bin\Debug\flag.png", UriKind.Relative));
            
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
            return;
        }

        //지뢰 랜덤 위치 배정, 행 셔플 -> 열 셔플로 진행
        private void MineSufle()
        {
            Random random = new Random();
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y - 1; j++)
                {
                    //int rand_1 = random.Next(i, y - 1);
                    int rand_1 = random.Next(j + 1, y);

                    int temp = mineBackground[rand_1][i];
                    mineBackground[rand_1][i] = mineBackground[j][i];
                    mineBackground[j][i] = temp;
                }
            }
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x - 1; j++)
                {
                    int rand_2 = random.Next(j + 1, x);

                    int temp = mineBackground[i][rand_2];
                    mineBackground[i][rand_2] = mineBackground[i][j];
                    mineBackground[i][j] = temp;
                }
            }
            for(int i = 0; i < x; i++)
            {
                for(int j = 0; j < y; j++)
                {
                    int button_row = j % y;
                    int button_column = i % x;
                    mineBackground[button_row][button_column] = mineBackground[button_row][button_column] == 9 ? 9 : CheckRound_MineCount(button_row, button_column);
                }
            }
            
            return;
        }
        //게임 보드 생성, 지뢰 생성 및 기본 보드 생성
        private void gameBoardCreate()
        {
            mineBackground.Clear();
            leftButtonCount = 0;
            rightButtonCount = 0;
            for (int i = 0; i < y; i++)
            {
                List<int> temp = new List<int>();
                for (int j = 0; j < x; j++)
                {
                    temp.Add(j + i * x < mine ? 9 : 0);
                }
                mineBackground.Add(temp);
            }
            MineSufle();
        }
        //보드 크기, 지뢰 개수를 입력받아 해당 게임보드 생성
        private void Button_GameStart(object sender, RoutedEventArgs e)
        {
            buttons.Clear();
            //텍스트가 비어있을 경우 오류메세지
            if(boardX.Text == "" || boardY.Text == "" || mineEA.Text == "")
            {
                MessageBox.Show("입력값을 확인해주세요.");
                return;
            }

            x = int.Parse(boardX.Text);
            y = int.Parse(boardY.Text);
            mine = int.Parse(mineEA.Text);

            //지뢰 개수가 보드 크기와 같거나 많으면 오류메세지
            if (x * y <= mine || mine == 0)
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
            grid1.Width = sizeX;
            grid1.Height = sizeY;
            grid1.Margin = new Thickness(0, 0, 0, y <= 12 ? (Convert.ToInt32(mainWindow.Height) - sizeY - 80) / 2 : 20);

            //동적 버튼 생성, 버튼의 크기 20x20 지정, 버튼 이름을 button_번호로 입력하여 해당 번호 사용
            //토글 버튼을 사용하여 시각화 높임, 2중 for문을 사용하여 행렬 방식 표시
            //PreviewMousecDown을 사용하여 좌클릭시 기본으로 사용되는 Click이벤트 충돌 방지
            for (int i = 0; i < y; i++)
            {
                for(int j = 0; j < x; j++)
                {
                    buttons.Add(new Button());
                    buttons[i * x + j].Name = $"button_{i * x + j}";
                    buttons[i * x + j].Height = 20;
                    buttons[i * x + j].Width = 20;
                    buttons[i * x + j].HorizontalAlignment = HorizontalAlignment.Left;
                    buttons[i * x + j].VerticalAlignment = VerticalAlignment.Top;
                    buttons[i * x + j].Margin = new Thickness(buttons[i * x + j].Width * j, buttons[i * x + j].Height * i, 0, 0);
                    buttons[i * x + j].PreviewMouseDown += new MouseButtonEventHandler(GameButton_Click);

                    grid1.Children.Add(buttons[i * x + j]);
                }
            }
            //보드 생성 및 버튼 연동
            gameBoardCreate();
            return;
        }

        //해당 버튼 주변 지뢰 개수 카운트
        private int CheckRound_MineCount(int button_row, int button_column)
        {
            int mineCount = 0;
            if (mineBackground[button_row][button_column] == 9) return 9;

            if (button_row != 0 && button_column != 0 && button_row != (y - 1) && button_column != (x - 1))
            {
                mineCount = mineBackground[button_row - 1][button_column - 1] == 9 ? mineCount + 1 : mineCount;
                mineCount = mineBackground[button_row - 1][button_column] == 9 ? mineCount + 1 : mineCount;
                mineCount = mineBackground[button_row - 1][button_column + 1] == 9 ? mineCount + 1 : mineCount;

                mineCount = mineBackground[button_row][button_column - 1] == 9 ? mineCount + 1 : mineCount;
                mineCount = mineBackground[button_row][button_column + 1] == 9 ? mineCount + 1 : mineCount;

                mineCount = mineBackground[button_row + 1][button_column - 1] == 9 ? mineCount + 1 : mineCount;
                mineCount = mineBackground[button_row + 1][button_column] == 9 ? mineCount + 1 : mineCount;
                mineCount = mineBackground[button_row + 1][button_column + 1] == 9 ? mineCount + 1 : mineCount;
            }
            else if (button_row == 0 && button_column == 0)
            {
                mineCount = mineBackground[button_row][button_column + 1] == 9 ? mineCount + 1 : mineCount;

                mineCount = mineBackground[button_row + 1][button_column] == 9 ? mineCount + 1 : mineCount;
                mineCount = mineBackground[button_row + 1][button_column + 1] == 9 ? mineCount + 1 : mineCount;
            }
            else if (button_row == (y - 1) && button_column == (x - 1))
            {
                mineCount = mineBackground[button_row - 1][button_column - 1] == 9 ? mineCount + 1 : mineCount;
                mineCount = mineBackground[button_row - 1][button_column] == 9 ? mineCount + 1 : mineCount;

                mineCount = mineBackground[button_row][button_column - 1] == 9 ? mineCount + 1 : mineCount;
            }
            else if (button_row == 0 && button_column == (x - 1))
            {
                mineCount = mineBackground[button_row][button_column - 1] == 9 ? mineCount + 1 : mineCount;

                mineCount = mineBackground[button_row + 1][button_column - 1] == 9 ? mineCount + 1 : mineCount;
                mineCount = mineBackground[button_row + 1][button_column] == 9 ? mineCount + 1 : mineCount;
            }
            else if (button_row == (y - 1) && button_column == 0)
            {
                mineCount = mineBackground[button_row - 1][button_column] == 9 ? mineCount + 1 : mineCount;
                mineCount = mineBackground[button_row - 1][button_column + 1] == 9 ? mineCount + 1 : mineCount;

                mineCount = mineBackground[button_row][button_column + 1] == 9 ? mineCount + 1 : mineCount;
            }
            else if (button_row == 0)
            {
                mineCount = mineBackground[button_row][button_column - 1] == 9 ? mineCount + 1 : mineCount;
                mineCount = mineBackground[button_row][button_column + 1] == 9 ? mineCount + 1 : mineCount;

                mineCount = mineBackground[button_row + 1][button_column - 1] == 9 ? mineCount + 1 : mineCount;
                mineCount = mineBackground[button_row + 1][button_column] == 9 ? mineCount + 1 : mineCount;
                mineCount = mineBackground[button_row + 1][button_column + 1] == 9 ? mineCount + 1 : mineCount;
            }
            else if (button_column == 0)
            {
                mineCount = mineBackground[button_row - 1][button_column] == 9 ? mineCount + 1 : mineCount;
                mineCount = mineBackground[button_row - 1][button_column + 1] == 9 ? mineCount + 1 : mineCount;

                mineCount = mineBackground[button_row][button_column + 1] == 9 ? mineCount + 1 : mineCount;

                mineCount = mineBackground[button_row + 1][button_column] == 9 ? mineCount + 1 : mineCount;
                mineCount = mineBackground[button_row + 1][button_column + 1] == 9 ? mineCount + 1 : mineCount;
            }
            else if (button_row == (y - 1))
            {
                mineCount = mineBackground[button_row - 1][button_column - 1] == 9 ? mineCount + 1 : mineCount;
                mineCount = mineBackground[button_row - 1][button_column] == 9 ? mineCount + 1 : mineCount;
                mineCount = mineBackground[button_row - 1][button_column + 1] == 9 ? mineCount + 1 : mineCount;

                mineCount = mineBackground[button_row][button_column - 1] == 9 ? mineCount + 1 : mineCount;
                mineCount = mineBackground[button_row][button_column + 1] == 9 ? mineCount + 1 : mineCount;
            }
            else
            {
                mineCount = mineBackground[button_row - 1][button_column - 1] == 9 ? mineCount + 1 : mineCount;
                mineCount = mineBackground[button_row - 1][button_column] == 9 ? mineCount + 1 : mineCount;

                mineCount = mineBackground[button_row][button_column - 1] == 9 ? mineCount + 1 : mineCount;

                mineCount = mineBackground[button_row + 1][button_column - 1] == 9 ? mineCount + 1 : mineCount;
                mineCount = mineBackground[button_row + 1][button_column] == 9 ? mineCount + 1 : mineCount;
            }
            return mineCount;
        }

        private void ZeroRound(int buttonNum)
        {
            int buttonRowOrigin = buttonNum / x;
            int buttonColumnOrigin = buttonNum % x;
            for (int i = -1; i < 2; i++)
            {
                for(int j = -1; j < 2; j++)
                {
                    int buttonLocation = buttonNum + (i * x) + j;
                    int buttonRow = buttonLocation / x;
                    int buttonColumn = buttonLocation % x;

                    if (buttonRowOrigin > 0 && buttonColumnOrigin > 0 && buttonRowOrigin < (y - 1) && buttonColumnOrigin < (x - 1))
                    {}
                    //좌측상단 클릭시
                    else if (buttonRowOrigin == 0 && buttonColumnOrigin == 0)
                    {
                        if (i < 0 || j < 0) continue;
                    }
                    //우측하단 클릭시
                    else if (buttonRowOrigin == (y - 1) && buttonColumnOrigin == (x - 1))
                    {
                        if (i > 0 || j > 0) continue;
                    }
                    //우측상단 클릭시
                    else if (buttonRowOrigin == 0 && buttonColumnOrigin == (x - 1))
                    {
                        if (i < 0 || j > 0) continue;
                    }
                    //좌측하단 클릭시
                    else if (buttonRowOrigin == (y - 1) && buttonColumnOrigin == 0)
                    {
                        if (i > 0 || j < 0) continue;
                    }
                    //상단면 클릭시
                    else if (buttonRowOrigin == 0 && buttonColumnOrigin > 0)
                    {
                        if (i < 0) continue;
                    }
                    //하단면 클릭시
                    else if (buttonRowOrigin == (y - 1) && buttonColumnOrigin > 0)
                    {
                        if (i > 0) continue;
                    }
                    //우측면 클릭시
                    else if (buttonRowOrigin > 0 && buttonColumnOrigin == 0)
                    {
                        if (j < 0) continue;
                    }
                    //좌측면 클릭시
                    else if (buttonRowOrigin > 0 && buttonColumnOrigin == (x - 1))
                    {
                        if (j > 0) continue;
                    }
                    else
                    {
                        continue;
                    }
                    if(buttons[buttonLocation].Content == flagImg)
                    {
                        return;
                    }
                    if(!(i == 0 && j == 0)) leftButtonCount++;
                    buttons[buttonLocation].Content = CheckRound_MineCount(buttonRow, buttonColumn);
                    buttons[buttonLocation].IsEnabled = false;
                }
            }
        }
        //마우스 동작 구현
        private void FindMine(object sender, string right_left)
        {
            Button button = sender as Button;
            string[] buttonNumber = button.Name.Split('_');
            int buttonNum = int.Parse(buttonNumber[1]);
            int button_row = buttonNum / x;
            int button_column = buttonNum % x;

            if (right_left == "Same")
            {

            }
            else if(right_left == "Left")
            {
                if (button.Content == flagImg)
                {
                    button.IsEnabled = true;
                }
                else
                {

                    //첫 클릭이 지뢰일 경우 보드 새로 생성, while문을 통해 해당 버튼이 지뢰가 아닐때까지 반복
                    while(mineBackground[button_row][button_column] == 9 && leftButtonCount == 0)
                    {
                        gameBoardCreate();
                    }

                    if(mineBackground[button_row][button_column] == 9)
                    {
                        button.Content = findMineImg;
                        for(int i = 0; i < buttons.Count; i++)
                        {
                            buttons[i].IsEnabled = false;
                        }
                        MessageBox.Show("아쉽습니다.", "GameOver");
                    }
                    else
                    {
                        button.Content = mineBackground[button_row][button_column];
                        leftButtonCount++;
                        button.IsEnabled = false;

                        if (CheckRound_MineCount(button_row, button_column) == 0)
                        {
                            ZeroRound(buttonNum);
                        }
                    }
                }
                
            }
            else if(right_left == "Right")
            {
                if(button.Content != flagImg)
                {
                    button.Content = flagImg;
                    rightButtonCount++;
                }
                else
                {
                    button.Content = mineBackground[button_row][button_column];
                    rightButtonCount++;
                }
            }
            if(leftButtonCount == (x * y - mine))
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].IsEnabled = false;
                }
                MessageBox.Show("축하합니다.","WIN");
            }
            return;
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

            return;
        }
    }
}
