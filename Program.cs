using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeployCopy
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            if (args.Length < 2)
            {
                Console.WriteLine("quickdeploycopy.exe source target");
                return;
            }

            Console.WriteLine("Load " + args[1]);
            string[] targets = null;
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + args[1]))
            {
                targets = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + args[1]);
            }
            else
            {
                if (File.Exists(args[1]))
                {
                    targets = File.ReadAllLines(args[1]);
                }
            }
            if (targets == null)
            {
                Console.WriteLine("target file not existed");
                return;
            }
            var tdirs = targets.Where(x => !string.IsNullOrEmpty(x)).ToList();


            //Source
            Console.WriteLine("Load source.txt");
            var source = "";
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + args[1]))
            {
                source = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + args[0]);
            }
            else
            {
                if (File.Exists(args[0]))
                {
                    source = File.ReadAllText(args[0]);
                }
            }
            if (string.IsNullOrEmpty(source))
            {
                Console.WriteLine("source file not existed");
                return;

            }


            foreach (var t in targets)
            {

                CopyFilesRecursively(new DirectoryInfo(source), new DirectoryInfo(t));

            }


            Console.WriteLine("SUCCESS !!!");

            Console.ReadLine();

        }

        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {

            Parallel.ForEach(source.GetDirectories(), (dir) =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Copy Dir : " + dir + " => " + target.FullName + dir.Name + "\r\n");
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));

            });


            Parallel.ForEach(source.GetFiles(), (file) =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Copy File : " + file.FullName + "=> " + Path.Combine(target.FullName, file.Name) + "\r\n");
                file.CopyTo(Path.Combine(target.FullName, file.Name), true);

            });
            Console.ForegroundColor = ConsoleColor.White;
        }





    }
}
