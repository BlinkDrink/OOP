namespace Lab2
{
    public class Time
    {
        #region Properties
        private int hour;
        private int minute;
        private int second;

        public int Second
        {
            get { return second; }
            set
            {
                if (value >= 0 && value < 60)
                {
                    second = value;
                }
                else
                {
                    Console.WriteLine($"Invalid input for seconds: {value}");
                }
            }
        }


        public int Minute
        {
            get { return minute; }
            set
            {
                if (value >= 0 && value < 60)
                {
                    minute = value;
                }
                else
                {
                    Console.WriteLine($"Invalid input for minutes: {value}");
                }
            }
        }


        public int Hour
        {
            get { return hour; }
            set
            {
                if (value >= 0 && value < 24)
                    hour = value;
                else
                    Console.WriteLine($"Invalid input for hours: {value}");
            }
        }
        #endregion

        #region Constructors
        public Time()
        {
            Hour = 0;
            Minute = 0;
            Second = 0;
        }

        public Time(int hour, int minute, int second)
        {
            Hour = hour;
            Minute = minute;
            Second = second;
        }
        #endregion

        #region Methods
        public override string ToString() => $"Time [{hour}:{minute}:{second}]";
        #endregion

    }
}
