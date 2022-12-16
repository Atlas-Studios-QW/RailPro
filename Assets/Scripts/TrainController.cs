using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    private void OnTriggerEnter(Collider FrontCollider)
    {
        print(FrontCollider.gameObject.name);
    }
}
