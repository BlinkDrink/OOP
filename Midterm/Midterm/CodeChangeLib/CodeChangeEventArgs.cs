using System;
using System.Collections.Generic;
using System.Security.RightsManagement;

namespace CodeChangeLib
{
    public class CodeChangeEventArgs : EventArgs
    {

        #region Properties
        public List<string> Codes { get; init; }
        #endregion

        #region Constructors
        public CodeChangeEventArgs(List<string> codes)
        {
            Codes = codes;
        } 
        #endregion
    }
}

