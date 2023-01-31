using System.Collections.Generic;
using UnityEngine;

namespace Game.Tools
{
    public class GameObjectStack : MonoBehaviour
    {
        [SerializeField] public int maxItemsCount;
        public List<GameObject> stack = new List<GameObject>();
        public void Push(GameObject item)
        {
            int length = stack.Count;
            if ((length != 0 && stack[length - 1].gameObject != item) || length == 0)
            {
                stack.Add(item);
            }
        }
        public void Pop()
        {
            int length = stack.Count;
            if (length != 0)
            {
                Destroy(stack[length - 1].gameObject);
            }
        }
        public GameObject GetTopItem()
        {
            int length = stack.Count;
            if(length != 0)
            {
                return stack[length-1];
            }
            return null;
        }
        void Update()
        {
            int length = stack.Count;
            if(length > maxItemsCount)
            {
                int itemsToRemove = length - maxItemsCount;
                List<GameObject> substitudeStack = new List<GameObject>();
                for (int i = 0; i < itemsToRemove; i++)
                {
                    GameObject objToDestory = stack[i];
                    Destroy(objToDestory.gameObject);
                }
                for (int i = itemsToRemove; i <= maxItemsCount; i++)
                {
                    substitudeStack.Add(stack[i]);
                }
                stack = substitudeStack;
            }
        }
    }
}

