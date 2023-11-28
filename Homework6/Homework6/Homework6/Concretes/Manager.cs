namespace Homework6.Concretes
{
    public class Manager : Employee
    {

        #region Constructors
        public Manager(string name, string worksAt) : base(name, worksAt) { }
        #endregion

        #region Methods
        internal void ManageProductQuantity(Store store)
        {
            // Handle ProductQuantity events
        }
        #endregion
    }
}
