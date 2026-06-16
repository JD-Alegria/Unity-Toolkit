using UnityEngine;

namespace Jaleg.Toolkit;

public static class AudioDetachClip
{
    public static AudioSource Play(
        AudioClip clip,
        Transform soundLocation,
        float spatialBlend = 0f,
        float volume = 1f,
        float minDistance = 1f,
        float maxDistance = 20f,
        AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic)
    {
        if (clip == null || soundLocation == null) return null;
        
        GameObject tempAudioSourceGameObject = new GameObject("Temporary AudioSource");
        tempAudioSourceGameObject.transform.position = soundLocation.position;
        
        AudioSource tempAudioSource = tempAudioSourceGameObject.AddComponent<AudioSource>();
        tempAudioSource.spatialBlend = spatialBlend;
        tempAudioSource.volume = volume;
        tempAudioSource.minDistance = minDistance;
        tempAudioSource.maxDistance = maxDistance;
        tempAudioSource.rolloffMode = rolloffMode;

        tempAudioSource.clip = clip;
        tempAudioSource.Play();
        
        GameObject.Destroy(tempAudioSourceGameObject, clip.length);
        return tempAudioSource;
    }
}
