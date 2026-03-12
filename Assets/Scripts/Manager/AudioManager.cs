using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
  [SerializeField] private AudioSource source;

  [SerializeField] private AudioClip Bg;
  [SerializeField] private AudioClip flip;
  [SerializeField] private AudioClip match;
  [SerializeField] private AudioClip mismatch;
  [SerializeField] private AudioClip gameOver;
  public void PlayFlip()
  {
    source.PlayOneShot(flip);
  }

  public void PlayMatch()
  {
    source.PlayOneShot(match);
  }

  public void PlayMismatch()
  {
    source.PlayOneShot(mismatch);
  }

  public void PlayGameOver()
  {
    source.PlayOneShot(gameOver);
  }
}