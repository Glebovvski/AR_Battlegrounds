using UnityEngine;

namespace Defendable
{
    public class DefencePosition : MonoBehaviour
    {
        public Defence Defence { get; private set; }
        public bool IsFree => Defence != null;
        public Vector3 Position => this.transform.position;

        public void SetDefence(Defence defence)
        {
            if (IsFree)
                Defence = defence;
        }
    }
}