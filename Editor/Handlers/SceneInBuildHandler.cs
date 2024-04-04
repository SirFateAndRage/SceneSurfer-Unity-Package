using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;


namespace SFR.SceneSurfer
{
    public class SceneInBuildHandler
    {
        private static SceneInBuildHandler _instance;
        private List<EditorBuildSettingsScene> _editorScenesInBuild;

        public Action<List<EditorBuildSettingsScene>> OnChangeOnEditorbuildsSettings;
        public List<EditorBuildSettingsScene> EditoSceneInBuild { get => _editorScenesInBuild; }

        public static SceneInBuildHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new();

                return _instance;
            }
        }

        private SceneInBuildHandler() => UpdateScene();

        private void UpdateScene()
        {
            _editorScenesInBuild = new List<EditorBuildSettingsScene>();
            _editorScenesInBuild = EditorBuildSettings.scenes.ToList();

            OnChangeOnEditorbuildsSettings?.Invoke(EditorBuildSettings.scenes.ToList());
        }

        public void RebuildOnEvent() => UpdateScene();

        public void CleanUp()
        {
            OnChangeOnEditorbuildsSettings = null;
            _editorScenesInBuild.Clear();
        }
    }
}