using System;
using NCommander;
using Substrate;

namespace sub
{
    public class Commands
    {
        public static readonly Command ClearCommand = 
            new Command {
                Name = "clear",
                Description = "Clear a space",
                HelpText = "Clear a space",
                Params = new [] {
                    new Parameter{ Name = "world", ParameterType = ParameterTypes.World },
                    new Parameter{ Name = "minx",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "maxx",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "miny",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "maxy",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "minz",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "maxz",  ParameterType = ParameterType.Integer },
                },
                ExecuteDelegate = args =>
                {
                    AnvilWorld world = (AnvilWorld)(args["world"]);
                    int minx = (int)(args["minx"]);
                    int maxx = (int)(args["maxx"]);
                    int miny = (int)(args["miny"]);
                    int maxy = (int)(args["maxy"]);
                    int minz = (int)(args["minz"]);
                    int maxz = (int)(args["maxz"]);

                    BlockManager bm = world.GetBlockManager();

                    bm.ClearSpace(minx, maxx, miny, maxy, minz, maxz);

                    world.Save();
                }
            };

        public static readonly Command FillCommand =
            new Command {
                Name = "fill",
                Description = "Fill a space",
                HelpText = "Fill a space",
                Params = new [] {
                    new Parameter{ Name = "world",      ParameterType = ParameterTypes.World },
                    new Parameter{ Name = "minx",       ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "maxx",       ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "miny",       ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "maxy",       ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "minz",       ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "maxz",       ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "block_type", ParameterType = ParameterTypes.BlockType },
                    new Parameter{ Name = "data",       ParameterType = ParameterType.Integer, IsOptional = true },
                },
                ExecuteDelegate = args =>
                {
                    AnvilWorld world = (AnvilWorld)(args["world"]);
                    int minx = (int)(args["minx"]);
                    int maxx = (int)(args["maxx"]);
                    int miny = (int)(args["miny"]);
                    int maxy = (int)(args["maxy"]);
                    int minz = (int)(args["minz"]);
                    int maxz = (int)(args["maxz"]);
                    int blockType = (int)(args["block_type"]);

                    BlockManager bm = world.GetBlockManager();

                    bm.FillSpace(minx, maxx, miny, maxy, minz, maxz, blockType);

                    world.Save();
                }
            };
    }
}

