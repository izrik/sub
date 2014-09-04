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
            name: "block_type",
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
            helpText: "The available block types are:\r\n\r\n" + GetListOfBlockTypes(),
            outputType: typeof(int)
        );

        static string GetListOfBlockTypes()
        {
            var sb = new StringBuilder();
            string line = "    ";

            var blockTypeNames = BlockTypesByName._BlockTypesByName.Keys.ToList();
            blockTypeNames.Sort();

            int i = 1;
            foreach (var name in blockTypeNames)
            {
                bool last = (i == (blockTypeNames.Count));

                var entry = string.Format(
                                "{0} ({1}/{3}){2}",
                                name,
                                BlockTypesByName._BlockTypesByName[name],
                                (last ? "" : ", "),
                                BlockTypesByName._BlockTypesByName[name].ToString("X02"));

                if (line.Length + entry.Length + 1 > 79)
                {
                    sb.AppendLine(line);
                    line = "    ";
                }

                line += entry;
                i++;
            }

            return sb.ToString();
        }

        public static readonly ParameterType World = new ParameterType(
            name: "world",
            convertAction: AnvilWorld.Open,
            description: "The path of a folder containing a world's data files",
            outputType: typeof(AnvilWorld)
        );
    }
}

