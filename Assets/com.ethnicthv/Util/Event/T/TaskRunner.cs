using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace com.ethnicthv.Util.Event.T
{
    public class TaskRunner
    {
    }

    public class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
    {
        private readonly LinkedList<Task> _tasks = new();
        private readonly int _maxDegreeOfParallelism;
        private int _delegatesQueuedOrRunning = 0;

        public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
        {
            if (maxDegreeOfParallelism < 1) throw new ArgumentOutOfRangeException(nameof(maxDegreeOfParallelism));
            _maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        protected sealed override void QueueTask(Task task)
        {
            lock (_tasks)
            {
                _tasks.AddLast(task);
                if (_delegatesQueuedOrRunning >= _maxDegreeOfParallelism) return;
                ++_delegatesQueuedOrRunning;
                NotifyThreadPoolOfPendingWork();
            }
        }

        private void NotifyThreadPoolOfPendingWork()
        {
            ThreadPool.UnsafeQueueUserWorkItem(_ =>
            {
                Task task;
                lock (_tasks)
                {
                    if (_tasks.Count == 0)
                    {
                        --_delegatesQueuedOrRunning;
                        return;
                    }

                    task = _tasks.First.Value;
                    _tasks.RemoveFirst();
                }

                base.TryExecuteTask(task);

                lock (_tasks)
                {
                    if (_tasks.Count > 0)
                        NotifyThreadPoolOfPendingWork();
                    else
                        --_delegatesQueuedOrRunning;
                }
            }, null);
        }

        protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (taskWasPreviouslyQueued) return false;

            if (!Thread.CurrentThread.IsThreadPoolThread) return false;
            if (_delegatesQueuedOrRunning >= _maxDegreeOfParallelism) return false;
            ++_delegatesQueuedOrRunning;
            bool result = base.TryExecuteTask(task);
            --_delegatesQueuedOrRunning;
            return result;

        }

        protected sealed override bool TryDequeue(Task task)
        {
            lock (_tasks) return _tasks.Remove(task);
        }

        public sealed override int MaximumConcurrencyLevel => _maxDegreeOfParallelism;

        protected sealed override IEnumerable<Task> GetScheduledTasks()
        {
            var lockTaken = false;
            try
            {
                Monitor.TryEnter(_tasks, ref lockTaken);
                if (lockTaken) return _tasks;
                else throw new NotSupportedException();
            }
            finally
            {
                if (lockTaken) Monitor.Exit(_tasks);
            }
        }
    }
}