namespace foxer.Core.Utils
{
    public class RefValue<T>
       where T : struct
    {
        public T Value { get; set; }

        public RefValue(T value)
        {
            Value = value;
        }
    }
}
