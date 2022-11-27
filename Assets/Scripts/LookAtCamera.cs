using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform cameraTr;
    // Start is called before the first frame update
    void Start()
    {
        cameraTr = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        cameraTr = Camera.main.transform;
        RotateToCam();
    }

    private void RotateToCam()
    {
        Vector3 look;
        cameraTr = Camera.main.transform;
        look.x = transform.position.x - cameraTr.position.x;
        look.y = transform.position.y - cameraTr.position.y;
        look.z = transform.position.z - cameraTr.position.z;
        transform.rotation = Quaternion.LookRotation(look);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.parent.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
