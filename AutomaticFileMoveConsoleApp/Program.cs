using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace AutomaticFileMoveConsoleApp
{
    class Program
    {
        static string sourceDirectory;
        static string destinationDirectory;

        static void Main(string[] args)
        {
            #region//Get source directory
            bool validDirectory = false;
            Console.WriteLine("Enter the source directory and press Enter:");
            while (!validDirectory)
            {
                sourceDirectory =  Console.ReadLine();
                validDirectory = Directory.Exists(sourceDirectory);
                if (!validDirectory)
                {
                    DisplayError();
                    Console.WriteLine("The specified source directory doesn't exist.\nPlease re-enter the source directory:");
                }
            }
            #endregion

            #region//Get destination directory
            validDirectory = false;
            Console.WriteLine("Enter the destination directory and press Enter:");
            while (!validDirectory)
            {
                destinationDirectory = Console.ReadLine();
                validDirectory = Directory.Exists(destinationDirectory);
                if (!validDirectory)
                {
                    DisplayError();
                    Console.WriteLine("The specified destination directory doesn't exist.\nPlease re-enter the destination directory:");
                }
            }
            #endregion
            DateTime startTime = DateTime.Now;
            Console.WriteLine("Start Time: {0}", startTime);
            MoveAllExistingFiles(sourceDirectory);
            DateTime endTime = DateTime.Now;
            Console.WriteLine("Start Time: {0}", startTime);
            Console.WriteLine("End Time: {0}", endTime);
            TimeSpan elapsedTime = endTime - startTime;
            Console.WriteLine("Total Elapsed Time (hours): {0}", elapsedTime.TotalHours);
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();

        }

        private static void DisplayError()
        {
            Console.WriteLine();
            Console.WriteLine("@@@             ");
            Console.WriteLine("@               ");
            Console.WriteLine("@@  @@ @@ @@@ @@");
            Console.WriteLine("@   @  @  @ @ @ ");
            Console.WriteLine("@@@ @  @  @@@ @ ");
            Console.WriteLine();
        }

        static void MoveAllExistingFiles(string sourcePath)
        {
            Console.WriteLine("Processing: {0}", sourcePath);
            #region Gather all sub-dreictories
            string[] directories = Directory.GetDirectories(sourcePath);
            foreach (string directory in directories)
            {
                string curDestinationDirectory = directory.Replace(sourceDirectory, destinationDirectory);
                if (!Directory.Exists(curDestinationDirectory))
                {
                    Directory.CreateDirectory(curDestinationDirectory);
                }
                MoveAllExistingFiles(directory);
            }
            #endregion

            string[] files = Directory.GetFiles(sourcePath);
            int index = 0;
            int count = files.Length;
            bool displayPercentage = true;
            Console.Write("Completed:");
            foreach (string file in files)
            {
                MoveFile(file);
                index++;
                int percentage = (index*100)/count;
                if (percentage % 10 == 0)
                {
                    if (displayPercentage)
                    {
                        displayPercentage = false;
                        Console.Write(" {0}%", percentage);
                    }
                }
                else if (!displayPercentage)
                {
                    displayPercentage = true;
                }
            }
            Console.Write("\n");
            Console.WriteLine("Completed: {0}", sourcePath);
        }

        /// <summary>
        /// Moves filename from the source path to the destination path as defined on the form
        /// OVERWRITES all duplicated filenames
        /// </summary>
        /// <param name="fileName"></param>
        static void MoveFile(string fileName)
        {
            string sourceFile = "";
            string destinationFile = "";

            sourceFile = fileName;
            FileInfo sf = new FileInfo(sourceFile);
            destinationFile = @"" + destinationDirectory + "\\" + sf.Name;
            FileInfo df = new FileInfo(destinationFile);


            

            //To overwrite if file already exists in destination, the destination file must be deleted first
            if (df.Exists)
                df.Delete();
            
            // To move a file or folder to a new location:
            bool fileMoved = false;
            int tries = 0;
            while (!fileMoved)
            {
                if (tries == 1000) break;
                try
                {
                    tries++;
                    sf.MoveTo(destinationFile);
                    fileMoved = true;
                }
                catch
                {
                    fileMoved = false;
                }
            }
        }

    }
}
