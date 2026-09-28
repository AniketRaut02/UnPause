using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Levels/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public float timerDuration = 60f;
    public bool startWithDashLocked = false;
    public bool startWithInvertedControls = false;
    public bool firstDeathIsFree = false;
    public string nextSceneName;

}