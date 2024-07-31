using System;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEditor.PackageManager.UI;

namespace SampleCreator.Editor
{
    public static class SampleHelper
    {
        public static void AddSampleToPackage(string packageJson, Sampledd sampledd)
        {
            // Search "samples" key in package.json 
            
        }

        /// <summary>
        /// Get samples from package.json. Returns null if no samples are found.
        /// </summary>
        /// <param name="packageJson">The package.json file as a string.</param>
        /// <returns>An array of Sample objects or null if no samples are found.</returns>
        public static Sample[] GetSamplesFromPackage(string packageJson)
        {
            try
            {
                //var samples = Sample.FindByPackage();


            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine($"JSON parsing error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            // Return null if "samples" key is not found or in case of an error
            return null;
        }
    }
}