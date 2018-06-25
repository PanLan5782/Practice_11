using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_11
{
        class Program
        {
            const int NN = 10;

            private static int[] ReadValue(out bool ok, string data) 
            {
                ok = true;
                int[] digits = new int[data.Length];
                for (int i = 0; i < data.Length; i++)
                    digits[i] = Convert.ToInt32(data[i]) - 48;
                for (int i = 0; i < digits.Length; i++)
                    if ((digits[i] != 0) && (digits[i] != 1))
                        ok = false;
                return digits;
            }

            static int[][] ReadMatrix(StreamReader inputMatrix, out bool check) 
            {
                int[][] M = new int[NN][];
                check = true;


                for (int row = 0; row < NN && check; row++)
                {
                    string data = inputMatrix.ReadLine();
                    M[row] = ReadValue(out check, data); 


                    if (M[row].Length != NN)
                    {
                        check = false;
                    }
                }


                if (check)
                {
                    check = CheckMatrix(M);

                    if (!check)
                        Console.WriteLine("Матрица не может быть ключом");
                }

                inputMatrix.Close();
                return M;
            }

            static bool CheckMatrix(int[][] matrix) 
            {
                if (CheckMatrixValues(matrix))
                {

                    for (int row = 0; row < NN / 2; row++)
                        for (int column = 0; column < NN / 2; column++)
                        {
                            if (matrix[row][column] + matrix[column][NN - 1 - row] 
                                + matrix[NN - 1 - row][NN - 1 - column]         
                                + matrix[NN - 1 - column][row] != 3)               
                            {
                                return false;
                            }
                        }

                }
                else
                {
                    return false;
                }

                return true;
            }

            static bool CheckMatrixValues(int[][] matrix) 
            {
                bool okValues = true;

                for (int row = 0; row < NN; row++)
                {
                    for (int column = 0; column < NN; column++)
                        if (matrix[row][column] != 0 && matrix[row][column] != 1)
                        {
                            okValues = false;
                            break;
                        }

                    if (!okValues)
                        break;
                }

                return okValues;
            }

            static string read(StreamReader inputString, out bool ok) 
            {
                string symbols = string.Empty;
                ok = true;


                    symbols = inputString.ReadLine();
                    inputString.Close();


                if (symbols == null || ok && symbols.Length != 100)
                {
                    ok = false;
                }

                return symbols;
            }

            static string Encrypt(int[][] M, string symbols) 
            {
                int currentSymbol = 0;
                string result = string.Empty;

                char[][] cryptMatrix = new char[NN][];

                for (int row = 0; row < NN; row++)
                    cryptMatrix[row] = new char[NN];

                for (int turn = 1; turn <= 4; turn++)
                {
                    
                    for (int row = 0; row < NN; row++)
                        for (int column = 0; column < NN; column++)
                            if (M[row][column] == 0)
                            {
                                cryptMatrix[row][column] = symbols[currentSymbol];
                                currentSymbol++;
                            }

                    
                    TurnMatrix(ref M);
                }

                result = ReadEncryptResult(cryptMatrix);

                return result;
            }

            static void TurnMatrix(ref int[][] matrix) 
            {
                for (int row = 0; row < NN / 2; row++)
                    for (int column = 0; column < NN / 2; column++)
                    {
                        int temp = matrix[row][column];
                        matrix[row][column] = matrix[column][NN - 1 - row];
                        matrix[column][NN - 1 - row] = matrix[NN - 1 - row][NN - 1 - column];
                        matrix[NN - 1 - row][NN - 1 - column] = matrix[NN - 1 - column][row];
                        matrix[NN - 1 - column][row] = temp;
                    }
            }

            static string ReadEncryptResult(char[][] matrix) 
            {
                string result = string.Empty;

                for (int row = 0; row < NN; row++)
                    for (int column = 0; column < NN; column++)
                        result += matrix[row][column].ToString();

                return result;
            }

            static string Decipher(int[][] matrix, string code) 
            {
                char[][] codeMatrix = new char[NN][];
                int currentSymbol = 0;

                
                for (int row = 0; row < NN; row++)
                {
                    codeMatrix[row] = new char[NN];

                    for (int column = 0; column < NN; column++)
                    {
                        codeMatrix[row][column] = code[currentSymbol];
                        currentSymbol++;
                    }
                }

                string result = string.Empty;

                for (int turn = 1; turn <= 4; turn++)
                {
                    for (int row = 0; row < NN; row++)
                        for (int column = 0; column < NN; column++)
                            if (matrix[row][column] == 0)
                                result += codeMatrix[row][column].ToString();

                    
                    TurnMatrix(ref matrix);
                }

                return result;
            }

            static void Main(string[] args)
            {
                int[][] matrix = new int[0][];
                string symbols = string.Empty;
                bool ok;



                    using (StreamReader input = new StreamReader("matrix.txt"))
                    {
                        matrix = ReadMatrix(input, out ok);
                    }


                if (ok)
                {

                        using (StreamReader inputString = new StreamReader("string.txt"))
                            symbols = read(inputString, out ok);


                    if (ok)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        string cryptSymbols = Encrypt(matrix, symbols);
                        Console.WriteLine( cryptSymbols);

                        string decryptedSymbols = Decipher(matrix, cryptSymbols);
                        Console.WriteLine( decryptedSymbols);
                    }
                }


                Console.ReadLine();
            }

        }
    }
