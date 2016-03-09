using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonesWPF
{
    interface ISubject
    {
        void registerObserver(IObserval observal);
        void notifyObserver(IObserval observal);
        void removeObserver(IObserval observal);
    }

    interface IObserval
    {
        void update();
    }

    class Observer
    {

    }
}
