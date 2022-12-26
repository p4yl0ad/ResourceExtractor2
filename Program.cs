using System;
using System.IO;
using Vestris.ResourceLib;

namespace ResourceExtractor
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                ShowHelp();
                return 1;
            }

            switch (args[0])
            {
                case "list":
                    List(args[1]);
                    return 0;

                case "extract":
                    if (args.Length < 4)
                    {
                        ShowHelp();
                        return 1;
                    }
                    Extract(args[1], args[2], args[3]);
                    return 0;

                case "dump":
                    DumpAll(args[1], args[2]);
                    return 0;

                case "finddrivers":
                    FindDrivers(args[1]);
                    return 0;

                default:
                    ShowHelp();
                    return 1;
            }
        }

        private static void ShowHelp()
        {;
            Console.WriteLine("\nUSAGE: ");
            Console.WriteLine("\nResourceExtractor list <executable>");
            Console.WriteLine("ResourceExtractor extract <executable> <id> <extract to file name>");
            Console.WriteLine("ResourceExtractor dump <executable> <output directory>");
            Console.WriteLine("ResourceExtractor finddrivers <directory to recurse>\n");
        }

        private static void DumpAll(string filename, string hostdirname)
        {
            string sfilename = Path.GetFileNameWithoutExtension(filename);
            Console.WriteLine("[+] Dumping for file: " + sfilename);

            string outdir = Directory.GetCurrentDirectory() + "\\" + hostdirname + "\\" + sfilename;

            if (Directory.Exists(outdir))
            {
                Console.WriteLine("[i] Directory {outdir} exists, using it");
            } else {
                Directory.CreateDirectory(outdir);
            }

            Enumerate(filename, (resourceId, resource) =>
            {
                string newresourceId = resourceId.Replace('/', '_');
                Console.WriteLine("{0}\t{1}\n", newresourceId, resource.Size);

                string outfilepath = outdir + "\\" + newresourceId;
                Console.WriteLine("[+] Writing to file" + outfilepath);

                File.WriteAllBytes(outfilepath, resource.WriteAndGetBytes());
                return false;
            });
        }

        private static void Enumerate(string filename, Func<string, Resource, bool> visit)
        {
            using (var resources = new ResourceInfo())
            {
                resources.Load(filename);

                foreach (var resource in resources)
                {
                    var resourceId = string.Format("{0}/{1}/{2}", resource.Type.TypeName, resource.Name, resource.Language);

                    if (visit(resourceId, resource))
                    {
                        return;
                    }
                }
            }
        }
        
        private static void FindDrivers(string path)
        {
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                FileAttributes attr = File.GetAttributes(file);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    Console.WriteLine($"[+] {file} is a directory");
                }
                else
                {
                    byte[] buffer = new byte[2];
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        var bytes_read = fs.Read(buffer, 0, buffer.Length);
                        fs.Close();

                        if (bytes_read != buffer.Length)
                        {
                            Console.WriteLine("[-] Unable to read bytes");
                        }
                        else
                        {
                            if (buffer[0] == 0x4d && buffer[1] == 0x5A)
                            {
                                Console.WriteLine($"\t\t[+] Found PE file {file}");
                                var peHeader = new PeNet.PeFile(file);

                                foreach (var imp in peHeader.ImportedFunctions)
                                {

                                    // IoCreateDevice
                                    // IoAttachDeviceToDeviceStack

                                    // MmMapIoSpace
                                    // MmUnmapIoSpace
                                    // ZwMapViewOfSection
                                    // ZwUnmapViewOfSection
                                    // MmGetPhysicalAddress
                                    // MmBuildMdlForNonPagedPool
                                    // MmMapLockedPages
                                    // ExAllocatePoolWithTag
                                    // ExFreePoolWithTag
                                    // MmGetSystemRoutineAddress
                                    // MmProbeAndLockPages
                                    // KeStackAttachProcess
                                    // RtlCompareMemory
                                    // IoAllocateMdl
                                    // MmProbeAndLockPages
                                    // MmMapLockedPagesSpecifyCache
                                    // MmMapIoSpaceEx
                                    // KeInitializeApc
                                    // KeInsertQueueApc
                                    // MmProbeAndLockPages
                                    // MmMapLockedPagesSpecifyCache
                                    // KeStackAttachProcess


                                    Console.WriteLine($"{imp.DLL} - {imp.Name} - {imp.Hint} - {imp.IATOffset}");
                                }

                                //foreach (var exp in peHeader.ExportedFunctions)
                                //{
                                //    Console.WriteLine($"{exp.Name}");
                                //}

                            }
                        }
                    }
                }
            }
        }

        private static void List(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("The executable {0} does not exist", filename);
                Environment.Exit(1);
            }

            Enumerate(filename, (resourceId, resource) =>
            {
                Console.WriteLine("{0}\t{1}", resourceId, resource.Size);
                return false;
            });
        }

        private static void Extract(string filename, string id, string destinationFilename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("The executable {0} does not exist", filename);
                Environment.Exit(1);
            }

            Enumerate(filename, (resourceId, resource) =>
            {
                if (resourceId == id)
                {
                    File.WriteAllBytes(destinationFilename, resource.WriteAndGetBytes());
                    return true;
                }
                return false;
            });
        }



    }
}
