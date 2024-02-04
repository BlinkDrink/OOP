using System.Runtime.Serialization;
using System.ServiceModel;

[ServiceContract]
public interface IOrderWService
{
    [OperationContract]
    Product[] Retrieve();

    [OperationContract]
    void Update(string sender, string productID, int qty);
}

// Assuming Product is defined elsewhere, possibly in a shared library
[DataContract]
public class Product
{
    [DataMember]
    public string ID { get; set; }

    [DataMember]
    public Category ProductCategory { get; set; }

    [DataMember]
    public int ReorderLevel { get; set; }

    [DataMember]
    public int Qty { get; set; }

    // Constructor
    public Product()
    {
        Qty = 12;
    }
}

public enum Category
{
    SOFTWARE, HARDWARE, EBOOKS
}
