using Meadow;
using Meadow.Foundation;
using System;
using System.Threading.Tasks;

namespace AvaloniaSample.Simulation
{
    public abstract class SimulatedSensor<T> : SamplingSensorBase<T>
        where T : struct
    {
        public event EventHandler<IChangeResult<T>> SensorUpdated = delegate { };

        public T? LastReading { get; private set; } = null;

        protected SimulatedSensor()
        {
        }

        protected abstract void RaiseUpdatedEvent(T newValue);

        public override void StartUpdating(TimeSpan? updateInterval = null)
        {
            UpdateInterval = updateInterval ?? TimeSpan.FromSeconds(5);
            var task = new Task(SampleProc, TaskCreationOptions.LongRunning);
            task.Start();
        }

        public override void StopUpdating()
        {
            SamplingTokenSource?.Cancel();
        }

        private async void SampleProc()
        {
            SamplingTokenSource = new System.Threading.CancellationTokenSource();

            IsSampling = true;

            while (!SamplingTokenSource.IsCancellationRequested)
            {
                var value = await ReadSensor();

                //                if (LastReading == null || !value.Equals(LastReading.Value))
                {
                    SensorUpdated?.Invoke(this, new ChangeResult<T>(value, LastReading));
                }

                LastReading = value;
                await Task.Delay(UpdateInterval);
            }

            IsSampling = false;
            SamplingTokenSource.Dispose();
            SamplingTokenSource = null;
        }
    }
}