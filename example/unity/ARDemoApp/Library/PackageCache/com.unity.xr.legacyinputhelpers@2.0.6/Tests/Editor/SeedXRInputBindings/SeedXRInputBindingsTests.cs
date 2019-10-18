using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEditor;

namespace UnityEditor.XR.LegacyInputHelpers.Tests
{

    [TestFixture]
    public class TestSeededInput
    {

        const int kNumOverlaps = 4; // we know there are four overlaps between the input asset, and the seeded assets.
        const int kNumDupesToKeep = 2; 

        [Test]
        public void SeededInput_FillsOutCompleteData()
        {

            // load the input asset
            var inputManagerAsset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
            var serializedObject = new SerializedObject(inputManagerAsset);
            var inputManagerCurrentData = serializedObject.FindProperty("m_Axes");

            // cache the number of items so we can reset.            
            int prevInputManagerSize = inputManagerCurrentData.arraySize;
            
            SeedXRInputBindings tsxib = new SeedXRInputBindings();

            Dictionary<string, SeedXRInputBindings.BindingData> axisMap = new Dictionary<string, SeedXRInputBindings.BindingData>();
            for (int i = 0; i < tsxib.axisList.Count; ++i)
            {
                axisMap.Add(tsxib.axisList[i].name, new SeedXRInputBindings.BindingData() { newDataIndex = i, exists = false, inputManagerIndex = -1 });
            }

            tsxib.GenerateXRBindings();

            inputManagerAsset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
            serializedObject = new SerializedObject(inputManagerAsset);
            inputManagerCurrentData = serializedObject.FindProperty("m_Axes");

            // did we create the right number of things?
            Assert.That(inputManagerCurrentData.arraySize == (prevInputManagerSize + tsxib.axisList.Count) - kNumOverlaps); // we subtract kNumOverlaps because we know there are 4 things duplicated and they shouldnt be included.

            List<SeedXRInputBindings.InputAxis> currentInputData = new List<SeedXRInputBindings.InputAxis>();

            tsxib.LoadExistingDataAndCheckAgainstNewData(inputManagerCurrentData, ref axisMap, ref currentInputData);

            // the axis map should now be true for every element.
            foreach(var item in axisMap)
            {
                Assert.That(item.Value.exists == true);
            }
            
            inputManagerCurrentData.arraySize = prevInputManagerSize;
            serializedObject.ApplyModifiedProperties();
            AssetDatabase.Refresh();
        }

        [Test]
        public void SeededInput_DoesntAddDuplicates()
        {       
            // load the input asset
            var inputManagerAsset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
            var serializedObject = new SerializedObject(inputManagerAsset);
            var inputManagerCurrentData = serializedObject.FindProperty("m_Axes");

            // cache the number of items so we can reset.            
            int prevInputManagerSize = inputManagerCurrentData.arraySize;
            inputManagerCurrentData.arraySize = 0;

            SeedXRInputBindings tsxib = new SeedXRInputBindings();

            Dictionary<string, SeedXRInputBindings.BindingData> axisMap = new Dictionary<string, SeedXRInputBindings.BindingData>();
            for (int i = 0; i < tsxib.axisList.Count; ++i)
            {
                axisMap.Add(tsxib.axisList[i].name, new SeedXRInputBindings.BindingData() { newDataIndex = i, exists = false, inputManagerIndex = -1 });
            }

            tsxib.GenerateXRBindings();

            // slam back the value to a smaller number        
            inputManagerAsset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
            serializedObject = new SerializedObject(inputManagerAsset);
            inputManagerCurrentData = serializedObject.FindProperty("m_Axes");
            // we want to maintain a few records to ensure that we dont add duplicates.
            inputManagerCurrentData.arraySize = kNumDupesToKeep;

            List<SeedXRInputBindings.InputAxis> currentInputData = new List<SeedXRInputBindings.InputAxis>();

            tsxib.LoadExistingDataAndCheckAgainstNewData(inputManagerCurrentData, ref axisMap, ref currentInputData);

            // now, we should only have two elements that match, seeing as we only left two items in the asset.
            int trueCount = 0;
            foreach (var item in axisMap)
            {
                if (item.Value.exists)
                    trueCount++;
            }
            Assert.That(trueCount == kNumDupesToKeep);

            tsxib.GenerateXRBindings();
            
            inputManagerAsset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
            serializedObject = new SerializedObject(inputManagerAsset);
            inputManagerCurrentData = serializedObject.FindProperty("m_Axes");
            Assert.That(inputManagerCurrentData.arraySize == (prevInputManagerSize + tsxib.axisList.Count) - kNumOverlaps); // we subtract kNumOverlaps because we know there are 4 things duplicated and they shouldnt be included.
            tsxib.LoadExistingDataAndCheckAgainstNewData(inputManagerCurrentData, ref axisMap, ref currentInputData);

            // the axis map should now be true for every element.
            foreach (var item in axisMap)
            {
                Assert.That(item.Value.exists == true);
            }

            inputManagerCurrentData.arraySize = prevInputManagerSize;
            serializedObject.ApplyModifiedProperties();
            AssetDatabase.Refresh();
        }

    }
}
