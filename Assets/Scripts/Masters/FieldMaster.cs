using UnityEngine;

[CreateAssetMenu(fileName = "FieldMaster", menuName = "Scriptable Objects/FieldMaster")]
public class FieldMaster : ScriptableObject
{
    public const int Width = 16;
    public const int Height = 10;

    public FieldGridEntityType[] cells = new FieldGridEntityType[Width * Height];
}
