using UnityEngine;
using YieldMagic;
using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Linq;

public class ChartFiller : MonoBehaviour
{
    [SerializeField]
    private Transform Container;
    [SerializeField]
    private LinearChart chart;
    private string[] masks = new string[] { "InOut", "OutIn", "In", "Out" };
    private string[] easingNames;
    MethodInfo[] functions;
    void Awake()
    {
        Type type = typeof(Easing);
        functions = type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                        .Where(x => x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(float) &&
                            x.ReturnType == typeof(float)).ToArray();
        easingNames = functions.Select(x => x.Name).ToArray();
        Debug.Log(string.Join("\n ", easingNames.Select(x=>string.Format("case EasingType.{0}:" +
                                                                       "EasingFunction = Easing.{0};" +
                                                                       " break;", x)).ToArray()));
        for (int i = 0; i < easingNames.Length; i++) 
        {
            var name = easingNames[i];
            foreach (var m in masks)
            {
                if (name.EndsWith(m, false, CultureInfo.InvariantCulture))
                {
                    easingNames[i] = name.Substring(0, name.Length - m.Length);
                    break;
                }
            }
        }

        easingNames = easingNames.Distinct().ToArray();
        StartCoroutine(someDelay());
    }

    public IEnumerator someDelay()
    {
        //w8 for all initializations
        yield return null;
        chart.gameObject.SetActive(true);
        foreach (var easingName in easingNames)
        {
            var go = Instantiate(chart);
            go.transform.SetParent(Container, false);
            go.GetComponent<LinearChart>().Draw(easingName, functions.Where(x => x.Name.Contains(easingName)).Select(x => MakeDelegate<float, float>(x, null)).ToArray());
        }
        chart.gameObject.SetActive(false);
    }

    public static Func<T, TResult> MakeDelegate<T, TResult>(MethodInfo method, object target)
    {
        var f = (Func<T, TResult>)Delegate.CreateDelegate(typeof(Func<T, TResult>), target, method);
        return t => f(t);
    }
}
