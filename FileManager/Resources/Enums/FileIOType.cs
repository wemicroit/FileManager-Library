using System;
using System.Collections.Generic;
using System.Text;

namespace WeMicroIt.Utils.FileConverter.Resources.Enums
{
    /// <include file='./docs/Enums.xml' path='doc/enums[@name="FileIOType"]/enum/*'/> 
    public enum FileIOType
    {
        /// <include file='./docs/Enums.xml' path='doc/enums[@name="FileIOType"]/field[@name="Block"]/*'/> 
        Block = 0,
        /// <include file='./docs/Enums.xml' path='doc/enums[@name="FileIOType"]/field[@name="Lines"]/*'/> 
        Lines = 1,
        /// <include file='./docs/Enums.xml' path='doc/enums[@name="FileIOType"]/field[@name="Line"]/*'/> 
        Line = 2,
        /// <include file='./docs/Enums.xml' path='doc/enums[@name="FileIOType"]/field[@name="Data"]/*'/> 
        Data = 3,
        /// <include file='./docs/Enums.xml' path='doc/enums[@name="FileIOType"]/field[@name="Stream"]/*'/> 
        Stream = 4,
    }
}
