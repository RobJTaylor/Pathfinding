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
        private List<int> xSpaces = new List<int>();
        private List<int> ySpaces = new List<int>();
 
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
                this.grid.GetGridPosition(positionX,positionY).SetSpaceOccupant(unit);
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
            button.Name = "_" + row.ToString() + column.ToString();
            if (this.grid.GetGridPosition(row, column).GetSpaceOccupant() >= 0)
            {
                button.Content = " U ";
                button.Foreground = Brushes.White;
                button.Background = Brushes.Green;
            }
            else if (this.grid.GetGridPosition(row, column).GetSpaceType() == 0)
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
            if (this.grid.GetGridPosition(x, y).GetSpaceOccupant() < 0)
            {
                MessageBox.Show("Please select a unit to move");
            } else
            {
                int newX = Convert.ToInt32(Interaction.InputBox("New X Position:", "Move Unit", "0"));
                int newY = Convert.ToInt32(Interaction.InputBox("New Y Position:", "Move Unit", "0"));

                //Spin off a new thread, set stack size to 70 Mb
                //This fixes a potential stackoverflow.
                int returnValue = 0;
                var thread = new Thread(_ => returnValue = FindPath(x, y, newX, newY, x, y), 70000000);
                thread.Start();
                thread.Join();

                //We have found the correct space, start moving unit
                if (returnValue == 1)
                {
                    int unit = this.grid.GetGridPosition(x, y).GetSpaceOccupant();
                    this.grid.GetGridPosition(x, y).RemoveOccupant();

                    for (int space = this.xSpaces.Count - 1; space >= 0; space--)
                    {
                        if (space < this.xSpaces.Count - 1)
                        {
                            //this.grid.GetGridPosition(xSpaces[space + 1], ySpaces[space + 1]).RemoveOccupant();
                            this.grid.SetPlace(xSpaces[space+1], ySpaces[space+1], xSpaces[space], ySpaces[space], unit);
                            Dispatcher.Invoke(new Action(() => { RedrawGrid(xSpaces[space + 1], ySpaces[space + 1], xSpaces[space], ySpaces[space]); }), System.Windows.Threading.DispatcherPriority.ContextIdle, null);
                        }

                        //this.grid.GetGridPosition(xSpaces[space], ySpaces[space]).SetSpaceOccupant(unit);
                        Console.WriteLine("Unit " + unit + " moved to: X " + xSpaces[space] + " Y " + ySpaces[space]);
                    }

                    //this.grid.SetPlace(xSpaces[0], ySpaces[0], newX, newY, unit);
                    //RedrawGrid(xSpaces[0], ySpaces[0], newX, newY);
                }
            }            
        }

        private void RedrawGrid(int oldX, int oldY, int newX, int newY)
        {
            var grid = this.gridWindow.grid;

            //Remove old button and replace
            UIElement oldChild = grid.FindName("_" + oldX.ToString() + oldY.ToString()) as UIElement;
            grid.Children.Remove(oldChild);
            RowDefinition oldDef = new RowDefinition();
            Button oldButton = ButtonStyler(oldX, oldY);
            Grid.SetRow(oldButton, oldX);
            Grid.SetColumn(oldButton, oldY);
            oldDef.Height = GridLength.Auto;
            grid.RowDefinitions.Add(oldDef);
            grid.Children.Add(oldButton);

            Thread.Sleep(50);
    
            //Remove new button and replace
            UIElement newChild = grid.FindName("_" + newX.ToString() + newY.ToString()) as UIElement;
            grid.Children.Remove(newChild);
            RowDefinition newDef = new RowDefinition();
            Button newButton = ButtonStyler(newX, newY);
            Grid.SetRow(newButton, newX);
            Grid.SetColumn(newButton, newY);
            newDef.Height = GridLength.Auto;
            grid.RowDefinitions.Add(newDef);
            grid.Children.Add(newButton);

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
                this.xSpaces.Add(x);
                this.ySpaces.Add(y);
                return 1;
            }

            //We can move left
            if(possibleSpaces[0] == 1)
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
                        this.xSpaces.Add(x);
                        this.ySpaces.Add(y);
                        return 1;
                    }
                }
            }

            //We can move right
            if (possibleSpaces[1] == 1)
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
                        this.xSpaces.Add(x);
                        this.ySpaces.Add(y);
                        return 1;
                    }
                }
            }

            //We can move down
            if (possibleSpaces[2] == 1)
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
                        this.xSpaces.Add(x);
                        this.ySpaces.Add(y);
                        return 1;
                    }
                }
            }

            //We can move up
            if (possibleSpaces[3] == 1)
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
                        this.xSpaces.Add(x);
                        this.ySpaces.Add(y);
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

            //Left
            if (x - 1 >= 0 && this.grid.GetGridPosition(x - 1, y).GetSpaceOccupant() == -1 && this.grid.GetGridPosition(x - 1, y).GetSpaceType() == 0)
            {
                possibleSpaces[0] = 1;
            }

            //Right
            if (x + 1 <= this.grid.GetGridX() && this.grid.GetGridPosition(x + 1, y).GetSpaceOccupant() == -1 && this.grid.GetGridPosition(x + 1, y).GetSpaceType() == 0)
            {
                possibleSpaces[1] = 1;
            }

            //Down
            if (y + 1 <= this.grid.GetGridY() && this.grid.GetGridPosition(x, y + 1).GetSpaceOccupant() == -1 && this.grid.GetGridPosition(x, y + 1).GetSpaceType() == 0)
            {
                possibleSpaces[2] = 1;
            }

            //Up
            if (y - 1 >= 0 && this.grid.GetGridPosition(x, y - 1).GetSpaceOccupant() == -1 && this.grid.GetGridPosition(x, y - 1).GetSpaceType() == 0)
            {
                possibleSpaces[3] = 1;
            }

            return possibleSpaces;
        }
    }
}
