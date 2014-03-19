using System;
using NCommander;
using System.Text;
using System.Linq;
using Substrate;

namespace sub
{
    public static class ParameterTypes
    {
        public static readonly ParameterType BlockType = new ParameterType(
            name: "BlockType",
            convertAction: arg =>
            {
                arg = arg.ToLower();
                if (BlockTypesByName._BlockTypesByName.ContainsKey(arg))
                {
                return BlockTypesByName._BlockTypesByName[arg];
                }
    
                return int.Parse(arg);
            },
            description: "The name or number of a kind of block, e.g., air, stone, dirt",
            helpText: "The available block types are:\r\nr\n" + 
                string.Join(
                    "\r\n", 
                    BlockTypesByName._BlockTypesByName.Keys.Select(
                        name => string.Format("  {0}", name))),
            outputType: typeof(int)
        );

        public static readonly ParameterType World = new ParameterType(
            name: "World",
            convertAction: AnvilWorld.Open,
            description: "The path of a folder containing a world's data files",
            outputType: typeof(AnvilWorld)
        );
    }
}

