using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCtrl : MonoBehaviour
{
    [SerializeField] private float transitionTime;
    [SerializeField]private Animator sceneAnim;

    public void ChangeScene(string _scene)
    {
        StartCoroutine(SceneChangeTransition(_scene));
    }
    public void ChangeSceneNoTransition(string _scene)
    {
        SceneManager.LoadScene(_scene);
    }

    private IEnumerator SceneChangeTransition(string _scene)
    {
        if (!sceneAnim.gameObject.activeInHierarchy) sceneAnim.gameObject.SetActive(true);
        sceneAnim.SetBool("On", true);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(_scene);
    }
}
