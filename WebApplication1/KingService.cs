using King.Azure.Data;
using King.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class Factory : ITaskFactory<object>
    {
        public IEnumerable<IRunnable> Tasks(object passthrough)
        {
            yield return new RecurringRunner(new Runs(new BackgroundBackground()));
        }
    }

    public class Runs : IRuns
    {
        IProcessor<object> task;
        public Runs(IProcessor<object> task)
        {
            this.task = task;
        }
        public int MinimumPeriodInSeconds
        {
            get
            {
                return 2;
            }
        }

        public async Task<bool> Run()
        {
            return await task.Process(null);
        }
    }


    public class BackgroundBackground : IProcessor<object>
    {
        public async Task<bool> Process(object data)
        {
            var id = Guid.NewGuid();

            await Task.Run(() => { Thread.Sleep(5000); Trace.TraceInformation("Background A {0}", id); });

            await Task.Run(() => { Thread.Sleep(10000); Trace.TraceInformation("Background B {0}", id); });

            await Task.Run(() => { Thread.Sleep(15000); Trace.TraceInformation("Background C {0}", id); });

            Trace.TraceInformation("Foreground");

            return true;
        }
    }
}