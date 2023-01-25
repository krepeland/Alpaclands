using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMenu
{
    public class GM_Sphere : MonoBehaviour
    {
        [SerializeField] public int SphereID;
        [SerializeField] private float startSize;
        [SerializeField] private float targetSize;
        [SerializeField] private float startPos;
        [SerializeField] private float targetPos;
        [SerializeField] private float resizeT;
        [SerializeField] private bool isResizing;
        [SerializeField] private Transform SelectTransform;
        [SerializeField] private float resizeSpeed = 1;


        [SerializeField] private RectTransform rectTransform;

        private void Start()
        {
            GM_Manager.singleton.RegisterSphere(this);
        }

        public RectTransform GetRectTransform() {
            return rectTransform;
        }

        public void SetSelectTransformEnabled(bool isEnabled) {
            SelectTransform.gameObject.SetActive(isEnabled);
        }

        private void Update()
        {
            if (isResizing)
            {
                resizeT += Time.deltaTime * 2f * resizeSpeed;
                if (resizeT < 1)
                {
                    rectTransform.localScale = Vector3.one * Mathf.Lerp(startSize, targetSize, resizeT);
                    rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(startPos, targetPos, resizeT), 0);
                }
                else
                {
                    rectTransform.localScale = Vector3.one * targetSize;
                    rectTransform.anchoredPosition = new Vector2 (targetPos, 0);
                    isResizing = false;
                    resizeT = 0;
                }
            }
        }

        public void SetTargetSize(float size, float targetX, bool setNow = false, float speed = 1) {
            if (setNow)
            {
                startPos = targetX;
                targetPos = targetX;
                startSize = size;
                targetSize = size;
                resizeSpeed = speed;
                rectTransform.localScale = Vector3.one * targetSize;
                rectTransform.anchoredPosition = new Vector2(targetX, 0);
            }
            else
            {
                startPos = rectTransform.anchoredPosition.x;
                targetPos = targetX;
                startSize = rectTransform.localScale.x;
                targetSize = size;
                isResizing = true;
                resizeSpeed = speed;
                resizeT = 0;
            }
        }

        public void Selected() {
            GM_Manager.singleton.SelectedNow(SphereID);
        }
    }
}
