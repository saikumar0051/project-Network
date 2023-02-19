using System.IO;
using System.Text;
using System.Net;
using System.Management;
using System.Diagnostics;
using System.Globalization;
using System.Reflection.Emit;

namespace CPUUsage
{
    class Program
    {
        static void Main(string[] args)
        {


            // Server Name
            string HostName = Dns.GetHostName();
            Console.WriteLine("Server Name: " + HostName);

            // Current time
            var timeStamp = System.DateTime.Now;
            Console.WriteLine("Current Time on the Server: " + timeStamp);

            // Current date
            var currentDate = timeStamp.ToString("ddMMyy");
            Console.WriteLine("Current Date on the Server: " + currentDate);


            Process process = Process.GetCurrentProcess();
            Console.WriteLine("User CPU time for current process: " + process.UserProcessorTime.TotalSeconds + " seconds");

            //Change the path to desired location
            String file = @"C:\Users\Sai\project\csharp\" + HostName + "_cpuUsage_" + currentDate + ".csv";

            // Set the variable "separator" to ",".
            String delimiter = ",";
            StringBuilder output = new StringBuilder();

            if (!File.Exists(file))
            {

                string createText = "Server_name" + delimiter + "Date" + delimiter + "cpuUsage" + delimiter + "User_cpu_time" + delimiter
                                     + "Kernal_cpu_time" + delimiter + Environment.NewLine;
                File.WriteAllText(file, createText);
            }


            PerformanceCounter cpuCounter;
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            cpuCounter.NextValue();

            Thread.Sleep(1000);

            string currentCpuUsage = cpuCounter.NextValue() + "%";


            Console.WriteLine("CPU Usage: " + currentCpuUsage);
            PerformanceCounter pc = new PerformanceCounter("Processor", "% Privileged Time", "_Total");
            float KernalTime;
            pc.NextValue();

            Thread.Sleep(1000);

            KernalTime = pc.NextValue();
            Console.WriteLine(KernalTime);

            string appendText = HostName + delimiter + timeStamp.ToString() + delimiter + currentCpuUsage.ToString() + delimiter
                                 + process.UserProcessorTime.TotalSeconds.ToString() + delimiter + KernalTime + delimiter + Environment.NewLine;
            File.AppendAllText(file, appendText);



            try
            {
                File.AppendAllText(file, output.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Data could not be written to the CSV file.");
                return;
            }

            Console.WriteLine("The data has been successfully saved to the CSV file");


        }



    }

}
