using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("UnityEditor.XR.LegacyInputHelpers.Tests")]

namespace UnityEditor.XR.LegacyInputHelpers
{
    /// <summary>
    /// The SeedXRInputBindings class is used to populate the Input Asset with the cross platform bindings published by Unity for most XR Devices.
    /// </summary>
    public class SeedXRInputBindings 
    {
        #region InputAxisDataAndConfig
        // Same as InputAxis.h 
        internal class InputAxis
        {
            public string name = "";
            public string descriptiveName = "";
            public string descriptiveNegativeName = "";
            public string negativeButton = "";
            public string positiveButton = "";
            public string altNegativeButton = "";
            public string altPositiveButton = "";
            public float gravity = 0.0f;
            public float dead = 0.001f;
            public float sensitivity = 1.0f;
            public bool snap = false;
            public bool invert = false;
            public int type = 0;
            public int axis = 0;
            public int joyNum = 0;
        }

        //
        // NB: ALL AXIS VALUES WILL BE -1'd DURING PROCESSING, SO USE THE "REAL" AXIS VALUE
        //
        internal List<InputAxis> axisList = new List<InputAxis>
        {
            #region LeftHand
            //######################################################################################################################################
            // Left Hand
            //######################################################################################################################################  
            // Axis Data
            new InputAxis()
            {
                name = "XRI_Left_Primary2DAxis_Vertical",
                descriptiveName = "Device joystick/touchpad horizontal motion",
                dead = 0.19f,
                axis = 2,
                type = 2,
            },
            new InputAxis()
            {
                name = "XRI_Left_Primary2DAxis_Horizontal",
                descriptiveName = "Device joystick/touchpad horizontal motion",
                dead = 0.19f,
                axis = 1,
                type = 2,
            },
            new InputAxis()
            {
                name = "XRI_Left_Secondary2DAxis_Vertical",
                descriptiveName = "Device joystick/touchpad horizontal motion.",
                dead = 0.19f,
                axis = 18,
                type = 2,
            },
            new InputAxis()
            {
                name = "XRI_Left_Secondary2DAxis_Horizontal",
                descriptiveName = "Device joystick/touchpad horizontal motion",
                dead = 0.19f,
                axis = 17,
                type = 2,
            },
            new InputAxis()
            {
                name = "XRI_Left_Trigger",
                descriptiveName = "Device trigger axis",
                axis = 9,
                type = 2,
            },
            new InputAxis()
            {
                name = "XRI_Left_Grip",
                descriptiveName = "Device grip axis",
                axis = 11,
                type = 2,
            },
            new InputAxis()
            {
                name = "XRI_Left_IndexTouch",
                descriptiveName = "Device index finger proximity touch axis.",
                dead = 0.19f,
                axis = 13,
                type = 2,
            },
            new InputAxis()
            {
                name = "XRI_Left_ThumbTouch",
                descriptiveName = "Device thumb proximity touch axis",
                dead = 0.19f,
                axis = 15,
                type = 2,
            },
            // Button Data
            new InputAxis()
            {
                name = "XRI_Left_PrimaryButton",
                descriptiveName = "Device primary button",
                positiveButton = "joystick button 2",
                gravity = 1000.0f,
                sensitivity = 1000.0f,
                type = 0,
            },
            new InputAxis()
            {
                name = "XRI_Left_SecondaryButton",
                descriptiveName = "Device secondary button",
                positiveButton = "joystick button 3",
                gravity = 1000.0f,
                sensitivity = 1000.0f,
                type = 0,
            },
            new InputAxis()
            {
                name = "XRI_Left_PrimaryTouch",
                descriptiveName = "Device primary touch",
                positiveButton = "joystick button 12",
                gravity = 0.0f,
                dead = 0.0f,
                sensitivity = 0.1f,
                type = 0,
            },
            new InputAxis()
            {
                name = "XRI_Left_SecondaryTouch",
                descriptiveName = "Device secondary button",
                positiveButton = "joystick button 13",
                gravity = 0.0f,
                dead = 0.0f,
                sensitivity = 0.1f,
                type = 0,
            },
            new InputAxis()
            {
                name = "XRI_Left_GripButton",
                descriptiveName = "Device grip button",
                positiveButton = "joystick button 4",
                gravity = 0.0f,
                dead = 0.0f,
                sensitivity = 0.1f,
                type = 0,
            },
            new InputAxis()
            {
                name = "XRI_Left_TriggerButton",
                descriptiveName = "Device trigger button",
                positiveButton = "joystick button 14",
                gravity = 0.0f,
                dead = 0.0f,
                sensitivity = 0.1f,
                type = 0,
            },
            new InputAxis()
            {
                name = "XRI_Left_MenuButton",
                descriptiveName = "Device menu button",
                positiveButton = "joystick button 6",
                gravity = 1000.0f,
                sensitivity = 1000.0f,
                type = 0,
            },
            new InputAxis()
            {
                name = "XRI_Left_Primary2DAxisClick",
                descriptiveName = "Device stick/touchpad click",
                positiveButton = "joystick button 8",
                gravity = 0.0f,
                dead = 0.0f,
                sensitivity = 0.1f,
                type = 0,
            },
            new InputAxis()
            {
                name = "XRI_Left_Primary2DAxisTouch",
                descriptiveName = "Device stick/touchpad touch",
                positiveButton = "joystick button 16",
                gravity = 0.0f,
                dead = 0.0f,
                sensitivity = 0.1f,
                type = 0,
            },
            new InputAxis()
            {
                name = "XRI_Left_Thumbrest",
                descriptiveName = "Device thumbrest",
                positiveButton = "joystick button 18",
                gravity = 0.0f,
                dead = 0.0f,
                sensitivity = 0.1f,
                type = 0,
            },
            #endregion           
            #region RightHand
            //######################################################################################################################################
            // Right Hand
            //######################################################################################################################################
            new InputAxis()
            {
                name = "XRI_Right_Primary2DAxis_Vertical",
                descriptiveName = "Device joystick/touchpad horizontal motion",
                dead = 0.19f,
                axis = 5,
                type = 2,
            },
            new InputAxis()
            {
                name = "XRI_Right_Primary2DAxis_Horizontal",
                descriptiveName = "Device joystick/touchpad horizontal motion",
                dead = 0.19f,
                axis = 4,
                type = 2,
            },
            new InputAxis()
            {
                name = "XRI_Right_Secondary2DAxis_Vertical",
                descriptiveName = "Device joystick/touchpad horizontal motion.",
                dead = 0.19f,
                axis = 20,
                type = 2,
            },
            new InputAxis()
            {
                name = "XRI_Right_Secondary2DAxis_Horizontal",
                descriptiveName = "Device joystick/touchpad horizontal motion",
                dead = 0.19f,
                axis = 19,
                type = 2,
            },
            new InputAxis()
            {
                name = "XRI_Right_Trigger",
                descriptiveName = "Device trigger axis",
                axis = 10,
                type = 2,
            },
            new InputAxis()
            {
                name = "XRI_Right_Grip",
                descriptiveName = "Device grip axis",
                axis = 12,
                type = 2,
            },
            new InputAxis()
            {
                name = "XRI_Right_IndexTouch",
                descriptiveName = "Device index finger proximity touch axis.",
                dead = 0.19f,
                axis = 14,
                type = 2,
            },
            new InputAxis()
            {
                name = "XRI_Right_ThumbTouch",
                descriptiveName = "Device thumb proximity touch axis",
                dead = 0.19f,
                axis = 16,
                type = 2,
            },
            // Button Data
            new InputAxis()
            {
                name = "XRI_Right_PrimaryButton",
                descriptiveName = "Device primary button",
                positiveButton = "joystick button 0",
                gravity = 1000.0f,
                sensitivity = 1000.0f,
                type = 0,
            },
            new InputAxis()
            {
                name = "XRI_Right_SecondaryButton",
                descriptiveName = "Device secondary button",
                positiveButton = "joystick button 1",
                gravity = 1000.0f,
                sensitivity = 1000.0f,
                type = 0,
            },
            new InputAxis()
            {
                name = "XRI_Right_PrimaryTouch",
                descriptiveName = "Device primary touch",
                positiveButton = "joystick button 10",
                gravity = 0.0f,
                dead = 0.0f,
                sensitivity = 0.1f,
                type = 0,
            },
            new InputAxis()
            {
                name = "XRI_Right_SecondaryTouch",
                descriptiveName = "Device secondary button",
                positiveButton = "joystick button 11",
                gravity = 0.0f,
                dead = 0.0f,
                sensitivity = 0.1f,
                type = 0,
            },
            new InputAxis()
            {
                name = "XRI_Right_GripButton",
                descriptiveName = "Device grip button",
                positiveButton = "joystick button 5",
                gravity = 0.0f,
                dead = 0.0f,
                sensitivity = 0.1f,
                type = 0,
            },
            new InputAxis()
            {
                name = "XRI_Right_TriggerButton",
                descriptiveName = "Device trigger button",
                positiveButton = "joystick button 15",
                gravity = 0.0f,
                dead = 0.0f,
                sensitivity = 0.1f,
                type = 0,
            },
            new InputAxis()
            {
                name = "XRI_Right_MenuButton",
                descriptiveName = "Device menu button",
                positiveButton = "joystick button 7",
                gravity = 1000.0f,
                sensitivity = 1000.0f,
                type = 0,
            },
            new InputAxis()
            {
                name = "XRI_Right_Primary2DAxisClick",
                descriptiveName = "Device stick/touchpad click",
                positiveButton = "joystick button 9",
                gravity = 0.0f,
                dead = 0.0f,
                sensitivity = 0.1f,
                type = 0,
            },
            new InputAxis()
            {
                name = "XRI_Right_Primary2DAxisTouch",
                descriptiveName = "Device stick/touchpad touch",
                positiveButton = "joystick button 17",
                gravity = 0.0f,
                dead = 0.0f,
                sensitivity = 0.1f,
                type = 0,
            },
            new InputAxis()
            {
                name = "XRI_Right_Thumbrest",
                descriptiveName = "Device thumbrest",
                positiveButton = "joystick button 19",
                gravity = 0.0f,
                dead = 0.0f,
                sensitivity = 0.1f,
                type = 0,
            },
            #endregion           
            #region UGuiRequired
            //######################################################################################################################################
            // UGui Required
            //######################################################################################################################################
            new InputAxis()
            {
                name = "Submit",
                descriptiveName = "Submit",
                positiveButton = "joystick button 0",
                gravity = 0.0f,
                dead = 0.0f,
                sensitivity = 0.1f,
                type = 0,
            },
            new InputAxis()
            {
                name = "Cancel",
                descriptiveName = "Cancel",
                positiveButton = "joystick button 1",
                gravity = 0.0f,
                dead = 0.0f,
                sensitivity = 0.1f,
                type = 0,
            },            
            new InputAxis()
            {
                name = "Horizontal",
                descriptiveName = "Horizontal",
                dead = 0.19f,
                axis = 4,
                type = 2,
            },
            new InputAxis()
            {
                name = "Vertical",
                descriptiveName = "Vertical",
                dead = 0.19f,
                axis = 5,
                type = 2,
            },
            #endregion
            //######################################################################################################################################
            // Combined Trigger
            //######################################################################################################################################
            #region Combined
             new InputAxis()
            {
                name = "XRI_Combined_Trigger",
                descriptiveName = "Combined Trigger",
                dead = 0.19f,
                axis = 3,
                type = 2,
            },
            new InputAxis()
            {
                name = "XRI_DPad_Vertical",
                descriptiveName = "Device directional pad. These values are replicated l/r",
                axis = 7,
                type = 2,
            },
            new InputAxis()
            {
                name = "XRI_DPad_Horizontal",
                descriptiveName = "Device directional pad. These values are replicated l/r",
                axis = 6,
                type = 2,
            },
            #endregion
        };
        
        internal struct BindingData
        {
            public int newDataIndex;
            public int inputManagerIndex;
            public bool exists;
        }
        #endregion

        [MenuItem("Assets/Seed XR Input Bindings")]
        static public void GenerateXRBindingsMenuItem()
        {
            SeedXRInputBindings sxrib = new SeedXRInputBindings();
            sxrib.GenerateXRBindings();
            SettingsService.OpenProjectSettings("Project/Input");
        }

        /// <summary>
        /// Main entrypoint for generating the XR Bindings and adding them to the Input Asset. The Custom uGUI editor calls this function when the user wishes to
        /// seed the Input Asset with XR bindings.
        /// </summary>        
        public void GenerateXRBindings()
        {
            // seed map of axis data so we can whitewash against existing.        
            Dictionary<string, BindingData> axisMap = new Dictionary<string, BindingData>();
            for (int i = 0; i < axisList.Count; ++i)
            {
                axisMap.Add(axisList[i].name, new BindingData() { newDataIndex = i, exists = false, inputManagerIndex = -1 });
                if (axisList[i].axis > 0)
                {
                    axisList[i].axis--;
                }
            }

            // load the input asset
            var inputManagerAsset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
            if (inputManagerAsset != null)
            {
                var serializedObject = new SerializedObject(inputManagerAsset);
                var inputManagerCurrentData = serializedObject.FindProperty("m_Axes");

                if (inputManagerCurrentData != null)
                {
                    List<InputAxis> currentInputData = new List<InputAxis>();
                    LoadExistingDataAndCheckAgainstNewData(inputManagerCurrentData, ref axisMap, ref currentInputData);
                    serializedObject.ApplyModifiedProperties();

                    ApplyDataToInputManager(currentInputData, axisList, axisMap, ref inputManagerCurrentData);

                    serializedObject.ApplyModifiedProperties();
                    AssetDatabase.Refresh();
                }
            }

        }

        #region InternalProcessingCode

        internal void ApplyDataToInputManager(List<InputAxis> inputManagerData, List<InputAxis> newData, Dictionary<string, BindingData> newDataMap, ref SerializedProperty arrayRoot)
        {
            // likely will be larger than we need, but that's ok. it'll be big enough for all the data which is worst case
            arrayRoot.arraySize = inputManagerData.Count + newData.Count;

            int arrayIndex = inputManagerData.Count;
            // write everything that doesn't clash from our new data
            for (int i = 0; i < newData.Count; ++i)
            {
                BindingData bindingData;
                if (newDataMap.TryGetValue(newData[i].name, out bindingData))
                {
                    if (bindingData.exists == true)
                    {
                        continue;
                    }
                }
                var axisEntry = arrayRoot.GetArrayElementAtIndex(arrayIndex);
                WriteDataToInputAxis(newData[i], ref axisEntry);
                arrayIndex++;
            }
            arrayRoot.arraySize = arrayIndex;
        }

        internal void WriteDataToInputAxis(InputAxis sourceData, ref SerializedProperty serializedProperty)
        {
            var iteratorProperty = serializedProperty.Copy();
            iteratorProperty.Next(true);
            do
            {
                switch (iteratorProperty.name)
                {
                    case "m_Name":
                        iteratorProperty.stringValue = sourceData.name;
                        break;
                    case "descriptiveName":
                        iteratorProperty.stringValue = sourceData.descriptiveName;
                        break;
                    case "descriptiveNegativeName":
                        iteratorProperty.stringValue = sourceData.descriptiveNegativeName;
                        break;
                    case "negativeButton":
                        iteratorProperty.stringValue = sourceData.negativeButton;
                        break;
                    case "positiveButton":
                        iteratorProperty.stringValue = sourceData.positiveButton;
                        break;
                    case "altNegativeButton":
                        iteratorProperty.stringValue = sourceData.altNegativeButton;
                        break;
                    case "altPositiveButton":
                        iteratorProperty.stringValue = sourceData.altPositiveButton;
                        break;
                    case "gravity":
                        iteratorProperty.floatValue = sourceData.gravity;
                        break;
                    case "dead":
                        iteratorProperty.floatValue = sourceData.dead;
                        break;
                    case "sensitivity":
                        iteratorProperty.floatValue = sourceData.sensitivity;
                        break;
                    case "snap":
                        iteratorProperty.boolValue = sourceData.snap;
                        break;
                    case "invert":
                        iteratorProperty.boolValue = sourceData.invert;
                        break;
                    case "type":
                        iteratorProperty.intValue = sourceData.type;
                        break;
                    case "axis":
                        iteratorProperty.intValue = sourceData.axis;
                        break;
                    case "joyNum":
                        iteratorProperty.intValue = sourceData.joyNum;
                        break;

                }
            } while (iteratorProperty.Next(false));
        }

        internal void LoadExistingDataAndCheckAgainstNewData(SerializedProperty arrayRoot, ref Dictionary<string, BindingData> newDataMap, ref List<InputAxis> existingData)
        {
            existingData.Clear();
            for (int i = 0; i < arrayRoot.arraySize; ++i)
            {
                InputAxis readData = new InputAxis();

                var axisEntry = arrayRoot.GetArrayElementAtIndex(i);
                var iteratorProperty = axisEntry.Copy();
                iteratorProperty.Next(true);
                do
                {
                    switch (iteratorProperty.name)
                    {
                        case "m_Name":
                            readData.name = iteratorProperty.stringValue;
                            BindingData bindingData;
                            if (newDataMap.TryGetValue(readData.name, out bindingData))
                            {
                                // using TryGetElement returns a copy, not very useful.                           
                                bindingData.exists = true;
                                bindingData.inputManagerIndex = i;
                                newDataMap[readData.name] = bindingData;
                            }
                            break;
                        case "descriptiveName":
                            readData.descriptiveName = iteratorProperty.stringValue;
                            break;
                        case "descriptiveNegativeName":
                            readData.descriptiveNegativeName = iteratorProperty.stringValue;
                            break;
                        case "negativeButton":
                            readData.negativeButton = iteratorProperty.stringValue;
                            break;
                        case "positiveButton":
                            readData.positiveButton = iteratorProperty.stringValue;
                            break;
                        case "altNegativeButton":
                            readData.altNegativeButton = iteratorProperty.stringValue;
                            break;
                        case "altPositiveButton":
                            readData.altPositiveButton = iteratorProperty.stringValue;
                            break;
                        case "gravity":
                            readData.gravity = iteratorProperty.floatValue;
                            break;
                        case "dead":
                            readData.dead = iteratorProperty.floatValue;
                            break;
                        case "sensitivity":
                            readData.sensitivity = iteratorProperty.floatValue;
                            break;
                        case "snap":
                            readData.snap = iteratorProperty.boolValue;
                            break;
                        case "invert":
                            readData.invert = iteratorProperty.boolValue;
                            break;
                        case "type":
                            readData.type = iteratorProperty.intValue;
                            break;
                        case "axis":
                            readData.axis = iteratorProperty.intValue;
                            break;
                        case "joyNum":
                            readData.joyNum = iteratorProperty.intValue;
                            break;
                    }
                } while (iteratorProperty.Next(false));
                existingData.Add(readData);
            }
        }
        #endregion
    }
}