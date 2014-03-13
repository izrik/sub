using System;
using Substrate;

namespace sub
{
    public static class BlockManagerHelper
    {
        public static void FillSpace(this BlockManager bm,
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

        public static void FillSpace(this BlockManager bm,
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

        public static void ClearSpace(this BlockManager bm,
            int minx, int maxx,
            int miny, int maxy,
            int minz, int maxz)
        {
            FillSpace(bm, minx, maxx, miny, maxy, minz, maxz, BlockType.AIR);
        }

        public static void ReplaceBlockType(this BlockManager bm,
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

        public static void MakeWallPerpendicularToX(this BlockManager bm, int x, int miny, int maxy, int minz, int maxz, int blockType)
        {
            FillSpace(bm, x, x, miny, maxy, minz, maxz, blockType);
        }

        public static void MakeWallPerpendicularToZ(this BlockManager bm, int minx, int maxx, int miny, int maxy, int z, int blockType)
        {
            FillSpace(bm, minx, maxx, miny, maxy, z, z, blockType);
        }

        public static void MakeFloor(this BlockManager bm, int minx, int maxx, int y, int minz, int maxz, int blockType)
        {
            FillSpace(bm, minx, maxx, y, y, minz, maxz, blockType);
        }
    }
}

