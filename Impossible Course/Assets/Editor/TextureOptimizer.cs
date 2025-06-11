using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureOptimizer : EditorWindow
{
    [MenuItem("Tools/Optimize Textures for Android")]
    public static void OptimizeTextures()
    {
        string[] guids = AssetDatabase.FindAssets("t:texture", new[] { "Assets" });
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            
            if (textureImporter != null)
            {
                TextureImporterPlatformSettings androidSettings = new TextureImporterPlatformSettings();
                androidSettings.name = "Android";
                androidSettings.overridden = true;
                androidSettings.maxTextureSize = 1024;
                androidSettings.format = TextureImporterFormat.ETC2_RGBA8;
                androidSettings.compressionQuality = 50;
                androidSettings.allowsAlphaSplitting = false;
                androidSettings.androidETC2FallbackOverride = AndroidETC2FallbackOverride.UseBuildSettings;
                
                textureImporter.SetPlatformTextureSettings(androidSettings);
                
                // Mipmaps sadece gerekli texture'lar için
                if (path.Contains("Environment") || path.Contains("Character"))
                {
                    textureImporter.mipmapEnabled = true;
                }
                else
                {
                    textureImporter.mipmapEnabled = false;
                }
                
                AssetDatabase.ImportAsset(path);
                Debug.Log($"Optimized texture: {path}");
            }
        }
        
        Debug.Log("Texture optimization completed!");
    }
} 