using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using System;
using System.Collections.Generic;

namespace Venera.Shell.Programs
{
    internal class Disk : BuiltIn
    {
        public override string Name => "disk";

        public override string Description => "View all disks and partitions. DO NOT USE";

        public override ExitCode Execute(string[] args)
        {
            List<Cosmos.System.FileSystem.Disk> disks = Kernel.FileSystem.GetDisks();

            foreach (Cosmos.System.FileSystem.Disk disk in disks)
            {
                Console.WriteLine($"{disk.Host.Type} ({disk.Size} bytes:");

                foreach (ManagedPartition part in disk.Partitions)
                {
                    Console.WriteLine($"\t{part.Host.Host.BlockCount}");
                }
            }

            return ExitCode.Success;
        }
    }
}
