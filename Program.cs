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

                var commands = new [] {
                    Commands.ClearCommand,
                    Commands.FillCommand,
                    Commands.SetDataCommand,
                    Commands.ReplaceCommand,
                    Commands.SphereCommand,
                    Commands.TorchGridCommand,
                    Commands.DisruptCommand,
                    Commands.SliceXYCommand,
                    Commands.MapCommand,
                    Commands.HeightMapCommand,
                    new CreateBuildingCommand(),
                    new MengerSpongeCommand(),
                    new SierpinskiTetrahedronCommand(),
                    new GaussianCommand(),
                };

                foreach (var command in commands)
                {
                    commander.Commands.Add(command.Name, command);
                }

                commander.HelpTopics["block_type_number"] = () => {

                    Console.Write("block_type_number");

                    var  parameterType = ParameterTypes.BlockType;
                    if (!string.IsNullOrWhiteSpace(parameterType.Description))
                    {
                        Console.Write(" - {0}", parameterType.Description);
                    }

                    Console.WriteLine();
                    Console.WriteLine();

                    if (!string.IsNullOrWhiteSpace(parameterType.HelpText))
                    {
                        Console.WriteLine("The available block types are:");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine(ParameterTypes.GetListOfBlockTypes(true));
                        Console.WriteLine();
                    }
                };

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
