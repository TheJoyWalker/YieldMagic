public abstract class CustomAccessorProperty<T> : GenericProperty<T> where T : struct
{
    public string AccessName;
    public override string Path
    {
        get { return base.Path; }
        set
        {
            base.Path = value;
            AccessName = _pathParts[_pathParts.Length - 1].Value;
        }
    }

    protected object ResolveToObject(object o)
    {
        return UnityPathResolver.ResolveToObject(o, _pathParts);
    }
}