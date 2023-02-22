using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCtrl : MonoBehaviour
{
    [SerializeField] private float transitionTime;
    private Animator sceneAnim;

    private void Start()
    {
        sceneAnim = this.GetComponentInChildren<Animator>();
    }
    public void ChangeScene(string _scene)
    {
        StartCoroutine(SceneChangeTransition(_scene));
    }

    private IEnumerator SceneChangeTransition(string _scene)
    {
        sceneAnim.SetBool("On", true);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(_scene);
    }
}
