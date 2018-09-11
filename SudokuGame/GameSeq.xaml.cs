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
    public partial class GameSeq : Window
    {
        private int[,] sudokuBoard;



        public GameSeq(int[,] Board)
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

        private void btnMain_Click(object sender, RoutedEventArgs e)
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
            labelp1.Content = board[0, 0]; labelp2.Content = board[0, 1]; labelp3.Content = board[0, 2]; labelp4.Content = board[0, 3];
            labelp5.Content = board[0, 4]; labelp6.Content = board[0, 5]; labelp7.Content = board[0, 6]; labelp8.Content = board[0, 7];
            labelp9.Content = board[0, 8];

            labelp10.Content = board[1, 0]; labelp11.Content = board[1, 1]; labelp12.Content = board[1, 2]; labelp13.Content = board[1, 3];
            labelp14.Content = board[1, 4]; labelp15.Content = board[1, 5]; labelp16.Content = board[1, 6]; labelp17.Content = board[1, 7];
            labelp18.Content = board[1, 8];

            labelp19.Content = board[2, 0]; labelp20.Content = board[2, 1]; labelp21.Content = board[2, 2]; labelp22.Content = board[2, 3];
            labelp23.Content = board[2, 4]; labelp24.Content = board[2, 5]; labelp25.Content = board[2, 6]; labelp26.Content = board[2, 7];
            labelp27.Content = board[2, 8];

            labelp28.Content = board[3, 0]; labelp29.Content = board[3, 1]; labelp30.Content = board[3, 2]; labelp31.Content = board[3, 3];
            labelp32.Content = board[3, 4]; labelp33.Content = board[3, 5]; labelp34.Content = board[3, 6]; labelp35.Content = board[3, 7];
            labelp36.Content = board[3, 8];

            labelp37.Content = board[4, 0]; labelp38.Content = board[4, 1]; labelp39.Content = board[4, 2]; labelp40.Content = board[4, 3];
            labelp41.Content = board[4, 4]; labelp42.Content = board[4, 5]; labelp43.Content = board[4, 6]; labelp44.Content = board[4, 7];
            labelp45.Content = board[4, 8];

            labelp46.Content = board[5, 0]; labelp47.Content = board[5, 1]; labelp48.Content = board[5, 2]; labelp49.Content = board[5, 3];
            labelp50.Content = board[5, 4]; labelp51.Content = board[5, 5]; labelp52.Content = board[5, 6]; labelp53.Content = board[5, 7];
            labelp54.Content = board[5, 8];

            labelp55.Content = board[6, 0]; labelp56.Content = board[6, 1]; labelp57.Content = board[6, 2]; labelp58.Content = board[6, 3];
            labelp59.Content = board[6, 4]; labelp60.Content = board[6, 5]; labelp61.Content = board[6, 6]; labelp62.Content = board[6, 7];
            labelp63.Content = board[6, 8];

            labelp64.Content = board[7, 0]; labelp65.Content = board[7, 1]; labelp66.Content = board[7, 2]; labelp67.Content = board[7, 3];
            labelp68.Content = board[7, 4]; labelp69.Content = board[7, 5]; labelp70.Content = board[7, 6]; labelp71.Content = board[7, 7];
            labelp72.Content = board[7, 8];

            labelp73.Content = board[8, 0]; labelp74.Content = board[8, 1]; labelp75.Content = board[8, 2]; labelp76.Content = board[8, 3];
            labelp77.Content = board[8, 4]; labelp78.Content = board[8, 5]; labelp79.Content = board[8, 6]; labelp80.Content = board[8, 7];
            labelp81.Content = board[8, 8];
        }




    }
}
