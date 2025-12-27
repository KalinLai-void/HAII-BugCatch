using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIServices.Implementations.ElevenLabs;
using UnityEngine.Events;
public class TTSManager : MonoBehaviour
{
    public ElevenLabsTtsService ttsService;
    public Animator animator;
    public void TextToSpeech(string ctx)
    {
        if(gameObject.activeSelf)   ttsService.Speak(ctx, HandleTtsAudioClip, HandleTtsError);   
    }

    private void HandleTtsAudioClip(AudioClip clip) // Copy from AIServices.Core.NpcInteractionManager
    {
        Debug.Log("TTS 音頻準備就緒");

        // 播放音頻 - 尋找AudioSource組件
        AudioSource audioSource = null;

        if (audioSource == null)
        {
            // 如果沒有設置voiceHandler，嘗試在當前GameObject上找AudioSource
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            // 如果還是沒有，創建一個臨時的AudioSource
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.Log("自動創建AudioSource組件");
        }

        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
            StartCoroutine(CheckAudioStatus(audioSource));
            Debug.Log("播放TTS音頻");
        }
        else
        {
            Debug.LogWarning("無法找到或創建AudioSource組件");
        }
    }
    private void HandleTtsError(string error) // Copy from AIServices.Core.NpcInteractionManager
    {
        Debug.LogError($"TTS 錯誤: {error}");
    }

    IEnumerator CheckAudioStatus(AudioSource audioSource)
    {
        while(audioSource.isPlaying)
        {
            if (!animator.GetBool("IsTalking")) animator.SetBool("IsTalking", true);
            yield return null;

        }
        animator.SetBool("IsTalking", false);
    }
}
