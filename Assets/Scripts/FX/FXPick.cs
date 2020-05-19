using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: make IPooledObject
public class FXPick : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}