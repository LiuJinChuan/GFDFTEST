using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFDF.Infrastruct.Core
{
    public interface ILogger
    {
        void Debug(string message);

        void Error(Exception ex);

        void Fatal(string message);

        void Warn(string message);

        void Info(string message);
    }
}
