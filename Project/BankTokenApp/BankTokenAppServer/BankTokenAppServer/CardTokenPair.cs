using System.Xml.Serialization;

namespace BankTokenAppServer
{
    [Serializable]
    [XmlType("CardTokenPair")]
    public class CardTokenPair
    {
        [XmlElement("CardNumber")]
        public string? CardNumber { get; set; }

        [XmlElement("Token")]
        public string? Token { get; set; }

        public CardTokenPair()
        {
        }

        public CardTokenPair(string cardNumber, string token)
        {
            CardNumber = cardNumber;
            Token = token;
        }
    }
}
