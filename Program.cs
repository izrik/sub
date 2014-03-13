using System;
using NDesk.Options;
using Substrate;

namespace sub
{
    class MainClass
    {
        static OptionSet _options;

        static bool showHelp = false;
        static bool showVersion = false;
        static bool verbose = false;

        public static void Main(string[] args)
        {
            try
            {
                _options = new OptionSet() {
                    {   "h|?|help",
                        "Print this help text and exit",
                        x => showHelp = true },
                    {   "v|version",
                        "Print version and exit",
                        x => showVersion = true },
                    {   "verbose",
                        "Print extra information with some subcommands",
                        x => verbose = true },
                };

                args = _options.Parse(args).ToArray();


                if (showHelp || args.Length < 1)
                {
                    ShowUsage();
                    return;
                }
                else if (showVersion)
                {
                    ShowVersion();
                    return;
                }

                var command = args[0].ToLower();

                if (command == "help")
                {
                    ShowUsage();
                }
                else if (command == "clear")
                {
                    var folder = args[1];

                    AnvilWorld world = AnvilWorld.Open(folder);
                    BlockManager bm = world.GetBlockManager();

                    int minx = int.Parse(args[2]);
                    int maxx = int.Parse(args[3]);
                    int miny = int.Parse(args[4]);
                    int maxy = int.Parse(args[5]);
                    int minz = int.Parse(args[6]);
                    int maxz = int.Parse(args[7]);

                    bm.ClearSpace(minx, maxx, miny, maxy, minz, maxz);

                    world.Save();
                }
                else if (command == "fill")
                {
                    var folder = args[1];

                    AnvilWorld world = AnvilWorld.Open(folder);
                    BlockManager bm = world.GetBlockManager();

                    int minx = int.Parse(args[2]);
                    int maxx = int.Parse(args[3]);
                    int miny = int.Parse(args[4]);
                    int maxy = int.Parse(args[5]);
                    int minz = int.Parse(args[6]);
                    int maxz = int.Parse(args[7]);

                    int blockType = int.Parse(args[8]);

                    if (args.Length > 9)
                    {
                        int data = int.Parse(args[9]);
                        bm.FillSpace(minx, maxx, miny, maxy, minz, maxz, blockType, data);
                    }
                    else
                    {
                        bm.FillSpace(minx, maxx, miny, maxy, minz, maxz, blockType);
                    }

                    world.Save();
                }
                else
                {
                    Console.WriteLine("Unknown command, \"{0}\"", command);
                }
            }
            catch (Exception ex)
            {
                Console.Write("There was an internal error");
                if (verbose)
                {
                    Console.WriteLine(":");
                    Console.WriteLine(ex.ToString());
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }

        static void ShowVersion()
        {
            Console.WriteLine("sub.exe version x.y.z");
        }

        static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("    sub [options]");
            Console.WriteLine("    sub help");
            Console.WriteLine("    sub clear [folder] [args]");
            Console.WriteLine("    sub fill [folder] [args]");
            Console.WriteLine();
            Console.WriteLine("Subcommands:");
            Console.WriteLine();
            Console.WriteLine("    help,           Help");
            Console.WriteLine("    clear,          Clear a space");
            Console.WriteLine("    fill,           Fill a space");
            Console.WriteLine();

            _options.WriteOptionDescriptions(Console.Out);
        }
    }
}
