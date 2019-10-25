using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voicemeeter
{
    /// <summary>
    /// Observable levels monitor.  Use Rx to subscribe to the levels of the selected Channels.
    /// Usage:  
    ///  using System.Reactive.Linq;
    ///  var levels = new Voicemeeter.Levels(channels, 20);
    ///  var subscription = levels.Subscribe(x => DoSomethingWithFloatArray(x));
    ///  ...
    ///  subscription.Dispose(); levels.Dispose();
    /// </summary>
    public class Levels : IDisposable, IObservable<float[]>
    {
        public class Channel
        {
            public LevelType LevelType { get; set; }
            public int ChannelNumber { get; set; }
        };

        private readonly List<Channel> channels = new List<Channel>();
        private readonly List<IObserver<float[]>> observers = new List<IObserver<float[]>>();
        private readonly IObservable<int> timer;
        private IDisposable timerSubscription;

        public Levels(Channel[] channels, int milliseconds = 20)
        {
            this.channels = new List<Channel>(channels);
            this.timer = Observable.Interval(TimeSpan.FromMilliseconds(milliseconds)).Select(_ => 1);
            Watch();
        }

        private void Watch()
        {
            timerSubscription = timer.Subscribe(_ =>
            {
                var values = new List<float>(channels.Count);
                foreach(var channel in channels)
                {
                    values.Add(VoiceMeeter.Remote.GetLevel(channel.LevelType, channel.ChannelNumber));
                }
                Notify(values.ToArray());
            });
        }

        public IDisposable Subscribe(IObserver<float[]> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);

            return new Unsubscriber(observers, observer);
        }

        private void Notify(float[] values)
        {
            foreach (var observer in observers)
                observer.OnNext(values);
        }

        public void Dispose()
        {
            timerSubscription?.Dispose();
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<float[]>> _observers;
            private readonly IObserver<float[]> _observer;

            public Unsubscriber(List<IObserver<float[]>> observers, IObserver<float[]> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}

