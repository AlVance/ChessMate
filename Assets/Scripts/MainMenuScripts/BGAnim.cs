using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAnim : MonoBehaviour
{
    [SerializeField] private Material bgMat;
    [SerializeField] private float bgSpeed;

    // Update is called once per frame
    void Update()
    {
        bgMat.mainTextureOffset += new Vector2(0, bgSpeed * Time.deltaTime);
    }
}
