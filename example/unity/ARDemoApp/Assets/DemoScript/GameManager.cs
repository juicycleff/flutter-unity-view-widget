using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

/// <summary>
/// Game manager.
/// </summary>
[Serializable]
public class GameManager : Singleton<GameManager>
{

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Loads the game scene.
    /// </summary>
    /// <param name="message">Message.</param>
	public void LoadGameScene(string message)
    {
        Debug.Log(message);
        int sceneLevel;
        int.TryParse(message, out sceneLevel);
        SceneManager.LoadScene(true ? sceneLevel : 2);
    }
}
