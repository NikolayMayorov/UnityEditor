using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HeroParamEditor : EditorWindow
{
    private HeroParam _playerParam;

    private GUIStyle _styleHeader = new GUIStyle();

    private GUIStyle _styleSubHeader = new GUIStyle();

    private bool _showMainSkill = false;

    private bool _showOtherSkill = false;

    [MenuItem("Tower Defence/Hero param")]
    public static void CreateWindow()
    {
        EditorWindow.GetWindow<HeroParamEditor>();
    }

    private void OnGUI()
    {
        _styleHeader.fontStyle = FontStyle.Bold;


        _styleSubHeader.fontStyle = FontStyle.Bold;


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Профайлер с настройками героя", _styleHeader, GUILayout.Width(400));
        _playerParam = EditorGUILayout.ObjectField(_playerParam, typeof(HeroParam), true, GUILayout.Width(400)) as HeroParam;
        EditorGUILayout.EndHorizontal();
       
        if (_playerParam != null)
        {
            #region Основные способности героя Rage, Curse, Star
            EditorGUILayout.Separator();
    
            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField("Показать основные способности героя", GUILayout.Width(300));
            //_showMainSkill = EditorGUILayout.Toggle( _showMainSkill);
            //EditorGUILayout.EndHorizontal();

           // if (_showMainSkill)
            {
                EditorGUILayout.LabelField("Активные способности героя", _styleHeader);
                EditorGUILayout.Separator();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Время отката - Бешенство", GUILayout.Width(300));
                _playerParam.skillTwoCooldownTimerValue = EditorGUILayout.DelayedFloatField( _playerParam.skillTwoCooldownTimerValue, GUILayout.Width(400));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Время отката - Проклятая земля", GUILayout.Width(300));
                _playerParam.skillThreeCooldownTimerValue = EditorGUILayout.DelayedFloatField(_playerParam.skillThreeCooldownTimerValue, GUILayout.Width(400));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Время отката - Взрыв", GUILayout.Width(300));
                _playerParam.skillFourCooldownTimerValue = EditorGUILayout.DelayedFloatField(_playerParam.skillFourCooldownTimerValue, GUILayout.Width(400));
                EditorGUILayout.EndHorizontal();

            }

            #endregion


            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField("Показать остальные способности героя", GUILayout.Width(300));
            //_showOtherSkill = EditorGUILayout.Toggle(_showOtherSkill);
            //EditorGUILayout.EndHorizontal();

           // if (_showOtherSkill)
            {
                #region TriggerSkill
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Триггерные навыки героя", _styleHeader);
                EditorGUILayout.LabelField("Шрапнель", _styleSubHeader);
                _playerParam.cooldownTimerValueFlak = EditorGUILayout.DelayedFloatField("Время отката ", _playerParam.cooldownTimerValueFlak);
                _playerParam.probabilityFlak = EditorGUILayout.DelayedFloatField("Probability ", _playerParam.probabilityFlak);

               // EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Огненная волна", _styleSubHeader);
                _playerParam.cooldownTimerValueFlame = EditorGUILayout.DelayedFloatField("Время отката ", _playerParam.cooldownTimerValueFlame);
                _playerParam.probabilityFlame = EditorGUILayout.DelayedFloatField("Probability ", _playerParam.probabilityFlame);

              //  EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Иглы", _styleSubHeader);
                _playerParam.cooldownTimerValueNails = EditorGUILayout.DelayedFloatField("Время отката ", _playerParam.cooldownTimerValueNails);
                _playerParam.probabilityNails = EditorGUILayout.DelayedFloatField("Probability ", _playerParam.probabilityNails);
                #endregion

                #region KillSkill
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Навыки убийства героя", _styleHeader);
                EditorGUILayout.LabelField("Мина", _styleHeader);
                _playerParam.killCountValueMine = EditorGUILayout.DelayedFloatField("Kill count ", _playerParam.killCountValueMine);
                EditorGUILayout.LabelField("Гейзер", _styleHeader);
                _playerParam.killCountValueGazer = EditorGUILayout.DelayedFloatField("Kill count ", _playerParam.killCountValueGazer);     
                EditorGUILayout.LabelField("Кража энергии", _styleHeader);
                _playerParam.killCountValueRefresh = EditorGUILayout.DelayedFloatField("Kill count ", _playerParam.killCountValueRefresh);
                #endregion
            }
        }


        if (GUI.changed)
            EditorUtility.SetDirty(_playerParam);
    }

}
