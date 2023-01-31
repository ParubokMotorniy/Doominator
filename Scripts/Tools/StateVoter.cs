using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tools
{
    public class StateVoter : MonoBehaviour
    {
        [SerializeField] List<Component> IVoters = new List<Component>();
        private bool votingResult;
        private void Update()
        {
            for(int i = 0; i < IVoters.Count; i++)
            {
                IStateVoter voter = IVoters[i] as IStateVoter;
                if (!voter.Vote())
                {
                    votingResult = false;
                    break;
                } else if( i == (IVoters.Count - 1))           
                {
                    votingResult = true;
                }                                                 
            }
        }
        public bool GetVotingResult()
        {
            return votingResult;
        }
    }
}

