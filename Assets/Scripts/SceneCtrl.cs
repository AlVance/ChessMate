using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCtrl : MonoBehaviour
{
    public void ChangeScene(string _scene)
    {
        SceneManager.LoadScene(_scene);
    }
}
