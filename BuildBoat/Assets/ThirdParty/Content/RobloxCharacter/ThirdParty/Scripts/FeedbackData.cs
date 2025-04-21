using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FeedbackData
{
  [SerializeField] private List<AudioClip> _audioClips;
  [SerializeField] private ParticleSystem[] _particleSystems;
  
  public void Play(AudioSource audioSource, bool isOptimize = true)
  {
    if (_audioClips.Count > 0)
    {
      audioSource.clip = _audioClips.GetRandomRepeatElement();
      audioSource.Play();
    }

    if (DeviceData.IsMobile && isOptimize)
    {
      return;
    }
    
    foreach (var particle in _particleSystems)
    {
      particle.Play();
    }
  }
}