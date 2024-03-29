using UnityEngine.UIElements;
using UnityEngine;

namespace SFR.SceneSurfer
{
    public class SceneUIBuilder
    {
        public VisualElement SceneListContainer { get; private set; }

        public SceneUIBuilder(VisualElement rootElement)
        {
            SceneListContainer = new VisualElement();
            CreateSceneSurferContainer();
            SetLabel();
            rootElement.Add(SceneListContainer);
        }

        private void CreateSceneSurferContainer()
        {
            SceneListContainer = new VisualElement();
            SceneListContainer.style.borderTopWidth = 1;
            SceneListContainer.style.borderBottomWidth = 1;
            SceneListContainer.style.borderLeftWidth = 1;
            SceneListContainer.style.borderRightWidth = 1;
            SceneListContainer.style.borderTopColor = Color.black;
            SceneListContainer.style.borderBottomColor = Color.black;
            SceneListContainer.style.borderLeftColor = Color.black;
            SceneListContainer.style.borderRightColor = Color.black;
            SceneListContainer.style.marginBottom = 5;

        }

        private void SetLabel()
        {
            Label title = new Label("Scenes in Build");
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.style.marginTop = 5;
            title.style.marginBottom = 5;
            title.style.unityTextAlign = TextAnchor.MiddleCenter;
            SceneListContainer.Add(title);
        }
    }
}