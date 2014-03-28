using System;
using NDesk.Options;
using Substrate;
using NCommander;
using System.Reflection;

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

                var commander = new Commander("sub", GetVersionStringFromAssembly());
                commander.AdditionalUsage = () => {
                     Console.WriteLine("Options:");
                     Console.WriteLine();
                     _options.WriteOptionDescriptions(Console.Out);
                     Console.WriteLine();
                };

                commander.Commands.Add(Commands.ClearCommand.Name, Commands.ClearCommand);
                commander.Commands.Add(Commands.FillCommand.Name, Commands.FillCommand);
                commander.Commands.Add(Commands.SetDataCommand.Name, Commands.SetDataCommand);
                commander.Commands.Add(Commands.ReplaceCommand.Name, Commands.ReplaceCommand);
                commander.Commands.Add(Commands.SphereCommand.Name, Commands.SphereCommand);
                commander.Commands.Add(Commands.TorchGridCommand.Name, Commands.TorchGridCommand);
                commander.Commands.Add(Commands.DisruptCommand.Name, Commands.DisruptCommand);
                commander.Commands.Add(Commands.SliceXYCommand.Name, Commands.SliceXYCommand);
                commander.Commands.Add(Commands.MapCommand.Name, Commands.MapCommand);


                if (showVersion)
                {
                    commander.ShowVersion();
                    return;
                }
                else if (showHelp || args.Length < 1)
                {
                    commander.ShowUsage();
                    return;
                }

                commander.ProcessArgs(args);
            }
            catch (Exception ex)
            {
                Console.Write("There was an internal error");
                if (verbose)
                {
                    Console.WriteLine(":");
                    Console.WriteLine(ex);
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }

        public static string GetVersionStringFromAssembly()
        {
            return Assembly.GetCallingAssembly().GetName().Version.ToString();
        }
    }
}
