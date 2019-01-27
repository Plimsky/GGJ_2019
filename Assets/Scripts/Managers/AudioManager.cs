using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    [RequireComponent(typeof(AudioSource), typeof(AudioSource), typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [HideInInspector] public static AudioManager instance;
        [Header("AudioSources")]
        public AudioSource m_introAudioSource;
        public AudioSource m_loopAudioSource;
        public AudioSource m_OneShotAudioSource;

        [Header("AudioClips")]
        public AudioClip beamStartClip;
        public AudioClip beamLoopClip;
        public AudioClip collisionClip;

        private bool m_isBeamActivated = false;
        private bool m_isBeamPlaying = false;
        private bool m_isCollision = false;
        private bool m_isCollisionPlaying = false;

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
            if (m_isBeamActivated && !m_isBeamPlaying)
                StartBeam();
            else if (!m_isBeamActivated && m_isBeamPlaying)
            {
                ResetAudioSource(m_introAudioSource);
                ResetAudioSource(m_loopAudioSource);
                m_isBeamPlaying = false;
            }

            if (m_isCollision && !m_isCollisionPlaying)
            {
                PlaySoundOnce(collisionClip);
                m_isCollisionPlaying = true;
            }
            else if(m_isCollisionPlaying)
            {
                if (!m_OneShotAudioSource.isPlaying)
                    m_isCollisionPlaying = false;
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
    }
}