<img src="https://th.bing.com/th/id/OIG2.NKZTAfcTfxiSzuUPiUPP?pid=ImgGn" width="175">


# Venera
A take on operating systems, using CosmOS. 

For instructions on how to compile executables for Venera, see VoPo/CompileGoodies/Readme.md.


## Processes
Processes are de facto ELF executables. They are loaded into memory und executed. 
Processes are registered in a process table, which is a list of processes internally.
To access the list of runned processes, you can either use 'faketop', or read 0:\Sys\PT or each individual process file in 0:\Sys\proc\.
PT is representative of every elf file run since booting, as there is no multitasking yet. Even though, files in Sys\proc\ are created
when a process is registered, and deleted when it is unregistered.
The files have the following format:
```
<PID> <name> <entrypoint> <path of executable> <start of process:date> <start of process:time>
```

All entries are seperated by a space. Each Line is a new process.