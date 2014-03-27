using System;
using NCommander;
using Substrate;
using System.Collections;
using System.Collections.Generic;

namespace sub
{
    public static partial class Commands
    {
        public static void SwapMinMax(ref int min, ref int max)
        {
            if (min > max)
            {
                int temp = min;
                min = max;
                max = temp;
            }
        }

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

                    if (args.ContainsKey("data"))
                    {
                        int data = (int)(args["data"]);
                        bm.FillSpace(minx, maxx, miny, maxy, minz, maxz, blockType, data);
                    }
                    else
                    {
                        bm.FillSpace(minx, maxx, miny, maxy, minz, maxz, blockType);
                    }

                    world.Save();
                }
            };

        public static readonly Command SetDataCommand =
            new Command {
                Name = "set_data",
                Description = "Set the data bits of blocks in a region.",
                HelpText = "Don't set the block type. Just set the data bits.",
                Params = new [] {
                    new Parameter{ Name = "world", ParameterType = ParameterTypes.World },
                    new Parameter{ Name = "minx",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "maxx",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "miny",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "maxy",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "minz",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "maxz",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "data",  ParameterType = ParameterType.Integer },
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
                    int data = (int)(args["data"]);

                    BlockManager bm = world.GetBlockManager();

                    int x;
                    int y;
                    int z;

                    for (x = minx; x <= maxx; x++)
                    {
                        for (z = minz; z <= maxz; z++)
                        {
                            for (y = miny; y < maxy; y++)
                            {
                                bm.SetData(x, y, z, data);
                            }
                        }
                    }

                    world.Save();
                }
            };

        public static readonly Command ReplaceCommand = 
            new Command {
                Name = "replace",
                Description = "Replace one block type with another",
                HelpText = "Search through a rectangular region. Whenever a block of the first block type is found, replace it with a block of the second block type.",
                Params = new [] {
                    new Parameter{ Name = "world", ParameterType = ParameterTypes.World },
                    new Parameter{ Name = "minx",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "maxx",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "miny",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "maxy",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "minz",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "maxz",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "from",  ParameterType = ParameterTypes.BlockType },
                    new Parameter{ Name = "to",    ParameterType = ParameterTypes.BlockType },
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
                    int from = (int)(args["from"]);
                    int to = (int)(args["to"]);

                    BlockManager bm = world.GetBlockManager();

                    bm.ReplaceBlockType(minx, maxx, miny, maxy, minz, maxz, from, to);

                    world.Save();
                }
            };

    }
}

