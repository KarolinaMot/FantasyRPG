using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCharacterCollision : MonoBehaviour
{
    // Start is called before the first frame update
    public CapsuleCollider characterCollider;
    public CapsuleCollider characterBlockerCollider;

    void Start()
    {
        Physics.IgnoreCollision(characterCollider, characterBlockerCollider, true);
    }

}
