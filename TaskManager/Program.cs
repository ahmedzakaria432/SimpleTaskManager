using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManager
{
    class Program
    {
        

        public static void ChangePriority(int PID,string Priority )
        {
            try
            {
                Process ProcessTochange = Process.GetProcessById(PID);
                switch (Priority)
                {
                    //Normal,High,BelowNormal,RealTime,AboveNormal,Idle
                    case "Normal":
                        ProcessTochange.PriorityClass = ProcessPriorityClass.Normal;
                        break;
                    case "High":
                        ProcessTochange.PriorityClass = ProcessPriorityClass.High;
                        Console.WriteLine("changed to " + ProcessTochange.PriorityClass);
                        break;
                    case "BelowNormal":
                        ProcessTochange.PriorityClass = ProcessPriorityClass.BelowNormal;
                        break;
                    case "RealTime":
                        ProcessTochange.PriorityClass = ProcessPriorityClass.RealTime;
                        break;
                    case "AboveNormal":
                        ProcessTochange.PriorityClass = ProcessPriorityClass.AboveNormal;
                        break;
                    case "Idle":
                        ProcessTochange.PriorityClass = ProcessPriorityClass.Idle;
                        break;
                    default:
                        Console.WriteLine("There is no such piriority");
                        break;



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public static void KillProcess(int PID)
        {
            try {
                Process ProcessToKill = Process.GetProcessById(PID);
                ProcessToKill.Kill();
                Console.WriteLine("Killed Sucessfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private static DateTime lastTime;
        private static TimeSpan lastTotalProcessorTime;

        private static DateTime curTime;
        private static TimeSpan curTotalProcessorTime;
        public static double CalculateCpuUsage(Process process)
        {
           

           

            try
            {
                lastTime=default(DateTime);
                lastTotalProcessorTime=default(TimeSpan);
                

                if (lastTime == default(DateTime))
                {
                    lastTime = DateTime.Now;
                    lastTotalProcessorTime = process.TotalProcessorTime;
                }

                Thread.Sleep(200);

                    curTime = DateTime.Now;
                    curTotalProcessorTime = process.TotalProcessorTime;
                    
                    double CPUUsage = (curTotalProcessorTime.TotalMilliseconds - lastTotalProcessorTime.TotalMilliseconds) /
                    curTime.Subtract(lastTime).TotalMilliseconds / Convert.ToDouble(Environment.ProcessorCount);
                    lastTime = curTime;
                    lastTotalProcessorTime = curTotalProcessorTime;
                    
                    
                    return CPUUsage*100;
                }
            
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return 0.0;

        }
        public static void showAndCalcProcessInfo()
        {
            var RunningProcesses = Process.GetProcesses();
            
            foreach (var Process in RunningProcesses)
            {
                try
                {
                   
                    ProcessInfo CurrentProcess = new ProcessInfo { ProcessID = Process.Id, ProcessName = Process.ProcessName, CpuUsage = CalculateCpuUsage(Process) };
                    
                    Console.WriteLine(CurrentProcess);
                    Console.WriteLine();
                   

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
            


        }


        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to simple Task Manager \n \n");
            while (! Console.KeyAvailable)
            {
                
               
                Console.WriteLine("1- Please press 1 to show Process name and process ID and CPU usage for all Running process   ");
                Console.WriteLine("2- Plese Press 2 to Kill any Process ");
                Console.WriteLine("3- Please press 3 to change priority of any process ");
                Console.WriteLine("4- Please Press 4 to exit");
                Console.WriteLine();
                var Selection = Console.ReadLine();
                switch (Selection)
                {
                    case "1":
                         showAndCalcProcessInfo();
                        
                        break;
                    case "2":
                        try {
                            Console.WriteLine("Please enter the Id Of process, you want to kill");
                            var pidTokill = Console.ReadLine();
                            KillProcess(int.Parse(pidTokill));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message); ;
                        }


                        break;
                    case "3":
                        try {
                            Console.WriteLine("Please enter PID ");
                            var PidToChange = Console.ReadLine();
                            Console.WriteLine("Please enter desired Priority\n Notice: available priorities is Normal, High, BelowNormal, RealTime, AboveNormal, Idle ");
                            var Priority = Console.ReadLine();
                            ChangePriority(Convert.ToInt32( PidToChange),Priority);
                        }
                        catch ( Exception ex) {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case "4":
                        Environment.Exit(0);
                        break;


                    default:
                        Console.WriteLine("Sorry!!! You must choose one of available choices ");
                        Console.WriteLine();
                        Console.WriteLine();
                        break;
                }



            }

        }
    }
}
