using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMenu
{
    public class GM_Manager : MonoBehaviour
    {
        float diameter;

        [SerializeField] RectTransform canvasPointTR;
        [SerializeField] RectTransform canvasPointTRCenter;
        [SerializeField] RectTransform canvasPointBL;
        [SerializeField] RectTransform canvasPointBLCenter;

        List<GM_Sphere> spheres;

        public int SelectedIdNow;

        public static GM_Manager singleton;
        public bool isHiding;

        private void Awake()
        {
            singleton = this;
            spheres = new List<GM_Sphere>();
        }

        private void Start()
        {
            spheres.Sort((x, y) => { return x.SphereID - y.SphereID; });
            RecalculateSpheresSize();
            foreach (var sphere in spheres)
            {
                //sphere.GetRectTransform().sizeDelta = new Vector2(diameter, diameter);
                sphere.transform.localScale = Vector2.one * (diameter / 1080f);
            }
            SelectedNow(0, true);
            foreach (var e in spheres)
            {
                e.SetTargetSize(0, e.GetRectTransform().anchoredPosition.x, true);
            }

            SelectedNow(0, false);
        }

        public void RegisterSphere(GM_Sphere sphere) {
            spheres.Add(sphere);
        }

        public void SelectedNow(int ID, bool setNow = false) {
            if (isHiding)
                return;
            var oldId = SelectedIdNow;
            SelectedIdNow = ID;
            foreach (var sphere in spheres) {
                var size = 0f;
                var targetPos = 0f;
                var isCanBeSelected = true;
                if (sphere.SphereID == ID)
                {
                    size = diameter / 1080f;
                    targetPos = 0;
                    sphere.GetRectTransform().SetSiblingIndex(2);
                    isCanBeSelected = false;
                }

                if (sphere.SphereID == GetNextSphereIndex(ID, -1))
                {
                    size = diameter / 4320f;
                    targetPos = -0.65f * diameter;
                    sphere.GetRectTransform().SetSiblingIndex(0);
                }

                if (sphere.SphereID == GetNextSphereIndex(ID, 1))
                {
                    size = diameter / 4320f;
                    targetPos = 0.65f * diameter;
                    sphere.GetRectTransform().SetSiblingIndex(0);
                }

                if (sphere.SphereID == oldId)
                {
                    sphere.GetRectTransform().SetSiblingIndex(1);
                }

                sphere.SetSelectTransformEnabled(isCanBeSelected);
                sphere.SetTargetSize(size, targetPos, setNow);
            }
        }

        private int GetNextSphereIndex(int ID, int delta) {
            return (ID + delta + spheres.Count) % spheres.Count;
        }

        void RecalculateSpheresSize()
        {
            canvasPointTR.ForceUpdateRectTransforms();
            canvasPointTRCenter.position = canvasPointTR.position;
            canvasPointTRCenter.ForceUpdateRectTransforms();

            canvasPointBL.ForceUpdateRectTransforms();
            canvasPointBLCenter.position = canvasPointBL.position;
            canvasPointBLCenter.ForceUpdateRectTransforms();

            var width = canvasPointTRCenter.anchoredPosition.x - canvasPointBLCenter.anchoredPosition.x;
            var height = canvasPointTRCenter.anchoredPosition.y - canvasPointBLCenter.anchoredPosition.y;

            diameter = width / 1.55f;
            diameter = Mathf.Clamp(diameter, 0, height * 0.95f);

            //sphereTest1.sizeDelta = new Vector2(diameter, diameter);
            //sphereTest1.anchoredPosition = new Vector2(0, 0);
            //
            //sphereTest2.sizeDelta = new Vector2(diameter, diameter);
            //sphereTest2.localScale = new Vector2(0.25f, 0.25f);
            //sphereTest2.anchoredPosition = new Vector2(0.65f * diameter, 0);
            //
            //sphereTest3.sizeDelta = new Vector2(diameter, diameter);
            //sphereTest3.localScale = new Vector2(0.25f, 0.25f);
            //sphereTest3.anchoredPosition = new Vector2(-0.65f * diameter, 0);

        }

        public void Hide()
        {
            isHiding = true;
            foreach (var e in spheres) 
            {
                e.SetTargetSize(0, e.GetRectTransform().anchoredPosition.x, false, 2);
            }
        }
    }
}
