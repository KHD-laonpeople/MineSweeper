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
using MahApps.Metro.Controls;
using BoardDatas;

namespace MINESWEEPER
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        //지뢰, 깃발 이미지 한번만 불러오도록 구성. 윈도우 생성자에서 한 번만 이미지 저장(상대경로)
        //파일 내부에 이미지 업로드하여 어느곳에서든 사용가능하게 구성
        private readonly Image mineImg = new Image();
        private readonly Image findMineImg = new Image();
        private readonly Image flagImg = new Image();

        public int x = 0;
        public int y = 0;
        public int mine = 0;

        public int leftButtonCount = 0;

        public List<Button> buttons = new List<Button>();
        private BoardData boardData = new BoardData();

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
        private void TextBox_PreviewTextInput(Object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            return;
        }
        private void WindowSizeControl(int x, int y)
        {
            int sizeX = x * 20;
            int sizeY = y * 20;
            mainWindow.Width = y > 12 ? sizeX + 60 : 300;
            mainWindow.Height = x > 12 ? sizeY + 120 : 360;
            grid1.Children.Clear();
            grid1.Width = sizeX;
            grid1.Height = sizeY;
            grid1.Margin = new Thickness(0, 0, 0, y <= 12 ? (Convert.ToInt32(mainWindow.Height) - sizeY - 80) / 2 : 20);

            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    buttons.Add(new Button());
                    buttons[i * x + j].Name = $"button_{i * x + j}";
                    buttons[i * x + j].PreviewMouseDown += new MouseButtonEventHandler(GameButton_Click);

                    grid1.Children.Add(buttons[i * x + j]);
                }
            }
        }
        //보드 크기, 지뢰 개수를 입력받아 해당 게임보드 생성
        private void Button_GameStart(object sender, RoutedEventArgs e)
        {
            buttons.Clear();
            leftButtonCount = 0;
            //텍스트가 비어있을 경우 오류메세지
            if (boardX.Text == "" || boardY.Text == "" || mineEA.Text == "")
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
                MessageBox.Show("지뢰개수를 확인해주세요.");
                return;
            }

            //생성 개수에 따른 윈도우 사이즈 변경
            WindowSizeControl(x, y);

            boardData.GameBoardCreate(x,y,mine);
            bool[,] mineLocation = boardData.CreateMineLocations(x, y, mine);
            int[,] mineCount = boardData.CreateMineMap(mineLocation);

            return;
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
                    if(!(i == 0 && j == 0) && buttons[buttonLocation].Content == null) leftButtonCount++;

                    int mineCount = boardData.buttonDatas[buttonRow][buttonColumn].GetRoundMineCount();
                    buttons[buttonLocation].Content = mineCount;
                    //buttons[buttonLocation].IsEnabled = false;
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
                //주변이 지뢰, 깃발이 아니면 모두 IsEnabled = true
                //IsEnabled = true로 할 경우 same 기능 불가능 - 색상 변경으로 바꿔야할듯
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
                    while(boardData.buttonDatas[button_row][button_column].GetMine() && leftButtonCount == 0)
                    {
                        boardData.GameBoardCreate(x,y,mine);
                    }
                    //지뢰일 경우
                    if(boardData.buttonDatas[button_row][button_column].GetMine())
                    {
                        button.Content = findMineImg;
                        for(int i = 0; i < buttons.Count; i++)
                        {
                            buttons[i].IsEnabled = false;
                        }
                        MessageBox.Show("아쉽습니다.", "GameOver");
                    }
                    //지뢰가 아닐 경우
                    else
                    {
                        if (button.Content == null) leftButtonCount++;
                        button.Content = boardData.buttonDatas[button_row][button_column].GetRoundMineCount();

                        if (boardData.buttonDatas[button_row][button_column].GetRoundMineCount() == 0)
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
                }
                else
                {
                    button.Content = null;
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
