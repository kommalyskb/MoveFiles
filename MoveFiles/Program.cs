using log4net;
using log4net.Config;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MoveFiles
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            // Load configuration
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            Console.WriteLine("############ Move File ##################");
            Console.Write("Please specify source path: ");
            var source = Console.ReadLine();
            Console.Write("Please specify desination path: ");
            var destination = Console.ReadLine();
            Console.Write("Please specify start date(yyyy-MM-dd): ");
            var start = Console.ReadLine();
            Console.Write("Please specify end date(yyyy-MM-dd): ");
            var end = Console.ReadLine();
            Console.Write("Do you want to move or copy file? 'x' for move file, 'c' for copy file: ");
            var op = Console.ReadLine();

            Console.WriteLine($"Source: {source}\nDestination: {destination}\nStart Date: {start}\nEnd Date: {end}\nOperation: {op}\nPress any key to continue...");
            // Check destination folder
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }
            Console.ReadLine();
            bool opb = false;
            if (op == "c")
            {
                opb = true;
            }

            GetFileInfor(source, destination, start, end, opb);

            Console.ReadLine();
        }

        private static void GetFileInfor(string source, string destination, string startDate, string endDate, bool iscopy)
        {
            DateTime start = Convert.ToDateTime(startDate);
            DateTime end = Convert.ToDateTime(endDate);
            DirectoryInfo rootDir = new DirectoryInfo(source);
            FileInfo[] files = rootDir.GetFiles().
                Where(x => x.CreationTime.Date >= start && x.CreationTime.Date <= end).ToArray();
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            foreach (FileInfo file in files)
            {
                try
                {
                    if (iscopy)
                    {
                        // Move file
                        File.Copy(file.FullName, $"{destination}\\{file.Name}", false);
                        Console.WriteLine($"{file.Name} has copied - on: {DateTime.Now}");
                    }
                    else
                    {
                        // Move file
                        File.Move(file.FullName, $"{destination}\\{file.Name}");
                        Console.WriteLine($"{file.Name} has moved - on: {DateTime.Now}", false);
                    }

                }
                catch (Exception ex)
                {
                    // Write log
                    log.Error($"File: {file.Name} - Err: {ex.Message}");
                }
            }

            // Stop timing.
            stopwatch.Stop();

            // Write result.
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
        }
    }
}
