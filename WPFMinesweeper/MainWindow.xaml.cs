using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace WPFMinesweeper
{
    public struct mineinfo
    {
        public String mineid;
        public int row;
        public int col;
        public int minecount;

    };

    
    public partial class MainWindow : Window
    {
        Grid grid1;
        public int nRows, nCols;
        static int nSquares;
        List<int> mineButtons;
        static int nOpenMineCount ;
        static int nCurrentMineCount = 0;
        TextBlock t1;        
        public List<mineinfo> mineinfolist;

        public MainWindow()
        {
            InitializeComponent();
                        
            mineButtons = new List<int>();            
            mineinfolist = new List<mineinfo>();
            
            nRows = 9;
            nCols = 9;
            ResetGame();
            t1 = new TextBlock();
            t1.Text = "H";
                        
        }

        

        public void CreateNumberGrid()
        {
            for (int i = 0; i < nRows; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                grid1.RowDefinitions.Add(rowDefinition);
            }

            for (int i = 0; i < nCols; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                grid1.ColumnDefinitions.Add(columnDefinition);
            }
                        
        }


        public void CreateTiles()
        {
            int tileCount = 1;
            for (int r = 0; r < nRows; r++)
            {
                for (int c = 0; c < nCols; c++)
                {
                    
                   
                    GameButton btn = new GameButton(r * nRows + c);                    
                    btn.Click += new RoutedEventHandler(GameButton_Click);                    
                    grid1.Children.Add(btn);
                    Grid.SetRow(btn, r);
                    Grid.SetColumn(btn, c);
                    tileCount++;
                }

            }


        }

        
        public void GameButton_Click(object sender, RoutedEventArgs e)
        {
            GameButton btn = sender as GameButton;
            
            if (mineButtons.Contains(btn.Id))
            {
                btn.Template = (ControlTemplate)FindResource("mytemplate1");
                //MediaPlayer mplayer = new MediaPlayer();
                //mplayer.Open(new Uri(@"..\..\EXPLODE.WAV", UriKind.Relative));
                //mplayer.Play();
                OpenAllMines();
                ShowResultDialog(Brushes.Red, "L + Ratio");
                

                
            }
            else
            {
                btn.Template = (ControlTemplate)FindResource("mytemplate");

                if (0 == btn.isButtonOpened)
                {
                    btn.isButtonOpened = 1;
                    nCurrentMineCount++;

                    if (nOpenMineCount == nCurrentMineCount)
                    {
                        OpenAllMines();                        
                        ShowResultDialog(Brushes.Green, "Weiner!!");                        
                        return;
                    }

                    btn = sender as GameButton;
                    int row = ((int)btn.GetValue(Grid.RowProperty));
                    int col = ((int)btn.GetValue(Grid.ColumnProperty));
                    StringBuilder bs = new StringBuilder("row=");
                    bs.Append(row.ToString());
                    bs.Append("Col=");
                    bs.Append(col.ToString());
                    int x = CalculateNumberForButton(row, col);
                    btn.Content = x;

                    if (0 == x)
                    {
                        OpenFullButton(row, col);
                    }
           

                }
            }

            

        }

        public void ShowResultDialog(Brush clrBrush, String msg)
        {
            ResultMessageDialog dlg = new ResultMessageDialog();
            dlg.Owner = this;
            dlg.textBlock1.Foreground = clrBrush;
            dlg.textBlock1.Text = msg;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dlg.ShowDialog();
            
        }

        public mineinfo PrepareMineInfo(int row, int col, int minecount)
        {
            mineinfo minfo = new mineinfo();
            minfo.row = row;
            minfo.col = col;
            minfo.minecount = minecount;

            return minfo;
            
        }

        public void OpenFullButton(int row, int col)
        {
            int digit;
            int m;
            List<mineinfo> mineinfolist = new List<mineinfo>();

            if ((row > 0) && (col > 0))
            {
                digit = (row - 1) * nRows + (col - 1);
                m = OpenButton(digit);
                mineinfo minfo = PrepareMineInfo(row - 1, col - 1, m);
                mineinfolist.Add(minfo);
                
            }

            if (row > 0)
            {
                digit = (row - 1) * nRows + (col);
                m = OpenButton(digit);
                mineinfo minfo = PrepareMineInfo(row - 1, col, m);                
                mineinfolist.Add(minfo);
            }

            if (row > 0 & col <(nCols-1))
            {
                digit = (row - 1) * nRows + (col+1);
                m = OpenButton(digit);
                mineinfo minfo = PrepareMineInfo(row - 1, col+1, m);                
                mineinfolist.Add(minfo);
            }

            if (col > 0)
            {
                digit = (row ) * nRows + (col - 1);
                m = OpenButton(digit);
                mineinfo minfo = PrepareMineInfo(row, col - 1, m);                
                mineinfolist.Add(minfo);
            }

            if (col < (nCols - 1))
            {
                digit = (row) * nRows + (col + 1);
                m = OpenButton(digit);
                mineinfo minfo = PrepareMineInfo(row, col + 1, m);                
                mineinfolist.Add(minfo);
            }

            if ((row < (nRows-1)) && (col > 0))
            {
                digit = (row + 1) * nRows + (col - 1);
                m = OpenButton(digit);
                mineinfo minfo = PrepareMineInfo(row+1, col - 1, m);                
                mineinfolist.Add(minfo);
            }

            if ((row < (nRows - 1)))
            {
                digit = (row + 1) * nRows + (col);
                m = OpenButton(digit);
                mineinfo minfo = PrepareMineInfo(row+1, col, m);                
                mineinfolist.Add(minfo);
            }

            if ((row < (nRows - 1)) && (col < (nCols - 1)))
            {
                digit = (row + 1) * nRows + (col + 1);
                m = OpenButton(digit);
                mineinfo minfo = PrepareMineInfo(row+1, col + 1, m);                
                mineinfolist.Add(minfo);
            }

            foreach (mineinfo om in mineinfolist)
            {
                StringBuilder sb = new StringBuilder();
                
                if (om.minecount == 0)
                {                    
                    OpenFullButton(om.row, om.col);
                }
            }

            return;
        }

        public void OpenMine(int digit)
        {
            GameButton btn =  (GameButton)grid1.Children[digit];
            btn.Template = (ControlTemplate)FindResource("mytemplate1");

        }

        public int OpenButton(int digit)
        {
            GameButton btn =  (GameButton)grid1.Children[digit];
            btn.Template = (ControlTemplate)FindResource("mytemplate");
            int x = -1;

            if (0 == btn.isButtonOpened)
            {
                btn.isButtonOpened = 1;
                nCurrentMineCount++;

                int row = ((int)btn.GetValue(Grid.RowProperty));
                int col = ((int)btn.GetValue(Grid.ColumnProperty));
           
                x = CalculateNumberForButton(row, col);
                btn.Content = x;


                if (nOpenMineCount == nCurrentMineCount)
                {
                    OpenAllMines();
                    ShowResultDialog(Brushes.Green, "Weiner");                    
                }

            }

           
            return x;
        }

        public void AddMenuToGrid()
        {
            ContextMenu mnu = new ContextMenu();

            MenuItem undoItem = new MenuItem();
            undoItem.Header = "NewGame";
            undoItem.Click += new RoutedEventHandler(NewGame_Click);
            mnu.Items.Add(undoItem);

            MenuItem replayItem = new MenuItem();
            replayItem.Header = "Settings";
            replayItem.Click += new RoutedEventHandler(Settings_Click);            
            mnu.Items.Add(replayItem);

            grid1.ContextMenu = mnu;

        }

        public void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsDlg dlg = new SettingsDlg();
            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dlg.ShowDialog();

        }

        public void NewGame_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }

        void ResetGame()
        {
            grid1 = new Grid();
            grid1.MaxHeight = 900;
            grid1.MaxWidth = 900;            
            mineButtons = new List<int>();

            Border border = new Border();
            border.Child = grid1;

            Content = border;
            nSquares = nRows * nCols;
            nOpenMineCount = nSquares - nRows;
            CreateNumberGrid();
            CreateTiles();
            GenerateMines();
            nCurrentMineCount = 0;
            AddMenuToGrid();

        }

     
        public void GenerateMines()
        {
            Random rnd = new Random(DateTime.Now.Second);
            int number = rnd.Next(1, nSquares) % nSquares;
            mineButtons.Add(number);

            for (int count = 0; count < (nRows-1); )
            {
                number = rnd.Next(1, nSquares) % nSquares;
                if (true == mineButtons.Contains(number)) continue;
                else
                {
                    mineButtons.Add(number);
                    count++;
                }
            }

        }

        public void OpenAllMines()
        {
            foreach (int mine in mineButtons)
            {                
                OpenMine(mine);
            }

        }

        public int CalculateNumberForButton(int row, int col)
        {
            int digit = row * nRows + col;

            int minecount = 0;

            if (0 == row && 0 == col)
            {
                digit = col + 1;

                if (mineButtons.Contains(digit))
                {
                    minecount += 1;
                }

                digit = col + nCols;

                if (mineButtons.Contains(digit))
                {
                    minecount += 1;
                }

                digit = col + nCols + 1;

                if (mineButtons.Contains(digit))
                {
                    minecount += 1;
                }

                if (col > 0)
                {
                    digit = col - 1;

                    if (mineButtons.Contains(digit))
                    {
                        minecount += 1;
                    }

                    digit = col + nCols - 1;

                    if (mineButtons.Contains(digit))
                    {
                        minecount += 1;
                    }
                }
                return minecount;
            }

            if ((0 == row) && (col > 0))
            {
                digit = col - 1;

                if (mineButtons.Contains(digit))
                {
                    minecount += 1;
                }

                if (col != (nCols - 1))
                {
                    digit = col + 1;

                    if (mineButtons.Contains(digit))
                    {
                        minecount += 1;
                    }
                }

                digit = nRows + col - 1;
                if (mineButtons.Contains(digit))
                {
                    minecount += 1;
                }

                digit = nRows + col;

                if (mineButtons.Contains(digit))
                {
                    minecount += 1;
                }

                if (col != (nCols - 1))
                {
                    digit = nRows + col + 1;

                    if (mineButtons.Contains(digit))
                    {
                        minecount += 1;
                    }
                }

                return minecount;
            }

            if ((0 == col) && (row > 0))
            {
                digit = (row - 1) * nRows;

                if (mineButtons.Contains(digit))
                {
                    minecount += 1;
                }

                digit = (row - 1) * nRows + 1;

                if (mineButtons.Contains(digit))
                {
                    minecount += 1;
                }

                digit = row * nRows + 1;
                if (mineButtons.Contains(digit))
                {
                    minecount += 1;
                }

                digit = (row + 1) * nRows;

                if (mineButtons.Contains(digit))
                {
                    minecount += 1;
                }

                digit = (row + 1) * nRows + 1;

                if (mineButtons.Contains(digit))
                {
                    minecount += 1;
                }

                return minecount;


            }


            if (row > 0 && col > 0)
            {

                int tRow = row - 1;
                digit = tRow * nRows + col - 1;

                if (mineButtons.Contains(digit))
                {
                    minecount += 1;
                }

                digit = tRow * nRows + col;

                if (mineButtons.Contains(digit))
                {
                    minecount += 1;
                }

                if (col != (nCols - 1))
                {
                    digit = tRow * nRows + col + 1;

                    if (mineButtons.Contains(digit))
                    {
                        minecount += 1;
                    }
                }

               
                digit = row * nRows + col - 1;

                if (mineButtons.Contains(digit))
                {
                    minecount += 1;
                }

                if (col != (nCols - 1))
                {
                    digit = row * nRows + col + 1;

                    if (mineButtons.Contains(digit))
                    {
                        minecount += 1;
                    }
                }
                
                tRow = row + 1;
                digit = tRow * nRows + col - 1;

                if (mineButtons.Contains(digit))
                {
                    minecount += 1;
                }

                digit = tRow * nRows + col;

                if (mineButtons.Contains(digit))
                {
                    minecount += 1;
                }

                if (col != (nCols - 1))
                {
                    digit = tRow * nRows + col + 1;

                    if (mineButtons.Contains(digit))
                    {
                        minecount += 1;
                    }
                }


                return minecount;
            }

            return -1;
        }

    }
}
