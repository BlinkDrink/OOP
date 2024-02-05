using System;

namespace EducationServiceLib
{
    public class Course
    {
        private static int currCourse = 0;

        public static int MAX_IN_COURSE = 10;
        public string ID;

        private ServiceType serviceType;
        private string title;
        private int numOfStudents;

        public ServiceType ServiceType
        {
            get => serviceType;
            set
            {
                serviceType = value;
            }
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
            }
        }
        public int NumOfStudents
        {
            get => numOfStudents;
            set
            {
                numOfStudents = value; 
            }
        }

        public Course(ServiceType serviceType, string title, int numOfStudents)
        {
            Title = title;
            ID = (currCourse++.ToString().PadLeft(4, '0')) + "-2024";
            NumOfStudents = numOfStudents;
            ServiceType = serviceType;
        }

        public Course(Course other)
        {
            this.ID = other.ID;
            this.ServiceType = other.ServiceType;
            this.Title = other.Title;
            this.NumOfStudents = other.NumOfStudents;
        }

        public override string ToString()
        {
            return $"CID: {ID}, Cat: {ServiceType}, {Title}, Students: {NumOfStudents}";
        }
    }
}
