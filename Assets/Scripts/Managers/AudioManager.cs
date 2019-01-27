using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    [RequireComponent(typeof(AudioSource), typeof(AudioSource), typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [HideInInspector] public static AudioManager instance;
        [Header("FX AudioSources")]
        public AudioSource m_introAudioSource;
        public AudioSource m_loopAudioSource;
        public AudioSource m_OneShotAudioSource;
        public AudioSource m_EngineAudioSource;
        [Header("Music AudioSources")]
        public AudioSource m_AmbientMusicAudioSource;

        [Header("FX AudioClips")]
        public AudioClip beamStartClip;
        public AudioClip beamLoopClip;
        public AudioClip propulsionSound;
        public AudioClip powerUpClip;
        public AudioClip[] collisionClips;
        public AudioClip explosionClip;
        [Header("Music AudioClips")]
        public AudioClip AmbientMusicClip;

        private bool m_isBeamActivated = false;
        private bool m_isBeamPlaying = false;
        private bool m_isCollision = false;
        private bool m_isCollisionPlaying = false;
        private bool m_isPropulsion = false;
        private bool m_isPropulsionPlaying = false;
        private bool m_isPowerUp = false;
        private bool m_isPowerUpPlaying = false;
        private bool m_isExplosionPlaying = false;
        private bool m_isExplosion = false;
        private bool m_isMusicPlaying = false;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Update()
        {
            if (m_EngineAudioSource == null || m_introAudioSource == null || m_loopAudioSource == null || m_OneShotAudioSource == null)
            {
                return;
            }

            UpdateBeam();
            UpdateCollision();
            UpdateExplosion();
            UpdatePowerUp();
            UpdatePropulsion();
            UpdateMusic();
            
        }

        private void UpdateBeam()
        {
            if (m_isBeamActivated && !m_isBeamPlaying)
                StartBeam();
            else if (!m_isBeamActivated && m_isBeamPlaying)
            {
                ResetAudioSource(m_introAudioSource);
                ResetAudioSource(m_loopAudioSource);
                m_isBeamPlaying = false;
            }
        }

        private void UpdateCollision()
        {
            if (!m_isExplosion && m_isCollision && !m_isCollisionPlaying)
            {
                PlaySoundOnce(collisionClips[Random.Range(0, 6)]);
                m_isCollisionPlaying = true;
            }
            else if (m_isCollisionPlaying && !m_isCollision)
                m_isCollisionPlaying = false;
        }


        private void UpdatePropulsion()
        {
            if (m_isPropulsion && !m_isPropulsionPlaying)
            {
                Propulse();
            }
            else if (m_isPropulsionPlaying && !m_isPropulsion)
            {
                ResetAudioSource(m_EngineAudioSource);
                m_isPropulsionPlaying = false;
            }
        }


        private void UpdatePowerUp()
        {
            if (m_isPowerUp && !m_isPowerUpPlaying)
            {
                PlaySoundOnce(powerUpClip);
                m_isPowerUpPlaying = true;
            }
            else if (m_isPowerUpPlaying)
            {
                m_isPowerUp = false;
                m_isPowerUpPlaying = false;
            }
        }


        private void UpdateExplosion()
        {
            if (m_isExplosion && !m_isExplosionPlaying)
            {
                m_isExplosionPlaying = true;
                PlaySoundOnce(explosionClip);
            }
            else if (m_isExplosionPlaying && !m_OneShotAudioSource.isPlaying)
            {
                m_isExplosionPlaying = false;
                m_isExplosion = false;
            }
        }

        private void UpdateMusic()
        {
            if (m_isMusicPlaying == false)
            {
                m_isMusicPlaying = true;
                m_AmbientMusicAudioSource.loop = true;
                m_AmbientMusicAudioSource   .clip = AmbientMusicClip;
                m_AmbientMusicAudioSource.Play();
            }
        }

        private void ResetAudioSource(AudioSource p_as)
        {
            p_as.Stop();
            p_as.clip = null;
            p_as.loop = false;
        }

        public void PlaySoundOnce(AudioClip p_audioClip)
        {
            m_OneShotAudioSource.PlayOneShot(p_audioClip);
        }

        public void Propulse()
        {
            m_EngineAudioSource.clip = propulsionSound;
            m_EngineAudioSource.Play();
            m_EngineAudioSource.loop = true;
            m_isPropulsionPlaying = true;
        }

        public void StartBeam()
        {
            m_isBeamPlaying = true;
            m_introAudioSource.clip = beamStartClip;
            m_loopAudioSource.clip = beamLoopClip;

            double introDuration = (double)m_introAudioSource.clip.samples / m_introAudioSource.clip.frequency;
            double startTime = AudioSettings.dspTime;

            m_introAudioSource.PlayScheduled(startTime);

            m_loopAudioSource.PlayScheduled(startTime + introDuration);
            m_loopAudioSource.loop = true;
        }

        public void SetIsBeamActivated(bool p_iba)
        {
            m_isBeamActivated = p_iba;
        }

        public void SetIsCollision(bool p_ic)
        {
            m_isCollision = p_ic;
        }

        public void SetIsPropulsion(bool p_ip)
        {
            m_isPropulsion = p_ip;
        }

        public void SetIsPowerUp(bool p_ipu)
        {
            m_isPowerUp = p_ipu;
        }

        public void SetIsExplosion(bool p_ie)
        {
            m_isExplosion = p_ie;
        }

        public void SetVolume (AudioSource p_audioSource, float p_volume)
        {
            p_audioSource.volume = p_volume;
        }
    }
}