using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public string currentMap;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _instance = this;
    }

    public void ReloadScene()
    {
        ChangeScene(SceneManager.GetActiveScene().name);
    }
    public void ChangeScene(string _scene)
    {
        SceneManager.LoadScene(_scene);
    }
    public void ChangeSceneDelay(string _name, float _delay)
    {
        StartCoroutine(ChangeDelay(_name, _delay));
    }
    public IEnumerator ChangeDelay(string _name, float _delay)
    {
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(_name);
    }

    public static GameManager Instance
    {
        get
        {
            if (_instance is null) Debug.LogError("Game Manager is NULL");
            return _instance;
        }
    }
}
