namespace Homework5.Concretes
{
    public class CountDown : Homework5.Interfaces.IEnumerator
    {
        #region Private Fields
        /// <summary>
        /// Private integer field representing the inner counter
        /// </summary>
        protected int count = 17;
        #endregion

        /// <summary>
        /// object Current exposes the inner counter 
        /// </summary>
        public virtual object Current => count;

        #region Methods
        /// <summary>
        /// Moves the counter to the next value in the sequence and checks if the value is greater than 0
        /// </summary>
        /// <returns>True if count > 0, false otherwise</returns>
        public virtual bool MoveNext()
        {
            return count-- > 0;
        }

        /// <summary>
        /// Resets our counter to its initial value
        /// </summary>
        public virtual void Reset()
        {
            count = 17;
        }
        #endregion
    }
}
