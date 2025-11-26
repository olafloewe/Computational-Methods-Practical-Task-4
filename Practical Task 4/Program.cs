using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practical_Task_4 {
    internal class Program {

        // Prints the matrix to the console
        public static void PrintMatrix(double[,] S){
            for (int i = 0; i < S.GetLength(0); i++){
                for (int j = 0; j < S.GetLength(1); j++){
                    Console.Write($"{S[i, j]} ");
                }
                Console.WriteLine();
            }
        }

        // Swaps rows of a matrix in memory reference
        public static void SwapRows(double[,] S, int target, int destination){
            // guard clause
            if (target > S.GetLength(0) || destination > S.GetLength(0) || target < 0 || destination < 0) throw new IndexOutOfRangeException("Row index out of range.");

            int width = S.GetLength(1);
            double[] tmp = new double[width];

            // swap rows
            for (int i = 0; i < width; i++) {
                tmp[i] = S[destination, i]; // evacuate destination row
                S[destination, i] = S[target, i]; // write target row
                S[target, i] = tmp[i]; // save destination row
            }
        }

        // Scales row of a matrix in memory reference
        public static void ScaleRow(double[,] S, int target, double factor){
            // guard clause
            if (target > S.GetLength(0) || target < 0 ) throw new IndexOutOfRangeException("Row index out of range.");
            if(factor == double.NaN || double.IsInfinity(factor)) throw new ArgumentException("Scale must be a valid number.");

            int width = S.GetLength(1);

            // scale row by a factor
            for (int i = 0; i < width; i++){
                S[target, i] *= (double)factor; // scaled element
            }
        }

        // Adds two rows of a matrix in memory reference
        public static void AddRows(double[,] S, int target, int addition){
            // guard clause
            if (target > S.GetLength(0) || addition > S.GetLength(0) || target < 0 || addition < 0) throw new IndexOutOfRangeException("Row index out of range.");
            
            int width = S.GetLength(1);

            // add rows
            for (int i = 0; i < width; i++){
                S[target, i] += S[addition,i]; // add element
            }
        }

        // sorts the matrix into row echelon form
        public static void RowEchelonForm(double[,] S){
            int row = 0;
            int index = 1;
            do {
                while (S[row, row] == 0) SwapRows(S, row, index++);
                row++;
            } while (row < S.GetLength(0));
        }

        /*
        public static double[] SystemSolve(double[,] S) {
            Input:
                a two-dimensional array of real numbers (the augmented matrix)
            Output:
                a one-dimensional array - the solution (or an empty array if there are no or infinitely many solutions)
            Examples:
                SystemSolve({{0, 4, 2, -2}, {-2, 3, 1, -7}, {4, 5, 2, 4}})
                    -> {2, -2, 3}
                SystemSolve({{1, 3, 5}, {2, 6, 5}})
                    -> {}
                SystemSolve({{1, 3, 5}, {3, -2, 4}, {4, -1, 9}, {7, -3, 13}})
                    -> {2, 1}
        */
        public static double[,] SystemSolve(double[,] S){
            int height = S.GetLength(0);
            int width = S.GetLength(1);

            // fewer equations than variables => infinitely many solutions
            if (width + 1 < height) return new double[0,0];

            int limit = width - 1; // last column is constants
            int row = 0;

            do{
                // row echelon form
                if (S[row,row] == 0) RowEchelonForm(S);

                Console.WriteLine($"Pre {row}:");
                PrintMatrix(S);

                // scale to 1
                // ScaleRow(S, row, 1 / S[row, row]);
                
                // eliminate below and above
                for(int i = 0; i < limit; i++){
                    if (S[row, i] == 0 || i == row) continue; // no need to eliminate
                    ScaleRow(S, row, S[row, i]); // scale to negative coefficient

                    ScaleRow(S, row, -1); // scale to negative coefficient
                    AddRows(S, i, row); // add to eliminate

                    ScaleRow(S, row, -1); // scale to negative coefficient
                    ScaleRow(S, row, (double)1 / S[row, i]); // scale back
                }

                Console.WriteLine($"Post {row}:");
                PrintMatrix(S);
                Console.WriteLine();

                row++;
            } while (row < limit);

            // TODO ONE SOLUTION

            // TODO NO SOLUTION

            return S;
        }

        public static void Main(string[] args) {
            // height = GetLength(0)
            // width = GetLength(1)

            double[,] augmentedMatrix = new double[,] {
                { 1, 1, 1, 6 },
                { 2, -1, 1, 3 },
                { 1, 2, -1, 2 }
            };

            Console.WriteLine("Matrix");
            PrintMatrix(augmentedMatrix);
            Console.WriteLine();
            /*
            Console.WriteLine("Swap");
            SwapRows(augmentedMatrix, 0, 2);
            PrintMatrix(augmentedMatrix);
            Console.WriteLine();
            */
            Console.WriteLine("Scale");
            ScaleRow(augmentedMatrix, 0, 2);
            PrintMatrix(augmentedMatrix);
            Console.WriteLine();
            /*
            Console.WriteLine("Scale Back");
            ScaleRow(augmentedMatrix, 0, (double)1/2);
            PrintMatrix(augmentedMatrix);
            Console.WriteLine();
            */
            /*
            Console.WriteLine("Add");
            AddRows(augmentedMatrix, 2, 0);
            PrintMatrix(augmentedMatrix);
            Console.WriteLine();
            */
            
            // SystemSolve(augmentedMatrix);
            PrintMatrix(SystemSolve(augmentedMatrix));
            Console.WriteLine();
            
            // HOLD THE LINE (CMD prompt) !!!
            Console.ReadKey();
        }
    }
}