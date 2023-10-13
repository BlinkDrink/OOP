namespace Homework1
{
    public class Table
    {
        #region Properties

        /// <summary>
        /// Field representing the beginning of the interval
        /// </summary>
        private double intervalStart;

        /// <summary>
        /// Property for intervalStart
        /// </summary>
        public double IntervalStart
        {
            get { return intervalStart; }
            set { intervalStart = value; }
        }

        /// <summary>
        /// Field representing the end of the interval
        /// </summary>
        private double intervalEnd;

        /// <summary>
        /// Property for intervalEnd
        /// </summary>
        public double IntervalEnd
        {
            get { return intervalEnd; }
            set { intervalEnd = value; }
        }

        /// <summary>
        /// Field representing the step of the interval 
        /// </summary>
        private double intervalStep;

        /// <summary>
        /// Property for intervalStep
        /// </summary>
        public double IntervalStep
        {
            get { return intervalStep; }
            set { intervalStep = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor that initializes the inner data.
        /// If intervalStart is greater than intervalEnd - swap their values.
        /// </summary>
        /// <param name="intervalStart">Value to set for IntervalStart</param>
        /// <param name="intervalEnd">Value to set for IntervalEnd</param>
        /// <param name="intervalStep">Value to set for IntervalStep</param>
        public Table(double intervalStart, double intervalEnd, double intervalStep)
        {
            IntervalStart = intervalStart;
            IntervalEnd = intervalEnd;
            IntervalStep = intervalStep;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Simple private method used for calculating the function 
        /// f(x) = |x-2|^2/(x^2 + 1)
        /// </summary>
        /// <param name="x">The argument we pass for the function f</param>
        /// <returns></returns>
        private double MyFunction(double x)
        {
            return Math.Pow(Math.Abs(x - 2), 2) / (Math.Pow(x, 2) + 1);
        }

        /// <summary>
        /// Method used for printing the table with values for the function
        /// f(x) = |x-2|^2/(x^2 + 1)
        /// </summary>
        public void MakeTable()
        {
            /// Calculate how many prints we will make (i.e. for interval (0,2,0.1) we will make 21 prints
            int ticks = Convert.ToInt32((IntervalEnd - IntervalStart) / IntervalStep);

            ///Print the header of the table in the format "x    f(x)"
            Console.WriteLine("x\tf(x)");

            /// Iterate through the number of ticks and beware of the ending of the interval
            for (int i = 0; i <= ticks && IntervalStart + i * IntervalStep <= IntervalEnd; i++)
            {
                if (i % 20 == 0 && i != 0)
                {
                    /// Prompt the user for input on whether to continue printing values or not
                    Console.WriteLine("\nType \"return\" to continue ...");
                    string input = Console.ReadLine();
                    if (input != "return")
                        break;
                }

                /// Print the current x and the value of f(x) separated by tab (\t)
                double currentX = IntervalStart + i * IntervalStep;
                Console.WriteLine($"{currentX:F4}\t{MyFunction(currentX):F4}");
            }
        }
        #endregion
    }
}
