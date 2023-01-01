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
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;//Unity�Դ�����Ч����������ܿؼ�,��������Է���Ŀ�����Ч���ֵ��л����뵭���ȴ�Ч��
    [Header("�������")]
    public AudioMixerSnapshot normalSnapshot;//AudioMixer����µĿ�������,ÿһ�������Ŀ��տ��Ƶ��������ֹ��,��ͬ����ЧЧ��Volumeֵ��С��
    public AudioMixerSnapshot ambientSnapshot;
    public AudioMixerSnapshot muteSnapshot;
    private float musicTransitionSecond = 8f;
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
            PlayAmbientClip(ambient,1f);//�Ȳ��Ż�����Ч
            yield return new WaitForSeconds(MusicStartSecond);
            PlayMusicClip(music,musicTransitionSecond);//�ٲ��ű�������
        }
    }
    /// <summary>
    /// ���ű�������
    /// </summary>
    /// <param name="soundDetails"></param>
    private void PlayMusicClip(SoundDetails soundDetails,float transitionTime)
    {
        audioMixer.SetFloat("MusicVolume", ConvertSoundVolume(soundDetails.soundVolume));//Ϊһ�������Ĺ���趨Volumeֵ(floatҪת��Ϊ����)
        gameSourse.clip = soundDetails.soundClip;
        if (gameSourse.isActiveAndEnabled)
            gameSourse.Play();
        normalSnapshot.TransitionTo(transitionTime);//�Կ��ս��в�ֵת��(���뵭��)
    }
    /// <summary>
    /// ���Ż�����Ч
    /// </summary>
    /// <param name="soundDetails"></param>
    private void PlayAmbientClip(SoundDetails soundDetails, float transitionTime)
    {
        audioMixer.SetFloat("AmbientVolume", ConvertSoundVolume(soundDetails.soundVolume));
        ambientSource.clip = soundDetails.soundClip;
        if (ambientSource.isActiveAndEnabled)
            ambientSource.Play();
        ambientSnapshot.TransitionTo(transitionTime);//�Կ��ս��в�ֵת��(���뵭��)
    }
    /// <summary>
    /// ������ת��Ϊ���������
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    private float ConvertSoundVolume(float amount)
    {
        return (amount * 100 - 80);
    }
}
