using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Platforms
{
    [ExecuteAlways]
    public class PlatformPrefabInfo : MonoBehaviour
    {
        [SerializeField] public Vector2 platformModuleBounds;
        [SerializeField] public Transform moduleTop,moduleBottom;
        void OnDrawGizmos()
        {
            Vector2 topLeft = new Vector2(transform.position.x - platformModuleBounds.x / 2, transform.position.y + platformModuleBounds.y / 2);
            Vector2 bottomRight = new Vector2(transform.position.x + platformModuleBounds.x / 2, transform.position.y - platformModuleBounds.y / 2);
            Vector2 topRight = new Vector2(transform.position.x + platformModuleBounds.x / 2, transform.position.y + platformModuleBounds.y / 2);
            Vector2 bottomLeft = new Vector2(transform.position.x - platformModuleBounds.x / 2, transform.position.y - platformModuleBounds.y / 2);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
    }
}

