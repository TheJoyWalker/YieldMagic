using System;
using System.Linq;
using System.Reflection;

[Serializable]
public class ReflectedProperty<T> : CustomAccessorProperty<T> where T : struct
{

    public override IPropertyUpdater GetUpdater(object targetObject)
    {
        if (string.IsNullOrEmpty(AccessName))
            throw new Exception("No Method name specified.");

        var type = targetObject.GetType();
        Func<T> getter = null;
        Action<T> setter;

        var bindingFlags = BindingFlags.Instance | BindingFlags.Public;
        MethodInfo[] methods = type.GetMethods(bindingFlags);
        PropertyInfo[] props = type.GetProperties(bindingFlags);
        FieldInfo[] fields = type.GetFields(bindingFlags);


        var method = methods.FirstOrDefault(x => x.Name == AccessName);
        var prop = props.FirstOrDefault(x => x.Name == AccessName);
        var field = fields.FirstOrDefault(x => x.Name == AccessName);

        if (method == null && prop == null && field == null)
            throw new Exception(string.Format("No Method, property or Field {0}", AccessName));

        //Still don't like it since we will have boxing here
        if (field != null)
        {
            setter = value => field.SetValue(targetObject, value);
            getter = () => (T)field.GetValue(targetObject);
        }

        setter = Utility.CreateAction<T>(method != null ? method : prop.GetSetMethod(), targetObject);

        switch (OffsetMode)
        {
            case PropertyOffsetMode.CurrentTo:
            case PropertyOffsetMode.AddOffset:
            case PropertyOffsetMode.MulOffset:
                if (prop.GetGetMethod() == null)
                    throw new Exception(string.Format("Offset mode needs Getter or Field {0}", AccessName));
                getter = Utility.CreateGetter<T>(prop, targetObject);
                FromVaue = getter();
                break;
            case PropertyOffsetMode.FromTo:
            default:
                break;
        }
        return new PropertyUpdater<T>()
        {
            Getter = getter,
            Setter = setter,
            From = FromVaue,
            To = ToValue,
            Lerp = Lerp
        };
    }
}