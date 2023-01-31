using Cinemachine;
using Game.Combat;
using Game.HealthManagement;
using Game.PickUps;
using Game.Platforms;
using Game.System;
using Game.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Player {
    public class PlayerController : MonoBehaviour,IStateVoter
    {
        [SerializeField] float maxJumpForce,minJumpForce,minFingerMoveToJump,maxFingerMove, maxJumpAngle, minJumpAngle, averagePlayerRadius, minShootRange;
        [SerializeField] int maxNumberOfTrajectoryPoints, pointsPerSemiarc,maxAirJumps, maxBullets;
        [SerializeField] LayerMask jumpObstacles,enemies,whatIsGround;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] Text livesText, bulletsText;
        [SerializeField] Invisibility maskInvisibility;
        public LineRenderer jumpLine, fingerLine;

        private Touch pullTouch;
        private Health health; 
        private Rigidbody2D playerRigidbody;
        private int bulletsAmount = 2, currentAvailableJumps = 0, addedJumps = 0;
        private float previousYVelocity = 0;
        private Shooter playerShooter;
        private PlayerFighter fighter;
        private StateVoter ignorationVoter;
        private bool isGrounded, invisible = false,playerCanBeIgnored = false;


        void Start()
        {
            playerRigidbody = GetComponent<Rigidbody2D>();
            playerShooter = GetComponent<Shooter>();
            fighter = GetComponent<PlayerFighter>();
            health = GetComponent<Health>();
            ignorationVoter = GetComponent<StateVoter>();
            currentAvailableJumps = maxAirJumps;
        }
        void Update()
        {
            playerCanBeIgnored = !ignorationVoter.GetVotingResult();
            if (Input.touchCount > 0 && health.IsAlive() && !health.IsStunned())
            {
                pullTouch = Input.GetTouch(0);
                Vector3 fingerCurrentPosition = Camera.main.ScreenToWorldPoint(pullTouch.position);

                DrawFingerDrag(pullTouch,fingerLine);

                Vector2 fingerMoveDirection = Camera.main.ScreenToWorldPoint(pullTouch.rawPosition) - fingerCurrentPosition;

                Math.FlipCharacter(new Vector3(transform.position.x+fingerMoveDirection.x, transform.position.y, transform.position.z), transform);

                float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, fingerMoveDirection.magnitude / (maxFingerMove - minFingerMoveToJump));

                bool canJump = Tools.Graphics.DrawTrajectory(transform.position, (fingerMoveDirection.normalized * jumpForce) / playerRigidbody.mass,pointsPerSemiarc, maxNumberOfTrajectoryPoints, minJumpAngle, maxJumpAngle,averagePlayerRadius, jumpLine, jumpObstacles);

                if (pullTouch.phase == TouchPhase.Canceled || pullTouch.phase == TouchPhase.Ended)
                {
                    if(fingerMoveDirection.magnitude >= minFingerMoveToJump)
                    {
                        if (canJump && currentAvailableJumps > 0)
                        {
                            playerRigidbody.AddForce(fingerMoveDirection.normalized * jumpForce, ForceMode2D.Impulse);
                            AddForceWithoutPhysics(playerRigidbody, fingerMoveDirection.normalized * jumpForce, ForceMode2D.Impulse);
                            currentAvailableJumps -- ;
                        }
                    }
                    else
                    {
                        Math.FlipCharacter(new Vector3(fingerCurrentPosition.x, transform.position.y, transform.position.z), transform);
                        Attack(pullTouch);
                    }
                    jumpLine.positionCount = 0;
                    fingerLine.positionCount = 0;
                }
            }
            playerAnimator.SetFloat("yVelocity", playerRigidbody.velocity.y);
            playerAnimator.SetBool("isGrounded", isGrounded);
            bulletsText.text = bulletsAmount.ToString();
            livesText.text = health.GetLives().ToString();
        }
        private void LateUpdate()
        {
            previousYVelocity = playerRigidbody.velocity.y;
            if(health.GetLives() <= 0)
            {
                GameManagement.gameIsOver = true;
            }
        }
        private void DrawFingerDrag(Touch touch,LineRenderer line)
        {
            Vector3[] linePosiitons = new Vector3[2];
            line.positionCount = 2;
            linePosiitons[0] = Camera.main.ScreenToWorldPoint(pullTouch.rawPosition);
            linePosiitons[0].z = 0;
            linePosiitons[1] = Camera.main.ScreenToWorldPoint(pullTouch.position);
            linePosiitons[1].z = 0;
            line.SetPositions(linePosiitons);
        }
        private void AddForceWithoutPhysics(Rigidbody2D body, Vector2 force, ForceMode2D mode)
        {
            body.velocity = Vector2.zero;
            body.AddForce(force,mode);
        }
        public void GiveBullets(int bulletsToGive)
        {
            int bulletsToAdd = maxBullets - bulletsAmount;
            bulletsAmount += Mathf.Clamp(bulletsToGive,0,bulletsToAdd);
        }
        private void Attack(Touch touchToAim)
        {
            Vector3 convertedPosition = Camera.main.WorldToScreenPoint(transform.position);
            if (bulletsAmount > 0 && Vector3.Distance(convertedPosition,touchToAim.position) > minShootRange)
            {
                GameObject target = new GameObject();
                target.transform.position = Camera.main.ScreenToWorldPoint(touchToAim.position);
                Destroy(target, 5);
                if (playerShooter.Attack(target))
                {
                    bulletsAmount--;
                }
            } else
            {
                fighter.SwingSword(touchToAim);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((whatIsGround.value & (1 << collision.gameObject.layer)) > 0)
            {
                if(previousYVelocity < 0 && !isGrounded)
                {
                    currentAvailableJumps = maxAirJumps;
                    isGrounded = true;
                }
            }
            if (collision.gameObject.layer == 14)
            {
                Animation fadeAnimation = collision.gameObject.GetComponent<Animation>();
                if (fadeAnimation != null)
                {
                    fadeAnimation.Play();
                }
                IPerk perk = collision.gameObject.GetComponent<IPerk>();
                if (perk != null)
                {
                    perk.ActivatePerk(this.gameObject);
                }
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if ((whatIsGround.value & (1 << collision.gameObject.layer)) > 0)
            {
                isGrounded = true;
                if (currentAvailableJumps != maxAirJumps)
                {
                    currentAvailableJumps = maxAirJumps;
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if ((whatIsGround.value & (1 << collision.gameObject.layer)) > 0)
            {
                if (isGrounded)
                {
                    isGrounded = false;
                }
            }
        }
        public void OnHurtStateEnter()
        {
            jumpLine.positionCount = 0;
            fingerLine.positionCount = 0;
        }
        public void CharacterDied()
        {
            jumpLine.positionCount = 0;
            fingerLine.positionCount = 0;
        }
        public void SetInvisible(bool invisible, float invisibilityDuration)
        {
            this.invisible = invisible;
            if (invisible)
            {
                maskInvisibility.StartCoroutine(maskInvisibility.GoInvisible(invisibilityDuration));
            }
        }
        public bool IsInvisible()
        {
            return invisible;
        }
        public void OnResurrection()
        {
            Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponentInChildren<WallOfFireController>().MoveWall(-50);
        }

        public bool Vote()
        {
            return !invisible;
        }
        public bool PlayerCanBeIgnored()
        {
            return playerCanBeIgnored;
        }
    }
}
