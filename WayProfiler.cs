using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO сделать удаление пути из профайла

[CreateAssetMenu(menuName = "TowerDefence/WayProfiler", fileName = "WayProfiler")]
public class WayProfiler : ScriptableObject
{
    public List<BezierWay> bezierWays = new List<BezierWay>();


    /// <summary>
    /// По имени возвращает список точек
    /// </summary>
    /// <param name="wayName">Имя пути, точки которого возвращает метод</param>
    /// <returns>Список точек если путь существует, null если путь не существует</returns>
    public List<Vector3> GetPointsWay(string wayName)
    {
        List<Vector3> result = new List<Vector3>();
        if (bezierWays != null)
        {
            for (int i = 0; i < bezierWays.Count; i++)
            {
                if (bezierWays[i].name == wayName)
                {
                    foreach(var p in bezierWays[i].bezierDatas)
                        result.AddRange(p.points);
                    return result;
                }
            }
        }
        result = null;
        return result;
    }

    /// <summary>
    /// Очищает профайлер
    /// </summary>
    public void ClearWayProfiler()
    {
        bezierWays.Clear();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Список имен всех путей</returns>
    //public List<string> GetNamesWays()
    //{
    //    List<string> result = new List<string>();
    //    foreach (var temp in bezierWays)
    //        result.Add(temp.name);
    //    return result;
    //}

    public string[] GetNamesWays()
    {
        string[] result = new string[bezierWays.Count];
        for (int i = 0; i < bezierWays.Count; i++)
            result[i] = bezierWays[i].name;
        return result;
    }

    public int GetCountWay()
    {
        return bezierWays.Count;
    }

}

