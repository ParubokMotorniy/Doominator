using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class Bar : MonoBehaviour
    {
        [SerializeField] Component IBarOwnerComponent;
        [SerializeField] RectTransform bar;
        [SerializeField] string filledBarText,unfilledBarText;
        [SerializeField] Text barMessage;
        private IBarOwner barOwner;
        private float initialLength;
        void Start()
        {
            barOwner = IBarOwnerComponent as IBarOwner;
            initialLength = bar.localScale.y;
        }

        void Update()
        {
            bar.localScale = new Vector3(bar.localScale.x, initialLength * Mathf.Clamp(barOwner.ReportValuePercentage(), 0, 1), bar.localScale.z);
            if (bar.localScale.y < initialLength)
            {
                barMessage.text = unfilledBarText;
            } else if(barMessage.text != filledBarText)
            {
                barMessage.text = filledBarText;
            }
        }
    }
}

