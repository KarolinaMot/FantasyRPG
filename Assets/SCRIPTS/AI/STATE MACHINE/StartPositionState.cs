using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPositionState : State
{
    public override State RunCurrentState()
    {
        return this;
    }
}
