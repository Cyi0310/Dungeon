using System;

public class Observable<T>
{
    private T value;
    public T Value
    {
        get => value;
        set
        {
            if (!Equals(this.value, value))
            {
                this.value = value;
                ValueChanged?.Invoke(this.value);
            }
        }
    }
    public event Action<T> ValueChanged;

    public void Dispose()
    {
        ValueChanged = null;
    }
}
