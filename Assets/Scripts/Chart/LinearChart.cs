using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using YieldMagic;

public class LinearChart : MonoBehaviour
{
    [SerializeField]
    private Text _title;
    [SerializeField]
    private LineRenderer _lineRenderer;
    [SerializeField]
    private Transform _startTransform;
    [SerializeField]
    private Transform _endTransform;

    [SerializeField]
    private GameObject prefab;

    private Color[] colors = { Color.blue, Color.red, Color.green, Color.magenta };
    //    void Start()
    //    {
    //        Draw("Sin");
    //    }
    public void Draw(string Name, Func<float, float>[] functions)
    {
        //        Debug.Log("Draw:" + Name);
        _title.text = Name;
        Vector3 total = _endTransform.position - _startTransform.position;
        total = new Vector3(total.x, 1, total.y);
        for (int i = 0; i < functions.Length; i++)
        {
            var func = functions[i];
            GameObject go = GameObject.Instantiate(prefab);
            go.transform.SetParent(_startTransform.transform.parent, false);
            go.transform.position = _startTransform.position;
            var lineRenderer = go.GetComponent<LineRenderer>();
            lineRenderer.material.color = colors[i];
            var points = GetChartData(func, .001f).Select(x => div(mul(total, x), go.transform.parent.lossyScale)).ToArray();
            //            var points = GetChartData(func, .01f).ToArray();
            lineRenderer.SetVertexCount(points.Length);
            lineRenderer.SetPositions(points);
        }
    }

    public static void PutGoInPos(string name, Vector3 pos)
    {
        GameObject go = new GameObject(name);
        go.transform.position = pos;
    }
    Vector3 div(Vector3 one, Vector3 other)
    {
        return new Vector3(one.x / other.x, one.y / other.y, one.z / other.z);
    }
    Vector3 mul(Vector3 one, Vector3 other)
    {
        return new Vector3(one.x * other.x, one.y * other.y, one.z * other.z);
    }

    private static Vector3[] GetChartData(Func<float, float> func, float step)
    {
        List<Vector3> res = new List<Vector3>();
        float progress = 0;
        while (progress <= 1)
        {
            res.Add(new Vector3(progress, .1f, func(progress)));
            progress += step;
        }
        return res.Distinct().ToArray();
    }
}