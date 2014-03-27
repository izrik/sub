using System;
using NCommander;
using Substrate;
using System.Collections;
using System.Collections.Generic;

namespace sub
{
    public class Commands
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

        public static readonly Command SphereCommand =
            new Command {
                Name = "sphere",
                Description = "Generate a sphere",
                HelpText = "Generate a sphere",
                Params = new [] {
                    new Parameter{ Name = "world",      ParameterType = ParameterTypes.World },
                    new Parameter{ Name = "cx",         ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "cy",         ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "cz",         ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "radius",     ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "block_type", ParameterType = ParameterTypes.BlockType },
                },
                ExecuteDelegate = args =>
                {

                    AnvilWorld world = (AnvilWorld)(args["world"]);
                    int cx = (int)(args["cx"]);
                    int cy = (int)(args["cy"]);
                    int cz = (int)(args["cz"]);
                    int radius = (int)(args["radius"]);
                    int blockType = (int)(args["block_type"]);

                    int minx = cx - radius;
                    int maxx = cx + radius;
                    int miny = Math.Max(cy - radius, 1);
                    int maxy = Math.Min(cy + radius, 255);
                    int minz = cz - radius;
                    int maxz = cz + radius;
                    int r2 = radius * radius;

                    SwapMinMax(ref minx, ref maxx);
                    SwapMinMax(ref miny, ref maxy);
                    SwapMinMax(ref minz, ref maxz);

                    int x;
                    int y;
                    int z;

                    BlockManager bm = world.GetBlockManager();

                    int nx = maxx - minx + 1;

                    for (x = minx; x <= maxx; x++)
                    {
                        Console.Write('.');
                        int dx = cx - x;
                        int dx2 = dx * dx;
                        for (z = minz; z <= maxz; z++)
                        {
                            int dz = cz - z;
                            int dz2 = dz * dz;

                            if (dx2 + dz2 > r2)
                                continue;

                            for (y = miny; y <= maxy; y++)
                            {
                                int dy = cy - y;
                                int dy2 = dy * dy;

                                if (dx2 + dy2 + dz2 <= r2)
                                {
                                    bm.SetID(x, y, z, blockType);
                                }
                            }
                        }
                    }

                    world.Save();
                }
            };

        public static readonly Command TorchGridCommand = 
            new Command {
                Name = "torch_grid",
                Description = "Place torches at intervals",
                HelpText = "Place torches at intervals",
                Params = new [] {
                    new Parameter{ Name = "world", ParameterType = ParameterTypes.World },
                    new Parameter{ Name = "minx",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "maxx",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "modx",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "y",     ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "minz",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "maxz",  ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "modz",  ParameterType = ParameterType.Integer },
                },
                ExecuteDelegate = args =>
                {
                    AnvilWorld world = (AnvilWorld)(args["world"]);
                    int minx = (int)(args["minx"]);
                    int maxx = (int)(args["maxx"]);
                    int modx = (int)(args["modx"]);
                    int y = (int)(args["y"]);
                    int minz = (int)(args["minz"]);
                    int maxz = (int)(args["maxz"]);
                    int modz = (int)(args["modz"]);

                    SwapMinMax(ref minx, ref maxx);
                    SwapMinMax(ref minz, ref maxz);

                    BlockManager bm = world.GetBlockManager();

                    int x;
                    int z;

                    for (x = minx; x <= maxx; x++)
                    {
                        if (x % modx != 0)
                            continue;

                        for (z = minz; z <= maxz; z++)
                        {
                            if (z % modz != 0)
                                continue;

                            bm.SetID(x, y, z, BlockType.TORCH);
                            bm.SetData(x, y, z, (int)TorchOrientation.FLOOR);
                        }
                    }

                    world.Save();
                }
            };

        struct Point
        {
            public Point(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public int X;
            public int Y;
            public int Z;
        }
        public static readonly Command DisruptCommand =
            new Command {
                Name = "disrupt",
                Description = "The matter disrupter, a.k.a. the \"Little Doctor\"",
                //          12345678901234567890123456789012345678901234567890123456789012345678901234567890
                HelpText = "All connected blocks will be cleared. The disruptor starts at the given point " +
                    "and starts deleting blocks. The process then 'spreads' to adjacent blocks, which are " +
                    "deleted. It continues in this way, and any connected blocks that are not air continue " +
                    "the reaction, until there are no more blocks left. A bounding region is required, so " +
                    "that the reaction doesn't consume the entire world.",
                Params = new [] {
                    new Parameter{ Name = "world", ParameterType = ParameterTypes.World },
                    new Parameter{ Name = "sx", ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "sy", ParameterType = ParameterType.Integer },
                    new Parameter{ Name = "sz", ParameterType = ParameterType.Integer },
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
                    int sx = (int)(args["sx"]);
                    int sy = (int)(args["sy"]);
                    int sz = (int)(args["sz"]);
                    int minx = (int)(args["minx"]);
                    int maxx = (int)(args["maxx"]);
                    int miny = (int)(args["miny"]);
                    int maxy = (int)(args["maxy"]);
                    int minz = (int)(args["minz"]);
                    int maxz = (int)(args["maxz"]);

                    int x;
                    int y;
                    int z;

                    miny = Math.Max(Math.Min(miny, 255), 0);
                    maxy = Math.Max(Math.Min(maxy, 255), 0);

                    SwapMinMax(ref minx, ref maxx);
                    SwapMinMax(ref miny, ref maxy);
                    SwapMinMax(ref minz, ref maxz);

                    BlockManager bm = world.GetBlockManager();

                    var q = new Queue<Point>();
                    q.Enqueue(new Point(sx, sy, sz));

                    while (q.Count > 0)
                    {
                        var pt = q.Dequeue();
                        x = pt.X;
                        y = pt.Y;
                        z = pt.Z;
                        if (x >= minx && x <= maxx &&
                            y >= miny && y <= maxy &&
                            z >= minz && z <= maxz &&
                            bm.GetID(x ,y, z) != BlockType.AIR)
                        {
                            bm.SetID(x, y, z, BlockType.AIR);
                            q.Enqueue(new Point(x+1,y,z));
                            q.Enqueue(new Point(x-1,y,z));
                            q.Enqueue(new Point(x,y+1,z));
                            q.Enqueue(new Point(x,y-1,z));
                            q.Enqueue(new Point(x,y,z+1));
                            q.Enqueue(new Point(x,y,z-1));
                        }
                    }

                    world.Save();
                }
            };

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

