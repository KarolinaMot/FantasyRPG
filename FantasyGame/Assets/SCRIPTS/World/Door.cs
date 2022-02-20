using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    [SerializeField]
    private bool isRotatingDoor = true;
    [SerializeField]
    private float speed = 1f;
    [Header("Rotation Configs")]
    [SerializeField]
    private float rotationAmount = 90f;
    [SerializeField]
    private float forwardDirection = 0;

    private Vector3 startRotation;
    private Coroutine animationCoroutine;

    private void Awake()
    {
        startRotation = transform.rotation.eulerAngles;
    }

    public void Open(Vector3 userPosition)
    {
        if (!isOpen)
        {
            if(animationCoroutine != null)
                StopCoroutine(animationCoroutine);

            if (isRotatingDoor)
                animationCoroutine = StartCoroutine(DoRotstionOpen());

        }
    }

    private IEnumerator DoRotstionOpen()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        endRotation = Quaternion.Euler(new Vector3(transform.rotation.x, rotationAmount, transform.rotation.z));

        isOpen = true;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime;
        }
    }

    public void Close()
    {
        if (isOpen)
        {
           if(animationCoroutine != null)
               StopCoroutine(animationCoroutine);

            if (isRotatingDoor)
            {
                animationCoroutine = StartCoroutine(DoRotationClose());
            }
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRotation2 = transform.rotation;
        Quaternion endRotation2 = Quaternion.Euler(startRotation);

        isOpen = false;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation2, endRotation2, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }
}
