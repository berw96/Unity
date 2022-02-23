/*-----------------------------------------------------------
    THE ROOM (2022)
    
    COPYRIGHT ELLIOT WALKER [3368 6408]
    and HAN XUE [SN: 3367 5676]
-----------------------------------------------------------*/

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Radio : MonoBehaviour
{
    [SerializeField] List<AudioClip> _music_tracks = new List<AudioClip>();
    [SerializeField] AudioSource _music_audio_source;
    [SerializeField] AudioSource _sfx_audio_source;

    private int _current_track_index = 0;

    private void Awake() {
        PlayMusic();
    }

    private void Update() {
        if (!_music_audio_source.isPlaying || _music_audio_source.mute) {
            _current_track_index = Random.Range(0, _music_tracks.Count);
            if (_current_track_index >= _music_tracks.Count)
                _current_track_index = 8;
            _music_audio_source.clip = _music_tracks[_current_track_index];
            PlayMusic();
            PlaySoundEffect();
        }
    }

    /// <summary>
    /// Plays a random audio clip from a list of AudioClip components.
    /// </summary>
    private void PlayMusic() {
        _music_audio_source.Play();
    }

    /// <summary>
    /// Toggles an electronic beep sound effect whenever the radio is
    /// activated by the player.
    /// </summary>
    private void PlaySoundEffect() {
        _sfx_audio_source.Play();
    }
}
