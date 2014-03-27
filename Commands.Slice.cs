using System;
using NCommander;
using Substrate;

namespace sub
{
    public static partial class Commands
    {
        public static readonly Command SliceXYCommand =
            new Command {
            Name = "slice_xy",
            Description = "Show the block id's of a slice",
            Params = new [] {
                new Parameter{ Name = "world", ParameterType = ParameterTypes.World },
                new Parameter{ Name = "minx",  ParameterType = ParameterType.Integer },
                new Parameter{ Name = "maxx",  ParameterType = ParameterType.Integer },
                new Parameter{ Name = "miny",  ParameterType = ParameterType.Integer },
                new Parameter{ Name = "maxy",  ParameterType = ParameterType.Integer },
                new Parameter{ Name = "z",  ParameterType = ParameterType.Integer },
            },
            ExecuteDelegate = args =>
            {
                AnvilWorld world = (AnvilWorld)(args["world"]);
                int minx = (int)(args["minx"]);
                int maxx = (int)(args["maxx"]);
                int miny = (int)(args["miny"]);
                int maxy = (int)(args["maxy"]);
                int z = (int)(args["z"]);

                SwapMinMax(ref minx, ref maxx);
                SwapMinMax(ref miny, ref maxy);

                int x;
                int y;

                var bm = world.GetBlockManager();

                for (y = miny; y <= maxy; y++)
                {
                    for (x = minx; x <= maxx; x++)
                    {
                        int id = bm.GetID(x, y, z);
                        Console.Write("{0:X02} ", id);
                    }
                    Console.WriteLine();
                }

            }
        };
    }
}

