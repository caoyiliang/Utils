using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CUtils
{
    public static class TaskUtils
    {
        public static readonly Task<object> NullTask = Task.FromResult<object>(null);
    }
}
