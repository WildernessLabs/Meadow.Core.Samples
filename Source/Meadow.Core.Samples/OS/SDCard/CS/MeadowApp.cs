using Meadow;
using Meadow.Devices;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SDCard
{
    public class MeadowApp : App<F7FeatherV2>
    {
        public override Task Run()
        {
            if (!Device.PlatformOS.SdCardPresent)
            {
                Resolver.Log.Warn($"SD card is not detected");
                return Task.CompletedTask;
            }

            Resolver.Log.Info($"SD card is mounted at: {MeadowOS.FileSystem.SDCard}");

            var random = new Random();

            var name = $"test_{random.Next(32768)}.txt";

            using (var file = File.CreateText(Path.Combine(MeadowOS.FileSystem.SDCard, name)))
            {
                file.Write("Hello Meadow!");
            }

            Resolver.Log.Info($"Created {name}");

            Tree(MeadowOS.FileSystem.SDCard, true);

            Console.WriteLine("Sample complete");

            return Task.CompletedTask;
        }

        void Tree(string root, bool showSize = false)
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

                try
                {
                    files = Directory.GetFiles(folder);
                    Console.WriteLine($"{GetPrefix(depth, last && files.Length == 0)}{Path.GetFileName(folder)}");
                }
                catch
                {
                    Console.WriteLine($"{GetPrefix(depth, last)}{Path.GetFileName(folder)}");
                    Console.WriteLine($"{GetPrefix(depth + 1, last)}<cannot list files>");
                }
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var prefix = GetPrefix(depth + 1, last);
                        if (showSize)
                        {
                            FileInfo fi = null;
                            try
                            {
                                fi = new FileInfo(file);
                                prefix += $"[{fi.Length,8}]  ";

                            }
                            catch
                            {
                                prefix += $"[   error]  ";
                            }
                        }
                        Console.WriteLine($"{prefix}{Path.GetFileName(file)}");
                        fileCount++;
                    }
                }

                string[] dirs = null;
                try
                {
                    dirs = Directory.GetDirectories(folder);
                }
                catch
                {
                    if (files == null || files.Length == 0)
                    {
                        Console.WriteLine($"{GetPrefix(depth + 1, last)}<cannot list sub-directories>");
                    }
                    else
                    {
                        Console.WriteLine($"{GetPrefix(depth + 1)}<cannot list sub-directories>");
                    }
                }
                if (dirs != null)
                {
                    for (var i = 0; i < dirs.Length; i++)
                    {
                        ShowFolder(dirs[i], depth + 1, i == dirs.Length - 1);
                        folderCount++;
                    }
                }

                string GetPrefix(int d, bool isLast = false)
                {
                    var p = string.Empty;

                    for (var i = 0; i < d; i++)
                    {
                        if (i == d - 1)
                        {
                            p += "+--";
                        }
                        else if (isLast && i == d - 2)
                        {
                            p += "   ";
                        }
                        else
                        {
                            p += "|  ";
                        }
                    }

                    return p;
                }
            }
        }
    }
}