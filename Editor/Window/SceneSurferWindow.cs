using UnityEditor;
using UnityEngine;

namespace SFR.SceneSurfer
{
    public class SceneSurferWindow : EditorWindow
    {
        private SceneSurferWindowBuilder _sceneOrderManager;

        [MenuItem("SFR/Tools/SceneSurfer")]
        public static void InitializeWindow()
        {
            SceneSurferWindow window = GetWindow<SceneSurferWindow>("Scene Surfer");
            window.minSize = new Vector2(250, 200);
        }

        private void OnEnable() => _sceneOrderManager = new(this);

        private void OnDestroy() => _sceneOrderManager.Cleanup();
    }
}