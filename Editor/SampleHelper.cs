using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace SampleCreator.Editor
{
    public static class SampleHelper
    {
        
        private static void CopyDirectory(string sourceDir, string destinationDir)
        {
            // Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourceDir, destinationDir));
            }

            // Copy all the files
            foreach (string newPath in Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourceDir, destinationDir), true);
            }
        }
        
        public static void AddSampleToPackage(PackageInfo packageInfo, Sampledd sampledd)
        {
            //1. Copy and paste the sample folder to the package folder in Samples~ folder
            string packageFolder = "Packages/" + packageInfo.name;
            string sampleFolder = sampledd.Path;
            
            // Get the destination folder in absolute path (add absolute path to the package folder)
            string destinationFolder = Path.Combine(PackageInfo.FindForAssetPath("Packages/" + packageInfo.name).assetPath,"Samples~", sampledd.DisplayName);
        
            // Ensure the destination directory exists
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }
            
            CopyDirectory(sampleFolder, destinationFolder);
            
            // Delete the .meta files in the destination folder
            foreach (string file in Directory.GetFiles(destinationFolder, "*.meta", SearchOption.AllDirectories))
            {
                File.Delete(file);
            }
            
            Debug.Log($"Sample copied from {sampleFolder} to {destinationFolder}");
            
            //2. Update the package.json file with the new sample
            
            // Get the package json file located in the package folder
            string packageJsonPath = PackageInfo.FindForAssetPath("Packages/" + packageInfo.name).assetPath + "/package.json";
            
            // Read the package.json file
            string packageJson = System.IO.File.ReadAllText(packageJsonPath);
            
            // Parse the package.json file
            JObject package = JObject.Parse(packageJson);
            
            // Get the "samples" key from the package.json file
            JArray samples = (JArray)package["samples"];
            
            // Create a new sample object
            JObject sample = new JObject
            {
                ["displayName"] = sampledd.DisplayName,
                ["description"] = sampledd.Description,
                ["path"] = Path.Combine("Samples~", sampledd.DisplayName)
            };
            
            // Add the sample object to the "samples" key
            if (samples == null)
            {
                samples = new JArray();
                package["samples"] = samples;
            }
            
            samples.Add(sample);
            
            // Write the updated package.json file
            File.WriteAllText(packageJsonPath, package.ToString());
        }

        /// <summary>
        /// Get samples from package.json. Returns null if no samples are found.
        /// </summary>
        /// <param name="packageInfo">The package name</param>
        /// <returns>An array of Sample objects or null if no samples are found.</returns>
        public static Sample[] GetSamplesFromPackage(PackageInfo packageInfo)
        {
            IEnumerable<Sample> samples = Sample.FindByPackage(packageInfo.name, packageInfo.version);

            // Return null if "samples" key is not found or in case of an error
            return samples?.ToArray();
        }
    }
}