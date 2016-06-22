using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
using YieldMagic;

public static class ExpressionEx
{
    public static BinaryExpression Assign(Expression left, Expression right)
    {
        var assign = typeof(Assigner<>).MakeGenericType(left.Type).GetMethod("Assign");

        var assignExpr = Expression.Add(left, right, assign);

        return assignExpr;
    }

    private static class Assigner<T>
    {
        public static T Assign(ref T left, T right)
        {
            return (left = right);
        }
    }
}

public static class Utility
{
    public static Action<float> GetDelegate<T>(T start, T end, object target, Func<T, T, float, T> lerpFunc, PropertyInfo prop, FieldInfo field, MethodInfo method)
    {
        T startValue = (T)start;
        T endValue = (T)end;
        //            var setter = GetSetter<T>(prop.GetSetMethod(false), Target);

        if (field != null)
            return value => field.SetValue(target, lerpFunc(startValue, endValue, value));

        var mSetter = CreateAction<T>(method ?? prop.GetSetMethod(false), target);
        return value => mSetter(lerpFunc(startValue, endValue, value));
        //            if (Method != null)
        //                return value => mSetter(lerpFunc(startValue, endValue, value));
        //            if (prop != null)
        //                return value => setter(lerpFunc(startValue, endValue, value));
    }

    public static object GetMethodObject(string methodName, object target)
    {
        Type type = target.GetType();
        MethodInfo method = type.GetMethods(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault(x => x.Name == methodName);

        List<Type> args = new List<Type>(method.GetParameters().Select(x => x.ParameterType));

        Type delegateType = (method.ReturnType == typeof(void)
            ? Expression.GetActionType(args.ToArray())
            : Expression.GetFuncType(args.ToArray()));
        return Delegate.CreateDelegate(delegateType, target, method);
    }

    public static Action<T> GetSetter<T>(MethodInfo setter, object target)
    {
        return (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), target, setter);
    }

    public static Func<T, object> MakeDelegate<T, TResult>(MethodInfo method, object target)
    {
        var f = (Func<T, TResult>)Delegate.CreateDelegate(typeof(Func<T, TResult>), target, method);
        return t => f(t);
    }

    public static Action<T> CreateAction<T>(MethodInfo method, object target)
    {
        if (method.GetParameters().Length > 1)
        {
            Debug.LogError("You can use only single parameter function");
            return null;
        }

        Type delegateType;
        List<Type> args = new List<Type>(method.GetParameters().Select(x => x.ParameterType));

        Action<T> action = null;
        if (method.ReturnType == typeof(void))
        {
            delegateType = Expression.GetActionType(args.ToArray());
            Delegate d = Delegate.CreateDelegate(delegateType, target, method);
            return (Action<T>)d;
        }
        else
        {
            Debug.LogError("only void return type supported yet");
            args.Add(method.ReturnType);
            delegateType = Expression.GetFuncType(args.ToArray());
            var f = (Func<T, object>)Delegate.CreateDelegate(delegateType, target, method);
            return (value) => f(value);
            //                action = value => (Func<T,TResult>)d)(value);
        }
        return action;
    }

    public static Func<T> CreateGetter<T>(PropertyInfo prop, object target)
    {
        return (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), target, prop.GetGetMethod());
    }
    public static Action<TValue> GetSetter<TObject, TValue>(Action<TObject, TValue> action, TObject target)
    {
        return (value) => action(target, value);
    }
}