using System;
using NCommander;
using Substrate;
using System.Collections.Generic;

namespace sub
{
    public class GaussianCommand : Command
    {
        public GaussianCommand()
        {
            Name = "gaussian";
            Params = new [] {
                new Parameter{ Name = "world", ParameterType = ParameterTypes.World },
            };
        }

        protected override void InternalExecute(System.Collections.Generic.Dictionary<string, object> args)
        {
            var world = (AnvilWorld)args["world"];

            var bm = world.GetBlockManager();

            int maxx = 225;
            int maxz = 275;
            var pts = new Queue<Point>();
            var good = new HashSet<Point>();
            var outside = new HashSet<Point>();
            var bad = new HashSet<Point>();

            pts.Enqueue(new Point(maxx, 0, maxz));
            var tops = new Dictionary<Point, int>();
            int lastBadHeight = 64;

            while (pts.Count > 0)
            {
                var pt = pts.Dequeue();
                if (pt.X > maxx+5 || pt.Z > maxz+5)
                    continue;
                if (good.Contains(pt) ||
                    bad.Contains(pt) ||
                    outside.Contains(pt))
                    continue;

                var type = bm.GetBlockTypeAtSurface(pt.X, pt.Z);
                if (type == BlockType.WATER ||
                    type == BlockType.STATIONARY_WATER ||
                    type == BlockType.AIR)
                {
                    lastBadHeight = bm.GetHeight(pt.X, pt.Z);
                    bad.Add(pt);
                }
                else
                {
                    if (type == BlockType.GRASS ||
                        type == BlockType.SAND ||
                        type == BlockType.DIRT ||
                        type == BlockType.STONE)
                    {
                        tops[pt] = type;
                    }
                    else
                    {
                        tops[pt] = BlockType.GRASS;
                    }

                    if (pt.X > maxx || pt.Z > maxz)
                    {
                        outside.Add(pt);
                    }
                    else
                    {
                        good.Add(pt);
                    }
                    pts.Enqueue(new Point(pt.X - 1, 0, pt.Z));
                    pts.Enqueue(new Point(pt.X + 1, 0, pt.Z));
                    pts.Enqueue(new Point(pt.X, 0, pt.Z - 1));
                    pts.Enqueue(new Point(pt.X, 0, pt.Z + 1));
                }
            }

            var heights = new Dictionary<Point,int>();
            foreach (var pt in good)
            {
                heights[pt] = bm.GetHeight(pt.X, pt.Z);
            }
            foreach (var pt in outside)
            {
                heights[pt] = bm.GetHeight(pt.X, pt.Z);
            }


            const float zz = 3;
            const int width = 25;
            const int width2 = width * width;
            const float sigma = width / zz;
            const float sigma2 = sigma * sigma;
            float A = (float)(1 / (sigma * Math.Sqrt(2 * Math.PI))); 

            int k = 0;
            foreach (var pt in good)
            {
                k++;

                var x = pt.X;
                var z = pt.Z;
                float total = 0;
                float n = 0;

//                int dx;
//                int dz;
//                for (dx = -width; dx <= width; dx++)
//                {
//                    var dx2 = dx * dx;
//                    for (dz = -width; dz <= width; dz++)
//                    {
//                        var dz2 = dz * dz;
//                        var s = dx2 + dz2;
//                        if (s > width2)
//                            continue;
//                        float w = (float)(A * Math.Exp(s / sigma2));
//                        update(heights, x, z, ref total, ref n, w);
//                    }
//                }
//
//                int h;
//                if (n > 0)
//                {
//                    h = (int)Math.Round(total / n);
//                }
//                else
//                {
//                    h = heights[pt];
//                }
                int h = lastBadHeight;

                if (h < 1)
                    h = 1;


                int y;
                for (y = 255; y >= h; y--)
                {
                    bm.SetID(x, y, z, BlockType.AIR);
                }
                var h2 = heights[pt];
                for (y = h - 1; y > h2; y--)
                {
                    bm.SetID(x, y, z, BlockType.DIRT);
                }
                bm.SetID(x, h-1, z, tops[pt]);
            }

            world.Save();
        }

        static void update(Dictionary<Point,int> heights, int x, int z, ref float total, ref float n, float w)
        {
            var npt = new Point(x, 0, z);
            if (heights.ContainsKey(npt))
            {
                total += heights[npt] * w;
                n += w;
            }
        }
    }
}

