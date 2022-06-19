using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL_Utility.Logger
{
    public interface IJLLogger
    {
        public void Log(string message, JLLogType type = JLLogType.INFO);
    }
}
