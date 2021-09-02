using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFDF.Infrastruct.Core
{
    public interface ICacheService
    {
        void Set(string key, object obj, int seconds = 7200);
        T Get<T>(string key) where T : class;
        void Remove(string key);
    }
}
