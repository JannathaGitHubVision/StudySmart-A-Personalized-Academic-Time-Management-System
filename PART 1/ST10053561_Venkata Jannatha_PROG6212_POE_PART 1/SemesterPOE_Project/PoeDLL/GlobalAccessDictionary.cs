using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeDLL
{
    public class GlobalAccessDictionary
    {
        public static Dictionary<string, ModulesInfo> ModuleDictionary { get; } = new Dictionary<string, ModulesInfo>();

    }
}
