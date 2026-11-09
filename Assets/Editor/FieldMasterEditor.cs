using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldMaster))]
public class FieldMasterEditor: Editor
{
    // FieldMaster.gridsを操作するプロパティ
    private SerializedProperty _grids;

    // 選択中のフィールド単一の種別
    private FieldGridEntityType _selectType = FieldGridEntityType.NONE;

    // フィールド単一の種別リスト
    private readonly FieldGridEntityType[] _typeList =
    {
        FieldGridEntityType.NONE,
        FieldGridEntityType.ROAD,
        FieldGridEntityType.FOREST
    };

    private void OnEnable()
    {
        // FieldMaster.gridsを設定する
        _grids = serializedObject.FindProperty("grids");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("フィールドの配置を編集します。", EditorStyles.boldLabel);

        // FieldMaster.gridsを最新の状態に更新
        serializedObject.Update();

        // 設定詳細のヘルプボックス
        EditorGUILayout.HelpBox(
            "下記種別の色をクリックすることで選択中の色が変わります。\nその状態でマス目をクリックすると選択中の色に変わります。",
             MessageType.Info
        );
        GUILayout.Space(10);

        // フィールド全体
        for(int i = 0; i < FieldMaster.Height; i++)
        {
            // 水平方向へのレイアウト開始
            EditorGUILayout.BeginHorizontal();
            for(int j = 0; j < FieldMaster.Width; j++)
            {
                int index = i * FieldMaster.Width + j;
                // FieldMaster.grids[index]を設定する
                SerializedProperty grid = _grids.GetArrayElementAtIndex(index);
                // 30x30の幅で自動拡張しない領域を取得する
                Rect gridRect = GUILayoutUtility.GetRect(30, 30, GUILayout.ExpandWidth(false));
                // 領域を描画する
                Color gridColor = GetFieldGridEntityTypeColor((FieldGridEntityType)grid.enumValueIndex);
                EditorGUI.DrawRect(gridRect, gridColor);
                // 透明なボタンを描画する
                if(GUI.Button(gridRect, GUIContent.none, GUIStyle.none))
                {
                    // ボタンが押下された場合、選択中の色を設定する
                    grid.enumValueIndex = (int)_selectType;
                    Debug.Log("grid[" + j + " , " + i + "]を押下");
                }
                GUILayout.Space(2);
            }
            // 水平方向へのレイアウト終了
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(2);
        }
        GUILayout.Space(10);
        
        // 水平方向へのレイアウト開始
        EditorGUILayout.BeginHorizontal();
        // フィールド単一の種別に紐づく色
        foreach(var item in _typeList)
        {
            // 30x30の幅で自動拡張しない領域を取得する
            Rect typeRect = GUILayoutUtility.GetRect(30, 30, GUILayout.ExpandWidth(false));
            Color typeColor = GetFieldGridEntityTypeColor(item);
            // 領域を描画する
            EditorGUI.DrawRect(typeRect, typeColor);
            // ieldGridEntityType.NONE以外の場合、透明なボタンを描画する
            if(item != FieldGridEntityType.NONE && GUI.Button(typeRect, GUIContent.none, GUIStyle.none))
            {
                // ボタンが押下された場合、選択中の種別に押下した種別を設定する
                _selectType = item;
                Debug.Log(nameof(FieldGridEntityType) + "(" + item + ")を押下");
            }
            EditorGUILayout.LabelField(item.ToString(), GUILayout.Width(50), GUILayout.Height(30));
        }
        // 選択中の色
        // 30x30の幅で自動拡張しない領域を取得する
        Rect selectTypeRect = GUILayoutUtility.GetRect(30, 30, GUILayout.ExpandWidth(false));
        // 領域を描画する
        EditorGUI.DrawRect(selectTypeRect, GetFieldGridEntityTypeColor(_selectType));
        EditorGUILayout.LabelField("選択中", GUILayout.Width(50), GUILayout.Height(30));
        // 水平方向へのレイアウト終了
        EditorGUILayout.EndHorizontal();

        // 設定情報をFieldMaster.gridsへ設定する
        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// フィールド単一の種別の色を取得する
    /// </summary>
    /// <param name="type">フィールド単一の種別</param>
    /// <returns>色情報</returns>
    private Color GetFieldGridEntityTypeColor(FieldGridEntityType type)
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
