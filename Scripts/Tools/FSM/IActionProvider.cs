using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Tools
{
    public interface IActionProvider
    {
        void Action();
        void CancelAction();
    }
}

