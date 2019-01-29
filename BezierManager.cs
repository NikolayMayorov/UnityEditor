using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class BezierData
{
    public Vector3 P0;
    public Vector3 P1;
    public Vector3 P2;
    public Vector3 P3;
    public int countPoint;
    public List<Vector3> points;

    public bool isHead;
    public bool isTail;

    public BezierData(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, int countPoint, bool isHead, bool isTail)
    {
        this.P0 = P0;
        this.P1 = P1;
        this.P2 = P2;
        this.P3 = P3;
        this.countPoint = countPoint;
        this.points = new List<Vector3>();
        this.isHead = isHead;
        this.isTail = isTail;
        FillPoints();
    }

    public void FillPoints()
    {
        points.Clear();
        float temp = (float)countPoint - 1.0f;
        for (int i = 0; i < countPoint; i++)
        {
            points.Add(Calculate(i / temp));
        }
    }

    public void DrawLine()
    {
        for (int i = 1; i < countPoint; i++)
        {
#if UNITY_EDITOR
            Handles.DrawLine(points[i - 1], points[i]);
#endif
        }
    }

    private Vector3 Calculate(float t)
    {
        float temp = 1 - t;
        Vector3 result = Mathf.Pow(temp, 3) * P0 + 3 * t * Mathf.Pow(temp, 2) * P1 + 3 * Mathf.Pow(t, 2) * temp * P2 + Mathf.Pow(t, 3) * P3;
        return result;
    }
}

[System.Serializable]
public class BezierWay
{
    public string name;
    public List<BezierData> bezierDatas = new List<BezierData>();
    public Color color;
    private const int offset = 20;

    /// <summary>
    /// Добавляет кривую в путь
    /// </summary>
    public void AddCurve(int countPoint)
    {
        Vector3 offsetP1 = bezierDatas[bezierDatas.Count - 1].P3 + new Vector3(offset, 0, 0);
        Vector3 offsetP2 = bezierDatas[bezierDatas.Count - 1].P3 + new Vector3(0, 0, offset);
        Vector3 offsetP3 = bezierDatas[bezierDatas.Count - 1].P3 + new Vector3(offset, 0, offset);
        bezierDatas.Add(new BezierData(bezierDatas[bezierDatas.Count - 1].P3, offsetP1, offsetP2, offsetP3, countPoint, false, true));
        foreach (var b in bezierDatas)
            b.isTail = false;
        bezierDatas[0].isHead = true;
        bezierDatas[bezierDatas.Count - 1].isTail = true;
    }

    /// <summary>
    /// Удаляет последнюю кривую из пути
    /// </summary>
    public void RemoveCurve()
    {
    }
}

public class BezierManager : MonoBehaviour
{
    private static BezierManager _instance;
    public static BezierManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BezierManager>();
            }
            return _instance;
        }
    }
    public WayProfiler wayProfiler;

    [HideInInspector]
    public BezierWay currentWay = null;

    [HideInInspector]
    public int indexInPopup = 0;

    /// <summary>
    /// По имени возвращает список точек
    /// </summary>
    /// <param name="wayName">Имя пути, точки которого возвращает метод</param>
    /// <returns>Список точек если путь существует, null если путь не существует</returns>
    public List<Vector3> GetPointsWay(string wayName)
    {
        return wayProfiler.GetPointsWay(wayName);
    }
}
