using System;
using System.Collections.Generic;
using Hellmade.Sound;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.ApplicationBase.AudioManager
{
    public class AudioManager : MonoBehaviour
    {

        public static string Sound = nameof(Sound);
        public static string Music = nameof(Music);
        
        private static AudioManager _audioManager;

        public static AudioManager Inst => _audioManager;

        [SerializeField] private AudioClip mainMetaThemeClip;
        [SerializeField] private AudioClip mainCoreThemeClip;
        [SerializeField] private AudioClip looseClip;
        [SerializeField] private AudioClip winClip;
        [SerializeField] private AudioClip buttonClip;
        [SerializeField] private List<AudioClip> blockClips;

        private Audio mainMeta;
        private Audio mainCore;
        private Audio loose;
        private Audio win;
        private Audio button;
        private List<Audio> blocks;

        private void Awake()
        {
            if (_audioManager == null)
                _audioManager = this;
            
            //PrepareAllSounds();
            bool isSoundOn = PlayerPrefs.GetInt(Sound) == 0;

            if (isSoundOn)
                EazySoundManager.GlobalSoundsVolume = 0.7f;
            else EazySoundManager.GlobalSoundsVolume = 0;

            bool isMusicOn = PlayerPrefs.GetInt(Music) == 0;

            if (isMusicOn)
                EazySoundManager.GlobalMusicVolume = 0.7f;
            else EazySoundManager.GlobalMusicVolume = 0;
            DontDestroyOnLoad(this);
        }

        private void PrepareAllSounds()
        {
            EazySoundManager.IgnoreDuplicateSounds = false;
            int mainMetaID = EazySoundManager.PrepareMusic(mainMetaThemeClip, 0.7f, true, false);
            mainMeta = EazySoundManager.GetMusicAudio(mainMetaID);
            
            int mainCoreID = EazySoundManager.PrepareMusic(mainCoreThemeClip, 0.7f, true, false);
            mainCore = EazySoundManager.GetMusicAudio(mainCoreID);

            int looseID = EazySoundManager.PrepareSound(looseClip, 0.8f);
            loose = EazySoundManager.GetSoundAudio(looseID);
            
            int winID = EazySoundManager.PrepareSound(winClip, 0.8f);
            win = EazySoundManager.GetSoundAudio(winID);

            int buttonID = EazySoundManager.PrepareUISound(buttonClip, 1);
            button = EazySoundManager.GetUISoundAudio(buttonID);

            blocks = new List<Audio>(blockClips.Count);
            for (int i = 0; i < blockClips.Count; i++)
            {
                int blockID = EazySoundManager.PrepareSound(blockClips[i], 1);
                blocks.Add(EazySoundManager.GetSoundAudio(blockID));
            }
        }

        public void PlayMainMetaTheme()
        {
            EazySoundManager.PlayMusic(mainMetaThemeClip, 0.7f, true, true);
        }

        public void PlayMainCoreTheme()
        {
            EazySoundManager.PlayMusic(mainCoreThemeClip, 0.7f, true, true);
        }

        public void PlayLoose() => EazySoundManager.PlaySound(looseClip, 0.7f);
        public void PlayWin() => EazySoundManager.PlaySound(winClip, 0.7f);

        public void PlayButton() => EazySoundManager.PlaySound(buttonClip, 0.7f);

        public void PlayBlocksTaps()
        {
            int randInd = Random.Range(0, blocks.Count);
            EazySoundManager.PlaySound(blockClips[randInd], 0.7f);
        }
    }
}