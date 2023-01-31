using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tools
{
    public class Graphics : MonoBehaviour
    {
        public static bool DrawTrajectory(Vector2 startingPoint, Vector2 targetVelocity, Vector2 currentVelocity, int pointsPerSemiarc, int maxNumberOfTrajectoryPoints, float minAngle, float maxAngle,float averagePlayerRadius, LineRenderer jumpLine, LayerMask pathObstacles)
        {
            float targetVelocityAngle = Mathf.Atan2(targetVelocity.y, targetVelocity.x);
            float currentVelocityAngle = Mathf.Atan2(currentVelocity.y, currentVelocity.x);

            float xVelocity = (targetVelocity.magnitude * Mathf.Cos(targetVelocityAngle) + currentVelocity.magnitude * Mathf.Cos(currentVelocityAngle));
            float yVelocity = (targetVelocity.magnitude * Mathf.Sin(targetVelocityAngle) + currentVelocity.magnitude * Mathf.Sin(currentVelocityAngle));

            float jumpAngle = Mathf.Atan2(yVelocity, xVelocity) * Mathf.Rad2Deg;
            if (jumpAngle <= minAngle || jumpAngle >= maxAngle)
            {
                jumpLine.positionCount = 0;
                return false;
            }

            float timeStep = (targetVelocity.magnitude * Mathf.Sin(targetVelocityAngle) / Mathf.Abs(Physics.gravity.y) + currentVelocity.magnitude * Mathf.Sin(currentVelocityAngle) / Mathf.Abs(Physics.gravity.y)) / (pointsPerSemiarc + 1);

            List<Vector3> linePosiitons = new List<Vector3>();
            linePosiitons.Add(startingPoint);
            jumpLine.positionCount = maxNumberOfTrajectoryPoints;

            float timeElapsed = timeStep;
            for (int i = 1; i <= (maxNumberOfTrajectoryPoints - 1); i++)
            {
                float nextPointX = startingPoint.x + (xVelocity * timeElapsed);
                float nextPointY = startingPoint.y + (yVelocity * timeElapsed - (Mathf.Abs(Physics.gravity.y) * timeElapsed * timeElapsed) / 2);

                Vector2 nextPoint;
                nextPoint = new Vector2(nextPointX, nextPointY);
                Vector2 direction = nextPoint - Tools.Math.Vector3To2(linePosiitons[i - 1]);

                RaycastHit2D obstacleHit = Physics2D.CircleCast(linePosiitons[i - 1], averagePlayerRadius, direction.normalized, direction.magnitude, pathObstacles);
                if (obstacleHit.collider != null)
                {
                    linePosiitons.Add(obstacleHit.point);
                    jumpLine.positionCount = linePosiitons.Count;
                    break;
                }
                else
                {
                    linePosiitons.Add(new Vector3(nextPointX, nextPointY, 0));
                    timeElapsed += timeStep;
                }
            }
            jumpLine.SetPositions(linePosiitons.ToArray());
            return true;
        }
        public static bool DrawTrajectory(Vector2 startingPoint, Vector2 targetVelocity,int pointsPerSemiarc, int maxNumberOfTrajectoryPoints, float minAngle, float maxAngle,float averagePlayerRadius, LineRenderer jumpLine, LayerMask pathObstacles)
        {
            float targetVelocityAngle = Mathf.Atan2(targetVelocity.y, targetVelocity.x);

            float xVelocity = (targetVelocity.magnitude * Mathf.Cos(targetVelocityAngle));
            float yVelocity = (targetVelocity.magnitude * Mathf.Sin(targetVelocityAngle));

            float jumpAngle = Mathf.Atan2(yVelocity, xVelocity) * Mathf.Rad2Deg;
            if (jumpAngle <= minAngle || jumpAngle >= maxAngle)
            {
                jumpLine.positionCount = 0;
                return false;
            }

            float timeStep = (targetVelocity.magnitude * Mathf.Sin(targetVelocityAngle) / Mathf.Abs(Physics.gravity.y)) / (pointsPerSemiarc + 1);

            List<Vector3> linePosiitons = new List<Vector3>();
            linePosiitons.Add(startingPoint);
            jumpLine.positionCount = maxNumberOfTrajectoryPoints;

            float timeElapsed = timeStep;
            for (int i = 1; i <= (maxNumberOfTrajectoryPoints - 1); i++)
            {
                float nextPointX = startingPoint.x + (xVelocity * timeElapsed);
                float nextPointY = startingPoint.y + (yVelocity * timeElapsed - (Mathf.Abs(Physics.gravity.y) * timeElapsed * timeElapsed) / 2);

                Vector2 nextPoint;
                nextPoint = new Vector2(nextPointX, nextPointY);
                Vector2 direction = nextPoint - Tools.Math.Vector3To2(linePosiitons[i - 1]);

                RaycastHit2D obstacleHit = Physics2D.Raycast(linePosiitons[i - 1], direction.normalized, direction.magnitude, pathObstacles);
                if (obstacleHit.collider != null)
                {
                    linePosiitons.Add(obstacleHit.point);
                    jumpLine.positionCount = linePosiitons.Count;
                    break;
                }
                else
                {
                    linePosiitons.Add(new Vector3(nextPointX, nextPointY, 0));
                    timeElapsed += timeStep;
                }
            }
            jumpLine.SetPositions(linePosiitons.ToArray());
            return true;
        }
    }
}
