using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace SudokuGame
{

    public partial class GamePar : Window
    {
                private int[,] sudokuBoard;

                public GamePar(int[,] Board)
        {
            InitializeComponent();
            this.sudokuBoard = Board;
            Stopwatch sp = Stopwatch.StartNew();
            sudoKosolver(sudokuBoard);
            sp.Stop();
            if (check())
            {
                this.time.Content = sp.ElapsedMilliseconds;
                assign(sudokuBoard);
            }
            else
            {
                MessageBox.Show("NO Solution Found");
            }
        }
        private void btnpMain_Click_1(object sender, RoutedEventArgs e)
        {
            var newForm = new MainWindow(); //create your new form.
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
        public static void existInRow(int[,] board, Dictionary<int, int> dictPoss, int row)
        {
            for (int c = 0; c < 9; c++)
            {
                // if here's a number in the same row , assign 1 to key , key will equal that number dicPos[5]=1 means the num 5 is exist in the same row
                if (board[row, c] != 0)
                    dictPoss[board[row, c]] = 1;
            }
        }
        public static void existInCol(int[,] board, Dictionary<int, int> dictPoss, int col)
        {

            for (int r = 0; r < 9; r++)
            {
                // store the existed numbers in the same col
                if (board[r, col] != 0)
                    dictPoss[board[r, col]] = 1;
            }
        }
        public static void existInSqr(int[,] board, Dictionary<int, int> dictPoss, int row, int col)
        {

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
        }
        public static void possibleNumbers(Dictionary<int, int> dictPoss)
        {
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
        }
        public static Dictionary<int, int> possiableEntries(int[,] board, int row, int col)
        {
            Dictionary<int, int> dictPoss = new Dictionary<int, int>();

            // row , col : position of the item in the board 

            for (int r = 1; r < 10; r++)
            {
                // initilize possible list with zeros
                dictPoss.Add(r, 0);
            }

            Task exists = new Task(() =>
            {
                new Task(() => existInRow(board, dictPoss, row), TaskCreationOptions.AttachedToParent).Start();
                new Task(() => existInCol(board, dictPoss, col), TaskCreationOptions.AttachedToParent).Start();
                new Task(() => existInSqr(board, dictPoss, row, col), TaskCreationOptions.AttachedToParent).Start();
            });
            exists.Start();
            Task possible = exists.ContinueWith(t => possibleNumbers(dictPoss));
            possible.Wait();

            //Task R = Task.Factory.StartNew(() => existInRow(board, dictPoss, row));
            //Task C = Task.Factory.StartNew(() => existInCol(board, dictPoss, col));
            //Task S = Task.Factory.StartNew(() => existInSqr(board, dictPoss, row, col));
            //Task.WaitAll(R, C, S);
            //Task P = Task.Factory.StartNew(() => possibleNumbers(dictPoss));
            //P.Wait();

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
        public Boolean check()
        {
            for (int jj = 0; jj < 9; jj++)
            {
                for (int ii = 0; ii < 9; ii++)
                {
                    if (sudokuBoard[jj, ii] == 0)
                        return false;
                }
            }

            return true;
        }

        private void assign(int[,] board)
        {
            label1.Content = board[0, 0]; label2.Content = board[0, 1]; label3.Content = board[0, 2]; label4.Content = board[0, 3];
            label5.Content = board[0, 4]; label6.Content = board[0, 5]; label7.Content = board[0, 6]; label8.Content = board[0, 7];
            label9.Content = board[0, 8];

            label10.Content = board[1, 0]; label11.Content = board[1, 1]; label12.Content = board[1, 2]; label13.Content = board[1, 3];
            label14.Content = board[1, 4]; label15.Content = board[1, 5]; label16.Content = board[1, 6]; label17.Content = board[1, 7];
            label18.Content = board[1, 8];

            label19.Content = board[2, 0]; label20.Content = board[2, 1]; label21.Content = board[2, 2]; label22.Content = board[2, 3];
            label23.Content = board[2, 4]; label24.Content = board[2, 5]; label25.Content = board[2, 6]; label26.Content = board[2, 7];
            label27.Content = board[2, 8];

            label28.Content = board[3, 0]; label29.Content = board[3, 1]; label30.Content = board[3, 2]; label31.Content = board[3, 3];
            label32.Content = board[3, 4]; label33.Content = board[3, 5]; label34.Content = board[3, 6]; label35.Content = board[3, 7];
            label36.Content = board[3, 8];

            label37.Content = board[4, 0]; label38.Content = board[4, 1]; label39.Content = board[4, 2]; label40.Content = board[4, 3];
            label41.Content = board[4, 4]; label42.Content = board[4, 5]; label43.Content = board[4, 6]; label44.Content = board[4, 7];
            label45.Content = board[4, 8];

            label46.Content = board[5, 0]; label47.Content = board[5, 1]; label48.Content = board[5, 2]; label49.Content = board[5, 3];
            label50.Content = board[5, 4]; label51.Content = board[5, 5]; label52.Content = board[5, 6]; label53.Content = board[5, 7];
            label54.Content = board[5, 8];

            label55.Content = board[6, 0]; label56.Content = board[6, 1]; label57.Content = board[6, 2]; label58.Content = board[6, 3];
            label59.Content = board[6, 4]; label60.Content = board[6, 5]; label61.Content = board[6, 6]; label62.Content = board[6, 7];
            label63.Content = board[6, 8];

            label64.Content = board[7, 0]; label65.Content = board[7, 1]; label66.Content = board[7, 2]; label67.Content = board[7, 3];
            label68.Content = board[7, 4]; label69.Content = board[7, 5]; label70.Content = board[7, 6]; label71.Content = board[7, 7];
            label72.Content = board[7, 8];

            label73.Content = board[8, 0]; label74.Content = board[8, 1]; label75.Content = board[8, 2]; label76.Content = board[8, 3];
            label77.Content = board[8, 4]; label78.Content = board[8, 5]; label79.Content = board[8, 6]; label80.Content = board[8, 7];
            label81.Content = board[8, 8];
        }





    }
}
