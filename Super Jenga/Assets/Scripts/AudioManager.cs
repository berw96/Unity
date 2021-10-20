using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class AudioManager : ScriptableObject
{
    private List<AudioClip> clips;

    public void PlayClip(AudioClip clip, GameObject go)
    {
        AudioSource.PlayClipAtPoint(clip, go.transform.position);
    }

    public void RegisterClip(AudioClip clip)
    {
        clips.Add(clip);
    }

    void RemoveClip(int index)
    {
        clips.RemoveAt(index);
    }

    public List<AudioClip> GetClips()
    {
        return this.clips;
    }
}
