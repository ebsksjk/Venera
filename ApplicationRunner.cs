﻿using CosmosELFCore;
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

            List<Object> args = null;

            public Application(String _name, byte[] _mCode, List<Object> _args) {
                //Kernel.PrintDebug("trying to create new application! name: " + _name + " args: ");
                
                this.name = _name; this.mCode = _mCode; this.args = _args;
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
                            foreach (Object arg in args) { 
                                switch (arg) {
                                    case char _ when arg is char:
                                        Kernel.PrintDebug("Pushing char: " + arg);
                                        aw.Push((char)arg);
                                        break;
                                    case byte _ when arg is byte:
                                        aw.Push((byte)arg);
                                        break;
                                    case short _ when arg is short:
                                        aw.Push((short)arg);
                                        break;
                                    case int _ when arg is int:
                                        aw.Push((int)arg);
                                        break;
                                    case uint _ when arg is uint:
                                        aw.Push((uint)arg);
                                        break;
                                    case string _ when arg is string:
                                        unsafe
                                        {
                                            fixed (byte* str = Encoding.ASCII.GetBytes((string)arg))
                                            {
                                                aw.Push((uint)str);
                                            }
                                        }
                                    break;
                                    default:
                                        Console.WriteLine("Unknown argument type: " + arg.GetType());
                                        return -1;
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
                            foreach (Object arg in args)
                            {
                                switch (arg)
                                {
                                    case char _ when arg is char:
                                        aw.Push((char)arg);
                                        break;
                                    case byte _ when arg is byte:
                                        aw.Push((byte)arg);
                                        break;
                                    case short _ when arg is short:
                                        aw.Push((short)arg);
                                        break;
                                    case int _ when arg is int:
                                        aw.Push((int)arg);
                                        break;
                                    case uint _ when arg is uint:
                                        aw.Push((uint)arg);
                                        break;
                                    default:
                                        Console.WriteLine("Unknown argument type: " + arg.GetType());
                                        return -1;
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

        public static int runApplication(String aName, byte[] aCode, List<Object> args){
            Application cApp = new Application(aName, aCode, args);
            int ret;

            int pid = ProcessTable.registerProcess(aName);

            Console.Write("Executing... pid: " + pid);
            ret = cApp.run();

            //Console.Write("ya!");
            ProcessTable.unregisterProcess(pid);

            return ret;

        }

        public static int runApplicationEntryPoint(String aName, byte[] aCode, List<Object> args, String entryPoint) {
            //Kernel.PrintDebug("received request to run " + entryPoint + " @ " + aName);
            Application cApp = new Application(aName, aCode, args);
            //Kernel.PrintDebug("created new Application!");
            int ret = 0;

            int pid = ProcessTable.registerProcess(aName);
            //Kernel.PrintDebug("registered process with pid: " + pid);

            Console.Write("Executing... pid: " + pid);
            ret = cApp.runEntryPoint(entryPoint);

            //Console.Write("ya!");
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
