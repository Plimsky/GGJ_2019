using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int m_lvlIndex = 0;
    [SerializeField] private List<LevelDataSO> m_lvlList = new List<LevelDataSO>();

    public Action OnNextLevel;


}