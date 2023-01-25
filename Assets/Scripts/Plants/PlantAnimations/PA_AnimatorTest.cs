using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlantAnimator {
    public class PA_AnimatorTest : MonoBehaviour
    {
        float T;
        int partNow;
        [SerializeField] List<GameObject> Parts;
        //[SerializeField] Transform PartsContainer;

        public List<PA_StickToGround> PA_FixAngles;
        public List<PA_LeanOffTheWall> PA_LeanOffTheWalls;

        private void Awake()
        {
            //PartsContainer.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }

        private void Start()
        {
            foreach (var e in PA_FixAngles)
            {
                e.Recalculate();
            }
            foreach (var e in PA_LeanOffTheWalls)
            {
                e.Recalculate();
            }
            foreach (var e in Parts)
            {
                e.transform.localScale = Vector3.zero;
            }
        }

        void Update()
        {
            if (partNow < Parts.Count)
            {
                T += Time.deltaTime * 3;
                Parts[partNow].transform.localScale = Vector3.one * T;
                if (T >= 1)
                {
                    Parts[partNow].transform.localScale = Vector3.one;
                    T = 0;
                    partNow += 1;
                }
            }
            else {
                GetComponent<Plant>().CombineFilters();
                Destroy(this);
            }
        }
    }
}
