using System;
using NCommander;
using Substrate;
using System.Collections.Generic;
using System.Linq;

namespace sub
{
    public static partial class Commands
    {
        public static readonly Command MapCommand =
            new Command {
                Name = "map",
                Description = "Draw a map of an area",
                Params = new [] {
                    new Parameter{ Name = "world", ParameterType = ParameterTypes.World },
                    new Parameter{ Name = "minx",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "maxx",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "minz",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "maxz",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "scale",  ParameterType = ParameterType.Integer, IsOptional = true },
                },
                ExecuteDelegate = args =>
                {
                    AnvilWorld world = (AnvilWorld)(args["world"]);
                    int minx = (int)(args["minx"]);
                    int maxx = (int)(args["maxx"]);
                    int minz = (int)(args["minz"]);
                    int maxz = (int)(args["maxz"]);
                    int scale = 8;
                    if (args.ContainsKey("scale"))
                    {
                        scale = (int)(args["scale"]);
                    }

                    SwapMinMax(ref minx, ref maxx);
                    SwapMinMax(ref minz, ref maxz);

                    maxx += ((scale - ((maxx - minx) % scale)) % scale);
                    maxz += ((scale - ((maxz - minz) % scale)) % scale);

                    int x;
                    int y;
                    int z;
                    int dx;
                    int dz;
                    int i;

                    int id = 0;

                    var bm = world.GetBlockManager();

                    var counts = new Dictionary<int, int>();

                    for (z = minz; z < maxz; z += scale)
                    {
                        for (x = minx; x < maxx; x += scale)
                        {
                            for (i = 0; i < 256; i++)
                            {
                                counts[i] = 0;
                            }

                            for (dz = 0; dz < scale; dz++)
                            {
                                for (dx = 0; dx < scale; dx++)
                                {
                                    for (y = 255; y >= 0; y--)
                                    {
                                        id = bm.GetID(x+dx, y, z+dz);
                                        if (id != BlockType.AIR)
                                        {
                                            break;
                                        }
                                    }

                                    counts[id]++;
                                }
                            }

                            id = counts.OrderByDescending(kvp => kvp.Value).First().Key;

                            if (id > 0)
                            {
                                Console.Write("{0:X02} ", id);
                            }
                            else
                            {
                                Console.Write(".. ");
                            }
                        }

                        Console.WriteLine();
                    }

                }

            };


        public static readonly Command HeightMapCommand =
            new Command {
            Name = "height_map",
            Description = "Draw a map of the altitude of an area",
            Params = new [] {
                new Parameter{ Name = "world", ParameterType = ParameterTypes.World },
                new Parameter{ Name = "minx",  ParameterType = ParameterType.Integer },
                new Parameter{ Name = "maxx",  ParameterType = ParameterType.Integer },
                new Parameter{ Name = "minz",  ParameterType = ParameterType.Integer },
                new Parameter{ Name = "maxz",  ParameterType = ParameterType.Integer },
                new Parameter{ Name = "scale",  ParameterType = ParameterType.Integer, IsOptional = true },
            },
            ExecuteDelegate = args =>
            {
                AnvilWorld world = (AnvilWorld)(args["world"]);
                int minx = (int)(args["minx"]);
                int maxx = (int)(args["maxx"]);
                int minz = (int)(args["minz"]);
                int maxz = (int)(args["maxz"]);
                int scale = 8;
                if (args.ContainsKey("scale"))
                {
                    scale = (int)(args["scale"]);
                }

                SwapMinMax(ref minx, ref maxx);
                SwapMinMax(ref minz, ref maxz);

                maxx += ((scale - ((maxx - minx) % scale)) % scale);
                maxz += ((scale - ((maxz - minz) % scale)) % scale);

                int x;
                int y;
                int z;
                int dx;
                int dz;
                int i;

                var bm = world.GetBlockManager();

                var counts = new Dictionary<int, int>();

                for (z = minz; z < maxz; z += scale)
                {
                    for (x = minx; x < maxx; x += scale)
                    {
                        for (i = 0; i < 256; i++)
                        {
                            counts[i] = 0;
                        }

                        for (dz = 0; dz < scale; dz++)
                        {
                            for (dx = 0; dx < scale; dx++)
                            {
                                for (y = 255; y >= 0; y--)
                                {
                                    var id = bm.GetID(x+dx, y, z+dz);
                                    if (id != BlockType.AIR)
                                    {
                                        break;
                                    }
                                }

                                if (y < 0) y = 0;

                                counts[y]++;
                            }
                        }

                        y = counts.OrderByDescending(kvp => kvp.Value).First().Key;

                        if (y > 0)
                        {
                            Console.Write("{0:X02} ", y);
                        }
                        else
                        {
                            Console.Write(".. ");
                        }
                    }

                    Console.WriteLine();
                }

            }

        };
    }
}

