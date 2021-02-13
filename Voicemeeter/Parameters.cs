using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voicemeeter
{
    /// <summary>
    /// Observable parameters monitor.  Use Rx to subscribe to parameter changes.
    /// Usage:
    ///  using System.Reactive.Linq;
    ///  var parameters = new Voicemeeter.Parameters();
    ///  var subscription = parameters.Parameters(x => DoSomethingTheParametersChanged(x));
    ///  ...
    ///  subscription.Dispose(); levels.Dispose();
    /// </summary>
    public class Parameters : IDisposable, IObservable<int>
    {
        private readonly List<IObserver<int>> observers = new List<IObserver<int>>();
        private readonly IObservable<int> timer;
        private IDisposable timerSubscription;

        public Parameters(int milliseconds = 20)
        {
            this.timer = Observable.Interval(TimeSpan.FromMilliseconds(milliseconds)).Select(_ => 1);
            Watch();
        }

        private void Watch()
        {
            timerSubscription = timer.Subscribe(_ =>
            {
                int response = VoiceMeeter.Remote.IsParametersDirty();
                if (response > 0)
                {
                    Notify(response);
                }
            });
        }

        public IDisposable Subscribe(IObserver<int> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);

            return new Unsubscriber(observers, observer);
        }

        private void Notify(int value)
        {
            foreach (var observer in observers)
                observer.OnNext(value);
        }

        public void Dispose()
        {
            timerSubscription?.Dispose();
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<int>> _observers;
            private readonly IObserver<int> _observer;

            public Unsubscriber(List<IObserver<int>> observers, IObserver<int> observer)
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