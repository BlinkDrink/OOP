using Homework5.Interfaces;

namespace Homework5.Concretes
{
    public class CountDown : IEnumerator
    {
        #region Private Fields
        /// <summary>
        /// Private integer field representing the inner counter
        /// </summary>
        private int count = 17;
        #endregion

        /// <summary>
        /// object Current exposes the inner counter 
        /// </summary>
        public object Current => count;

        #region Methods
        /// <summary>
        /// Moves the counter to the next value in the sequence and checks if the value is greater than 0
        /// </summary>
        /// <returns>True if count > 0, false otherwise</returns>
        public bool MoveNext()
        {
            return count-- > 0;
        }

        /// <summary>
        /// Resets our counter to its initial value
        /// </summary>
        public void Reset()
        {
            count = 17;
        }
        #endregion
    }
}
