using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BEAR
{
    public class Logger
    {
        String errorLogFileName = "Attorney_Forecast_ErrorLog_" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt";
        StreamWriter errorLogFile;

        public Logger()
        {
            try
            {
                errorLogFile = new StreamWriter(this.errorLogFileName, true);
                WriteOpeningLine();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        public Logger(String fileNameOverride)
        {
            try
            {
                this.errorLogFileName = fileNameOverride;
                errorLogFile = new StreamWriter(this.errorLogFileName, true);
                WriteOpeningLine();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

        }

        public static void QuickLog(String fileNameOverride, String lineOne, String lineTwo, String lineThree)
        {
            try
            {
                Logger quickLog = new Logger(fileNameOverride);
                quickLog.WriteLine(lineOne);
                if(! lineTwo.Equals("")) quickLog.WriteLine(lineTwo);
                if(! lineThree.Equals("")) quickLog.WriteLine(lineThree);
                quickLog.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

        }         
               



        public void WriteOpeningLine()
        {
            try
            {
                this.errorLogFile.WriteLine("^^^^^^^^^^^^^^^^^^^^");
                this.errorLogFile.WriteLine("Logging Started: " + Convert.ToString(System.DateTime.Now));
                this.errorLogFile.Flush();
            }
            catch (IOException e)
            {
                Console.WriteLine("IOException" + e.Message);
                Console.WriteLine(e.StackTrace);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception" + e.Message);
                Console.WriteLine(e.StackTrace);
            }

        }


        public void WriteLine(String txt)
        {
            try
            {
                this.errorLogFile.WriteLine(txt);
                this.errorLogFile.Flush();
            }
            catch (IOException e)
            {
                Console.WriteLine("IOException" + e.Message);
                Console.WriteLine(e.StackTrace);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception" + e.Message);
                Console.WriteLine(e.StackTrace);
            }

        }

        public void Close()
        {
            try
            {
                this.errorLogFile.WriteLine("Logging Closed: " + Convert.ToString(System.DateTime.Now));
                this.errorLogFile.WriteLine("VVVVVVVVVVVVVVVVVVVV");
                this.errorLogFile.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

        }


    }
}
