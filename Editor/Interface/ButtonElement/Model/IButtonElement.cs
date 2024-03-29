using UnityEngine.UIElements;

namespace SFR.SceneSurfer
{
    public interface IButtonElement
    {
        public Button GetButton();

        public void BindButton(string path, VisualElement element);
    }
}