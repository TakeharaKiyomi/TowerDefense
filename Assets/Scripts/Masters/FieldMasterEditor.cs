using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldMaster))]
public class FieldMasterEditor: Editor
{
    private SerializedProperty _cells;

    private FieldGridEntityType _selectType = FieldGridEntityType.ROAD;

    private Color _baseColor;

    private FieldGridEntityType[] _selectList =
    {
        FieldGridEntityType.NONE,
        FieldGridEntityType.ROAD,
        FieldGridEntityType.FOREST
    };

    private void OnEnable()
    {
        _cells = serializedObject.FindProperty("cells");
        _baseColor = GUI.backgroundColor;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("フィールドの配置を編集します。", EditorStyles.boldLabel);
        serializedObject.Update();

        EditorGUILayout.HelpBox(
            "下記種別の色をクリックすることで選択中の色が変わります。\nその状態でマス目をクリックすると選択中の色に変わります。",
             MessageType.Info
        );

        GUILayout.Space(10);

        for(int i = 0; i < FieldMaster.Height; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for(int j = 0; j < FieldMaster.Width; j++)
            {
                int index = i * FieldMaster.Width + j;
                SerializedProperty cell = _cells.GetArrayElementAtIndex(index);

                Rect rect = GUILayoutUtility.GetRect(30, 30, GUILayout.Width(30), GUILayout.Height(30));
                Color color = GetColor((FieldGridEntityType)cell.enumValueIndex);
                EditorGUI.DrawRect(rect, color);
                GUI.backgroundColor = color;
                if(GUI.Button(rect, GUIContent.none, GUIStyle.none))
                {
                    cell.enumValueIndex = (int)_selectType;
                    Debug.Log("ボタン押されたよ");
                }
                GUI.backgroundColor = _baseColor;
                GUILayout.Space(2);
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(2);
        }
        GUILayout.Space(10);
        
        EditorGUILayout.BeginHorizontal();
        
        foreach(var item in _selectList)
        {
            Rect rect2 = GUILayoutUtility.GetRect(30, 30, GUILayout.Width(30), GUILayout.Height(30));
            Color color = GetColor(item);
            EditorGUI.DrawRect(rect2, color);
            GUI.backgroundColor = color;
            if(GUI.Button(rect2, GUIContent.none, GUIStyle.none))
            {
                _selectType = item;
                Debug.Log("ボタン押されたよ");
            }
            GUI.backgroundColor = _baseColor;
            EditorGUILayout.LabelField(item.ToString(), GUILayout.Width(50), GUILayout.Height(30));
        }

        Rect rect4 = GUILayoutUtility.GetRect(30, 30, GUILayout.Width(30), GUILayout.Height(30));
        EditorGUI.DrawRect(rect4, GetColor(_selectType));
        EditorGUILayout.LabelField("選択中", GUILayout.Width(50), GUILayout.Height(30));
        
        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
    }

    private Color GetColor(FieldGridEntityType type)
    {
        return type switch
        {
            FieldGridEntityType.NONE => Color.gray,
            FieldGridEntityType.ROAD => Color.yellow,
            FieldGridEntityType.FOREST => Color.green,
            _ => Color.red
        };
    }
}
