using CosmosELFCore;
using System;
using System.Collections.Generic;
using System.Text;
using XSharp.Assembler.x86;

namespace Venera {
    public static class ApplicationRunner {
        private class Application {
            String name = null;
            const String entrypoint = "main";
            //FILE
            byte[] mCode = null;

            String[] args = null;

            public Application(String _name, byte[] _mCode, String[] _args) {
                //Kernel.PrintDebug("trying to create new application! name: " + _name + " args: ");
                if(_args != null)
                {
                    foreach (String _arg in _args)
                    {
                        //Kernel.PrintDebug(_arg);
                    }
                }
                
                this.name = _name; this.mCode = _mCode; this.args = _args;

                if (this.args != null)
                {
                    //Kernel.PrintDebug("real args");
                    foreach (String arg in args)
                    {
                        //Kernel.PrintDebug(arg);
                    }
                }
            }

            public int runEntryPoint(string entry){
                unsafe
                {
                    fixed (byte* appPtr = mCode)
                    {
                        var exe = new UnmanagedExecutable(appPtr);
                        exe.Load();
                        exe.Link();

                        //Kernel.PrintDebug("Executing " + entry + " @ " + name);
                        ArgumentWriter aw = new ArgumentWriter();
                        if (args != null) {
                            foreach (String arg in args) {
                                fixed (byte* str = Encoding.ASCII.GetBytes(arg)) {
                                    aw.Push((uint)str);
                                }
                            }
                        }

                        //Kernel.PrintDebug("Invoke!");
                        exe.Invoke(entry);
                        //Kernel.PrintDebug("Invoked!");

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

                        if (args != null)
                        {
                            foreach (String arg in args)
                            {
                                fixed (byte* str = Encoding.ASCII.GetBytes(arg))
                                {
                                    aw.Push((uint)str);
                                }
                            }
                        }

                        //Kernel.PrintDebug("Invoking " + entrypoint + " @ " + name);
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
            //Kernel.PrintDebug("received request to run " + entryPoint + " @ " + aName);
            Application cApp = new Application(aName, aCode, args);
            //Kernel.PrintDebug("created new Application!");
            int ret = 0;

            int pid = ProcessTable.registerProcess(aName);
            //Kernel.PrintDebug("registered process with pid: " + pid);

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
            //Kernel.PrintDebug("received request to register a process " + name);
            _processTable.Add(new Process(++cPID, name));
            //Kernel.PrintDebug(name + " is registered with PID " + cPID);
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
            //Kernel.PrintDebug("Creating Process " + _name + " with pid " + _pid);
            pid = _pid;
            //start = DateTime.Now;
        }
    }


}
