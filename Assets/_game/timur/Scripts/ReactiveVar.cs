using System;
using System.Collections.Generic;

namespace _game
{
    public class ReactiveVar<T>
    {
        private T _value;
        private readonly List<Action<T>> _subscribers = new List<Action<T>>();

        public ReactiveVar(T initialValue)
        {
            _value = initialValue;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    _value = value;
                    NotifySubscribers();
                }
            }
        }

        public void Subscribe(Action<T> subscriber)
        {
            if (subscriber == null) throw new ArgumentNullException(nameof(subscriber));
            _subscribers.Add(subscriber);
        }

        public void Unsubscribe(Action<T> subscriber)
        {
            _subscribers.Remove(subscriber);
        }

        private void NotifySubscribers()
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber(_value);
            }
        }
    }
}