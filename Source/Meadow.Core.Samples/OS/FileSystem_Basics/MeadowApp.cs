using System;
using System.IO;
using System.Threading;
using Meadow;
using Meadow.Devices;

namespace MeadowApp
{
    public class MeadowApp : App<F7FeatherV2, MeadowApp>
    {
        public MeadowApp()
        {
            Console.WriteLine("Meadow File System Tests");
            // list out the named directories available at MeadowOS.FileSystem.[x]
            EnumerateNamedDirectories();

            // get the size of the `app.exe` file.
            FileStatus(Path.Combine(MeadowOS.FileSystem.UserFileSystemRoot, "App.exe"));

            // create a `hello.txt` file in the `/Temp` directory
            CreateFile(MeadowOS.FileSystem.TempDirectory, "hello.txt");

            // check on that file.
            FileStatus(Path.Combine(MeadowOS.FileSystem.TempDirectory, "hello.txt"));

            //Tree("/", true);
            // write out a tree of all files in the user file system root
            Tree(MeadowOS.FileSystem.UserFileSystemRoot, true);

            Console.WriteLine("Testing complete");
        }

        void EnumerateNamedDirectories()
        {
            Console.WriteLine("The following named directories are available:");
            Console.WriteLine($"\t MeadowOS.FileSystem.UserFileSystemRoot: {MeadowOS.FileSystem.UserFileSystemRoot}");
            Console.WriteLine($"\t MeadowOS.FileSystem.CacheDirectory: {MeadowOS.FileSystem.CacheDirectory}");
            Console.WriteLine($"\t MeadowOS.FileSystem.DataDirectory: {MeadowOS.FileSystem.DataDirectory}");
            Console.WriteLine($"\t MeadowOS.FileSystem.DocumentsDirectory: {MeadowOS.FileSystem.DocumentsDirectory}");
            Console.WriteLine($"\t MeadowOS.FileSystem.TempDirectory: {MeadowOS.FileSystem.TempDirectory}");
        }

        private void CreateFile(string path, string filename)
        {
            Console.WriteLine($"Creating '{path}/{filename}'...");

            if (!Directory.Exists(path)) {
                Console.WriteLine("Directory doesn't exist, creating.");
                Directory.CreateDirectory(path);
            }

            try {
                using (var fs = File.CreateText(Path.Combine(path,filename))) {
                    fs.WriteLine("Hello Meadow File!");
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        protected void FileStatus(string path)
        {
            Console.Write($"FileStatus() File: {Path.GetFileName(path)} ");
            try {
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read)) {
                    Console.WriteLine($"Size: {stream.Length,-8}");
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        protected void Tree(string root, bool showSize = false)
        {
            var fileCount = 0;
            var folderCount = 0;

            ShowFolder(root, 0);
            Console.WriteLine(string.Empty);
            Console.WriteLine($"{folderCount} directories");
            Console.WriteLine($"{fileCount} files");

            void ShowFolder(string folder, int depth, bool last = false)
            {
                string[] files = null;

                try {
                    files = Directory.GetFiles(folder);
                    Console.WriteLine($"{GetPrefix(depth, last && files.Length == 0)}{Path.GetFileName(folder)}");
                } catch {
                    Console.WriteLine($"{GetPrefix(depth, last)}{Path.GetFileName(folder)}");
                    Console.WriteLine($"{GetPrefix(depth + 1, last)}<cannot list files>");
                }
                if (files != null) {
                    foreach (var file in files) {
                        var prefix = GetPrefix(depth + 1, last);
                        if (showSize) {
                            FileInfo fi = null;
                            try {
                                fi = new FileInfo(file);
                                prefix += $"[{fi.Length,8}]  ";

                            } catch {
                                prefix += $"[   error]  ";
                            }
                        }
                        Console.WriteLine($"{prefix}{Path.GetFileName(file)}");
                        fileCount++;
                    }
                }

                string[] dirs = null;
                try {
                    dirs = Directory.GetDirectories(folder);
                } catch {
                    if (files == null || files.Length == 0) {
                        Console.WriteLine($"{GetPrefix(depth + 1, last)}<cannot list sub-directories>");
                    } else {
                        Console.WriteLine($"{GetPrefix(depth + 1)}<cannot list sub-directories>");
                    }
                }
                if (dirs != null) {
                    for (var i = 0; i < dirs.Length; i++) {
                        ShowFolder(dirs[i], depth + 1, i == dirs.Length - 1);
                        folderCount++;
                    }
                }

                string GetPrefix(int d, bool isLast = false)
                {
                    var p = string.Empty;

                    for (var i = 0; i < d; i++) {
                        if (i == d - 1) {
                            p += "+--";
                        } else if (isLast && i == d - 2) {
                            p += "   ";
                        } else {
                            p += "|  ";
                        }
                    }

                    return p;
                }
            }
        }

        protected void DirectoryListTest(string path)
        {
            Console.WriteLine($"Enumerating path '{path}'");

            var dirs = Directory.GetDirectories(path);
            Console.WriteLine($" Found {dirs.Length} Directories {((dirs.Length > 0) ? ":" : string.Empty)}");
            foreach (var d in dirs) {
                Console.WriteLine($"   {d}");
            }
            var files = Directory.GetFiles(path);
            Console.WriteLine($" Found {files.Length} Files {((files.Length > 0) ? ":" : string.Empty)}");
            foreach (var f in files) {
                Console.WriteLine($"   {f}");
            }
        }

        protected void DirectoryListTest2()
        {
            Console.WriteLine("Enumerating logical drives...");

            var drives = Directory.GetLogicalDrives();

            Console.WriteLine($" Found {drives.Length} logical drives");

            foreach (var d in drives) {
                Console.WriteLine($"  DRIVE '{d}'");

                ShowFolder(d, 3);

                void ShowFolder(string path, int indent)
                {
                    var name = Path.GetDirectoryName(path);
                    name = string.IsNullOrEmpty(name) ? "/" : name;
                    Console.WriteLine($"{new string(' ', indent)}+ {name}");

                    foreach (var fse in Directory.GetFileSystemEntries(path)) {
                        Console.WriteLine($"{new string(' ', indent + 3)}fse {fse}");
                    }

                    foreach (var dir in Directory.GetDirectories(d)) {
                        ShowFolder(dir, indent + 1);
                    }

                    foreach (var f in Directory.GetFiles(path)) {
                        Console.WriteLine($"{new string(' ', indent + 3)}f{f}");

                        var fi = new FileInfo(f);
                        if (fi.Exists) {
                            Console.WriteLine($"{new string(' ', indent + 4)} Exists as file");
                        }
                        var di = new DirectoryInfo(f);
                        if (fi.Exists) {
                            Console.WriteLine($"{new string(' ', indent + 4)} Exists as directory");
                        }
                    }
                }
            }

            Console.WriteLine("Opening file as a dir...");
            foreach (var f in Directory.GetFiles("/meadow0")) {
                Console.WriteLine($"f {f}");
            }
        }
    }
}