using System.ComponentModel;

namespace Homework6.Concretes
{
    public class Employee
    {
        #region Properties
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private Store worksAt;

        public Store WorksAt
        {
            get { return worksAt; }
            set { worksAt = value; }
        }
        #endregion

        #region Constructors
        public Employee(string name, Store worksAt)
        {
            Name = name;
            WorksAt = worksAt;
        }
        #endregion

        #region Methods
        internal void ManageListOfProducts()
        {

        }

        public void ManageQty(Product product, int newQuantity)
        {
            WorksAt.OnUpdateQuantity(WorksAt.ListOfProducts.IndexOf(product), newQuantity);
        }

        public void GetAppointed(Store store)
        {
            // Subscribe to events and handle appointments
            store.PropertyChanged += HandleStorePropertyChange;
            store.Appoint += HandleStoreAppointment;
            worksAt = store;
            Console.WriteLine($"Employee {Name} appointed to {store.STORE_NAME}");
        }

        private void HandleStorePropertyChange(object sender, PropertyChangedEventArgs e)
        {
            // Handle PropertyChange events
        }

        private void HandleStoreAppointment(object sender, EventArgs e)
        {
            // Handle appointment events
        }

        public override string ToString() { return $"{Name} {WorksAt}"; }
        #endregion
    }
}
