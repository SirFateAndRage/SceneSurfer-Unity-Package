using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;

namespace SFR.SceneSurfer
{
    public class RemoveSceneFromSceneSurfer : IButtonElement
    {
        public Button GetButton()
        {
            Button button = new Button();
            button.text = "X";
            button.style.backgroundColor = Color.red;
            button.style.color = Color.white;
            return button;
        }

        public void BindButton(string path, VisualElement element)
        {
            var removeButton = element.Query<Button>().Where(button => button.text == "X").First();
            removeButton.clicked += () =>
            {
                RemoveSceneFromBuild(path);
            };
        }

        private void RemoveSceneFromBuild(string scenePath)
        {
            var newScenesList = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

            for (int i = 0; i < newScenesList.Count; i++)
            {
                if (newScenesList[i].path == scenePath)
                {
                    newScenesList.RemoveAt(i);
                    EditorBuildSettings.scenes = newScenesList.ToArray();
                    break;
                }
            }
        }
    }
}