using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using Missiles;
using UnityEngine;

namespace Defendable
{
    public class MissileLauncher : CannonDefense
    {
        protected override bool IsReady { get => Time.time - lastShotTime > RelaodTime; set => IsReady = value; }

        private List<EnemyTimePositionInfo> enemyPositions = new List<EnemyTimePositionInfo>();
        protected override void Attack(Enemy enemy)
        {
            var bullet = PoolManager.Instance.GetFromPool<LaunchableMissile>(PoolObjectType.LaunchableMissile);
            bullet.transform.position = cannon.position;
            bullet.transform.rotation = tower.rotation;
            var approxPos = EnemyTimePositionInfo.CalculateInterceptCourse(enemyPositions.Last().Position, (enemyPositions.Last().Position - enemyPositions.First().Position), bullet.transform.position, bullet.Speed);
            //var target = enemy.transform.position;
            bullet.Launch(approxPos);
            lastShotTime = Time.time;
        }

        protected override bool RotateToEnemy(Enemy enemy)
        {
            if (enemyPositions.Count == 0)
                enemyPositions.Add(new EnemyTimePositionInfo(Time.time, enemy.Position));
            var direction = (enemy.transform.position - tower.transform.position);
            var targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z), tower.up);
            tower.rotation = Quaternion.RotateTowards(tower.rotation, targetRotation, rotationSpeed * Time.deltaTime);


            if (Quaternion.Angle(targetRotation, tower.rotation) <= angleThreshold)
            {
                enemyPositions.Add(new EnemyTimePositionInfo(Time.time, enemy.Position));
                return true;
            }
            return false;
        }
    }
}


public class EnemyTimePositionInfo
{
    public float Timestamp { get; private set; }
    public Vector3 Position { get; private set; }

    public EnemyTimePositionInfo(float time, Vector3 position)
    {
        Timestamp = time;
        Position = position;
    }

    public static float GetApproximateSpeed(List<EnemyTimePositionInfo> list)
    {
        var firstDetectedPos = list.First();
        var lastDetectedPos = list.Last();
        var time = lastDetectedPos.Timestamp - firstDetectedPos.Timestamp;
        var direction = (lastDetectedPos.Position - firstDetectedPos.Position).sqrMagnitude;

        return direction / time;

    }

    public static float FindClosestPointOfApproach(Vector3 aPos1, Vector3 aSpeed1, Vector3 aPos2, Vector3 aSpeed2)
    {
        Vector3 PVec = aPos1 - aPos2;
        Vector3 SVec = aSpeed1 - aSpeed2;
        float d = SVec.sqrMagnitude;
        // if d is 0 then the distance between Pos1 and Pos2 is never changing
        // so there is no point of closest approach... return 0
        // 0 means the closest approach is now!
        if (d >= -0.0001f && d <= 0.0002f)
            return 0.0f;
        return (-Vector3.Dot(PVec, SVec) / d);
    }

    public static Vector3 CalculateInterceptCourse(Vector3 aTargetPos, Vector3 aTargetSpeed, Vector3 aInterceptorPos, float aInterceptorSpeed)
    {
        Vector3 targetDir = aTargetPos - aInterceptorPos;
        float iSpeed2 = aInterceptorSpeed * aInterceptorSpeed;
        float tSpeed2 = aTargetSpeed.sqrMagnitude;
        float fDot1 = Vector3.Dot(targetDir, aTargetSpeed);
        float targetDist2 = targetDir.sqrMagnitude;
        float d = (fDot1 * fDot1) - targetDist2 * (tSpeed2 - iSpeed2);
        if (d < 0.1f)  // negative == no possible course because the interceptor isn't fast enough
            return Vector3.zero;
        float sqrt = Mathf.Sqrt(d);
        float S1 = (-fDot1 - sqrt) / targetDist2;
        float S2 = (-fDot1 + sqrt) / targetDist2;
        if (S1 < 0.0001f)
        {
            if (S2 < 0.0001f)
                return Vector3.zero;
            else
                return (S2) * targetDir + aTargetSpeed;
        }
        else if (S2 < 0.0001f)
            return (S1) * targetDir + aTargetSpeed;
        else if (S1 < S2)
            return (S2) * targetDir + aTargetSpeed;
        else
            return (S1) * targetDir + aTargetSpeed;
    }
}