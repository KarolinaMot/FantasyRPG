using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public CinemachineFreeLook freeLook;
    public Slider xValue;
    public Slider yValue;
    public Slider distance;
    private Vector3 startingDistance;


    void Awake(){
        startingDistance = new Vector3(freeLook.m_Orbits[0].m_Radius, freeLook.m_Orbits[1].m_Radius, freeLook.m_Orbits[2].m_Radius);
    }


    public void Disable(GameObject panel){
        panel.SetActive(false);
    }

    public void Enable(GameObject panel){
        panel.SetActive(true);
    }

    public void PauseGame(){
        Time.timeScale = 0;
    }

    public void ResumeGame(){
        Time.timeScale = 1;
    }

    public void XAxisSensitivity(){
        freeLook.m_XAxis.m_MaxSpeed = xValue.value;
    }

    public void YAxisSensitivity(){
        freeLook.m_YAxis.m_MaxSpeed = yValue.value;
    }

    public void DistanceFromPlayer(){
        freeLook.m_Orbits[0].m_Radius = startingDistance.x * distance.value;
        freeLook.m_Orbits[1].m_Radius = startingDistance.y * distance.value;
        freeLook.m_Orbits[2].m_Radius = startingDistance.y * distance.value;
    }
}
