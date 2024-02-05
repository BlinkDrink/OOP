using EducationServiceLib;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

namespace EducationServer
{
    class Program
    {
        private readonly object courseLock;
        private readonly object fileLock;
        private Dictionary<string, Course> courses;
        private List<string> titles;

        static void Main(string[] args)
        {
            Program program = new Program();
            program.RunServer();
        }

        public Program()
        {
            Random random = new Random();

            int initialCount = random.Next(6, 21);

            titles = new List<string>();
            for (int i = 1; i <= initialCount; i++)
            {
                titles.Add($"Title No. {i}");
            }

            courses = new Dictionary<string, Course>();
            for (int i = 0; i < initialCount; i++)
            {
                string title = titles[i];
                ServiceType serviceType = (ServiceType)random.Next(0, Enum.GetValues(typeof(ServiceType)).Length);
                int numOfStudents = random.Next(6, 21);
                Course course = new Course(serviceType, title, numOfStudents);
                courses.Add(course.Title, course);
            }
        }

        void RunServer() 
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 55555); 
            tcpListener.Start();
            Console.WriteLine("Server is listening on port 55555...");

            try
            {
                while (true)
                {
                    TcpClient client = tcpListener.AcceptTcpClient();
                    Console.WriteLine("A new client connected...");
                    ThreadPool.QueueUserWorkItem(ProcessOrder, client);
                    //Thread clientThread = new Thread(() => ProcessOrder(client));
                    //clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                tcpListener.Stop();
            }
        }

        public void ProcessOrder(object clientObj)
        {
            TcpClient client = (TcpClient)clientObj;

            try
            {
                using (NetworkStream networkStream = client.GetStream())
                using (BinaryReader reader = new BinaryReader(networkStream))
                using (BinaryWriter writer = new BinaryWriter(networkStream))
                {
                    string titlesString = string.Join(",", titles);
                    string serviceTypesString = string.Join(",", Enum.GetNames(typeof(ServiceType)));

                    writer.Write(titlesString);
                    writer.Write(serviceTypesString);

                    while (true)
                    {
                        string requestData = reader.ReadString();
                        WriteData(requestData);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }

        public void WriteData(string data)
        {
            try
            {
                string[] requestData = data.Split(',');

                string selectedTitle = requestData[0];
                string selectedCourseTitle = requestData[1];
                ServiceType selectedType = (ServiceType)Enum.Parse(typeof(ServiceType), requestData[2]) ;
                int selectedQty = int.Parse(requestData[3]);

                Course course = new Course(selectedType,selectedCourseTitle, selectedQty);

                lock (courseLock)
                {
                    courses[selectedTitle] = course;
                }

                string fileName = $"{selectedTitle}.txt";
                lock (fileLock)
                {
                    using (StreamWriter writer = new StreamWriter(fileName, true)) // Добавяме, не презаписваме (Append)
                    {
                        writer.WriteLine(course.ToString());
                    }
                }

                if (selectedQty > Course.MAX_IN_COURSE)
                {
                    Course copy = new Course(course);
                    copy.NumOfStudents = Course.MAX_IN_COURSE;

                    lock (courseLock)
                    {
                        courses[copy.Title] = copy;
                    }

                    fileName = $"{copy.Title}.txt";
                    lock (fileLock)
                    {
                        using (StreamWriter writer = new StreamWriter(fileName, true))
                        {
                            writer.WriteLine(copy.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing data: {ex.Message}");
            }
        }
    }
}
