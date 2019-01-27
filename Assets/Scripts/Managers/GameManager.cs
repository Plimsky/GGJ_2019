using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public enum GameState
    {
        NONE,
        PAUSE,
        RUN,
        DEAD,
        QUIT
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [HideInInspector] public GameState m_state;

        [Header("Level Management")]
        [SerializeField] private List<LevelDataSO> m_lvlList = new List<LevelDataSO>();
        [SerializeField] private PlayerDataSO      m_playerData;
        [SerializeField] private GameObject        m_player;
        [SerializeField] private GameObject        m_warper;
        [SerializeField] private GameObject        m_targetArrowPrefab;
        public                   Action            OnNextLevel;

        private int m_lvlIndex = 0;

        private void Awake()
        {
            m_state  = GameState.RUN;

            if (m_warper != null)
                m_warper.SetActive(false);

            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);

                if (m_lvlIndex == 0)
                    m_playerData.Reset();
            }
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            m_player = GameObject.FindGameObjectWithTag("Player");
            m_warper = GameObject.FindGameObjectWithTag("Environment/Warper");
        }

        public void OnUpdateStats()
        {
            CheckLife();
            if (m_state != GameState.DEAD)
            {
                CheckFragments();
//                CheckEnemies();
            }
        }

        private void CheckFragments()
        {
            if (m_playerData.m_fragments >= m_lvlList[m_lvlIndex].m_minimalFragments)
            {
                m_warper.SetActive(true);
                GameObject refTargetArrow =
                    Instantiate(m_targetArrowPrefab, m_player.transform.position, Quaternion.identity);
                refTargetArrow.GetComponent<targetArrow>().m_player = m_player.transform;
                refTargetArrow.GetComponent<targetArrow>().m_target = m_warper.transform;
            }

            if (m_playerData.m_fragments == m_lvlList[m_lvlIndex].m_totalFragments)
                Debug.Log("YOU'VE GOT ALL FRAGMENTS");
        }

        private void CheckLife()
        {
            if (m_playerData.m_life <= 0)
            {
                m_state = GameState.DEAD;
            }
        }

        public void NextLevel()
        {
            Debug.Log("Loading Next Level");
            if (OnNextLevel != null)
                OnNextLevel();

            m_lvlIndex++;
        }
    }
}