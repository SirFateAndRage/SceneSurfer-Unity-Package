using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SFR.SceneSurfer
{
    public class SceneInBuild : WindowBuilder
    {
        private SceneSurferListViewBuilder _sceneListView;
        private AddSceneBuilder _sceneActionHandler;
        private SceneSurferGoToSceneButton _sceneSuferGoToSceneButton;

        public SceneInBuild(EditorWindowInitializer window) : base(window)
        {
        }

        protected override void CreateUI(EditorWindowInitializer windowInitializer)
        {
            VisualElement rootElement = windowInitializer.rootVisualElement;

            MainBoxUIView uiBuilder = new(rootElement,1,5,"Scene in build",Color.black);
            VisualElement sceneSurferMainContainer = uiBuilder.SceneListContainer;

            _sceneActionHandler = new(uiBuilder.SceneListContainer);

            List<IButtonElement> buttonElementList = GetButtonElementList();
            _sceneListView = new SceneSurferListViewBuilder(sceneSurferMainContainer, buttonElementList);


            VisualElement spaceElement = new();
            spaceElement.style.height = 10;
            rootElement.Add(spaceElement);
        }

        private List<IButtonElement> GetButtonElementList()
        {
            List<IButtonElement> buttonElementList = new();
            var sceneSelector = new SceneSurferGoToSceneButton();

            _sceneSuferGoToSceneButton = sceneSelector;
            IButtonElement removeScene = new RemoveSceneFromSceneSurfer();
            buttonElementList.Add(sceneSelector);
            buttonElementList.Add(removeScene);
            return buttonElementList;
        }

        protected override void RegisterEvents()
        {
            EditorBuildSettings.sceneListChanged += SceneInBuildHandler.Instance.RebuildOnEvent;
            EditorBuildSettings.sceneListChanged += _sceneActionHandler.UpdateButton;
            EditorSceneManager.sceneOpened += _sceneActionHandler.UpdateButton;

            EditorSceneManager.sceneOpened += _sceneSuferGoToSceneButton.UpdateAddSceneButton;
        }

        public override void Cleanup()
        {
            SceneInBuildHandler.Instance.CleanUp();

            EditorBuildSettings.sceneListChanged -= SceneInBuildHandler.Instance.RebuildOnEvent;

            EditorBuildSettings.sceneListChanged -= _sceneActionHandler.UpdateButton;
            EditorSceneManager.sceneOpened -= _sceneActionHandler.UpdateButton;

            EditorSceneManager.sceneOpened -= _sceneSuferGoToSceneButton.UpdateAddSceneButton;
        }
    }
}