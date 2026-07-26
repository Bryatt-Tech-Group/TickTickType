using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource sourceA;
    public AudioSource sourceB;
    public float fadeDuration = 1.5f;

    private AudioSource activeSource;
    private AudioSource inactiveSource;

    void Start()
    {
        activeSource = sourceA;
        inactiveSource = sourceB;
    }

    public void PlaySong(AudioClip clip, bool loop = true)
    {
        StopAllCoroutines();
        StartCoroutine(CrossfadeTo(clip, loop));
    }

    IEnumerator CrossfadeTo(AudioClip newClip, bool loop)
    {
        inactiveSource.clip = newClip;
        inactiveSource.loop = loop;
        inactiveSource.volume = 0f;
        inactiveSource.Play();

        float t = 0f;
        float startVolume = activeSource.volume;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float progress = t / fadeDuration;

            activeSource.volume = Mathf.Lerp(startVolume, 0f, progress);
            inactiveSource.volume = Mathf.Lerp(0f, 1f, progress);

            yield return null;
        }

        activeSource.Stop();
        activeSource.volume = startVolume; // reset for next time it's reused

        // Swap roles
        (activeSource, inactiveSource) = (inactiveSource, activeSource);
    }
}