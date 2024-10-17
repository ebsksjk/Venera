using CosmosELFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Venera
{

    /*
    unsafe
            {
                fixed (byte* ptr = TestFile.test_so)
                {
                    var exe = new UnmanagedExecutable(ptr);
                    exe.Load();
                    exe.Link();

                    Console.WriteLine("Executing");
                    new ArgumentWriter();
                    exe.Invoke("tty_clear");

                    new ArgumentWriter()
                        .Push(5)  //fg
                        .Push(15); //bg
                    exe.Invoke("tty_set_color");

                    fixed (byte* str = Encoding.ASCII.GetBytes("Hello World"))
                    {
                        new ArgumentWriter()
                            .Push((uint)str);
                        exe.Invoke("tty_puts");
                    }


                }
     
     */
    public static class ApplicationRunner {
        private class Application {
            String name;
            const String entrypoint = "main";
            //FILE
            byte[] mCode;

            String[] args;

            public Application(String _name, byte[] _mCode, String[] _args) {
                name = _name; mCode = _mCode; args = _args;
            }

            public int runEntryPoint(string entry){
                unsafe
                {
                    fixed (byte* appPtr = mCode)
                    {
                        var exe = new UnmanagedExecutable(appPtr);
                        exe.Load();
                        exe.Link();

                        Console.WriteLine("Executing " + entry + " @ " + name);
                        ArgumentWriter aw = new ArgumentWriter();
                        if (args != null) {
                            foreach (String arg in args) {
                                fixed (byte* str = Encoding.ASCII.GetBytes(arg)) {
                                    aw.Push((uint)str);
                                }
                            }
                        }

                        Console.WriteLine("Invoke!");
                        exe.Invoke(entry);
                        Console.WriteLine("Invoked!");

                        return 0;
                    }
                }
            }

            public int run() {
                unsafe {
                    fixed (byte* appPtr = mCode) {
                        var exe = new UnmanagedExecutable(appPtr);
                        exe.Load();
                        exe.Link();

                        ArgumentWriter aw = new ArgumentWriter();

                        foreach(String arg in args) {
                            fixed (byte* str = Encoding.ASCII.GetBytes(arg)) {
                                aw.Push((uint)str);
                            }
                        }

                        exe.Invoke(entrypoint);

                        return 0;
                    }
                }

            }
        }

        public static int runApplication(String aName, byte[] aCode, String[] args){
            Application cApp = new Application(aName, aCode, args);
            int ret;

            int pid = ProcessTable.registerProcess(aName);

            Console.Write("Executing... pid: " + pid);
            ret = cApp.run();

            Console.Write("ya!");
            ProcessTable.unregisterProcess(pid);

            return ret;

        }

        public static int runApplicationEntryPoint(String aName, byte[] aCode, String[] args, String entryPoint) {
            Console.WriteLine("received request to run " + entryPoint + " @ " + aName);
            Application cApp = new Application(aName, aCode, args);
            Console.WriteLine("created new Application!");
            int ret = 0;

            int pid = ProcessTable.registerProcess(aName);
            Console.WriteLine("registered process with pid: " + pid);

            Console.Write("Executing... pid: " + pid);
            ret = cApp.runEntryPoint(entryPoint);

            Console.Write("ya!");
            ProcessTable.unregisterProcess(pid);

            return ret;
        }

    }

    public static class ProcessTable {

        private static int cPID = 0;
        static List<Process> _processTable = new();

        public static int registerProcess(String name) {
            Console.WriteLine("received request to register a process " + name);
            _processTable.Add(new Process(++cPID, name));
            Console.WriteLine(name + " is registered with PID " + cPID);
            return cPID;
        }

        public static void unregisterProcess(int PID) {
            _processTable.RemoveAll(x => x.pid  == PID);
        }


    }

    public class Process {
        public int pid;
        public String name;
        //public DateTime start;

        public Process(int _pid, String _name) {
            Console.WriteLine("Creating Process " + _name + " with pid " + _pid);
            pid = _pid;
            //start = DateTime.Now;
        }
    }


}
