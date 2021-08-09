using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public abstract State RunCurrentState();
}
