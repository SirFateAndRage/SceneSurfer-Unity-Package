using UnityEditor;

namespace SFR.SceneSurfer
{
    public class SceneCreationListener : AssetPostprocessor
    {
        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var importedAsset in importedAssets)
            {
                if (!System.Array.Exists(deletedAssets, element => element == importedAsset) &&
                    !System.Array.Exists(movedFromAssetPaths, element => element == importedAsset) &&
                    IsSceneAsset(importedAsset))
                {
                    SceneNotInBuildHandler.Instance.UpdateScenes();
                    SceneInBuildHandler.Instance.RebuildOnEvent();
                }
            }
        }

        private static bool IsSceneAsset(string assetPath)
        {
            return assetPath.EndsWith(".unity");
        }
    }
}