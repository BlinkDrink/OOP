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

        private string worksAt;

        public string WorksAt
        {
            get { return worksAt; }
            set { worksAt = value; }
        }
        #endregion

        #region Constructors
        public Employee(string name, string worksAt)
        {
            Name = name;
            WorksAt = worksAt;
        }
        #endregion

        #region Methods
        public override string ToString() { return $"{Name} {WorksAt}"; }
        #endregion
    }
}
