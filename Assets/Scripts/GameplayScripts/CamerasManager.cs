using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamerasManager : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private GameObject gameplayCam;
    [SerializeField] private GameObject topDownCam;
    [SerializeField] private GameObject closeupCam;

    [SerializeField] private Vector3 closeCamOffset;
    // Start is called before the first frame update
    void Start()
    {
        gameplayCam.SetActive(false);
        topDownCam.SetActive(false);
        closeupCam.SetActive(false);

        topDownCam.SetActive(true);
    }

    public void SetCamPos(NewMap map)
    {
        gameplayCam.transform.position = new Vector3((map.size.y / 2) - .5f + map.size.y, 15, (map.size.x / 2) - .15f);
        topDownCam.transform.position = new Vector3(0, 15, (map.size.x / 2) - .15f);
    }
    
    public void CamTransitionToCloseup(Transform targetPos)
    {
        closeupCam.transform.position = targetPos.position + closeCamOffset;
        closeupCam.GetComponent<CinemachineVirtualCamera>().Follow = targetPos;
        closeupCam.GetComponent<CinemachineVirtualCamera>().LookAt = targetPos;
        closeupCam.SetActive(true);

        gameplayCam.SetActive(false);
        topDownCam.SetActive(false);
    }
    public void CamTransitionToGameplay()
    {
        gameplayCam.SetActive(true);
        
        topDownCam.SetActive(false);
        closeupCam.SetActive(false);
    }

    public void CamTansitionToTopdown()
    {
        topDownCam.SetActive(true);

        gameplayCam.SetActive(false);
        closeupCam.SetActive(false);
    }


}
