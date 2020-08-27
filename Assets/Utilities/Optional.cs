using System;

namespace Assets.Utilities
{

    public interface IOptional<T>
    {
        bool HasValue();
        T GetValue();
    }

    internal sealed class Optional<T> : IOptional<T>
    {
        private readonly T value;
        private readonly bool hasValue;
        private readonly string errorString;

        public Optional(T value, bool hasValue, string errorString)
        {
            this.value = value;
            this.hasValue = hasValue;
            this.errorString = errorString;
        }

        public T GetValue()
        {
            if (this.hasValue)
            {
                return this.value;
            }
            throw new ApplicationException(errorString);
        }

        public bool HasValue()
        {
            return this.hasValue;
        }
    }

    public static class Optional
    {
        public static IOptional<T> Some<T>(T value) => new Optional<T>(value, true, null);
        public static IOptional<T> None<T>(string errorString) => new Optional<T>(default(T), false, errorString);
    }
}