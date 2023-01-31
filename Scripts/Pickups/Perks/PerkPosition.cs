using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PickUps
{
    public class PerkPosition : MonoBehaviour
    {
        [Range(0,1)]
        [SerializeField] private float commonDropRate, rareDropRate, ultrarareDropRate;

        public float GetCommonRate()
        {
            return commonDropRate;
        }
        public float GetRareRate()
        {
            return rareDropRate;
        }
        public float GetUltrarareRate()
        {
            return ultrarareDropRate;
        }
        public Dictionary<Perk, float> GetDropRates()
        {
            Dictionary<Perk,float> rates = new Dictionary<Perk, float>();
            rates.Add(Perk.Common,commonDropRate);
            rates.Add(Perk.Rare, rareDropRate);
            rates.Add(Perk.Ultrarare,ultrarareDropRate);
            return rates;
        }
    }
}

