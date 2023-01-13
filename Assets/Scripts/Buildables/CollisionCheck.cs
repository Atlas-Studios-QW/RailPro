using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    public int CollisionAmount = 1;
    private void OnTriggerExit(Collider other)
    {
        CollisionAmount--;
    }

    private void OnTriggerEnter(Collider other)
    {
        CollisionAmount++;
    }

    private void Start()
    {
        CollisionAmount = 0;
    }
}
