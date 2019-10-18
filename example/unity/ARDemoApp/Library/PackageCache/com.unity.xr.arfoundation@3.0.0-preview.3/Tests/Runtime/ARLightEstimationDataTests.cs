using NUnit.Framework;
using System.Collections.Generic;

namespace UnityEngine.XR.ARFoundation
{
    [TestFixture]
    public class ARLightEstimationDataTestFixture
    {
        [Test]
        public void ARLightEstimationData_TestBrightnessConversion()
        {
            Dictionary<float, float> brightnessToLumensMapping = new Dictionary<float, float> {
                // {brightness (0 -> 1), intensity in lumens (0 -> 2000)}
                {0f, 0f},
                {-1f, 0f},
                {0.25f, 500f},
                {0.5f, 1000f},
                {0.75f, 1500f},
                {1f, 2000f},
                {1.3f, 2000f}
            };

            var obj = new ARLightEstimationData();
            foreach (var testPair in brightnessToLumensMapping)
            {
                // If intensity is not filled, expect it to be converted based on the brightness.
                obj.averageIntensityInLumens = null;
                obj.averageBrightness = testPair.Key;
                Assert.AreEqual(obj.averageIntensityInLumens, testPair.Value, "Conversion from averageBrightness to averageIntensityInLumens failed.");
            }

            obj.averageBrightness = 0.5f;
            obj.averageIntensityInLumens = 5000f;
            Assert.AreEqual(obj.averageBrightness, 0.5f, "If averageBrightness is set, no conversion should be performed.");
        }

        [Test]
        public void ARLightEstimationData_TestIntensityConversion()
        {
            Dictionary<float, float> brightnessToLumensMapping = new Dictionary<float, float> {
                // {intensity in lumens (0 -> 2000), brightness (0 -> 1)}
                {0f, 0f},
                {-100f, 0f},
                {500f, 0.25f},
                {1000f, 0.5f},
                {1500f, 0.75f},
                {2000f, 1f},
                {2500f, 1f}
            };

            var obj = new ARLightEstimationData();
            foreach (var testPair in brightnessToLumensMapping)
            {
                // If brightness is not filled, expect it to be converted based on the intensity.
                obj.averageIntensityInLumens = testPair.Key;
                obj.averageBrightness = null;
                Assert.AreEqual(obj.averageBrightness, testPair.Value, "Conversion from averageIntensityInLumens to averageBrightness failed.");
            }

            obj.averageBrightness = 0.5f;
            obj.averageIntensityInLumens = 5000f;
            Assert.AreEqual(obj.averageIntensityInLumens, 5000f, "If averageIntensityInLumens is set, no conversion should be performed.");
        }

        [Test]
        public void ARLightEstimationData_TestGetHashCode()
        {
            var obj1 = new ARLightEstimationData();
            var obj2 = new ARLightEstimationData();

            obj1.averageIntensityInLumens = 1115f;
            obj1.averageBrightness = null;
            obj2.averageIntensityInLumens = 1233f;
            obj2.averageBrightness = null;
            Assert.AreNotEqual(obj1.GetHashCode(), obj2.GetHashCode(), "Hash codes should differ when averageIntensityInLumens is different.");

            obj1.averageBrightness = 0.5f;
            obj1.averageIntensityInLumens = null;
            obj2.averageBrightness = 0.6f;
            obj2.averageIntensityInLumens = null;
            Assert.AreNotEqual(obj1.GetHashCode(), obj2.GetHashCode(), "Hash codes should differ when averageBrightness is different.");

            obj1.averageBrightness = 0.5f;
            obj1.averageIntensityInLumens = null;
            obj2.averageBrightness = 0.5f;
            obj2.averageIntensityInLumens = null;
            Assert.AreEqual(obj1.GetHashCode(), obj2.GetHashCode(), "Hash codes should match when averageBrightness is same.");
        }

        [Test]
        public void ARLightEstimationData_TestEquality()
        {
            var obj1 = new ARLightEstimationData();
            var obj2 = new ARLightEstimationData();
            Assert.AreEqual(obj1, obj2, "Freshly created ARLightEstimationData objects should match.");

            obj1.averageBrightness = 0.1f;
            obj2.averageBrightness = 0.5f;
            Assert.AreNotEqual(obj1, obj2, "ARLightEstimationData with different averageBrightness values should not match.");

            obj1.averageBrightness = 0.5f;
            obj2.averageBrightness = 0.5f;
            Assert.AreEqual(obj1, obj2, "ARLightEstimationData with same averageBrightness values should match.");

            obj1.averageBrightness = null;
            obj2.averageBrightness = 0.5f;
            Assert.AreNotEqual(obj1, obj2, "ARLightEstimationData with different averageBrightness values should not match.");

            obj1.averageIntensityInLumens = 1000;
            obj2.averageBrightness = 0.5f;
            Assert.AreEqual(obj1, obj2, "ARLightEstimationData with same calculated brightness values should match.");

            obj1.averageIntensityInLumens = 2000;
            obj2.averageBrightness = 0.5f;
            Assert.AreNotEqual(obj1, obj2, "ARLightEstimationData with different calculated brightness values should not match.");

            obj1.averageIntensityInLumens = null;
            obj1.averageBrightness = null;
            obj1.colorCorrection = new Color(1f, 0.5f, 0.5f, 1f);

            obj2.averageIntensityInLumens = null;
            obj2.averageBrightness = null;
            obj2.colorCorrection = new Color(1f, 0.5f, 0.5f, 1f);
            Assert.AreEqual(obj1, obj2, "ARLightEstimationData with same color correction should match.");

            obj1.averageIntensityInLumens = null;
            obj1.averageBrightness = null;
            obj1.averageColorTemperature = 5230f;

            obj2.averageIntensityInLumens = null;
            obj2.averageBrightness = null;
            obj2.averageColorTemperature = 5230f;
            Assert.AreEqual(obj1, obj2, "ARLightEstimationData with same color temperature should match.");

            obj1.averageIntensityInLumens = null;
            obj1.averageBrightness = null;
            obj1.averageColorTemperature = 5230f;
            obj1.colorCorrection = new Color(1f, 0.5f, 0.5f, 1f);

            obj2.averageIntensityInLumens = null;
            obj2.averageBrightness = null;
            obj2.averageColorTemperature = 5230f;
            obj2.colorCorrection = new Color(1f, 0.5f, 0.5f, 1f);
            Assert.AreEqual(obj1, obj2, "ARLightEstimationData with same properties should match.");

            obj1.averageIntensityInLumens = null;
            obj1.averageBrightness = null;
            obj1.averageColorTemperature = 5230f;
            obj1.colorCorrection = new Color(1f, 0.5f, 0.5f, 1f);

            obj2.averageIntensityInLumens = 2300;
            obj2.averageBrightness = 0.5f;
            obj2.averageColorTemperature = 1222f;
            obj2.colorCorrection = new Color(1f, 1f, 0.5f, 1f);
            Assert.AreNotEqual(obj1, obj2, "ARLightEstimationData with different properties should not match.");
        }
    }
}