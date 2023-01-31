using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tools
{
    public class Math : MonoBehaviour
    {
        public static Vector3 Vector2To3 (Vector2 vectorToConvert)
        {
            return new Vector3(vectorToConvert.x, vectorToConvert.y, 0);
        }
        public static Vector2 Vector3To2 (Vector3 vectorToConvert)
        {
            return new Vector3(vectorToConvert.x, vectorToConvert.y);
        }
        public static void FlipCharacter(Vector3 toLookAt,Transform toRotate)
        {
            Vector3 direction = toLookAt - toRotate.position;
            float angle = Vector3.Angle(toRotate.right,direction.normalized);
            toRotate.Rotate(new Vector3(0,angle,0));
        }

    }
}
