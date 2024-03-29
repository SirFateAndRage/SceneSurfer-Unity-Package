using UnityEngine.UIElements;
using UnityEngine;

namespace SFR.SceneSurfer
{
    public class MainBoxUIView
    {
        private readonly float _border;
        private readonly float _margin;
        private readonly string _labelText;
        private readonly Color _borderColor;
        public VisualElement SceneListContainer { get; private set; }

        public MainBoxUIView(VisualElement rootElement,float border,float margin,string labelText,Color borderColor)
        {
            _border = border;
            _margin = margin;
            _labelText = labelText;
            _borderColor = borderColor;

            Initialize(rootElement);
        }

        private void Initialize(VisualElement rootElement)
        {
            SceneListContainer = new VisualElement();
            CreateSceneSurferContainer();
            SetLabel();
            rootElement.Add(SceneListContainer);
        }

        private void CreateSceneSurferContainer()
        {
            SceneListContainer = new VisualElement();
            SceneListContainer.style.borderTopWidth = _border;
            SceneListContainer.style.borderBottomWidth = _border;
            SceneListContainer.style.borderLeftWidth = _border;
            SceneListContainer.style.borderRightWidth = _border;
            SceneListContainer.style.borderTopColor = _borderColor;
            SceneListContainer.style.borderBottomColor = _borderColor;
            SceneListContainer.style.borderLeftColor = _borderColor;
            SceneListContainer.style.borderRightColor = _borderColor;
            SceneListContainer.style.marginBottom = _margin;

        }

        private void SetLabel()
        {
            Label title = new(_labelText);
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.style.marginTop = _margin;
            title.style.marginBottom = _margin;
            title.style.unityTextAlign = TextAnchor.MiddleCenter;
            SceneListContainer.Add(title);
        }
    }
}