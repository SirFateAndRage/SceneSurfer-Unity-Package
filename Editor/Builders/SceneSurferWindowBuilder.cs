using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

namespace SFR.SceneSurfer
{
    public class SceneSurferWindowBuilder
    {
        private SceneSurferWindow _window;
        private SceneSurferListViewBuilder _sceneListView;
        private AddSceneBuilder _sceneActionHandler;
        private SceneSurferGoToSceneButton _sceneSuferGoToSceneButton;

        public SceneSurferWindowBuilder(SceneSurferWindow window)
        {
            _window = window;
            Initialize();
        }

        private void Initialize()
        {
            CreateUI();
            RegisterEvents();
        }

        private void CreateUI()
        {
            VisualElement rootElement = _window.rootVisualElement;

            SceneUIBuilder uiBuilder = new(rootElement);
            VisualElement sceneSurferMainContainer = uiBuilder.SceneListContainer;

            _sceneActionHandler = new(uiBuilder.SceneListContainer);

            List<IButtonElement> buttonElementList = GetButtonElementList();
            _sceneListView = new SceneSurferListViewBuilder(sceneSurferMainContainer, buttonElementList);
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

        private void RegisterEvents()
        {
            EditorBuildSettings.sceneListChanged += _sceneListView.UpdateUI;

            EditorBuildSettings.sceneListChanged += _sceneActionHandler.UpdateButton;
            EditorSceneManager.sceneOpened += _sceneActionHandler.UpdateButton;

            EditorSceneManager.sceneOpened += _sceneSuferGoToSceneButton.UpdateAddSceneButton;
        }

        public void Cleanup()
        {
            EditorBuildSettings.sceneListChanged -= _sceneListView.UpdateUI;

            EditorBuildSettings.sceneListChanged -= _sceneActionHandler.UpdateButton;
            EditorSceneManager.sceneOpened -= _sceneActionHandler.UpdateButton;

            EditorSceneManager.sceneOpened -= _sceneSuferGoToSceneButton.UpdateAddSceneButton;
        }
    }
}