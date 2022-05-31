using System;
using UnityEngine;

namespace Sound
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
        private AudioSource _audioSource;
        [SerializeField] private AudioClip achievementUnlocked;
        [SerializeField] private AudioClip tutorialCorrect;
        [SerializeField] private AudioClip tutorialWrong;

        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(Sound clip)
        {
            switch (clip)
            {
                case Sound.AchievementUnlocked:
                    _audioSource.PlayOneShot(achievementUnlocked);
                    break;
                case Sound.TutorialCorrect:
                    _audioSource.PlayOneShot(tutorialCorrect);
                    break;
                case Sound.TutorialWrong:
                    _audioSource.PlayOneShot(tutorialWrong);
                    break;
            }
        }
    }
}