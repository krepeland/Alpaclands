using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMenu
{
    public class PlantSlot : CustomButton
    {
        [Header("Main")]
        [SerializeField] string SlotKey = "Slot_";
        [SerializeField] List<string> KeysToCanBeUnlocked = new List<string>();
        [SerializeField] int Cost;

        [Header("Other")]
        [SerializeField] private SlotLockState lockState;

        [SerializeField] private GameObject costObject;
        [SerializeField] private RectTransform costCoinObject;
        [SerializeField] private Text costText;
        [SerializeField] private Text costText1;

        [SerializeField] private Animator plantImageAnimator;
        [SerializeField] private Image backgroundImage;

        [SerializeField] private Color LockedColor;
        [SerializeField] private Color UnlockableColor;
        [SerializeField] private Color UnlockedColor;

        [SerializeField] private List<GameObject> ObjectsOnUnlock;

        [SerializeField] private bool UseAnimator = true;

        private void Awake()
        {
            costText.text = Cost.ToString();
            costText1.text = Cost.ToString();

            costCoinObject.anchoredPosition += new Vector2(30 * (costText.text.Length - 1), 0);

            KeyManager.AddEvent(SlotKey, (x) => CheckState());
            KeyManager.AddEvent("Coins", (x) => CheckState());

            foreach (var key in KeysToCanBeUnlocked) {
                KeyManager.AddEvent(key, (x) => CheckState());
            }

            CheckState();
            OutHover();
        }

        void CheckState()
        {
            if (KeyManager.GetKey(SlotKey) != 0)
            {
                UpdateState(SlotLockState.unlocked);
                return;
            }
            bool canBeOpened = true;

            if (KeyManager.GetKey("Coins") < Cost)
            {
                canBeOpened = false;
            }
            else
            {

                foreach (var key in KeysToCanBeUnlocked)
                {
                    if (KeyManager.GetKey(key) == 0)
                    {
                        canBeOpened = false;
                    }
                }
            }

            UpdateState(canBeOpened ? SlotLockState.canBeUnlocked : SlotLockState.locked);
        }

        public void UpdateState(SlotLockState lockState) {
            this.lockState = lockState;
            plantImageAnimator.enabled = UseAnimator && lockState == SlotLockState.unlocked;
            if (!UseAnimator && lockState == SlotLockState.unlocked) {
                plantImageAnimator.GetComponent<Image>().color = Color.white;
            }
            foreach (var e in ObjectsOnUnlock)
            {
                e.SetActive(lockState == SlotLockState.unlocked);
            }

            switch (lockState) {
                case SlotLockState.locked:
                    backgroundImage.color = LockedColor;
                    break;
                case SlotLockState.canBeUnlocked:
                    backgroundImage.color = UnlockableColor;
                    break;
                case SlotLockState.unlocked:
                    backgroundImage.color = UnlockedColor;
                    costObject.SetActive(false);
                    break;
            }
        }

        public override void OnHover()
        {
            if (lockState == SlotLockState.canBeUnlocked) {
                transform.localScale = Vector3.one * 1.1f;
            }
            if(lockState != SlotLockState.unlocked)
                costObject.SetActive(true);
        }

        public override void OutHover()
        {
            transform.localScale = Vector3.one;
            costObject.SetActive(false);
        }

        public override void OnClicked(int mouseButton)
        {
            if (lockState == SlotLockState.canBeUnlocked)
            {
                KeyManager.AddToKey("Coins", -Cost);
                KeyManager.SetKey(SlotKey, 1);
                OutHover();

                SoundManager.TryPlayClickSound();
                AchievmentsManager.Achievment_CheckStore();
            }
        }
    }
}
