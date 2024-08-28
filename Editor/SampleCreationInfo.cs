namespace SampleCreator.Editor
{
    public struct SampleCreationInfo
    {
        /// <summary>
        /// The display name of the sample, it will be used in the Samples~ folder
        /// </summary>
        public string DisplayName;
        
        /// <summary>
        /// The description of the sample
        /// </summary>
        public string Description;
        
        /// <summary>
        /// The path to be copied to the package folder in Samples~ folder.
        /// </summary>
        /// <remarks>The name folder of copied path will be renamed by <see cref="DisplayName"/> when it will be copied.</remarks>
        public string Path;
    }
}