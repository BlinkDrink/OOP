// Fig. 4.12: GradeBook.cs
// GradeBook class with a constructor to initialize the course name.
using System;

public class GradeBook
{
    private string courseName; // course name for this GradeBook
    private string courseInstructor;


    // constructor initializes courseName with string supplied as argument
    public GradeBook(string name, string instructor)
    {
        CourseName = name; // initialize courseName using property
        CourseInstructor = instructor; // initialize courseInstructor using property
    } // end constructor

    // property to get and set the course name
    public string CourseName
    {
        get
        {
            return courseName;
        } // end get
        set
        {
            courseName = value;
        } // end set
    } // end property CourseName

    public string CourseInstructor { get => courseInstructor; set => courseInstructor = value; }

    public int CourseStart { get; private set; }

    // display a welcome message to the GradeBook user
    public void DisplayMessage()
    {
        // use property CourseName to get the 
        // name of the course that this GradeBook represents
        Console.WriteLine($"Welcome to the grade book for\n{CourseName}\nThis course is presented by: {CourseInstructor}!");
    } // end method DisplayMessage

    public Tuple<int, string, string> GradeBookTitle()
    {
        return Tuple.Create(CourseStart, CourseName, CourseInstructor);
    }

    public void ChangeCourseTitle(Tuple<string, string> title)
    {
        CourseInstructor = title.Item1;
        CourseName = title.Item2;
    }

} // end class GradeBook


