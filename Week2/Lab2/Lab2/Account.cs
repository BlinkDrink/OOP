namespace Lab2
{
    /// <summary>
    /// Class representing a bank account
    /// </summary>
    public class Account
    {
        // Ctrl + K + S => #region
        #region Properties
        /// <summary>
        /// Backing field for AnualInterestRate
        /// </summary>
        private decimal anualInterestRate; // Field
        private decimal balance; // Field


        /// <summary>
        /// Id of an account
        /// read-only init property
        /// </summary>
        public string Id { get; init; } // Property

        /// <summary>
        /// Full property for AnualInterestRate property
        /// </summary>
        public decimal AnualInterestRate
        {
            get { return anualInterestRate; }
            set
            {
                anualInterestRate = value > 0m ? value : 0.01m;
            }
        }

        /// <summary>
        /// Property for Balance
        /// </summary>
        public decimal Balance
        {
            get { return balance; }
            set
            {
                balance = value >= 0m ? value : 0m;
            }
        }

        /// <summary>
        /// Init-only property for DateCreated
        /// </summary>
        public DateTime DateCreated { get; init; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Account()
        {
            Id = "00000";
        }

        /// <summary>
        /// Constructor for Account
        /// ctor - snippet
        /// </summary>
        /// <param name="anualInterestRate">Initial value for AnualInterest Rate</param>
        /// <param name="balance">Initial value for Balance</param>
        /// <param name="id">Initial value for Id</param>
        public Account(decimal anualInterestRate, decimal balance, string id)
        {
            AnualInterestRate = anualInterestRate;
            Balance = balance;
            DateCreated = DateTime.Now;
            Id = id;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deposit method for the account object
        /// </summary>
        /// <param name="amountToDeposit"></param>
        public void Deposit(decimal amountToDeposit)
        {
            if (amountToDeposit > 0)
            {
                balance += amountToDeposit;
            }
            else
            {
                Console.WriteLine($"Wrong input {amountToDeposit}");
            }
        }

        /// <summary>
        /// Withdraw method for the account object
        /// </summary>
        /// <param name="amountToWithdraw"></param>
        public void Withdraw(decimal amountToWithdraw)
        {
            if (amountToWithdraw > 0)
            {
                Balance -= amountToWithdraw;
            }
            else
            {
                Console.WriteLine($"Not enough funds to withdraw {amountToWithdraw}");
            }
        }

        /// <summary>
        /// Overriden toString method used for printing the object to console 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Id: {Id}\nAnual Interest Rate: {AnualInterestRate * 100:F}%\nBalance: {Balance:F}\nDate Created: {DateCreated}";
        }
        #endregion
    }
}
