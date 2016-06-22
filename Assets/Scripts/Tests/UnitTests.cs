using System;
using UnityEngine;
using System.Collections;
using YieldMagic;

public class UnitTests : MonoBehaviour
{
    private static Func<float, float> easInOut = Easing.CircularIn;

    [SerializeField]
    private Transform one;
    [SerializeField]
    private Transform other;

    [SerializeField]
    private float defaultDuration = 2f;
    //    [SerializeField]
    //    private float breakDuration = 1f;
    //    [SerializeField]
    //    private int stressCount = 1000;
    [SerializeField]
    private MonoBehaviour _dummyBehaviour;
    [SerializeField]
    private Transform _dummyTransform;

    public Transform ScledTransform;
    // Use this for initialization
    void Start()
    {
        StartCoroutine("Test");
        //        var a = delegate { if (isActiveAndEnabled) i = 1; };
        //        ScledTransform.localScale = GetGlobalScale(ScledTransform, Vector3.one);
    }

    public Vector3 GetGlobalScale(Transform t, Vector3 targetScale)
    {
        var lossy = t.lossyScale;
        var local = t.localScale;
        return targetScale = new Vector3(lossy.x / local.x * targetScale.x, lossy.y / local.y * targetScale.y, lossy.z / local.z * targetScale.z);
    }

    public IEnumerator Test()
    {
        /*        Vector3 smth=Vector3.back;
                var config = new GameObjectTweenConfig(this);
                config.Scale(1f, 2f);
                config.Rotate(1f, new Vector3(0, 90, 0));
                config.Move(1f, Vector3.one * 3f);
                config.Material.Color(1f, Color.black);


                yield return new YieldTransformTween(config, this, that);



                new GameObjectTweenConfig().Scale(1f, 2f).Rotate(1f, new Vector3(0, 90, 0));

                config.Translate<Vector3>(gameObject.transform.position, Vector3.one * 3f);
                //config.Translate<Vector3>("/material.spritePosition", Vector3.one*3f);

                config.Transform.Move(fromPos, toPos, time, delay);
                config.Component.Value(params);
                config.Component.SetValue("", 5f);


                yield return new WaitTimeOrCondition(1f, () => breakFlag).Override(onComplete: (source) => { Debug.Log("a"); });

                yield return new YieldTransformTween(one.transform, 1f).Move(Vector3.one).Override(onComplete: (source) => { Debug.Log("a"); });

                var mazafaka=new object();
                yield return
                    new YieldTween(mazafaka).AddProperty(new ReflectedProperty<int>()
                    {
                        MethodName = "foo",
                        FromVaue = 5,
                        ToValue = 6
                    });*/
        yield break;
    }



    public IEnumerator timeIssues()
    {
        float start = Time.timeSinceLevelLoad;
        float delta = 0;
        float diff = 0;
        while (true)
        {
            yield return 0;
            delta += Time.deltaTime;
            diff = Time.timeSinceLevelLoad - start;
            var isOk = diff == delta;
            string logString = string.Format("<color={3}><b>diff:{0} delta:{1} faulted={2}</b></color>", diff, delta,
                isOk, isOk ? "green" : "red");
            Debug.LogFormat(logString);
        }
    }


    #region util
    IEnumerator testYieldTransition(string caption, Func<IEnumerator> createDelegate, Func<float, bool> checkDelegate, float breakTime = 0, Func<float, string> expectedValue = null, Func<string> getValue = null, bool silent = false)
    {
        if (!silent)
            log("test " + string.Format("<b><color=green>{0}</color></b>", caption));

        breakFlag = false;//this prevents brake before breakIn()
        float time = Time.timeSinceLevelLoad;
        bool useBreakOut = breakTime > 0;

        if (useBreakOut)
            StartCoroutine(breakIn(breakTime));
        yield return createDelegate();

        float timePassed = Time.timeSinceLevelLoad - time;
        //substract delta cuz it was called last frame
        var delCheck = checkDelegate(timePassed);
        var timeCheck = breakTime - timePassed < Time.deltaTime;
        var ok = !useBreakOut ? delCheck : delCheck && timeCheck;
        string resultString = string.Format("<b><color={1}>{0}</color></b>", ok ? "passed" : "failed", ok ? "green" : "red");

        if (!silent)
            log(ok, "{0} => <Color=green>{1}</color> timePassed={2}, timeBroke={3}", caption, resultString, timePassed, breakTime > 0);
        if (!delCheck && !silent)
        {
            log("del failed");
            if (expectedValue != null)
                Debug.Log("expectedValue:   " + expectedValue(timePassed));
            if (getValue != null)
                Debug.Log("getValue:   " + getValue());
        }
        if (useBreakOut && !timeCheck && !silent)
            log("time failed - start:{0} end:{1} passed:{2} delta:{3} f:{4}", time, Time.timeSinceLevelLoad, timePassed, Time.deltaTime, Time.frameCount);
    }

    IEnumerator breakIn(float sec)
    {
        yield return new WaitForSeconds(sec);
        breakFlag = true;
    }
    private bool breakFlag;
    private bool BreakInstruction()
    {
        return breakFlag;
    }

    /// <summary>
    /// polar offset
    /// </summary>
    /// <param name="radius">circle radius</param>
    /// <param name="angle">Rad angle</param>
    /// <returns></returns>
    public Vector3 Polar(float radius, float angle)
    {
        return new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
    }

    private void log(string s, params object[] data)
    {
        Debug.LogFormat(s, data);
    }
    private void log(bool ok, string s, params object[] data)
    {
        if (ok)
            Debug.LogFormat(s, data);
        else
            Debug.LogErrorFormat(s, data);
    }
    private string GetPreciseLog(Vector3 v3)
    {
        return string.Format("{0}:{1}:{2}", v3.x, v3.y, v3.z);
    }
    #endregion
}
