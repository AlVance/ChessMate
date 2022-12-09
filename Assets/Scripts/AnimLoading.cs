using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimLoading : MonoBehaviour
{
    public GameObject[] stones;

    public int[] newAnim;

    public float timeStep = .1f;

    public bool loopForward;

    private void Start()
    {
        for (int i = 0; i < stones.Length; i++)
        {
            stones[i].SetActive(false);
        }
        StartCoroutine(Spawning(true));
    }

    public IEnumerator Spawning(bool _active)
    {
        for (int i = 0; i < newAnim.Length; i++)
        {
            stones[newAnim[i]].SetActive(_active);
            yield return new WaitForSeconds(timeStep);
        }
        if (loopForward) StartCoroutine(Spawning(!_active));
        else StartCoroutine(DesSpawning(!_active));
    }

    IEnumerator DesSpawning(bool _active)
    {
        for (int i = newAnim.Length - 1; i >= 0; i--)
        {
            stones[newAnim[i]].SetActive(_active);
            yield return new WaitForSeconds(timeStep);
        }

        StartCoroutine(Spawning(!_active));
    }
}
