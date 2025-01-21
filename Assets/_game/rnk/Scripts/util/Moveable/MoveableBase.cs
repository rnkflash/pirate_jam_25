using _game.rnk.Scripts.battleSystem;
using UnityEngine;

public abstract class MoveableBase : ManagedBehaviour
{
    public Vector3 targetPosition;
    public bool IsCloseEnough()
    {
        return false;
    }
    public bool IsMoving()
    {
        return false;
    }
}