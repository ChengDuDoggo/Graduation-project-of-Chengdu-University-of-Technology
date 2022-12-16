using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("�������ݿ�")]
    public SoundDetailsList_SO soundDetailsData;
    public SceneSoundList_SO sceneSoundData;//���Ǵ�����ֵ����ݿ�,���Ǽ�ֵ��ͬ�������ò�ͬ
    [Header("Audio Source")]
    public AudioSource ambientSource;
    public AudioSource gameSourse;
    public float MusicStartSecond => Random.Range(5f, 15f);
    private Coroutine soundRoutine;
    private void OnEnable()
    {
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
    }
    private void OnDisable()
    {
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
    }

    private void OnAfterSceneLoadedEvent()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneSoundItem sceneSound = sceneSoundData.GetSceneSoundItem(currentSceneName);
        if (sceneSound == null)
            return;
        SoundDetails ambient = soundDetailsData.GetSoundDetails(sceneSound.ambient);
        SoundDetails music = soundDetailsData.GetSoundDetails(sceneSound.music);
        if (soundRoutine != null)
            StopCoroutine(soundRoutine);
        soundRoutine = StartCoroutine(PlaySoundRoutine(music, ambient));
    }
    private IEnumerator PlaySoundRoutine(SoundDetails music,SoundDetails ambient)
    {
        if (music != null && ambient != null)
        {
            PlayAmbientClip(ambient);
            yield return new WaitForSeconds(MusicStartSecond);
            PlayMusicClip(music);
        }
    }
    /// <summary>
    /// ���ű�������
    /// </summary>
    /// <param name="soundDetails"></param>
    private void PlayMusicClip(SoundDetails soundDetails)
    {
        gameSourse.clip = soundDetails.soundClip;
        if (gameSourse.isActiveAndEnabled)
            gameSourse.Play();
    }
    /// <summary>
    /// ���Ż�����Ч
    /// </summary>
    /// <param name="soundDetails"></param>
    private void PlayAmbientClip(SoundDetails soundDetails)
    {
        ambientSource.clip = soundDetails.soundClip;
        if (ambientSource.isActiveAndEnabled)
            ambientSource.Play();
    }
}
