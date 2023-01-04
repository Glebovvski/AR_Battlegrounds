using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Missiles
{
    public class CannonBullet : Missile
    {
        public override void OnTriggerEnter(Collider other)
        {
            if(!IsFromPool) return;

            PoolManager.Instance.ReturnToPool(this.gameObject, PoolObjectType.CannonBullet);
            Debug.LogError("RETURN TO POOL");
        }
    }
}