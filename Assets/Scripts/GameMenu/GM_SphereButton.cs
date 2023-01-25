using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMenu
{
    public class GM_SphereButton : CustomButton
    {
        [SerializeField] GM_Sphere owner;
        public override void OnClicked(int mouseButton)
        {
            if (SettingsManager.singleton.IsSettingsOpened)
                return;
            base.OnClicked(mouseButton);
            owner.Selected();
        }
    }
}
