// Fig. 4.13: GradeBookTest.cs
// GradeBook constructor used to specify the course name at the 
// time each GradeBook object is created.

using System;

public class GradeBookTest
{
    // Main method begins program execution
    public static void Main(string[] args)
    {
        // create GradeBook object
        GradeBook gradeBook1 = new GradeBook( // invokes constructor
           "CS101 Introduction to C# Programming", "Ivan Ivanov");
        GradeBook gradeBook2 = new GradeBook( // invokes constructor
           "CS102 Data Structures in C#", "Georgi Georgiev");

        gradeBook1.DisplayMessage();
        gradeBook2.DisplayMessage();

        gradeBook1.ChangeCourseTitle(Tuple.Create("Python 101", "Kristian Velkov"));
        gradeBook1.DisplayMessage();

    } // end Main
} // end class GradeBookTest

