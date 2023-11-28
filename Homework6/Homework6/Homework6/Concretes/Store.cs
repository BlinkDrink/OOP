using System.ComponentModel;

namespace Homework6.Concretes
{
    public class Store : INotifyPropertyChanged
    {
        #region Properties
        private static int cnt = 1;
        private List<Product> listOfProducts = new List<Product>();
        private Employee worker;
        private Manager manager;
        public string STORE_NAME { get; init; }
        public List<Product> ListOfProducts
        {
            get { return listOfProducts; }
            set
            {
                listOfProducts = value.Select(p => new Product(p.Description, p.Quantity)).ToList();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ListOfProducts"));
            }
        }
        public Employee Worker { get; set; }
        #endregion

        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler Appoint;
        #endregion

        #region Constructors
        public Store()
        {
            STORE_NAME = $"Store {cnt++}";
        }
        #endregion

        #region Methods
        public void OnUpdateQuantity(int index, int newQty)
        {
            if (index >= 0 && index < listOfProducts.Count)
            {
                Product updatedProduct = listOfProducts[index];
                int oldQty = updatedProduct.Quantity;
                updatedProduct.Quantity = newQty;

                Console.WriteLine($"Product at index {index} updated by {Worker.Name}: {updatedProduct.Description} - Quantity: {oldQty} -> {newQty}");

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductQuantity"));
                // Call ManageProductQuantity() or other relevant handling methods
            }
        }

        public void OnAppointment(Employee employee)
        {
            if (employee is Manager)
                manager = (Manager)employee;
            else
                worker = employee;

            employee.GetAppointed(this);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ListofProducts"));
            Appoint?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
