using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SFR.SceneSurfer
{
    public class SceneNotInBuildHandler
    {
        private static SceneNotInBuildHandler _instance;

        public static SceneNotInBuildHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new();

                return _instance;
            }
        }

        private List<EditorBuildSettingsScene> _scenesNotInBuild;

        public Action<List<EditorBuildSettingsScene>> sceneNoInBuildSubscriber;

        public List<EditorBuildSettingsScene> ScenesNotInBuild { get => _scenesNotInBuild;}

        private SceneNotInBuildHandler() => UpdateSceneNotInBuild();

        public void OnPackageImported(string packageName) => UpdateSceneNotInBuild();

        public void Cleanup()
        {
            sceneNoInBuildSubscriber = null;
            _scenesNotInBuild.Clear();
        }

        public void UpdateSceneNotInBuild()
        {
            string[] allScenePaths = AssetDatabase.FindAssets("t:Scene")
                                                   .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                                                   .ToArray();

            _scenesNotInBuild = new List<EditorBuildSettingsScene>();
            foreach (var scenePath in allScenePaths)
            {
                if (!IsSceneInBuild(scenePath))
                {
                    _scenesNotInBuild.Add(new EditorBuildSettingsScene(scenePath, true));
                }
            }
            sceneNoInBuildSubscriber?.Invoke(_scenesNotInBuild);
        }

        public void RebuildOnRemoveScene(string scenePath)
        {
            _scenesNotInBuild.RemoveAll(scene => scene.path == scenePath);
            sceneNoInBuildSubscriber?.Invoke(_scenesNotInBuild);
        }

        private bool IsSceneInBuild(string scenePath)
        {
            foreach (var buildScene in EditorBuildSettings.scenes)
            {
                if (buildScene.path == scenePath)
                {
                    return true;
                }
            }
            return false;
        }
    }
}