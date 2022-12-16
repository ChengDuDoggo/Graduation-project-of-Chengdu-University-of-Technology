using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="SceneSoundList_SO",menuName ="Sound/SceneSoundList")]
public class SceneSoundList_SO : ScriptableObject
{
    public List<SceneSoundItem> sceneSoundList;
    public SceneSoundItem GetSceneSoundItem(string name)
    {
        return sceneSoundList.Find(s => s.sceneName == name);//拉姆达表达式
    }
}
[System.Serializable]
public class SceneSoundItem
{
    [SceneName] public string sceneName;
    public SoundName ambient;
    public SoundName music;
}
