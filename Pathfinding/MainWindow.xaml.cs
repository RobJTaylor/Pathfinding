using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using Microsoft.VisualBasic;

namespace Pathfinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CustomGrid grid;
        private Unit[] units;
        private GridWindow gridWindow;
        private List<int> xChecked = new List<int>();
        private List<int> yChecked = new List<int>();
 
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int x = Convert.ToInt32(TB_GridX.Text);
            int y = Convert.ToInt32(TB_GridY.Text);
            int probability = Convert.ToInt32(TB_Probability.Text);
            int units = Convert.ToInt32(TB_Units.Text);

            //Initialise grid and units
            this.grid = new CustomGrid(x,y,probability,units);
            this.units = new Unit[units];
            for (int unit = 0; unit < units; unit++)
            {
                this.units[unit] = new Unit(unit, this.grid);
                int positionX = this.units[unit].GetPositionX();
                int positionY = this.units[unit].GetPositionY();
                this.grid.SetPlace(positionX, positionY, 3);
            }

            this.grid.PrintGrid();
            this.gridWindow = new GridWindow();
            var grid = gridWindow.grid;
            var populatedGrid = PopulateGrid(grid);
            gridWindow.Owner = this;
            gridWindow.Show();
        }

        private Grid PopulateGrid(Grid grid)
        {
            for (int column = 0; column < this.grid.GetGridY(); column++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                colDef.Width = GridLength.Auto;
                grid.ColumnDefinitions.Add(colDef);
                for (int row = 0; row < this.grid.GetGridX(); row++)
                {
                    RowDefinition rowDef = new RowDefinition();
                    Button button = ButtonStyler(row, column);
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, column);
                    rowDef.Height = GridLength.Auto;
                    grid.RowDefinitions.Add(rowDef);
                    grid.Children.Add(button);
                }
            }
            return grid;
        }

        private Button ButtonStyler(int row, int column)
        {
            Button button = new Button();
            button.ToolTip = row + " " + column;
            button.Click += delegate { MoveUnit(row, column); };
            if (this.grid.GetGridPosition(row, column) == 3)
            {
                button.Content = " U ";
                button.Foreground = Brushes.White;
                button.Background = Brushes.Green;
            }
            else if (this.grid.GetGridPosition(row, column) == 0)
            {
                button.Content = "   ";
                button.Foreground = Brushes.Black;
                button.Background = Brushes.White;
            }
            else
            {
                button.Content = "  ";
                button.Background = Brushes.Gray;
            }
            return button;
        }

        private void MoveUnit(int x, int y)
        {
            if (this.grid.GetGridPosition(x, y) != 3)
            {
                MessageBox.Show("Please select a unit to move");
            } else
            {
                int newX = Convert.ToInt32(Interaction.InputBox("New X Position:", "Move Unit", "0"));
                int newY = Convert.ToInt32(Interaction.InputBox("New Y Position:", "Move Unit", "0"));

                //Spin off a new thread, set stack size to 700 Mb
                //This is fixes a potential stackoverflow.
                var thread = new Thread(_ => FindPath(x, y, newX, newY, x, y), 700000000);
                thread.Start();
                thread.Join();
            }            
        }

        private int FindPath(int x, int y, int newX, int newY, int lastX, int lastY)
        {
            Console.WriteLine("Testing " + x + " " + y);
            this.xChecked.Add(x);
            this.yChecked.Add(y);

            //0 = left, 1 = right, 2 = down, 3 = up
            int[] possibleSpaces = CheckAjacentSpaces(x, y);

            if(x == newX && y == newY)
            {
                MessageBox.Show("Space found!!");
                return 1;
            }

            //We can move left
            if(possibleSpaces[0] == 1 && x - 1 != lastX)
            {
                lastX = x;
                lastY = y;
                Boolean check = true;
                for (int i = 0; i < this.xChecked.Count; i++)
                {
                    if (x - 1 == xChecked[i] && y == yChecked[i])
                    {
                        check = false;
                    }
                }

                if (check == true && x - 1 > 0)
                {
                    int findPath = FindPath(x - 1, y, newX, newY, lastX, lastY);
                    if (findPath == 1)
                    {
                        return 1;
                    }
                }
            }

            //We can move right
            if (possibleSpaces[1] == 1 && x + 1 != lastX)
            {
                lastX = x;
                lastY = y;
                Boolean check = true;
                for (int i = 0; i < this.xChecked.Count; i++)
                {
                    if (x + 1 == xChecked[i] && y == yChecked[i])
                    {
                        check = false;
                    }
                }

                if (check == true && x + 1 < this.grid.GetGridX() - 1)
                {
                    int findPath = FindPath(x + 1, y, newX, newY, lastX, lastY);
                    if (findPath == 1)
                    {
                        return 1;
                    }
                }
            }

            //We can move down
            if (possibleSpaces[2] == 1 && y + 1 != lastY)
            {
                lastX = x;
                lastY = y;
                Boolean check = true;
                for (int i = 0; i < this.xChecked.Count; i++)
                {
                    if (x == xChecked[i] && y + 1 == yChecked[i])
                    {
                        check = false;
                    }
                }

                if (check == true && y + 1 < this.grid.GetGridY() - 1)
                {
                    int findPath = FindPath(x, y + 1, newX, newY, lastX, lastY);
                    if (findPath == 1)
                    {
                        return 1;
                    }
                }
            }

            //We can move up
            if (possibleSpaces[3] == 1 && y - 1 != lastY)
            {
                lastX = x;
                lastY = y;
                Boolean check = true;
                for (int i = 0; i < this.xChecked.Count; i++)
                {
                    if (x == xChecked[i] && y - 1 == yChecked[i])
                    {
                        check = false;
                    }
                }

                if (check == true && y - 1 > 0)
                {
                    int findPath = FindPath(x, y - 1, newX, newY, lastX, lastY);
                    if (findPath == 1)
                    {
                        return 1;
                    }
                }
            }

            return 0;
        }

        private int[] CheckAjacentSpaces(int x, int y)
        {
            int[] possibleSpaces = new int[4];

            for (int i = 0; i < 3; i ++)
            {
                possibleSpaces[i] = 0;
            }

            if (x - 1 >= 0 && this.grid.GetGridPosition(x - 1, y) != 3 && this.grid.GetGridPosition(x - 1, y) != 1)
            {
                possibleSpaces[0] = 1;
            }

            if (x + 1 <= this.grid.GetGridX() && this.grid.GetGridPosition(x + 1, y) != 3 && this.grid.GetGridPosition(x + 1, y) != 1)
            {
                possibleSpaces[1] = 1;
            }

            if (y + 1 <= this.grid.GetGridY() && this.grid.GetGridPosition(x, y + 1) != 3 && this.grid.GetGridPosition(x, y + 1) != 1)
            {
                possibleSpaces[2] = 1;
            }

            if (y - 1 >= 0 && this.grid.GetGridPosition(x, y - 1) != 3 && this.grid.GetGridPosition(x, y - 1) != 1)
            {
                possibleSpaces[3] = 1;
            }

            return possibleSpaces;
        }
    }
}
