using System;
using Substrate;
using System.Collections.Generic;
using System.Reflection;

namespace sub
{
    public class BlockTypesByName
    {
        public static readonly Dictionary<string, int> _BlockTypesByName = new Dictionary<string, int>();

        static BlockTypesByName()
        {
            var type = typeof(BlockType);
            var fields = type.GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                var value = (int)field.GetValue(null);
                var name = field.Name.ToLower();

                _BlockTypesByName.Add(name, value);
            }
        }
    }
}
