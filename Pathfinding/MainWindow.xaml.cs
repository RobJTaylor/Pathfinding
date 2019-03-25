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
    }
}
