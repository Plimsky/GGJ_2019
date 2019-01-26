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
        [HideInInspector] public static GameManager instance;
        [HideInInspector] public        GameState   m_state;

        [SerializeField] private List<LevelDataSO> m_lvlList = new List<LevelDataSO>();
        [SerializeField] private PlayerDataSO      m_playerData;
        public                   Action            OnNextLevel;

        private int m_lvlIndex = 0;

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
        }

        public void OnUpdateStats()
        {
            CheckLife();
            CheckFragments();
//            CheckEnemies();
        }

        private void CheckFragments()
        {
            if (m_playerData.m_fragments >= m_lvlList[m_lvlIndex].m_minimalFragments)
                Debug.Log("CAN JUMP OMG");

            if (m_playerData.m_fragments == m_lvlList[m_lvlIndex].m_totalFragments)
                Debug.Log("YOU'VE GOT ALL FRAGMENTS");
        }

        private void CheckLife()
        {
            if (m_playerData.m_life <= 0)
            {
                m_state = GameState.DEAD;
                Debug.Log("YOU'RE DEAD");
            }
        }
    }
}