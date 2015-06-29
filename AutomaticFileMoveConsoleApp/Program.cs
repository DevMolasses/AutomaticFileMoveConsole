using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AutomaticFileMoveConsoleApp
{
    class Program
    {
        static ulong filesMovedCount = 0;
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

            MoveAllExistingFiles(sourceDirectory);

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
            string[] directories = Directory.GetDirectories(sourcePath);
            foreach (string directory in directories)
            {
                string curDestinationDirectory = directory.Replace(sourceDirectory, destinationDirectory);
                if (!Directory.Exists(curDestinationDirectory))
                {
                    Directory.CreateDirectory(curDestinationDirectory);
                    Console.WriteLine(String.Format("New Directory: {0}", curDestinationDirectory));
                }
                MoveAllExistingFiles(directory);
            }

            string[] files = Directory.GetFiles(sourcePath);
            foreach (string file in files)
            {
                string curFile = System.IO.Path.GetFullPath(file);
                curFile = curFile.Replace(sourceDirectory, "");
                MoveFile(curFile);
            }
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

            sourceFile = @"" + sourceDirectory + "\\" + fileName;
            destinationFile = @"" + destinationDirectory + "\\" + fileName;


            //To overwrite if file already exists in destination, the destination file must be deleted first
            File.Delete(destinationFile);
            
            // To move a file or folder to a new location:
            bool fileMoved = false;
            int tries = 0;
            while (!fileMoved)
            {
                if (tries == 1000) break;
                try
                {
                    tries++;
                    System.IO.File.Move(sourceFile, destinationFile);
                    fileMoved = true;
                    filesMovedCount++;
                    Console.WriteLine(String.Format("File {0}: {1}", filesMovedCount, sourceFile.Replace(sourceDirectory,"")));
                    
                }
                catch
                {
                }
            }
        }

    }
}
