using System.Collections;
using NUnit.Framework;
using SampleCreator.Editor;
using UnityEngine;
using UnityEngine.TestTools;

public class SampleCreatorTest
{
    [Test]
    public void Sample_GetSample_Null()
    {
        string packageJson = @"
        {
          ""name"": ""com.clix.unityeventviewer"",
          ""displayName"": ""Unity Event Viewer"",
          ""version"": ""1.0.0"",
          ""unity"": ""2022.1"",
          ""description"": ""A tool to view Unity events in the editor from GameObject, Prefab and ScriptableObject."",
          ""keywords"": [
            ""unity"",
            ""UnityEvent""
          ],
          ""repository"": {
            ""url"": ""https://github.com/clixmods/UnityEvent-Viewer.git"",
            ""type"": ""git"",
            ""revision"": ""f110c8c230d253654afed153569030a587cc7557""
          },
          ""upmCi"": {
            ""footprint"": ""f9e81f42318ed88a56c8bf031a82ee5c30370e88""
          }
        }
        ";

        //Sampledd[] samples = SampleHelper.GetSamplesFromPackage(packageJson);

        //Assert.IsNull(samples);
    }

    [Test]
    public void Sample_GetSample_Found()
    {
        string packageJson = @"
        {
          ""name"": ""com.clix.unityeventviewer"",
          ""displayName"": ""Unity Event Viewer"",
          ""version"": ""1.0.0"",
          ""unity"": ""2022.1"",
          ""description"": ""A tool to view Unity events in the editor from GameObject, Prefab and ScriptableObject."",
          ""keywords"": [
            ""unity"",
            ""UnityEvent""
          ],
          ""repository"": {
            ""url"": ""https://github.com/clixmods/UnityEvent-Viewer.git"",
            ""type"": ""git"",
            ""revision"": ""f110c8c230d253654afed153569030a587cc7557""
          },
          ""upmCi"": {
            ""footprint"": ""f9e81f42318ed88a56c8bf031a82ee5c30370e88""
          }
          ""samples"": [
                  {
                      ""displayName"": ""HDRP Shaders"",
                      ""description"": ""Contains sample shaders for the High Definition render pipeline"",
                      ""path"": ""Samples~/SamplesHDRP""
                  }
              ]
        }
        ";

        // Sampledd[] samples = SampleHelper.GetSamplesFromPackage(packageJson);
        //
        // Assert.IsNotNull(samples);
        // Assert.AreEqual(1, samples.Length);
        // Assert.AreEqual("HDRP Shaders", samples[0].DisplayName);
        // Assert.AreEqual("Contains sample shaders for the High Definition render pipeline", samples[0].Description);
        // Assert.AreEqual("Samples~/SamplesHDRP", samples[0].Path);
    }
}