using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnEditor : EditorWindow
{
    private SpawnProfiler _spawnProfiler;

    private WayProfiler _wayProfiler;

    private AvailableSpawnObjectsProfiler _availableSpawnObjectsProfiler;

    private GUIStyle _styleHeader = new GUIStyle();

    private GUIStyle _styleHeaderWave = new GUIStyle();

    private string[] _wayList;

    private bool _showSetWay;

    private Vector2 scrollView;



    int countTest;

    GameObject gameObject = null;

    [MenuItem("Tower Defence/Spawn Editor")]
    public static void CreateWindow()
    {
        EditorWindow.GetWindow<SpawnEditor>();
    }

    private void OnGUI()
    {
        _styleHeader.fontStyle = FontStyle.Bold;
        _styleHeaderWave.fontStyle = FontStyle.Bold;
       
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Спавн профайлер", _styleHeader, GUILayout.Width(300));
        _spawnProfiler = EditorGUILayout.ObjectField(_spawnProfiler, typeof(SpawnProfiler), true, GUILayout.Width(400)) as SpawnProfiler;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Профайлер путей", _styleHeader, GUILayout.Width(300));
        _wayProfiler = EditorGUILayout.ObjectField(_wayProfiler, typeof(WayProfiler), true, GUILayout.Width(400)) as WayProfiler;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Профайлер доступных объектов для спавна", _styleHeader, GUILayout.Width(300));
        _availableSpawnObjectsProfiler = EditorGUILayout.ObjectField(_availableSpawnObjectsProfiler, typeof(AvailableSpawnObjectsProfiler), true, GUILayout.Width(400)) as AvailableSpawnObjectsProfiler;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        if (_spawnProfiler && _wayProfiler && _availableSpawnObjectsProfiler)
        {

            //otladka
            _spawnProfiler.availableSpawn = _availableSpawnObjectsProfiler;
            //_spawnProfiler.InitWave(HelperSpawn.countWave);
            //_spawnProfiler.InitWave(_spawnProfiler.WAVE_COUNT_FROM_INSPECTOR);
            //otladka

            #region Ввод основных параметров
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Номер левела", _styleHeader, GUILayout.Width(300));
            _spawnProfiler.levelNumber = EditorGUILayout.IntField(_spawnProfiler.levelNumber, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField("Количество волн на левеле   " + _spawnProfiler.waveInfo.waveCount, _styleHeader, GUILayout.Width(300));
            EditorGUILayout.LabelField("Количество волн на левеле   " , _styleHeader, GUILayout.Width(300));
            int tempWaveCount = EditorGUILayout.DelayedIntField(_spawnProfiler.waveInfo.waveCount, GUILayout.Width(100));
            if (_spawnProfiler.waveInfo.waveCount != tempWaveCount)
            {
                if (!_spawnProfiler.InitWave(tempWaveCount))
                    Debug.LogError("Недопустимое значение для изменения количества волн");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Минимальная задержка спавна объекта", _styleHeader, GUILayout.Width(300));
            _spawnProfiler._minDelaySpawn = EditorGUILayout.FloatField(_spawnProfiler._minDelaySpawn, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Максимальная задержка спавна объекта", _styleHeader, GUILayout.Width(300));
            _spawnProfiler._maxDelaySpawn = EditorGUILayout.FloatField(_spawnProfiler._maxDelaySpawn, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

  

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Количество спавн поинтов на левеле", _styleHeader, GUILayout.Width(300));
            int tempSpawnPointCount = EditorGUILayout.DelayedIntField(_spawnProfiler.spawnPointCount, GUILayout.Width(100));
            if (_spawnProfiler.spawnPointCount != tempSpawnPointCount)
            {
                if (!_spawnProfiler.ResizeSpawnPointCount(tempSpawnPointCount))
                    Debug.LogError("Недопустимое значение для изменения количества спавн поинтов");
            }


            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
#endregion

            _wayList = new string[_wayProfiler.GetCountWay()];
            _wayList = _wayProfiler.GetNamesWays();

            _showSetWay = EditorGUILayout.Foldout(_showSetWay, "Выбор пути для спавн поинтов");
            if (_showSetWay)
            {
                for (int j = 0; j < _spawnProfiler.spawnPointCount; j++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Спавн поинт № " + (j + 1), GUILayout.Width(300));
                    int selectedWayIndex = EditorGUILayout.Popup(_spawnProfiler.spawnDatas[j].selectedWayIndex, _wayList);
                    EditorGUILayout.EndHorizontal();
                    _spawnProfiler.spawnDatas[j].namePath = _wayList[selectedWayIndex];
                    _spawnProfiler.spawnDatas[j].selectedWayIndex = selectedWayIndex;
                    _spawnProfiler.spawnDatas[j].path = _wayProfiler.GetPointsWay(_spawnProfiler.spawnDatas[j].namePath);
                }
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            scrollView = EditorGUILayout.BeginScrollView(scrollView);
            for (int indexWaveNumber = 0; indexWaveNumber < _spawnProfiler.waveInfo.waveCount; indexWaveNumber++)
            {
                EditorGUILayout.LabelField("Волна № " + (indexWaveNumber + 1), _styleHeader, GUILayout.Width(300));
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Секунд до волны", GUILayout.Width(300));
                _spawnProfiler.waveInfo.timeBeforeWave[indexWaveNumber] = EditorGUILayout.DelayedIntField(_spawnProfiler.waveInfo.timeBeforeWave[indexWaveNumber], GUILayout.Width(100));
                EditorGUILayout.EndHorizontal();



                for (int indexSpawnPoint = 0; indexSpawnPoint < _spawnProfiler.spawnPointCount; indexSpawnPoint++)
                {
                    EditorGUILayout.LabelField("    Спавн поинт № " + (indexSpawnPoint + 1), _styleHeader, GUILayout.Width(300));
                 

                    for (int indexAvalaibleSpawnObjects = 0; indexAvalaibleSpawnObjects < _availableSpawnObjectsProfiler.spawnObjects.Count; indexAvalaibleSpawnObjects++)
                    {
                        //GameObject temp;
                        int tempV = 1;
                        EditorGUILayout.BeginHorizontal();
                        //  temp = EditorGUILayout.ObjectField(_availableSpawnObjectsProfiler.spawnObjects[indexAvalaibleSpawnObjects], typeof(GameObject), true, GUILayout.Width(500)) as GameObject;
                        if (_availableSpawnObjectsProfiler.spawnObjects[indexAvalaibleSpawnObjects] != null)
                        {
                            EditorGUILayout.LabelField("    " + _availableSpawnObjectsProfiler.spawnObjects[indexAvalaibleSpawnObjects].name, GUILayout.Width(300));
                            tempV = EditorGUILayout.DelayedIntField(_spawnProfiler.spawnDatas[indexSpawnPoint].spawnPointDatas[indexWaveNumber].countForSpawn[indexAvalaibleSpawnObjects]);
                            _spawnProfiler.spawnDatas[indexSpawnPoint].spawnPointDatas[indexWaveNumber].countForSpawn[indexAvalaibleSpawnObjects] = tempV;
                        }

                        EditorGUILayout.EndHorizontal();
                    }


              
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                }



                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();


            }



            EditorGUILayout.EndScrollView();
        }

        if (GUI.changed)
            EditorUtility.SetDirty(_spawnProfiler);

    }
}


