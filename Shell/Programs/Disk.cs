using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using System;
using System.Collections.Generic;

namespace Venera.Shell.Programs
{
    internal class Disk : BuiltIn
    {
        public override string Name => "disk";

        public override string Description => "View all disks and partitions.";

        public override ExitCode Execute(string[] args)
        {
            List<Cosmos.System.FileSystem.Disk> disks = Kernel.FileSystem.GetDisks();

            foreach (Cosmos.System.FileSystem.Disk disk in disks)
            {
                string type = disk.Host.Type switch
                {
                    //determining the Type of the mounted disk
                    Cosmos.HAL.BlockDevice.BlockDeviceType.HardDrive => "HDD",
                    Cosmos.HAL.BlockDevice.BlockDeviceType.RemovableCD => "CD",
                    _ => "Removable"
                };

                Console.WriteLine($"{type} ({disk.Size} bytes:");


                foreach (ManagedPartition part in disk.Partitions)
                {
                    Console.WriteLine($"\t{part.Host.Host.BlockCount}");
                }
            }

            return ExitCode.Success;
        }
    }
}
