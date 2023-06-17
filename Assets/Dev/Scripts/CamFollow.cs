using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform Player;
    public Vector3 Offset;


    void Update()
    {
        if (Player != null)
            transform.position = Player.position + Offset;
    }
}
