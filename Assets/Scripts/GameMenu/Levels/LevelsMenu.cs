using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameMenu {
    public class LevelsMenu : MonoBehaviour
    {
        [SerializeField] private List<LevelButton> levelButtons;
        private bool isLevelLoading;
        public static LevelsMenu singleton;

        void Awake()
        {
            singleton = this;

            var score = KeyManager.GetKey("Score");
            var lv = KeyManager.GetKey("Level", 0);
            if (score >= 500 && lv < 1)
                lv = 1;
            if (score >= 1500 && lv < 2)
                lv = 2;

            KeyManager.SetKey("Level", lv);


            var oldLevel = KeyManager.GetKey("OldLevel", -1);
            var LevelNow = KeyManager.GetKey("Level");

            for (var i = 0; i < levelButtons.Count; i++) {
                var levelButton = levelButtons[i];
                if (LevelNow > i)
                {
                    levelButton.SetLockState(LevelLockState.unlocked);
                }
                else {
                    if (LevelNow == i)
                    {
                        if (LevelNow != oldLevel)
                        {
                            levelButton.SetLockState(LevelLockState.unlock);
                        }
                        else
                        {
                            levelButton.SetLockState(LevelLockState.unlocked);
                        }
                    }
                    else
                    {
                        levelButton.SetLockState(LevelLockState.locked);
                    }
                }
            }
            KeyManager.SetKey("OldLevel", LevelNow);
        }

        private void Start()
        {
            LevelSelected(KeyManager.GetKey("SelectedNow", 0));
        }

        public void PlayPressed() {
            if (isLevelLoading)
                return;

            isLevelLoading = true;
            MenuTutorial.TutorialBroken();
            GM_Manager.singleton.Hide();
            StartCoroutine(LoadLevel());
        }

        private IEnumerator LoadLevel() {
            yield return new WaitForSeconds(0.75f);
            SceneManager.LoadScene(2);
        }

        public void LevelSelected(int selectedID) {
            KeyManager.SetKey("SelectedNow", selectedID);

            for (var i = 0; i < levelButtons.Count; i++) {
                levelButtons[i].SetSelectedState(i == selectedID);
            }
        }

        void Update()
        {

        }
    }
}
