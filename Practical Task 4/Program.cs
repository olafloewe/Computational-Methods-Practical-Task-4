using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practical_Task_4 {
    internal class Program {

        // Swaps rows of a matrix in memory reference
        public static void SwapRow(double[,] S, int target, int destination){
            // guard clause
            if (target > S.GetLength(0) || destination > S.GetLength(0) || target < 0 || destination < 0) throw new IndexOutOfRangeException("Row index out of range.");

            int width = S.GetLength(1);
            double[] tmp = new double[width];

            // copy destination row to tmp
            for (int i = 0; i < width; i++) {
                tmp[i] = S[destination, i]; // evacuate destination row
                S[destination, i] = S[target, i]; // write target row
                S[target, i] = tmp[i]; // save destination row
            }
        }

        // Prints the matrix to the console
        public static void PrintMatrix(double[,] S){
            for (int i = 0; i < S.GetLength(0); i++){
                for (int j = 0; j < S.GetLength(1); j++){
                    Console.Write($"{S[i, j]} ");
                }
                Console.WriteLine();
            }
        }

        // Scales row of a matrix in given reference
        public static void ScaleRow(double[,] S, int target, double scale){
            // guard clause
            if (target > S.GetLength(0) || target < 0 ) throw new IndexOutOfRangeException("Row index out of range.");
            if(scale == double.NaN || double.IsInfinity(scale)) throw new ArgumentException("Scale must be a valid number.");

            int width = S.GetLength(1);

            // copy destination row to tmp
            for (int i = 0; i < width; i++){
                S[target, i] = S[target, i] * scale; // scaled element
            }
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
        public static double[] SystemSolve(double[,] S){



            return new double[] { };
        }

        public static void Main(string[] args) {
            // height = Getlength(0)
            // width = Getlength(1)

            double[,] augmentedMatrix = new double[,] {
                { 0, 4, 2, -2 },
                { -2, 3, 1, -7 },
                { 4, 5, 2, 4 }
            };

            PrintMatrix(augmentedMatrix);
            Console.WriteLine();
            SwapRow(augmentedMatrix, 0, 2);
            PrintMatrix(augmentedMatrix);
            Console.WriteLine();
            ScaleRow(augmentedMatrix, 0, -2.3345);
            PrintMatrix(augmentedMatrix);

            SystemSolve(augmentedMatrix);

            // HOLD THE LINE (CMD prompt) !!!
            Console.ReadKey();
        }
    }
}