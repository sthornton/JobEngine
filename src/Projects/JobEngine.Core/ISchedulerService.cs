using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Core
{
    public interface ISchedulerService
    {
        void Start(string connString);

        void Stop();
    }
}
