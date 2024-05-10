using System;
using CodeBase.ApplicationBase.AudioManager;
using Hellmade.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.DialogsSystem.Settings
{
    public class SettingsDialog : DialogBase
    {
        [SerializeField] private SwitchToggle _switchToggleSound;
        [SerializeField] private SwitchToggle _switchToggleMusic;
        
        public override void Initialize()
        {
            bool isSoundOn = PlayerPrefs.GetInt(AudioManager.Sound) == 0;
            _switchToggleSound.Initialize(isSoundOn);
            _switchToggleSound.OnSwitchClickedEvent += OnSoundSwitchToggleClickedHandler;
            bool isMusicOn = PlayerPrefs.GetInt(AudioManager.Music) == 0;
            _switchToggleMusic.Initialize(isMusicOn);
            _switchToggleMusic.OnSwitchClickedEvent += OnMusicSwitchToggleClickedHandler;
            base.Initialize();
        }

        private void OnMusicSwitchToggleClickedHandler(bool value)
        {
            EazySoundManager.GlobalMusicVolume = value ? 0.7f : 0;
            PlayerPrefs.SetInt(AudioManager.Music, value ? 0 : 1);
        }
        
        private void OnSoundSwitchToggleClickedHandler(bool value)
        {
            EazySoundManager.GlobalSoundsVolume = value ? 0.7f : 0;
            PlayerPrefs.SetInt(AudioManager.Sound, value ? 0 : 1);
        }

        public override void CloseDialog()
        {
            _switchToggleSound.OnSwitchClickedEvent -= OnSoundSwitchToggleClickedHandler;
            _switchToggleMusic.OnSwitchClickedEvent -= OnMusicSwitchToggleClickedHandler;
            base.CloseDialog();
        }
    }
}