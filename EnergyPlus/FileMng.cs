using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EnergyPlus
{
    public class FileMng
    {
        public static string CreateTmpFile(string idfFileLocation)
        {
            //write the entire currentIdfFile to Temp, so it can be passed around instead of the idf.
            //once the user wants to save, this temp file becomes the new idf and this new idf is passed
            //here to create a new temp file.  The old one is destroyed

            StringBuilder output = new StringBuilder();
            string fileName = string.Empty;
            try
            {
                fileName = Path.GetTempFileName();
                FileInfo fileInfo = new FileInfo(fileName);

                fileInfo.Attributes = FileAttributes.Temporary;

                Encoding encoding;
                string line;
                //open the existing idfFile
                //and write it to the tempFile Location
                using (StreamReader reader = new StreamReader(idfFileLocation))
                {
                    encoding = reader.CurrentEncoding;
                    while ((line = reader.ReadLine()) != null)
                    {
                        output.AppendLine(line);
                    }
                    using (StreamWriter writer = new StreamWriter(fileName, false, encoding))
                    {
                        writer.Write(output.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to create temp file or set its attributes.");

            }
            return fileName;
        }

        //temp text file update
        //the idea being that I've created an entire string ready to write
        //the string tmpFile is the location of the file
        public static bool UpdateTmpFile(string tmpFileLoc, StringBuilder output, bool tf, Encoding encoding)
        {
            try
            {
                // Write to the temp file.
                using (StreamWriter streamWriter = new StreamWriter(tmpFileLoc, tf, encoding))
                {
                    streamWriter.Write(output.ToString());
                }
                Console.WriteLine("TEMP file updated.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing to TEMP file: " + ex.Message);
            }
            return true;
        }

        public static bool CreateLogFile(string fileLocation, StringBuilder logFileString, bool TF, Encoding encoding)
        {
            try
            {
                // Write to the temp file.
                using (StreamWriter streamWriter = new StreamWriter(fileLocation, TF, encoding))
                {
                    streamWriter.Write(logFileString.ToString());
                }
                Console.WriteLine("Log file written.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing to log file file: " + ex.Message);
            }
            return true;
        }

        //delete the temp text file
        //I believe here we are passing the location of the TempFile
        public static bool DeleteTmpFile(string tmpFile)
        {
            try
            {
                // Delete the temp file (if it exists)
                if (File.Exists(tmpFile))
                {
                    File.Delete(tmpFile);
                    Console.WriteLine("TEMP file deleted.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Could not find Temp File");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Major Error deleting TEMP file: " + ex.Message);
                return false;
            }

        }

        internal static bool SaveFileDialog(string desiredfilePath, string tempFileLocation)
        {
            StringBuilder output = new StringBuilder();
            string line;
            using (StreamReader reader = new StreamReader(tempFileLocation))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    output.AppendLine(line);
                }
            }
            using (StreamWriter writer = new StreamWriter(desiredfilePath, false))
            {
                writer.Write(output.ToString());
                writer.Close();
            }
            return true;
        }
    }
}
