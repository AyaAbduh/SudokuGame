using System;
using System.Collections.Generic;
using System.IO;
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

namespace SudokuGame
{
    public partial class MainWindow : Window
    {
        private static int[,] SudokuBoard = new int[9, 9];
        private static string FilePath = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "TEXT Files (*.txt)|*.txt";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                FilePath = dlg.FileName;
                getBoard();
            }
        }

        private void btnStartSeq_Click(object sender, RoutedEventArgs e)
        {
                var newForm = new GameSeq(SudokuBoard); //create your new form.
                newForm.Show(); //show the new form.
                this.Close(); //only if you want to close the current form1
        }

        private void btnStartPar_Click(object sender, RoutedEventArgs e)
        {
            var newForm = new GamePar(SudokuBoard); //create your new form.
            newForm.Show(); //show the new form.
            this.Close(); //only if you want to close the current form
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------
        public static Boolean isFull(int[,] board)
        {

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i, j] == 0)
                        return false;
                }


            }
            return true; // true if there's no any empty place in the board

        }
        public static Dictionary<int, int> possiableEntries(int[,] board, int row, int col)
        {
            Dictionary<int, int> dictPoss = new Dictionary<int, int>();

            for (int r = 1; r < 10; r++)
            {
                // initilize possible list with zeros
                dictPoss.Add(r, 0);
            }

            for (int c = 0; c < 9; c++)
            {
                // if here's a number in the same row , assign 1 to key , key will equal that number dicPos[5]=1 // means the num 5 is exist in the same row
                if (board[row, c] != 0)
                    dictPoss[board[row, c]] = 1;
            }


            for (int r = 0; r < 9; r++)
            {
                // store the existed numbers in the same col
                if (board[r, col] != 0)
                    dictPoss[board[r, col]] = 1;
            }

            int currentSrow = 0;
            int currentScol = 0;

            if (row >= 0 && row <= 2)
            {
                currentSrow = 0;
            }
            else
            {
                if (row >= 3 && row <= 5)
                {
                    currentSrow = 3;
                }
                else
                {

                    currentSrow = 6;

                }
            }

            ////////////////
            if (col >= 0 && col <= 2)
            {
                currentScol = 0;
            }
            else
            {
                if (col >= 3 && col <= 5)
                {
                    currentScol = 3;
                }
                else
                {
                    currentScol = 6;
                }
            }

            ////////////
            for (int i = currentSrow; i < currentSrow + 3; i++)
            {
                for (int j = currentScol; j < currentScol + 3; j++)
                {// store the existed numbers in the same squre : 3*3
                    if (board[i, j] != 0)
                        dictPoss[board[i, j]] = 1;
                }
            }


            for (int i = 1; i < 10; i++)
            {
                // reserve dictionry , return possible numbers that can be used for playing 
                if (dictPoss[i] == 0) // if dic[5] == 0 : no row or col or same squre use it ,, -> dispos[5]=5
                {
                    dictPoss[i] = i;
                }
                else // this key is used 
                    dictPoss[i] = 0;
            }


            return dictPoss;

        }
        public static bool sudoKosolver(int[,] board)
        {
            int i = 0, j = 0;
            Dictionary<int, int> posibility;

            if (isFull(board)) // base case : print and exit if the board if full 
            {
                //Console.WriteLine("Doneee");
                return true;
            }
            else
            {
                //Console.WriteLine("not yet");
                for (int i2 = 0; i2 < 9; i2++)
                {
                    for (int j2 = 0; j2 < 9; j2++)
                    {
                        if (board[i2, j2] == 0)
                        {
                            i = i2;
                            j = j2;
                            break;
                        }
                    }
                }
                // get all possibilities 
                posibility = possiableEntries(board, i, j);
                for (int c = 1; c < 10; c++)
                {
                    if (posibility[c] != 0)
                    {
                        board[i, j] = posibility[c];
                        if (sudoKosolver(board))
                            return true;
                        board[i, j] = 0;
                    }
                }
            }
            return false;
        }
        public static void getBoard()
        {
            int r = 0;
            try
            {
                foreach (string line in File.ReadLines(@FilePath))
                {
                    String line2 = line.Replace('*', '0');
                    for (int i = 0; i < line2.Length; i++)
                    {
                        SudokuBoard[r, i] = int.Parse("" + line2[i]);
                    }
                    if (r < 9)
                        r += 1;
                }
                //Console.WriteLine(board[0,4]);
            }
            catch (IOException UAEx)
            {
                Console.WriteLine(UAEx.Message);
            }
        }
        public static Boolean check()
        {
            for (int jj = 0; jj < 9; jj++)
            {
                for (int ii = 0; ii < 9; ii++)
                {
                    if (SudokuBoard[jj, ii] == 0)
                        return false;
                }
            }

            return true;
        }


    }
}
