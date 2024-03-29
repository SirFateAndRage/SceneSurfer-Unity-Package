using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SFR.SceneSurfer
{
    public class SceneInProjectListView
    {
        private ListView _listView;
        private readonly List<IButtonElement> _buttonElementsList;
        private List<EditorBuildSettingsScene> _notInBuildList;

        public SceneInProjectListView(VisualElement container, List<IButtonElement> buttonsElements)
        {
            _notInBuildList = SceneNotInBuildHandler.Instance.ScenesNotInBuild;
            _buttonElementsList = buttonsElements;

            SceneNotInBuildHandler.Instance.sceneNoInBuildSubscriber += LoadScene;

            _listView = new ListView(_notInBuildList, EditorGUIUtility.singleLineHeight + 5, MakeItem, BindItem);
            _listView.style.flexGrow = 1;
            container.Add(_listView);
        }

        private VisualElement MakeItem()
        {
            var container = new VisualElement();
            container.style.flexDirection = FlexDirection.Row;

            CreateBorder(container);

            CreateLabel(container);

            SetButtonsElements(container);

            return container;
        }

        private void CreateBorder(VisualElement container)
        {
            container.style.borderTopWidth = 1;
            container.style.borderBottomWidth = 1;
            container.style.borderLeftWidth = 1;
            container.style.borderRightWidth = 1;
            container.style.borderTopColor = Color.gray;
            container.style.borderBottomColor = Color.gray;
            container.style.borderLeftColor = Color.gray;
            container.style.borderRightColor = Color.gray;
        }
        private void CreateLabel(VisualElement container)
        {
            Label nameLabel = new Label();
            nameLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
            nameLabel.style.flexGrow = 1;
            container.Add(nameLabel);
        }

        private void SetButtonsElements(VisualElement container)
        {
            if (null == _buttonElementsList || _buttonElementsList.Count == 0)
                return;

            foreach (IButtonElement buttonElement in _buttonElementsList)
            {
                VisualElement button = buttonElement.GetButton();
                container.Add(button);
            }
        }

        private void BindItem(VisualElement element, int index)
        {
            var scene = _notInBuildList[index];
            var nameLabel = element.Q<Label>();
            nameLabel.text = System.IO.Path.GetFileNameWithoutExtension(scene.path);

            BindButtonElements(scene.path, element);

        }

        private void BindButtonElements(string path, VisualElement container)
        {
            if (null == _buttonElementsList || _buttonElementsList.Count == 0)
                return;

            foreach (IButtonElement buttonElement in _buttonElementsList)
            {
                buttonElement.BindButton(path, container);
            }
        }

        private void LoadScene(List<EditorBuildSettingsScene> obj)
        {
            _notInBuildList = obj;
            _listView.itemsSource = _notInBuildList;
            _listView.Rebuild();
        }
    }
}