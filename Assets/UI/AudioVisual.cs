using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVisual : MonoBehaviour
{
    [SerializeField] private float statValue;
    [SerializeField] private List<float> thresholds;
    [SerializeField] private List<AudioClip> audioClips;
    [SerializeField] private List<Image> overlay;
    [SerializeField] private bool loopAudio;

    private AudioSource audioSource;
    private int currentThreshold = 0;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (statValue != thresholds[currentThreshold]) {
            Clear();
            currentThreshold = statValue > thresholds[currentThreshold] ? currentThreshold + 1 : currentThreshold - 1;
            PlayAudio();
            ShowVisual();
        }
    }

    //TODO: add a way to play audio clips in a loop
    void PlayAudio() {
        if (audioClips[currentThreshold] != null) {
            audioSource.clip = audioClips[currentThreshold];
            audioSource.Play();
        }
    }

    //TODO: add a way to make animations with the overlay
    void ShowVisual() {
        if (overlay[currentThreshold] != null) {
            overlay[currentThreshold].enabled = true;
        }
    }

    void Clear() {
        audioSource.Stop();
        overlay[currentThreshold].enabled = false;
    }

}
