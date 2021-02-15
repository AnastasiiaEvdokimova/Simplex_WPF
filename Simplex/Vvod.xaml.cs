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
using System.Windows.Shapes;

namespace Simplex
{
    /// <summary>
    /// Логика взаимодействия для Vvod.xaml
    /// </summary>
    public partial class Vvod : Window
    {
        bool all = true;
        public Vvod()
        {
            InitializeComponent();
            StatClass.MainEquations = new double[StatClass.perem, StatClass.ypav];
            StatClass.EquationRightSide = new double[StatClass.ypav];
            StatClass.Signs = new int[StatClass.ypav];
            StatClass.Fx = new double[StatClass.perem];
            initializeGrid(grid1, StatClass.MainEquations, false);
            initializeGrid(grid3, StatClass.EquationRightSide, false);
            initializeGrid(grid2, StatClass.Signs, true);
            initializeGrid(grid4, StatClass.Fx, true);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Запись в матрицы данных из гридов путем вызова соответсвующей процедуры
            getValuesFromGrid(grid1, StatClass.MainEquations, false);
            getValuesFromGrid(grid3, StatClass.EquationRightSide, false);
            getValuesFromGrid(grid2, StatClass.Signs);
            getValuesFromGrid(grid4, StatClass.Fx, true);
            //Переход обратно на основную форму
            if (all)
            {
                MainWindow vv = new MainWindow();
                vv.Show();
                vv.StartSolution();
                Hide();

            }
            all = true;

        }

        private void initializeGrid(Grid grid, double[,] matrix, bool f)
        {
            if (grid != null)
            {
                //Очитска перед тем как что то делать 
                grid.Children.Clear();
                grid.ColumnDefinitions.Clear();
                grid.RowDefinitions.Clear();
                // вычисление количества сток и столбцов
                int columns = matrix.GetLength(0);
                int rows = matrix.GetLength(1);

                //Добавление коректного числа столбцов
                for (int x = 0; x < columns; x++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star), });
                }
                //Добавление коректного числа строк
                for (int y = 0; y < rows; y++)
                {

                    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star), });
                }
                //Так как ключ переданный при вызове  означает false будет выполнен ниже представленный код
                if (f == false)
                {
                    for (int x = 0; x < columns; x++)
                    {
                        for (int y = 0; y < rows; y++)
                        {
                            double cell = (double)matrix[x, y];
                            TextBox t = new TextBox();
                            t.Width = 35;
                            t.Text = cell.ToString();
                            t.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            t.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            t.SetValue(Grid.RowProperty, y);
                            t.SetValue(Grid.ColumnProperty, x);
                            t.TextAlignment = TextAlignment.Center;
                          //  t.Text = "";
                            grid.Children.Add(t);

                        }
                    }
                }
                //Проверка на то не является ли данный грид который необходимо комбобоксами
                if (f == true)
                {
                    for (int x = 0; x < columns; x++)
                    {
                        for (int y = 0; y < rows; y++)
                        {
                            double cell = (double)matrix[x, y];
                            ComboBox c = new ComboBox();
                            c.Width = 45;
                            c.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            c.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            c.SetValue(Grid.RowProperty, y);
                            c.SetValue(Grid.ColumnProperty, x);
                            c.Items.Add("=");
                            c.Items.Add("<=");
                            c.Items.Add("=>");
                            c.SelectedIndex = 0;
                            grid.Children.Add(c);
                        }
                    }

                }
            }
        }

        private void initializeGrid(Grid grid, int[] matrix, bool f)
        {
            if (grid != null)
            {
                //Очитска перед тем как что то делать 
                grid.Children.Clear();
                grid.ColumnDefinitions.Clear();
                grid.RowDefinitions.Clear();
                // вычисление количества сток и столбцов
                int columns = matrix.GetLength(0);
                int rows = matrix.GetLength(0);

                //Добавление коректного числа столбцов
                //for (int x = 0; x < columns; x++)
                //{
                //    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star), });
                //}
                //Добавление коректного числа строк
                for (int y = 0; y < rows; y++)
                {

                    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star), });
                }
                //Так как ключ переданный при вызове  означает false будет выполнен ниже представленный код
            
                //Проверка на то не является ли данный грид который необходимо комбобоксами
               
                 
                        for (int y = 0; y < rows; y++)
                        {
                            double cell = (double)matrix[y];
                            ComboBox c = new ComboBox();
                            c.Width = 45;
                            c.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            c.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            c.SetValue(Grid.RowProperty, y);
                            c.Items.Add("=");
                            c.Items.Add("<=");
                            c.Items.Add("=>");
                            c.SelectedIndex = 0;
                            grid.Children.Add(c);
                        }
                    

                
            }
        }

        private void initializeGrid(Grid grid, double[] matrix, bool f)
        {
            if (grid != null)
            {
                //Очитска перед тем как что то делать 
                grid.Children.Clear();
                grid.ColumnDefinitions.Clear();
                grid.RowDefinitions.Clear();
                // вычисление количества сток и столбцов
                int columns = matrix.GetLength(0);
              int rows = matrix.GetLength(0);

                //Добавление коректного числа столбцов
                if (f)
                for (int x = 0; x < columns; x++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star), });
                }
                //Добавление коректного числа строк
                if (!f)
                for (int y = 0; y < rows; y++)
                {

                    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star), });
                }
                //Так как ключ переданный при вызове  означает false будет выполнен ниже представленный код
               if (f)
                    for (int x = 0; x < columns; x++)
                    {
                        
                            double cell = (double)matrix[x];
                            TextBox t = new TextBox();
                            t.Width = 35;
                            t.Text = cell.ToString();
                            t.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            t.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                           // t.SetValue(Grid.RowProperty, y);
                            t.SetValue(Grid.ColumnProperty, x);
                            t.TextAlignment = TextAlignment.Center;
                            //  t.Text = "";
                            grid.Children.Add(t);

                        
                    }
                if (!f)
                    for (int y = 0; y < rows; y++)
                    {

                        double cell = (double)matrix[y];
                        TextBox t = new TextBox();
                        t.Width = 35;
                        t.Text = cell.ToString();
                        t.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                        t.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        t.SetValue(Grid.RowProperty, y);
                       // t.SetValue(Grid.ColumnProperty, x);
                        t.TextAlignment = TextAlignment.Center;
                        //  t.Text = "";
                        grid.Children.Add(t);


                    }
                //Проверка на то не является ли данный грид который необходимо комбобоксами


            }
            }
        

        //Функция записи в матрицы 
        private void getValuesFromGrid(Grid grid, double[,] matrix, bool f)
        {

            int columns = grid.ColumnDefinitions.Count;
            int rows = grid.RowDefinitions.Count;
            //Проверка на то не является ли данный грид с комбобоксами
            if (f == false)
            {
                for (int c = 0; c < grid.Children.Count; c++)
                {
                    TextBox t = (TextBox)grid.Children[c];
                    int row = Grid.GetRow(t);
                    int column = Grid.GetColumn(t);
                    //Проверка на корректность введенных данных
                    if (!double.TryParse(t.Text, out matrix[column, row]))
                    {
                        MessageBox.Show("Проверьте коректность введенных данных");
                        all = false;
                        return;

                    }


                }
            }

           

        }

        private void getValuesFromGrid(Grid grid, int [] matrix)
        {

            int rows = grid.RowDefinitions.Count;
            
                for (int c = 0; c < grid.Children.Count; c++)
                {
                    ComboBox c1 = (ComboBox)grid.Children[c];
                    int row = Grid.GetRow(c1);
                    matrix[row] = c1.SelectedIndex;
                }
            
        }

        private void getValuesFromGrid(Grid grid, double[] matrix, bool f)
        {
            int columns = grid.ColumnDefinitions.Count;
            int rows = grid.RowDefinitions.Count;
            //Проверка на вертикальность или горизонтальность таблицы
            if (f)
            {
                for (int c = 0; c < grid.Children.Count; c++)
                {
                    TextBox t = (TextBox)grid.Children[c];
                    int row = Grid.GetRow(t);
                    int column = Grid.GetColumn(t);
                    //Проверка на корректность введенных данных
                    if (!double.TryParse(t.Text, out matrix[column]))
                    {
                        MessageBox.Show("Проверьте коректность введенных данных");
                        all = false;
                        return;
                    }

                }
            }
            else
            {
                for (int c = 0; c < grid.Children.Count; c++)
                {
                    TextBox t = (TextBox)grid.Children[c];
                    int row = Grid.GetRow(t);
                    int column = Grid.GetColumn(t);
                    //Проверка на корректность введенных данных
                    if (!double.TryParse(t.Text, out matrix[row]))
                    {
                        MessageBox.Show("Проверьте коректность введенных данных");
                        all = false;
                        return;
                    }

                }
            }
           
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
