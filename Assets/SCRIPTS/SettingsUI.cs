using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public GameObject settings;
    public CinemachineFreeLook freeLook;
    public Slider xAxis;
    public Slider yAxis;

    private void Awake()
    {
        this.freeLook = this.GetComponent<CinemachineFreeLook>();
    }

    //public void SetCameraXSensitivity(float sensitivityX)
    //{
        
    //}

    //public void SetCameraYSensitivity(float sensitivityY)
    //{
        
    //}

    public void CloseSettings()
    {
        this.freeLook.m_XAxis.m_MaxSpeed = xAxis.value;
        this.freeLook.m_YAxis.m_MaxSpeed = yAxis.value;
        settings.SetActive(false);
        gameObject.SetActive(true);
        Time.timeScale = 1;


    }

    public void OpenSettings()
    {
        xAxis.value = this.freeLook.m_XAxis.m_MaxSpeed;
        yAxis.value = this.freeLook.m_YAxis.m_MaxSpeed;



        settings.SetActive(true);
        gameObject.SetActive(false);
        Time.timeScale = 0;
    }



}
