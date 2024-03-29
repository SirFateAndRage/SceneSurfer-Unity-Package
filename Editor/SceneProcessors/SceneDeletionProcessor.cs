using UnityEditor;
using UnityEngine;

namespace SFR.SceneSurfer
{
    public class SceneDeletionProcessor : AssetModificationProcessor
    {
        public static  AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
        {
            if (assetPath.EndsWith(".unity"))
            {
                if(options == RemoveAssetOptions.DeleteAssets|| options == RemoveAssetOptions.MoveAssetToTrash)
                    SceneNotInBuildHandler.Instance.RebuildOnRemoveScene(assetPath);
            }
            return AssetDeleteResult.DidNotDelete;
        }
    }
}