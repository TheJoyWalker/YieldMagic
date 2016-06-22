using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;
using ThreadPriority = System.Threading.ThreadPriority;

public class WhatThingsCost : MonoBehaviour
{
    public UnityEngine.UI.Text text;
    public UnityEngine.UI.Text IterationCountText;
    public int IterationCount = 10000000;
    public float i;
    private float otherValue;
    private Vector3 v3;
    public float Setter
    {
        get { return i; }
        set { i = value; }
    }
    bool testBool = true;
    Func<float, float> testFunc;

    void Start()
    {

    }

    public void RunTest()
    {
        IterationCount = int.Parse(IterationCountText.text);
        text.text = "";
#if DEBUG || UNITY_EDITOR
        log("------");
        log("WARNING: This test should be built in release mode!");
        log("it will still run in editor but will result in invalid time");
        log("------");
#endif
        log("Test operation costs");
        log("The result is in Stopwatch.ElapsedTicks and is compared to min result");
        log("ex: add - x (+(x-min) ticks (x-m)/m*100)");
        otherValue = UnityEngine.Random.value;
        TestSetterMap();
        //        TestValueVsRefVal();
        //        TestOperations();
        //        TestSetterApproaches();
        //        TestDynamicExpressions();
        //        TestIfs();
    }
    #region ValVsRef
    public void TestValueVsRefVal()
    {
        TimeTest(new Action[]
        {
            () => SetRefI(ref v3),
            () => GetNewI(v3),
        }, IterationCount, "value vs ref value args", new List<string>() { "ref val", "val" });
    }
    public Vector3 GetNewI(Vector3 value)
    {
        return value;
    }
    public Vector3 SetRefI(ref Vector3 value)
    {
        return value;
    }
    public WhatThingsCost GetRef(WhatThingsCost value)
    {
        return value;
    }

    #endregion
    #region TestMath
    public void TestOperations()
    {
        var setter = Utility.GetSetter<float>(GetType().GetProperties().FirstOrDefault(x => x.Name == "Setter").GetSetMethod(), this);
        TimeTest(new Action[] { add, sub, mul, div, sin, pow, pow_assignment, Mathf_pow }, IterationCount, "test math operations");
    }

    public void add()
    {
        i += otherValue;
    }
    public void sub()
    {
        i -= otherValue;
    }
    public void mul()
    {
        i *= otherValue;
    }
    public void div()
    {
        i /= otherValue;
    }

    public void sin()
    {
        i = Mathf.Sin(i);
    }

    public void pow()
    {
        i = i * i;
    }
    public void pow_assignment()
    {
        i *= i;
    }
    public void Mathf_pow()
    {
        i = Mathf.Pow(i, 2);
    }
    #endregion


    //    private Action<T> CompiledExpressionWithMethodCall<T>(object Target, string setterName)
    //    {
    //        Type t = Target.GetType();
    //        MethodInfo methodInfo =
    //            t.GetProperties(BindingFlags.Instance | BindingFlags.Public)
    //                .FirstOrDefault(x => x.Name == setterName)
    //                .GetSetMethod();
    //        methodInfo = methodInfo.MakeGenericMethod(typeof(T));
    //
    //
    //        var instanceParameter = Expression.Parameter(typeof(object), "obj");
    //        var valueParameter = Expression.Parameter(typeof(T), "value");
    //        var body = Expression.PropertyOrField(Expression.Constant(Target), setterName);
    //
    //        var expr1 = Expression.Lambda<Action<T>>(Expression.Call(body = valueParameter));
    //        var expr = Expression.Lambda<Action<T>>(Expression.Call(Expression.Constant(Target), valueParameter));
    //
    //        var setterExpr = Expression.Lambda<Action<object, object>>(
    //               Expression.Call(Expression.Constant(Target)
    //
    //                   Expression.Convert(instanceParameter, typeof(T)),
    //                   setMethod,
    //                   Expression.Convert(valueParameter, propertyInfo.PropertyType)),
    //                   instanceParameter,
    //                   valueParameter
    //               );
    //
    //
    //        var l = Expression.Parameter(typeof(IEnumerable<T>), "l");
    //        Expression.PropertyOrField()
    //        var labmda = Expression.Lambda<Action<T>>();
    //        return labmda.compile();
    //    }

    private static void SetMethod<T>(ref T left, T right) where T : struct
    {
        left = right;
    }

    private void TestSetterMap()
    {
        var setter = Utility.GetSetter<float>(GetType().GetProperties().FirstOrDefault(x => x.Name == "Setter").GetSetMethod(), this);

        var lambdaSetter = Utility.GetSetter<WhatThingsCost, float>((target, value) => target.Setter = value, this);
        var lambda = new Action<WhatThingsCost, float>((target, value) => target.Setter = value);

        var lambda2 = new Action<float>(newValue => Setter = newValue);
        //        var l3=new Action<float,float>((ref old));
        //        var expr = CompiledExpressionWithMethodCall<float>(this, "otherValue");
        //        var dExpr = MakeSetter(GetType().GetProperty("Setter"));
        TimeTest(new Action[]
        {
            () => setter(otherValue),
            () => lambda(this, otherValue),
            () => lambda2(otherValue),
            () => lambdaSetter(otherValue),
            }, IterationCount, "TestSetterMap", new List<string>() { "reflection delegate", "lambda", "reflection expr" });

    }
    public void TestSetterApproaches()
    {
        var setter = Utility.GetSetter<float>(GetType().GetProperties().FirstOrDefault(x => x.Name == "Setter").GetSetMethod(), this);
        var wrappedSetter = new Func<float>(() => Setter = otherValue);
        var wrappedField = new Func<float>(() => i = otherValue);
        object thisObj = this;
        TimeTest(new Action[]
        {
            () => setter(otherValue),
            () => Setter=otherValue,
            () => wrappedSetter(),
            () => i = otherValue,
            () => wrappedField(),
            () => SetValue(),
            () => ((WhatThingsCost)thisObj).i = otherValue,
        }, IterationCount, "value set approaches", new List<string>() { "reflection delegate", "setter", "wrappedSetter", "Field", "wrappedField", "inst function", "Field with cast" });
        //        ,
    }
    private void SetValue()
    {
        i = otherValue;
    }

    public void TestDynamicExpressions()
    {
        var setter = Utility.GetSetter<float>(GetType().GetProperties().FirstOrDefault(x => x.Name == "Setter").GetSetMethod(), this);
        TimeTest(new Action[]
        {
            delegate{i = otherValue;},
            ()=> { i = otherValue; },
            ()=>i = otherValue,
        }, IterationCount, "anonymous methods", new List<string>() { "delegate", "anonymous", "lambda" });
    }
    #region TestIOrEmptyDelegate
    public void TestIfs()
    {
        TimeTest(new Action[]
        {
            IfReferenceNull,
            IfBool,
            IfFloat,
            EmptyMethod,
            () => { }
        }, IterationCount, "Ifs vs empty delegate");
    }

    private void IfReferenceNull()
    {
        if (testFunc == null)
            i = otherValue;
    }

    private void IfBool()
    {
        if (testFunc == null)
            i = otherValue;
    }
    private void IfFloat()
    {
        if (i == 1f)
            i = otherValue;
    }

    private void EmptyMethod()
    {
    }
    #endregion

    private void log(string s)
    {
        text.text += s + "\n";
    }

    public void TimeTest(Action[] actions, int count, string title, List<string> names = null)
    {
        log("Test: " + title);
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
        Thread.CurrentThread.Priority = ThreadPriority.Highest;
        Dictionary<string, long> resultList = new Dictionary<string, long>();
        if (names == null)
            names = new List<string>();
        //warm up
        foreach (var action in actions)
            action();

        foreach (var action in actions)
        {
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
                action();
            sw.Stop();
            string actionName = action.Method.Name;
            if (names.Count > 0)
            {
                actionName = names[0];
                names.RemoveAt(0);
            }
            resultList.Add(actionName, sw.ElapsedTicks);
        }
        var maxLength = resultList.Keys.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;
        var paddedPairs =
            resultList.Select(x => new KeyValuePair<string, long>(x.Key.PadRight(maxLength), x.Value));
        var minValue = resultList.Values.Min();
        var minElement = resultList.FirstOrDefault(x => x.Value == minValue);
        log(string.Format("Best - '{0}' ({1} ticks)", minElement.Key, minElement.Value));
        log(string.Join("\n", paddedPairs.Select(x => string.Format("{0}\t\t=> {1} (+{2} ticks {3:F}%)", x.Key, x.Value, x.Value - minValue, (x.Value - minValue) / (float)minValue * 100f)).ToArray()));
        log("================");
    }
}