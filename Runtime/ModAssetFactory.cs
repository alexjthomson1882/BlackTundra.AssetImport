using BlackTundra.Foundation.IO;
using BlackTundra.ModFramework.Media;
using BlackTundra.ModFramework.Model;
using BlackTundra.ModFramework.Prefab;
using BlackTundra.ModFramework.Utility;

namespace BlackTundra.ModFramework {

    internal static class ModAssetFactory {

        #region constant

        #endregion

        #region logic

        internal static ModAsset Create(in ModInstance modInstance, in FileSystemReference fsr) {
            string assetAbsolutePath = fsr.AbsolutePath;
            string assetPath = assetAbsolutePath[modInstance.childModAssetFsrPathStartIndex..];
            ulong guid = modInstance._guidIdentifier | ((ulong)assetAbsolutePath.ToLower().GetHashCode() * ModInstance.ModAssetGUIDMask);
            ModAssetType assetType = ImportUtility.ExtensionToAssetType(fsr.FileExtension);
            return assetType switch {
                // media:
                ModAssetType.MediaPng or ModAssetType.MediaBmp or ModAssetType.MediaTif or ModAssetType.MediaTga or ModAssetType.MediaPsd or ModAssetType.MediaJpg => new ModImage(
                    modInstance, guid, assetType, fsr, assetPath
                ),
                // audio:
                ModAssetType.MediaWav => new WavAudio(
                    modInstance, guid, assetType, fsr, assetPath
                ),
                // model:
                ModAssetType.ModelObj => new ObjModel(
                    modInstance, guid, assetType, fsr, assetPath, MeshBuilderOptions.OptimizeMesh
                ),
                // json:
                ModAssetType.JsonObj => new ModPrefab(
                    modInstance, guid, assetType, fsr, assetPath
                ),
                // unknown:
                _ => null
            };
        }

        #endregion

    }

}