using System;
using System.Linq;
using System.Reflection;

[Serializable]
public class ReflectedProperty<T> : CustomAccessorProperty<T> where T : struct
{

    //I really hope serialization can handle this, otherwise all's srewed xD
    public override void Initialize(object target)
    {
        if (string.IsNullOrEmpty(AccessName))
            throw new Exception("No Method name specified.");
    }

    public override void UpdateAccessors(object target)
    {
        var type = target.GetType();

        var bindingFlags = BindingFlags.Instance | BindingFlags.Public;
        MethodInfo[] methods = target.GetType().GetMethods(bindingFlags);
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
            Setter = value => field.SetValue(target, value);
            Getter = () => (T)field.GetValue(target);
        }

        Setter = Utility.CreateAction<T>(method != null ? method : prop.GetSetMethod(), target);

        switch (OffsetMode)
        {
            case PropertyOffsetMode.CurrentTo:
            case PropertyOffsetMode.AddOffset:
            case PropertyOffsetMode.MulOffset:
                if (prop.GetGetMethod() == null)
                    throw new Exception(string.Format("Offset mode needs Getter or Field {0}", AccessName));
                Getter = Utility.CreateGetter<T>(prop, target);
                FromVaue = Getter();
                break;
            case PropertyOffsetMode.FromTo:
            default:
                break;
        }
    }
}