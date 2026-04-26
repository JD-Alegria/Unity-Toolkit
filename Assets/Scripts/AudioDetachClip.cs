using UnityEngine;

public class AudioDetachClip
{
    void PlayDetachedClip(AudioClip clip)
    {
        if (clip == null) return;
        
        //create a new GO
        GameObject tempAudioSourceGameOjbect = new GameObject("Player Death AudioSource");
        tempAudioSourceGameOjbect.transform.position = transform.position;
        
        //add the audio source component //set it up
        AudioSource tempAudioSource = tempAudioSourceGameOjbect.AddComponent<AudioSource>();
        tempAudioSource.spatialBlend = audioSource != null ? audioSource.spatialBlend : 0f;
        tempAudioSource.volume = audioSource != null ? audioSource.volume : 1f;
        tempAudioSource.minDistance = audioSource != null ? audioSource.minDistance : 1f;
        tempAudioSource.maxDistance = audioSource != null ? audioSource.maxDistance : 20f;
        tempAudioSource.rolloffMode = audioSource != null ? audioSource.rolloffMode : AudioRolloffMode.Logarithmic;

        //play the clip
        tempAudioSource.clip = clip;
        tempAudioSource.Play();
        
        //destroy the temp GO
        Destroy(tempAudioSourceGameOjbect, clip.length);
    }
}