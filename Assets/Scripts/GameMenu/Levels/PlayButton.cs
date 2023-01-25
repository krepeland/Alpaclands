using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMenu
{
    public class PlayButton : CustomButton
    {
        [SerializeField] private LevelsMenu LevelsMenu;

        public override void OnHover()
        {
            if (SettingsManager.singleton.IsSettingsOpened)
            {
                OutHover();
                return;
            }
            transform.localScale = Vector3.one * 1.1f;
            base.OnHover();
        }

        public override void OutHover()
        {
            transform.localScale = Vector3.one;
            base.OutHover();
        }

        public override void OnClicked(int mouseButton)
        {
            if (SettingsManager.singleton.IsSettingsOpened)
            {
                OutHover();
                return;
            }
            LevelsMenu.PlayPressed();
        }
    }
}
