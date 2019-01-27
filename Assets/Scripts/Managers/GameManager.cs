using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [SerializeField]
        public List<LevelDataSO> m_lvlList = new List<LevelDataSO>();

        [SerializeField] public PlayerDataSO m_playerData;
        [SerializeField] private GameObject   m_player;
        [SerializeField] private GameObject   m_warper;
        [SerializeField] private GameObject   m_targetArrowPrefab;
        public                   Action       OnNextLevel;

        public int m_lvlIndex = 0;

        private void Awake()
        {
            m_state = GameState.RUN;


            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);

                if (m_lvlIndex == 0)
                    m_playerData.Reset();
            }
            else
                Destroy(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene p_arg0, LoadSceneMode p_arg1)
        {
            m_player = GameObject.FindGameObjectWithTag("Player");
            m_warper = GameObject.FindGameObjectWithTag("Environment/Warper");

            if (m_warper != null)
                m_warper.SetActive(false);
        }


        public void OnUpdateStats()
        {
            CheckLife();
            if (m_state != GameState.DEAD)
            {
                CheckFragments();
            }
        }

        private void CheckFragments()
        {
            Debug.Log("TOTAL FRAGMENTS : " + m_playerData.m_fragments + " / " +
                      m_lvlList[m_lvlIndex].m_minimalFragments);
            if (m_lvlIndex < m_lvlList.Count && m_playerData.m_fragments >= m_lvlList[m_lvlIndex].m_minimalFragments)
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
                SceneManager.LoadScene(m_lvlIndex + 1);
                m_playerData.Reset();
            }
        }

        public void NextLevel()
        {
            if (OnNextLevel != null)
                OnNextLevel();

            m_lvlIndex++;
            int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextScene < SceneManager.sceneCountInBuildSettings - 1)
                SceneManager.LoadScene(nextScene);
        }
    }
}