using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace GameMenu
{

    public class LevelButton : CustomButton
    {
        [SerializeField] LevelLockState lockState;
        [SerializeField] Transform lockTransform;
        [SerializeField] Transform brokenLockTransform;
        [SerializeField] Transform openedColorTransform;

        [SerializeField] Image LevelImage;

        [SerializeField] Color LockedLevelColor;
        [SerializeField] Color UnLockedLevelColor;

        [SerializeField] Transform SelectedLevelTransform;

        [SerializeField] int LevelID;

        int state;
        float TtillUnlock;

        private void Start()
        {
            switch (lockState) {
                case LevelLockState.locked:
                    enabled = false;
                    break;
                case LevelLockState.unlock:
                    enabled = true;
                    break;
                case LevelLockState.unlocked:
                    enabled = false;
                    lockTransform.gameObject.SetActive(false);
                    openedColorTransform.localScale = new Vector3(1, 1, 1);
                    LevelImage.color = UnLockedLevelColor;
                    break;
            }
        }

        public void SetLockState(LevelLockState levelLockState) {
            lockState = levelLockState;
        }

        public void SetSelectedState(bool isSelected) {
            SelectedLevelTransform.gameObject.SetActive(isSelected);
        }

        public override void OnClicked(int mouseButton)
        {
            if (SettingsManager.singleton.IsSettingsOpened)
            {
                OutHover();
                return;
            }
            if (lockState != LevelLockState.locked)
                LevelsMenu.singleton.LevelSelected(LevelID);
        }

        private void Update()
        {
            TtillUnlock += Time.deltaTime;
            switch (state) {
                case 0:
                    if (TtillUnlock > 1f)
                    {
                        lockTransform.gameObject.SetActive(false);
                        brokenLockTransform.gameObject.SetActive(true);
                        state = 1;
                        TtillUnlock = 0;
                        OnClicked(0);
                    }
                    break;
                case 1:
                    if (TtillUnlock > 1f)
                    {
                        state = 2;
                        TtillUnlock = 0;
                    }
                    break;
                case 2:
                    openedColorTransform.localScale = new Vector3(TtillUnlock, TtillUnlock, 1);
                    LevelImage.color = Color.Lerp(LockedLevelColor, UnLockedLevelColor, TtillUnlock);
                    if (TtillUnlock > 1f)
                    {
                        openedColorTransform.localScale = new Vector3(1, 1, 1);
                        enabled = false;
                    }
                    break;
            }
        }

        public override void OnHover()
        {
            if (SettingsManager.singleton.IsSettingsOpened)
            {
                OutHover();
                return;
            }
            if (lockState == LevelLockState.locked)
                return;
            transform.localScale = Vector3.one * 0.8f;
        }

        public override void OutHover()
        {
            transform.localScale = Vector3.one * 0.75f;
        }
    }
}
