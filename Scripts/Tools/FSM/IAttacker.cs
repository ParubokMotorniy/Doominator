using System;
using UnityEngine;
namespace Game.Tools
{
    public interface IAttacker
    {
        bool Attack(GameObject target);
    }
}

