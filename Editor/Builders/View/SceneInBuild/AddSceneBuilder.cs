using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

namespace SFR.SceneSurfer
{
    public class AddSceneBuilder
    {
        private readonly Button _button;

        public void UpdateButton(Scene scene, OpenSceneMode mode) => UpdateAddCurrentSceneButtonState();
        public void UpdateButton() => UpdateAddCurrentSceneButtonState();

        public AddSceneBuilder(VisualElement sceneListContainer)
        {
            _button = new();
            _button.clickable.clicked += AddCurrentSceneToBuildSettings;
            _button.style.marginTop = 5;
            _button.style.marginBottom = 5;
            sceneListContainer.Add(_button);

            UpdateAddCurrentSceneButtonState();
        }

        private void AddCurrentSceneToBuildSettings()
        {
            var currentScenePath = SceneManager.GetActiveScene().path;

            if (!IsSceneActiveInBuild(currentScenePath))
            {
                var newScenesList = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes)
                {
                new EditorBuildSettingsScene(currentScenePath, true)
                };

                EditorBuildSettings.scenes = newScenesList.ToArray();
            }

            UpdateAddCurrentSceneButtonState();
        }

        private bool IsSceneActiveInBuild(string scenePath)
        {
            foreach (var buildScene in EditorBuildSettings.scenes)
            {
                if (buildScene.path == scenePath)
                {
                    if (buildScene.enabled)
                        return true;
                }
            }
            return false;
        }

        private void UpdateAddCurrentSceneButtonState()
        {
            var currentScenePath = SceneManager.GetActiveScene().path;
            if (IsSceneExistInBuild(currentScenePath))
            {
                SetButton(false, "Current scene already in build", Color.green, Color.white);
                return;
            }

            SetButton(true, "Add current scene", Color.yellow, Color.black);
        }

        private void SetButton(bool isEnabled, string textToShow, Color backGroundColor, Color StyleColor)
        {
            _button.SetEnabled(isEnabled);
            _button.text = textToShow;
            _button.style.backgroundColor = backGroundColor;
            _button.style.color = StyleColor;

        }

        private bool IsSceneExistInBuild(string scenePath)
        {
            foreach (var buildScene in EditorBuildSettings.scenes)
            {
                if (buildScene.path == scenePath)
                    return true;
            }
            return false;
        }
    }
}