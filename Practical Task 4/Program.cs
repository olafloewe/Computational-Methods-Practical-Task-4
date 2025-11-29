using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practical_Task_4 {


    //  made this..... no idea how to make it work in a 2d array and go back and forth between double and this ¯\_(ツ)_/¯
    
    /*
    // fraction class to increase precision and get rid of rounding errors
    internal class Fraction{
        private int numerator;
        private int denominator;
        public Fraction(int numerator, int denominator){
            if (denominator == 0) throw new DivideByZeroException("Denominator cannot be zero.");
            this.numerator = numerator;
            this.denominator = denominator;
        }

        // single operand plus
        public static Fraction operator +(Fraction a){
            return a;
        }

        // single operand minus
        public static Fraction operator -(Fraction a){
            return -a;
        }

        // double operand plus
        public static Fraction operator +(Fraction a, Fraction b){
            return new Fraction((a.numerator * b.denominator) + (b.numerator * a.denominator), a.denominator * b.denominator);
        }

        // double operand minus
        public static Fraction operator -(Fraction a, Fraction b){
            return a + (-b);
        }

        // double operand multiply
        public static Fraction operator *(Fraction a, Fraction b){
            return new Fraction(a.numerator * b.numerator, a.denominator * b.denominator);
        }

        // double operand division
        public static Fraction operator /(Fraction a, Fraction b){
            if (b.numerator == 0) throw new DivideByZeroException("Cannot divide by zero fraction.");
            return new Fraction(a.numerator * b.denominator, b.numerator * a.denominator);
        }

        // override default ToString method
        public override string ToString(){
            return $"{numerator}/{denominator}";
        }
    }
    */


    internal class Program {

        // Prints the matrix to the console
        public static void PrintMatrix(double[,] S){
            for (int i = 0; i < S.GetLength(0); i++){
                for (int j = 0; j < S.GetLength(1); j++){
                    Console.Write($"{S[i, j]} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        // Swaps rows of a matrix in memory reference
        public static void SwapRows(double[,] S, int target, int destination){
            // guard clause
            int height = S.GetLength(0);
            if (target >= height || destination >= height || target < 0 || destination < 0) throw new IndexOutOfRangeException("Row index out of range.");
            double[] tmp = new double[height];

            // swap rows
            for (int i = 0; i < height; i++) {
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
            int width = S.GetLength(1);
            if (target > width || addition > width || target < 0 || addition < 0) throw new IndexOutOfRangeException("Row index out of range.");

            // add rows
            for (int i = 0; i < width; i++){
                S[target, i] += S[addition,i]; // add element
            }
        }

        // Adds two rows of a matrix in memory reference
        public static void AddRows(double[,] S, int target, double[] addition){
            // guard clause
            int width = S.GetLength(1);
            if (target > width || target < 0 || addition.Length != width) throw new IndexOutOfRangeException("Row index out of range.");

            // add rows
            for (int i = 0; i < width; i++){
                S[target, i] += addition[i]; // add element
            }
        }

        // sorts the matrix into row echelon form
        public static void RowEchelonForm(double[,] S){
            // int row = 0;
            int index = 1;
            for(int row = 0; row < S.GetLength(0); row++) {
                while (S[row, row] == 0) SwapRows(S, row, index++);
                row++;
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
            int height = S.GetLength(0);
            int width = S.GetLength(1);

            // fewer equations than variables => infinitely many solutions
            if (width + 1 < height) return new double[0];
            int row = 0;

            do{
                // row echelon form
                if (S[row,row] == 0) RowEchelonForm(S);

                try {
                    ScaleRow(S, row, 1 / S[row, row]); // scale to one
                }
                catch (Exception e){
                    return new double[0]; // no solution
                }
                
                // ScaleRow(S, row, -1); // scale to negative coefficient
                for(int i = 0; i < height; i++){
                    if (i == row) continue; // dont eliminate self
                    if (S[i, row] == 0) continue; // skip zero coefficients
                    // Console.WriteLine($"Eliminating row {i} using row {row} with scale tmp {-S[i, row]}");
                    double[] tmpRow = new double[width];

                    double tmp = 1 / -S[i, row]; // store scale to revert later
                    // ScaleRow(S, row, -S[i, row]); // coefficient of rows above / below 

                    for (int j = 0; j < width; j++){
                        tmpRow[j] = S[row, j] * -S[i, row]; // copy and scale row
                    }

                    AddRows(S, i, tmpRow); // eliminate above / below
                    // ScaleRow(S, row, tmp); // scale back

                    // PrintMatrix(S);
                }
                
                row++;
            } while (row < height);

            double[] solution = new double[height];

            for (int i = 0; i < height; i++){
                solution[i] = S[i, width - 1]; // constant column
            }

            return solution;
        }

        private static void PrintSolution(double[] solution){
            // print array solution with formating
            Console.Write("Solution:\n{ ");
            for (int i = 0; i < solution.Length; i++){
                Console.Write($"x{i}={solution[i]}, ");
            }
            Console.WriteLine("}");
        }

        public static void Main(string[] args) {
            double[,] augmentedMatrix = new double[,] {
                { 1, 1, 2 },
                { 2, 2, 4 }
            };

            Console.WriteLine("Matrix: ");
            PrintMatrix(augmentedMatrix);

            // solve system
            PrintSolution(SystemSolve(augmentedMatrix));

            // HOLD THE LINE (CMD prompt) !!!
            Console.ReadKey();
        }  
    }
}