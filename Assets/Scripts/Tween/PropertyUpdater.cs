using System;

public class PropertyUpdater<T> : IPropertyUpdater
{
    public T From;
    public T To;
    public Func<T, T, float, T> Lerp;
    public Func<T> Getter;
    public Action<T> Setter;

    public void Update(float p)
    {
        Setter(Lerp(From, To, p));
    }
}