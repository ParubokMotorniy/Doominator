using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayerY : MonoBehaviour
{
    [SerializeField] private GameObject player;
    void Update()
    {
        transform.position = new Vector3(0, player.transform.position.y, player.transform.position.z);
    }
}
