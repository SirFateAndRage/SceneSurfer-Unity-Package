using UnityEditor;
using UnityEngine;

namespace SFR.SceneSurfer
{
    public class EditorWindowInitializer : EditorWindow
    {
        private SceneInBuild _SceneOnBuildBuilder;

        [MenuItem("SFR/Tools/SceneSurfer")]
        public static void InitializeWindow()
        {
            EditorWindowInitializer window = GetWindow<EditorWindowInitializer>("Scene Surfer");
            window.minSize = new Vector2(250, 200);
        }

        private void OnEnable() => _SceneOnBuildBuilder = new(this);

        private void OnDestroy() => _SceneOnBuildBuilder.Cleanup();
    }
}