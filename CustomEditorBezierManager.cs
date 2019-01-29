using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//TODO сделать отдельные методы для создания и удаления пути

#if UNITY_EDITOR
[CustomEditor(typeof(WayProfiler))]
public class CustomEditorWayProfiler : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Очистить профайлер"))
        {
            WayProfiler wayProfiler = target as WayProfiler;
            ((WayProfiler)target).ClearWayProfiler();
        }
    }
}



[CustomEditor (typeof(BezierManager))]
public class CustomEditorBezierManager : Editor
{
    private const int POINT_COUNT = 50;

    private Color COLOR_WAY = Color.green;

    private BezierManager bezierManager;

    private string nameWay = string.Empty;

    private string[] wayPopup = new string[] { };

    private Color colorWay = Color.green;

    private int prevIndex = 0;

    private void OnEnable()
    {
        bezierManager = target as BezierManager;
    }

    private BezierWay CreateWay(string name, int countPoint, Color color)
    {
        BezierWay bezierWay = new BezierWay();
        bezierWay.color = color;
        bezierWay.name = name;       
        bezierManager.currentWay = bezierWay;
        bezierManager.wayProfiler.bezierWays.Add(bezierWay);
        bezierManager.currentWay.bezierDatas.Add(new BezierData(Vector3.zero, new Vector3(20, 0, 0), new Vector3(0, 0, 20), new Vector3(20, 0, 20), countPoint, true,true));
        bezierManager.currentWay.bezierDatas[bezierManager.currentWay.bezierDatas.Count - 1].isTail = true;
        return bezierWay;
    }

    private void AddCurve(int countPoint)
    {
        bezierManager.currentWay.bezierDatas.Add(new BezierData(bezierManager.currentWay.bezierDatas[0].P3, new Vector3(20, 0, 0), new Vector3(0, 0, 20), new Vector3(20, 0, 20), countPoint, false, true));
        bezierManager.currentWay.bezierDatas[bezierManager.currentWay.bezierDatas.Count - 2].isTail = false;
    }

    private void FillWayPopup()
    {
        int count = bezierManager.wayProfiler.bezierWays.Count;
        wayPopup = new string[count];
        for (int i = 0; i < count; i++)
        {
            wayPopup[i] = bezierManager.wayProfiler.bezierWays[i].name;
        }
    }

    /// <summary>
    /// Проверка существует ли уже такое имя в списке
    /// </summary>
    /// <param name="name"></param>
    /// <returns>true - такое имя уже есть, false - такого имени нет</returns>
    private bool CheckExistName(string name)
    {
        for (int i = 0; i < wayPopup.Length; i++)
        {
            if (name == wayPopup[i])
            {
                return true;
            }
        }
        return false;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUIStyle styleHeader = new GUIStyle();
        styleHeader.fontSize = 20;
        styleHeader.alignment = TextAnchor.MiddleCenter;
        FillWayPopup();
        for (int i = 0; i < 10; i++)
            EditorGUILayout.Space();
        EditorGUILayout.LabelField("Область создания пути", styleHeader);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        nameWay = EditorGUILayout.TextField("Название пути", nameWay);
        if (GUILayout.Button("Создать путь"))
        {
            if (nameWay == string.Empty)
            {
                Debug.LogError("Укажите имя пути");
            }
            else if (CheckExistName(nameWay))
            {
                Debug.LogError("Путь с таким именем уже существует. Задайте другое имя");
            }
            else
            {
                bezierManager.indexInPopup = wayPopup.Length;
                CreateWay(nameWay, POINT_COUNT, COLOR_WAY);
            }
        }

        for (int i = 0; i < 10; i++)
            EditorGUILayout.Space();
        EditorGUILayout.LabelField("Область редактироавния пути", styleHeader);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        bezierManager.indexInPopup = EditorGUILayout.Popup("Выбор пути", bezierManager.indexInPopup, wayPopup);
        if (prevIndex != bezierManager.indexInPopup)
        {
            prevIndex = bezierManager.indexInPopup;
            bezierManager.currentWay = bezierManager.wayProfiler.bezierWays[bezierManager.indexInPopup];
            EditorUtility.SetDirty(bezierManager);
        }


        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if(bezierManager.wayProfiler.bezierWays.Count > 0)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Цвет пути");
            colorWay = EditorGUILayout.ColorField(bezierManager.currentWay.color);
            GUILayout.EndHorizontal();
            bezierManager.currentWay.color = colorWay;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Добавить кривую"))
            {
                //AddCurve(POINT_COUNT);
                bezierManager.currentWay.AddCurve(POINT_COUNT);
            }
            /*
            if (GUILayout.Button("Удалить весь путь"))
            {
                bezierManager.wayProfiler.bezierWays.RemoveAt(bezierManager.wayProfiler.bezierWays.Count - 1);
                bezierManager.indexInPopup = wayPopup.Length - 2;
            }
            */
            if (bezierManager.currentWay.bezierDatas.Count > 1)
            {
                if (GUILayout.Button("Удалить последнюю кривую"))
                {
                    bezierManager.currentWay.bezierDatas.RemoveAt(bezierManager.currentWay.bezierDatas.Count - 1);
                    bezierManager.currentWay.bezierDatas[bezierManager.currentWay.bezierDatas.Count - 1].isTail = true;
                }
            }

            GUILayout.EndHorizontal();
        }



        for (int i = 0; i < 30; i++)
            EditorGUILayout.Space();

        EditorUtility.SetDirty(bezierManager.wayProfiler);
        serializedObject.ApplyModifiedProperties();

    }

    private Vector3 ResetY(Vector3 vec)
    {
        return new Vector3(vec.x, 0.0f, vec.z);
    }

    private void OnSceneGUI()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.blue;
        style.fontSize = 30;

        if (bezierManager.wayProfiler.bezierWays.Count > 0 && bezierManager.currentWay != null)
        {
            for (int i = 0; i < bezierManager.currentWay.bezierDatas.Count; i++)
            {
                Handles.color = Color.red;
                Handles.DrawLine(bezierManager.currentWay.bezierDatas[i].P0, bezierManager.currentWay.bezierDatas[i].P1);
                Handles.DrawLine(bezierManager.currentWay.bezierDatas[i].P2, bezierManager.currentWay.bezierDatas[i].P3);

                if (bezierManager.currentWay.bezierDatas[i].isHead)
                    bezierManager.currentWay.bezierDatas[i].P0 = Handles.FreeMoveHandle(bezierManager.currentWay.bezierDatas[i].P0, Quaternion.identity, 3.0f, Vector3.zero, Handles.SphereHandleCap);
                else
                    bezierManager.currentWay.bezierDatas[i].P0 = bezierManager.currentWay.bezierDatas[i - 1].P3;

                bezierManager.currentWay.bezierDatas[i].P1 = Handles.FreeMoveHandle(bezierManager.currentWay.bezierDatas[i].P1, Quaternion.identity, 3.0f, Vector3.zero, Handles.SphereHandleCap);
                bezierManager.currentWay.bezierDatas[i].P2 = Handles.FreeMoveHandle(bezierManager.currentWay.bezierDatas[i].P2, Quaternion.identity, 3.0f, Vector3.zero, Handles.SphereHandleCap);
                bezierManager.currentWay.bezierDatas[i].P3 = Handles.FreeMoveHandle(bezierManager.currentWay.bezierDatas[i].P3, Quaternion.identity, 3.0f, Vector3.zero, Handles.SphereHandleCap);

                bezierManager.currentWay.bezierDatas[i].P0 = ResetY(bezierManager.currentWay.bezierDatas[i].P0);
                bezierManager.currentWay.bezierDatas[i].P1 = ResetY(bezierManager.currentWay.bezierDatas[i].P1);
                bezierManager.currentWay.bezierDatas[i].P2 = ResetY(bezierManager.currentWay.bezierDatas[i].P2);
                bezierManager.currentWay.bezierDatas[i].P3 = ResetY(bezierManager.currentWay.bezierDatas[i].P3);

                if (bezierManager.currentWay.bezierDatas[i].isHead)
                    Handles.Label(bezierManager.currentWay.bezierDatas[i].P0, "Start", style);
                if (bezierManager.currentWay.bezierDatas[i].isTail)
                    Handles.Label(bezierManager.currentWay.bezierDatas[i].P3, "Finish", style);
                Handles.color = bezierManager.currentWay.color;
                bezierManager.currentWay.bezierDatas[i].FillPoints();
                bezierManager.currentWay.bezierDatas[i].DrawLine();

            }
        }
        else
        {
            bezierManager.currentWay = null;
            bezierManager.indexInPopup = 0;
        }

        SceneView.RepaintAll();
    }

}
#endif