using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlantAnimator
{
    public class PA_StickToGround : MonoBehaviour
    {
        [SerializeField] private float ObstacleRayForwardDistance = 1;
        [SerializeField] private LayerMask ObstacleMask;
        [SerializeField] private Transform PartTransform;

        public bool IsCalculateAtAwake = false;
        public PA_StickToGround next;

        void Start()
        {
            if(IsCalculateAtAwake)
                Recalculate();
        }

        public void Recalculate()
        {
            bool[] hits = new bool[55];
            for (var i = -27; i < 28; i++)
            {
                var ray = new Ray(transform.position, Quaternion.AngleAxis(-i * 5, transform.right) * transform.up * ObstacleRayForwardDistance);
                if (Physics.Raycast(ray, out var hitForward, ObstacleRayForwardDistance, ObstacleMask))
                {
                    hits[i + 27] = true;
                }
                else
                {
                    hits[i + 27] = false;
                }
            }

            var best = 28;
            for (var i = -26; i < 27; i++)
            {
                if (hits[i + 27] && (!hits[i + 26] || !hits[i + 28]))
                {
                    if (Mathf.Abs(i) < Mathf.Abs(best))
                        best = i;
                }
            }
            if (best == 28)
            {
                best = 0;
            }
            PartTransform.localRotation = Quaternion.Euler(-5 * best, 0, 0);

            if (next != null)
                next.Recalculate();

            Destroy(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.up * ObstacleRayForwardDistance);
            for (var i = -18; i < 19; i++)
            {
                Gizmos.color = new Color(1 / 17f * i, 0, 0);
                Gizmos.DrawLine(
                    transform.position, transform.position + Quaternion.AngleAxis(-i * 5, transform.right) * transform.up * ObstacleRayForwardDistance);
            }
        }
    }
}