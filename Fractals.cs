using System;
using Substrate;
using System.Collections.Generic;
using NCommander;

namespace sub
{
    public static class Fractals
    {

        public static void ClearSpace(BlockManager bm,
            int minx, int maxx,
            int miny, int maxy,
            int minz, int maxz,
            int wallHeight, int wallMaterial)
        {
            int x;
            int y;
            int z;

            for (x = minx; x <= maxx; x++)
            {
                for (z = minz; z <= maxz; z++)
                {
                    for (y = miny; y <= maxy; y++)
                    {
                        bm.SetID(x, y, z, BlockType.AIR);
                    }
                }
            }

            for (x = minx; x <= maxx; x++)
            {
                for (y = 1; y <= wallHeight; y++)
                {
                    bm.SetID(x, y, minz, wallMaterial);
                    bm.SetBlockLight(x, y, minz, 0);
                    bm.SetSkyLight(x, y, minz, 15);
                    bm.SetID(x, y, maxz, wallMaterial);
                    bm.SetBlockLight(x, y, maxz, 0);
                    bm.SetSkyLight(x, y, maxz, 15);
                }
            }

            for (z = minz; z <= maxz; z++)
            {
                for (y = 1; y <= wallHeight; y++)
                {
                    bm.SetID(minx, y, z, wallMaterial);
                    bm.SetBlockLight(minx, y, z, 0);
                    bm.SetSkyLight(minx, y, z, 15);
                    bm.SetID(maxx, y, z, wallMaterial);
                    bm.SetBlockLight(maxx, y, z, 0);
                    bm.SetSkyLight(maxx, y, z, 15);
                }
            }

            for (x = minx; x <= maxx; x++)
            {
                for (z = minz; z <= maxz; z++)
                {
                    bm.SetID(x, 0, z, wallMaterial);
                    bm.SetBlockLight(x, 0, z, 0);
                    bm.SetSkyLight(x, 0, z, 15);
                }
            }
        }
    }

    public class MengerSpongeCommand : Command
    {
        public MengerSpongeCommand()
        {
            Name = "menger_sponge";
            Params = new [] {
                new Parameter { Name="world", ParameterType=ParameterTypes.World },
                new Parameter { Name="size", ParameterType=ParameterType.Integer },
                new Parameter { Name="minx", ParameterType=ParameterType.Integer },
                new Parameter { Name="miny", ParameterType=ParameterType.Integer },
                new Parameter { Name="minz", ParameterType=ParameterType.Integer },
            };
        }

        protected override void InternalExecute(Dictionary<string, object> args)
        {
            var world = (AnvilWorld)args["world"];
            var size = (int)args["size"];
            var minx = (int)args["minx"];
            var miny = (int)args["miny"];
            var minz = (int)args["minz"];

            var bm = world.GetBlockManager();

            Fractals.ClearSpace(bm, minx - 2, minx + size + 2, miny - 2, miny + size + 2, minz - 2, minz + size + 2, 0, 1);
            MengerSponge.RenderStatic(bm, size, minx, miny, minz);

            world.Save();
        }
    }

    public class SierpinskiTetrahedronCommand : Command
    {
        public SierpinskiTetrahedronCommand()
        {
            Name = "sierptetra";
            Description = "Sierpinski tetrahedron";
            Params = new [] {
                new Parameter { Name="world", ParameterType=ParameterTypes.World },
                new Parameter { Name="size", ParameterType=ParameterType.Integer },
                new Parameter { Name="minx", ParameterType=ParameterType.Integer },
                new Parameter { Name="miny", ParameterType=ParameterType.Integer },
                new Parameter { Name="minz", ParameterType=ParameterType.Integer },
            };
        }

        protected override void InternalExecute(Dictionary<string, object> args)
        {
            var world = (AnvilWorld)args["world"];
            var size = (int)args["size"];
            var minx = (int)args["minx"];
            var miny = (int)args["miny"];
            var minz = (int)args["minz"];

            var bm = world.GetBlockManager();

            Fractals.ClearSpace(bm, minx - 2, minx + size + 2, miny - 2, miny + size + 2, minz - 2, minz + size + 2, 0, 1);
            SierpinskiTetrahedron.RenderStatic(bm, size, minx, miny, minz);

            world.Save();
        }
    }

    public struct Point
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

    public static class BlockManagerHelper2
    {
        public static void SetID(this BlockManager bm, Point position, int id)
        {
            bm.SetID(position.X, position.Y, position.Z, id);
        }
    }

    public class MengerSponge
    {
        public int Size;
        public Point Position;
        public int X
        {
            get { return Position.X; }
            set { Position.X = value; }
        }
        public int Y
        {
            get { return Position.Y; }
            set { Position.Y = value; }
        }
        public int Z
        {
            get { return Position.Z; }
            set { Position.Z = value; }
        }
        public List<MengerSponge> Subsponges = new List<MengerSponge>();

        public void Render(BlockManager bm)
        {
            if (Size == 1)
            {
                bm.SetID(Position, BlockType.STONE);
            }
            else
            {
                foreach (MengerSponge sub in Subsponges)
                {
                    sub.Render(bm);
                }
            }
        }

        public static MengerSponge Generate(int size, Point position)
        {
            return Generate(size, position.X, position.Y, position.Z);
        }
        public static MengerSponge Generate(int size, int x, int y, int z)
        {
            double s = Math.Log(size, 3);
            double s2 = Math.Floor(Math.Round(s, 3));
            double s3 = Math.Pow(3, s2);
            double s4 = Math.Round(s3);
            size = (int)s4;

            MengerSponge sp = new MengerSponge{Size = size, Position = new Point(x, y, z) };

            if (size > 1)
            {
                int size2 = size / 3;
                int size3 = 2 * size2;

                sp.Subsponges.Add(MengerSponge.Generate(size2, x, y, z));
                sp.Subsponges.Add(MengerSponge.Generate(size2, x, y + size2, z));
                sp.Subsponges.Add(MengerSponge.Generate(size2, x, y + size3, z));

                sp.Subsponges.Add(MengerSponge.Generate(size2, x + size2, y, z));
                sp.Subsponges.Add(MengerSponge.Generate(size2, x + size2, y + size3, z));

                sp.Subsponges.Add(MengerSponge.Generate(size2, x + size3, y, z));
                sp.Subsponges.Add(MengerSponge.Generate(size2, x + size3, y + size2, z));
                sp.Subsponges.Add(MengerSponge.Generate(size2, x + size3, y + size3, z));

                sp.Subsponges.Add(MengerSponge.Generate(size2, x + size3, y, z + size2));
                sp.Subsponges.Add(MengerSponge.Generate(size2, x + size3, y + size3, z + size2));

                sp.Subsponges.Add(MengerSponge.Generate(size2, x + size3, y, z + size3));
                sp.Subsponges.Add(MengerSponge.Generate(size2, x + size3, y + size2, z + size3));
                sp.Subsponges.Add(MengerSponge.Generate(size2, x + size3, y + size3, z + size3));

                sp.Subsponges.Add(MengerSponge.Generate(size2, x + size2, y, z + size3));
                sp.Subsponges.Add(MengerSponge.Generate(size2, x + size2, y + size3, z + size3));

                sp.Subsponges.Add(MengerSponge.Generate(size2, x, y, z + size3));
                sp.Subsponges.Add(MengerSponge.Generate(size2, x, y + size2, z + size3));
                sp.Subsponges.Add(MengerSponge.Generate(size2, x, y + size3, z + size3));

                sp.Subsponges.Add(MengerSponge.Generate(size2, x, y, z + size2));
                sp.Subsponges.Add(MengerSponge.Generate(size2, x, y + size3, z + size2));
            }

            return sp;
        }

        public static void RenderStatic(BlockManager bm, int size, int x, int y, int z)
        {
            double s = Math.Log(size, 3);
            double s2 = Math.Floor(Math.Round(s, 3));
            double s3 = Math.Pow(3, s2);
            double s4 = Math.Round(s3);

            RenderStatic2(bm, (int)(s4), x, y, z);
        }
        public static void RenderStatic2(BlockManager bm, int size, int x, int y, int z)
        {
            if (size > 1)
            {
                int size2 = size / 3;
                int size3 = 2 * size2;

                RenderStatic2(bm, size2, x, y, z);
                RenderStatic2(bm, size2, x, y + size2, z);
                RenderStatic2(bm, size2, x, y + size3, z);

                RenderStatic2(bm, size2, x + size2, y, z);
                RenderStatic2(bm, size2, x + size2, y + size3, z);

                RenderStatic2(bm, size2, x + size3, y, z);
                RenderStatic2(bm, size2, x + size3, y + size2, z);
                RenderStatic2(bm, size2, x + size3, y + size3, z);

                RenderStatic2(bm, size2, x + size3, y, z + size2);
                RenderStatic2(bm, size2, x + size3, y + size3, z + size2);

                RenderStatic2(bm, size2, x + size3, y, z + size3);
                RenderStatic2(bm, size2, x + size3, y + size2, z + size3);
                RenderStatic2(bm, size2, x + size3, y + size3, z + size3);

                RenderStatic2(bm, size2, x + size2, y, z + size3);
                RenderStatic2(bm, size2, x + size2, y + size3, z + size3);

                RenderStatic2(bm, size2, x, y, z + size3);
                RenderStatic2(bm, size2, x, y + size2, z + size3);
                RenderStatic2(bm, size2, x, y + size3, z + size3);

                RenderStatic2(bm, size2, x, y, z + size2);
                RenderStatic2(bm, size2, x, y + size3, z + size2);
            }
            else
            {
                bm.SetID(x, y, z, BlockType.STONE);
//                Console.WriteLine("{0}, {1}, {2}", x, y, z);
            }
        }
    }

    public class SierpinskiTetrahedron
    {
        public static void RenderStatic(BlockManager bm, int size, int x, int y, int z)
        {
            if (size == 3)
            {
                Console.WriteLine("{0}, {1}, {2}", x, y, z);
                Point[] ps = {
                    new Point(x,y,z),
                    new Point(x+1,y,z),
                    new Point(x+1,y+1,z),
                    new Point(x+2,y,z),
                    new Point(x,y+1,z),
                    new Point(x,y+2,z),
                    new Point(x,y,z+1),
                    new Point(x,y+1,z+1),
                    new Point(x,y,z+2),
                    new Point(x+1,y,z+1),
                };

                foreach (Point p in ps)
                {
                    bm.SetID(p.X, p.Y, p.Z, BlockType.STONE);
                    bm.SetBlockLight(p.X, p.Y, p.Z, 15);
                    bm.SetSkyLight(p.X, p.Y, p.Z, 15);
                }
            }
            else
            {
                int size2 = size/2;
                RenderStatic(bm, size2, x, y, z);
                RenderStatic(bm, size2, x, y + size2, z);
                RenderStatic(bm, size2, x + size2, y, z);
                RenderStatic(bm, size2, x, y, z + size2);
            }
        }
    }
}
