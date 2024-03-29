using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;

namespace SFR.SceneSurfer
{
    public class SceneSurferListViewBuilder
    {
        private ListView _listView;
        private EditorBuildSettingsScene[] _scenes;
        private readonly SceneSurferToggleBuilder _sceneSurferToggleBuilder;
        private readonly List<IButtonElement> _buttonElementsList;

        private void LoadScenes() => _scenes = EditorBuildSettings.scenes;

        public SceneSurferListViewBuilder(VisualElement container,List<IButtonElement> buttonsElements)
        {
            _buttonElementsList = buttonsElements;
            _sceneSurferToggleBuilder = new();

            LoadScenes();

            _listView = new ListView(_scenes, EditorGUIUtility.singleLineHeight + 5, MakeItem, BindItem);
            _listView.style.flexGrow = 1;
            _listView.reorderable = true;
            container.Add(_listView);
        }

        private VisualElement MakeItem()
        {
            var container = new VisualElement();
            container.style.flexDirection = FlexDirection.Row;

            CreateBorder(container);

            VisualElement toggleElement = _sceneSurferToggleBuilder.GetSurferToggle();
            container.Add(toggleElement);

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
            foreach(IButtonElement buttonElement in _buttonElementsList)
            {
                VisualElement button = buttonElement.GetButton();
                container.Add(button);
            }
        }

        private void BindItem(VisualElement element, int index)
        {
            var scene = _scenes[index];
            var nameLabel = element.Q<Label>();
            nameLabel.text = System.IO.Path.GetFileNameWithoutExtension(scene.path);

            _sceneSurferToggleBuilder.BindToggle(scene.path, element);

            BindButtonElements(scene.path, element);

            element.RegisterCallback<DragUpdatedEvent>(evt =>
            {
                var mouseY = evt.mousePosition.y;
                var targetIndex = Mathf.Clamp((int)(mouseY / EditorGUIUtility.singleLineHeight + 5), 0, _scenes.Length - 1);

                var topLine = element.ElementAt(1);
                var bottomLine = element.ElementAt(2); 

                if (targetIndex == index)
                {
                    topLine.style.display = DisplayStyle.Flex;
                    bottomLine.style.display = DisplayStyle.Flex;
                }
                else
                {
                    topLine.style.display = DisplayStyle.None;
                    bottomLine.style.display = DisplayStyle.None;
                }

                evt.StopPropagation();
            });
            EditorBuildSettings.scenes = _scenes;
        }

        private void BindButtonElements(string path,VisualElement container)
        {
            foreach (IButtonElement buttonElement in _buttonElementsList)
            {
                buttonElement.BindButton(path, container);
            }
        }

        public void UpdateUI()
        {
            LoadScenes();
            _listView.itemsSource = _scenes;
            _listView.Rebuild();
        }
    }
}