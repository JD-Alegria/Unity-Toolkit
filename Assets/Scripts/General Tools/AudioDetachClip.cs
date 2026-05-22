using UnityEngine;

public class AudioDetachClip
{
    void PlayDetachedClip(AudioClip clip, Transform soundLocation, float spatialBlend = 0f, float volume = 1f, float minDistance = 1f, float maxDistance = 20f, AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic)
    {
        if (clip == null) return;
        
        //create a new GO
        GameObject tempAudioSourceGameObject = new GameObject("Temporary AudioSource");
        tempAudioSourceGameObject.transform.position = soundLocation.position;
        
        //add the audio source component //set it up
        AudioSource tempAudioSource = tempAudioSourceGameObject.AddComponent<AudioSource>();
        tempAudioSource.spatialBlend = spatialBlend;
        tempAudioSource.volume = volume;
        tempAudioSource.minDistance = minDistance;
        tempAudioSource.maxDistance = maxDistance;
        tempAudioSource.rolloffMode = rolloffMode;

        //play the clip
        tempAudioSource.clip = clip;
        tempAudioSource.Play();
        
        //destroy the temp GO
        GameObject.Destroy(tempAudioSourceGameObject, clip.length);
    }
}