using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.UI;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

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