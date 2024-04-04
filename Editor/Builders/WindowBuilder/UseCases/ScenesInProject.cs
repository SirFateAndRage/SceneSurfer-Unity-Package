using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace SFR.SceneSurfer
{
    public class ScenesInProject : WindowBuilder
    {
        private SceneSurferGoToSceneButton _sceneSuferGoToSceneButton;
        public ScenesInProject(EditorWindowInitializer window) : base(window)
        {
           
        }

        public override void Cleanup()
        {
            SceneNotInBuildHandler.Instance.Cleanup();

            EditorBuildSettings.sceneListChanged -= SceneNotInBuildHandler.Instance.UpdateScenes;
            AssetDatabase.importPackageCompleted -= SceneNotInBuildHandler.Instance.OnPackageImported;
            EditorBuildSettings.sceneListChanged -= SceneNotInBuildHandler.Instance.UpdateScenes;
            EditorSceneManager.sceneOpened -= _sceneSuferGoToSceneButton.UpdateAddSceneButton;
        }

        protected override void CreateUI(EditorWindowInitializer windowInitializer)
        {

            SceneNotInBuildHandler.Instance.UpdateScenes();

            VisualElement rootElement = windowInitializer.rootVisualElement;

            MainBoxUIView uiBuilder = new(rootElement, 1, 5, "Scene in project", Color.black);
            VisualElement sceneSurferMainContainer = uiBuilder.SceneListContainer;

            List<IButtonElement> buttonElementList = GetButtonElementList();

            SceneInProjectListView sceneInprojectListView = new(sceneSurferMainContainer, buttonElementList);
        }

        private List<IButtonElement> GetButtonElementList()
        {
            List<IButtonElement> buttonElementList = new();
            var sceneSelector = new SceneSurferGoToSceneButton();

            var deleteSceneAsset = new DeleSceneAsset();

            _sceneSuferGoToSceneButton = sceneSelector;
            buttonElementList.Add(sceneSelector);
            buttonElementList.Add(deleteSceneAsset);
            return buttonElementList;
        }

        protected override void RegisterEvents()
        {
            EditorBuildSettings.sceneListChanged += SceneNotInBuildHandler.Instance.UpdateScenes;
            AssetDatabase.importPackageCompleted += SceneNotInBuildHandler.Instance.OnPackageImported;
            EditorBuildSettings.sceneListChanged += SceneNotInBuildHandler.Instance.UpdateScenes;
            EditorSceneManager.sceneOpened += _sceneSuferGoToSceneButton.UpdateAddSceneButton;
        }
    }
}