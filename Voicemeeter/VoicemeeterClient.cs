using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voicemeeter
{
    public class VoicemeeterClient : IDisposable, IObservable<float>
    {
        public void Dispose()
        {
            try
            {
                RemoteWrapper.Logout();
            }
            catch (Exception)
            {
            }
        }

        private List<IObserver<float>> observers = new List<IObserver<float>>();

        public IDisposable Subscribe(IObserver<float> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);

            return new Unsubscriber(observers, observer);
        }

        private void Notify(float value)
        {
            foreach (var observer in observers)
                observer.OnNext(value);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<float>> _observers;
            private IObserver<float> _observer;

            public Unsubscriber(List<IObserver<float>> observers, IObserver<float> observer)
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
