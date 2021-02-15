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

namespace Simplex
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// //Interface mostly by Малкеров Геннадий (с изменениями)
    /// //Everything else by Skylin (Евдокимова Анастасия)
    /// </summary>
    ///

    //Общий класс для всей программы (нужен для предачи данных между формами)
    public static class StatClass
    {
        public static int perem=1;
        public static int ypav=1;
        public static double[,] MainEquations;//Основные уравнения
        public static int[] Signs;//Знаки уравнений
        public static double[] EquationRightSide;//правая часть уравнений
        public static double[] Fx;//Формула F(X)
        static public bool vbit = false;
       public static bool[] correctColumnsChecked; //для проверки правильных столбцов
        public static int[] CorrectUnosPositionKeeper; //хранит данные о правильных столбцах
        //  public static int[,] ;

        public static String simpleString = "Тестовая строка";
    }
    public partial class MainWindow : Window
    {
        public static int m = 0;
        public static double[,] mass;
        public static double[,] solveArray;
        public static double[,] FxArray;
        public static int kol = 0,vi=0,vj=0;


        public MainWindow()
        {
            InitializeComponent();
            //Для того чтобы после открытия основной формы не обнулялись кобобоксы с количеством переменных 
            matrix1width.SelectedIndex = StatClass.perem - 1;
            matrix1height.SelectedIndex = StatClass.ypav - 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Запись в переменные StatClass и дальнейщий преход на форму ввода для ввода данных
            StatClass.perem = matrix1width.SelectedIndex + 1;
            StatClass.ypav = matrix1height.SelectedIndex + 1;
            StatClass.vbit = true;
            Vvod vv = new Vvod();
            vv.Show();
            Hide();
        }
        // Кнопка подсчета результата
        public void StartSolution()
        {
           
                string s = "", s1 = "", s2 = " ";
                int i = 0, j = 0, kol1 = 0;
                kol1 = 0;
                s = "F(x)= ";
                for (i = 0; i < StatClass.perem; i++)
                {
                    if (StatClass.Fx[i] == 0)
                    {
                        kol1++;
                    }
                }
                if (kol1 != StatClass.perem)
                {
                    Otvet.Items.Add("Функциональное уравнение");
                    for (i = 0; i < StatClass.perem; i++)
                    {
                        if (StatClass.Fx[i] != 0)
                        {
                            kol1--;
                            s1 = " ";
                            s2 = "";
                            s1 = Convert.ToString(StatClass.Fx[i]);
                            s1 = s1 + "*x" + Convert.ToString(i + 1);
                            if (StatClass.Fx[i] > 0)
                                s2 = " + ";
                            else
                                s2 = " ";
                            s = s + s1 + s2;

                        }
                    }
                    int kolsimvl = s.Length;

                    s = s.Substring(0, s.Length - 2);
                    Otvet.Items.Add(s);
                    Otvet.Items.Add("Изначальная матрица");
                    mass = new double[StatClass.ypav, StatClass.perem];
                    for (i = 0; i < StatClass.ypav; i++)
                    {
                        for (j = 0; j < StatClass.perem; j++)
                        {
                        mass[i, j] = StatClass.MainEquations[j, i];
                    
                        }
                    }
                    Vvivod(mass, StatClass.perem);
                    Add();

                    s = "";
                    kol1 = m;
                    Otvet.Items.Add("Добавленное в Fx( )");
                    for (i = 0; i < m; i++)
                    {
                        kol1--;
                        s1 = "";
                        s2 = "";
                        s1 = Convert.ToString(solveArray[StatClass.ypav, i]);
                        s1 = s1 + "        ";
                        if (solveArray[StatClass.ypav, i] > 0)
                            s2 = " + ";
                        s = s + s1 + s2;
                    }
                    kolsimvl = s.Length;
                    s = s.Substring(0, s.Length - 2);
                    Otvet.Items.Add(s);
                    Otvet.Items.Add("\nДобавляем " + Math.Abs(kol) + " фиктивных переменных\n");
                    Otvet.Items.Add("Получилось\n");
                    Vvivod(solveArray, m);
                    Otvet.Items.Add("Начинаем решение\n");
                bool fin = CheckSolution(solveArray); //проверяем решение, заполняем матрицу проверки правильных столбцов
                do
                {
                    LeadingSearch(solveArray, out int li, out int lj);
                    Transform(solveArray, li, lj);
                    fin = CheckSolution(solveArray);
                }
                while (!fin);
                }
                else if (kol1 == StatClass.perem)
                {
                    MessageBox.Show("Решение не будет иметь смысла так как Функциональное уравнение не содержит ненулевых значений", "Симлекс метод");
                }

            }
        
      
        private static string Znak(int k)
        {
            //Узнаем какой знак необходиом поставить в зависимости от выбора
            string s = "";
            if (StatClass.Signs[k] == 0)
                s = " = ";
            else if (StatClass.Signs[k] == 1)
                s = " <= ";
            else if (StatClass.Signs[k] == 2)
                s = " >= ";
            return s;
        }
        public  void Vvivod(double[,] mas,int m)
        {
            string s = "", s1 = "", s2 = "";
            int i = 0, j = 0;
            for (i = 0; i < StatClass.ypav; i++)
            {
                s = ""; s1 = ""; s2 = "";
                for (j = 0; j < m; j++)
                {
                    s = s + (" " + mas[i, j] + "*x" + Convert.ToString(j + 1));
                    if (j < m - 1)
                        if (mas[i, j + 1] >= 0 && j < m - 1)
                            s = s + " +";
                        else
                            s = s + " ";
                }
                s1 = Znak(i);
                s2 = Convert.ToString(StatClass.EquationRightSide[i]);
                s = s + s1 + s2;
               Otvet.Items.Add(s);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private static void Vedi(double[,] mas, double[,] znachen, double[,] znaki, double[,] fx)
        {
            int i = 0, j = 0,ij=0;
            double[] del,itog;
            double ni = 0;
            itog = new double[StatClass.perem];
            del = new double[StatClass.perem];
            for (i = 0; i < StatClass.ypav; i++)
            { 
                for (j = 0; j < m; j++)
                {
                    if (mas[i, j] > 0)
                        del[i] = znachen[0, i] / mas[i, j];
                    else
                        del[i] = 0;
                    
                }
                ni = del[0];
                for (ij = 0; ij < del.Length - 1; ij++)
                {
                    if (ni < del[ij] && del[ij]!=0)
                    {
                        ni = del[ij];
                       
                    }
                }
                itog[i] = ni*fx[0,i];
            }
        }



        //Добавление фиктивных пременных
        

        private void Add()
        {
            int tr = 0;
            int i = 0, j = 0;
            kol = 0;
            //Узнаем количество их в уравнении простым пересчетом знаков отличных от нуля(нуль был в комбобоксе при выборе равном)
            for (i = 0; i < StatClass.ypav; i++)
            {
                if (StatClass.Signs[i] != 0)
                {
                    kol++;
                }
            }
            //Если количество не равно нулю, добавляем фиктивные элементы
            if (kol != 0)
            {
                //получаем количество элементов вместе с фиктивными переменными и правой частью уравнений
                m = StatClass.perem + kol;
                //выделяем место под изначальное уравнение и правую часть
                solveArray = new double[StatClass.ypav+1, m+1];
                
                //копируем изначальный массив в новый 
                for (i = 0; i < StatClass.ypav; i++)
                {
                    for (j = 0; j < StatClass.perem; j++)
                    {
                        solveArray[i, j] = mass[i, j];
                    }
                }
                //копируем изначальное уравнение
                for (j = 0; j < StatClass.perem; j++)
                {
                    solveArray[StatClass.ypav, j] = StatClass.Fx[j];
                }

                //подставляем фиктивные столбцы в изначальное уравнение
                for (j = StatClass.perem; j < m; j++)
                {
                    solveArray[StatClass.ypav, j] = 0;
                }

                //Узнаем и добавляем в нужные места единицы с определенными знаками
                for (i = 0; i < StatClass.ypav; i++)
                {
                    for (j = StatClass.perem; j < m; j++)
                    {
                        if (StatClass.Signs[i] != 0 && tr < j)
                        {
                            if (StatClass.Signs[i] == 2)
                                solveArray[i, j] = -1;
                            else
                                solveArray[i, j] = 1;
                            tr = j;
                            StatClass.Signs[i] = 0;
                            break;
                        }
                    }
                }
                //подставляем правую часть уравнения для удобства дальнейших вычислений
               for (i = 0; i< StatClass.ypav; i++)
                {
                    solveArray[i, m] = StatClass.EquationRightSide[i];
                }
            }
            else
            {
                m = StatClass.perem; 
                 //учитываем изначальную функцию и правую часть
                solveArray = new double[StatClass.ypav+1, m+1];
                //копируем массив переменных уравнений для упрощения дальнейших вычислений
                for (i = 0; i < StatClass.ypav; i++)
                {
                    for (j = 0; j < StatClass.perem; j++)
                    {
                        solveArray[i, j] = mass[i, j];
                    }
                }
                //добавляем правую часть
                for (i = 0; i< StatClass.ypav; i++)
                {
                    solveArray[i, m] = StatClass.EquationRightSide[i];
                }
       
                for (j = 0; j < m; j++)
                {
                    solveArray[StatClass.ypav, j] = StatClass.Fx[j];
                }
            }

           
        }

        //Ищем ведущий элемент
        private void LeadingSearch(double [,] data, out int leadi, out int leadj) //поиск коэффикиентов ведущего элемента
        {
            int n;
            int li, lj;
            double [] MinSearcher = new double [m];
            int[] iSaver = new int[m];
            StatClass.correctColumnsChecked = new bool[StatClass.ypav];
            bool need = CheckSolution(data);
            for (int j = 0; j < m; j++)
            {
                if (data[StatClass.ypav, j] > 0) //если нужно избавляться от положительного коэффициента исходной функции
                {
                    for (n = 0; n < StatClass.ypav; n++)
                        if (data[n, j] > 0 && (!StatClass.correctColumnsChecked[n])) //ищем первый подходящий элемент
                        {
                            MinSearcher[j] = data[n, m] / data[n, j];
                            iSaver[j] = n;
                            break;
                        }
                   
                    for (int i = n; i < StatClass.ypav; i++)
                    {
             
                        if (data[i, j] > 0 && ((double) data[i, m] / data[i, j] < MinSearcher[j]) && (!StatClass.correctColumnsChecked[n])) //если элемент больше нуля, а правая часть, деленная на него, меньше текущей записанной
                        {
                            MinSearcher[j] = data[i, m] / data[i, j]; //записываем минимальный
                            iSaver[j] = i; //сохраняем вертикальный индекс элемента
                        }
                    }
                    MinSearcher[j] *= data[StatClass.ypav, j]; //домножаем на коэф. исходной функции
                }
                else
                    MinSearcher[j] = -1; //если в данном столбце поиск ведущего не нужен, записываем -1             
            }
            //запсываем первый элемент как максимальный
            double max = MinSearcher[0];
            li = iSaver[0];
            lj = 0;
            for (int j = 1; j<m; j++) //ищем максимальное среди минимальных
            {
                if (MinSearcher[j] > max)
                {
                    max = MinSearcher[j];
                    li = iSaver[j];
                    lj = j;
                   
                }
            }
            leadi = li;
            leadj = lj;
            if (MinSearcher[lj] == -1) //если нет положительных чисел, но правильных столбцов недостаточно
            {
                //  MessageBox.Show("Не удалось найти ведущий элемент");
                iSaver = new int[m];
                for (int j = 0; j < m; j++)
                {
                    if (StatClass.CorrectUnosPositionKeeper[j] == -1) //если столбец неправильный
                    {
                        for (n = 0; n < StatClass.ypav; n++)
                            if ((data[n, j] > 0) && (!StatClass.correctColumnsChecked[n])) //доп. проверка на отсутствие правильного столбца в этой строке
                            {
                                MinSearcher[j] = data[n, m] / data[n, j];
                                iSaver[j] = n;
                                break;
                            }

                        for (int i = n; i < StatClass.ypav; i++)
                        {

                            if (data[i, j] > 0 && ((double)data[i, m] / data[i, j] < MinSearcher[j]) && (!StatClass.correctColumnsChecked[n])) //если элемент больше нуля, а правая часть, деленная на него, меньше текущей записанной + нет правильного столбца для этой строки
                            {
                                MinSearcher[j] = data[i, m] / data[i, j]; //записываем минимальный
                                iSaver[j] = i; //сохраняем вертикальный индекс элемента
                            }
                        }
                        // MinSearcher[j] *= data[StatClass.ypav, j]; //домножаем на коэф. исходной функции (не требуется при поиске правильных столбцов, тк тогда внизу все числа отрицательные)
                    }
                    else MinSearcher[j] = -1;
                
                }
                 max = MinSearcher[0];
                li = iSaver[0];
                lj = 0;
                for (int j = 1; j < m; j++) //ищем максимальное среди минимальных
                {
                    if (MinSearcher[j] > max)
                    {
                        max = MinSearcher[j];
                        li = iSaver[j];
                        lj = j;

                    }
                }
                if (MinSearcher[lj] != -1)
                {
                    leadi = li;
                    leadj = lj;
                    Otvet.Items.Add("Индекс ведущего элемента: " + leadi + " - " + leadj);
                   
                    return;
                }
                else //проведем поиск ведущих в любфх неправильных столбцах
                {
                    iSaver = new int[m];
                    for (int j = 0; j < m; j++)
                    {
                        if (StatClass.CorrectUnosPositionKeeper[j] == -1) //если столбец неправильный
                        {
                            for (n = 0; n < StatClass.ypav; n++)
                                if ((data[n, j] > 0)) //доп. проверка на отсутствие правильного столбца в этой строке
                                {
                                    MinSearcher[j] = data[n, m] / data[n, j];
                                    iSaver[j] = n;
                                    break;
                                }

                            for (int i = n; i < StatClass.ypav; i++)
                            {

                                if (data[i, j] > 0 && ((double)data[i, m] / data[i, j] < MinSearcher[j])) //если элемент больше нуля, а правая часть, деленная на него, меньше текущей записанной + нет правильного столбца для этой строки
                                {
                                    MinSearcher[j] = data[i, m] / data[i, j]; //записываем минимальный
                                    iSaver[j] = i; //сохраняем вертикальный индекс элемента
                                }
                            }
                            // MinSearcher[j] *= data[StatClass.ypav, j]; //домножаем на коэф. исходной функции (не требуется при поиске правильных столбцов, тк тогда внизу все числа отрицательные)
                        }
                        else MinSearcher[j] = -1;

                    }
                    max = MinSearcher[0];
                    li = iSaver[0];
                    lj = 0;
                    for (int j = 1; j < m; j++) //ищем максимальное среди минимальных
                    {
                        if (MinSearcher[j] > max)
                        {
                            max = MinSearcher[j];
                            li = iSaver[j];
                            lj = j;

                        }
                    }
                    leadi = li;
                    leadj = lj;
                    Otvet.Items.Add("Индекс ведущего элемента: " + leadi + " - " + leadj);              
                }
                

            }
            else Otvet.Items.Add("Индекс ведущего элемента: " + leadi + " - " + leadj);

        }

        //трансформация матрицы вокруг ведущего элемента
        private void Transform(double [,] data, int leadi, int leadj) //передаем индексы ведущего элемента
        {
            double[,] TemporaryData = new double[StatClass.ypav+1, m+1];
                Array.Copy(data, TemporaryData, data.Length);    //сохр. данные исходной матрицы для вычислений

            for (int i = 0; i <= StatClass.ypav; i++) //включаем в трансформацию исходное уравнение
                for (int j = 0; j <= m; j++) //включаем в трансформацию правую сторону
                {
                    if (!(i == StatClass.ypav && j == m))
                    {
                        if (i == leadi) //если элемент в одной строке с ведущим + cам ведущий
                        {
                            data[i, j] = TemporaryData[i, j]/ TemporaryData[leadi, leadj];
                          
                        }
                        else if (j == leadj) //если элемент в одном столбце с ведущим
                        {
                            data[i, j] = 0;
                        }
                        else
                        {
                            data[i, j] = (TemporaryData[i, j] * TemporaryData[leadi, leadj] - TemporaryData[leadi, j] * TemporaryData[i, leadj])/ TemporaryData[leadi, leadj]; //как удобно, что диагональ с ведущим элементом всегда главная
                        }
                    }
                    else
                        data[i, j] = TemporaryData[i, j] - (TemporaryData[leadi, j] * TemporaryData[i, leadj] / TemporaryData[leadi, leadj]); //пытаемся учитывать f
                }
            //выводим преобразованную матрицу
            Otvet.Items.Add("Преобразованная вокруг ведущего элемента матрица:");

            for (int i= 0; i<=StatClass.ypav; i++)
            {
                StringBuilder s = new StringBuilder();
                for (int j = 0; j<=m; j++)
                {
                    s.Append(String.Format("{0, 5:0.0000}", data[i, j]) + " ");
                }           
                Otvet.Items.Add(s.ToString());
            }            
        }


        private bool CheckSolution(double[,] data) //проверяем преобразованную матрицу на соответствие критериям
        {
            for (int j = 0; j<m; j++) //сразу проверяем коэф. исходного уравнения
            {
                if (data[StatClass.ypav, j] > 0)
                    return false;
            }
            bool CorrectAmountOfUnos;
            StatClass.correctColumnsChecked = new bool[StatClass.ypav];
            int CorrectColumnCounter = 0;
            StatClass.CorrectUnosPositionKeeper = new int[m];
            for (int j = 0; j < m; j++) //проверяем правильные столбцы
            {
                CorrectAmountOfUnos = true;

                for (int i = 0; i < StatClass.ypav; i++)
                {
                    if (((data[i, j] != 0) && (data[i, j] != 1))) //если не 1 и 0, то столбец неправильный
                    {
                        StatClass.CorrectUnosPositionKeeper[j] = -1; //неправильные столбцы помечаем как -1 
                        break;
                    }
                    if ((data[i, j] == 1))
                    {
                        if (CorrectAmountOfUnos) //проверка на то, что единица уже была в этом стообце
                        {
                            StatClass.CorrectUnosPositionKeeper[j] = i; //правильные столбцы помечаем позицией 1
                            CorrectAmountOfUnos = false; //повторная единица сделает столбец неправильным
                        }
                        else
                        {
                            StatClass.CorrectUnosPositionKeeper[j] = -1;
                            break;
                        }
                    }
                   
                }
            }
            for (int j = 0; j < m; j++)
            {
                if (StatClass.CorrectUnosPositionKeeper[j] != -1)
                {
                    CorrectColumnCounter++;
                   
                    StatClass.correctColumnsChecked[StatClass.CorrectUnosPositionKeeper[j]] = true;
                   
                }
            }
            if (CorrectColumnCounter >= StatClass.ypav)
            {
                for (int i = 0; i < m; i++)
                {
                    //if (StatClass.CorrectUnosPositionKeeper[i]!=-1)
                    //if ((StatClass.correctColumnsChecked[StatClass.CorrectUnosPositionKeeper[i]]))
                    //        return false;
                }
                StringBuilder s = new StringBuilder();
                Otvet.Items.Add("Ответ: ");
                int n;
                for (int i = 0; i < m; i++)
                {
                    n = i + 1;
                    if ((StatClass.CorrectUnosPositionKeeper[i] != -1) && (i < StatClass.perem))
                    {
                        s.Append("x" + n + " = " + data[StatClass.CorrectUnosPositionKeeper[i], m] + "\n");
                    }
                    else if (i < StatClass.perem)
                            {
                        s.Append("x" + n + " = " + 0 + "\n");
                    }

                }
                Otvet.Items.Add(s.ToString());
            }
            else return false;
                        return true;
        }
    }
}
