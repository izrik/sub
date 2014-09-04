using System;
using Substrate;
using NCommander;

namespace sub
{
    public class CreateBuildingCommand : Command
    {
        public CreateBuildingCommand()
        {
            Name = "create_building";
            Params = new [] {
                new Parameter { Name="world", ParameterType=ParameterTypes.World, },
                new Parameter { Name="minx", ParameterType=ParameterType.Integer, },
                new Parameter { Name="maxx", ParameterType=ParameterType.Integer, },
                new Parameter { Name="minz", ParameterType=ParameterType.Integer, },
                new Parameter { Name="maxz", ParameterType=ParameterType.Integer, },
                new Parameter { Name="num_floors", ParameterType=ParameterType.Integer, },
            };
        }

        protected override void InternalExecute(System.Collections.Generic.Dictionary<string, object> args)
        {
            var world = (AnvilWorld)(args["world"]);
            int minx = (int)(args["minx"]);
            int maxx = (int)(args["maxx"]);
            int minz = (int)(args["minz"]);
            int maxz = (int)(args["maxz"]);
            int numFloors = (int)args["num_floors"];

            var bm = world.GetBlockManager();

            GenerateOfficeBuilding(bm, minx, maxx, minz, maxz, numFloors);

            world.Save();
        }

        public static void Main(string[] args, int asdf)
        {
            AnvilWorld world = AnvilWorld.Open("/Users/" + System.Environment.UserName + "/Library/Application Support/minecraft/saves/City");
            BlockManager bm = world.GetBlockManager();

            ClearSpace(bm, -12, 11, 64, 255, -12, 11);
            GenerateOfficeBuilding(bm, -10, 9, -10, 9, 10);

            ClearSpace(bm, -9, 8, 64, 255, 14, 31);
            GenerateOfficeBuilding(bm, -8, 7, 15, 30, 16);

            ClearSpace(bm, -14, 13, 64, 255, 39, 54);
            GenerateOfficeBuilding(bm, -13, 12, 40, 53, 6);


            world.Save();

        }

        public static void FillSpace(BlockManager bm,
                                     int minx, int maxx,
                                     int miny, int maxy,
                                     int minz, int maxz,
                                     int blockType)
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
                        bm.SetID(x, y, z, blockType);
                    }
                }
            }
        }
        public static void FillSpace(BlockManager bm,
                                     int minx, int maxx,
                                     int miny, int maxy,
                                     int minz, int maxz,
                                     int blockType, int data)
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
                        bm.SetID(x, y, z, blockType);
                        bm.SetData(x, y, z, data);
                    }
                }
            }
        }

        public static void ClearSpace(BlockManager bm,
                                      int minx, int maxx,
                                      int miny, int maxy,
                                      int minz, int maxz)
        {
            FillSpace(bm, minx, maxx, miny, maxy, minz, maxz, BlockType.AIR);
        }

        public static void ReplaceBlockType(BlockManager bm,
                                           int minx, int maxx,
                                           int miny, int maxy,
                                           int minz, int maxz,
                                           int blockTypeFrom,
                                           int blockTypeTo)
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
                        if (bm.GetID(x, y, z) == blockTypeFrom)
                        {
                            bm.SetID(x, y, z, blockTypeTo);
                        }
                    }
                }
            }
        }

        public static void MakeWallPerpendicularToX(BlockManager bm, int x, int miny, int maxy, int minz, int maxz, int blockType)
        {
            FillSpace(bm, x, x, miny, maxy, minz, maxz, blockType);
        }

        public static void MakeWallPerpendicularToZ(BlockManager bm, int minx, int maxx, int miny, int maxy, int z, int blockType)
        {
            FillSpace(bm, minx, maxx, miny, maxy, z, z, blockType);
        }

        public static void MakeFloor(BlockManager bm, int minx, int maxx, int y, int minz, int maxz, int blockType)
        {
            FillSpace(bm, minx, maxx, y, y, minz, maxz, blockType);
        }

        public static void GenerateOfficeBuilding(BlockManager bm, int minx, int maxx, int minz, int maxz, int numFloors)
        {
            int sizex = maxx - minx + 1;
            int sizez = maxz - minz + 1;
            int miny = bm.GetHeight(minx, minz);
            int maxy = miny;
            int xx;
            int zz;

            for (xx = minx; xx <= maxx; xx++)
            {
                for (zz = minz; zz <= maxz; zz++)
                {
                    int y = bm.GetHeight(xx, zz);
                    miny = Math.Min(miny, y);
                    maxy = Math.Max(maxy, y);
                }
            }

            FillSpace(bm, minx, maxx, miny, maxy, minz-1, maxz, BlockType.AIR, 0);

            MakeFloor(bm, minx, maxx, miny - 1, minz, maxz, BlockType.QUARTZ_BLOCK);

            int n;
            int inner_wx = (sizex - 8) / 2;
            int inner_wz = (sizez - 8) / 2;
            int ix = minx + inner_wx + 1;
            int iz = minz + inner_wz + 1;
            for (n = 0; n < numFloors; n++)
            {
                int y = miny + 4 * n;

                //ceiling
                MakeFloor(bm, minx, maxx, y+3, minz, maxz, BlockType.QUARTZ_BLOCK);

                //outer walls
                MakeWallPerpendicularToX(bm, minx, y + 0, y + 2, minz, maxz, BlockType.GLASS_PANE);
                MakeWallPerpendicularToX(bm, maxx, y + 0, y + 2, minz, maxz, BlockType.GLASS_PANE);
                MakeWallPerpendicularToZ(bm, minx, maxx, y + 0, y + 2, minz, BlockType.GLASS_PANE);
                MakeWallPerpendicularToZ(bm, minx, maxx, y + 0, y + 2, maxz, BlockType.GLASS_PANE);
                for (int yy = y; yy < y + 3; yy++)
                {
                    bm.SetID(minx, yy, minz, BlockType.GLASS);
                    bm.SetID(maxx, yy, minz, BlockType.GLASS);
                    bm.SetID(minx, yy, maxz, BlockType.GLASS);
                    bm.SetID(maxx, yy, maxz, BlockType.GLASS);
                }

                //inner wall
                MakeWallPerpendicularToX(bm, ix, y + 0, y + 3, iz, iz + 5, BlockType.QUARTZ_BLOCK);
                MakeWallPerpendicularToX(bm, ix + 5, y + 0, y + 3, iz, iz + 5, BlockType.QUARTZ_BLOCK);
                MakeWallPerpendicularToZ(bm, ix, ix + 5, y + 0, y + 3, iz, BlockType.QUARTZ_BLOCK);
                MakeWallPerpendicularToZ(bm, ix, ix + 5, y + 0, y + 3, iz + 5, BlockType.QUARTZ_BLOCK);

                //inner door to stairs
                bm.SetID(ix, y + 0, iz + 3, BlockType.AIR);
                bm.SetID(ix, y + 1, iz + 3, BlockType.AIR);

                //stairs
                bm.SetID(ix + 2, y + 3, iz + 1, BlockType.AIR);
                bm.SetID(ix + 3, y + 3, iz + 1, BlockType.AIR);
                bm.SetID(ix + 4, y + 3, iz + 1, BlockType.AIR);
                bm.SetID(ix + 2, y + 3, iz + 2, BlockType.AIR);
                bm.SetID(ix + 3, y + 3, iz + 2, BlockType.AIR);
                bm.SetID(ix + 4, y + 3, iz + 2, BlockType.AIR);
                bm.SetID(ix + 2, y + 3, iz + 3, BlockType.AIR);
                bm.SetID(ix + 3, y + 3, iz + 3, BlockType.AIR);
                bm.SetID(ix + 4, y + 3, iz + 3, BlockType.AIR);
                bm.SetID(ix + 2, y + 3, iz + 4, BlockType.AIR);
                bm.SetID(ix + 3, y + 3, iz + 4, BlockType.AIR);
                bm.SetID(ix + 4, y + 3, iz + 4, BlockType.AIR);
                bm.SetID(        ix + 2, y + 0, iz + 1, BlockType.QUARTZ_STAIRS);
                OrientStairs(bm, ix + 2, y + 0, iz + 1, Direction.East);
                bm.SetID(        ix + 2, y + 0, iz + 2, BlockType.QUARTZ_STAIRS);
                OrientStairs(bm, ix + 2, y + 0, iz + 2, Direction.East);
                bm.SetID(        ix + 3, y + 1, iz + 1, BlockType.QUARTZ_STAIRS);
                OrientStairs(bm, ix + 3, y + 1, iz + 1, Direction.East);
                bm.SetID(        ix + 3, y + 1, iz + 2, BlockType.QUARTZ_STAIRS);
                OrientStairs(bm, ix + 3, y + 1, iz + 2, Direction.East);
                bm.SetID(        ix + 3, y + 2, iz + 3, BlockType.QUARTZ_STAIRS);
                OrientStairs(bm, ix + 3, y + 2, iz + 3, Direction.West);
                bm.SetID(        ix + 3, y + 2, iz + 4, BlockType.QUARTZ_STAIRS);
                OrientStairs(bm, ix + 3, y + 2, iz + 4, Direction.West);
                bm.SetID(        ix + 2, y + 3, iz + 3, BlockType.QUARTZ_STAIRS);
                OrientStairs(bm, ix + 2, y + 3, iz + 3, Direction.West);
                bm.SetID(        ix + 2, y + 3, iz + 4, BlockType.QUARTZ_STAIRS);
                OrientStairs(bm, ix + 2, y + 3, iz + 4, Direction.West);
                //landing
                bm.SetID(        ix + 4, y + 1, iz + 1, BlockType.QUARTZ_BLOCK);
                bm.SetID(        ix + 4, y + 1, iz + 2, BlockType.QUARTZ_BLOCK);
                bm.SetID(        ix + 4, y + 1, iz + 3, BlockType.QUARTZ_BLOCK);
                bm.SetID(        ix + 4, y + 1, iz + 4, BlockType.QUARTZ_BLOCK);
            }

            if (true)
            {
                // access to roof
                // wall
                int y = miny + 4 * numFloors;
                MakeWallPerpendicularToX(bm, ix, y + 0, y + 3, iz, iz + 5, BlockType.QUARTZ_BLOCK);
                MakeWallPerpendicularToX(bm, ix + 5, y + 0, y + 3, iz, iz + 5, BlockType.QUARTZ_BLOCK);
                MakeWallPerpendicularToZ(bm, ix, ix + 5, y + 0, y + 3, iz, BlockType.QUARTZ_BLOCK);
                MakeWallPerpendicularToZ(bm, ix, ix + 5, y + 0, y + 3, iz + 5, BlockType.QUARTZ_BLOCK);
                // door to stairs
                bm.SetID(ix, y + 0, iz + 3, BlockType.AIR);
                bm.SetID(ix, y + 1, iz + 3, BlockType.AIR);
                //ceiling
                MakeFloor(bm, ix, ix + 5, y+3, iz, iz + 5, BlockType.QUARTZ_BLOCK);

            }
        }

        public enum Direction
        {
            North,
            South,
            East,
            West,
        }

        public static void OrientStairs(BlockManager bm, int x, int y, int z, Direction direction)
        {
            int data = 0;
            switch (direction)
            {
            case Direction.North: data = 3; break;
            case Direction.South: data = 2; break;
            case Direction.East: data = 0; break;
            case Direction.West: data = 1; break;
            default: throw new ArgumentOutOfRangeException("direction");
            }
            bm.SetData(x, y, z, data);
        }
    }
}
