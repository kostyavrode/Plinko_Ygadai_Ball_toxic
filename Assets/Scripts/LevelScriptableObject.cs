using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Game/Level")]
public class LevelScriptableObject : ScriptableObject
{
    public Vector2[] ballPositions;
    public Vector2[] targetPositions;
}