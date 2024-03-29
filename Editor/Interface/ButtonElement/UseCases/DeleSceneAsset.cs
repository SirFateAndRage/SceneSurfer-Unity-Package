using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SFR.SceneSurfer
{
    public class DeleSceneAsset : IButtonElement
    {
        public Button GetButton()
        {
            Button button = new();
            button.text = "Delete Asset";
            button.style.backgroundColor = Color.red;
            button.style.color = Color.white;
            return button;
        }

        public void BindButton(string path, VisualElement element)
        {
            var removeButton = element.Query<Button>().Where(button => button.text == "Delete Asset").First();
            removeButton.clicked += () =>
            {
                bool confirmed = EditorUtility.DisplayDialog("Delete Scene Asset",
                                             $"Are you sure you want to delete : {path}?",

                                             "Yes", "No");

                if (confirmed)
                {
                    RemoveSceneFromProject(path);
                }
            };
        }

        private void RemoveSceneFromProject(string scenePath)
        {
            AssetDatabase.DeleteAsset(scenePath);
            AssetDatabase.Refresh();
        }
    }
}