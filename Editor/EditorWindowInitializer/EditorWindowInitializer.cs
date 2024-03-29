using UnityEditor;
using UnityEngine;

namespace SFR.SceneSurfer
{
    public class EditorWindowInitializer : EditorWindow
    {
        private SceneInBuild _sceneOnBuildBuilder;
        private ScenesInProject _sceneInProject;

        [MenuItem("SFR/Tools/SceneSurfer")]
        public static void InitializeWindow()
        {
            EditorWindowInitializer window = GetWindow<EditorWindowInitializer>("Scene Surfer");
            window.minSize = new Vector2(250, 200);
        }

        private void OnEnable()
        {
            _sceneOnBuildBuilder = new(this);
            _sceneInProject = new(this);
        }

        private void OnDestroy()
        {
            _sceneOnBuildBuilder.Cleanup();
            _sceneInProject.Cleanup();
        }
    }
}