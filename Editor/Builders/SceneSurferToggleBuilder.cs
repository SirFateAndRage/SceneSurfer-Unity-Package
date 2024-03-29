using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace SFR.SceneSurfer
{
    public class SceneSurferToggleBuilder
    {
        public Toggle GetSurferToggle()
        {
            Toggle toggle = new();
            return toggle;
        }

        public void BindToggle(string path, VisualElement element)
        {
            Toggle activeToggle = element.Q<Toggle>();

            bool sceneIsActive = IsSceneActiveInBuild(path);


            activeToggle.value = sceneIsActive;

            activeToggle.RegisterValueChangedCallback(evt =>
            {
                // Cuando cambia el valor del Toggle, actualizar el estado de la escena en el Build Settings
                UpdateSceneActiveState(path, evt.newValue);
            });
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

        private void UpdateSceneActiveState(string scenePath, bool isActive)
        {
            var newScenesList = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

            for (int i = 0; i < newScenesList.Count; i++)
            {
                if (newScenesList[i].path == scenePath)
                {
                    newScenesList[i] = new EditorBuildSettingsScene(scenePath, isActive);
                    EditorBuildSettings.scenes = newScenesList.ToArray();
                    break;
                }
            }
        }
    }
}