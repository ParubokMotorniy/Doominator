using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Tools
{
    public class ActionStack : MonoBehaviour
    {
        [SerializeField] Component IActionStart;
        private List<IActionProvider> actionStack = new List<IActionProvider>();
        public void Push(IActionProvider item)
        {
            int length = actionStack.Count;
            if ((length != 0 && actionStack[length - 1] != item) || length == 0)
            {
                actionStack.Add(item);
            }
        }
        public void Pop()
        {
            int length = actionStack.Count;
            if (length != 0)
            {
                actionStack[length - 1].CancelAction();
                actionStack.RemoveAt(length - 1);
            }
        }
        public IActionProvider GetTopItem()
        {
            int length = actionStack.Count;
            if (length != 0)
            {
                return actionStack[length - 1];
            }
            return null;
        }
        private void Update()
        {
            int length = actionStack.Count;
            if (length != 0)
            {
                actionStack[length - 1].Action();
            }
        }
        private void Start()
        {
            Push(IActionStart as IActionProvider);
        }
    }
}

