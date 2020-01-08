using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpcContract
{
    /// <summary>
    /// Represents an alias to a file in the plugin. Copied directly from existing ScriptedSandbox code.
    /// </summary>
    public class FileAlias
    {
        /// <summary>
        /// Kept for compatibility reasons only. Specify the architecture-specific paths instead.
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// The path to the file relative to the plugin's base path, for 32-bit DLLs
        /// </summary>
        public string path_x86 { get; set; }

        /// <summary>
        /// The path to the file relative to the plugin's base path, for 64-bit DLLs
        /// </summary>
        public string path_amd64 { get; set; }

        /// <summary>
        /// Determines if the file is optional.
        /// </summary>
        /// <remarks>If this property is true it means we will not verify its existence on startup of the plugin, and
        /// calling createExternalObject will simply return null of the extension doesn't exist. If this is false (the 
        /// default value) we will check for the files existence on plugin load and throw an exception if it is not present.</remarks>
        public bool optional { get; set; }
    }
}
