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

    public Product()
    {
        Qty = 12;
    }

    public override string ToString()
    {
        return $"PID: {ID}, Category: {ProductCategory}, Quantity: {Qty}, Reorder Level: {ReorderLevel}";
    }
}

public enum Category
{
    SOFTWARE, HARDWARE, EBOOKS
}
