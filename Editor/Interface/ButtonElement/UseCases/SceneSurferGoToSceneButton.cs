using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System;

namespace SFR.SceneSurfer
{
    public class SceneSurferGoToSceneButton : IButtonElement
    {
        private Button _mainButton;
        private string _mainButtonCurrentPath;

        public Button GetButton()
        {
            Button button = new();
            button.style.width = 60; 
            button.style.height = EditorGUIUtility.singleLineHeight;
            button.text = "Go To";


            button.style.borderTopWidth = 1;
            button.style.borderBottomWidth = 1;
            button.style.borderLeftWidth = 1;
            button.style.borderRightWidth = 1;
            button.style.borderTopColor = Color.white;
            button.style.borderBottomColor = Color.white;
            button.style.borderLeftColor = Color.white;
            button.style.borderRightColor = Color.white;

            return button;
        }

        public void BindButton(string path, VisualElement element)
        {
            var button = element.Q<Button>();
            button.clickable.clicked += () =>
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(path);
                    UpdateBUttonUI(button);
                }
            };

            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.path != path)
            {
                SetButtonToDefault(button);
                return;
            }

            SetMainButton(button, "Opened", false, Color.green);
        }

        private void UpdateBUttonUI(Button button)
        {
            if (_mainButton == null)
                _mainButton = button;

            SetButtonToDefault(_mainButton);

            SetMainButton(button, "Opened", false, Color.green);
        }

        private void SetButtonToDefault(Button button)
        {
            if (null == button)
                return;

            button.text = "Go To";
            button.SetEnabled(true);
            button.style.backgroundColor = Color.gray;
        }
        private void SetMainButton(Button button, string text,bool isEnable, Color color)
        {
            button.text = text;
            button.SetEnabled(isEnable);
            button.style.backgroundColor = color;
            _mainButton = button;
        }

        public void UpdateAddSceneButton(Scene scene, OpenSceneMode mode)
        {
            if(scene.path != _mainButtonCurrentPath)
            {
                SetButtonToDefault(_mainButton);
                return;
            }
        }
    }
}