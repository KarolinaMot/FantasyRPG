using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject openDoorText;
    StarterAssetsInputs inputs;
    public GameObject doorHinge;
    public GameObject front;
    public GameObject back;
    Quaternion openRotation1;
    Quaternion openRotation2;
    Quaternion closeRotation;
    public Vector3 startPosition;
    float movementTime = 0;
    public bool inTrigger = false;
    bool isInFront;
    public bool doorIsOpen =  false;
    public bool doorIsClosing = false;
    public bool slidingDoor = false;
    public float slideAmmount;

    void Start(){
        closeRotation = doorHinge.transform.rotation;
        doorHinge.transform.Rotate(0,-90,0);
        openRotation1 = doorHinge.transform.rotation;
        doorHinge.transform.Rotate(0, 180, 0);
        openRotation2 = doorHinge.transform.rotation;
        doorHinge.transform.Rotate(0,-90,0);
        startPosition = doorHinge.transform.position;

        openDoorText = GameObject.Find("OpenDoorText");
        inputs = GameObject.FindObjectOfType<StarterAssetsInputs>();

    }
    void Update(){
        if(!inTrigger){
            openDoorText.SetActive(false);
        }
        else if(inputs.interact && !doorIsOpen && !doorIsClosing){
            if(!slidingDoor){
                if(isInFront){
                    RotateBack();
                }
                else{
                    RotateFront();
                }
            }
            else{
                doorHinge.transform.position = Vector3.Lerp(startPosition, new Vector3(startPosition.x + slideAmmount, startPosition.y, startPosition.z), movementTime);
                movementTime += Time.deltaTime;
                if(doorHinge.transform.position.x >= startPosition.x + slideAmmount - 0.1f){
                    doorIsOpen = true;
                    inputs.interact = false;
                    movementTime = 0;
                }
            }
        }
        
        if(doorIsClosing){
            if(!slidingDoor){
                if(isInFront){
                    CloseDoorBack();
                }
                else{
                    CloseDoorFront();
                }
            }
            else{
                doorHinge.transform.position = Vector3.Lerp(new Vector3(startPosition.x + slideAmmount, startPosition.y, startPosition.z), startPosition, movementTime);
                movementTime += Time.deltaTime;
                if(doorHinge.transform.position.x == startPosition.x){
                    doorIsOpen = false;
                    doorIsClosing = false;
                    movementTime = 0;
                }
            }
        }

    }

    void RotateBack(){
            doorHinge.transform.rotation = Quaternion.Slerp(closeRotation, openRotation1, movementTime);
            movementTime += Time.deltaTime;
            if(doorHinge.transform.rotation == openRotation1){
                doorIsOpen = true;
                inputs.interact = false;
                movementTime = 0;
            }
    }

    void RotateFront(){
            doorHinge.transform.rotation = Quaternion.Slerp(closeRotation, openRotation2, movementTime);
            movementTime += Time.deltaTime;
            if(doorHinge.transform.rotation == openRotation2){
                doorIsOpen = true;
                inputs.interact = false;
                movementTime = 0;
            }
    }

    void CloseDoorBack(){
            doorHinge.transform.rotation = Quaternion.Slerp(openRotation1, closeRotation, movementTime);
            movementTime += Time.deltaTime;
            if(doorHinge.transform.rotation == closeRotation){
                doorIsClosing = false;
                doorIsOpen = false;
                movementTime = 0;
            } 
   }

   void CloseDoorFront(){
        doorHinge.transform.rotation = Quaternion.Slerp(openRotation2, closeRotation, movementTime);
        movementTime += Time.deltaTime;
        if(doorHinge.transform.rotation == closeRotation){
            doorIsClosing = false;
            doorIsOpen = false;
            movementTime = 0;
        } 
   }


    private void OnTriggerEnter(Collider other)
    {
        openDoorText.SetActive(true);
        inTrigger = true;
        if(!doorIsOpen && !slidingDoor)
        isInFront = Vector3.Distance(other.transform.position, front.transform.position) <= Vector3.Distance(other.transform.position, back.transform.position );
        inputs.interactable = true;
    }

    private void OnTriggerExit(Collider other){
        openDoorText.SetActive(false);
        inTrigger = false;
        inputs.interactable = false;

        if(doorIsOpen)
            doorIsClosing = true;
    }
}
