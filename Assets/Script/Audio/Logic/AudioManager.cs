using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    [Header("音乐数据库")]
    public SoundDetailsList_SO soundDetailsData;
    public SceneSoundList_SO sceneSoundData;//都是存放音乐的数据库,但是键值不同所有作用不同
    [Header("Audio Source")]
    public AudioSource ambientSource;
    public AudioSource gameSourse;
    public float MusicStartSecond => Random.Range(5f, 15f);
    private Coroutine soundRoutine;
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;//Unity自带的音效混响组件的总控件,该组件可以方便的控制音效音乐的切换淡入淡出等待效果
    [Header("音响快照")]
    public AudioMixerSnapshot normalSnapshot;//AudioMixer组件下的快照属性,每一个单独的快照控制单独的音乐轨道,不同的音效效果Volume值大小等
    public AudioMixerSnapshot ambientSnapshot;
    public AudioMixerSnapshot muteSnapshot;
    private float musicTransitionSecond = 8f;
    private void OnEnable()
    {
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.PlaySoundEvent += OnPlaySoundEvent;
        EventHandler.EndGameEvent += OnEndGameEvent;
    }
    private void OnDisable()
    {
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.PlaySoundEvent -= OnPlaySoundEvent;
        EventHandler.EndGameEvent -= OnEndGameEvent;
    }

    private void OnEndGameEvent()
    {
        if (soundRoutine != null)
        {
            StopCoroutine(soundRoutine);
        }
        muteSnapshot.TransitionTo(1f);
    }

    private void OnPlaySoundEvent(SoundName soundName)
    {
        var soundDetails = soundDetailsData.GetSoundDetails(soundName);
        if (soundDetails != null)
            EventHandler.CallInitSoundEffect(soundDetails);
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
            PlayAmbientClip(ambient,1f);//先播放环境音效
            yield return new WaitForSeconds(MusicStartSecond);
            PlayMusicClip(music,musicTransitionSecond);//再播放背景音乐
        }
    }
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="soundDetails"></param>
    private void PlayMusicClip(SoundDetails soundDetails,float transitionTime)
    {
        audioMixer.SetFloat("MusicVolume", ConvertSoundVolume(soundDetails.soundVolume));//为一个单独的轨道设定Volume值(float要转化为比特)
        gameSourse.clip = soundDetails.soundClip;
        if (gameSourse.isActiveAndEnabled)
            gameSourse.Play();
        normalSnapshot.TransitionTo(transitionTime);//对快照进行插值转换(淡入淡出)
    }
    /// <summary>
    /// 播放环境音效
    /// </summary>
    /// <param name="soundDetails"></param>
    private void PlayAmbientClip(SoundDetails soundDetails, float transitionTime)
    {
        audioMixer.SetFloat("AmbientVolume", ConvertSoundVolume(soundDetails.soundVolume));
        ambientSource.clip = soundDetails.soundClip;
        if (ambientSource.isActiveAndEnabled)
            ambientSource.Play();
        ambientSnapshot.TransitionTo(transitionTime);//对快照进行插值转换(淡入淡出)
    }
    /// <summary>
    /// 浮点数转化为音响比特数
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    private float ConvertSoundVolume(float amount)
    {
        return (amount * 100 - 80);
    }
    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", (value * 100 - 80));
    }
}
