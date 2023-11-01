namespace Homework3
{
    /// <summary>
    /// Class representing the cipher which will be decrypted
    /// </summary>
    public class RouteCipher
    {
        #region Properties
        private int key;

        public int Key
        {
            get { return key; }
            set { key = value; }
        }
        #endregion

        #region Constructors
        public RouteCipher(int key)
        {
            Key = key;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Helper method used for putting the characters from a string into an appropriate matrix
        /// </summary>
        /// <param name="plainText">The text to be represented in matrix form</param>
        /// <returns>A tuple containing the matrix representation of the text, the rows and columns of the matrix</returns>
        private (char[][], int, int) FormMatrixFromString(string plainText)
        {
            string noSpaces = plainText.Replace(" ", "").Replace(",", "").ToUpper(); // remove whitespaces and commas in the text
            int columns = Math.Abs(Key); // get the columns from the key
            int rows = (noSpaces.Length + columns - 1) / columns; // take the ceil of integer division 

            char[][] chars = new char[rows][];
            // Putting the characters inisde a matrix
            for (int i = 0; i < rows; i++)
            {
                chars[i] = new char[columns];
                for (int j = 0; j < columns; j++)
                {
                    if (i * columns + j < noSpaces.Length)
                        chars[i][j] = noSpaces[i * columns + j]; // set the matrix with the plain text
                    else
                        chars[i][j] = 'X'; // if we have exhausted all characters in plainText then fill the matrix with Xes
                }
            }

            return (chars, rows, columns);
        }

        /// <summary>
        /// Text encryption method. Uses a spiraling matrix route to form the encrypted word
        /// </summary>
        /// <param name="plainText">The string to be encrypted</param>
        /// <returns>The encrypted string form</returns>
        public string Encrypt(string plainText)
        {
            string result = "";
            (char[][], int, int) data = FormMatrixFromString(plainText);

            if (Key > 0)
                GetTopLeft(data.Item1, 0, 0, data.Item3, data.Item2, ref result, true, 0);
            else
                GetBottomRight(data.Item1, 0, 0, data.Item3, data.Item2, ref result, true, 0);

            return result;
        }

        /// <summary>
        /// Helper function dedicated to traversing the matrix once from topleft to downleft then from downleft to downright
        /// </summary>
        /// <param name="a">The char matrix used to represent the route cipher</param>
        /// <param name="left_col">left most column index for the traversal of the matrix</param>
        /// <param name="up_row">up most row index</param>
        /// <param name="right_col">right most column index</param>
        /// <param name="down_row">down most row index</param>
        /// <param name="result">the resulting string which is passed by refference</param>
        private void GetTopLeft(char[][] a, int left_col, int up_row, int right_col, int down_row, ref string result, bool flag, int currChar)
        {
            int i = 0, j = 0;

            // print values in the column.
            for (j = up_row; j < down_row; j++)
            {
                if (flag == true)
                    result += a[j][left_col];
                else
                    a[j][left_col] = result[currChar++];
            }

            // print values in the row.
            for (i = left_col + 1; i < right_col; i++)
            {
                if (flag == true)
                    result += a[j - 1][i];
                else
                    a[j - 1][i] = result[currChar++];
            }


            // see if more layers need to be printed.
            if (right_col - left_col > 1)
            {
                // Since we have exhausted our left_most col and bottom_most row we increment left_col and down_row 
                GetBottomRight(a, left_col + 1, up_row, right_col, down_row - 1, ref result, flag, currChar);
            }

        }

        /// <summary>
        /// Helper function dedicated to traversing the matrix once from bottomright to upright then from upright to leftright
        /// </summary>
        /// <param name="a">The char matrix used to represent the route cipher</param>
        /// <param name="left_col">left most column index for the traversal of the matrix</param>
        /// <param name="up_row">up most row index</param>
        /// <param name="right_col">right most column index</param>
        /// <param name="down_row">down most row index</param>
        /// <param name="result">the resulting string which is passed by refference</param>
        private void GetBottomRight(char[][] a, int left_col, int up_row, int right_col, int down_row, ref string result, bool flag, int currChar)
        {
            int i = 0, j = 0;

            // print values in the column.
            for (j = down_row - 1; j >= up_row; j--)
            {
                if (flag == true)
                    result += a[j][right_col - 1];
                else
                    a[j][right_col - 1] = result[currChar++];
            }

            // print values in the row.
            for (i = right_col - 2; i >= left_col; i--)
            {
                if (flag == true)
                    result += a[j + 1][i];
                else
                    a[j + 1][i] += result[currChar++];
            }


            // see if more layers need to be printed.
            if (right_col - left_col > 1)
            {
                // Since we have exhausted our right_most column and top_most row we increment the top_most row 
                // and decrement right_most col
                GetTopLeft(a, left_col, up_row + 1, right_col - 1, down_row, ref result, flag, currChar);
            }
        }

        /// <summary>
        /// Text decryption method. Uses spiraling matrix route to decrypt the given word
        /// </summary>
        /// <param name="plainText">The text to be decrypted</param>
        /// <returns></returns>
        public string Decrpyt(string cipherText)
        {
            string result = "";
            int columns = Math.Abs(Key);
            int rows = cipherText.Length / columns;
            char[][] grid = new char[rows][];
            for (int i = 0; i < rows; i++)
            {
                grid[i] = new char[columns];
            }

            if (Key > 0)
                GetTopLeft(grid, 0, 0, columns, rows, ref cipherText, false, 0);
            else
                GetBottomRight(grid, 0, 0, columns, rows, ref cipherText, false, 0);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result += grid[i][j];
                }
            }

            return result;
        }

        public override string ToString() { return $"The key is: {Key}"; }
    }
    #endregion
}
