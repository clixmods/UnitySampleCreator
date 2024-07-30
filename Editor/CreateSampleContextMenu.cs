using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace SampleCreator.Editor
{
    public static class CreateSampleContextMenu
    {
        [MenuItem("Assets/Create Samples for this folder...", true)]
        private static bool IsFolderSelected()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            Debug.Log(System.IO.Directory.Exists(path));
                
            return System.IO.Directory.Exists(path);
        }
        
        [MenuItem("Assets/Create Samples for this folder...", false, 20)]
        private static void CreateSamples()
        {
            string folderPath = GetSelectedFolderPath();

            if (!string.IsNullOrEmpty(folderPath))
            {
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogWarning("No folder selected. Please select a folder in the Project view.");
            }
        }
        
        private static string GetSelectedFolderPath()
        {
            if (Selection.activeObject != null)
            {
                string path = AssetDatabase.GetAssetPath(Selection.activeObject);

                if (System.IO.Directory.Exists(path))
                {
                    return path;
                }
                else
                {
                    path = System.IO.Path.GetDirectoryName(path);
                    if (System.IO.Directory.Exists(path))
                    {
                        return path;
                    }
                }
            }

            return null;
        }

        private static void AddSampleToPackage(string packageJson, Sample sample)
        {
            // Search "samples" key in package.json 
            
        }
        
        /// <summary>
        /// Get samples from package.json. Returns null if no samples are found.
        /// </summary>
        /// <param name="packageJson">The package.json file as a string.</param>
        private static Sample[] GetSamplesFromPackage(string packageJson)
        {
            // Search "samples" key in package.json 
            JObject json = JObject.Parse(packageJson);
            
            if (json["samples"] != null)
            {
                return json["samples"].ToObject<Sample[]>();
            }
            
            return null;
        }
    }
}