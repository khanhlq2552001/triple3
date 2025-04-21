using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.MainGame
{
    public class AudioController : MonoBehaviour
    {

        public static float DefaultBgmVolume = 1;
        public static float DefaultSfxVolume = 1;
        public static float DefaultVoiceVolume = 1;
        public static float DefaultBgmVolumeScale = 1f;
        public static float DefaultSfxVolumeScale = 1;
        public static float DefaultVoiceVolumeScale = 1;

        private static float bgmVolume = -1;
        private static float sfxVolume = -1;
        private static float voiceVolume = -1;

        private static float bgmVolumeScale = 1;

        public static string PREF_SFX_VOLUME = "SFX_VOLUME";
        public static string PREF_BGM_VOLUME = "BGM_VOLUME";

        public const string upgrade = "upgrade";
        public const string upgrade_sucess = "sound_nangcapxongphong";
        public const string level_Up = "level_up4";

        public static float BgmVolume
        {
            get
            {
                if (bgmVolume == -1)
                {
                    bgmVolume = PlayerPrefs.GetFloat("BGM_VOLUME", DefaultBgmVolume);
                }

                return bgmVolume;
            }

            set
            {
                bgmVolume = value;
                instance.m_BgmSource.volume = bgmVolume * DefaultBgmVolumeScale * bgmVolumeScale;
            }
        }

        public static float SfxVolume
        {
            get
            {
                if (sfxVolume == -1)
                {
                    sfxVolume = PlayerPrefs.GetFloat("SFX_VOLUME", DefaultSfxVolume);
                }

                return sfxVolume;
            }

            set
            {
                sfxVolume = value;
            }
        }

        public static float VoiceVolume
        {
            get
            {
                if (voiceVolume == -1)
                {
                    voiceVolume = PlayerPrefs.GetFloat("VOICE_VOLUME", DefaultVoiceVolume);
                }

                return voiceVolume;
            }

            set
            {
                voiceVolume = value;
            }
        }

        public void ChangeBGSound(bool state)
        {
            if (state)
            {
                BgmVolume = 1f;
            }
            else
            {
                BgmVolume = 0;
            }
        }

        public void ChangeSFXSound(bool state)
        {
            if (state)
            {
                sfxVolume = 1f;
            }
            else
            {
                sfxVolume = 0;
            }
        }

        public static void Save()
        {
            PlayerPrefs.SetFloat("BGM_VOLUME", BgmVolume);
            PlayerPrefs.SetFloat("SFX_VOLUME", SfxVolume);
            PlayerPrefs.SetFloat("VOICE_VOLUME", VoiceVolume);
            PlayerPrefs.Save();
        }

        [SerializeField] private string m_BgmPath = "Sounds/Bgm";
        [SerializeField] private string m_SfxPath = "Sounds/Sfx";
        [SerializeField] private string m_VoicePath = "Sounds/Voice";
        [SerializeField] private string m_ButtonTapSfx = "sfx_button_tap";
        [SerializeField] private AudioSource m_BgmSource;
        [SerializeField] private AudioSource m_SfxSource;
        [SerializeField] private AudioSource m_VoiceSource;




        public static AudioController instance { get; protected set; }

        private Dictionary<string, float> soundFxLimit = new Dictionary<string, float>();

        private Dictionary<string, AudioClip> m_AudioDict = new Dictionary<string, AudioClip>();
        private bool m_PauseBgm;

        public string sfxPath
        {
            get { return m_SfxPath; }
        }

        public string bgmPath
        {
            get { return m_BgmPath; }
        }

        public string voicePath
        {
            get { return m_VoicePath; }
        }

        public AudioSource voiceSource
        {
            get { return m_VoiceSource; }
        }

        public void PlayButtonTapSfx()
        {
            PlaySfx(m_ButtonTapSfx, 1);
        }

        /// <summary>
        /// Play a random sfx from the array
        /// </summary>
        public void PlaySfx(string[] array, float volumeScale = 1f)
        {
            if (array.Length > 0)
                PlaySfx(array[UnityEngine.Random.Range(0, array.Length)], volumeScale, true);
        }

        public void PlaySfx(AudioClip[] array, float volumeScale = 1f)
        {
            if (array.Length > 0)
                PlaySfx(array[UnityEngine.Random.Range(0, array.Length)], volumeScale);
        }

        public void PlaySfx(AudioClip audioClip, float volumeScale = 1f)
        {
            m_SfxSource.PlayOneShot(audioClip, SfxVolume * DefaultSfxVolumeScale * volumeScale);
        }

        public void PlaySfx(string audioName, float volumeScale = 1f, bool oneShot = true, bool isLoop = false, float delay = 0, Action actionMoveCam = null)
        {
            if (!CanPlaySound(audioName)) return;

            StartCoroutine(CoroutinePlaySFX(audioName, volumeScale, oneShot, isLoop, delay, actionMoveCam));
        }

        private bool CanPlaySound(string audioName)
        {
            if (soundFxLimit.ContainsKey(audioName))
            {
                if (Time.time - soundFxLimit[audioName] >= .5f)
                {
                    soundFxLimit[audioName] = Time.time;
                    return true;
                }
                return false;
            }
            else soundFxLimit.Add(audioName, Time.time);
            return true;
        }

        private bool isPlayingUpgrade;

        private IEnumerator CoroutinePlaySFX(string audioName, float volumeScale = 1f, bool oneShot = true, bool isLoop = false, float delay = 0, Action actionMoveCam = null)
        {
            if (isPlayingUpgrade && audioName == upgrade)
            {
                actionMoveCam?.Invoke();
            }
            else
            {
                if (audioName == upgrade)
                {
                    isPlayingUpgrade = true;
                    actionMoveCam?.Invoke();
                }
                yield return new WaitForSeconds(delay);
                CacheClip(m_SfxPath, audioName);

                m_SfxSource.volume = SfxVolume * DefaultSfxVolumeScale * volumeScale;
                if (oneShot)
                {
                    m_SfxSource.PlayOneShot(m_AudioDict[audioName], m_SfxSource.volume);
                }
                if (isLoop)
                {
                    m_SfxSource.loop = isLoop;
                    m_SfxSource.clip = m_AudioDict[audioName];
                    m_SfxSource.Play();
                }
                if (audioName == upgrade)
                {
                    isPlayingUpgrade = false;
                }
            }
        }

        public void StopSfx()
        {
            m_SfxSource.Stop();
        }



        public float GetLength(string audioName)
        {
            return (m_AudioDict.ContainsKey(audioName) ? m_AudioDict[audioName].length : 0);
        }

        public float PlayVoice(AudioClip audioClip, float volumeScale = 1f)
        {
            m_VoiceSource.volume = VoiceVolume * DefaultVoiceVolume * volumeScale;
            m_VoiceSource.PlayOneShot(audioClip, m_VoiceSource.volume);
            return audioClip.length;
        }

        public void PlayVoice(string audioName, float volumeScale = 1f, bool playOneShot = false)
        {
            CacheClip(m_VoicePath, audioName);

            m_VoiceSource.volume = VoiceVolume * DefaultVoiceVolume * volumeScale;
            if (playOneShot)
            {
                m_VoiceSource.PlayOneShot(m_AudioDict[audioName], m_VoiceSource.volume);
            }
            else
            {
                m_VoiceSource.clip = m_AudioDict[audioName];
                m_VoiceSource.Play();
            }
        }

        public void SetVoicePitch(float pitch)
        {
            m_VoiceSource.pitch = pitch;
        }

        public void PlayBgm(string audioName, float volumeScale = 1f)
        {
            bgmVolumeScale = volumeScale;
            m_BgmSource.volume = BgmVolume * DefaultBgmVolumeScale * bgmVolumeScale;

            bool currentClipNull = (m_BgmSource.clip == null);
            bool sameCurrentClip = (!currentClipNull && m_BgmSource.clip.name == audioName);

            if (sameCurrentClip)
            {
                if (!m_BgmSource.isPlaying)
                {
                    m_BgmSource.Play();
                }
            }
            else
            {
                if (!currentClipNull)
                {
                    StopBgm(true);
                }
                m_BgmSource.clip = Resources.Load<AudioClip>(System.IO.Path.Combine(m_BgmPath, audioName));
                m_BgmSource.Play();
            }
        }

        public void StopBgm(bool clearClip = false)
        {
            if (m_BgmSource.isPlaying)
            {
                m_BgmSource.Stop();
            }

            if (clearClip && m_BgmSource.clip != null)
            {
                Resources.UnloadAsset(m_BgmSource.clip);
                m_BgmSource.clip = null;
            }
        }

        public bool pauseBgm
        {
            set
            {
                m_PauseBgm = value;

                if (m_PauseBgm)
                {
                    m_BgmSource.Pause();
                }
                else
                {
                    m_BgmSource.UnPause();
                }
            }
        }

        public void CacheClip(AudioClip clip, string audioName)
        {
            if (!m_AudioDict.ContainsKey(audioName))
            {
                m_AudioDict.Add(audioName, clip);
            }
        }

        public void CacheClip(string path, string audioName)
        {
            /*
            if (m_AudioDict.Count > 10) //clear cache if too large
            {
                foreach (var item in m_AudioDict)
                {
                    Resources.UnloadAsset(item.Value);
                }
                m_AudioDict.Clear();
            }
            */
            if (!m_AudioDict.ContainsKey(audioName))
            {
                AudioClip clip = Resources.Load<AudioClip>(System.IO.Path.Combine(path, audioName));
                if (clip == null) { Debug.LogError("audioclip not found " + System.IO.Path.Combine(path, audioName)); }
                m_AudioDict.Add(audioName, clip);
            }
        }

        public void CacheSfx(string[] array)
        {
            foreach (var item in array)
            {
                CacheSfx(item);
            }
        }

        public void CacheSfx(string audioName)
        {
            CacheClip(m_SfxPath, audioName);
        }

        public void ClearCacheClip(string audioName, bool unloadClip)
        {
            if (m_AudioDict.ContainsKey(audioName))
            {
                if (unloadClip)
                {
                    Resources.UnloadAsset(m_AudioDict[audioName]);
                }
                m_AudioDict.Remove(audioName);
            }
        }

        public void ClearCacheClip()
        {
            foreach (var item in m_AudioDict)
            {
                Resources.UnloadAsset(item.Value);
            }
            m_AudioDict.Clear();
        }

        public void Fadeout()
        {
        }

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);

                instance = this;
            }
        }

        public void CheckSound()
        {
            if (Manager.Instance.UserData.soundOn)
            {
                ChangeSFXSound(true);
            }
            else
            {
                ChangeSFXSound(false);
            }
        }

        public void CheckMusic()
        {
            if (Manager.Instance.UserData.musicOn)
            {
                ChangeBGSound(true);
            }
            else
            {
                ChangeBGSound(false);
            }
        }

        private void Start()
        {
            CheckSound();

            CheckMusic();
            PlaySoundBGHome();
        }

        public void PlaySoundBGHome()
        {
            PlayBgm("game_bgm_01", 0.2f);
        }

        public void PlaySoundBGGame()
        {
            PlayBgm("bgmInGame", 0.2f);
        }

        public void PlaySoundButton()
        {
            PlaySfx("click_01", 0.9f);
        }

        public void PlaySoundBuy()
        {
            PlaySfx("buy_01", 0.9f);
        }

        public void PlaySoundAnVat()
        {
            int ran = UnityEngine.Random.Range(0,2);

            if(ran == 0)
            {
                PlaySfx("Right 4", 0.9f);
            }
            else
            {
                PlaySfx("Right 5", 0.9f);
            }
        }

        public void PlaySoundScrack()
        {
            PlaySfx("Explosion 3", 1f);
        }

        public void PlaySoundIce()
        {
            PlaySfx("ice", 1f);
        }

        public void PlaySoundMagic()
        {
            PlaySfx("magicSpell", 0.8f);
        }

        public void PlaySoundWin()
        {
            PlaySfx("level_completed_04", 0.7f);
        }

        public void PlaySoundLose()
        {
            PlaySfx("level_lose_03", 0.7f);
        }

        public void PlaySoundHit()
        {
            PlaySfx("Hit 1", 0.3f);
        }

        public void PlaySoundChooseObject()
        {
            Debug.Log("ok"); 
            PlaySfx("move_03", 1f);
        }

        public void PlaySoundCoin()
        {
            PlaySfx("coin-dropped-81172", 1f);
        }

        public void PlaySoundTime()
        {
            PlaySfx("Clock 1 (mp3cut.net)", 0.6f);
        }

        public void PlaySoundMatch()
        {
            PlaySfx("match_01", 1);
        }
        public void PlaySoundBoom()
        {
            PlaySfx("bomb_02", 1.5f);
        }
    }
}
