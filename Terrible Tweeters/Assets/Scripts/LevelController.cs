using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private Monster[]  _monsters;
    [SerializeField] private string _nextLevelName;

    private void OnEnable()
    {
        _monsters = FindObjectsOfType<Monster>();
    }

    // Update is called once per frame
    void Update()
    {
        if(MonstersAreAllDead())
            GoToNextLevel();
    }

    private void GoToNextLevel()
    {
        SceneManager.LoadScene(_nextLevelName);
    }

    private bool MonstersAreAllDead()
    {
        foreach (var monster in _monsters)
        {
            if(monster.gameObject.activeSelf)
                return false;
        }


        return true;
    }
}
