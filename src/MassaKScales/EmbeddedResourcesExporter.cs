using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassaKScales
{
    using System.IO;
    using System.Reflection;

    static class EmbeddedResourcesExporter
    {
        public static void ExtractResourceToFile(this Assembly assembly, string resourceName, string fileName)
        {
            using (var stream = assembly.GetManifestResourceStream(ByPartName(assembly, resourceName)))
            {
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    stream.CopyTo(fileStream);
                }
            }
        }
        private static string ByPartName(Assembly assembly, string fileName)
        {
            return assembly.GetManifestResourceNames().Single(s => s.EndsWith(fileName));
        }
    }
}
