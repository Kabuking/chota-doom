using System;

namespace Modules.Common.Datatype
{
    public struct Optional<T> where T : class
    {
        private readonly T _value;

        public bool HasValue { get; }

        public T Value => HasValue ? _value : throw new InvalidOperationException("No value present");

        public Optional(T value)
        {
            _value = value;
            HasValue = value != null;
        }

        public static implicit operator Optional<T>(T value)
        {
            return new Optional<T>(value);
        }

        public T OrElse(T other)
        {
            return HasValue ? _value : other;
        }

        public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            return HasValue ? some(_value) : none();
        }

        public void IfPresent(Action<T> action)
        {
            if (HasValue)
            {
                action(_value);
            }
        }

        public override string ToString()
        {
            return HasValue ? _value.ToString() : "No value";
        }
    }
}