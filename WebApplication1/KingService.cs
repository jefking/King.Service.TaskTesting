using King.Azure.Data;
using King.Service;
using System.Collections.Generic;
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
                return 5;
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
            await Task.Run(() => { Thread.Sleep(5000); });

            return true;
        }
    }
}