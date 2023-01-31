using Cinemachine;
using Game.HealthManagement;
using Game.System;
using UnityEngine;
namespace Game.Platforms
{
    public class WallOfFireController : MonoBehaviour
    {
        [Range(0, 1)]
        [SerializeField] float wallSpeed;
        public CinemachineVirtualCamera currentPlayerCam;
        private Vector3 startLocalPosition;
        private void Start()
        {
            startLocalPosition = transform.localPosition;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player" && !GameManagement.gameIsOver)
            {
                collision.gameObject.GetComponent<Health>().TakeDamage(collision.gameObject.GetComponent<Health>().GetLives());
                GameManagement.gameIsOver = true;
            }
        }
        void Update()
        {
            if (!GameManagement.gameIsOver)
            {
                float wallPositionUpdate = wallSpeed * Time.deltaTime;
                MoveWall(wallPositionUpdate);
            }          
        }
        public void MoveWall(float moveAmount)
        {
            float modifiedMoveAmount = 0;
            if((transform.localPosition.y + moveAmount) >= startLocalPosition.y)
            {
                modifiedMoveAmount = moveAmount;
            } else
            {
                modifiedMoveAmount = -transform.localPosition.y + startLocalPosition.y;
            }
            transform.localPosition += new Vector3(0, modifiedMoveAmount, 0);
            currentPlayerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight -= 0.16875f * (moveAmount / 6.05f);
            currentPlayerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY -= 0.084375f * (moveAmount / 6.05f);
        }
    }

}