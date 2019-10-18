#include "il2cpp-config.h"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include <cstring>
#include <string.h>
#include <stdio.h>
#include <cmath>
#include <limits>
#include <assert.h>
#include <stdint.h>

#include "codegen/il2cpp-codegen.h"
#include "il2cpp-object-internals.h"

template <typename R>
struct VirtFuncInvoker0
{
	typedef R (*Func)(void*, const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeObject* obj)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		return ((Func)invokeData.methodPtr)(obj, invokeData.method);
	}
};
struct VirtActionInvoker0
{
	typedef void (*Action)(void*, const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeObject* obj)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		((Action)invokeData.methodPtr)(obj, invokeData.method);
	}
};
template <typename R, typename T1, typename T2, typename T3, typename T4>
struct VirtFuncInvoker4
{
	typedef R (*Func)(void*, T1, T2, T3, T4, const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1, T2 p2, T3 p3, T4 p4)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		return ((Func)invokeData.methodPtr)(obj, p1, p2, p3, p4, invokeData.method);
	}
};
template <typename R, typename T1, typename T2>
struct VirtFuncInvoker2
{
	typedef R (*Func)(void*, T1, T2, const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1, T2 p2)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		return ((Func)invokeData.methodPtr)(obj, p1, p2, invokeData.method);
	}
};
template <typename R, typename T1, typename T2, typename T3>
struct VirtFuncInvoker3
{
	typedef R (*Func)(void*, T1, T2, T3, const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1, T2 p2, T3 p3)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		return ((Func)invokeData.methodPtr)(obj, p1, p2, p3, invokeData.method);
	}
};
template <typename R, typename T1>
struct VirtFuncInvoker1
{
	typedef R (*Func)(void*, T1, const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		return ((Func)invokeData.methodPtr)(obj, p1, invokeData.method);
	}
};
template <typename T1>
struct VirtActionInvoker1
{
	typedef void (*Action)(void*, T1, const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		((Action)invokeData.methodPtr)(obj, p1, invokeData.method);
	}
};

// System.ArgumentNullException
struct ArgumentNullException_t581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD;
// System.Char[]
struct CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2;
// System.Collections.Generic.List`1<System.Object>
struct List_1_t05CC3C859AB5E6024394EF9A42E3E696628CA02D;
// System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceImage>
struct List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F;
// System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceObject>
struct List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79;
// System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceObjectEntry>
struct List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE;
// System.Collections.IDictionary
struct IDictionary_t1BD5C1546718A374EA8122FBD6C6EE45331E8CE7;
// System.Diagnostics.StackTrace[]
struct StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196;
// System.IndexOutOfRangeException
struct IndexOutOfRangeException_tEC7665FC66525AB6A6916A7EB505E5591683F0CF;
// System.IntPtr[]
struct IntPtrU5BU5D_t4DC01DCB9A6DF6C9792A6513595D7A11E637DCDD;
// System.InvalidOperationException
struct InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1;
// System.NotSupportedException
struct NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010;
// System.Object[]
struct ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A;
// System.Reflection.Binder
struct Binder_t4D5CB06963501D32847C057B57157D6DC49CA759;
// System.Reflection.MemberFilter
struct MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381;
// System.Runtime.Serialization.SafeSerializationManager
struct SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770;
// System.Security.Cryptography.RandomNumberGenerator
struct RandomNumberGenerator_t12277F7F965BA79C54E4B3BFABD27A5FFB725EE2;
// System.String
struct String_t;
// System.Type
struct Type_t;
// System.Type[]
struct TypeU5BU5D_t7FE623A666B49176DE123306221193E888A12F5F;
// System.Void
struct Void_t22962CB4C05B1D89B55A6E1139F0E87A90987017;
// UnityEngine.ISubsystemDescriptor
struct ISubsystemDescriptor_t44607A9BE5154ED0C7924B81776A709724545E6C;
// UnityEngine.Object
struct Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0;
// UnityEngine.ScriptableObject
struct ScriptableObject_tAB015486CEAB714DA0D5C1BA389B84FB90427734;
// UnityEngine.SubsystemDescriptor
struct SubsystemDescriptor_t889FE96B75F157379A8337541A119F08DD9C75D4;
// UnityEngine.SubsystemDescriptor`1<System.Object>
struct SubsystemDescriptor_1_t951C98846FCA6E211E4065CE53CF8AFB40DB5E24;
// UnityEngine.SubsystemDescriptor`1<UnityEngine.XR.ARSubsystems.XRRaycastSubsystem>
struct SubsystemDescriptor_1_t27A8F3D552922AA16C799BCC575330BC6DF59F47;
// UnityEngine.SubsystemDescriptor`1<UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem>
struct SubsystemDescriptor_1_tFFF984F9F6942FDAE965131A8954D4C4EA20B6B5;
// UnityEngine.SubsystemDescriptor`1<UnityEngine.XR.ARSubsystems.XRSessionSubsystem>
struct SubsystemDescriptor_1_t0679ED150419770F94E42102F28D688E8F1DC9DA;
// UnityEngine.Subsystem`1<System.Object>
struct Subsystem_1_t730D4B66354D4BECD6DEDE1050EA133DF9B7A61F;
// UnityEngine.Subsystem`1<UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor>
struct Subsystem_1_t749909AA5D71BD615BDF40BDE7C8F81B8D63F4C7;
// UnityEngine.Texture2D
struct Texture2D_tBBF96AC337723E2EF156DF17E09D4379FD05DE1C;
// UnityEngine.XR.ARSubsystems.Promise`1<System.Int32Enum>
struct Promise_1_tF9FBB5000BE390F6ECFC210DC39F175828FBA068;
// UnityEngine.XR.ARSubsystems.Promise`1<UnityEngine.XR.ARSubsystems.SessionAvailability>
struct Promise_1_t7E6FC51116CF1FB8FFF9D4D6621E5F7231AE9A6B;
// UnityEngine.XR.ARSubsystems.Promise`1<UnityEngine.XR.ARSubsystems.SessionInstallationStatus>
struct Promise_1_tE8B65103AAD59FA1265B1977CE59DC6DA5220626;
// UnityEngine.XR.ARSubsystems.TrackingSubsystem`2<UnityEngine.XR.ARSubsystems.XRReferencePoint,System.Object>
struct TrackingSubsystem_2_tA3D4B822865BAE0754B253CF8551A3EBB7073851;
// UnityEngine.XR.ARSubsystems.TrackingSubsystem`2<UnityEngine.XR.ARSubsystems.XRReferencePoint,UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor>
struct TrackingSubsystem_2_t62AFA2295FCEFC2C3818E3B9EDB3C1AF80509899;
// UnityEngine.XR.ARSubsystems.XRRaycastSubsystem
struct XRRaycastSubsystem_t1181EA314910ABB4E1F50BF2F7650EC1512A0A20;
// UnityEngine.XR.ARSubsystems.XRRaycastSubsystem/Provider
struct Provider_t1231BE7466AD9536EA5EB62866FF0B6F07452732;
// UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor
struct XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7;
// UnityEngine.XR.ARSubsystems.XRReferenceImageLibrary
struct XRReferenceImageLibrary_t38B21DC6650EADA892125F045DCBF71EBC3F6A8F;
// UnityEngine.XR.ARSubsystems.XRReferenceImage[]
struct XRReferenceImageU5BU5D_t1EEAB0EDA5828C38140B4D8D48E11247C4789BCE;
// UnityEngine.XR.ARSubsystems.XRReferenceObjectEntry
struct XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8;
// UnityEngine.XR.ARSubsystems.XRReferenceObjectEntry[]
struct XRReferenceObjectEntryU5BU5D_tE6BE009B13CF3D71DD2B512668EB369AA9120B26;
// UnityEngine.XR.ARSubsystems.XRReferenceObjectLibrary
struct XRReferenceObjectLibrary_tC9950535C1214232A691C32EDA0E95C703056260;
// UnityEngine.XR.ARSubsystems.XRReferenceObject[]
struct XRReferenceObjectU5BU5D_t0AF9158E0468305ABEAA331C8925977310FBD498;
// UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem
struct XRReferencePointSubsystem_tF175EC0188CC6EFB0A0633BA11FCA4D81A6A88E3;
// UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem/Provider
struct Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773;
// UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor
struct XRReferencePointSubsystemDescriptor_t5972C5E3DEF485A89A3AA7D9D47CCD8A67FDB74D;
// UnityEngine.XR.ARSubsystems.XRSessionSubsystem
struct XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0;
// UnityEngine.XR.ARSubsystems.XRSessionSubsystem/Provider
struct Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980;
// UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor
struct XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079;
// UnityEngine.XR.ARSubsystems.XRSubsystem`1<System.Object>
struct XRSubsystem_1_tF1AF6BF44AE813FB0C425AC60D3C9E9DF5904C3A;
// UnityEngine.XR.ARSubsystems.XRSubsystem`1<UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor>
struct XRSubsystem_1_tD0089F4B0849EA9C41ED01F79404C4A1A0B31B9B;
// UnityEngine.XR.ARSubsystems.XRSubsystem`1<UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor>
struct XRSubsystem_1_t4F81A9C71B4D3ABB1A7FF9080551E6E0951A5F5B;

IL2CPP_EXTERN_C RuntimeClass* ArgumentNullException_t581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Boolean_tB53F6830F670160873277339AA58F15CAED4399C_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Guid_t_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* IndexOutOfRangeException_tEC7665FC66525AB6A6916A7EB505E5591683F0CF_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Int32_t585191389E07734F19F3156FF88FB3EF4800D102_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* IntPtr_t_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* ScreenOrientation_t4AB8E2E02033B0EAEA0260B05B1D88DA8058BB51_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* SubsystemRegistration_t0A22FECC46483ABBFFC039449407F73FF11F5A1A_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* TextureFormat_t7C6B5101554065C47682E592D1E26079D4EC2DCE_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Type_t_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Vector2_tA85D2DD88578276CA8A8796756458277E72D073D_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* XRReferencePointSubsystemDescriptor_t5972C5E3DEF485A89A3AA7D9D47CCD8A67FDB74D_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C String_t* _stringLiteral39BF4FB45444043D19EFF93848D9F1A0BEA05065;
IL2CPP_EXTERN_C String_t* _stringLiteral40EB8A6D17A1FA9CEBAFE1F18BB8257C8CE3E703;
IL2CPP_EXTERN_C String_t* _stringLiteral4A5C099E77D1F0180583C811D9E0FFDBBD8056EE;
IL2CPP_EXTERN_C String_t* _stringLiteral74E2C3E4176EC7B2A7DD081FFCB26DBE573A880E;
IL2CPP_EXTERN_C String_t* _stringLiteral7C920AC9C27322B466EC79E3F70C59D0EB2E27E3;
IL2CPP_EXTERN_C String_t* _stringLiteral8D5D999C478CCB89B73D744A0781362458C30380;
IL2CPP_EXTERN_C String_t* _stringLiteral975146EEDDCE8BEE0A51FB0C58FD4F55FB3E29BF;
IL2CPP_EXTERN_C String_t* _stringLiteralB39A7E18F4E7B76A8C729E54E8852F98B5EA76F3;
IL2CPP_EXTERN_C String_t* _stringLiteralB803153B51A277297BD55B487E4635F22FE4D888;
IL2CPP_EXTERN_C String_t* _stringLiteralBAE9CCE613FB80AADF756BAC01EBC9DEF93DA2C0;
IL2CPP_EXTERN_C String_t* _stringLiteralC85048E55E2A449A7E41FC166F56CC576CF57166;
IL2CPP_EXTERN_C String_t* _stringLiteralC94A2BBDF45E627BA76EC101C53EBB78511EF25A;
IL2CPP_EXTERN_C String_t* _stringLiteralD0A3E7F81A9885E99049D1CAE0336D269D5E47A9;
IL2CPP_EXTERN_C String_t* _stringLiteralD60A29563E597D126763623865B515BACF0F6C94;
IL2CPP_EXTERN_C String_t* _stringLiteralD877CA1B88372977DE10E74458237B9D64D74F9B;
IL2CPP_EXTERN_C String_t* _stringLiteralDA39A3EE5E6B4B0D3255BFEF95601890AFD80709;
IL2CPP_EXTERN_C String_t* _stringLiteralF3CD8D2D9D73EC0CE1DAD87B8E5780B970B7AAAF;
IL2CPP_EXTERN_C const RuntimeMethod* Enumerator_Dispose_m7B6EFE249DF6F43B30BEE8350B87A4231CAA0406_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Enumerator_MoveNext_m0B347A783040C9CE4B9C1889A87ED80B3A0DCB8B_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Enumerator_get_Current_mD47F152CC824D5536C956CF7F08372836B4344C6_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* List_1_GetEnumerator_m7ABEC2AD664AC6AC1073E546BAAFC0350626A321_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* List_1_GetEnumerator_mC07E381535940BCF47E02BD6CA84103EBBBB217E_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* List_1_GetEnumerator_mD7B1224990EE97B1239AF20ACDA9FC21BCAEEBC5_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* List_1_IndexOf_m60FE5836AABB98EF6AD4A55ECB7085D854CA5E87_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* List_1_IndexOf_m80147D21315270B53CA3E7AE6FA3B97264231AB2_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* List_1__ctor_m370BA08A79B7950CA8E75862CD6F4921BAF6D7AB_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* List_1__ctor_m95E964C07FFF3EC898BA56E8B9ADE6E55ECEA0B9_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* List_1_get_Count_mA303D95DAA9717D8D3148C3F20FA1B2750CE77B4_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* List_1_get_Count_mC10370473AE5833D24A5F9361290A6B38CC73C32_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* List_1_get_Item_m0935B6D4B21A74A42474E6C822B5DA4F8A32FE00_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* List_1_get_Item_mE2BB4CDB177C0DA1EF1FB0A174459691F207C7DF_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* NativeArray_1_Dispose_m0A2104A4244A4841BC56C79F09040160C5209941_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* NativeArray_1_Dispose_m87C3176D32CFB61D46241114A0DAA158BA654620_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* NativeArray_1_Dispose_m922795C73F226D113E9B538E2D95D7423522B868_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* NativeArray_1_Equals_m2151C41783B1650AC28A718289E8D4FC5DAA2D42_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* NativeArray_1_Equals_m60C6EC22F3E5BF3FA454EE59E114A0E3610DCC41_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* NativeArray_1_Equals_mEC619E11BC3B323365C4E22C4493BE4F3FF49F27_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* NativeArray_1_GetHashCode_m3C1068B41EC1F4520ED44D2FD0E3E44F304099E1_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* NativeArray_1_GetHashCode_m4A3A547F590E7BCA81A2099A6ECA91A4C9D595F1_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* NativeArray_1_GetHashCode_m8DD866CBD1A50C075DA392F6DFDE1B9B7767DAD3_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* NativeArray_1_get_IsCreated_m871B990AAFB1ACE6BDFE2A895BA589B9E8C3769E_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* NativeArray_1_get_IsCreated_mA83E57BF410BDF5144D71C3C740F1BB04D255073_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* NativeArray_1_get_IsCreated_mC80A890C3A78C986492F0259406F63788CED59A1_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Nullable_1_get_HasValue_m86766498BBF2DC3AB0128F33AC3F200BF93BB1B6_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Nullable_1_get_Value_m763182B16D2A96848EAFACFA31AC9985EFCC0F30_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Promise_1_CreateResolvedPromise_m33AA652B2B2460A4777ED6EA73923156280826AA_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Promise_1_CreateResolvedPromise_m36CC1CB8A654C9849F7BB9FE6C69A725C78CC01D_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Provider_Raycast_m6063A859AC10ACF7F27A4AE0AAD82C3A0DF831AE_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Provider_Raycast_mF885EFF5FFD12196F53C9FE405EC523020C35CE2_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Provider_get_frameRate_m55D5E1C5FDF9ADAB43DE7E3F74C7A463ECA63F33_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Provider_set_matchFrameRate_mFB25AA4BD319D4968EEB29FB2A8EA8B79D4ED031_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* SubsystemDescriptor_1__ctor_m39ABF0C7663CF93F6B094FB18264078421C6950F_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* SubsystemDescriptor_1__ctor_mCE93F7D6739E1AADE5429632560447F18FA623F6_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* SubsystemDescriptor_1__ctor_mE7F10A17EF25AE3D50D0B7D5CAA54D07FE709FAB_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Subsystem_1_get_SubsystemDescriptor_mD9F3F4ECB180FF40D21E57235A6D775BDB014D9B_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* TrackingSubsystem_2__ctor_mC69AD6AEF92FA2B9D7D5B83C79A418BAB05D0F76_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* XRReferenceImageLibrary_get_Item_m9FE52A96359701129EC85E3AFB382E08E7E3B799_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* XRReferenceObject_FindEntry_mBCEBCEF4265B7D210FFA15179493BF8BDBB70C94_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* XRReferencePointSubsystem_GetChanges_mE0EC8049CED1EA604A751066DB97430E803BE487_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* XRSessionSubsystem_InstallAsync_m35E08EF7130491F2E498C990109FA7323A2ABCCC_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* XRSubsystem_1__ctor_m3D9DD5E9EFF90C8BD6855FB1E5FED02E410B19E6_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* XRSubsystem_1__ctor_m9E32FFF260FC17D2C65B47C162DD6B914C8867FB_RuntimeMethod_var;
IL2CPP_EXTERN_C const uint32_t Cinfo_Equals_m2118FE6BBF6355D645E72BDD6662CF313B8E94EB_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t Cinfo_Equals_m7D9C657E628D2DFC390587FBE0BDFEB1D5CDC92D_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t Cinfo_Equals_mA57BDAF996011C56A0017EFBBB805683CFA03452_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t Cinfo_Equals_mC210DE78D45CD980BF7B5A186241CFC5CC243D2E_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t Cinfo_Equals_mC668E44130D6114FFD62DA8470840EE5E39DBBB5_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t Cinfo_Equals_mC919333F29E857CC3F929451A080AC4A38385E01_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t Cinfo_GetHashCode_m06E1060A5995A0C346AB7C0E56CCAC4BEC758A6E_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t Cinfo_GetHashCode_mA1002FD6C9DDB0C39442B7692A7CDB5562C61086_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t Cinfo_GetHashCode_mCC56E718130099F9809415C6C1CE9DC981F06211_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t Cinfo_ToString_m9E094799A4E711569550478753C87EE9FC40DC24_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t Provider_GetAvailabilityAsync_m9CC6F74169601931E94DA1177C34A543048B0A01_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t Provider_InstallAsync_m074282442B17B260C246A17F8D7E1C4E947A6017_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t Provider_Raycast_m6063A859AC10ACF7F27A4AE0AAD82C3A0DF831AE_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t Provider_Raycast_mF885EFF5FFD12196F53C9FE405EC523020C35CE2_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t Provider_get_frameRate_m55D5E1C5FDF9ADAB43DE7E3F74C7A463ECA63F33_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t Provider_get_nativePtr_mEEB293C5FF1BBA91207EABA24752E9DF7A715609_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t Provider_get_sessionId_mD3AED2746B5F6920BD6BCC4234D6B9189CCF4BFA_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t Provider_set_matchFrameRate_mFB25AA4BD319D4968EEB29FB2A8EA8B79D4ED031_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t TrackableId_get_invalidId_mBE9FA1EC8F2EC1575C1B31666EA928A3382DF1CDUnity_XR_ARSubsystems1_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRPointCloudData_Dispose_mDF78595F088472E60327A1D366AA787C68A3EDE3_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRPointCloudData_Equals_mC870B135A9697D1A2AFB40892E70D7D403590E2A_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRPointCloudData_Equals_mDE5097D689526E5461CBBC48C36E6221F71F7598_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRPointCloudData_GetHashCode_mA67A28CD8661AAE597D4466135AF1750D2569409_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRPointCloudData_ToString_m3790E45AE87D2C6F63D664EF736F530B7A4FCB4D_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRRaycastHit_Equals_m0A24B5C58B6CA930CDD05F6F17F54FD60DA10DE5_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRRaycastHit__cctor_m304374EB65F3AE9EFA5D8418B9CF3CE8A90B752B_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRRaycastHit_get_defaultValue_m17AEBDAC971A56C3FC4C7C4E2E14ECC357658DFAUnity_XR_ARSubsystems1_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRRaycastHit_get_defaultValue_m17AEBDAC971A56C3FC4C7C4E2E14ECC357658DFA_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRRaycastSubsystemDescriptor_RegisterDescriptor_mFA32B9879B902AA46943CF8809094299062A41DB_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRRaycastSubsystemDescriptor__ctor_m449AFD6137C639FB21F17C456B20BC954875BC9E_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRRaycastSubsystem_Raycast_m46598C4ACA7D6AC6B6DA53A92ED1349F327EC6BF_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRRaycastSubsystem_Raycast_mD6335AB75E7AD15295138215F593EAB71754E6FA_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRRaycastSubsystem__ctor_mE8BD2BFB3AFD44403F3A663CA5D5AAC707419506_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferenceImageLibrary_GetEnumerator_m7F1F01CC0BD4EF373F5B3766D2D1476901C4DCFD_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferenceImageLibrary__ctor_m783C31BF895E269078A6F82966CE008024A5450D_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferenceImageLibrary_get_Item_m9FE52A96359701129EC85E3AFB382E08E7E3B799_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferenceImageLibrary_get_count_m031E8D7C22B586EE44B0256912F688867E9DEB84_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferenceImageLibrary_indexOf_m6E722FBAD970A7FB4C9415EDF6F46BAA25B89116_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferenceImage_Equals_m90DF560ECB15F4363E40AF3C3A2F82F5E0FD147D_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferenceImage_ToString_m37745D8B95903B29F917CE6E0A141E79E6F4B937_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferenceImage__ctor_m6B1ECFC5354FC9FDC73635BF5E693DA33DE02A4B_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferenceObjectLibrary_GetEnumerator_m7ECEFBA77B4669ADD3EDCE0841E4FF9C79C3312D_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferenceObjectLibrary__ctor_mA11977E1ED613E7E7EF79C3F2494E243F619C442_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferenceObjectLibrary_get_Item_m09F2CB2A14D830F37053B8B6E02E9659F3527996_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferenceObjectLibrary_get_count_mAB2D34091CFC65B152FD3FD7095480FF5D6CC99C_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferenceObjectLibrary_indexOf_mBF56521C68737F5FBBA77684AD76A78E19C5B1F0_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferenceObject_Equals_mC83DEBBA89CBF9EF334B79634C9CA099B166DBC9_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferenceObject_FindEntry_mBCEBCEF4265B7D210FFA15179493BF8BDBB70C94_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferencePointSubsystemDescriptor_Create_m4E9D9DF5FCE2FE3F8672653BC733F87D3A4327D0_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferencePointSubsystemDescriptor__ctor_mB5AD6FF2521FF148612D7662DF93BD6CA68069B2_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferencePointSubsystem_GetChanges_mE0EC8049CED1EA604A751066DB97430E803BE487_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferencePointSubsystem__ctor_mD93381DE24CA18A7BA022014E77BEBA9AA2CD6E9_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferencePoint_Equals_mD22BFD6609737E5CC6A31D2C1B519CD5207C89BC_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferencePoint__cctor_m3A7635582CA05369AD6537861329B761A106DB82_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferencePoint__ctor_mF823D8DC414C0B110A0C29F28668C1F3DA2B2997_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferencePoint_get_defaultValue_mCCFAF4140E24AC2FDF1C8D19043E57B6BFEAC0ADUnity_XR_ARSubsystems1_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRReferencePoint_get_defaultValue_mCCFAF4140E24AC2FDF1C8D19043E57B6BFEAC0AD_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRSessionSubsystemDescriptor_RegisterDescriptor_m7A9F84E8A57323CDB5DC415BA05E72D6A72025E4_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRSessionSubsystemDescriptor__ctor_mF2A65C6A814FB2D22D5ED1608E5EFD5B0CD9A6E2_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRSessionSubsystem_InstallAsync_m35E08EF7130491F2E498C990109FA7323A2ABCCC_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRSessionSubsystem__ctor_m2817E6FDD974187708CFD270DE4C6D3132774648_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRSessionUpdateParams_Equals_m415AB0E24C9CF0C013872ED16C571B65DACF24B1_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRSessionUpdateParams_ToString_m7150FEAE08C59544392C3D47B3CB5AC318B82F4A_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRTextureDescriptor_Equals_m8D2E1A6303E60A653F70870CBD04845414F6A0A5_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRTextureDescriptor_Reset_m64A787FBD1F11161369A72A7D61763DDF8D74EBC_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRTextureDescriptor_ToString_mA7C17125D54876E04397C7022031B6A346CA9A7F_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRTextureDescriptor_get_valid_mF060565C5E24FDF97771F6FDA3235562DF01977B_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRTrackedImage_Equals_m7C7F0B2FC7A6818276C2BC763CF0465333453B9C_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRTrackedImage__cctor_m4D42652FA025B44DA4EEAF27F15B77E11DAF4614_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRTrackedImage_get_defaultValue_mC27C0C8BAC99DFBD1900C92FBA0D4940D86468EE_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRTrackedObject_Equals_mF0CA07E970C48D514F2B9BBEC0FE44F46429C524_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRTrackedObject__cctor_mF6797A036790C2B6133B8B8A44C64B49FDBFF296_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t XRTrackedObject_get_defaultValue_m4623361129019EE5722A95C580171705EA1F3901_MetadataUsageId;
struct Exception_t_marshaled_com;
struct Exception_t_marshaled_pinvoke;

struct ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A;
struct XRReferenceImageU5BU5D_t1EEAB0EDA5828C38140B4D8D48E11247C4789BCE;
struct XRReferenceObjectU5BU5D_t0AF9158E0468305ABEAA331C8925977310FBD498;

IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END

#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Object

struct Il2CppArrayBounds;

// System.Array


// System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceImage>
struct  List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F  : public RuntimeObject
{
public:
	// T[] System.Collections.Generic.List`1::_items
	XRReferenceImageU5BU5D_t1EEAB0EDA5828C38140B4D8D48E11247C4789BCE* ____items_1;
	// System.Int32 System.Collections.Generic.List`1::_size
	int32_t ____size_2;
	// System.Int32 System.Collections.Generic.List`1::_version
	int32_t ____version_3;
	// System.Object System.Collections.Generic.List`1::_syncRoot
	RuntimeObject * ____syncRoot_4;

public:
	inline static int32_t get_offset_of__items_1() { return static_cast<int32_t>(offsetof(List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F, ____items_1)); }
	inline XRReferenceImageU5BU5D_t1EEAB0EDA5828C38140B4D8D48E11247C4789BCE* get__items_1() const { return ____items_1; }
	inline XRReferenceImageU5BU5D_t1EEAB0EDA5828C38140B4D8D48E11247C4789BCE** get_address_of__items_1() { return &____items_1; }
	inline void set__items_1(XRReferenceImageU5BU5D_t1EEAB0EDA5828C38140B4D8D48E11247C4789BCE* value)
	{
		____items_1 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____items_1), (void*)value);
	}

	inline static int32_t get_offset_of__size_2() { return static_cast<int32_t>(offsetof(List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F, ____size_2)); }
	inline int32_t get__size_2() const { return ____size_2; }
	inline int32_t* get_address_of__size_2() { return &____size_2; }
	inline void set__size_2(int32_t value)
	{
		____size_2 = value;
	}

	inline static int32_t get_offset_of__version_3() { return static_cast<int32_t>(offsetof(List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F, ____version_3)); }
	inline int32_t get__version_3() const { return ____version_3; }
	inline int32_t* get_address_of__version_3() { return &____version_3; }
	inline void set__version_3(int32_t value)
	{
		____version_3 = value;
	}

	inline static int32_t get_offset_of__syncRoot_4() { return static_cast<int32_t>(offsetof(List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F, ____syncRoot_4)); }
	inline RuntimeObject * get__syncRoot_4() const { return ____syncRoot_4; }
	inline RuntimeObject ** get_address_of__syncRoot_4() { return &____syncRoot_4; }
	inline void set__syncRoot_4(RuntimeObject * value)
	{
		____syncRoot_4 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____syncRoot_4), (void*)value);
	}
};

struct List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F_StaticFields
{
public:
	// T[] System.Collections.Generic.List`1::_emptyArray
	XRReferenceImageU5BU5D_t1EEAB0EDA5828C38140B4D8D48E11247C4789BCE* ____emptyArray_5;

public:
	inline static int32_t get_offset_of__emptyArray_5() { return static_cast<int32_t>(offsetof(List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F_StaticFields, ____emptyArray_5)); }
	inline XRReferenceImageU5BU5D_t1EEAB0EDA5828C38140B4D8D48E11247C4789BCE* get__emptyArray_5() const { return ____emptyArray_5; }
	inline XRReferenceImageU5BU5D_t1EEAB0EDA5828C38140B4D8D48E11247C4789BCE** get_address_of__emptyArray_5() { return &____emptyArray_5; }
	inline void set__emptyArray_5(XRReferenceImageU5BU5D_t1EEAB0EDA5828C38140B4D8D48E11247C4789BCE* value)
	{
		____emptyArray_5 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____emptyArray_5), (void*)value);
	}
};


// System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceObject>
struct  List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79  : public RuntimeObject
{
public:
	// T[] System.Collections.Generic.List`1::_items
	XRReferenceObjectU5BU5D_t0AF9158E0468305ABEAA331C8925977310FBD498* ____items_1;
	// System.Int32 System.Collections.Generic.List`1::_size
	int32_t ____size_2;
	// System.Int32 System.Collections.Generic.List`1::_version
	int32_t ____version_3;
	// System.Object System.Collections.Generic.List`1::_syncRoot
	RuntimeObject * ____syncRoot_4;

public:
	inline static int32_t get_offset_of__items_1() { return static_cast<int32_t>(offsetof(List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79, ____items_1)); }
	inline XRReferenceObjectU5BU5D_t0AF9158E0468305ABEAA331C8925977310FBD498* get__items_1() const { return ____items_1; }
	inline XRReferenceObjectU5BU5D_t0AF9158E0468305ABEAA331C8925977310FBD498** get_address_of__items_1() { return &____items_1; }
	inline void set__items_1(XRReferenceObjectU5BU5D_t0AF9158E0468305ABEAA331C8925977310FBD498* value)
	{
		____items_1 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____items_1), (void*)value);
	}

	inline static int32_t get_offset_of__size_2() { return static_cast<int32_t>(offsetof(List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79, ____size_2)); }
	inline int32_t get__size_2() const { return ____size_2; }
	inline int32_t* get_address_of__size_2() { return &____size_2; }
	inline void set__size_2(int32_t value)
	{
		____size_2 = value;
	}

	inline static int32_t get_offset_of__version_3() { return static_cast<int32_t>(offsetof(List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79, ____version_3)); }
	inline int32_t get__version_3() const { return ____version_3; }
	inline int32_t* get_address_of__version_3() { return &____version_3; }
	inline void set__version_3(int32_t value)
	{
		____version_3 = value;
	}

	inline static int32_t get_offset_of__syncRoot_4() { return static_cast<int32_t>(offsetof(List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79, ____syncRoot_4)); }
	inline RuntimeObject * get__syncRoot_4() const { return ____syncRoot_4; }
	inline RuntimeObject ** get_address_of__syncRoot_4() { return &____syncRoot_4; }
	inline void set__syncRoot_4(RuntimeObject * value)
	{
		____syncRoot_4 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____syncRoot_4), (void*)value);
	}
};

struct List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79_StaticFields
{
public:
	// T[] System.Collections.Generic.List`1::_emptyArray
	XRReferenceObjectU5BU5D_t0AF9158E0468305ABEAA331C8925977310FBD498* ____emptyArray_5;

public:
	inline static int32_t get_offset_of__emptyArray_5() { return static_cast<int32_t>(offsetof(List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79_StaticFields, ____emptyArray_5)); }
	inline XRReferenceObjectU5BU5D_t0AF9158E0468305ABEAA331C8925977310FBD498* get__emptyArray_5() const { return ____emptyArray_5; }
	inline XRReferenceObjectU5BU5D_t0AF9158E0468305ABEAA331C8925977310FBD498** get_address_of__emptyArray_5() { return &____emptyArray_5; }
	inline void set__emptyArray_5(XRReferenceObjectU5BU5D_t0AF9158E0468305ABEAA331C8925977310FBD498* value)
	{
		____emptyArray_5 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____emptyArray_5), (void*)value);
	}
};


// System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceObjectEntry>
struct  List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE  : public RuntimeObject
{
public:
	// T[] System.Collections.Generic.List`1::_items
	XRReferenceObjectEntryU5BU5D_tE6BE009B13CF3D71DD2B512668EB369AA9120B26* ____items_1;
	// System.Int32 System.Collections.Generic.List`1::_size
	int32_t ____size_2;
	// System.Int32 System.Collections.Generic.List`1::_version
	int32_t ____version_3;
	// System.Object System.Collections.Generic.List`1::_syncRoot
	RuntimeObject * ____syncRoot_4;

public:
	inline static int32_t get_offset_of__items_1() { return static_cast<int32_t>(offsetof(List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE, ____items_1)); }
	inline XRReferenceObjectEntryU5BU5D_tE6BE009B13CF3D71DD2B512668EB369AA9120B26* get__items_1() const { return ____items_1; }
	inline XRReferenceObjectEntryU5BU5D_tE6BE009B13CF3D71DD2B512668EB369AA9120B26** get_address_of__items_1() { return &____items_1; }
	inline void set__items_1(XRReferenceObjectEntryU5BU5D_tE6BE009B13CF3D71DD2B512668EB369AA9120B26* value)
	{
		____items_1 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____items_1), (void*)value);
	}

	inline static int32_t get_offset_of__size_2() { return static_cast<int32_t>(offsetof(List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE, ____size_2)); }
	inline int32_t get__size_2() const { return ____size_2; }
	inline int32_t* get_address_of__size_2() { return &____size_2; }
	inline void set__size_2(int32_t value)
	{
		____size_2 = value;
	}

	inline static int32_t get_offset_of__version_3() { return static_cast<int32_t>(offsetof(List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE, ____version_3)); }
	inline int32_t get__version_3() const { return ____version_3; }
	inline int32_t* get_address_of__version_3() { return &____version_3; }
	inline void set__version_3(int32_t value)
	{
		____version_3 = value;
	}

	inline static int32_t get_offset_of__syncRoot_4() { return static_cast<int32_t>(offsetof(List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE, ____syncRoot_4)); }
	inline RuntimeObject * get__syncRoot_4() const { return ____syncRoot_4; }
	inline RuntimeObject ** get_address_of__syncRoot_4() { return &____syncRoot_4; }
	inline void set__syncRoot_4(RuntimeObject * value)
	{
		____syncRoot_4 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____syncRoot_4), (void*)value);
	}
};

struct List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE_StaticFields
{
public:
	// T[] System.Collections.Generic.List`1::_emptyArray
	XRReferenceObjectEntryU5BU5D_tE6BE009B13CF3D71DD2B512668EB369AA9120B26* ____emptyArray_5;

public:
	inline static int32_t get_offset_of__emptyArray_5() { return static_cast<int32_t>(offsetof(List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE_StaticFields, ____emptyArray_5)); }
	inline XRReferenceObjectEntryU5BU5D_tE6BE009B13CF3D71DD2B512668EB369AA9120B26* get__emptyArray_5() const { return ____emptyArray_5; }
	inline XRReferenceObjectEntryU5BU5D_tE6BE009B13CF3D71DD2B512668EB369AA9120B26** get_address_of__emptyArray_5() { return &____emptyArray_5; }
	inline void set__emptyArray_5(XRReferenceObjectEntryU5BU5D_tE6BE009B13CF3D71DD2B512668EB369AA9120B26* value)
	{
		____emptyArray_5 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____emptyArray_5), (void*)value);
	}
};


// System.Reflection.MemberInfo
struct  MemberInfo_t  : public RuntimeObject
{
public:

public:
};


// System.String
struct  String_t  : public RuntimeObject
{
public:
	// System.Int32 System.String::m_stringLength
	int32_t ___m_stringLength_0;
	// System.Char System.String::m_firstChar
	Il2CppChar ___m_firstChar_1;

public:
	inline static int32_t get_offset_of_m_stringLength_0() { return static_cast<int32_t>(offsetof(String_t, ___m_stringLength_0)); }
	inline int32_t get_m_stringLength_0() const { return ___m_stringLength_0; }
	inline int32_t* get_address_of_m_stringLength_0() { return &___m_stringLength_0; }
	inline void set_m_stringLength_0(int32_t value)
	{
		___m_stringLength_0 = value;
	}

	inline static int32_t get_offset_of_m_firstChar_1() { return static_cast<int32_t>(offsetof(String_t, ___m_firstChar_1)); }
	inline Il2CppChar get_m_firstChar_1() const { return ___m_firstChar_1; }
	inline Il2CppChar* get_address_of_m_firstChar_1() { return &___m_firstChar_1; }
	inline void set_m_firstChar_1(Il2CppChar value)
	{
		___m_firstChar_1 = value;
	}
};

struct String_t_StaticFields
{
public:
	// System.String System.String::Empty
	String_t* ___Empty_5;

public:
	inline static int32_t get_offset_of_Empty_5() { return static_cast<int32_t>(offsetof(String_t_StaticFields, ___Empty_5)); }
	inline String_t* get_Empty_5() const { return ___Empty_5; }
	inline String_t** get_address_of_Empty_5() { return &___Empty_5; }
	inline void set_Empty_5(String_t* value)
	{
		___Empty_5 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___Empty_5), (void*)value);
	}
};


// System.ValueType
struct  ValueType_t4D0C27076F7C36E76190FB3328E232BCB1CD1FFF  : public RuntimeObject
{
public:

public:
};

// Native definition for P/Invoke marshalling of System.ValueType
struct ValueType_t4D0C27076F7C36E76190FB3328E232BCB1CD1FFF_marshaled_pinvoke
{
};
// Native definition for COM marshalling of System.ValueType
struct ValueType_t4D0C27076F7C36E76190FB3328E232BCB1CD1FFF_marshaled_com
{
};

// UnityEngine.CustomYieldInstruction
struct  CustomYieldInstruction_t819BB0973AFF22766749FF087B8AEFEAF3C2CB7D  : public RuntimeObject
{
public:

public:
};


// UnityEngine.Subsystem
struct  Subsystem_t6B987736D8E48098F860AC55D76905DE1F48CE8C  : public RuntimeObject
{
public:
	// UnityEngine.ISubsystemDescriptor UnityEngine.Subsystem::m_subsystemDescriptor
	RuntimeObject* ___m_subsystemDescriptor_0;

public:
	inline static int32_t get_offset_of_m_subsystemDescriptor_0() { return static_cast<int32_t>(offsetof(Subsystem_t6B987736D8E48098F860AC55D76905DE1F48CE8C, ___m_subsystemDescriptor_0)); }
	inline RuntimeObject* get_m_subsystemDescriptor_0() const { return ___m_subsystemDescriptor_0; }
	inline RuntimeObject** get_address_of_m_subsystemDescriptor_0() { return &___m_subsystemDescriptor_0; }
	inline void set_m_subsystemDescriptor_0(RuntimeObject* value)
	{
		___m_subsystemDescriptor_0 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_subsystemDescriptor_0), (void*)value);
	}
};


// UnityEngine.SubsystemDescriptor
struct  SubsystemDescriptor_t889FE96B75F157379A8337541A119F08DD9C75D4  : public RuntimeObject
{
public:
	// System.String UnityEngine.SubsystemDescriptor::<id>k__BackingField
	String_t* ___U3CidU3Ek__BackingField_0;
	// System.Type UnityEngine.SubsystemDescriptor::<subsystemImplementationType>k__BackingField
	Type_t * ___U3CsubsystemImplementationTypeU3Ek__BackingField_1;

public:
	inline static int32_t get_offset_of_U3CidU3Ek__BackingField_0() { return static_cast<int32_t>(offsetof(SubsystemDescriptor_t889FE96B75F157379A8337541A119F08DD9C75D4, ___U3CidU3Ek__BackingField_0)); }
	inline String_t* get_U3CidU3Ek__BackingField_0() const { return ___U3CidU3Ek__BackingField_0; }
	inline String_t** get_address_of_U3CidU3Ek__BackingField_0() { return &___U3CidU3Ek__BackingField_0; }
	inline void set_U3CidU3Ek__BackingField_0(String_t* value)
	{
		___U3CidU3Ek__BackingField_0 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___U3CidU3Ek__BackingField_0), (void*)value);
	}

	inline static int32_t get_offset_of_U3CsubsystemImplementationTypeU3Ek__BackingField_1() { return static_cast<int32_t>(offsetof(SubsystemDescriptor_t889FE96B75F157379A8337541A119F08DD9C75D4, ___U3CsubsystemImplementationTypeU3Ek__BackingField_1)); }
	inline Type_t * get_U3CsubsystemImplementationTypeU3Ek__BackingField_1() const { return ___U3CsubsystemImplementationTypeU3Ek__BackingField_1; }
	inline Type_t ** get_address_of_U3CsubsystemImplementationTypeU3Ek__BackingField_1() { return &___U3CsubsystemImplementationTypeU3Ek__BackingField_1; }
	inline void set_U3CsubsystemImplementationTypeU3Ek__BackingField_1(Type_t * value)
	{
		___U3CsubsystemImplementationTypeU3Ek__BackingField_1 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___U3CsubsystemImplementationTypeU3Ek__BackingField_1), (void*)value);
	}
};


// UnityEngine.XR.ARSubsystems.XRRaycastSubsystem_Provider
struct  Provider_t1231BE7466AD9536EA5EB62866FF0B6F07452732  : public RuntimeObject
{
public:

public:
};


// UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem_Provider
struct  Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773  : public RuntimeObject
{
public:

public:
};


// UnityEngine.XR.ARSubsystems.XRSessionSubsystem_Provider
struct  Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980  : public RuntimeObject
{
public:

public:
};


// System.Boolean
struct  Boolean_tB53F6830F670160873277339AA58F15CAED4399C 
{
public:
	// System.Boolean System.Boolean::m_value
	bool ___m_value_0;

public:
	inline static int32_t get_offset_of_m_value_0() { return static_cast<int32_t>(offsetof(Boolean_tB53F6830F670160873277339AA58F15CAED4399C, ___m_value_0)); }
	inline bool get_m_value_0() const { return ___m_value_0; }
	inline bool* get_address_of_m_value_0() { return &___m_value_0; }
	inline void set_m_value_0(bool value)
	{
		___m_value_0 = value;
	}
};

struct Boolean_tB53F6830F670160873277339AA58F15CAED4399C_StaticFields
{
public:
	// System.String System.Boolean::TrueString
	String_t* ___TrueString_5;
	// System.String System.Boolean::FalseString
	String_t* ___FalseString_6;

public:
	inline static int32_t get_offset_of_TrueString_5() { return static_cast<int32_t>(offsetof(Boolean_tB53F6830F670160873277339AA58F15CAED4399C_StaticFields, ___TrueString_5)); }
	inline String_t* get_TrueString_5() const { return ___TrueString_5; }
	inline String_t** get_address_of_TrueString_5() { return &___TrueString_5; }
	inline void set_TrueString_5(String_t* value)
	{
		___TrueString_5 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___TrueString_5), (void*)value);
	}

	inline static int32_t get_offset_of_FalseString_6() { return static_cast<int32_t>(offsetof(Boolean_tB53F6830F670160873277339AA58F15CAED4399C_StaticFields, ___FalseString_6)); }
	inline String_t* get_FalseString_6() const { return ___FalseString_6; }
	inline String_t** get_address_of_FalseString_6() { return &___FalseString_6; }
	inline void set_FalseString_6(String_t* value)
	{
		___FalseString_6 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___FalseString_6), (void*)value);
	}
};


// System.Collections.Generic.List`1_Enumerator<System.Object>
struct  Enumerator_tE0C99528D3DCE5863566CE37BD94162A4C0431CD 
{
public:
	// System.Collections.Generic.List`1<T> System.Collections.Generic.List`1_Enumerator::list
	List_1_t05CC3C859AB5E6024394EF9A42E3E696628CA02D * ___list_0;
	// System.Int32 System.Collections.Generic.List`1_Enumerator::index
	int32_t ___index_1;
	// System.Int32 System.Collections.Generic.List`1_Enumerator::version
	int32_t ___version_2;
	// T System.Collections.Generic.List`1_Enumerator::current
	RuntimeObject * ___current_3;

public:
	inline static int32_t get_offset_of_list_0() { return static_cast<int32_t>(offsetof(Enumerator_tE0C99528D3DCE5863566CE37BD94162A4C0431CD, ___list_0)); }
	inline List_1_t05CC3C859AB5E6024394EF9A42E3E696628CA02D * get_list_0() const { return ___list_0; }
	inline List_1_t05CC3C859AB5E6024394EF9A42E3E696628CA02D ** get_address_of_list_0() { return &___list_0; }
	inline void set_list_0(List_1_t05CC3C859AB5E6024394EF9A42E3E696628CA02D * value)
	{
		___list_0 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___list_0), (void*)value);
	}

	inline static int32_t get_offset_of_index_1() { return static_cast<int32_t>(offsetof(Enumerator_tE0C99528D3DCE5863566CE37BD94162A4C0431CD, ___index_1)); }
	inline int32_t get_index_1() const { return ___index_1; }
	inline int32_t* get_address_of_index_1() { return &___index_1; }
	inline void set_index_1(int32_t value)
	{
		___index_1 = value;
	}

	inline static int32_t get_offset_of_version_2() { return static_cast<int32_t>(offsetof(Enumerator_tE0C99528D3DCE5863566CE37BD94162A4C0431CD, ___version_2)); }
	inline int32_t get_version_2() const { return ___version_2; }
	inline int32_t* get_address_of_version_2() { return &___version_2; }
	inline void set_version_2(int32_t value)
	{
		___version_2 = value;
	}

	inline static int32_t get_offset_of_current_3() { return static_cast<int32_t>(offsetof(Enumerator_tE0C99528D3DCE5863566CE37BD94162A4C0431CD, ___current_3)); }
	inline RuntimeObject * get_current_3() const { return ___current_3; }
	inline RuntimeObject ** get_address_of_current_3() { return &___current_3; }
	inline void set_current_3(RuntimeObject * value)
	{
		___current_3 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___current_3), (void*)value);
	}
};


// System.Collections.Generic.List`1_Enumerator<UnityEngine.XR.ARSubsystems.XRReferenceObjectEntry>
struct  Enumerator_t8F224602FB8179E9E02E220ED94A72EA075C763F 
{
public:
	// System.Collections.Generic.List`1<T> System.Collections.Generic.List`1_Enumerator::list
	List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE * ___list_0;
	// System.Int32 System.Collections.Generic.List`1_Enumerator::index
	int32_t ___index_1;
	// System.Int32 System.Collections.Generic.List`1_Enumerator::version
	int32_t ___version_2;
	// T System.Collections.Generic.List`1_Enumerator::current
	XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8 * ___current_3;

public:
	inline static int32_t get_offset_of_list_0() { return static_cast<int32_t>(offsetof(Enumerator_t8F224602FB8179E9E02E220ED94A72EA075C763F, ___list_0)); }
	inline List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE * get_list_0() const { return ___list_0; }
	inline List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE ** get_address_of_list_0() { return &___list_0; }
	inline void set_list_0(List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE * value)
	{
		___list_0 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___list_0), (void*)value);
	}

	inline static int32_t get_offset_of_index_1() { return static_cast<int32_t>(offsetof(Enumerator_t8F224602FB8179E9E02E220ED94A72EA075C763F, ___index_1)); }
	inline int32_t get_index_1() const { return ___index_1; }
	inline int32_t* get_address_of_index_1() { return &___index_1; }
	inline void set_index_1(int32_t value)
	{
		___index_1 = value;
	}

	inline static int32_t get_offset_of_version_2() { return static_cast<int32_t>(offsetof(Enumerator_t8F224602FB8179E9E02E220ED94A72EA075C763F, ___version_2)); }
	inline int32_t get_version_2() const { return ___version_2; }
	inline int32_t* get_address_of_version_2() { return &___version_2; }
	inline void set_version_2(int32_t value)
	{
		___version_2 = value;
	}

	inline static int32_t get_offset_of_current_3() { return static_cast<int32_t>(offsetof(Enumerator_t8F224602FB8179E9E02E220ED94A72EA075C763F, ___current_3)); }
	inline XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8 * get_current_3() const { return ___current_3; }
	inline XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8 ** get_address_of_current_3() { return &___current_3; }
	inline void set_current_3(XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8 * value)
	{
		___current_3 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___current_3), (void*)value);
	}
};


// System.Enum
struct  Enum_t2AF27C02B8653AE29442467390005ABC74D8F521  : public ValueType_t4D0C27076F7C36E76190FB3328E232BCB1CD1FFF
{
public:

public:
};

struct Enum_t2AF27C02B8653AE29442467390005ABC74D8F521_StaticFields
{
public:
	// System.Char[] System.Enum::enumSeperatorCharArray
	CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___enumSeperatorCharArray_0;

public:
	inline static int32_t get_offset_of_enumSeperatorCharArray_0() { return static_cast<int32_t>(offsetof(Enum_t2AF27C02B8653AE29442467390005ABC74D8F521_StaticFields, ___enumSeperatorCharArray_0)); }
	inline CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* get_enumSeperatorCharArray_0() const { return ___enumSeperatorCharArray_0; }
	inline CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2** get_address_of_enumSeperatorCharArray_0() { return &___enumSeperatorCharArray_0; }
	inline void set_enumSeperatorCharArray_0(CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* value)
	{
		___enumSeperatorCharArray_0 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___enumSeperatorCharArray_0), (void*)value);
	}
};

// Native definition for P/Invoke marshalling of System.Enum
struct Enum_t2AF27C02B8653AE29442467390005ABC74D8F521_marshaled_pinvoke
{
};
// Native definition for COM marshalling of System.Enum
struct Enum_t2AF27C02B8653AE29442467390005ABC74D8F521_marshaled_com
{
};

// System.Guid
struct  Guid_t 
{
public:
	// System.Int32 System.Guid::_a
	int32_t ____a_1;
	// System.Int16 System.Guid::_b
	int16_t ____b_2;
	// System.Int16 System.Guid::_c
	int16_t ____c_3;
	// System.Byte System.Guid::_d
	uint8_t ____d_4;
	// System.Byte System.Guid::_e
	uint8_t ____e_5;
	// System.Byte System.Guid::_f
	uint8_t ____f_6;
	// System.Byte System.Guid::_g
	uint8_t ____g_7;
	// System.Byte System.Guid::_h
	uint8_t ____h_8;
	// System.Byte System.Guid::_i
	uint8_t ____i_9;
	// System.Byte System.Guid::_j
	uint8_t ____j_10;
	// System.Byte System.Guid::_k
	uint8_t ____k_11;

public:
	inline static int32_t get_offset_of__a_1() { return static_cast<int32_t>(offsetof(Guid_t, ____a_1)); }
	inline int32_t get__a_1() const { return ____a_1; }
	inline int32_t* get_address_of__a_1() { return &____a_1; }
	inline void set__a_1(int32_t value)
	{
		____a_1 = value;
	}

	inline static int32_t get_offset_of__b_2() { return static_cast<int32_t>(offsetof(Guid_t, ____b_2)); }
	inline int16_t get__b_2() const { return ____b_2; }
	inline int16_t* get_address_of__b_2() { return &____b_2; }
	inline void set__b_2(int16_t value)
	{
		____b_2 = value;
	}

	inline static int32_t get_offset_of__c_3() { return static_cast<int32_t>(offsetof(Guid_t, ____c_3)); }
	inline int16_t get__c_3() const { return ____c_3; }
	inline int16_t* get_address_of__c_3() { return &____c_3; }
	inline void set__c_3(int16_t value)
	{
		____c_3 = value;
	}

	inline static int32_t get_offset_of__d_4() { return static_cast<int32_t>(offsetof(Guid_t, ____d_4)); }
	inline uint8_t get__d_4() const { return ____d_4; }
	inline uint8_t* get_address_of__d_4() { return &____d_4; }
	inline void set__d_4(uint8_t value)
	{
		____d_4 = value;
	}

	inline static int32_t get_offset_of__e_5() { return static_cast<int32_t>(offsetof(Guid_t, ____e_5)); }
	inline uint8_t get__e_5() const { return ____e_5; }
	inline uint8_t* get_address_of__e_5() { return &____e_5; }
	inline void set__e_5(uint8_t value)
	{
		____e_5 = value;
	}

	inline static int32_t get_offset_of__f_6() { return static_cast<int32_t>(offsetof(Guid_t, ____f_6)); }
	inline uint8_t get__f_6() const { return ____f_6; }
	inline uint8_t* get_address_of__f_6() { return &____f_6; }
	inline void set__f_6(uint8_t value)
	{
		____f_6 = value;
	}

	inline static int32_t get_offset_of__g_7() { return static_cast<int32_t>(offsetof(Guid_t, ____g_7)); }
	inline uint8_t get__g_7() const { return ____g_7; }
	inline uint8_t* get_address_of__g_7() { return &____g_7; }
	inline void set__g_7(uint8_t value)
	{
		____g_7 = value;
	}

	inline static int32_t get_offset_of__h_8() { return static_cast<int32_t>(offsetof(Guid_t, ____h_8)); }
	inline uint8_t get__h_8() const { return ____h_8; }
	inline uint8_t* get_address_of__h_8() { return &____h_8; }
	inline void set__h_8(uint8_t value)
	{
		____h_8 = value;
	}

	inline static int32_t get_offset_of__i_9() { return static_cast<int32_t>(offsetof(Guid_t, ____i_9)); }
	inline uint8_t get__i_9() const { return ____i_9; }
	inline uint8_t* get_address_of__i_9() { return &____i_9; }
	inline void set__i_9(uint8_t value)
	{
		____i_9 = value;
	}

	inline static int32_t get_offset_of__j_10() { return static_cast<int32_t>(offsetof(Guid_t, ____j_10)); }
	inline uint8_t get__j_10() const { return ____j_10; }
	inline uint8_t* get_address_of__j_10() { return &____j_10; }
	inline void set__j_10(uint8_t value)
	{
		____j_10 = value;
	}

	inline static int32_t get_offset_of__k_11() { return static_cast<int32_t>(offsetof(Guid_t, ____k_11)); }
	inline uint8_t get__k_11() const { return ____k_11; }
	inline uint8_t* get_address_of__k_11() { return &____k_11; }
	inline void set__k_11(uint8_t value)
	{
		____k_11 = value;
	}
};

struct Guid_t_StaticFields
{
public:
	// System.Guid System.Guid::Empty
	Guid_t  ___Empty_0;
	// System.Object System.Guid::_rngAccess
	RuntimeObject * ____rngAccess_12;
	// System.Security.Cryptography.RandomNumberGenerator System.Guid::_rng
	RandomNumberGenerator_t12277F7F965BA79C54E4B3BFABD27A5FFB725EE2 * ____rng_13;
	// System.Security.Cryptography.RandomNumberGenerator System.Guid::_fastRng
	RandomNumberGenerator_t12277F7F965BA79C54E4B3BFABD27A5FFB725EE2 * ____fastRng_14;

public:
	inline static int32_t get_offset_of_Empty_0() { return static_cast<int32_t>(offsetof(Guid_t_StaticFields, ___Empty_0)); }
	inline Guid_t  get_Empty_0() const { return ___Empty_0; }
	inline Guid_t * get_address_of_Empty_0() { return &___Empty_0; }
	inline void set_Empty_0(Guid_t  value)
	{
		___Empty_0 = value;
	}

	inline static int32_t get_offset_of__rngAccess_12() { return static_cast<int32_t>(offsetof(Guid_t_StaticFields, ____rngAccess_12)); }
	inline RuntimeObject * get__rngAccess_12() const { return ____rngAccess_12; }
	inline RuntimeObject ** get_address_of__rngAccess_12() { return &____rngAccess_12; }
	inline void set__rngAccess_12(RuntimeObject * value)
	{
		____rngAccess_12 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____rngAccess_12), (void*)value);
	}

	inline static int32_t get_offset_of__rng_13() { return static_cast<int32_t>(offsetof(Guid_t_StaticFields, ____rng_13)); }
	inline RandomNumberGenerator_t12277F7F965BA79C54E4B3BFABD27A5FFB725EE2 * get__rng_13() const { return ____rng_13; }
	inline RandomNumberGenerator_t12277F7F965BA79C54E4B3BFABD27A5FFB725EE2 ** get_address_of__rng_13() { return &____rng_13; }
	inline void set__rng_13(RandomNumberGenerator_t12277F7F965BA79C54E4B3BFABD27A5FFB725EE2 * value)
	{
		____rng_13 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____rng_13), (void*)value);
	}

	inline static int32_t get_offset_of__fastRng_14() { return static_cast<int32_t>(offsetof(Guid_t_StaticFields, ____fastRng_14)); }
	inline RandomNumberGenerator_t12277F7F965BA79C54E4B3BFABD27A5FFB725EE2 * get__fastRng_14() const { return ____fastRng_14; }
	inline RandomNumberGenerator_t12277F7F965BA79C54E4B3BFABD27A5FFB725EE2 ** get_address_of__fastRng_14() { return &____fastRng_14; }
	inline void set__fastRng_14(RandomNumberGenerator_t12277F7F965BA79C54E4B3BFABD27A5FFB725EE2 * value)
	{
		____fastRng_14 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____fastRng_14), (void*)value);
	}
};


// System.Int32
struct  Int32_t585191389E07734F19F3156FF88FB3EF4800D102 
{
public:
	// System.Int32 System.Int32::m_value
	int32_t ___m_value_0;

public:
	inline static int32_t get_offset_of_m_value_0() { return static_cast<int32_t>(offsetof(Int32_t585191389E07734F19F3156FF88FB3EF4800D102, ___m_value_0)); }
	inline int32_t get_m_value_0() const { return ___m_value_0; }
	inline int32_t* get_address_of_m_value_0() { return &___m_value_0; }
	inline void set_m_value_0(int32_t value)
	{
		___m_value_0 = value;
	}
};


// System.IntPtr
struct  IntPtr_t 
{
public:
	// System.Void* System.IntPtr::m_value
	void* ___m_value_0;

public:
	inline static int32_t get_offset_of_m_value_0() { return static_cast<int32_t>(offsetof(IntPtr_t, ___m_value_0)); }
	inline void* get_m_value_0() const { return ___m_value_0; }
	inline void** get_address_of_m_value_0() { return &___m_value_0; }
	inline void set_m_value_0(void* value)
	{
		___m_value_0 = value;
	}
};

struct IntPtr_t_StaticFields
{
public:
	// System.IntPtr System.IntPtr::Zero
	intptr_t ___Zero_1;

public:
	inline static int32_t get_offset_of_Zero_1() { return static_cast<int32_t>(offsetof(IntPtr_t_StaticFields, ___Zero_1)); }
	inline intptr_t get_Zero_1() const { return ___Zero_1; }
	inline intptr_t* get_address_of_Zero_1() { return &___Zero_1; }
	inline void set_Zero_1(intptr_t value)
	{
		___Zero_1 = value;
	}
};


// System.Single
struct  Single_tDDDA9169C4E4E308AC6D7A824F9B28DC82204AE1 
{
public:
	// System.Single System.Single::m_value
	float ___m_value_0;

public:
	inline static int32_t get_offset_of_m_value_0() { return static_cast<int32_t>(offsetof(Single_tDDDA9169C4E4E308AC6D7A824F9B28DC82204AE1, ___m_value_0)); }
	inline float get_m_value_0() const { return ___m_value_0; }
	inline float* get_address_of_m_value_0() { return &___m_value_0; }
	inline void set_m_value_0(float value)
	{
		___m_value_0 = value;
	}
};


// System.UInt64
struct  UInt64_tA02DF3B59C8FC4A849BD207DA11038CC64E4CB4E 
{
public:
	// System.UInt64 System.UInt64::m_value
	uint64_t ___m_value_0;

public:
	inline static int32_t get_offset_of_m_value_0() { return static_cast<int32_t>(offsetof(UInt64_tA02DF3B59C8FC4A849BD207DA11038CC64E4CB4E, ___m_value_0)); }
	inline uint64_t get_m_value_0() const { return ___m_value_0; }
	inline uint64_t* get_address_of_m_value_0() { return &___m_value_0; }
	inline void set_m_value_0(uint64_t value)
	{
		___m_value_0 = value;
	}
};


// System.Void
struct  Void_t22962CB4C05B1D89B55A6E1139F0E87A90987017 
{
public:
	union
	{
		struct
		{
		};
		uint8_t Void_t22962CB4C05B1D89B55A6E1139F0E87A90987017__padding[1];
	};

public:
};


// UnityEngine.Quaternion
struct  Quaternion_t319F3319A7D43FFA5D819AD6C0A98851F0095357 
{
public:
	// System.Single UnityEngine.Quaternion::x
	float ___x_0;
	// System.Single UnityEngine.Quaternion::y
	float ___y_1;
	// System.Single UnityEngine.Quaternion::z
	float ___z_2;
	// System.Single UnityEngine.Quaternion::w
	float ___w_3;

public:
	inline static int32_t get_offset_of_x_0() { return static_cast<int32_t>(offsetof(Quaternion_t319F3319A7D43FFA5D819AD6C0A98851F0095357, ___x_0)); }
	inline float get_x_0() const { return ___x_0; }
	inline float* get_address_of_x_0() { return &___x_0; }
	inline void set_x_0(float value)
	{
		___x_0 = value;
	}

	inline static int32_t get_offset_of_y_1() { return static_cast<int32_t>(offsetof(Quaternion_t319F3319A7D43FFA5D819AD6C0A98851F0095357, ___y_1)); }
	inline float get_y_1() const { return ___y_1; }
	inline float* get_address_of_y_1() { return &___y_1; }
	inline void set_y_1(float value)
	{
		___y_1 = value;
	}

	inline static int32_t get_offset_of_z_2() { return static_cast<int32_t>(offsetof(Quaternion_t319F3319A7D43FFA5D819AD6C0A98851F0095357, ___z_2)); }
	inline float get_z_2() const { return ___z_2; }
	inline float* get_address_of_z_2() { return &___z_2; }
	inline void set_z_2(float value)
	{
		___z_2 = value;
	}

	inline static int32_t get_offset_of_w_3() { return static_cast<int32_t>(offsetof(Quaternion_t319F3319A7D43FFA5D819AD6C0A98851F0095357, ___w_3)); }
	inline float get_w_3() const { return ___w_3; }
	inline float* get_address_of_w_3() { return &___w_3; }
	inline void set_w_3(float value)
	{
		___w_3 = value;
	}
};

struct Quaternion_t319F3319A7D43FFA5D819AD6C0A98851F0095357_StaticFields
{
public:
	// UnityEngine.Quaternion UnityEngine.Quaternion::identityQuaternion
	Quaternion_t319F3319A7D43FFA5D819AD6C0A98851F0095357  ___identityQuaternion_4;

public:
	inline static int32_t get_offset_of_identityQuaternion_4() { return static_cast<int32_t>(offsetof(Quaternion_t319F3319A7D43FFA5D819AD6C0A98851F0095357_StaticFields, ___identityQuaternion_4)); }
	inline Quaternion_t319F3319A7D43FFA5D819AD6C0A98851F0095357  get_identityQuaternion_4() const { return ___identityQuaternion_4; }
	inline Quaternion_t319F3319A7D43FFA5D819AD6C0A98851F0095357 * get_address_of_identityQuaternion_4() { return &___identityQuaternion_4; }
	inline void set_identityQuaternion_4(Quaternion_t319F3319A7D43FFA5D819AD6C0A98851F0095357  value)
	{
		___identityQuaternion_4 = value;
	}
};


// UnityEngine.SubsystemDescriptor`1<UnityEngine.XR.ARSubsystems.XRRaycastSubsystem>
struct  SubsystemDescriptor_1_t27A8F3D552922AA16C799BCC575330BC6DF59F47  : public SubsystemDescriptor_t889FE96B75F157379A8337541A119F08DD9C75D4
{
public:

public:
};


// UnityEngine.SubsystemDescriptor`1<UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem>
struct  SubsystemDescriptor_1_tFFF984F9F6942FDAE965131A8954D4C4EA20B6B5  : public SubsystemDescriptor_t889FE96B75F157379A8337541A119F08DD9C75D4
{
public:

public:
};


// UnityEngine.SubsystemDescriptor`1<UnityEngine.XR.ARSubsystems.XRSessionSubsystem>
struct  SubsystemDescriptor_1_t0679ED150419770F94E42102F28D688E8F1DC9DA  : public SubsystemDescriptor_t889FE96B75F157379A8337541A119F08DD9C75D4
{
public:

public:
};


// UnityEngine.Subsystem`1<UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor>
struct  Subsystem_1_t1D44399B8190A9E3533EE5C5D5915B75D79B2E66  : public Subsystem_t6B987736D8E48098F860AC55D76905DE1F48CE8C
{
public:

public:
};


// UnityEngine.Subsystem`1<UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor>
struct  Subsystem_1_t1A38EF1402A7E5DBC48E0755E358926BB0054890  : public Subsystem_t6B987736D8E48098F860AC55D76905DE1F48CE8C
{
public:

public:
};


// UnityEngine.Subsystem`1<UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor>
struct  Subsystem_1_t749909AA5D71BD615BDF40BDE7C8F81B8D63F4C7  : public Subsystem_t6B987736D8E48098F860AC55D76905DE1F48CE8C
{
public:

public:
};


// UnityEngine.Vector2
struct  Vector2_tA85D2DD88578276CA8A8796756458277E72D073D 
{
public:
	// System.Single UnityEngine.Vector2::x
	float ___x_0;
	// System.Single UnityEngine.Vector2::y
	float ___y_1;

public:
	inline static int32_t get_offset_of_x_0() { return static_cast<int32_t>(offsetof(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D, ___x_0)); }
	inline float get_x_0() const { return ___x_0; }
	inline float* get_address_of_x_0() { return &___x_0; }
	inline void set_x_0(float value)
	{
		___x_0 = value;
	}

	inline static int32_t get_offset_of_y_1() { return static_cast<int32_t>(offsetof(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D, ___y_1)); }
	inline float get_y_1() const { return ___y_1; }
	inline float* get_address_of_y_1() { return &___y_1; }
	inline void set_y_1(float value)
	{
		___y_1 = value;
	}
};

struct Vector2_tA85D2DD88578276CA8A8796756458277E72D073D_StaticFields
{
public:
	// UnityEngine.Vector2 UnityEngine.Vector2::zeroVector
	Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  ___zeroVector_2;
	// UnityEngine.Vector2 UnityEngine.Vector2::oneVector
	Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  ___oneVector_3;
	// UnityEngine.Vector2 UnityEngine.Vector2::upVector
	Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  ___upVector_4;
	// UnityEngine.Vector2 UnityEngine.Vector2::downVector
	Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  ___downVector_5;
	// UnityEngine.Vector2 UnityEngine.Vector2::leftVector
	Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  ___leftVector_6;
	// UnityEngine.Vector2 UnityEngine.Vector2::rightVector
	Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  ___rightVector_7;
	// UnityEngine.Vector2 UnityEngine.Vector2::positiveInfinityVector
	Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  ___positiveInfinityVector_8;
	// UnityEngine.Vector2 UnityEngine.Vector2::negativeInfinityVector
	Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  ___negativeInfinityVector_9;

public:
	inline static int32_t get_offset_of_zeroVector_2() { return static_cast<int32_t>(offsetof(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D_StaticFields, ___zeroVector_2)); }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  get_zeroVector_2() const { return ___zeroVector_2; }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D * get_address_of_zeroVector_2() { return &___zeroVector_2; }
	inline void set_zeroVector_2(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  value)
	{
		___zeroVector_2 = value;
	}

	inline static int32_t get_offset_of_oneVector_3() { return static_cast<int32_t>(offsetof(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D_StaticFields, ___oneVector_3)); }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  get_oneVector_3() const { return ___oneVector_3; }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D * get_address_of_oneVector_3() { return &___oneVector_3; }
	inline void set_oneVector_3(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  value)
	{
		___oneVector_3 = value;
	}

	inline static int32_t get_offset_of_upVector_4() { return static_cast<int32_t>(offsetof(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D_StaticFields, ___upVector_4)); }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  get_upVector_4() const { return ___upVector_4; }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D * get_address_of_upVector_4() { return &___upVector_4; }
	inline void set_upVector_4(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  value)
	{
		___upVector_4 = value;
	}

	inline static int32_t get_offset_of_downVector_5() { return static_cast<int32_t>(offsetof(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D_StaticFields, ___downVector_5)); }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  get_downVector_5() const { return ___downVector_5; }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D * get_address_of_downVector_5() { return &___downVector_5; }
	inline void set_downVector_5(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  value)
	{
		___downVector_5 = value;
	}

	inline static int32_t get_offset_of_leftVector_6() { return static_cast<int32_t>(offsetof(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D_StaticFields, ___leftVector_6)); }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  get_leftVector_6() const { return ___leftVector_6; }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D * get_address_of_leftVector_6() { return &___leftVector_6; }
	inline void set_leftVector_6(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  value)
	{
		___leftVector_6 = value;
	}

	inline static int32_t get_offset_of_rightVector_7() { return static_cast<int32_t>(offsetof(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D_StaticFields, ___rightVector_7)); }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  get_rightVector_7() const { return ___rightVector_7; }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D * get_address_of_rightVector_7() { return &___rightVector_7; }
	inline void set_rightVector_7(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  value)
	{
		___rightVector_7 = value;
	}

	inline static int32_t get_offset_of_positiveInfinityVector_8() { return static_cast<int32_t>(offsetof(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D_StaticFields, ___positiveInfinityVector_8)); }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  get_positiveInfinityVector_8() const { return ___positiveInfinityVector_8; }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D * get_address_of_positiveInfinityVector_8() { return &___positiveInfinityVector_8; }
	inline void set_positiveInfinityVector_8(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  value)
	{
		___positiveInfinityVector_8 = value;
	}

	inline static int32_t get_offset_of_negativeInfinityVector_9() { return static_cast<int32_t>(offsetof(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D_StaticFields, ___negativeInfinityVector_9)); }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  get_negativeInfinityVector_9() const { return ___negativeInfinityVector_9; }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D * get_address_of_negativeInfinityVector_9() { return &___negativeInfinityVector_9; }
	inline void set_negativeInfinityVector_9(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  value)
	{
		___negativeInfinityVector_9 = value;
	}
};


// UnityEngine.Vector2Int
struct  Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905 
{
public:
	// System.Int32 UnityEngine.Vector2Int::m_X
	int32_t ___m_X_0;
	// System.Int32 UnityEngine.Vector2Int::m_Y
	int32_t ___m_Y_1;

public:
	inline static int32_t get_offset_of_m_X_0() { return static_cast<int32_t>(offsetof(Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905, ___m_X_0)); }
	inline int32_t get_m_X_0() const { return ___m_X_0; }
	inline int32_t* get_address_of_m_X_0() { return &___m_X_0; }
	inline void set_m_X_0(int32_t value)
	{
		___m_X_0 = value;
	}

	inline static int32_t get_offset_of_m_Y_1() { return static_cast<int32_t>(offsetof(Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905, ___m_Y_1)); }
	inline int32_t get_m_Y_1() const { return ___m_Y_1; }
	inline int32_t* get_address_of_m_Y_1() { return &___m_Y_1; }
	inline void set_m_Y_1(int32_t value)
	{
		___m_Y_1 = value;
	}
};

struct Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905_StaticFields
{
public:
	// UnityEngine.Vector2Int UnityEngine.Vector2Int::s_Zero
	Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  ___s_Zero_2;
	// UnityEngine.Vector2Int UnityEngine.Vector2Int::s_One
	Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  ___s_One_3;
	// UnityEngine.Vector2Int UnityEngine.Vector2Int::s_Up
	Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  ___s_Up_4;
	// UnityEngine.Vector2Int UnityEngine.Vector2Int::s_Down
	Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  ___s_Down_5;
	// UnityEngine.Vector2Int UnityEngine.Vector2Int::s_Left
	Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  ___s_Left_6;
	// UnityEngine.Vector2Int UnityEngine.Vector2Int::s_Right
	Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  ___s_Right_7;

public:
	inline static int32_t get_offset_of_s_Zero_2() { return static_cast<int32_t>(offsetof(Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905_StaticFields, ___s_Zero_2)); }
	inline Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  get_s_Zero_2() const { return ___s_Zero_2; }
	inline Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905 * get_address_of_s_Zero_2() { return &___s_Zero_2; }
	inline void set_s_Zero_2(Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  value)
	{
		___s_Zero_2 = value;
	}

	inline static int32_t get_offset_of_s_One_3() { return static_cast<int32_t>(offsetof(Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905_StaticFields, ___s_One_3)); }
	inline Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  get_s_One_3() const { return ___s_One_3; }
	inline Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905 * get_address_of_s_One_3() { return &___s_One_3; }
	inline void set_s_One_3(Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  value)
	{
		___s_One_3 = value;
	}

	inline static int32_t get_offset_of_s_Up_4() { return static_cast<int32_t>(offsetof(Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905_StaticFields, ___s_Up_4)); }
	inline Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  get_s_Up_4() const { return ___s_Up_4; }
	inline Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905 * get_address_of_s_Up_4() { return &___s_Up_4; }
	inline void set_s_Up_4(Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  value)
	{
		___s_Up_4 = value;
	}

	inline static int32_t get_offset_of_s_Down_5() { return static_cast<int32_t>(offsetof(Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905_StaticFields, ___s_Down_5)); }
	inline Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  get_s_Down_5() const { return ___s_Down_5; }
	inline Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905 * get_address_of_s_Down_5() { return &___s_Down_5; }
	inline void set_s_Down_5(Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  value)
	{
		___s_Down_5 = value;
	}

	inline static int32_t get_offset_of_s_Left_6() { return static_cast<int32_t>(offsetof(Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905_StaticFields, ___s_Left_6)); }
	inline Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  get_s_Left_6() const { return ___s_Left_6; }
	inline Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905 * get_address_of_s_Left_6() { return &___s_Left_6; }
	inline void set_s_Left_6(Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  value)
	{
		___s_Left_6 = value;
	}

	inline static int32_t get_offset_of_s_Right_7() { return static_cast<int32_t>(offsetof(Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905_StaticFields, ___s_Right_7)); }
	inline Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  get_s_Right_7() const { return ___s_Right_7; }
	inline Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905 * get_address_of_s_Right_7() { return &___s_Right_7; }
	inline void set_s_Right_7(Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  value)
	{
		___s_Right_7 = value;
	}
};


// UnityEngine.Vector3
struct  Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720 
{
public:
	// System.Single UnityEngine.Vector3::x
	float ___x_2;
	// System.Single UnityEngine.Vector3::y
	float ___y_3;
	// System.Single UnityEngine.Vector3::z
	float ___z_4;

public:
	inline static int32_t get_offset_of_x_2() { return static_cast<int32_t>(offsetof(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720, ___x_2)); }
	inline float get_x_2() const { return ___x_2; }
	inline float* get_address_of_x_2() { return &___x_2; }
	inline void set_x_2(float value)
	{
		___x_2 = value;
	}

	inline static int32_t get_offset_of_y_3() { return static_cast<int32_t>(offsetof(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720, ___y_3)); }
	inline float get_y_3() const { return ___y_3; }
	inline float* get_address_of_y_3() { return &___y_3; }
	inline void set_y_3(float value)
	{
		___y_3 = value;
	}

	inline static int32_t get_offset_of_z_4() { return static_cast<int32_t>(offsetof(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720, ___z_4)); }
	inline float get_z_4() const { return ___z_4; }
	inline float* get_address_of_z_4() { return &___z_4; }
	inline void set_z_4(float value)
	{
		___z_4 = value;
	}
};

struct Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720_StaticFields
{
public:
	// UnityEngine.Vector3 UnityEngine.Vector3::zeroVector
	Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  ___zeroVector_5;
	// UnityEngine.Vector3 UnityEngine.Vector3::oneVector
	Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  ___oneVector_6;
	// UnityEngine.Vector3 UnityEngine.Vector3::upVector
	Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  ___upVector_7;
	// UnityEngine.Vector3 UnityEngine.Vector3::downVector
	Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  ___downVector_8;
	// UnityEngine.Vector3 UnityEngine.Vector3::leftVector
	Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  ___leftVector_9;
	// UnityEngine.Vector3 UnityEngine.Vector3::rightVector
	Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  ___rightVector_10;
	// UnityEngine.Vector3 UnityEngine.Vector3::forwardVector
	Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  ___forwardVector_11;
	// UnityEngine.Vector3 UnityEngine.Vector3::backVector
	Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  ___backVector_12;
	// UnityEngine.Vector3 UnityEngine.Vector3::positiveInfinityVector
	Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  ___positiveInfinityVector_13;
	// UnityEngine.Vector3 UnityEngine.Vector3::negativeInfinityVector
	Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  ___negativeInfinityVector_14;

public:
	inline static int32_t get_offset_of_zeroVector_5() { return static_cast<int32_t>(offsetof(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720_StaticFields, ___zeroVector_5)); }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  get_zeroVector_5() const { return ___zeroVector_5; }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720 * get_address_of_zeroVector_5() { return &___zeroVector_5; }
	inline void set_zeroVector_5(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  value)
	{
		___zeroVector_5 = value;
	}

	inline static int32_t get_offset_of_oneVector_6() { return static_cast<int32_t>(offsetof(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720_StaticFields, ___oneVector_6)); }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  get_oneVector_6() const { return ___oneVector_6; }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720 * get_address_of_oneVector_6() { return &___oneVector_6; }
	inline void set_oneVector_6(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  value)
	{
		___oneVector_6 = value;
	}

	inline static int32_t get_offset_of_upVector_7() { return static_cast<int32_t>(offsetof(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720_StaticFields, ___upVector_7)); }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  get_upVector_7() const { return ___upVector_7; }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720 * get_address_of_upVector_7() { return &___upVector_7; }
	inline void set_upVector_7(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  value)
	{
		___upVector_7 = value;
	}

	inline static int32_t get_offset_of_downVector_8() { return static_cast<int32_t>(offsetof(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720_StaticFields, ___downVector_8)); }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  get_downVector_8() const { return ___downVector_8; }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720 * get_address_of_downVector_8() { return &___downVector_8; }
	inline void set_downVector_8(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  value)
	{
		___downVector_8 = value;
	}

	inline static int32_t get_offset_of_leftVector_9() { return static_cast<int32_t>(offsetof(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720_StaticFields, ___leftVector_9)); }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  get_leftVector_9() const { return ___leftVector_9; }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720 * get_address_of_leftVector_9() { return &___leftVector_9; }
	inline void set_leftVector_9(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  value)
	{
		___leftVector_9 = value;
	}

	inline static int32_t get_offset_of_rightVector_10() { return static_cast<int32_t>(offsetof(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720_StaticFields, ___rightVector_10)); }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  get_rightVector_10() const { return ___rightVector_10; }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720 * get_address_of_rightVector_10() { return &___rightVector_10; }
	inline void set_rightVector_10(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  value)
	{
		___rightVector_10 = value;
	}

	inline static int32_t get_offset_of_forwardVector_11() { return static_cast<int32_t>(offsetof(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720_StaticFields, ___forwardVector_11)); }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  get_forwardVector_11() const { return ___forwardVector_11; }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720 * get_address_of_forwardVector_11() { return &___forwardVector_11; }
	inline void set_forwardVector_11(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  value)
	{
		___forwardVector_11 = value;
	}

	inline static int32_t get_offset_of_backVector_12() { return static_cast<int32_t>(offsetof(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720_StaticFields, ___backVector_12)); }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  get_backVector_12() const { return ___backVector_12; }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720 * get_address_of_backVector_12() { return &___backVector_12; }
	inline void set_backVector_12(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  value)
	{
		___backVector_12 = value;
	}

	inline static int32_t get_offset_of_positiveInfinityVector_13() { return static_cast<int32_t>(offsetof(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720_StaticFields, ___positiveInfinityVector_13)); }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  get_positiveInfinityVector_13() const { return ___positiveInfinityVector_13; }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720 * get_address_of_positiveInfinityVector_13() { return &___positiveInfinityVector_13; }
	inline void set_positiveInfinityVector_13(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  value)
	{
		___positiveInfinityVector_13 = value;
	}

	inline static int32_t get_offset_of_negativeInfinityVector_14() { return static_cast<int32_t>(offsetof(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720_StaticFields, ___negativeInfinityVector_14)); }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  get_negativeInfinityVector_14() const { return ___negativeInfinityVector_14; }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720 * get_address_of_negativeInfinityVector_14() { return &___negativeInfinityVector_14; }
	inline void set_negativeInfinityVector_14(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  value)
	{
		___negativeInfinityVector_14 = value;
	}
};


// UnityEngine.XR.ARSubsystems.SerializableGuid
struct  SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821 
{
public:
	// System.UInt64 UnityEngine.XR.ARSubsystems.SerializableGuid::m_GuidLow
	uint64_t ___m_GuidLow_1;
	// System.UInt64 UnityEngine.XR.ARSubsystems.SerializableGuid::m_GuidHigh
	uint64_t ___m_GuidHigh_2;

public:
	inline static int32_t get_offset_of_m_GuidLow_1() { return static_cast<int32_t>(offsetof(SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821, ___m_GuidLow_1)); }
	inline uint64_t get_m_GuidLow_1() const { return ___m_GuidLow_1; }
	inline uint64_t* get_address_of_m_GuidLow_1() { return &___m_GuidLow_1; }
	inline void set_m_GuidLow_1(uint64_t value)
	{
		___m_GuidLow_1 = value;
	}

	inline static int32_t get_offset_of_m_GuidHigh_2() { return static_cast<int32_t>(offsetof(SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821, ___m_GuidHigh_2)); }
	inline uint64_t get_m_GuidHigh_2() const { return ___m_GuidHigh_2; }
	inline uint64_t* get_address_of_m_GuidHigh_2() { return &___m_GuidHigh_2; }
	inline void set_m_GuidHigh_2(uint64_t value)
	{
		___m_GuidHigh_2 = value;
	}
};

struct SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821_StaticFields
{
public:
	// UnityEngine.XR.ARSubsystems.SerializableGuid UnityEngine.XR.ARSubsystems.SerializableGuid::k_Empty
	SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  ___k_Empty_0;

public:
	inline static int32_t get_offset_of_k_Empty_0() { return static_cast<int32_t>(offsetof(SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821_StaticFields, ___k_Empty_0)); }
	inline SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  get_k_Empty_0() const { return ___k_Empty_0; }
	inline SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821 * get_address_of_k_Empty_0() { return &___k_Empty_0; }
	inline void set_k_Empty_0(SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  value)
	{
		___k_Empty_0 = value;
	}
};


// UnityEngine.XR.ARSubsystems.TrackableId
struct  TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 
{
public:
	// System.UInt64 UnityEngine.XR.ARSubsystems.TrackableId::m_SubId1
	uint64_t ___m_SubId1_1;
	// System.UInt64 UnityEngine.XR.ARSubsystems.TrackableId::m_SubId2
	uint64_t ___m_SubId2_2;

public:
	inline static int32_t get_offset_of_m_SubId1_1() { return static_cast<int32_t>(offsetof(TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47, ___m_SubId1_1)); }
	inline uint64_t get_m_SubId1_1() const { return ___m_SubId1_1; }
	inline uint64_t* get_address_of_m_SubId1_1() { return &___m_SubId1_1; }
	inline void set_m_SubId1_1(uint64_t value)
	{
		___m_SubId1_1 = value;
	}

	inline static int32_t get_offset_of_m_SubId2_2() { return static_cast<int32_t>(offsetof(TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47, ___m_SubId2_2)); }
	inline uint64_t get_m_SubId2_2() const { return ___m_SubId2_2; }
	inline uint64_t* get_address_of_m_SubId2_2() { return &___m_SubId2_2; }
	inline void set_m_SubId2_2(uint64_t value)
	{
		___m_SubId2_2 = value;
	}
};

struct TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47_StaticFields
{
public:
	// UnityEngine.XR.ARSubsystems.TrackableId UnityEngine.XR.ARSubsystems.TrackableId::s_InvalidId
	TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___s_InvalidId_0;

public:
	inline static int32_t get_offset_of_s_InvalidId_0() { return static_cast<int32_t>(offsetof(TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47_StaticFields, ___s_InvalidId_0)); }
	inline TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  get_s_InvalidId_0() const { return ___s_InvalidId_0; }
	inline TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 * get_address_of_s_InvalidId_0() { return &___s_InvalidId_0; }
	inline void set_s_InvalidId_0(TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  value)
	{
		___s_InvalidId_0 = value;
	}
};


// UnityEngine.XR.ARSubsystems.XRReferenceObject
struct  XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA 
{
public:
	// System.UInt64 UnityEngine.XR.ARSubsystems.XRReferenceObject::m_GuidLow
	uint64_t ___m_GuidLow_0;
	// System.UInt64 UnityEngine.XR.ARSubsystems.XRReferenceObject::m_GuidHigh
	uint64_t ___m_GuidHigh_1;
	// System.String UnityEngine.XR.ARSubsystems.XRReferenceObject::m_Name
	String_t* ___m_Name_2;
	// System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceObjectEntry> UnityEngine.XR.ARSubsystems.XRReferenceObject::m_Entries
	List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE * ___m_Entries_3;

public:
	inline static int32_t get_offset_of_m_GuidLow_0() { return static_cast<int32_t>(offsetof(XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA, ___m_GuidLow_0)); }
	inline uint64_t get_m_GuidLow_0() const { return ___m_GuidLow_0; }
	inline uint64_t* get_address_of_m_GuidLow_0() { return &___m_GuidLow_0; }
	inline void set_m_GuidLow_0(uint64_t value)
	{
		___m_GuidLow_0 = value;
	}

	inline static int32_t get_offset_of_m_GuidHigh_1() { return static_cast<int32_t>(offsetof(XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA, ___m_GuidHigh_1)); }
	inline uint64_t get_m_GuidHigh_1() const { return ___m_GuidHigh_1; }
	inline uint64_t* get_address_of_m_GuidHigh_1() { return &___m_GuidHigh_1; }
	inline void set_m_GuidHigh_1(uint64_t value)
	{
		___m_GuidHigh_1 = value;
	}

	inline static int32_t get_offset_of_m_Name_2() { return static_cast<int32_t>(offsetof(XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA, ___m_Name_2)); }
	inline String_t* get_m_Name_2() const { return ___m_Name_2; }
	inline String_t** get_address_of_m_Name_2() { return &___m_Name_2; }
	inline void set_m_Name_2(String_t* value)
	{
		___m_Name_2 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_Name_2), (void*)value);
	}

	inline static int32_t get_offset_of_m_Entries_3() { return static_cast<int32_t>(offsetof(XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA, ___m_Entries_3)); }
	inline List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE * get_m_Entries_3() const { return ___m_Entries_3; }
	inline List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE ** get_address_of_m_Entries_3() { return &___m_Entries_3; }
	inline void set_m_Entries_3(List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE * value)
	{
		___m_Entries_3 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_Entries_3), (void*)value);
	}
};

// Native definition for P/Invoke marshalling of UnityEngine.XR.ARSubsystems.XRReferenceObject
struct XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA_marshaled_pinvoke
{
	uint64_t ___m_GuidLow_0;
	uint64_t ___m_GuidHigh_1;
	char* ___m_Name_2;
	List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE * ___m_Entries_3;
};
// Native definition for COM marshalling of UnityEngine.XR.ARSubsystems.XRReferenceObject
struct XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA_marshaled_com
{
	uint64_t ___m_GuidLow_0;
	uint64_t ___m_GuidHigh_1;
	Il2CppChar* ___m_Name_2;
	List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE * ___m_Entries_3;
};

// UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo
struct  Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD 
{
public:
	// System.String UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo::<id>k__BackingField
	String_t* ___U3CidU3Ek__BackingField_0;
	// System.Type UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo::<subsystemImplementationType>k__BackingField
	Type_t * ___U3CsubsystemImplementationTypeU3Ek__BackingField_1;
	// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo::<supportsTrackableAttachments>k__BackingField
	bool ___U3CsupportsTrackableAttachmentsU3Ek__BackingField_2;

public:
	inline static int32_t get_offset_of_U3CidU3Ek__BackingField_0() { return static_cast<int32_t>(offsetof(Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD, ___U3CidU3Ek__BackingField_0)); }
	inline String_t* get_U3CidU3Ek__BackingField_0() const { return ___U3CidU3Ek__BackingField_0; }
	inline String_t** get_address_of_U3CidU3Ek__BackingField_0() { return &___U3CidU3Ek__BackingField_0; }
	inline void set_U3CidU3Ek__BackingField_0(String_t* value)
	{
		___U3CidU3Ek__BackingField_0 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___U3CidU3Ek__BackingField_0), (void*)value);
	}

	inline static int32_t get_offset_of_U3CsubsystemImplementationTypeU3Ek__BackingField_1() { return static_cast<int32_t>(offsetof(Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD, ___U3CsubsystemImplementationTypeU3Ek__BackingField_1)); }
	inline Type_t * get_U3CsubsystemImplementationTypeU3Ek__BackingField_1() const { return ___U3CsubsystemImplementationTypeU3Ek__BackingField_1; }
	inline Type_t ** get_address_of_U3CsubsystemImplementationTypeU3Ek__BackingField_1() { return &___U3CsubsystemImplementationTypeU3Ek__BackingField_1; }
	inline void set_U3CsubsystemImplementationTypeU3Ek__BackingField_1(Type_t * value)
	{
		___U3CsubsystemImplementationTypeU3Ek__BackingField_1 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___U3CsubsystemImplementationTypeU3Ek__BackingField_1), (void*)value);
	}

	inline static int32_t get_offset_of_U3CsupportsTrackableAttachmentsU3Ek__BackingField_2() { return static_cast<int32_t>(offsetof(Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD, ___U3CsupportsTrackableAttachmentsU3Ek__BackingField_2)); }
	inline bool get_U3CsupportsTrackableAttachmentsU3Ek__BackingField_2() const { return ___U3CsupportsTrackableAttachmentsU3Ek__BackingField_2; }
	inline bool* get_address_of_U3CsupportsTrackableAttachmentsU3Ek__BackingField_2() { return &___U3CsupportsTrackableAttachmentsU3Ek__BackingField_2; }
	inline void set_U3CsupportsTrackableAttachmentsU3Ek__BackingField_2(bool value)
	{
		___U3CsupportsTrackableAttachmentsU3Ek__BackingField_2 = value;
	}
};

// Native definition for P/Invoke marshalling of UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor/Cinfo
struct Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD_marshaled_pinvoke
{
	char* ___U3CidU3Ek__BackingField_0;
	Type_t * ___U3CsubsystemImplementationTypeU3Ek__BackingField_1;
	int32_t ___U3CsupportsTrackableAttachmentsU3Ek__BackingField_2;
};
// Native definition for COM marshalling of UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor/Cinfo
struct Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD_marshaled_com
{
	Il2CppChar* ___U3CidU3Ek__BackingField_0;
	Type_t * ___U3CsubsystemImplementationTypeU3Ek__BackingField_1;
	int32_t ___U3CsupportsTrackableAttachmentsU3Ek__BackingField_2;
};

// UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo
struct  Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A 
{
public:
	// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo::<supportsInstall>k__BackingField
	bool ___U3CsupportsInstallU3Ek__BackingField_0;
	// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo::<supportsMatchFrameRate>k__BackingField
	bool ___U3CsupportsMatchFrameRateU3Ek__BackingField_1;
	// System.String UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo::<id>k__BackingField
	String_t* ___U3CidU3Ek__BackingField_2;
	// System.Type UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo::<subsystemImplementationType>k__BackingField
	Type_t * ___U3CsubsystemImplementationTypeU3Ek__BackingField_3;

public:
	inline static int32_t get_offset_of_U3CsupportsInstallU3Ek__BackingField_0() { return static_cast<int32_t>(offsetof(Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A, ___U3CsupportsInstallU3Ek__BackingField_0)); }
	inline bool get_U3CsupportsInstallU3Ek__BackingField_0() const { return ___U3CsupportsInstallU3Ek__BackingField_0; }
	inline bool* get_address_of_U3CsupportsInstallU3Ek__BackingField_0() { return &___U3CsupportsInstallU3Ek__BackingField_0; }
	inline void set_U3CsupportsInstallU3Ek__BackingField_0(bool value)
	{
		___U3CsupportsInstallU3Ek__BackingField_0 = value;
	}

	inline static int32_t get_offset_of_U3CsupportsMatchFrameRateU3Ek__BackingField_1() { return static_cast<int32_t>(offsetof(Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A, ___U3CsupportsMatchFrameRateU3Ek__BackingField_1)); }
	inline bool get_U3CsupportsMatchFrameRateU3Ek__BackingField_1() const { return ___U3CsupportsMatchFrameRateU3Ek__BackingField_1; }
	inline bool* get_address_of_U3CsupportsMatchFrameRateU3Ek__BackingField_1() { return &___U3CsupportsMatchFrameRateU3Ek__BackingField_1; }
	inline void set_U3CsupportsMatchFrameRateU3Ek__BackingField_1(bool value)
	{
		___U3CsupportsMatchFrameRateU3Ek__BackingField_1 = value;
	}

	inline static int32_t get_offset_of_U3CidU3Ek__BackingField_2() { return static_cast<int32_t>(offsetof(Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A, ___U3CidU3Ek__BackingField_2)); }
	inline String_t* get_U3CidU3Ek__BackingField_2() const { return ___U3CidU3Ek__BackingField_2; }
	inline String_t** get_address_of_U3CidU3Ek__BackingField_2() { return &___U3CidU3Ek__BackingField_2; }
	inline void set_U3CidU3Ek__BackingField_2(String_t* value)
	{
		___U3CidU3Ek__BackingField_2 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___U3CidU3Ek__BackingField_2), (void*)value);
	}

	inline static int32_t get_offset_of_U3CsubsystemImplementationTypeU3Ek__BackingField_3() { return static_cast<int32_t>(offsetof(Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A, ___U3CsubsystemImplementationTypeU3Ek__BackingField_3)); }
	inline Type_t * get_U3CsubsystemImplementationTypeU3Ek__BackingField_3() const { return ___U3CsubsystemImplementationTypeU3Ek__BackingField_3; }
	inline Type_t ** get_address_of_U3CsubsystemImplementationTypeU3Ek__BackingField_3() { return &___U3CsubsystemImplementationTypeU3Ek__BackingField_3; }
	inline void set_U3CsubsystemImplementationTypeU3Ek__BackingField_3(Type_t * value)
	{
		___U3CsubsystemImplementationTypeU3Ek__BackingField_3 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___U3CsubsystemImplementationTypeU3Ek__BackingField_3), (void*)value);
	}
};

// Native definition for P/Invoke marshalling of UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor/Cinfo
struct Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A_marshaled_pinvoke
{
	int32_t ___U3CsupportsInstallU3Ek__BackingField_0;
	int32_t ___U3CsupportsMatchFrameRateU3Ek__BackingField_1;
	char* ___U3CidU3Ek__BackingField_2;
	Type_t * ___U3CsubsystemImplementationTypeU3Ek__BackingField_3;
};
// Native definition for COM marshalling of UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor/Cinfo
struct Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A_marshaled_com
{
	int32_t ___U3CsupportsInstallU3Ek__BackingField_0;
	int32_t ___U3CsupportsMatchFrameRateU3Ek__BackingField_1;
	Il2CppChar* ___U3CidU3Ek__BackingField_2;
	Type_t * ___U3CsubsystemImplementationTypeU3Ek__BackingField_3;
};

// System.Collections.Generic.List`1_Enumerator<UnityEngine.XR.ARSubsystems.XRReferenceObject>
struct  Enumerator_t3D111B64EA07FCA753146B875D55422A5191B57F 
{
public:
	// System.Collections.Generic.List`1<T> System.Collections.Generic.List`1_Enumerator::list
	List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * ___list_0;
	// System.Int32 System.Collections.Generic.List`1_Enumerator::index
	int32_t ___index_1;
	// System.Int32 System.Collections.Generic.List`1_Enumerator::version
	int32_t ___version_2;
	// T System.Collections.Generic.List`1_Enumerator::current
	XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  ___current_3;

public:
	inline static int32_t get_offset_of_list_0() { return static_cast<int32_t>(offsetof(Enumerator_t3D111B64EA07FCA753146B875D55422A5191B57F, ___list_0)); }
	inline List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * get_list_0() const { return ___list_0; }
	inline List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 ** get_address_of_list_0() { return &___list_0; }
	inline void set_list_0(List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * value)
	{
		___list_0 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___list_0), (void*)value);
	}

	inline static int32_t get_offset_of_index_1() { return static_cast<int32_t>(offsetof(Enumerator_t3D111B64EA07FCA753146B875D55422A5191B57F, ___index_1)); }
	inline int32_t get_index_1() const { return ___index_1; }
	inline int32_t* get_address_of_index_1() { return &___index_1; }
	inline void set_index_1(int32_t value)
	{
		___index_1 = value;
	}

	inline static int32_t get_offset_of_version_2() { return static_cast<int32_t>(offsetof(Enumerator_t3D111B64EA07FCA753146B875D55422A5191B57F, ___version_2)); }
	inline int32_t get_version_2() const { return ___version_2; }
	inline int32_t* get_address_of_version_2() { return &___version_2; }
	inline void set_version_2(int32_t value)
	{
		___version_2 = value;
	}

	inline static int32_t get_offset_of_current_3() { return static_cast<int32_t>(offsetof(Enumerator_t3D111B64EA07FCA753146B875D55422A5191B57F, ___current_3)); }
	inline XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  get_current_3() const { return ___current_3; }
	inline XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * get_address_of_current_3() { return &___current_3; }
	inline void set_current_3(XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  value)
	{
		___current_3 = value;
		Il2CppCodeGenWriteBarrier((void**)&(((&___current_3))->___m_Name_2), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&(((&___current_3))->___m_Entries_3), (void*)NULL);
		#endif
	}
};


// System.Exception
struct  Exception_t  : public RuntimeObject
{
public:
	// System.String System.Exception::_className
	String_t* ____className_1;
	// System.String System.Exception::_message
	String_t* ____message_2;
	// System.Collections.IDictionary System.Exception::_data
	RuntimeObject* ____data_3;
	// System.Exception System.Exception::_innerException
	Exception_t * ____innerException_4;
	// System.String System.Exception::_helpURL
	String_t* ____helpURL_5;
	// System.Object System.Exception::_stackTrace
	RuntimeObject * ____stackTrace_6;
	// System.String System.Exception::_stackTraceString
	String_t* ____stackTraceString_7;
	// System.String System.Exception::_remoteStackTraceString
	String_t* ____remoteStackTraceString_8;
	// System.Int32 System.Exception::_remoteStackIndex
	int32_t ____remoteStackIndex_9;
	// System.Object System.Exception::_dynamicMethods
	RuntimeObject * ____dynamicMethods_10;
	// System.Int32 System.Exception::_HResult
	int32_t ____HResult_11;
	// System.String System.Exception::_source
	String_t* ____source_12;
	// System.Runtime.Serialization.SafeSerializationManager System.Exception::_safeSerializationManager
	SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770 * ____safeSerializationManager_13;
	// System.Diagnostics.StackTrace[] System.Exception::captured_traces
	StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196* ___captured_traces_14;
	// System.IntPtr[] System.Exception::native_trace_ips
	IntPtrU5BU5D_t4DC01DCB9A6DF6C9792A6513595D7A11E637DCDD* ___native_trace_ips_15;

public:
	inline static int32_t get_offset_of__className_1() { return static_cast<int32_t>(offsetof(Exception_t, ____className_1)); }
	inline String_t* get__className_1() const { return ____className_1; }
	inline String_t** get_address_of__className_1() { return &____className_1; }
	inline void set__className_1(String_t* value)
	{
		____className_1 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____className_1), (void*)value);
	}

	inline static int32_t get_offset_of__message_2() { return static_cast<int32_t>(offsetof(Exception_t, ____message_2)); }
	inline String_t* get__message_2() const { return ____message_2; }
	inline String_t** get_address_of__message_2() { return &____message_2; }
	inline void set__message_2(String_t* value)
	{
		____message_2 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____message_2), (void*)value);
	}

	inline static int32_t get_offset_of__data_3() { return static_cast<int32_t>(offsetof(Exception_t, ____data_3)); }
	inline RuntimeObject* get__data_3() const { return ____data_3; }
	inline RuntimeObject** get_address_of__data_3() { return &____data_3; }
	inline void set__data_3(RuntimeObject* value)
	{
		____data_3 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____data_3), (void*)value);
	}

	inline static int32_t get_offset_of__innerException_4() { return static_cast<int32_t>(offsetof(Exception_t, ____innerException_4)); }
	inline Exception_t * get__innerException_4() const { return ____innerException_4; }
	inline Exception_t ** get_address_of__innerException_4() { return &____innerException_4; }
	inline void set__innerException_4(Exception_t * value)
	{
		____innerException_4 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____innerException_4), (void*)value);
	}

	inline static int32_t get_offset_of__helpURL_5() { return static_cast<int32_t>(offsetof(Exception_t, ____helpURL_5)); }
	inline String_t* get__helpURL_5() const { return ____helpURL_5; }
	inline String_t** get_address_of__helpURL_5() { return &____helpURL_5; }
	inline void set__helpURL_5(String_t* value)
	{
		____helpURL_5 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____helpURL_5), (void*)value);
	}

	inline static int32_t get_offset_of__stackTrace_6() { return static_cast<int32_t>(offsetof(Exception_t, ____stackTrace_6)); }
	inline RuntimeObject * get__stackTrace_6() const { return ____stackTrace_6; }
	inline RuntimeObject ** get_address_of__stackTrace_6() { return &____stackTrace_6; }
	inline void set__stackTrace_6(RuntimeObject * value)
	{
		____stackTrace_6 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____stackTrace_6), (void*)value);
	}

	inline static int32_t get_offset_of__stackTraceString_7() { return static_cast<int32_t>(offsetof(Exception_t, ____stackTraceString_7)); }
	inline String_t* get__stackTraceString_7() const { return ____stackTraceString_7; }
	inline String_t** get_address_of__stackTraceString_7() { return &____stackTraceString_7; }
	inline void set__stackTraceString_7(String_t* value)
	{
		____stackTraceString_7 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____stackTraceString_7), (void*)value);
	}

	inline static int32_t get_offset_of__remoteStackTraceString_8() { return static_cast<int32_t>(offsetof(Exception_t, ____remoteStackTraceString_8)); }
	inline String_t* get__remoteStackTraceString_8() const { return ____remoteStackTraceString_8; }
	inline String_t** get_address_of__remoteStackTraceString_8() { return &____remoteStackTraceString_8; }
	inline void set__remoteStackTraceString_8(String_t* value)
	{
		____remoteStackTraceString_8 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____remoteStackTraceString_8), (void*)value);
	}

	inline static int32_t get_offset_of__remoteStackIndex_9() { return static_cast<int32_t>(offsetof(Exception_t, ____remoteStackIndex_9)); }
	inline int32_t get__remoteStackIndex_9() const { return ____remoteStackIndex_9; }
	inline int32_t* get_address_of__remoteStackIndex_9() { return &____remoteStackIndex_9; }
	inline void set__remoteStackIndex_9(int32_t value)
	{
		____remoteStackIndex_9 = value;
	}

	inline static int32_t get_offset_of__dynamicMethods_10() { return static_cast<int32_t>(offsetof(Exception_t, ____dynamicMethods_10)); }
	inline RuntimeObject * get__dynamicMethods_10() const { return ____dynamicMethods_10; }
	inline RuntimeObject ** get_address_of__dynamicMethods_10() { return &____dynamicMethods_10; }
	inline void set__dynamicMethods_10(RuntimeObject * value)
	{
		____dynamicMethods_10 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____dynamicMethods_10), (void*)value);
	}

	inline static int32_t get_offset_of__HResult_11() { return static_cast<int32_t>(offsetof(Exception_t, ____HResult_11)); }
	inline int32_t get__HResult_11() const { return ____HResult_11; }
	inline int32_t* get_address_of__HResult_11() { return &____HResult_11; }
	inline void set__HResult_11(int32_t value)
	{
		____HResult_11 = value;
	}

	inline static int32_t get_offset_of__source_12() { return static_cast<int32_t>(offsetof(Exception_t, ____source_12)); }
	inline String_t* get__source_12() const { return ____source_12; }
	inline String_t** get_address_of__source_12() { return &____source_12; }
	inline void set__source_12(String_t* value)
	{
		____source_12 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____source_12), (void*)value);
	}

	inline static int32_t get_offset_of__safeSerializationManager_13() { return static_cast<int32_t>(offsetof(Exception_t, ____safeSerializationManager_13)); }
	inline SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770 * get__safeSerializationManager_13() const { return ____safeSerializationManager_13; }
	inline SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770 ** get_address_of__safeSerializationManager_13() { return &____safeSerializationManager_13; }
	inline void set__safeSerializationManager_13(SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770 * value)
	{
		____safeSerializationManager_13 = value;
		Il2CppCodeGenWriteBarrier((void**)(&____safeSerializationManager_13), (void*)value);
	}

	inline static int32_t get_offset_of_captured_traces_14() { return static_cast<int32_t>(offsetof(Exception_t, ___captured_traces_14)); }
	inline StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196* get_captured_traces_14() const { return ___captured_traces_14; }
	inline StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196** get_address_of_captured_traces_14() { return &___captured_traces_14; }
	inline void set_captured_traces_14(StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196* value)
	{
		___captured_traces_14 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___captured_traces_14), (void*)value);
	}

	inline static int32_t get_offset_of_native_trace_ips_15() { return static_cast<int32_t>(offsetof(Exception_t, ___native_trace_ips_15)); }
	inline IntPtrU5BU5D_t4DC01DCB9A6DF6C9792A6513595D7A11E637DCDD* get_native_trace_ips_15() const { return ___native_trace_ips_15; }
	inline IntPtrU5BU5D_t4DC01DCB9A6DF6C9792A6513595D7A11E637DCDD** get_address_of_native_trace_ips_15() { return &___native_trace_ips_15; }
	inline void set_native_trace_ips_15(IntPtrU5BU5D_t4DC01DCB9A6DF6C9792A6513595D7A11E637DCDD* value)
	{
		___native_trace_ips_15 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___native_trace_ips_15), (void*)value);
	}
};

struct Exception_t_StaticFields
{
public:
	// System.Object System.Exception::s_EDILock
	RuntimeObject * ___s_EDILock_0;

public:
	inline static int32_t get_offset_of_s_EDILock_0() { return static_cast<int32_t>(offsetof(Exception_t_StaticFields, ___s_EDILock_0)); }
	inline RuntimeObject * get_s_EDILock_0() const { return ___s_EDILock_0; }
	inline RuntimeObject ** get_address_of_s_EDILock_0() { return &___s_EDILock_0; }
	inline void set_s_EDILock_0(RuntimeObject * value)
	{
		___s_EDILock_0 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___s_EDILock_0), (void*)value);
	}
};

// Native definition for P/Invoke marshalling of System.Exception
struct Exception_t_marshaled_pinvoke
{
	char* ____className_1;
	char* ____message_2;
	RuntimeObject* ____data_3;
	Exception_t_marshaled_pinvoke* ____innerException_4;
	char* ____helpURL_5;
	Il2CppIUnknown* ____stackTrace_6;
	char* ____stackTraceString_7;
	char* ____remoteStackTraceString_8;
	int32_t ____remoteStackIndex_9;
	Il2CppIUnknown* ____dynamicMethods_10;
	int32_t ____HResult_11;
	char* ____source_12;
	SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770 * ____safeSerializationManager_13;
	StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196* ___captured_traces_14;
	Il2CppSafeArray/*NONE*/* ___native_trace_ips_15;
};
// Native definition for COM marshalling of System.Exception
struct Exception_t_marshaled_com
{
	Il2CppChar* ____className_1;
	Il2CppChar* ____message_2;
	RuntimeObject* ____data_3;
	Exception_t_marshaled_com* ____innerException_4;
	Il2CppChar* ____helpURL_5;
	Il2CppIUnknown* ____stackTrace_6;
	Il2CppChar* ____stackTraceString_7;
	Il2CppChar* ____remoteStackTraceString_8;
	int32_t ____remoteStackIndex_9;
	Il2CppIUnknown* ____dynamicMethods_10;
	int32_t ____HResult_11;
	Il2CppChar* ____source_12;
	SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770 * ____safeSerializationManager_13;
	StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196* ___captured_traces_14;
	Il2CppSafeArray/*NONE*/* ___native_trace_ips_15;
};

// System.Int32Enum
struct  Int32Enum_t6312CE4586C17FE2E2E513D2E7655B574F10FDCD 
{
public:
	// System.Int32 System.Int32Enum::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(Int32Enum_t6312CE4586C17FE2E2E513D2E7655B574F10FDCD, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};


// System.Nullable`1<UnityEngine.Vector2>
struct  Nullable_1_t2D31B61CB1F8452E868737CCA40082AA2D1F58DC 
{
public:
	// T System.Nullable`1::value
	Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  ___value_0;
	// System.Boolean System.Nullable`1::has_value
	bool ___has_value_1;

public:
	inline static int32_t get_offset_of_value_0() { return static_cast<int32_t>(offsetof(Nullable_1_t2D31B61CB1F8452E868737CCA40082AA2D1F58DC, ___value_0)); }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  get_value_0() const { return ___value_0; }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D * get_address_of_value_0() { return &___value_0; }
	inline void set_value_0(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  value)
	{
		___value_0 = value;
	}

	inline static int32_t get_offset_of_has_value_1() { return static_cast<int32_t>(offsetof(Nullable_1_t2D31B61CB1F8452E868737CCA40082AA2D1F58DC, ___has_value_1)); }
	inline bool get_has_value_1() const { return ___has_value_1; }
	inline bool* get_address_of_has_value_1() { return &___has_value_1; }
	inline void set_has_value_1(bool value)
	{
		___has_value_1 = value;
	}
};


// System.Reflection.BindingFlags
struct  BindingFlags_tE35C91D046E63A1B92BB9AB909FCF9DA84379ED0 
{
public:
	// System.Int32 System.Reflection.BindingFlags::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(BindingFlags_tE35C91D046E63A1B92BB9AB909FCF9DA84379ED0, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};


// System.RuntimeTypeHandle
struct  RuntimeTypeHandle_t7B542280A22F0EC4EAC2061C29178845847A8B2D 
{
public:
	// System.IntPtr System.RuntimeTypeHandle::value
	intptr_t ___value_0;

public:
	inline static int32_t get_offset_of_value_0() { return static_cast<int32_t>(offsetof(RuntimeTypeHandle_t7B542280A22F0EC4EAC2061C29178845847A8B2D, ___value_0)); }
	inline intptr_t get_value_0() const { return ___value_0; }
	inline intptr_t* get_address_of_value_0() { return &___value_0; }
	inline void set_value_0(intptr_t value)
	{
		___value_0 = value;
	}
};


// Unity.Collections.Allocator
struct  Allocator_t62A091275262E7067EAAD565B67764FA877D58D6 
{
public:
	// System.Int32 Unity.Collections.Allocator::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(Allocator_t62A091275262E7067EAAD565B67764FA877D58D6, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};


// UnityEngine.Object
struct  Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0  : public RuntimeObject
{
public:
	// System.IntPtr UnityEngine.Object::m_CachedPtr
	intptr_t ___m_CachedPtr_0;

public:
	inline static int32_t get_offset_of_m_CachedPtr_0() { return static_cast<int32_t>(offsetof(Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0, ___m_CachedPtr_0)); }
	inline intptr_t get_m_CachedPtr_0() const { return ___m_CachedPtr_0; }
	inline intptr_t* get_address_of_m_CachedPtr_0() { return &___m_CachedPtr_0; }
	inline void set_m_CachedPtr_0(intptr_t value)
	{
		___m_CachedPtr_0 = value;
	}
};

struct Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0_StaticFields
{
public:
	// System.Int32 UnityEngine.Object::OffsetOfInstanceIDInCPlusPlusObject
	int32_t ___OffsetOfInstanceIDInCPlusPlusObject_1;

public:
	inline static int32_t get_offset_of_OffsetOfInstanceIDInCPlusPlusObject_1() { return static_cast<int32_t>(offsetof(Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0_StaticFields, ___OffsetOfInstanceIDInCPlusPlusObject_1)); }
	inline int32_t get_OffsetOfInstanceIDInCPlusPlusObject_1() const { return ___OffsetOfInstanceIDInCPlusPlusObject_1; }
	inline int32_t* get_address_of_OffsetOfInstanceIDInCPlusPlusObject_1() { return &___OffsetOfInstanceIDInCPlusPlusObject_1; }
	inline void set_OffsetOfInstanceIDInCPlusPlusObject_1(int32_t value)
	{
		___OffsetOfInstanceIDInCPlusPlusObject_1 = value;
	}
};

// Native definition for P/Invoke marshalling of UnityEngine.Object
struct Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0_marshaled_pinvoke
{
	intptr_t ___m_CachedPtr_0;
};
// Native definition for COM marshalling of UnityEngine.Object
struct Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0_marshaled_com
{
	intptr_t ___m_CachedPtr_0;
};

// UnityEngine.Pose
struct  Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 
{
public:
	// UnityEngine.Vector3 UnityEngine.Pose::position
	Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  ___position_0;
	// UnityEngine.Quaternion UnityEngine.Pose::rotation
	Quaternion_t319F3319A7D43FFA5D819AD6C0A98851F0095357  ___rotation_1;

public:
	inline static int32_t get_offset_of_position_0() { return static_cast<int32_t>(offsetof(Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29, ___position_0)); }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  get_position_0() const { return ___position_0; }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720 * get_address_of_position_0() { return &___position_0; }
	inline void set_position_0(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  value)
	{
		___position_0 = value;
	}

	inline static int32_t get_offset_of_rotation_1() { return static_cast<int32_t>(offsetof(Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29, ___rotation_1)); }
	inline Quaternion_t319F3319A7D43FFA5D819AD6C0A98851F0095357  get_rotation_1() const { return ___rotation_1; }
	inline Quaternion_t319F3319A7D43FFA5D819AD6C0A98851F0095357 * get_address_of_rotation_1() { return &___rotation_1; }
	inline void set_rotation_1(Quaternion_t319F3319A7D43FFA5D819AD6C0A98851F0095357  value)
	{
		___rotation_1 = value;
	}
};

struct Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29_StaticFields
{
public:
	// UnityEngine.Pose UnityEngine.Pose::k_Identity
	Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___k_Identity_2;

public:
	inline static int32_t get_offset_of_k_Identity_2() { return static_cast<int32_t>(offsetof(Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29_StaticFields, ___k_Identity_2)); }
	inline Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  get_k_Identity_2() const { return ___k_Identity_2; }
	inline Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 * get_address_of_k_Identity_2() { return &___k_Identity_2; }
	inline void set_k_Identity_2(Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  value)
	{
		___k_Identity_2 = value;
	}
};


// UnityEngine.Ray
struct  Ray_tE2163D4CB3E6B267E29F8ABE41684490E4A614B2 
{
public:
	// UnityEngine.Vector3 UnityEngine.Ray::m_Origin
	Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  ___m_Origin_0;
	// UnityEngine.Vector3 UnityEngine.Ray::m_Direction
	Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  ___m_Direction_1;

public:
	inline static int32_t get_offset_of_m_Origin_0() { return static_cast<int32_t>(offsetof(Ray_tE2163D4CB3E6B267E29F8ABE41684490E4A614B2, ___m_Origin_0)); }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  get_m_Origin_0() const { return ___m_Origin_0; }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720 * get_address_of_m_Origin_0() { return &___m_Origin_0; }
	inline void set_m_Origin_0(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  value)
	{
		___m_Origin_0 = value;
	}

	inline static int32_t get_offset_of_m_Direction_1() { return static_cast<int32_t>(offsetof(Ray_tE2163D4CB3E6B267E29F8ABE41684490E4A614B2, ___m_Direction_1)); }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  get_m_Direction_1() const { return ___m_Direction_1; }
	inline Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720 * get_address_of_m_Direction_1() { return &___m_Direction_1; }
	inline void set_m_Direction_1(Vector3_tDCF05E21F632FE2BA260C06E0D10CA81513E6720  value)
	{
		___m_Direction_1 = value;
	}
};


// UnityEngine.ScreenOrientation
struct  ScreenOrientation_t4AB8E2E02033B0EAEA0260B05B1D88DA8058BB51 
{
public:
	// System.Int32 UnityEngine.ScreenOrientation::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(ScreenOrientation_t4AB8E2E02033B0EAEA0260B05B1D88DA8058BB51, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};


// UnityEngine.TextureFormat
struct  TextureFormat_t7C6B5101554065C47682E592D1E26079D4EC2DCE 
{
public:
	// System.Int32 UnityEngine.TextureFormat::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(TextureFormat_t7C6B5101554065C47682E592D1E26079D4EC2DCE, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};


// UnityEngine.XR.ARSubsystems.NotTrackingReason
struct  NotTrackingReason_t3106DE243E1555A213B3953CC3D001AC72B9C096 
{
public:
	// System.Int32 UnityEngine.XR.ARSubsystems.NotTrackingReason::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(NotTrackingReason_t3106DE243E1555A213B3953CC3D001AC72B9C096, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};


// UnityEngine.XR.ARSubsystems.SessionAvailability
struct  SessionAvailability_t5D8A6E743297DB42DFE5004CB3F6EF1A0EEAB5B9 
{
public:
	// System.Int32 UnityEngine.XR.ARSubsystems.SessionAvailability::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(SessionAvailability_t5D8A6E743297DB42DFE5004CB3F6EF1A0EEAB5B9, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};


// UnityEngine.XR.ARSubsystems.SessionInstallationStatus
struct  SessionInstallationStatus_t4030D915111F62D249BC71DE05C3BB4C856AADDF 
{
public:
	// System.Int32 UnityEngine.XR.ARSubsystems.SessionInstallationStatus::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(SessionInstallationStatus_t4030D915111F62D249BC71DE05C3BB4C856AADDF, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};


// UnityEngine.XR.ARSubsystems.TrackableType
struct  TrackableType_t078FFF635AE2E4FC51E7D7DB8AB1CB884D30EA1F 
{
public:
	// System.Int32 UnityEngine.XR.ARSubsystems.TrackableType::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(TrackableType_t078FFF635AE2E4FC51E7D7DB8AB1CB884D30EA1F, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};


// UnityEngine.XR.ARSubsystems.TrackingState
struct  TrackingState_t124D9E603E4E0453A85409CF7762EE8C946233F6 
{
public:
	// System.Int32 UnityEngine.XR.ARSubsystems.TrackingState::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(TrackingState_t124D9E603E4E0453A85409CF7762EE8C946233F6, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};


// UnityEngine.XR.ARSubsystems.XRReferenceImage
struct  XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E 
{
public:
	// UnityEngine.XR.ARSubsystems.SerializableGuid UnityEngine.XR.ARSubsystems.XRReferenceImage::m_SerializedGuid
	SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  ___m_SerializedGuid_0;
	// UnityEngine.XR.ARSubsystems.SerializableGuid UnityEngine.XR.ARSubsystems.XRReferenceImage::m_SerializedTextureGuid
	SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  ___m_SerializedTextureGuid_1;
	// UnityEngine.Vector2 UnityEngine.XR.ARSubsystems.XRReferenceImage::m_Size
	Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  ___m_Size_2;
	// System.Boolean UnityEngine.XR.ARSubsystems.XRReferenceImage::m_SpecifySize
	bool ___m_SpecifySize_3;
	// System.String UnityEngine.XR.ARSubsystems.XRReferenceImage::m_Name
	String_t* ___m_Name_4;
	// UnityEngine.Texture2D UnityEngine.XR.ARSubsystems.XRReferenceImage::m_Texture
	Texture2D_tBBF96AC337723E2EF156DF17E09D4379FD05DE1C * ___m_Texture_5;

public:
	inline static int32_t get_offset_of_m_SerializedGuid_0() { return static_cast<int32_t>(offsetof(XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E, ___m_SerializedGuid_0)); }
	inline SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  get_m_SerializedGuid_0() const { return ___m_SerializedGuid_0; }
	inline SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821 * get_address_of_m_SerializedGuid_0() { return &___m_SerializedGuid_0; }
	inline void set_m_SerializedGuid_0(SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  value)
	{
		___m_SerializedGuid_0 = value;
	}

	inline static int32_t get_offset_of_m_SerializedTextureGuid_1() { return static_cast<int32_t>(offsetof(XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E, ___m_SerializedTextureGuid_1)); }
	inline SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  get_m_SerializedTextureGuid_1() const { return ___m_SerializedTextureGuid_1; }
	inline SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821 * get_address_of_m_SerializedTextureGuid_1() { return &___m_SerializedTextureGuid_1; }
	inline void set_m_SerializedTextureGuid_1(SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  value)
	{
		___m_SerializedTextureGuid_1 = value;
	}

	inline static int32_t get_offset_of_m_Size_2() { return static_cast<int32_t>(offsetof(XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E, ___m_Size_2)); }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  get_m_Size_2() const { return ___m_Size_2; }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D * get_address_of_m_Size_2() { return &___m_Size_2; }
	inline void set_m_Size_2(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  value)
	{
		___m_Size_2 = value;
	}

	inline static int32_t get_offset_of_m_SpecifySize_3() { return static_cast<int32_t>(offsetof(XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E, ___m_SpecifySize_3)); }
	inline bool get_m_SpecifySize_3() const { return ___m_SpecifySize_3; }
	inline bool* get_address_of_m_SpecifySize_3() { return &___m_SpecifySize_3; }
	inline void set_m_SpecifySize_3(bool value)
	{
		___m_SpecifySize_3 = value;
	}

	inline static int32_t get_offset_of_m_Name_4() { return static_cast<int32_t>(offsetof(XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E, ___m_Name_4)); }
	inline String_t* get_m_Name_4() const { return ___m_Name_4; }
	inline String_t** get_address_of_m_Name_4() { return &___m_Name_4; }
	inline void set_m_Name_4(String_t* value)
	{
		___m_Name_4 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_Name_4), (void*)value);
	}

	inline static int32_t get_offset_of_m_Texture_5() { return static_cast<int32_t>(offsetof(XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E, ___m_Texture_5)); }
	inline Texture2D_tBBF96AC337723E2EF156DF17E09D4379FD05DE1C * get_m_Texture_5() const { return ___m_Texture_5; }
	inline Texture2D_tBBF96AC337723E2EF156DF17E09D4379FD05DE1C ** get_address_of_m_Texture_5() { return &___m_Texture_5; }
	inline void set_m_Texture_5(Texture2D_tBBF96AC337723E2EF156DF17E09D4379FD05DE1C * value)
	{
		___m_Texture_5 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_Texture_5), (void*)value);
	}
};

// Native definition for P/Invoke marshalling of UnityEngine.XR.ARSubsystems.XRReferenceImage
struct XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E_marshaled_pinvoke
{
	SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  ___m_SerializedGuid_0;
	SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  ___m_SerializedTextureGuid_1;
	Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  ___m_Size_2;
	int32_t ___m_SpecifySize_3;
	char* ___m_Name_4;
	Texture2D_tBBF96AC337723E2EF156DF17E09D4379FD05DE1C * ___m_Texture_5;
};
// Native definition for COM marshalling of UnityEngine.XR.ARSubsystems.XRReferenceImage
struct XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E_marshaled_com
{
	SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  ___m_SerializedGuid_0;
	SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  ___m_SerializedTextureGuid_1;
	Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  ___m_Size_2;
	int32_t ___m_SpecifySize_3;
	Il2CppChar* ___m_Name_4;
	Texture2D_tBBF96AC337723E2EF156DF17E09D4379FD05DE1C * ___m_Texture_5;
};

// UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor
struct  XRReferencePointSubsystemDescriptor_t5972C5E3DEF485A89A3AA7D9D47CCD8A67FDB74D  : public SubsystemDescriptor_1_tFFF984F9F6942FDAE965131A8954D4C4EA20B6B5
{
public:
	// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor::<supportsTrackableAttachments>k__BackingField
	bool ___U3CsupportsTrackableAttachmentsU3Ek__BackingField_2;

public:
	inline static int32_t get_offset_of_U3CsupportsTrackableAttachmentsU3Ek__BackingField_2() { return static_cast<int32_t>(offsetof(XRReferencePointSubsystemDescriptor_t5972C5E3DEF485A89A3AA7D9D47CCD8A67FDB74D, ___U3CsupportsTrackableAttachmentsU3Ek__BackingField_2)); }
	inline bool get_U3CsupportsTrackableAttachmentsU3Ek__BackingField_2() const { return ___U3CsupportsTrackableAttachmentsU3Ek__BackingField_2; }
	inline bool* get_address_of_U3CsupportsTrackableAttachmentsU3Ek__BackingField_2() { return &___U3CsupportsTrackableAttachmentsU3Ek__BackingField_2; }
	inline void set_U3CsupportsTrackableAttachmentsU3Ek__BackingField_2(bool value)
	{
		___U3CsupportsTrackableAttachmentsU3Ek__BackingField_2 = value;
	}
};


// UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor
struct  XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079  : public SubsystemDescriptor_1_t0679ED150419770F94E42102F28D688E8F1DC9DA
{
public:
	// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor::<supportsInstall>k__BackingField
	bool ___U3CsupportsInstallU3Ek__BackingField_2;
	// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor::<supportsMatchFrameRate>k__BackingField
	bool ___U3CsupportsMatchFrameRateU3Ek__BackingField_3;

public:
	inline static int32_t get_offset_of_U3CsupportsInstallU3Ek__BackingField_2() { return static_cast<int32_t>(offsetof(XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079, ___U3CsupportsInstallU3Ek__BackingField_2)); }
	inline bool get_U3CsupportsInstallU3Ek__BackingField_2() const { return ___U3CsupportsInstallU3Ek__BackingField_2; }
	inline bool* get_address_of_U3CsupportsInstallU3Ek__BackingField_2() { return &___U3CsupportsInstallU3Ek__BackingField_2; }
	inline void set_U3CsupportsInstallU3Ek__BackingField_2(bool value)
	{
		___U3CsupportsInstallU3Ek__BackingField_2 = value;
	}

	inline static int32_t get_offset_of_U3CsupportsMatchFrameRateU3Ek__BackingField_3() { return static_cast<int32_t>(offsetof(XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079, ___U3CsupportsMatchFrameRateU3Ek__BackingField_3)); }
	inline bool get_U3CsupportsMatchFrameRateU3Ek__BackingField_3() const { return ___U3CsupportsMatchFrameRateU3Ek__BackingField_3; }
	inline bool* get_address_of_U3CsupportsMatchFrameRateU3Ek__BackingField_3() { return &___U3CsupportsMatchFrameRateU3Ek__BackingField_3; }
	inline void set_U3CsupportsMatchFrameRateU3Ek__BackingField_3(bool value)
	{
		___U3CsupportsMatchFrameRateU3Ek__BackingField_3 = value;
	}
};


// UnityEngine.XR.ARSubsystems.XRSubsystem`1<UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor>
struct  XRSubsystem_1_tD0089F4B0849EA9C41ED01F79404C4A1A0B31B9B  : public Subsystem_1_t1D44399B8190A9E3533EE5C5D5915B75D79B2E66
{
public:
	// System.Boolean UnityEngine.XR.ARSubsystems.XRSubsystem`1::m_Running
	bool ___m_Running_1;
	// System.Boolean UnityEngine.XR.ARSubsystems.XRSubsystem`1::m_Destroyed
	bool ___m_Destroyed_2;

public:
	inline static int32_t get_offset_of_m_Running_1() { return static_cast<int32_t>(offsetof(XRSubsystem_1_tD0089F4B0849EA9C41ED01F79404C4A1A0B31B9B, ___m_Running_1)); }
	inline bool get_m_Running_1() const { return ___m_Running_1; }
	inline bool* get_address_of_m_Running_1() { return &___m_Running_1; }
	inline void set_m_Running_1(bool value)
	{
		___m_Running_1 = value;
	}

	inline static int32_t get_offset_of_m_Destroyed_2() { return static_cast<int32_t>(offsetof(XRSubsystem_1_tD0089F4B0849EA9C41ED01F79404C4A1A0B31B9B, ___m_Destroyed_2)); }
	inline bool get_m_Destroyed_2() const { return ___m_Destroyed_2; }
	inline bool* get_address_of_m_Destroyed_2() { return &___m_Destroyed_2; }
	inline void set_m_Destroyed_2(bool value)
	{
		___m_Destroyed_2 = value;
	}
};


// UnityEngine.XR.ARSubsystems.XRSubsystem`1<UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor>
struct  XRSubsystem_1_t4A91B9F3D884D2B92F50A94755FF14CA17429844  : public Subsystem_1_t1A38EF1402A7E5DBC48E0755E358926BB0054890
{
public:
	// System.Boolean UnityEngine.XR.ARSubsystems.XRSubsystem`1::m_Running
	bool ___m_Running_1;
	// System.Boolean UnityEngine.XR.ARSubsystems.XRSubsystem`1::m_Destroyed
	bool ___m_Destroyed_2;

public:
	inline static int32_t get_offset_of_m_Running_1() { return static_cast<int32_t>(offsetof(XRSubsystem_1_t4A91B9F3D884D2B92F50A94755FF14CA17429844, ___m_Running_1)); }
	inline bool get_m_Running_1() const { return ___m_Running_1; }
	inline bool* get_address_of_m_Running_1() { return &___m_Running_1; }
	inline void set_m_Running_1(bool value)
	{
		___m_Running_1 = value;
	}

	inline static int32_t get_offset_of_m_Destroyed_2() { return static_cast<int32_t>(offsetof(XRSubsystem_1_t4A91B9F3D884D2B92F50A94755FF14CA17429844, ___m_Destroyed_2)); }
	inline bool get_m_Destroyed_2() const { return ___m_Destroyed_2; }
	inline bool* get_address_of_m_Destroyed_2() { return &___m_Destroyed_2; }
	inline void set_m_Destroyed_2(bool value)
	{
		___m_Destroyed_2 = value;
	}
};


// UnityEngine.XR.ARSubsystems.XRSubsystem`1<UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor>
struct  XRSubsystem_1_t4F81A9C71B4D3ABB1A7FF9080551E6E0951A5F5B  : public Subsystem_1_t749909AA5D71BD615BDF40BDE7C8F81B8D63F4C7
{
public:
	// System.Boolean UnityEngine.XR.ARSubsystems.XRSubsystem`1::m_Running
	bool ___m_Running_1;
	// System.Boolean UnityEngine.XR.ARSubsystems.XRSubsystem`1::m_Destroyed
	bool ___m_Destroyed_2;

public:
	inline static int32_t get_offset_of_m_Running_1() { return static_cast<int32_t>(offsetof(XRSubsystem_1_t4F81A9C71B4D3ABB1A7FF9080551E6E0951A5F5B, ___m_Running_1)); }
	inline bool get_m_Running_1() const { return ___m_Running_1; }
	inline bool* get_address_of_m_Running_1() { return &___m_Running_1; }
	inline void set_m_Running_1(bool value)
	{
		___m_Running_1 = value;
	}

	inline static int32_t get_offset_of_m_Destroyed_2() { return static_cast<int32_t>(offsetof(XRSubsystem_1_t4F81A9C71B4D3ABB1A7FF9080551E6E0951A5F5B, ___m_Destroyed_2)); }
	inline bool get_m_Destroyed_2() const { return ___m_Destroyed_2; }
	inline bool* get_address_of_m_Destroyed_2() { return &___m_Destroyed_2; }
	inline void set_m_Destroyed_2(bool value)
	{
		___m_Destroyed_2 = value;
	}
};


// System.Collections.Generic.List`1_Enumerator<UnityEngine.XR.ARSubsystems.XRReferenceImage>
struct  Enumerator_t90B18C0FA1516886E459FA26C65233B656F793EB 
{
public:
	// System.Collections.Generic.List`1<T> System.Collections.Generic.List`1_Enumerator::list
	List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * ___list_0;
	// System.Int32 System.Collections.Generic.List`1_Enumerator::index
	int32_t ___index_1;
	// System.Int32 System.Collections.Generic.List`1_Enumerator::version
	int32_t ___version_2;
	// T System.Collections.Generic.List`1_Enumerator::current
	XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  ___current_3;

public:
	inline static int32_t get_offset_of_list_0() { return static_cast<int32_t>(offsetof(Enumerator_t90B18C0FA1516886E459FA26C65233B656F793EB, ___list_0)); }
	inline List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * get_list_0() const { return ___list_0; }
	inline List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F ** get_address_of_list_0() { return &___list_0; }
	inline void set_list_0(List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * value)
	{
		___list_0 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___list_0), (void*)value);
	}

	inline static int32_t get_offset_of_index_1() { return static_cast<int32_t>(offsetof(Enumerator_t90B18C0FA1516886E459FA26C65233B656F793EB, ___index_1)); }
	inline int32_t get_index_1() const { return ___index_1; }
	inline int32_t* get_address_of_index_1() { return &___index_1; }
	inline void set_index_1(int32_t value)
	{
		___index_1 = value;
	}

	inline static int32_t get_offset_of_version_2() { return static_cast<int32_t>(offsetof(Enumerator_t90B18C0FA1516886E459FA26C65233B656F793EB, ___version_2)); }
	inline int32_t get_version_2() const { return ___version_2; }
	inline int32_t* get_address_of_version_2() { return &___version_2; }
	inline void set_version_2(int32_t value)
	{
		___version_2 = value;
	}

	inline static int32_t get_offset_of_current_3() { return static_cast<int32_t>(offsetof(Enumerator_t90B18C0FA1516886E459FA26C65233B656F793EB, ___current_3)); }
	inline XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  get_current_3() const { return ___current_3; }
	inline XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * get_address_of_current_3() { return &___current_3; }
	inline void set_current_3(XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  value)
	{
		___current_3 = value;
		Il2CppCodeGenWriteBarrier((void**)&(((&___current_3))->___m_Name_4), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&(((&___current_3))->___m_Texture_5), (void*)NULL);
		#endif
	}
};


// System.SystemException
struct  SystemException_t5380468142AA850BE4A341D7AF3EAB9C78746782  : public Exception_t
{
public:

public:
};


// System.Type
struct  Type_t  : public MemberInfo_t
{
public:
	// System.RuntimeTypeHandle System.Type::_impl
	RuntimeTypeHandle_t7B542280A22F0EC4EAC2061C29178845847A8B2D  ____impl_9;

public:
	inline static int32_t get_offset_of__impl_9() { return static_cast<int32_t>(offsetof(Type_t, ____impl_9)); }
	inline RuntimeTypeHandle_t7B542280A22F0EC4EAC2061C29178845847A8B2D  get__impl_9() const { return ____impl_9; }
	inline RuntimeTypeHandle_t7B542280A22F0EC4EAC2061C29178845847A8B2D * get_address_of__impl_9() { return &____impl_9; }
	inline void set__impl_9(RuntimeTypeHandle_t7B542280A22F0EC4EAC2061C29178845847A8B2D  value)
	{
		____impl_9 = value;
	}
};

struct Type_t_StaticFields
{
public:
	// System.Reflection.MemberFilter System.Type::FilterAttribute
	MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 * ___FilterAttribute_0;
	// System.Reflection.MemberFilter System.Type::FilterName
	MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 * ___FilterName_1;
	// System.Reflection.MemberFilter System.Type::FilterNameIgnoreCase
	MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 * ___FilterNameIgnoreCase_2;
	// System.Object System.Type::Missing
	RuntimeObject * ___Missing_3;
	// System.Char System.Type::Delimiter
	Il2CppChar ___Delimiter_4;
	// System.Type[] System.Type::EmptyTypes
	TypeU5BU5D_t7FE623A666B49176DE123306221193E888A12F5F* ___EmptyTypes_5;
	// System.Reflection.Binder System.Type::defaultBinder
	Binder_t4D5CB06963501D32847C057B57157D6DC49CA759 * ___defaultBinder_6;

public:
	inline static int32_t get_offset_of_FilterAttribute_0() { return static_cast<int32_t>(offsetof(Type_t_StaticFields, ___FilterAttribute_0)); }
	inline MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 * get_FilterAttribute_0() const { return ___FilterAttribute_0; }
	inline MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 ** get_address_of_FilterAttribute_0() { return &___FilterAttribute_0; }
	inline void set_FilterAttribute_0(MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 * value)
	{
		___FilterAttribute_0 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___FilterAttribute_0), (void*)value);
	}

	inline static int32_t get_offset_of_FilterName_1() { return static_cast<int32_t>(offsetof(Type_t_StaticFields, ___FilterName_1)); }
	inline MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 * get_FilterName_1() const { return ___FilterName_1; }
	inline MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 ** get_address_of_FilterName_1() { return &___FilterName_1; }
	inline void set_FilterName_1(MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 * value)
	{
		___FilterName_1 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___FilterName_1), (void*)value);
	}

	inline static int32_t get_offset_of_FilterNameIgnoreCase_2() { return static_cast<int32_t>(offsetof(Type_t_StaticFields, ___FilterNameIgnoreCase_2)); }
	inline MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 * get_FilterNameIgnoreCase_2() const { return ___FilterNameIgnoreCase_2; }
	inline MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 ** get_address_of_FilterNameIgnoreCase_2() { return &___FilterNameIgnoreCase_2; }
	inline void set_FilterNameIgnoreCase_2(MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 * value)
	{
		___FilterNameIgnoreCase_2 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___FilterNameIgnoreCase_2), (void*)value);
	}

	inline static int32_t get_offset_of_Missing_3() { return static_cast<int32_t>(offsetof(Type_t_StaticFields, ___Missing_3)); }
	inline RuntimeObject * get_Missing_3() const { return ___Missing_3; }
	inline RuntimeObject ** get_address_of_Missing_3() { return &___Missing_3; }
	inline void set_Missing_3(RuntimeObject * value)
	{
		___Missing_3 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___Missing_3), (void*)value);
	}

	inline static int32_t get_offset_of_Delimiter_4() { return static_cast<int32_t>(offsetof(Type_t_StaticFields, ___Delimiter_4)); }
	inline Il2CppChar get_Delimiter_4() const { return ___Delimiter_4; }
	inline Il2CppChar* get_address_of_Delimiter_4() { return &___Delimiter_4; }
	inline void set_Delimiter_4(Il2CppChar value)
	{
		___Delimiter_4 = value;
	}

	inline static int32_t get_offset_of_EmptyTypes_5() { return static_cast<int32_t>(offsetof(Type_t_StaticFields, ___EmptyTypes_5)); }
	inline TypeU5BU5D_t7FE623A666B49176DE123306221193E888A12F5F* get_EmptyTypes_5() const { return ___EmptyTypes_5; }
	inline TypeU5BU5D_t7FE623A666B49176DE123306221193E888A12F5F** get_address_of_EmptyTypes_5() { return &___EmptyTypes_5; }
	inline void set_EmptyTypes_5(TypeU5BU5D_t7FE623A666B49176DE123306221193E888A12F5F* value)
	{
		___EmptyTypes_5 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___EmptyTypes_5), (void*)value);
	}

	inline static int32_t get_offset_of_defaultBinder_6() { return static_cast<int32_t>(offsetof(Type_t_StaticFields, ___defaultBinder_6)); }
	inline Binder_t4D5CB06963501D32847C057B57157D6DC49CA759 * get_defaultBinder_6() const { return ___defaultBinder_6; }
	inline Binder_t4D5CB06963501D32847C057B57157D6DC49CA759 ** get_address_of_defaultBinder_6() { return &___defaultBinder_6; }
	inline void set_defaultBinder_6(Binder_t4D5CB06963501D32847C057B57157D6DC49CA759 * value)
	{
		___defaultBinder_6 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___defaultBinder_6), (void*)value);
	}
};


// Unity.Collections.NativeArray`1<System.Single>
struct  NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 
{
public:
	// System.Void* Unity.Collections.NativeArray`1::m_Buffer
	void* ___m_Buffer_0;
	// System.Int32 Unity.Collections.NativeArray`1::m_Length
	int32_t ___m_Length_1;
	// Unity.Collections.Allocator Unity.Collections.NativeArray`1::m_AllocatorLabel
	int32_t ___m_AllocatorLabel_2;

public:
	inline static int32_t get_offset_of_m_Buffer_0() { return static_cast<int32_t>(offsetof(NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67, ___m_Buffer_0)); }
	inline void* get_m_Buffer_0() const { return ___m_Buffer_0; }
	inline void** get_address_of_m_Buffer_0() { return &___m_Buffer_0; }
	inline void set_m_Buffer_0(void* value)
	{
		___m_Buffer_0 = value;
	}

	inline static int32_t get_offset_of_m_Length_1() { return static_cast<int32_t>(offsetof(NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67, ___m_Length_1)); }
	inline int32_t get_m_Length_1() const { return ___m_Length_1; }
	inline int32_t* get_address_of_m_Length_1() { return &___m_Length_1; }
	inline void set_m_Length_1(int32_t value)
	{
		___m_Length_1 = value;
	}

	inline static int32_t get_offset_of_m_AllocatorLabel_2() { return static_cast<int32_t>(offsetof(NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67, ___m_AllocatorLabel_2)); }
	inline int32_t get_m_AllocatorLabel_2() const { return ___m_AllocatorLabel_2; }
	inline int32_t* get_address_of_m_AllocatorLabel_2() { return &___m_AllocatorLabel_2; }
	inline void set_m_AllocatorLabel_2(int32_t value)
	{
		___m_AllocatorLabel_2 = value;
	}
};

// Native definition for P/Invoke marshalling of Unity.Collections.NativeArray`1
#ifndef NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_pinvoke_define
#define NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_pinvoke_define
struct NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_pinvoke
{
	void* ___m_Buffer_0;
	int32_t ___m_Length_1;
	int32_t ___m_AllocatorLabel_2;
};
#endif
// Native definition for COM marshalling of Unity.Collections.NativeArray`1
#ifndef NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_com_define
#define NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_com_define
struct NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_com
{
	void* ___m_Buffer_0;
	int32_t ___m_Length_1;
	int32_t ___m_AllocatorLabel_2;
};
#endif

// Unity.Collections.NativeArray`1<System.UInt64>
struct  NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C 
{
public:
	// System.Void* Unity.Collections.NativeArray`1::m_Buffer
	void* ___m_Buffer_0;
	// System.Int32 Unity.Collections.NativeArray`1::m_Length
	int32_t ___m_Length_1;
	// Unity.Collections.Allocator Unity.Collections.NativeArray`1::m_AllocatorLabel
	int32_t ___m_AllocatorLabel_2;

public:
	inline static int32_t get_offset_of_m_Buffer_0() { return static_cast<int32_t>(offsetof(NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C, ___m_Buffer_0)); }
	inline void* get_m_Buffer_0() const { return ___m_Buffer_0; }
	inline void** get_address_of_m_Buffer_0() { return &___m_Buffer_0; }
	inline void set_m_Buffer_0(void* value)
	{
		___m_Buffer_0 = value;
	}

	inline static int32_t get_offset_of_m_Length_1() { return static_cast<int32_t>(offsetof(NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C, ___m_Length_1)); }
	inline int32_t get_m_Length_1() const { return ___m_Length_1; }
	inline int32_t* get_address_of_m_Length_1() { return &___m_Length_1; }
	inline void set_m_Length_1(int32_t value)
	{
		___m_Length_1 = value;
	}

	inline static int32_t get_offset_of_m_AllocatorLabel_2() { return static_cast<int32_t>(offsetof(NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C, ___m_AllocatorLabel_2)); }
	inline int32_t get_m_AllocatorLabel_2() const { return ___m_AllocatorLabel_2; }
	inline int32_t* get_address_of_m_AllocatorLabel_2() { return &___m_AllocatorLabel_2; }
	inline void set_m_AllocatorLabel_2(int32_t value)
	{
		___m_AllocatorLabel_2 = value;
	}
};

// Native definition for P/Invoke marshalling of Unity.Collections.NativeArray`1
#ifndef NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_pinvoke_define
#define NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_pinvoke_define
struct NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_pinvoke
{
	void* ___m_Buffer_0;
	int32_t ___m_Length_1;
	int32_t ___m_AllocatorLabel_2;
};
#endif
// Native definition for COM marshalling of Unity.Collections.NativeArray`1
#ifndef NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_com_define
#define NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_com_define
struct NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_com
{
	void* ___m_Buffer_0;
	int32_t ___m_Length_1;
	int32_t ___m_AllocatorLabel_2;
};
#endif

// Unity.Collections.NativeArray`1<UnityEngine.Vector3>
struct  NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 
{
public:
	// System.Void* Unity.Collections.NativeArray`1::m_Buffer
	void* ___m_Buffer_0;
	// System.Int32 Unity.Collections.NativeArray`1::m_Length
	int32_t ___m_Length_1;
	// Unity.Collections.Allocator Unity.Collections.NativeArray`1::m_AllocatorLabel
	int32_t ___m_AllocatorLabel_2;

public:
	inline static int32_t get_offset_of_m_Buffer_0() { return static_cast<int32_t>(offsetof(NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74, ___m_Buffer_0)); }
	inline void* get_m_Buffer_0() const { return ___m_Buffer_0; }
	inline void** get_address_of_m_Buffer_0() { return &___m_Buffer_0; }
	inline void set_m_Buffer_0(void* value)
	{
		___m_Buffer_0 = value;
	}

	inline static int32_t get_offset_of_m_Length_1() { return static_cast<int32_t>(offsetof(NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74, ___m_Length_1)); }
	inline int32_t get_m_Length_1() const { return ___m_Length_1; }
	inline int32_t* get_address_of_m_Length_1() { return &___m_Length_1; }
	inline void set_m_Length_1(int32_t value)
	{
		___m_Length_1 = value;
	}

	inline static int32_t get_offset_of_m_AllocatorLabel_2() { return static_cast<int32_t>(offsetof(NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74, ___m_AllocatorLabel_2)); }
	inline int32_t get_m_AllocatorLabel_2() const { return ___m_AllocatorLabel_2; }
	inline int32_t* get_address_of_m_AllocatorLabel_2() { return &___m_AllocatorLabel_2; }
	inline void set_m_AllocatorLabel_2(int32_t value)
	{
		___m_AllocatorLabel_2 = value;
	}
};

// Native definition for P/Invoke marshalling of Unity.Collections.NativeArray`1
#ifndef NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_pinvoke_define
#define NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_pinvoke_define
struct NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_pinvoke
{
	void* ___m_Buffer_0;
	int32_t ___m_Length_1;
	int32_t ___m_AllocatorLabel_2;
};
#endif
// Native definition for COM marshalling of Unity.Collections.NativeArray`1
#ifndef NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_com_define
#define NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_com_define
struct NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_com
{
	void* ___m_Buffer_0;
	int32_t ___m_Length_1;
	int32_t ___m_AllocatorLabel_2;
};
#endif

// Unity.Collections.NativeArray`1<UnityEngine.XR.ARSubsystems.TrackableId>
struct  NativeArray_1_tD7797CC5848BA9ECBD1172056474C8DD8022B1D6 
{
public:
	// System.Void* Unity.Collections.NativeArray`1::m_Buffer
	void* ___m_Buffer_0;
	// System.Int32 Unity.Collections.NativeArray`1::m_Length
	int32_t ___m_Length_1;
	// Unity.Collections.Allocator Unity.Collections.NativeArray`1::m_AllocatorLabel
	int32_t ___m_AllocatorLabel_2;

public:
	inline static int32_t get_offset_of_m_Buffer_0() { return static_cast<int32_t>(offsetof(NativeArray_1_tD7797CC5848BA9ECBD1172056474C8DD8022B1D6, ___m_Buffer_0)); }
	inline void* get_m_Buffer_0() const { return ___m_Buffer_0; }
	inline void** get_address_of_m_Buffer_0() { return &___m_Buffer_0; }
	inline void set_m_Buffer_0(void* value)
	{
		___m_Buffer_0 = value;
	}

	inline static int32_t get_offset_of_m_Length_1() { return static_cast<int32_t>(offsetof(NativeArray_1_tD7797CC5848BA9ECBD1172056474C8DD8022B1D6, ___m_Length_1)); }
	inline int32_t get_m_Length_1() const { return ___m_Length_1; }
	inline int32_t* get_address_of_m_Length_1() { return &___m_Length_1; }
	inline void set_m_Length_1(int32_t value)
	{
		___m_Length_1 = value;
	}

	inline static int32_t get_offset_of_m_AllocatorLabel_2() { return static_cast<int32_t>(offsetof(NativeArray_1_tD7797CC5848BA9ECBD1172056474C8DD8022B1D6, ___m_AllocatorLabel_2)); }
	inline int32_t get_m_AllocatorLabel_2() const { return ___m_AllocatorLabel_2; }
	inline int32_t* get_address_of_m_AllocatorLabel_2() { return &___m_AllocatorLabel_2; }
	inline void set_m_AllocatorLabel_2(int32_t value)
	{
		___m_AllocatorLabel_2 = value;
	}
};

// Native definition for P/Invoke marshalling of Unity.Collections.NativeArray`1
#ifndef NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_pinvoke_define
#define NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_pinvoke_define
struct NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_pinvoke
{
	void* ___m_Buffer_0;
	int32_t ___m_Length_1;
	int32_t ___m_AllocatorLabel_2;
};
#endif
// Native definition for COM marshalling of Unity.Collections.NativeArray`1
#ifndef NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_com_define
#define NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_com_define
struct NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_com
{
	void* ___m_Buffer_0;
	int32_t ___m_Length_1;
	int32_t ___m_AllocatorLabel_2;
};
#endif

// Unity.Collections.NativeArray`1<UnityEngine.XR.ARSubsystems.XRRaycastHit>
struct  NativeArray_1_t769CF3061467D3B5B0062090193576AD726411C1 
{
public:
	// System.Void* Unity.Collections.NativeArray`1::m_Buffer
	void* ___m_Buffer_0;
	// System.Int32 Unity.Collections.NativeArray`1::m_Length
	int32_t ___m_Length_1;
	// Unity.Collections.Allocator Unity.Collections.NativeArray`1::m_AllocatorLabel
	int32_t ___m_AllocatorLabel_2;

public:
	inline static int32_t get_offset_of_m_Buffer_0() { return static_cast<int32_t>(offsetof(NativeArray_1_t769CF3061467D3B5B0062090193576AD726411C1, ___m_Buffer_0)); }
	inline void* get_m_Buffer_0() const { return ___m_Buffer_0; }
	inline void** get_address_of_m_Buffer_0() { return &___m_Buffer_0; }
	inline void set_m_Buffer_0(void* value)
	{
		___m_Buffer_0 = value;
	}

	inline static int32_t get_offset_of_m_Length_1() { return static_cast<int32_t>(offsetof(NativeArray_1_t769CF3061467D3B5B0062090193576AD726411C1, ___m_Length_1)); }
	inline int32_t get_m_Length_1() const { return ___m_Length_1; }
	inline int32_t* get_address_of_m_Length_1() { return &___m_Length_1; }
	inline void set_m_Length_1(int32_t value)
	{
		___m_Length_1 = value;
	}

	inline static int32_t get_offset_of_m_AllocatorLabel_2() { return static_cast<int32_t>(offsetof(NativeArray_1_t769CF3061467D3B5B0062090193576AD726411C1, ___m_AllocatorLabel_2)); }
	inline int32_t get_m_AllocatorLabel_2() const { return ___m_AllocatorLabel_2; }
	inline int32_t* get_address_of_m_AllocatorLabel_2() { return &___m_AllocatorLabel_2; }
	inline void set_m_AllocatorLabel_2(int32_t value)
	{
		___m_AllocatorLabel_2 = value;
	}
};

// Native definition for P/Invoke marshalling of Unity.Collections.NativeArray`1
#ifndef NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_pinvoke_define
#define NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_pinvoke_define
struct NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_pinvoke
{
	void* ___m_Buffer_0;
	int32_t ___m_Length_1;
	int32_t ___m_AllocatorLabel_2;
};
#endif
// Native definition for COM marshalling of Unity.Collections.NativeArray`1
#ifndef NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_com_define
#define NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_com_define
struct NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_com
{
	void* ___m_Buffer_0;
	int32_t ___m_Length_1;
	int32_t ___m_AllocatorLabel_2;
};
#endif

// Unity.Collections.NativeArray`1<UnityEngine.XR.ARSubsystems.XRReferencePoint>
struct  NativeArray_1_t5AADFB4C72573FE3017795F15B8CBC88625A8876 
{
public:
	// System.Void* Unity.Collections.NativeArray`1::m_Buffer
	void* ___m_Buffer_0;
	// System.Int32 Unity.Collections.NativeArray`1::m_Length
	int32_t ___m_Length_1;
	// Unity.Collections.Allocator Unity.Collections.NativeArray`1::m_AllocatorLabel
	int32_t ___m_AllocatorLabel_2;

public:
	inline static int32_t get_offset_of_m_Buffer_0() { return static_cast<int32_t>(offsetof(NativeArray_1_t5AADFB4C72573FE3017795F15B8CBC88625A8876, ___m_Buffer_0)); }
	inline void* get_m_Buffer_0() const { return ___m_Buffer_0; }
	inline void** get_address_of_m_Buffer_0() { return &___m_Buffer_0; }
	inline void set_m_Buffer_0(void* value)
	{
		___m_Buffer_0 = value;
	}

	inline static int32_t get_offset_of_m_Length_1() { return static_cast<int32_t>(offsetof(NativeArray_1_t5AADFB4C72573FE3017795F15B8CBC88625A8876, ___m_Length_1)); }
	inline int32_t get_m_Length_1() const { return ___m_Length_1; }
	inline int32_t* get_address_of_m_Length_1() { return &___m_Length_1; }
	inline void set_m_Length_1(int32_t value)
	{
		___m_Length_1 = value;
	}

	inline static int32_t get_offset_of_m_AllocatorLabel_2() { return static_cast<int32_t>(offsetof(NativeArray_1_t5AADFB4C72573FE3017795F15B8CBC88625A8876, ___m_AllocatorLabel_2)); }
	inline int32_t get_m_AllocatorLabel_2() const { return ___m_AllocatorLabel_2; }
	inline int32_t* get_address_of_m_AllocatorLabel_2() { return &___m_AllocatorLabel_2; }
	inline void set_m_AllocatorLabel_2(int32_t value)
	{
		___m_AllocatorLabel_2 = value;
	}
};

// Native definition for P/Invoke marshalling of Unity.Collections.NativeArray`1
#ifndef NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_pinvoke_define
#define NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_pinvoke_define
struct NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_pinvoke
{
	void* ___m_Buffer_0;
	int32_t ___m_Length_1;
	int32_t ___m_AllocatorLabel_2;
};
#endif
// Native definition for COM marshalling of Unity.Collections.NativeArray`1
#ifndef NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_com_define
#define NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_com_define
struct NativeArray_1_t350F3793D2FE9D9CD5A50725BE978ED846FE3098_marshaled_com
{
	void* ___m_Buffer_0;
	int32_t ___m_Length_1;
	int32_t ___m_AllocatorLabel_2;
};
#endif

// UnityEngine.ScriptableObject
struct  ScriptableObject_tAB015486CEAB714DA0D5C1BA389B84FB90427734  : public Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0
{
public:

public:
};

// Native definition for P/Invoke marshalling of UnityEngine.ScriptableObject
struct ScriptableObject_tAB015486CEAB714DA0D5C1BA389B84FB90427734_marshaled_pinvoke : public Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0_marshaled_pinvoke
{
};
// Native definition for COM marshalling of UnityEngine.ScriptableObject
struct ScriptableObject_tAB015486CEAB714DA0D5C1BA389B84FB90427734_marshaled_com : public Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0_marshaled_com
{
};

// UnityEngine.Texture
struct  Texture_t387FE83BB848001FD06B14707AEA6D5A0F6A95F4  : public Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0
{
public:

public:
};

struct Texture_t387FE83BB848001FD06B14707AEA6D5A0F6A95F4_StaticFields
{
public:
	// System.Int32 UnityEngine.Texture::GenerateAllMips
	int32_t ___GenerateAllMips_4;

public:
	inline static int32_t get_offset_of_GenerateAllMips_4() { return static_cast<int32_t>(offsetof(Texture_t387FE83BB848001FD06B14707AEA6D5A0F6A95F4_StaticFields, ___GenerateAllMips_4)); }
	inline int32_t get_GenerateAllMips_4() const { return ___GenerateAllMips_4; }
	inline int32_t* get_address_of_GenerateAllMips_4() { return &___GenerateAllMips_4; }
	inline void set_GenerateAllMips_4(int32_t value)
	{
		___GenerateAllMips_4 = value;
	}
};


// UnityEngine.XR.ARSubsystems.Promise`1<UnityEngine.XR.ARSubsystems.SessionAvailability>
struct  Promise_1_t7E6FC51116CF1FB8FFF9D4D6621E5F7231AE9A6B  : public CustomYieldInstruction_t819BB0973AFF22766749FF087B8AEFEAF3C2CB7D
{
public:
	// T UnityEngine.XR.ARSubsystems.Promise`1::<result>k__BackingField
	int32_t ___U3CresultU3Ek__BackingField_0;
	// System.Boolean UnityEngine.XR.ARSubsystems.Promise`1::m_Complete
	bool ___m_Complete_1;

public:
	inline static int32_t get_offset_of_U3CresultU3Ek__BackingField_0() { return static_cast<int32_t>(offsetof(Promise_1_t7E6FC51116CF1FB8FFF9D4D6621E5F7231AE9A6B, ___U3CresultU3Ek__BackingField_0)); }
	inline int32_t get_U3CresultU3Ek__BackingField_0() const { return ___U3CresultU3Ek__BackingField_0; }
	inline int32_t* get_address_of_U3CresultU3Ek__BackingField_0() { return &___U3CresultU3Ek__BackingField_0; }
	inline void set_U3CresultU3Ek__BackingField_0(int32_t value)
	{
		___U3CresultU3Ek__BackingField_0 = value;
	}

	inline static int32_t get_offset_of_m_Complete_1() { return static_cast<int32_t>(offsetof(Promise_1_t7E6FC51116CF1FB8FFF9D4D6621E5F7231AE9A6B, ___m_Complete_1)); }
	inline bool get_m_Complete_1() const { return ___m_Complete_1; }
	inline bool* get_address_of_m_Complete_1() { return &___m_Complete_1; }
	inline void set_m_Complete_1(bool value)
	{
		___m_Complete_1 = value;
	}
};


// UnityEngine.XR.ARSubsystems.Promise`1<UnityEngine.XR.ARSubsystems.SessionInstallationStatus>
struct  Promise_1_tE8B65103AAD59FA1265B1977CE59DC6DA5220626  : public CustomYieldInstruction_t819BB0973AFF22766749FF087B8AEFEAF3C2CB7D
{
public:
	// T UnityEngine.XR.ARSubsystems.Promise`1::<result>k__BackingField
	int32_t ___U3CresultU3Ek__BackingField_0;
	// System.Boolean UnityEngine.XR.ARSubsystems.Promise`1::m_Complete
	bool ___m_Complete_1;

public:
	inline static int32_t get_offset_of_U3CresultU3Ek__BackingField_0() { return static_cast<int32_t>(offsetof(Promise_1_tE8B65103AAD59FA1265B1977CE59DC6DA5220626, ___U3CresultU3Ek__BackingField_0)); }
	inline int32_t get_U3CresultU3Ek__BackingField_0() const { return ___U3CresultU3Ek__BackingField_0; }
	inline int32_t* get_address_of_U3CresultU3Ek__BackingField_0() { return &___U3CresultU3Ek__BackingField_0; }
	inline void set_U3CresultU3Ek__BackingField_0(int32_t value)
	{
		___U3CresultU3Ek__BackingField_0 = value;
	}

	inline static int32_t get_offset_of_m_Complete_1() { return static_cast<int32_t>(offsetof(Promise_1_tE8B65103AAD59FA1265B1977CE59DC6DA5220626, ___m_Complete_1)); }
	inline bool get_m_Complete_1() const { return ___m_Complete_1; }
	inline bool* get_address_of_m_Complete_1() { return &___m_Complete_1; }
	inline void set_m_Complete_1(bool value)
	{
		___m_Complete_1 = value;
	}
};


// UnityEngine.XR.ARSubsystems.TrackingSubsystem`2<UnityEngine.XR.ARSubsystems.XRReferencePoint,UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor>
struct  TrackingSubsystem_2_t62AFA2295FCEFC2C3818E3B9EDB3C1AF80509899  : public XRSubsystem_1_t4A91B9F3D884D2B92F50A94755FF14CA17429844
{
public:

public:
};


// UnityEngine.XR.ARSubsystems.XRRaycastHit
struct  XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 
{
public:
	// UnityEngine.XR.ARSubsystems.TrackableId UnityEngine.XR.ARSubsystems.XRRaycastHit::m_TrackableId
	TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___m_TrackableId_1;
	// UnityEngine.Pose UnityEngine.XR.ARSubsystems.XRRaycastHit::m_Pose
	Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___m_Pose_2;
	// System.Single UnityEngine.XR.ARSubsystems.XRRaycastHit::m_Distance
	float ___m_Distance_3;
	// UnityEngine.XR.ARSubsystems.TrackableType UnityEngine.XR.ARSubsystems.XRRaycastHit::m_HitType
	int32_t ___m_HitType_4;

public:
	inline static int32_t get_offset_of_m_TrackableId_1() { return static_cast<int32_t>(offsetof(XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82, ___m_TrackableId_1)); }
	inline TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  get_m_TrackableId_1() const { return ___m_TrackableId_1; }
	inline TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 * get_address_of_m_TrackableId_1() { return &___m_TrackableId_1; }
	inline void set_m_TrackableId_1(TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  value)
	{
		___m_TrackableId_1 = value;
	}

	inline static int32_t get_offset_of_m_Pose_2() { return static_cast<int32_t>(offsetof(XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82, ___m_Pose_2)); }
	inline Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  get_m_Pose_2() const { return ___m_Pose_2; }
	inline Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 * get_address_of_m_Pose_2() { return &___m_Pose_2; }
	inline void set_m_Pose_2(Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  value)
	{
		___m_Pose_2 = value;
	}

	inline static int32_t get_offset_of_m_Distance_3() { return static_cast<int32_t>(offsetof(XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82, ___m_Distance_3)); }
	inline float get_m_Distance_3() const { return ___m_Distance_3; }
	inline float* get_address_of_m_Distance_3() { return &___m_Distance_3; }
	inline void set_m_Distance_3(float value)
	{
		___m_Distance_3 = value;
	}

	inline static int32_t get_offset_of_m_HitType_4() { return static_cast<int32_t>(offsetof(XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82, ___m_HitType_4)); }
	inline int32_t get_m_HitType_4() const { return ___m_HitType_4; }
	inline int32_t* get_address_of_m_HitType_4() { return &___m_HitType_4; }
	inline void set_m_HitType_4(int32_t value)
	{
		___m_HitType_4 = value;
	}
};

struct XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82_StaticFields
{
public:
	// UnityEngine.XR.ARSubsystems.XRRaycastHit UnityEngine.XR.ARSubsystems.XRRaycastHit::s_Default
	XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  ___s_Default_0;

public:
	inline static int32_t get_offset_of_s_Default_0() { return static_cast<int32_t>(offsetof(XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82_StaticFields, ___s_Default_0)); }
	inline XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  get_s_Default_0() const { return ___s_Default_0; }
	inline XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * get_address_of_s_Default_0() { return &___s_Default_0; }
	inline void set_s_Default_0(XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  value)
	{
		___s_Default_0 = value;
	}
};


// UnityEngine.XR.ARSubsystems.XRRaycastSubsystem
struct  XRRaycastSubsystem_t1181EA314910ABB4E1F50BF2F7650EC1512A0A20  : public XRSubsystem_1_tD0089F4B0849EA9C41ED01F79404C4A1A0B31B9B
{
public:
	// UnityEngine.XR.ARSubsystems.XRRaycastSubsystem_Provider UnityEngine.XR.ARSubsystems.XRRaycastSubsystem::m_Provider
	Provider_t1231BE7466AD9536EA5EB62866FF0B6F07452732 * ___m_Provider_3;

public:
	inline static int32_t get_offset_of_m_Provider_3() { return static_cast<int32_t>(offsetof(XRRaycastSubsystem_t1181EA314910ABB4E1F50BF2F7650EC1512A0A20, ___m_Provider_3)); }
	inline Provider_t1231BE7466AD9536EA5EB62866FF0B6F07452732 * get_m_Provider_3() const { return ___m_Provider_3; }
	inline Provider_t1231BE7466AD9536EA5EB62866FF0B6F07452732 ** get_address_of_m_Provider_3() { return &___m_Provider_3; }
	inline void set_m_Provider_3(Provider_t1231BE7466AD9536EA5EB62866FF0B6F07452732 * value)
	{
		___m_Provider_3 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_Provider_3), (void*)value);
	}
};


// UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor
struct  XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7  : public SubsystemDescriptor_1_t27A8F3D552922AA16C799BCC575330BC6DF59F47
{
public:
	// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor::<supportsViewportBasedRaycast>k__BackingField
	bool ___U3CsupportsViewportBasedRaycastU3Ek__BackingField_2;
	// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor::<supportsWorldBasedRaycast>k__BackingField
	bool ___U3CsupportsWorldBasedRaycastU3Ek__BackingField_3;
	// UnityEngine.XR.ARSubsystems.TrackableType UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor::<supportedTrackableTypes>k__BackingField
	int32_t ___U3CsupportedTrackableTypesU3Ek__BackingField_4;

public:
	inline static int32_t get_offset_of_U3CsupportsViewportBasedRaycastU3Ek__BackingField_2() { return static_cast<int32_t>(offsetof(XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7, ___U3CsupportsViewportBasedRaycastU3Ek__BackingField_2)); }
	inline bool get_U3CsupportsViewportBasedRaycastU3Ek__BackingField_2() const { return ___U3CsupportsViewportBasedRaycastU3Ek__BackingField_2; }
	inline bool* get_address_of_U3CsupportsViewportBasedRaycastU3Ek__BackingField_2() { return &___U3CsupportsViewportBasedRaycastU3Ek__BackingField_2; }
	inline void set_U3CsupportsViewportBasedRaycastU3Ek__BackingField_2(bool value)
	{
		___U3CsupportsViewportBasedRaycastU3Ek__BackingField_2 = value;
	}

	inline static int32_t get_offset_of_U3CsupportsWorldBasedRaycastU3Ek__BackingField_3() { return static_cast<int32_t>(offsetof(XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7, ___U3CsupportsWorldBasedRaycastU3Ek__BackingField_3)); }
	inline bool get_U3CsupportsWorldBasedRaycastU3Ek__BackingField_3() const { return ___U3CsupportsWorldBasedRaycastU3Ek__BackingField_3; }
	inline bool* get_address_of_U3CsupportsWorldBasedRaycastU3Ek__BackingField_3() { return &___U3CsupportsWorldBasedRaycastU3Ek__BackingField_3; }
	inline void set_U3CsupportsWorldBasedRaycastU3Ek__BackingField_3(bool value)
	{
		___U3CsupportsWorldBasedRaycastU3Ek__BackingField_3 = value;
	}

	inline static int32_t get_offset_of_U3CsupportedTrackableTypesU3Ek__BackingField_4() { return static_cast<int32_t>(offsetof(XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7, ___U3CsupportedTrackableTypesU3Ek__BackingField_4)); }
	inline int32_t get_U3CsupportedTrackableTypesU3Ek__BackingField_4() const { return ___U3CsupportedTrackableTypesU3Ek__BackingField_4; }
	inline int32_t* get_address_of_U3CsupportedTrackableTypesU3Ek__BackingField_4() { return &___U3CsupportedTrackableTypesU3Ek__BackingField_4; }
	inline void set_U3CsupportedTrackableTypesU3Ek__BackingField_4(int32_t value)
	{
		___U3CsupportedTrackableTypesU3Ek__BackingField_4 = value;
	}
};


// UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo
struct  Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 
{
public:
	// System.String UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::<id>k__BackingField
	String_t* ___U3CidU3Ek__BackingField_0;
	// System.Type UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::<subsystemImplementationType>k__BackingField
	Type_t * ___U3CsubsystemImplementationTypeU3Ek__BackingField_1;
	// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::<supportsViewportBasedRaycast>k__BackingField
	bool ___U3CsupportsViewportBasedRaycastU3Ek__BackingField_2;
	// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::<supportsWorldBasedRaycast>k__BackingField
	bool ___U3CsupportsWorldBasedRaycastU3Ek__BackingField_3;
	// UnityEngine.XR.ARSubsystems.TrackableType UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::<supportedTrackableTypes>k__BackingField
	int32_t ___U3CsupportedTrackableTypesU3Ek__BackingField_4;

public:
	inline static int32_t get_offset_of_U3CidU3Ek__BackingField_0() { return static_cast<int32_t>(offsetof(Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704, ___U3CidU3Ek__BackingField_0)); }
	inline String_t* get_U3CidU3Ek__BackingField_0() const { return ___U3CidU3Ek__BackingField_0; }
	inline String_t** get_address_of_U3CidU3Ek__BackingField_0() { return &___U3CidU3Ek__BackingField_0; }
	inline void set_U3CidU3Ek__BackingField_0(String_t* value)
	{
		___U3CidU3Ek__BackingField_0 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___U3CidU3Ek__BackingField_0), (void*)value);
	}

	inline static int32_t get_offset_of_U3CsubsystemImplementationTypeU3Ek__BackingField_1() { return static_cast<int32_t>(offsetof(Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704, ___U3CsubsystemImplementationTypeU3Ek__BackingField_1)); }
	inline Type_t * get_U3CsubsystemImplementationTypeU3Ek__BackingField_1() const { return ___U3CsubsystemImplementationTypeU3Ek__BackingField_1; }
	inline Type_t ** get_address_of_U3CsubsystemImplementationTypeU3Ek__BackingField_1() { return &___U3CsubsystemImplementationTypeU3Ek__BackingField_1; }
	inline void set_U3CsubsystemImplementationTypeU3Ek__BackingField_1(Type_t * value)
	{
		___U3CsubsystemImplementationTypeU3Ek__BackingField_1 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___U3CsubsystemImplementationTypeU3Ek__BackingField_1), (void*)value);
	}

	inline static int32_t get_offset_of_U3CsupportsViewportBasedRaycastU3Ek__BackingField_2() { return static_cast<int32_t>(offsetof(Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704, ___U3CsupportsViewportBasedRaycastU3Ek__BackingField_2)); }
	inline bool get_U3CsupportsViewportBasedRaycastU3Ek__BackingField_2() const { return ___U3CsupportsViewportBasedRaycastU3Ek__BackingField_2; }
	inline bool* get_address_of_U3CsupportsViewportBasedRaycastU3Ek__BackingField_2() { return &___U3CsupportsViewportBasedRaycastU3Ek__BackingField_2; }
	inline void set_U3CsupportsViewportBasedRaycastU3Ek__BackingField_2(bool value)
	{
		___U3CsupportsViewportBasedRaycastU3Ek__BackingField_2 = value;
	}

	inline static int32_t get_offset_of_U3CsupportsWorldBasedRaycastU3Ek__BackingField_3() { return static_cast<int32_t>(offsetof(Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704, ___U3CsupportsWorldBasedRaycastU3Ek__BackingField_3)); }
	inline bool get_U3CsupportsWorldBasedRaycastU3Ek__BackingField_3() const { return ___U3CsupportsWorldBasedRaycastU3Ek__BackingField_3; }
	inline bool* get_address_of_U3CsupportsWorldBasedRaycastU3Ek__BackingField_3() { return &___U3CsupportsWorldBasedRaycastU3Ek__BackingField_3; }
	inline void set_U3CsupportsWorldBasedRaycastU3Ek__BackingField_3(bool value)
	{
		___U3CsupportsWorldBasedRaycastU3Ek__BackingField_3 = value;
	}

	inline static int32_t get_offset_of_U3CsupportedTrackableTypesU3Ek__BackingField_4() { return static_cast<int32_t>(offsetof(Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704, ___U3CsupportedTrackableTypesU3Ek__BackingField_4)); }
	inline int32_t get_U3CsupportedTrackableTypesU3Ek__BackingField_4() const { return ___U3CsupportedTrackableTypesU3Ek__BackingField_4; }
	inline int32_t* get_address_of_U3CsupportedTrackableTypesU3Ek__BackingField_4() { return &___U3CsupportedTrackableTypesU3Ek__BackingField_4; }
	inline void set_U3CsupportedTrackableTypesU3Ek__BackingField_4(int32_t value)
	{
		___U3CsupportedTrackableTypesU3Ek__BackingField_4 = value;
	}
};

// Native definition for P/Invoke marshalling of UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo
struct Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704_marshaled_pinvoke
{
	char* ___U3CidU3Ek__BackingField_0;
	Type_t * ___U3CsubsystemImplementationTypeU3Ek__BackingField_1;
	int32_t ___U3CsupportsViewportBasedRaycastU3Ek__BackingField_2;
	int32_t ___U3CsupportsWorldBasedRaycastU3Ek__BackingField_3;
	int32_t ___U3CsupportedTrackableTypesU3Ek__BackingField_4;
};
// Native definition for COM marshalling of UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo
struct Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704_marshaled_com
{
	Il2CppChar* ___U3CidU3Ek__BackingField_0;
	Type_t * ___U3CsubsystemImplementationTypeU3Ek__BackingField_1;
	int32_t ___U3CsupportsViewportBasedRaycastU3Ek__BackingField_2;
	int32_t ___U3CsupportsWorldBasedRaycastU3Ek__BackingField_3;
	int32_t ___U3CsupportedTrackableTypesU3Ek__BackingField_4;
};

// UnityEngine.XR.ARSubsystems.XRReferencePoint
struct  XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 
{
public:
	// UnityEngine.XR.ARSubsystems.TrackableId UnityEngine.XR.ARSubsystems.XRReferencePoint::m_Id
	TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___m_Id_1;
	// UnityEngine.Pose UnityEngine.XR.ARSubsystems.XRReferencePoint::m_Pose
	Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___m_Pose_2;
	// UnityEngine.XR.ARSubsystems.TrackingState UnityEngine.XR.ARSubsystems.XRReferencePoint::m_TrackingState
	int32_t ___m_TrackingState_3;
	// System.IntPtr UnityEngine.XR.ARSubsystems.XRReferencePoint::m_NativePtr
	intptr_t ___m_NativePtr_4;
	// System.Guid UnityEngine.XR.ARSubsystems.XRReferencePoint::m_SessionId
	Guid_t  ___m_SessionId_5;

public:
	inline static int32_t get_offset_of_m_Id_1() { return static_cast<int32_t>(offsetof(XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9, ___m_Id_1)); }
	inline TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  get_m_Id_1() const { return ___m_Id_1; }
	inline TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 * get_address_of_m_Id_1() { return &___m_Id_1; }
	inline void set_m_Id_1(TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  value)
	{
		___m_Id_1 = value;
	}

	inline static int32_t get_offset_of_m_Pose_2() { return static_cast<int32_t>(offsetof(XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9, ___m_Pose_2)); }
	inline Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  get_m_Pose_2() const { return ___m_Pose_2; }
	inline Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 * get_address_of_m_Pose_2() { return &___m_Pose_2; }
	inline void set_m_Pose_2(Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  value)
	{
		___m_Pose_2 = value;
	}

	inline static int32_t get_offset_of_m_TrackingState_3() { return static_cast<int32_t>(offsetof(XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9, ___m_TrackingState_3)); }
	inline int32_t get_m_TrackingState_3() const { return ___m_TrackingState_3; }
	inline int32_t* get_address_of_m_TrackingState_3() { return &___m_TrackingState_3; }
	inline void set_m_TrackingState_3(int32_t value)
	{
		___m_TrackingState_3 = value;
	}

	inline static int32_t get_offset_of_m_NativePtr_4() { return static_cast<int32_t>(offsetof(XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9, ___m_NativePtr_4)); }
	inline intptr_t get_m_NativePtr_4() const { return ___m_NativePtr_4; }
	inline intptr_t* get_address_of_m_NativePtr_4() { return &___m_NativePtr_4; }
	inline void set_m_NativePtr_4(intptr_t value)
	{
		___m_NativePtr_4 = value;
	}

	inline static int32_t get_offset_of_m_SessionId_5() { return static_cast<int32_t>(offsetof(XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9, ___m_SessionId_5)); }
	inline Guid_t  get_m_SessionId_5() const { return ___m_SessionId_5; }
	inline Guid_t * get_address_of_m_SessionId_5() { return &___m_SessionId_5; }
	inline void set_m_SessionId_5(Guid_t  value)
	{
		___m_SessionId_5 = value;
	}
};

struct XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9_StaticFields
{
public:
	// UnityEngine.XR.ARSubsystems.XRReferencePoint UnityEngine.XR.ARSubsystems.XRReferencePoint::s_Default
	XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  ___s_Default_0;

public:
	inline static int32_t get_offset_of_s_Default_0() { return static_cast<int32_t>(offsetof(XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9_StaticFields, ___s_Default_0)); }
	inline XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  get_s_Default_0() const { return ___s_Default_0; }
	inline XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * get_address_of_s_Default_0() { return &___s_Default_0; }
	inline void set_s_Default_0(XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  value)
	{
		___s_Default_0 = value;
	}
};


// UnityEngine.XR.ARSubsystems.XRSessionSubsystem
struct  XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0  : public XRSubsystem_1_t4F81A9C71B4D3ABB1A7FF9080551E6E0951A5F5B
{
public:
	// UnityEngine.XR.ARSubsystems.XRSessionSubsystem_Provider UnityEngine.XR.ARSubsystems.XRSessionSubsystem::m_Provider
	Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * ___m_Provider_3;

public:
	inline static int32_t get_offset_of_m_Provider_3() { return static_cast<int32_t>(offsetof(XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0, ___m_Provider_3)); }
	inline Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * get_m_Provider_3() const { return ___m_Provider_3; }
	inline Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 ** get_address_of_m_Provider_3() { return &___m_Provider_3; }
	inline void set_m_Provider_3(Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * value)
	{
		___m_Provider_3 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_Provider_3), (void*)value);
	}
};


// UnityEngine.XR.ARSubsystems.XRSessionUpdateParams
struct  XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 
{
public:
	// UnityEngine.ScreenOrientation UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::<screenOrientation>k__BackingField
	int32_t ___U3CscreenOrientationU3Ek__BackingField_0;
	// UnityEngine.Vector2Int UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::<screenDimensions>k__BackingField
	Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  ___U3CscreenDimensionsU3Ek__BackingField_1;

public:
	inline static int32_t get_offset_of_U3CscreenOrientationU3Ek__BackingField_0() { return static_cast<int32_t>(offsetof(XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16, ___U3CscreenOrientationU3Ek__BackingField_0)); }
	inline int32_t get_U3CscreenOrientationU3Ek__BackingField_0() const { return ___U3CscreenOrientationU3Ek__BackingField_0; }
	inline int32_t* get_address_of_U3CscreenOrientationU3Ek__BackingField_0() { return &___U3CscreenOrientationU3Ek__BackingField_0; }
	inline void set_U3CscreenOrientationU3Ek__BackingField_0(int32_t value)
	{
		___U3CscreenOrientationU3Ek__BackingField_0 = value;
	}

	inline static int32_t get_offset_of_U3CscreenDimensionsU3Ek__BackingField_1() { return static_cast<int32_t>(offsetof(XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16, ___U3CscreenDimensionsU3Ek__BackingField_1)); }
	inline Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  get_U3CscreenDimensionsU3Ek__BackingField_1() const { return ___U3CscreenDimensionsU3Ek__BackingField_1; }
	inline Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905 * get_address_of_U3CscreenDimensionsU3Ek__BackingField_1() { return &___U3CscreenDimensionsU3Ek__BackingField_1; }
	inline void set_U3CscreenDimensionsU3Ek__BackingField_1(Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  value)
	{
		___U3CscreenDimensionsU3Ek__BackingField_1 = value;
	}
};


// UnityEngine.XR.ARSubsystems.XRTextureDescriptor
struct  XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD 
{
public:
	// System.IntPtr UnityEngine.XR.ARSubsystems.XRTextureDescriptor::m_NativeTexture
	intptr_t ___m_NativeTexture_0;
	// System.Int32 UnityEngine.XR.ARSubsystems.XRTextureDescriptor::m_Width
	int32_t ___m_Width_1;
	// System.Int32 UnityEngine.XR.ARSubsystems.XRTextureDescriptor::m_Height
	int32_t ___m_Height_2;
	// System.Int32 UnityEngine.XR.ARSubsystems.XRTextureDescriptor::m_MipmapCount
	int32_t ___m_MipmapCount_3;
	// UnityEngine.TextureFormat UnityEngine.XR.ARSubsystems.XRTextureDescriptor::m_Format
	int32_t ___m_Format_4;
	// System.Int32 UnityEngine.XR.ARSubsystems.XRTextureDescriptor::m_PropertyNameId
	int32_t ___m_PropertyNameId_5;

public:
	inline static int32_t get_offset_of_m_NativeTexture_0() { return static_cast<int32_t>(offsetof(XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD, ___m_NativeTexture_0)); }
	inline intptr_t get_m_NativeTexture_0() const { return ___m_NativeTexture_0; }
	inline intptr_t* get_address_of_m_NativeTexture_0() { return &___m_NativeTexture_0; }
	inline void set_m_NativeTexture_0(intptr_t value)
	{
		___m_NativeTexture_0 = value;
	}

	inline static int32_t get_offset_of_m_Width_1() { return static_cast<int32_t>(offsetof(XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD, ___m_Width_1)); }
	inline int32_t get_m_Width_1() const { return ___m_Width_1; }
	inline int32_t* get_address_of_m_Width_1() { return &___m_Width_1; }
	inline void set_m_Width_1(int32_t value)
	{
		___m_Width_1 = value;
	}

	inline static int32_t get_offset_of_m_Height_2() { return static_cast<int32_t>(offsetof(XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD, ___m_Height_2)); }
	inline int32_t get_m_Height_2() const { return ___m_Height_2; }
	inline int32_t* get_address_of_m_Height_2() { return &___m_Height_2; }
	inline void set_m_Height_2(int32_t value)
	{
		___m_Height_2 = value;
	}

	inline static int32_t get_offset_of_m_MipmapCount_3() { return static_cast<int32_t>(offsetof(XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD, ___m_MipmapCount_3)); }
	inline int32_t get_m_MipmapCount_3() const { return ___m_MipmapCount_3; }
	inline int32_t* get_address_of_m_MipmapCount_3() { return &___m_MipmapCount_3; }
	inline void set_m_MipmapCount_3(int32_t value)
	{
		___m_MipmapCount_3 = value;
	}

	inline static int32_t get_offset_of_m_Format_4() { return static_cast<int32_t>(offsetof(XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD, ___m_Format_4)); }
	inline int32_t get_m_Format_4() const { return ___m_Format_4; }
	inline int32_t* get_address_of_m_Format_4() { return &___m_Format_4; }
	inline void set_m_Format_4(int32_t value)
	{
		___m_Format_4 = value;
	}

	inline static int32_t get_offset_of_m_PropertyNameId_5() { return static_cast<int32_t>(offsetof(XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD, ___m_PropertyNameId_5)); }
	inline int32_t get_m_PropertyNameId_5() const { return ___m_PropertyNameId_5; }
	inline int32_t* get_address_of_m_PropertyNameId_5() { return &___m_PropertyNameId_5; }
	inline void set_m_PropertyNameId_5(int32_t value)
	{
		___m_PropertyNameId_5 = value;
	}
};


// UnityEngine.XR.ARSubsystems.XRTrackedImage
struct  XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 
{
public:
	// UnityEngine.XR.ARSubsystems.TrackableId UnityEngine.XR.ARSubsystems.XRTrackedImage::m_Id
	TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___m_Id_1;
	// System.Guid UnityEngine.XR.ARSubsystems.XRTrackedImage::m_SourceImageId
	Guid_t  ___m_SourceImageId_2;
	// UnityEngine.Pose UnityEngine.XR.ARSubsystems.XRTrackedImage::m_Pose
	Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___m_Pose_3;
	// UnityEngine.Vector2 UnityEngine.XR.ARSubsystems.XRTrackedImage::m_Size
	Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  ___m_Size_4;
	// UnityEngine.XR.ARSubsystems.TrackingState UnityEngine.XR.ARSubsystems.XRTrackedImage::m_TrackingState
	int32_t ___m_TrackingState_5;
	// System.IntPtr UnityEngine.XR.ARSubsystems.XRTrackedImage::m_NativePtr
	intptr_t ___m_NativePtr_6;

public:
	inline static int32_t get_offset_of_m_Id_1() { return static_cast<int32_t>(offsetof(XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8, ___m_Id_1)); }
	inline TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  get_m_Id_1() const { return ___m_Id_1; }
	inline TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 * get_address_of_m_Id_1() { return &___m_Id_1; }
	inline void set_m_Id_1(TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  value)
	{
		___m_Id_1 = value;
	}

	inline static int32_t get_offset_of_m_SourceImageId_2() { return static_cast<int32_t>(offsetof(XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8, ___m_SourceImageId_2)); }
	inline Guid_t  get_m_SourceImageId_2() const { return ___m_SourceImageId_2; }
	inline Guid_t * get_address_of_m_SourceImageId_2() { return &___m_SourceImageId_2; }
	inline void set_m_SourceImageId_2(Guid_t  value)
	{
		___m_SourceImageId_2 = value;
	}

	inline static int32_t get_offset_of_m_Pose_3() { return static_cast<int32_t>(offsetof(XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8, ___m_Pose_3)); }
	inline Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  get_m_Pose_3() const { return ___m_Pose_3; }
	inline Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 * get_address_of_m_Pose_3() { return &___m_Pose_3; }
	inline void set_m_Pose_3(Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  value)
	{
		___m_Pose_3 = value;
	}

	inline static int32_t get_offset_of_m_Size_4() { return static_cast<int32_t>(offsetof(XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8, ___m_Size_4)); }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  get_m_Size_4() const { return ___m_Size_4; }
	inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D * get_address_of_m_Size_4() { return &___m_Size_4; }
	inline void set_m_Size_4(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  value)
	{
		___m_Size_4 = value;
	}

	inline static int32_t get_offset_of_m_TrackingState_5() { return static_cast<int32_t>(offsetof(XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8, ___m_TrackingState_5)); }
	inline int32_t get_m_TrackingState_5() const { return ___m_TrackingState_5; }
	inline int32_t* get_address_of_m_TrackingState_5() { return &___m_TrackingState_5; }
	inline void set_m_TrackingState_5(int32_t value)
	{
		___m_TrackingState_5 = value;
	}

	inline static int32_t get_offset_of_m_NativePtr_6() { return static_cast<int32_t>(offsetof(XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8, ___m_NativePtr_6)); }
	inline intptr_t get_m_NativePtr_6() const { return ___m_NativePtr_6; }
	inline intptr_t* get_address_of_m_NativePtr_6() { return &___m_NativePtr_6; }
	inline void set_m_NativePtr_6(intptr_t value)
	{
		___m_NativePtr_6 = value;
	}
};

struct XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8_StaticFields
{
public:
	// UnityEngine.XR.ARSubsystems.XRTrackedImage UnityEngine.XR.ARSubsystems.XRTrackedImage::s_Default
	XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  ___s_Default_0;

public:
	inline static int32_t get_offset_of_s_Default_0() { return static_cast<int32_t>(offsetof(XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8_StaticFields, ___s_Default_0)); }
	inline XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  get_s_Default_0() const { return ___s_Default_0; }
	inline XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * get_address_of_s_Default_0() { return &___s_Default_0; }
	inline void set_s_Default_0(XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  value)
	{
		___s_Default_0 = value;
	}
};


// UnityEngine.XR.ARSubsystems.XRTrackedObject
struct  XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 
{
public:
	// UnityEngine.XR.ARSubsystems.TrackableId UnityEngine.XR.ARSubsystems.XRTrackedObject::m_TrackableId
	TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___m_TrackableId_0;
	// UnityEngine.Pose UnityEngine.XR.ARSubsystems.XRTrackedObject::m_Pose
	Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___m_Pose_1;
	// UnityEngine.XR.ARSubsystems.TrackingState UnityEngine.XR.ARSubsystems.XRTrackedObject::m_TrackingState
	int32_t ___m_TrackingState_2;
	// System.IntPtr UnityEngine.XR.ARSubsystems.XRTrackedObject::m_NativePtr
	intptr_t ___m_NativePtr_3;
	// System.Guid UnityEngine.XR.ARSubsystems.XRTrackedObject::m_ReferenceObjectGuid
	Guid_t  ___m_ReferenceObjectGuid_4;

public:
	inline static int32_t get_offset_of_m_TrackableId_0() { return static_cast<int32_t>(offsetof(XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260, ___m_TrackableId_0)); }
	inline TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  get_m_TrackableId_0() const { return ___m_TrackableId_0; }
	inline TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 * get_address_of_m_TrackableId_0() { return &___m_TrackableId_0; }
	inline void set_m_TrackableId_0(TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  value)
	{
		___m_TrackableId_0 = value;
	}

	inline static int32_t get_offset_of_m_Pose_1() { return static_cast<int32_t>(offsetof(XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260, ___m_Pose_1)); }
	inline Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  get_m_Pose_1() const { return ___m_Pose_1; }
	inline Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 * get_address_of_m_Pose_1() { return &___m_Pose_1; }
	inline void set_m_Pose_1(Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  value)
	{
		___m_Pose_1 = value;
	}

	inline static int32_t get_offset_of_m_TrackingState_2() { return static_cast<int32_t>(offsetof(XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260, ___m_TrackingState_2)); }
	inline int32_t get_m_TrackingState_2() const { return ___m_TrackingState_2; }
	inline int32_t* get_address_of_m_TrackingState_2() { return &___m_TrackingState_2; }
	inline void set_m_TrackingState_2(int32_t value)
	{
		___m_TrackingState_2 = value;
	}

	inline static int32_t get_offset_of_m_NativePtr_3() { return static_cast<int32_t>(offsetof(XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260, ___m_NativePtr_3)); }
	inline intptr_t get_m_NativePtr_3() const { return ___m_NativePtr_3; }
	inline intptr_t* get_address_of_m_NativePtr_3() { return &___m_NativePtr_3; }
	inline void set_m_NativePtr_3(intptr_t value)
	{
		___m_NativePtr_3 = value;
	}

	inline static int32_t get_offset_of_m_ReferenceObjectGuid_4() { return static_cast<int32_t>(offsetof(XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260, ___m_ReferenceObjectGuid_4)); }
	inline Guid_t  get_m_ReferenceObjectGuid_4() const { return ___m_ReferenceObjectGuid_4; }
	inline Guid_t * get_address_of_m_ReferenceObjectGuid_4() { return &___m_ReferenceObjectGuid_4; }
	inline void set_m_ReferenceObjectGuid_4(Guid_t  value)
	{
		___m_ReferenceObjectGuid_4 = value;
	}
};

struct XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260_StaticFields
{
public:
	// UnityEngine.XR.ARSubsystems.XRTrackedObject UnityEngine.XR.ARSubsystems.XRTrackedObject::s_Default
	XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260  ___s_Default_5;

public:
	inline static int32_t get_offset_of_s_Default_5() { return static_cast<int32_t>(offsetof(XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260_StaticFields, ___s_Default_5)); }
	inline XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260  get_s_Default_5() const { return ___s_Default_5; }
	inline XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * get_address_of_s_Default_5() { return &___s_Default_5; }
	inline void set_s_Default_5(XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260  value)
	{
		___s_Default_5 = value;
	}
};


// System.ArgumentException
struct  ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1  : public SystemException_t5380468142AA850BE4A341D7AF3EAB9C78746782
{
public:
	// System.String System.ArgumentException::m_paramName
	String_t* ___m_paramName_17;

public:
	inline static int32_t get_offset_of_m_paramName_17() { return static_cast<int32_t>(offsetof(ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1, ___m_paramName_17)); }
	inline String_t* get_m_paramName_17() const { return ___m_paramName_17; }
	inline String_t** get_address_of_m_paramName_17() { return &___m_paramName_17; }
	inline void set_m_paramName_17(String_t* value)
	{
		___m_paramName_17 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_paramName_17), (void*)value);
	}
};


// System.IndexOutOfRangeException
struct  IndexOutOfRangeException_tEC7665FC66525AB6A6916A7EB505E5591683F0CF  : public SystemException_t5380468142AA850BE4A341D7AF3EAB9C78746782
{
public:

public:
};


// System.InvalidOperationException
struct  InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1  : public SystemException_t5380468142AA850BE4A341D7AF3EAB9C78746782
{
public:

public:
};


// System.NotSupportedException
struct  NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010  : public SystemException_t5380468142AA850BE4A341D7AF3EAB9C78746782
{
public:

public:
};


// UnityEngine.Texture2D
struct  Texture2D_tBBF96AC337723E2EF156DF17E09D4379FD05DE1C  : public Texture_t387FE83BB848001FD06B14707AEA6D5A0F6A95F4
{
public:

public:
};


// UnityEngine.XR.ARSubsystems.TrackableChanges`1<UnityEngine.XR.ARSubsystems.XRReferencePoint>
struct  TrackableChanges_1_t5C8C3FBA23E5BBC147A6991B68520A756EF54FDF 
{
public:
	// System.Boolean UnityEngine.XR.ARSubsystems.TrackableChanges`1::<isCreated>k__BackingField
	bool ___U3CisCreatedU3Ek__BackingField_0;
	// Unity.Collections.NativeArray`1<T> UnityEngine.XR.ARSubsystems.TrackableChanges`1::m_Added
	NativeArray_1_t5AADFB4C72573FE3017795F15B8CBC88625A8876  ___m_Added_1;
	// Unity.Collections.NativeArray`1<T> UnityEngine.XR.ARSubsystems.TrackableChanges`1::m_Updated
	NativeArray_1_t5AADFB4C72573FE3017795F15B8CBC88625A8876  ___m_Updated_2;
	// Unity.Collections.NativeArray`1<UnityEngine.XR.ARSubsystems.TrackableId> UnityEngine.XR.ARSubsystems.TrackableChanges`1::m_Removed
	NativeArray_1_tD7797CC5848BA9ECBD1172056474C8DD8022B1D6  ___m_Removed_3;

public:
	inline static int32_t get_offset_of_U3CisCreatedU3Ek__BackingField_0() { return static_cast<int32_t>(offsetof(TrackableChanges_1_t5C8C3FBA23E5BBC147A6991B68520A756EF54FDF, ___U3CisCreatedU3Ek__BackingField_0)); }
	inline bool get_U3CisCreatedU3Ek__BackingField_0() const { return ___U3CisCreatedU3Ek__BackingField_0; }
	inline bool* get_address_of_U3CisCreatedU3Ek__BackingField_0() { return &___U3CisCreatedU3Ek__BackingField_0; }
	inline void set_U3CisCreatedU3Ek__BackingField_0(bool value)
	{
		___U3CisCreatedU3Ek__BackingField_0 = value;
	}

	inline static int32_t get_offset_of_m_Added_1() { return static_cast<int32_t>(offsetof(TrackableChanges_1_t5C8C3FBA23E5BBC147A6991B68520A756EF54FDF, ___m_Added_1)); }
	inline NativeArray_1_t5AADFB4C72573FE3017795F15B8CBC88625A8876  get_m_Added_1() const { return ___m_Added_1; }
	inline NativeArray_1_t5AADFB4C72573FE3017795F15B8CBC88625A8876 * get_address_of_m_Added_1() { return &___m_Added_1; }
	inline void set_m_Added_1(NativeArray_1_t5AADFB4C72573FE3017795F15B8CBC88625A8876  value)
	{
		___m_Added_1 = value;
	}

	inline static int32_t get_offset_of_m_Updated_2() { return static_cast<int32_t>(offsetof(TrackableChanges_1_t5C8C3FBA23E5BBC147A6991B68520A756EF54FDF, ___m_Updated_2)); }
	inline NativeArray_1_t5AADFB4C72573FE3017795F15B8CBC88625A8876  get_m_Updated_2() const { return ___m_Updated_2; }
	inline NativeArray_1_t5AADFB4C72573FE3017795F15B8CBC88625A8876 * get_address_of_m_Updated_2() { return &___m_Updated_2; }
	inline void set_m_Updated_2(NativeArray_1_t5AADFB4C72573FE3017795F15B8CBC88625A8876  value)
	{
		___m_Updated_2 = value;
	}

	inline static int32_t get_offset_of_m_Removed_3() { return static_cast<int32_t>(offsetof(TrackableChanges_1_t5C8C3FBA23E5BBC147A6991B68520A756EF54FDF, ___m_Removed_3)); }
	inline NativeArray_1_tD7797CC5848BA9ECBD1172056474C8DD8022B1D6  get_m_Removed_3() const { return ___m_Removed_3; }
	inline NativeArray_1_tD7797CC5848BA9ECBD1172056474C8DD8022B1D6 * get_address_of_m_Removed_3() { return &___m_Removed_3; }
	inline void set_m_Removed_3(NativeArray_1_tD7797CC5848BA9ECBD1172056474C8DD8022B1D6  value)
	{
		___m_Removed_3 = value;
	}
};


// UnityEngine.XR.ARSubsystems.XRPointCloudData
struct  XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA 
{
public:
	// Unity.Collections.NativeArray`1<UnityEngine.Vector3> UnityEngine.XR.ARSubsystems.XRPointCloudData::m_Positions
	NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  ___m_Positions_0;
	// Unity.Collections.NativeArray`1<System.Single> UnityEngine.XR.ARSubsystems.XRPointCloudData::m_ConfidenceValues
	NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  ___m_ConfidenceValues_1;
	// Unity.Collections.NativeArray`1<System.UInt64> UnityEngine.XR.ARSubsystems.XRPointCloudData::m_Identifiers
	NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  ___m_Identifiers_2;

public:
	inline static int32_t get_offset_of_m_Positions_0() { return static_cast<int32_t>(offsetof(XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA, ___m_Positions_0)); }
	inline NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  get_m_Positions_0() const { return ___m_Positions_0; }
	inline NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 * get_address_of_m_Positions_0() { return &___m_Positions_0; }
	inline void set_m_Positions_0(NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  value)
	{
		___m_Positions_0 = value;
	}

	inline static int32_t get_offset_of_m_ConfidenceValues_1() { return static_cast<int32_t>(offsetof(XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA, ___m_ConfidenceValues_1)); }
	inline NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  get_m_ConfidenceValues_1() const { return ___m_ConfidenceValues_1; }
	inline NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 * get_address_of_m_ConfidenceValues_1() { return &___m_ConfidenceValues_1; }
	inline void set_m_ConfidenceValues_1(NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  value)
	{
		___m_ConfidenceValues_1 = value;
	}

	inline static int32_t get_offset_of_m_Identifiers_2() { return static_cast<int32_t>(offsetof(XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA, ___m_Identifiers_2)); }
	inline NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  get_m_Identifiers_2() const { return ___m_Identifiers_2; }
	inline NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C * get_address_of_m_Identifiers_2() { return &___m_Identifiers_2; }
	inline void set_m_Identifiers_2(NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  value)
	{
		___m_Identifiers_2 = value;
	}
};

// Native definition for P/Invoke marshalling of UnityEngine.XR.ARSubsystems.XRPointCloudData
struct XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA_marshaled_pinvoke
{
	NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  ___m_Positions_0;
	NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  ___m_ConfidenceValues_1;
	NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  ___m_Identifiers_2;
};
// Native definition for COM marshalling of UnityEngine.XR.ARSubsystems.XRPointCloudData
struct XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA_marshaled_com
{
	NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  ___m_Positions_0;
	NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  ___m_ConfidenceValues_1;
	NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  ___m_Identifiers_2;
};

// UnityEngine.XR.ARSubsystems.XRReferenceImageLibrary
struct  XRReferenceImageLibrary_t38B21DC6650EADA892125F045DCBF71EBC3F6A8F  : public ScriptableObject_tAB015486CEAB714DA0D5C1BA389B84FB90427734
{
public:
	// System.UInt64 UnityEngine.XR.ARSubsystems.XRReferenceImageLibrary::m_GuidLow
	uint64_t ___m_GuidLow_4;
	// System.UInt64 UnityEngine.XR.ARSubsystems.XRReferenceImageLibrary::m_GuidHigh
	uint64_t ___m_GuidHigh_5;
	// System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceImage> UnityEngine.XR.ARSubsystems.XRReferenceImageLibrary::m_Images
	List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * ___m_Images_6;

public:
	inline static int32_t get_offset_of_m_GuidLow_4() { return static_cast<int32_t>(offsetof(XRReferenceImageLibrary_t38B21DC6650EADA892125F045DCBF71EBC3F6A8F, ___m_GuidLow_4)); }
	inline uint64_t get_m_GuidLow_4() const { return ___m_GuidLow_4; }
	inline uint64_t* get_address_of_m_GuidLow_4() { return &___m_GuidLow_4; }
	inline void set_m_GuidLow_4(uint64_t value)
	{
		___m_GuidLow_4 = value;
	}

	inline static int32_t get_offset_of_m_GuidHigh_5() { return static_cast<int32_t>(offsetof(XRReferenceImageLibrary_t38B21DC6650EADA892125F045DCBF71EBC3F6A8F, ___m_GuidHigh_5)); }
	inline uint64_t get_m_GuidHigh_5() const { return ___m_GuidHigh_5; }
	inline uint64_t* get_address_of_m_GuidHigh_5() { return &___m_GuidHigh_5; }
	inline void set_m_GuidHigh_5(uint64_t value)
	{
		___m_GuidHigh_5 = value;
	}

	inline static int32_t get_offset_of_m_Images_6() { return static_cast<int32_t>(offsetof(XRReferenceImageLibrary_t38B21DC6650EADA892125F045DCBF71EBC3F6A8F, ___m_Images_6)); }
	inline List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * get_m_Images_6() const { return ___m_Images_6; }
	inline List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F ** get_address_of_m_Images_6() { return &___m_Images_6; }
	inline void set_m_Images_6(List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * value)
	{
		___m_Images_6 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_Images_6), (void*)value);
	}
};


// UnityEngine.XR.ARSubsystems.XRReferenceObjectEntry
struct  XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8  : public ScriptableObject_tAB015486CEAB714DA0D5C1BA389B84FB90427734
{
public:

public:
};


// UnityEngine.XR.ARSubsystems.XRReferenceObjectLibrary
struct  XRReferenceObjectLibrary_tC9950535C1214232A691C32EDA0E95C703056260  : public ScriptableObject_tAB015486CEAB714DA0D5C1BA389B84FB90427734
{
public:
	// System.UInt64 UnityEngine.XR.ARSubsystems.XRReferenceObjectLibrary::m_GuidLow
	uint64_t ___m_GuidLow_4;
	// System.UInt64 UnityEngine.XR.ARSubsystems.XRReferenceObjectLibrary::m_GuidHigh
	uint64_t ___m_GuidHigh_5;
	// System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceObject> UnityEngine.XR.ARSubsystems.XRReferenceObjectLibrary::m_ReferenceObjects
	List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * ___m_ReferenceObjects_6;

public:
	inline static int32_t get_offset_of_m_GuidLow_4() { return static_cast<int32_t>(offsetof(XRReferenceObjectLibrary_tC9950535C1214232A691C32EDA0E95C703056260, ___m_GuidLow_4)); }
	inline uint64_t get_m_GuidLow_4() const { return ___m_GuidLow_4; }
	inline uint64_t* get_address_of_m_GuidLow_4() { return &___m_GuidLow_4; }
	inline void set_m_GuidLow_4(uint64_t value)
	{
		___m_GuidLow_4 = value;
	}

	inline static int32_t get_offset_of_m_GuidHigh_5() { return static_cast<int32_t>(offsetof(XRReferenceObjectLibrary_tC9950535C1214232A691C32EDA0E95C703056260, ___m_GuidHigh_5)); }
	inline uint64_t get_m_GuidHigh_5() const { return ___m_GuidHigh_5; }
	inline uint64_t* get_address_of_m_GuidHigh_5() { return &___m_GuidHigh_5; }
	inline void set_m_GuidHigh_5(uint64_t value)
	{
		___m_GuidHigh_5 = value;
	}

	inline static int32_t get_offset_of_m_ReferenceObjects_6() { return static_cast<int32_t>(offsetof(XRReferenceObjectLibrary_tC9950535C1214232A691C32EDA0E95C703056260, ___m_ReferenceObjects_6)); }
	inline List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * get_m_ReferenceObjects_6() const { return ___m_ReferenceObjects_6; }
	inline List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 ** get_address_of_m_ReferenceObjects_6() { return &___m_ReferenceObjects_6; }
	inline void set_m_ReferenceObjects_6(List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * value)
	{
		___m_ReferenceObjects_6 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_ReferenceObjects_6), (void*)value);
	}
};


// UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem
struct  XRReferencePointSubsystem_tF175EC0188CC6EFB0A0633BA11FCA4D81A6A88E3  : public TrackingSubsystem_2_t62AFA2295FCEFC2C3818E3B9EDB3C1AF80509899
{
public:
	// UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem_Provider UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem::m_Provider
	Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 * ___m_Provider_3;

public:
	inline static int32_t get_offset_of_m_Provider_3() { return static_cast<int32_t>(offsetof(XRReferencePointSubsystem_tF175EC0188CC6EFB0A0633BA11FCA4D81A6A88E3, ___m_Provider_3)); }
	inline Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 * get_m_Provider_3() const { return ___m_Provider_3; }
	inline Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 ** get_address_of_m_Provider_3() { return &___m_Provider_3; }
	inline void set_m_Provider_3(Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 * value)
	{
		___m_Provider_3 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_Provider_3), (void*)value);
	}
};


// System.ArgumentNullException
struct  ArgumentNullException_t581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD  : public ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// System.Object[]
struct ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A  : public RuntimeArray
{
public:
	ALIGN_FIELD (8) RuntimeObject * m_Items[1];

public:
	inline RuntimeObject * GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline RuntimeObject ** GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, RuntimeObject * value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)m_Items + index, (void*)value);
	}
	inline RuntimeObject * GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline RuntimeObject ** GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, RuntimeObject * value)
	{
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)m_Items + index, (void*)value);
	}
};
// UnityEngine.XR.ARSubsystems.XRReferenceImage[]
struct XRReferenceImageU5BU5D_t1EEAB0EDA5828C38140B4D8D48E11247C4789BCE  : public RuntimeArray
{
public:
	ALIGN_FIELD (8) XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  m_Items[1];

public:
	inline XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)&((m_Items + index)->___m_Name_4), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((m_Items + index)->___m_Texture_5), (void*)NULL);
		#endif
	}
	inline XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  value)
	{
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)&((m_Items + index)->___m_Name_4), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((m_Items + index)->___m_Texture_5), (void*)NULL);
		#endif
	}
};
// UnityEngine.XR.ARSubsystems.XRReferenceObject[]
struct XRReferenceObjectU5BU5D_t0AF9158E0468305ABEAA331C8925977310FBD498  : public RuntimeArray
{
public:
	ALIGN_FIELD (8) XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  m_Items[1];

public:
	inline XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)&((m_Items + index)->___m_Name_2), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((m_Items + index)->___m_Entries_3), (void*)NULL);
		#endif
	}
	inline XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  value)
	{
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)&((m_Items + index)->___m_Name_2), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((m_Items + index)->___m_Entries_3), (void*)NULL);
		#endif
	}
};


// System.Boolean Unity.Collections.NativeArray`1<UnityEngine.Vector3>::get_IsCreated()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool NativeArray_1_get_IsCreated_mA83E57BF410BDF5144D71C3C740F1BB04D255073_gshared (NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 * __this, const RuntimeMethod* method);
// System.Void Unity.Collections.NativeArray`1<UnityEngine.Vector3>::Dispose()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void NativeArray_1_Dispose_m87C3176D32CFB61D46241114A0DAA158BA654620_gshared (NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 * __this, const RuntimeMethod* method);
// System.Boolean Unity.Collections.NativeArray`1<System.Single>::get_IsCreated()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool NativeArray_1_get_IsCreated_m871B990AAFB1ACE6BDFE2A895BA589B9E8C3769E_gshared (NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 * __this, const RuntimeMethod* method);
// System.Void Unity.Collections.NativeArray`1<System.Single>::Dispose()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void NativeArray_1_Dispose_m0A2104A4244A4841BC56C79F09040160C5209941_gshared (NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 * __this, const RuntimeMethod* method);
// System.Boolean Unity.Collections.NativeArray`1<System.UInt64>::get_IsCreated()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool NativeArray_1_get_IsCreated_mC80A890C3A78C986492F0259406F63788CED59A1_gshared (NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C * __this, const RuntimeMethod* method);
// System.Void Unity.Collections.NativeArray`1<System.UInt64>::Dispose()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void NativeArray_1_Dispose_m922795C73F226D113E9B538E2D95D7423522B868_gshared (NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C * __this, const RuntimeMethod* method);
// System.Int32 Unity.Collections.NativeArray`1<UnityEngine.Vector3>::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t NativeArray_1_GetHashCode_m8DD866CBD1A50C075DA392F6DFDE1B9B7767DAD3_gshared (NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 * __this, const RuntimeMethod* method);
// System.Int32 Unity.Collections.NativeArray`1<System.Single>::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t NativeArray_1_GetHashCode_m4A3A547F590E7BCA81A2099A6ECA91A4C9D595F1_gshared (NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 * __this, const RuntimeMethod* method);
// System.Int32 Unity.Collections.NativeArray`1<System.UInt64>::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t NativeArray_1_GetHashCode_m3C1068B41EC1F4520ED44D2FD0E3E44F304099E1_gshared (NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C * __this, const RuntimeMethod* method);
// System.Boolean Unity.Collections.NativeArray`1<UnityEngine.Vector3>::Equals(Unity.Collections.NativeArray`1<!0>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool NativeArray_1_Equals_m2151C41783B1650AC28A718289E8D4FC5DAA2D42_gshared (NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 * __this, NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  ___other0, const RuntimeMethod* method);
// System.Boolean Unity.Collections.NativeArray`1<System.Single>::Equals(Unity.Collections.NativeArray`1<!0>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool NativeArray_1_Equals_mEC619E11BC3B323365C4E22C4493BE4F3FF49F27_gshared (NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 * __this, NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  ___other0, const RuntimeMethod* method);
// System.Boolean Unity.Collections.NativeArray`1<System.UInt64>::Equals(Unity.Collections.NativeArray`1<!0>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool NativeArray_1_Equals_m60C6EC22F3E5BF3FA454EE59E114A0E3610DCC41_gshared (NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C * __this, NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  ___other0, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRSubsystem`1<System.Object>::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRSubsystem_1__ctor_mE4899E6701EB0E563C03F950450FDDAC4FF21BAF_gshared (XRSubsystem_1_tF1AF6BF44AE813FB0C425AC60D3C9E9DF5904C3A * __this, const RuntimeMethod* method);
// System.Void UnityEngine.SubsystemDescriptor`1<System.Object>::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void SubsystemDescriptor_1__ctor_m524E72319B102242E9AE86C5C5A529D6291D8602_gshared (SubsystemDescriptor_1_t951C98846FCA6E211E4065CE53CF8AFB40DB5E24 * __this, const RuntimeMethod* method);
// System.Boolean System.Nullable`1<UnityEngine.Vector2>::get_HasValue()
IL2CPP_EXTERN_C inline IL2CPP_METHOD_ATTR bool Nullable_1_get_HasValue_m86766498BBF2DC3AB0128F33AC3F200BF93BB1B6_gshared_inline (Nullable_1_t2D31B61CB1F8452E868737CCA40082AA2D1F58DC * __this, const RuntimeMethod* method);
// !0 System.Nullable`1<UnityEngine.Vector2>::get_Value()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  Nullable_1_get_Value_m763182B16D2A96848EAFACFA31AC9985EFCC0F30_gshared (Nullable_1_t2D31B61CB1F8452E868737CCA40082AA2D1F58DC * __this, const RuntimeMethod* method);
// System.Int32 System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceImage>::get_Count()
IL2CPP_EXTERN_C inline IL2CPP_METHOD_ATTR int32_t List_1_get_Count_mC10370473AE5833D24A5F9361290A6B38CC73C32_gshared_inline (List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * __this, const RuntimeMethod* method);
// System.Collections.Generic.List`1/Enumerator<!0> System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceImage>::GetEnumerator()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Enumerator_t90B18C0FA1516886E459FA26C65233B656F793EB  List_1_GetEnumerator_mD7B1224990EE97B1239AF20ACDA9FC21BCAEEBC5_gshared (List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * __this, const RuntimeMethod* method);
// !0 System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceImage>::get_Item(System.Int32)
IL2CPP_EXTERN_C inline IL2CPP_METHOD_ATTR XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  List_1_get_Item_m0935B6D4B21A74A42474E6C822B5DA4F8A32FE00_gshared_inline (List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * __this, int32_t ___index0, const RuntimeMethod* method);
// System.Int32 System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceImage>::IndexOf(!0)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t List_1_IndexOf_m60FE5836AABB98EF6AD4A55ECB7085D854CA5E87_gshared (List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * __this, XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  ___item0, const RuntimeMethod* method);
// System.Void System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceImage>::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void List_1__ctor_m95E964C07FFF3EC898BA56E8B9ADE6E55ECEA0B9_gshared (List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * __this, const RuntimeMethod* method);
// System.Collections.Generic.List`1/Enumerator<!0> System.Collections.Generic.List`1<System.Object>::GetEnumerator()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Enumerator_tE0C99528D3DCE5863566CE37BD94162A4C0431CD  List_1_GetEnumerator_m52CC760E475D226A2B75048D70C4E22692F9F68D_gshared (List_1_t05CC3C859AB5E6024394EF9A42E3E696628CA02D * __this, const RuntimeMethod* method);
// !0 System.Collections.Generic.List`1/Enumerator<System.Object>::get_Current()
IL2CPP_EXTERN_C inline IL2CPP_METHOD_ATTR RuntimeObject * Enumerator_get_Current_mD7829C7E8CFBEDD463B15A951CDE9B90A12CC55C_gshared_inline (Enumerator_tE0C99528D3DCE5863566CE37BD94162A4C0431CD * __this, const RuntimeMethod* method);
// System.Boolean System.Collections.Generic.List`1/Enumerator<System.Object>::MoveNext()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Enumerator_MoveNext_m38B1099DDAD7EEDE2F4CDAB11C095AC784AC2E34_gshared (Enumerator_tE0C99528D3DCE5863566CE37BD94162A4C0431CD * __this, const RuntimeMethod* method);
// System.Void System.Collections.Generic.List`1/Enumerator<System.Object>::Dispose()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Enumerator_Dispose_m94D0DAE031619503CDA6E53C5C3CC78AF3139472_gshared (Enumerator_tE0C99528D3DCE5863566CE37BD94162A4C0431CD * __this, const RuntimeMethod* method);
// System.Int32 System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceObject>::get_Count()
IL2CPP_EXTERN_C inline IL2CPP_METHOD_ATTR int32_t List_1_get_Count_mA303D95DAA9717D8D3148C3F20FA1B2750CE77B4_gshared_inline (List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * __this, const RuntimeMethod* method);
// System.Collections.Generic.List`1/Enumerator<!0> System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceObject>::GetEnumerator()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Enumerator_t3D111B64EA07FCA753146B875D55422A5191B57F  List_1_GetEnumerator_m7ABEC2AD664AC6AC1073E546BAAFC0350626A321_gshared (List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * __this, const RuntimeMethod* method);
// !0 System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceObject>::get_Item(System.Int32)
IL2CPP_EXTERN_C inline IL2CPP_METHOD_ATTR XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  List_1_get_Item_mE2BB4CDB177C0DA1EF1FB0A174459691F207C7DF_gshared_inline (List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * __this, int32_t ___index0, const RuntimeMethod* method);
// System.Int32 System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceObject>::IndexOf(!0)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t List_1_IndexOf_m80147D21315270B53CA3E7AE6FA3B97264231AB2_gshared (List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * __this, XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  ___item0, const RuntimeMethod* method);
// System.Void System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceObject>::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void List_1__ctor_m370BA08A79B7950CA8E75862CD6F4921BAF6D7AB_gshared (List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.TrackingSubsystem`2<UnityEngine.XR.ARSubsystems.XRReferencePoint,System.Object>::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void TrackingSubsystem_2__ctor_m7BED931B501BCE6534C3BC2DC0CA30C73E917F90_gshared (TrackingSubsystem_2_tA3D4B822865BAE0754B253CF8551A3EBB7073851 * __this, const RuntimeMethod* method);
// !0 UnityEngine.Subsystem`1<System.Object>::get_SubsystemDescriptor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject * Subsystem_1_get_SubsystemDescriptor_mA35B9EF0D8FB264CBDA2C501B3C251816226A412_gshared (Subsystem_1_t730D4B66354D4BECD6DEDE1050EA133DF9B7A61F * __this, const RuntimeMethod* method);
// UnityEngine.XR.ARSubsystems.Promise`1<T> UnityEngine.XR.ARSubsystems.Promise`1<System.Int32Enum>::CreateResolvedPromise(T)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Promise_1_tF9FBB5000BE390F6ECFC210DC39F175828FBA068 * Promise_1_CreateResolvedPromise_mB605C1D88AB74006BD3AA2C791854687156606DA_gshared (int32_t ___result0, const RuntimeMethod* method);

// Unity.Collections.NativeArray`1<UnityEngine.Vector3> UnityEngine.XR.ARSubsystems.XRPointCloudData::get_positions()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  XRPointCloudData_get_positions_m2BDA572054D639DB35E9FDA3D15AEF3B7B39D40C_inline (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRPointCloudData::set_positions(Unity.Collections.NativeArray`1<UnityEngine.Vector3>)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRPointCloudData_set_positions_m78BB0E1E2A5860DAC6F60D6C9A6A37544FF9880E_inline (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  ___value0, const RuntimeMethod* method);
// Unity.Collections.NativeArray`1<System.Single> UnityEngine.XR.ARSubsystems.XRPointCloudData::get_confidenceValues()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  XRPointCloudData_get_confidenceValues_m156073A1640F58477DBCBAC6BDA05C3BF866ACE6_inline (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRPointCloudData::set_confidenceValues(Unity.Collections.NativeArray`1<System.Single>)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRPointCloudData_set_confidenceValues_mC837DE6B63BF8CBD8F2480C1A4ED3247AABCB861_inline (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  ___value0, const RuntimeMethod* method);
// Unity.Collections.NativeArray`1<System.UInt64> UnityEngine.XR.ARSubsystems.XRPointCloudData::get_identifiers()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  XRPointCloudData_get_identifiers_m6AA5FE2F151300371A1F5A8310A49A0D4A35BD23_inline (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRPointCloudData::set_identifiers(Unity.Collections.NativeArray`1<System.UInt64>)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRPointCloudData_set_identifiers_m550B2B8C6EF821D5BBD47C066FF6C961EF0CA562_inline (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  ___value0, const RuntimeMethod* method);
// System.Boolean Unity.Collections.NativeArray`1<UnityEngine.Vector3>::get_IsCreated()
inline bool NativeArray_1_get_IsCreated_mA83E57BF410BDF5144D71C3C740F1BB04D255073 (NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 * __this, const RuntimeMethod* method)
{
	return ((  bool (*) (NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 *, const RuntimeMethod*))NativeArray_1_get_IsCreated_mA83E57BF410BDF5144D71C3C740F1BB04D255073_gshared)(__this, method);
}
// System.Void Unity.Collections.NativeArray`1<UnityEngine.Vector3>::Dispose()
inline void NativeArray_1_Dispose_m87C3176D32CFB61D46241114A0DAA158BA654620 (NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 * __this, const RuntimeMethod* method)
{
	((  void (*) (NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 *, const RuntimeMethod*))NativeArray_1_Dispose_m87C3176D32CFB61D46241114A0DAA158BA654620_gshared)(__this, method);
}
// System.Boolean Unity.Collections.NativeArray`1<System.Single>::get_IsCreated()
inline bool NativeArray_1_get_IsCreated_m871B990AAFB1ACE6BDFE2A895BA589B9E8C3769E (NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 * __this, const RuntimeMethod* method)
{
	return ((  bool (*) (NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 *, const RuntimeMethod*))NativeArray_1_get_IsCreated_m871B990AAFB1ACE6BDFE2A895BA589B9E8C3769E_gshared)(__this, method);
}
// System.Void Unity.Collections.NativeArray`1<System.Single>::Dispose()
inline void NativeArray_1_Dispose_m0A2104A4244A4841BC56C79F09040160C5209941 (NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 * __this, const RuntimeMethod* method)
{
	((  void (*) (NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 *, const RuntimeMethod*))NativeArray_1_Dispose_m0A2104A4244A4841BC56C79F09040160C5209941_gshared)(__this, method);
}
// System.Boolean Unity.Collections.NativeArray`1<System.UInt64>::get_IsCreated()
inline bool NativeArray_1_get_IsCreated_mC80A890C3A78C986492F0259406F63788CED59A1 (NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C * __this, const RuntimeMethod* method)
{
	return ((  bool (*) (NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C *, const RuntimeMethod*))NativeArray_1_get_IsCreated_mC80A890C3A78C986492F0259406F63788CED59A1_gshared)(__this, method);
}
// System.Void Unity.Collections.NativeArray`1<System.UInt64>::Dispose()
inline void NativeArray_1_Dispose_m922795C73F226D113E9B538E2D95D7423522B868 (NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C * __this, const RuntimeMethod* method)
{
	((  void (*) (NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C *, const RuntimeMethod*))NativeArray_1_Dispose_m922795C73F226D113E9B538E2D95D7423522B868_gshared)(__this, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRPointCloudData::Dispose()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRPointCloudData_Dispose_mDF78595F088472E60327A1D366AA787C68A3EDE3 (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, const RuntimeMethod* method);
// System.Int32 Unity.Collections.NativeArray`1<UnityEngine.Vector3>::GetHashCode()
inline int32_t NativeArray_1_GetHashCode_m8DD866CBD1A50C075DA392F6DFDE1B9B7767DAD3 (NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 * __this, const RuntimeMethod* method)
{
	return ((  int32_t (*) (NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 *, const RuntimeMethod*))NativeArray_1_GetHashCode_m8DD866CBD1A50C075DA392F6DFDE1B9B7767DAD3_gshared)(__this, method);
}
// System.Int32 Unity.Collections.NativeArray`1<System.Single>::GetHashCode()
inline int32_t NativeArray_1_GetHashCode_m4A3A547F590E7BCA81A2099A6ECA91A4C9D595F1 (NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 * __this, const RuntimeMethod* method)
{
	return ((  int32_t (*) (NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 *, const RuntimeMethod*))NativeArray_1_GetHashCode_m4A3A547F590E7BCA81A2099A6ECA91A4C9D595F1_gshared)(__this, method);
}
// System.Int32 Unity.Collections.NativeArray`1<System.UInt64>::GetHashCode()
inline int32_t NativeArray_1_GetHashCode_m3C1068B41EC1F4520ED44D2FD0E3E44F304099E1 (NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C * __this, const RuntimeMethod* method)
{
	return ((  int32_t (*) (NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C *, const RuntimeMethod*))NativeArray_1_GetHashCode_m3C1068B41EC1F4520ED44D2FD0E3E44F304099E1_gshared)(__this, method);
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRPointCloudData::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRPointCloudData_GetHashCode_mA67A28CD8661AAE597D4466135AF1750D2569409 (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRPointCloudData::Equals(UnityEngine.XR.ARSubsystems.XRPointCloudData)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRPointCloudData_Equals_mDE5097D689526E5461CBBC48C36E6221F71F7598 (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA  ___other0, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRPointCloudData::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRPointCloudData_Equals_mC870B135A9697D1A2AFB40892E70D7D403590E2A (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, RuntimeObject * ___obj0, const RuntimeMethod* method);
// System.String System.String::Format(System.String,System.Object,System.Object,System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* String_Format_m26BBF75F9609FAD0B39C2242FEBAAD7D68F14D99 (String_t* ___format0, RuntimeObject * ___arg01, RuntimeObject * ___arg12, RuntimeObject * ___arg23, const RuntimeMethod* method);
// System.String UnityEngine.XR.ARSubsystems.XRPointCloudData::ToString()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* XRPointCloudData_ToString_m3790E45AE87D2C6F63D664EF736F530B7A4FCB4D (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, const RuntimeMethod* method);
// System.Boolean Unity.Collections.NativeArray`1<UnityEngine.Vector3>::Equals(Unity.Collections.NativeArray`1<!0>)
inline bool NativeArray_1_Equals_m2151C41783B1650AC28A718289E8D4FC5DAA2D42 (NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 * __this, NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  ___other0, const RuntimeMethod* method)
{
	return ((  bool (*) (NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 *, NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 , const RuntimeMethod*))NativeArray_1_Equals_m2151C41783B1650AC28A718289E8D4FC5DAA2D42_gshared)(__this, ___other0, method);
}
// System.Boolean Unity.Collections.NativeArray`1<System.Single>::Equals(Unity.Collections.NativeArray`1<!0>)
inline bool NativeArray_1_Equals_mEC619E11BC3B323365C4E22C4493BE4F3FF49F27 (NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 * __this, NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  ___other0, const RuntimeMethod* method)
{
	return ((  bool (*) (NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 *, NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 , const RuntimeMethod*))NativeArray_1_Equals_mEC619E11BC3B323365C4E22C4493BE4F3FF49F27_gshared)(__this, ___other0, method);
}
// System.Boolean Unity.Collections.NativeArray`1<System.UInt64>::Equals(Unity.Collections.NativeArray`1<!0>)
inline bool NativeArray_1_Equals_m60C6EC22F3E5BF3FA454EE59E114A0E3610DCC41 (NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C * __this, NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  ___other0, const RuntimeMethod* method)
{
	return ((  bool (*) (NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C *, NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C , const RuntimeMethod*))NativeArray_1_Equals_m60C6EC22F3E5BF3FA454EE59E114A0E3610DCC41_gshared)(__this, ___other0, method);
}
// UnityEngine.XR.ARSubsystems.TrackableId UnityEngine.XR.ARSubsystems.XRRaycastHit::get_trackableId()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  XRRaycastHit_get_trackableId_mAECCB1BE08FB0B5A48CB27D955250FE2068492CF_inline (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastHit::set_trackableId(UnityEngine.XR.ARSubsystems.TrackableId)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRRaycastHit_set_trackableId_mD5381CB555237421AA3A1A4F42BDBA66C2CEE77F_inline (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___value0, const RuntimeMethod* method);
// UnityEngine.Pose UnityEngine.XR.ARSubsystems.XRRaycastHit::get_pose()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  XRRaycastHit_get_pose_mE0B0A754E818C6FF3675A41CA95185A3E608C8C3_inline (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastHit::set_pose(UnityEngine.Pose)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRRaycastHit_set_pose_m3E6F13DE1371303DD66CD9D9E8B86500C24C5516_inline (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___value0, const RuntimeMethod* method);
// System.Single UnityEngine.XR.ARSubsystems.XRRaycastHit::get_distance()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR float XRRaycastHit_get_distance_mC748DE6ED96B0C735DCA4AD320FA0BF522246D19_inline (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastHit::set_distance(System.Single)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRRaycastHit_set_distance_m53218D1A8CBD8F632F988C439D5F98633A050815_inline (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, float ___value0, const RuntimeMethod* method);
// UnityEngine.XR.ARSubsystems.TrackableType UnityEngine.XR.ARSubsystems.XRRaycastHit::get_hitType()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRRaycastHit_get_hitType_m52BBF5DBDE1B3E7E01571EE029F68EB29E240DA6_inline (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastHit::set_hitType(UnityEngine.XR.ARSubsystems.TrackableType)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRRaycastHit_set_hitType_m776B39B9226EB310D47FB6A10BA78844AEC4EE58_inline (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, int32_t ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastHit::.ctor(UnityEngine.XR.ARSubsystems.TrackableId,UnityEngine.Pose,System.Single,UnityEngine.XR.ARSubsystems.TrackableType)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRRaycastHit__ctor_m522E98C4B6FD85F8386911C5BC497DE4968E3961 (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___trackableId0, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___pose1, float ___distance2, int32_t ___hitType3, const RuntimeMethod* method);
// System.Int32 UnityEngine.XR.ARSubsystems.TrackableId::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t TrackableId_GetHashCode_m89E7236D11700A1FAF335918CA65CDEB1BF4D973 (TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 * __this, const RuntimeMethod* method);
// System.Int32 UnityEngine.Pose::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Pose_GetHashCode_m17AC0D28F5BD43DE0CCFA4CC1A870C525E0D6066 (Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 * __this, const RuntimeMethod* method);
// System.Int32 System.Single::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Single_GetHashCode_m1BC0733E0C3851ED9D1B6C9C0B243BB88BE77AD0 (float* __this, const RuntimeMethod* method);
// System.Int32 System.Int32::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Int32_GetHashCode_m245C424ECE351E5FE3277A88EEB02132DAB8C25A (int32_t* __this, const RuntimeMethod* method);
// System.Int32 UnityEngine.XR.ARSubsystems.XRRaycastHit::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRRaycastHit_GetHashCode_m74F67C2F858CF9669399A90DC761E0E763C0827F (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastHit::Equals(UnityEngine.XR.ARSubsystems.XRRaycastHit)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRRaycastHit_Equals_m2E3F746F63AC5ED95DF5E79AB43C2DE8A8E42E60 (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  ___other0, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastHit::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRRaycastHit_Equals_m0A24B5C58B6CA930CDD05F6F17F54FD60DA10DE5 (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, RuntimeObject * ___obj0, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.TrackableId::Equals(UnityEngine.XR.ARSubsystems.TrackableId)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool TrackableId_Equals_mCE458E0FDCDD6E339FCC1926EE88EB7B3D45F943 (TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___other0, const RuntimeMethod* method);
// System.Boolean UnityEngine.Pose::Equals(UnityEngine.Pose)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Pose_Equals_m867264C8DF91FF8DC3AD957EF1625902CDEBAEDD (Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 * __this, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___other0, const RuntimeMethod* method);
// System.Boolean System.Single::Equals(System.Single)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Single_Equals_mCDFA927E712FBA83D076864E16C77E39A6E66FE7 (float* __this, float ___obj0, const RuntimeMethod* method);
// UnityEngine.XR.ARSubsystems.TrackableId UnityEngine.XR.ARSubsystems.TrackableId::get_invalidId()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  TrackableId_get_invalidId_mBE9FA1EC8F2EC1575C1B31666EA928A3382DF1CD_inline (const RuntimeMethod* method);
// UnityEngine.Pose UnityEngine.Pose::get_identity()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  Pose_get_identity_m19458DF84EAADD5E9302CABDC385B97DC91ECCBF (const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRSubsystem`1<UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor>::.ctor()
inline void XRSubsystem_1__ctor_m9E32FFF260FC17D2C65B47C162DD6B914C8867FB (XRSubsystem_1_tD0089F4B0849EA9C41ED01F79404C4A1A0B31B9B * __this, const RuntimeMethod* method)
{
	((  void (*) (XRSubsystem_1_tD0089F4B0849EA9C41ED01F79404C4A1A0B31B9B *, const RuntimeMethod*))XRSubsystem_1__ctor_mE4899E6701EB0E563C03F950450FDDAC4FF21BAF_gshared)(__this, method);
}
// UnityEngine.XR.ARSubsystems.XRRaycastHit UnityEngine.XR.ARSubsystems.XRRaycastHit::get_defaultValue()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  XRRaycastHit_get_defaultValue_m17AEBDAC971A56C3FC4C7C4E2E14ECC357658DFA_inline (const RuntimeMethod* method);
// System.Void System.NotSupportedException::.ctor(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void NotSupportedException__ctor_mD023A89A5C1F740F43F0A9CD6C49DC21230B3CEE (NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 * __this, String_t* ___message0, const RuntimeMethod* method);
// System.Void System.Object::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Object__ctor_m925ECA5E85CA100E3FB86A4F9E15C120E9A184C0 (RuntimeObject * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor::.ctor(UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRRaycastSubsystemDescriptor__ctor_m449AFD6137C639FB21F17C456B20BC954875BC9E (XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7 * __this, Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704  ___cinfo0, const RuntimeMethod* method);
// System.Boolean UnityEngine.SubsystemRegistration::CreateDescriptor(UnityEngine.SubsystemDescriptor)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool SubsystemRegistration_CreateDescriptor_m90E54AA3B7A6EC958B51A4775DC75DA4149F9DF2 (SubsystemDescriptor_t889FE96B75F157379A8337541A119F08DD9C75D4 * ___descriptor0, const RuntimeMethod* method);
// System.Void UnityEngine.SubsystemDescriptor`1<UnityEngine.XR.ARSubsystems.XRRaycastSubsystem>::.ctor()
inline void SubsystemDescriptor_1__ctor_mCE93F7D6739E1AADE5429632560447F18FA623F6 (SubsystemDescriptor_1_t27A8F3D552922AA16C799BCC575330BC6DF59F47 * __this, const RuntimeMethod* method)
{
	((  void (*) (SubsystemDescriptor_1_t27A8F3D552922AA16C799BCC575330BC6DF59F47 *, const RuntimeMethod*))SubsystemDescriptor_1__ctor_m524E72319B102242E9AE86C5C5A529D6291D8602_gshared)(__this, method);
}
// System.String UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo::get_id()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR String_t* Cinfo_get_id_m0418C70EB8FBC60BB9B83053AA5175AEFE31CAF8_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.SubsystemDescriptor::set_id(System.String)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void SubsystemDescriptor_set_id_m3289D5DA5921130F0338C6332907E686E36FF064_inline (SubsystemDescriptor_t889FE96B75F157379A8337541A119F08DD9C75D4 * __this, String_t* ___value0, const RuntimeMethod* method);
// System.Type UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo::get_subsystemImplementationType()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Type_t * Cinfo_get_subsystemImplementationType_m0E0FEE226FD08939BF83B7B2644BEC6362BA157B_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.SubsystemDescriptor::set_subsystemImplementationType(System.Type)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void SubsystemDescriptor_set_subsystemImplementationType_m727AB6BED84E57A382B8804F95A75A8012963EE1_inline (SubsystemDescriptor_t889FE96B75F157379A8337541A119F08DD9C75D4 * __this, Type_t * ___value0, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo::get_supportsViewportBasedRaycast()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR bool Cinfo_get_supportsViewportBasedRaycast_m323FE06DA2E4222E6BF4CE89541DB4630CB254B3_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor::set_supportsViewportBasedRaycast(System.Boolean)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRRaycastSubsystemDescriptor_set_supportsViewportBasedRaycast_m7A6EBE60F40966D0314C378B1441C7DB41C0720D_inline (XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7 * __this, bool ___value0, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo::get_supportsWorldBasedRaycast()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR bool Cinfo_get_supportsWorldBasedRaycast_mFBB112B068EE22D519CAC45E35255D6FDACCAE74_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor::set_supportsWorldBasedRaycast(System.Boolean)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRRaycastSubsystemDescriptor_set_supportsWorldBasedRaycast_m23E91C1C3684B0FE2AF37D3BE2A79B0D88BFC7B3_inline (XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7 * __this, bool ___value0, const RuntimeMethod* method);
// UnityEngine.XR.ARSubsystems.TrackableType UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo::get_supportedTrackableTypes()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t Cinfo_get_supportedTrackableTypes_mCC57E28DFCE93ECA772B1DE2E3E49AD030D79424_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor::set_supportedTrackableTypes(UnityEngine.XR.ARSubsystems.TrackableType)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRRaycastSubsystemDescriptor_set_supportedTrackableTypes_mA429421E574C9261CFC271AC43A521E43B990DCD_inline (XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7 * __this, int32_t ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo::set_id(System.String)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_id_mFFFBB447D6D0DF4A27428D414FE23BCCB16D78D1_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, String_t* ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo::set_subsystemImplementationType(System.Type)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_subsystemImplementationType_m00E3D77F5C33C670222169A514C9804886D4FD72_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, Type_t * ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo::set_supportsViewportBasedRaycast(System.Boolean)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_supportsViewportBasedRaycast_mBA63D727FCB8E5FE2EF1BD5CA2192B54768DEF0D_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, bool ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo::set_supportsWorldBasedRaycast(System.Boolean)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_supportsWorldBasedRaycast_mDDA30FB5ADF2800F44467B727F19668AAE05AFEF_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, bool ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo::set_supportedTrackableTypes(UnityEngine.XR.ARSubsystems.TrackableType)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_supportedTrackableTypes_m91638341F04B3BC3688660AFFE66308C19B13C6B_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, int32_t ___value0, const RuntimeMethod* method);
// System.Boolean System.Type::op_Equality(System.Type,System.Type)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Type_op_Equality_m7040622C9E1037EFC73E1F0EDB1DD241282BE3D8 (Type_t * ___left0, Type_t * ___right1, const RuntimeMethod* method);
// System.Int32 System.Boolean::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Boolean_GetHashCode_m92C426D44100ED098FEECC96A743C3CB92DFF737 (bool* __this, const RuntimeMethod* method);
// System.Int32 UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Cinfo_GetHashCode_mCC56E718130099F9809415C6C1CE9DC981F06211 (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo::Equals(UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_Equals_mC668E44130D6114FFD62DA8470840EE5E39DBBB5 (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704  ___other0, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_Equals_mA57BDAF996011C56A0017EFBBB805683CFA03452 (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, RuntimeObject * ___obj0, const RuntimeMethod* method);
// System.String System.String::Format(System.String,System.Object,System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* String_Format_m19325298DBC61AAC016C16F7B3CF97A8A3DEA34A (String_t* ___format0, RuntimeObject * ___arg01, RuntimeObject * ___arg12, const RuntimeMethod* method);
// System.String UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo::ToString()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Cinfo_ToString_m9E094799A4E711569550478753C87EE9FC40DC24 (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, const RuntimeMethod* method);
// System.Boolean System.String::Equals(System.String,System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool String_Equals_m90EB651A751C3444BADBBD5401109CE05B3E1CFB (String_t* ___a0, String_t* ___b1, const RuntimeMethod* method);
// System.Boolean System.Nullable`1<UnityEngine.Vector2>::get_HasValue()
inline bool Nullable_1_get_HasValue_m86766498BBF2DC3AB0128F33AC3F200BF93BB1B6_inline (Nullable_1_t2D31B61CB1F8452E868737CCA40082AA2D1F58DC * __this, const RuntimeMethod* method)
{
	return ((  bool (*) (Nullable_1_t2D31B61CB1F8452E868737CCA40082AA2D1F58DC *, const RuntimeMethod*))Nullable_1_get_HasValue_m86766498BBF2DC3AB0128F33AC3F200BF93BB1B6_gshared_inline)(__this, method);
}
// UnityEngine.Vector2 UnityEngine.Vector2::get_zero()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  Vector2_get_zero_mFE0C3213BB698130D6C5247AB4B887A59074D0A8 (const RuntimeMethod* method);
// !0 System.Nullable`1<UnityEngine.Vector2>::get_Value()
inline Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  Nullable_1_get_Value_m763182B16D2A96848EAFACFA31AC9985EFCC0F30 (Nullable_1_t2D31B61CB1F8452E868737CCA40082AA2D1F58DC * __this, const RuntimeMethod* method)
{
	return ((  Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  (*) (Nullable_1_t2D31B61CB1F8452E868737CCA40082AA2D1F58DC *, const RuntimeMethod*))Nullable_1_get_Value_m763182B16D2A96848EAFACFA31AC9985EFCC0F30_gshared)(__this, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferenceImage::.ctor(UnityEngine.XR.ARSubsystems.SerializableGuid,UnityEngine.XR.ARSubsystems.SerializableGuid,System.Nullable`1<UnityEngine.Vector2>,System.String,UnityEngine.Texture2D)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRReferenceImage__ctor_m6B1ECFC5354FC9FDC73635BF5E693DA33DE02A4B (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  ___guid0, SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  ___textureGuid1, Nullable_1_t2D31B61CB1F8452E868737CCA40082AA2D1F58DC  ___size2, String_t* ___name3, Texture2D_tBBF96AC337723E2EF156DF17E09D4379FD05DE1C * ___texture4, const RuntimeMethod* method);
// System.Guid UnityEngine.XR.ARSubsystems.SerializableGuid::get_guid()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Guid_t  SerializableGuid_get_guid_mDD1F60EF61B262769627D5F48F8840285E1986A0 (SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821 * __this, const RuntimeMethod* method);
// System.Guid UnityEngine.XR.ARSubsystems.XRReferenceImage::get_guid()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Guid_t  XRReferenceImage_get_guid_mEFF96705B63F80C7C38125D170F7E62B784AEED2 (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method);
// System.Guid UnityEngine.XR.ARSubsystems.XRReferenceImage::get_textureGuid()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Guid_t  XRReferenceImage_get_textureGuid_m818FF686F0FAB8AE85630A62E3FA74C2C81C2AC0 (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferenceImage::get_specifySize()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR bool XRReferenceImage_get_specifySize_m5792CAE7DC7ADB8591E770B1F32D71BCCE9F0597_inline (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method);
// UnityEngine.Vector2 UnityEngine.XR.ARSubsystems.XRReferenceImage::get_size()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  XRReferenceImage_get_size_m29A6DA526141F214BE2949524305EFE91C07FA32_inline (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method);
// System.Single UnityEngine.XR.ARSubsystems.XRReferenceImage::get_width()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR float XRReferenceImage_get_width_m52157ED5E292633BA8BA0CFE7F97A756B6985DB8 (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method);
// System.Single UnityEngine.XR.ARSubsystems.XRReferenceImage::get_height()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR float XRReferenceImage_get_height_mA55A287CAA626B1817465DA78D946D3EB82E0D8D (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method);
// System.String UnityEngine.XR.ARSubsystems.XRReferenceImage::get_name()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR String_t* XRReferenceImage_get_name_m6DD83ECED755444645A14816918747E2EEAE92A9_inline (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method);
// UnityEngine.Texture2D UnityEngine.XR.ARSubsystems.XRReferenceImage::get_texture()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Texture2D_tBBF96AC337723E2EF156DF17E09D4379FD05DE1C * XRReferenceImage_get_texture_m97887B57DD747DCE051484D1C97F1240B673FE16_inline (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method);
// System.String System.String::Format(System.String,System.Object[])
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* String_Format_mA3AC3FE7B23D97F3A5BAA082D25B0E01B341A865 (String_t* ___format0, ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* ___args1, const RuntimeMethod* method);
// System.String UnityEngine.XR.ARSubsystems.XRReferenceImage::ToString()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* XRReferenceImage_ToString_m37745D8B95903B29F917CE6E0A141E79E6F4B937 (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method);
// System.Int32 System.Guid::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Guid_GetHashCode_mEB01C6BA267B1CCD624BCA91D09B803C9B6E5369 (Guid_t * __this, const RuntimeMethod* method);
// System.Int32 UnityEngine.XR.ARSubsystems.XRReferenceImage::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRReferenceImage_GetHashCode_mFBF4D8E0A33B3EFDCB6D6E5A944AB4CF52AAB334 (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferenceImage::Equals(UnityEngine.XR.ARSubsystems.XRReferenceImage)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferenceImage_Equals_m5FBBB7CFF96645894AA08221795ABC2A98E4DEF5 (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  ___other0, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferenceImage::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferenceImage_Equals_m90DF560ECB15F4363E40AF3C3A2F82F5E0FD147D (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, RuntimeObject * ___obj0, const RuntimeMethod* method);
// System.Boolean System.Guid::Equals(System.Guid)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Guid_Equals_mC7FC66A530A8B6FC95E8F5F9E34AE81FD44CD245 (Guid_t * __this, Guid_t  ___g0, const RuntimeMethod* method);
// System.Int32 System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceImage>::get_Count()
inline int32_t List_1_get_Count_mC10370473AE5833D24A5F9361290A6B38CC73C32_inline (List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * __this, const RuntimeMethod* method)
{
	return ((  int32_t (*) (List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F *, const RuntimeMethod*))List_1_get_Count_mC10370473AE5833D24A5F9361290A6B38CC73C32_gshared_inline)(__this, method);
}
// System.Collections.Generic.List`1/Enumerator<!0> System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceImage>::GetEnumerator()
inline Enumerator_t90B18C0FA1516886E459FA26C65233B656F793EB  List_1_GetEnumerator_mD7B1224990EE97B1239AF20ACDA9FC21BCAEEBC5 (List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * __this, const RuntimeMethod* method)
{
	return ((  Enumerator_t90B18C0FA1516886E459FA26C65233B656F793EB  (*) (List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F *, const RuntimeMethod*))List_1_GetEnumerator_mD7B1224990EE97B1239AF20ACDA9FC21BCAEEBC5_gshared)(__this, method);
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRReferenceImageLibrary::get_count()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRReferenceImageLibrary_get_count_m031E8D7C22B586EE44B0256912F688867E9DEB84 (XRReferenceImageLibrary_t38B21DC6650EADA892125F045DCBF71EBC3F6A8F * __this, const RuntimeMethod* method);
// System.Void System.IndexOutOfRangeException::.ctor(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void IndexOutOfRangeException__ctor_mCCE2EFF47A0ACB4B2636F63140F94FCEA71A9BCA (IndexOutOfRangeException_tEC7665FC66525AB6A6916A7EB505E5591683F0CF * __this, String_t* ___message0, const RuntimeMethod* method);
// !0 System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceImage>::get_Item(System.Int32)
inline XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  List_1_get_Item_m0935B6D4B21A74A42474E6C822B5DA4F8A32FE00_inline (List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * __this, int32_t ___index0, const RuntimeMethod* method)
{
	return ((  XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  (*) (List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F *, int32_t, const RuntimeMethod*))List_1_get_Item_m0935B6D4B21A74A42474E6C822B5DA4F8A32FE00_gshared_inline)(__this, ___index0, method);
}
// System.Int32 System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceImage>::IndexOf(!0)
inline int32_t List_1_IndexOf_m60FE5836AABB98EF6AD4A55ECB7085D854CA5E87 (List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * __this, XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  ___item0, const RuntimeMethod* method)
{
	return ((  int32_t (*) (List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F *, XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E , const RuntimeMethod*))List_1_IndexOf_m60FE5836AABB98EF6AD4A55ECB7085D854CA5E87_gshared)(__this, ___item0, method);
}
// System.Guid UnityEngine.XR.ARSubsystems.GuidUtil::Compose(System.UInt64,System.UInt64)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Guid_t  GuidUtil_Compose_mF0A2DF31C9F5E45DC7786601C82B926546B021D4 (uint64_t ___low0, uint64_t ___high1, const RuntimeMethod* method);
// System.Void System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceImage>::.ctor()
inline void List_1__ctor_m95E964C07FFF3EC898BA56E8B9ADE6E55ECEA0B9 (List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * __this, const RuntimeMethod* method)
{
	((  void (*) (List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F *, const RuntimeMethod*))List_1__ctor_m95E964C07FFF3EC898BA56E8B9ADE6E55ECEA0B9_gshared)(__this, method);
}
// System.Void UnityEngine.ScriptableObject::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void ScriptableObject__ctor_m6E2B3821A4A361556FC12E9B1C71E1D5DC002C5B (ScriptableObject_tAB015486CEAB714DA0D5C1BA389B84FB90427734 * __this, const RuntimeMethod* method);
// System.String UnityEngine.XR.ARSubsystems.XRReferenceObject::get_name()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR String_t* XRReferenceObject_get_name_mB9D4C5BF34D4FF064180412CCE6585867BA98718_inline (XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * __this, const RuntimeMethod* method);
// System.Guid UnityEngine.XR.ARSubsystems.XRReferenceObject::get_guid()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Guid_t  XRReferenceObject_get_guid_mA7BD0F3F54EABC39D19355113087CD4DFF94BE57 (XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * __this, const RuntimeMethod* method);
// System.Void System.ArgumentNullException::.ctor(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void ArgumentNullException__ctor_mEE0C0D6FCB2D08CD7967DBB1329A0854BBED49ED (ArgumentNullException_t581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD * __this, String_t* ___paramName0, const RuntimeMethod* method);
// System.Collections.Generic.List`1/Enumerator<!0> System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceObjectEntry>::GetEnumerator()
inline Enumerator_t8F224602FB8179E9E02E220ED94A72EA075C763F  List_1_GetEnumerator_mC07E381535940BCF47E02BD6CA84103EBBBB217E (List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE * __this, const RuntimeMethod* method)
{
	return ((  Enumerator_t8F224602FB8179E9E02E220ED94A72EA075C763F  (*) (List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE *, const RuntimeMethod*))List_1_GetEnumerator_m52CC760E475D226A2B75048D70C4E22692F9F68D_gshared)(__this, method);
}
// !0 System.Collections.Generic.List`1/Enumerator<UnityEngine.XR.ARSubsystems.XRReferenceObjectEntry>::get_Current()
inline XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8 * Enumerator_get_Current_mD47F152CC824D5536C956CF7F08372836B4344C6_inline (Enumerator_t8F224602FB8179E9E02E220ED94A72EA075C763F * __this, const RuntimeMethod* method)
{
	return ((  XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8 * (*) (Enumerator_t8F224602FB8179E9E02E220ED94A72EA075C763F *, const RuntimeMethod*))Enumerator_get_Current_mD7829C7E8CFBEDD463B15A951CDE9B90A12CC55C_gshared_inline)(__this, method);
}
// System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Object_op_Inequality_m31EF58E217E8F4BDD3E409DEF79E1AEE95874FC1 (Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0 * ___x0, Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0 * ___y1, const RuntimeMethod* method);
// System.Type System.Object::GetType()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Type_t * Object_GetType_m2E0B62414ECCAA3094B703790CE88CBB2F83EA60 (RuntimeObject * __this, const RuntimeMethod* method);
// System.Boolean System.Collections.Generic.List`1/Enumerator<UnityEngine.XR.ARSubsystems.XRReferenceObjectEntry>::MoveNext()
inline bool Enumerator_MoveNext_m0B347A783040C9CE4B9C1889A87ED80B3A0DCB8B (Enumerator_t8F224602FB8179E9E02E220ED94A72EA075C763F * __this, const RuntimeMethod* method)
{
	return ((  bool (*) (Enumerator_t8F224602FB8179E9E02E220ED94A72EA075C763F *, const RuntimeMethod*))Enumerator_MoveNext_m38B1099DDAD7EEDE2F4CDAB11C095AC784AC2E34_gshared)(__this, method);
}
// System.Void System.Collections.Generic.List`1/Enumerator<UnityEngine.XR.ARSubsystems.XRReferenceObjectEntry>::Dispose()
inline void Enumerator_Dispose_m7B6EFE249DF6F43B30BEE8350B87A4231CAA0406 (Enumerator_t8F224602FB8179E9E02E220ED94A72EA075C763F * __this, const RuntimeMethod* method)
{
	((  void (*) (Enumerator_t8F224602FB8179E9E02E220ED94A72EA075C763F *, const RuntimeMethod*))Enumerator_Dispose_m94D0DAE031619503CDA6E53C5C3CC78AF3139472_gshared)(__this, method);
}
// UnityEngine.XR.ARSubsystems.XRReferenceObjectEntry UnityEngine.XR.ARSubsystems.XRReferenceObject::FindEntry(System.Type)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8 * XRReferenceObject_FindEntry_mBCEBCEF4265B7D210FFA15179493BF8BDBB70C94 (XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * __this, Type_t * ___type0, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferenceObject::Equals(UnityEngine.XR.ARSubsystems.XRReferenceObject)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferenceObject_Equals_m504798221B2E8A72005FD241A5C5E2A063FF37A2 (XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * __this, XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  ___other0, const RuntimeMethod* method);
// System.Int32 System.UInt64::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t UInt64_GetHashCode_mCBB4031BF70D0CBD023B4D71F4FEA37BE2C749AD (uint64_t* __this, const RuntimeMethod* method);
// System.Int32 UnityEngine.XR.ARSubsystems.XRReferenceObject::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRReferenceObject_GetHashCode_mE918BE08147EE066AF8CC5971E4F5A9221881678 (XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * __this, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferenceObject::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferenceObject_Equals_mC83DEBBA89CBF9EF334B79634C9CA099B166DBC9 (XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * __this, RuntimeObject * ___obj0, const RuntimeMethod* method);
// System.Int32 System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceObject>::get_Count()
inline int32_t List_1_get_Count_mA303D95DAA9717D8D3148C3F20FA1B2750CE77B4_inline (List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * __this, const RuntimeMethod* method)
{
	return ((  int32_t (*) (List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 *, const RuntimeMethod*))List_1_get_Count_mA303D95DAA9717D8D3148C3F20FA1B2750CE77B4_gshared_inline)(__this, method);
}
// System.Collections.Generic.List`1/Enumerator<!0> System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceObject>::GetEnumerator()
inline Enumerator_t3D111B64EA07FCA753146B875D55422A5191B57F  List_1_GetEnumerator_m7ABEC2AD664AC6AC1073E546BAAFC0350626A321 (List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * __this, const RuntimeMethod* method)
{
	return ((  Enumerator_t3D111B64EA07FCA753146B875D55422A5191B57F  (*) (List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 *, const RuntimeMethod*))List_1_GetEnumerator_m7ABEC2AD664AC6AC1073E546BAAFC0350626A321_gshared)(__this, method);
}
// !0 System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceObject>::get_Item(System.Int32)
inline XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  List_1_get_Item_mE2BB4CDB177C0DA1EF1FB0A174459691F207C7DF_inline (List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * __this, int32_t ___index0, const RuntimeMethod* method)
{
	return ((  XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  (*) (List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 *, int32_t, const RuntimeMethod*))List_1_get_Item_mE2BB4CDB177C0DA1EF1FB0A174459691F207C7DF_gshared_inline)(__this, ___index0, method);
}
// System.Int32 System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceObject>::IndexOf(!0)
inline int32_t List_1_IndexOf_m80147D21315270B53CA3E7AE6FA3B97264231AB2 (List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * __this, XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  ___item0, const RuntimeMethod* method)
{
	return ((  int32_t (*) (List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 *, XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA , const RuntimeMethod*))List_1_IndexOf_m80147D21315270B53CA3E7AE6FA3B97264231AB2_gshared)(__this, ___item0, method);
}
// System.Void System.Collections.Generic.List`1<UnityEngine.XR.ARSubsystems.XRReferenceObject>::.ctor()
inline void List_1__ctor_m370BA08A79B7950CA8E75862CD6F4921BAF6D7AB (List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * __this, const RuntimeMethod* method)
{
	((  void (*) (List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 *, const RuntimeMethod*))List_1__ctor_m370BA08A79B7950CA8E75862CD6F4921BAF6D7AB_gshared)(__this, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePoint::.ctor(UnityEngine.XR.ARSubsystems.TrackableId,UnityEngine.Pose,UnityEngine.XR.ARSubsystems.TrackingState,System.IntPtr)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRReferencePoint__ctor_mF823D8DC414C0B110A0C29F28668C1F3DA2B2997 (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___trackableId0, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___pose1, int32_t ___trackingState2, intptr_t ___nativePtr3, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePoint::.ctor(UnityEngine.XR.ARSubsystems.TrackableId,UnityEngine.Pose,UnityEngine.XR.ARSubsystems.TrackingState,System.IntPtr,System.Guid)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRReferencePoint__ctor_m816965A70CDD2827DE0808A49B135E411E8532BB (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___trackableId0, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___pose1, int32_t ___trackingState2, intptr_t ___nativePtr3, Guid_t  ___sessionId4, const RuntimeMethod* method);
// UnityEngine.XR.ARSubsystems.TrackableId UnityEngine.XR.ARSubsystems.XRReferencePoint::get_trackableId()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  XRReferencePoint_get_trackableId_m6D53542802F2444CE58861B8868274F9A8296D88_inline (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, const RuntimeMethod* method);
// UnityEngine.Pose UnityEngine.XR.ARSubsystems.XRReferencePoint::get_pose()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  XRReferencePoint_get_pose_mA4320629B8C7AE23D97FCD8E2C5FB9C9FB6AED9C_inline (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, const RuntimeMethod* method);
// UnityEngine.XR.ARSubsystems.TrackingState UnityEngine.XR.ARSubsystems.XRReferencePoint::get_trackingState()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRReferencePoint_get_trackingState_mBA0DB4050B734039D22D0ECF69CD6E8896DF52B8_inline (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, const RuntimeMethod* method);
// System.IntPtr UnityEngine.XR.ARSubsystems.XRReferencePoint::get_nativePtr()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR intptr_t XRReferencePoint_get_nativePtr_mE9EC85AD0E4976145CB0EDC4A74AA5BB076C5789_inline (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, const RuntimeMethod* method);
// System.Guid UnityEngine.XR.ARSubsystems.XRReferencePoint::get_sessionId()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Guid_t  XRReferencePoint_get_sessionId_m5DCAF1725B8A29481940252D80634C99A3C2F0D0_inline (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, const RuntimeMethod* method);
// System.Int32 System.IntPtr::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t IntPtr_GetHashCode_m0A6AE0C85A4AEEA127235FB5A32056F630E3749A (intptr_t* __this, const RuntimeMethod* method);
// System.Int32 UnityEngine.XR.ARSubsystems.XRReferencePoint::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRReferencePoint_GetHashCode_mD7BC968C92D3CC25E7D06502570A94B104F9E32C (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, const RuntimeMethod* method);
// System.Boolean System.IntPtr::op_Equality(System.IntPtr,System.IntPtr)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool IntPtr_op_Equality_mEE8D9FD2DFE312BBAA8B4ED3BF7976B3142A5934 (intptr_t ___value10, intptr_t ___value21, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePoint::Equals(UnityEngine.XR.ARSubsystems.XRReferencePoint)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferencePoint_Equals_mA58F0C1C266D740037A7D6700857A5E739160AF8 (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  ___other0, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePoint::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferencePoint_Equals_mD22BFD6609737E5CC6A31D2C1B519CD5207C89BC (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, RuntimeObject * ___obj0, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.TrackingSubsystem`2<UnityEngine.XR.ARSubsystems.XRReferencePoint,UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor>::.ctor()
inline void TrackingSubsystem_2__ctor_mC69AD6AEF92FA2B9D7D5B83C79A418BAB05D0F76 (TrackingSubsystem_2_t62AFA2295FCEFC2C3818E3B9EDB3C1AF80509899 * __this, const RuntimeMethod* method)
{
	((  void (*) (TrackingSubsystem_2_t62AFA2295FCEFC2C3818E3B9EDB3C1AF80509899 *, const RuntimeMethod*))TrackingSubsystem_2__ctor_m7BED931B501BCE6534C3BC2DC0CA30C73E917F90_gshared)(__this, method);
}
// System.Void System.InvalidOperationException::.ctor(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706 (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * __this, String_t* ___message0, const RuntimeMethod* method);
// UnityEngine.XR.ARSubsystems.XRReferencePoint UnityEngine.XR.ARSubsystems.XRReferencePoint::get_defaultValue()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  XRReferencePoint_get_defaultValue_mCCFAF4140E24AC2FDF1C8D19043E57B6BFEAC0AD_inline (const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor::.ctor(UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor/Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRReferencePointSubsystemDescriptor__ctor_mB5AD6FF2521FF148612D7662DF93BD6CA68069B2 (XRReferencePointSubsystemDescriptor_t5972C5E3DEF485A89A3AA7D9D47CCD8A67FDB74D * __this, Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD  ___cinfo0, const RuntimeMethod* method);
// System.Void UnityEngine.SubsystemDescriptor`1<UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem>::.ctor()
inline void SubsystemDescriptor_1__ctor_m39ABF0C7663CF93F6B094FB18264078421C6950F (SubsystemDescriptor_1_tFFF984F9F6942FDAE965131A8954D4C4EA20B6B5 * __this, const RuntimeMethod* method)
{
	((  void (*) (SubsystemDescriptor_1_tFFF984F9F6942FDAE965131A8954D4C4EA20B6B5 *, const RuntimeMethod*))SubsystemDescriptor_1__ctor_m524E72319B102242E9AE86C5C5A529D6291D8602_gshared)(__this, method);
}
// System.String UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor/Cinfo::get_id()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR String_t* Cinfo_get_id_mB1E35C0B52EEAA8EB934C4D3F02465CF8A752015_inline (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, const RuntimeMethod* method);
// System.Type UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor/Cinfo::get_subsystemImplementationType()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Type_t * Cinfo_get_subsystemImplementationType_m764697AF3D79BDFA6010287A8B542F9323693096_inline (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor/Cinfo::get_supportsTrackableAttachments()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR bool Cinfo_get_supportsTrackableAttachments_m041B030B1BD0114D8FCE9A9F804CFF5984FB07BD_inline (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor::set_supportsTrackableAttachments(System.Boolean)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRReferencePointSubsystemDescriptor_set_supportsTrackableAttachments_mA4F7709D4C170D414F862490ABBF0090DE46A8AB_inline (XRReferencePointSubsystemDescriptor_t5972C5E3DEF485A89A3AA7D9D47CCD8A67FDB74D * __this, bool ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor/Cinfo::set_id(System.String)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_id_m9CAE75E21B0DAE38A8619D1B04D17EDEC81E97D7_inline (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, String_t* ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor/Cinfo::set_subsystemImplementationType(System.Type)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_subsystemImplementationType_m9D5112215C6766E6561E42DA858B7F3D72F0005E_inline (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, Type_t * ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor/Cinfo::set_supportsTrackableAttachments(System.Boolean)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_supportsTrackableAttachments_mC0A061EDA609485B0E83FC7857E8573C93F38FD7_inline (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, bool ___value0, const RuntimeMethod* method);
// System.Int32 UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor/Cinfo::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Cinfo_GetHashCode_mA1002FD6C9DDB0C39442B7692A7CDB5562C61086 (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor/Cinfo::Equals(UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor/Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_Equals_m7D9C657E628D2DFC390587FBE0BDFEB1D5CDC92D (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD  ___other0, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor/Cinfo::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_Equals_mC210DE78D45CD980BF7B5A186241CFC5CC243D2E (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, RuntimeObject * ___obj0, const RuntimeMethod* method);
// !0 UnityEngine.Subsystem`1<UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor>::get_SubsystemDescriptor()
inline XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079 * Subsystem_1_get_SubsystemDescriptor_mD9F3F4ECB180FF40D21E57235A6D775BDB014D9B (Subsystem_1_t749909AA5D71BD615BDF40BDE7C8F81B8D63F4C7 * __this, const RuntimeMethod* method)
{
	return ((  XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079 * (*) (Subsystem_1_t749909AA5D71BD615BDF40BDE7C8F81B8D63F4C7 *, const RuntimeMethod*))Subsystem_1_get_SubsystemDescriptor_mA35B9EF0D8FB264CBDA2C501B3C251816226A412_gshared)(__this, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor::get_supportsInstall()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR bool XRSessionSubsystemDescriptor_get_supportsInstall_m2AA89682007FE1D8BB811FD152DE326FF7BB5A99_inline (XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRSubsystem`1<UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor>::.ctor()
inline void XRSubsystem_1__ctor_m3D9DD5E9EFF90C8BD6855FB1E5FED02E410B19E6 (XRSubsystem_1_t4F81A9C71B4D3ABB1A7FF9080551E6E0951A5F5B * __this, const RuntimeMethod* method)
{
	((  void (*) (XRSubsystem_1_t4F81A9C71B4D3ABB1A7FF9080551E6E0951A5F5B *, const RuntimeMethod*))XRSubsystem_1__ctor_mE4899E6701EB0E563C03F950450FDDAC4FF21BAF_gshared)(__this, method);
}
// UnityEngine.XR.ARSubsystems.Promise`1<T> UnityEngine.XR.ARSubsystems.Promise`1<UnityEngine.XR.ARSubsystems.SessionAvailability>::CreateResolvedPromise(T)
inline Promise_1_t7E6FC51116CF1FB8FFF9D4D6621E5F7231AE9A6B * Promise_1_CreateResolvedPromise_m36CC1CB8A654C9849F7BB9FE6C69A725C78CC01D (int32_t ___result0, const RuntimeMethod* method)
{
	return ((  Promise_1_t7E6FC51116CF1FB8FFF9D4D6621E5F7231AE9A6B * (*) (int32_t, const RuntimeMethod*))Promise_1_CreateResolvedPromise_mB605C1D88AB74006BD3AA2C791854687156606DA_gshared)(___result0, method);
}
// UnityEngine.XR.ARSubsystems.Promise`1<T> UnityEngine.XR.ARSubsystems.Promise`1<UnityEngine.XR.ARSubsystems.SessionInstallationStatus>::CreateResolvedPromise(T)
inline Promise_1_tE8B65103AAD59FA1265B1977CE59DC6DA5220626 * Promise_1_CreateResolvedPromise_m33AA652B2B2460A4777ED6EA73923156280826AA (int32_t ___result0, const RuntimeMethod* method)
{
	return ((  Promise_1_tE8B65103AAD59FA1265B1977CE59DC6DA5220626 * (*) (int32_t, const RuntimeMethod*))Promise_1_CreateResolvedPromise_mB605C1D88AB74006BD3AA2C791854687156606DA_gshared)(___result0, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor::.ctor(UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor/Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRSessionSubsystemDescriptor__ctor_mF2A65C6A814FB2D22D5ED1608E5EFD5B0CD9A6E2 (XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079 * __this, Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A  ___cinfo0, const RuntimeMethod* method);
// System.Void UnityEngine.SubsystemDescriptor`1<UnityEngine.XR.ARSubsystems.XRSessionSubsystem>::.ctor()
inline void SubsystemDescriptor_1__ctor_mE7F10A17EF25AE3D50D0B7D5CAA54D07FE709FAB (SubsystemDescriptor_1_t0679ED150419770F94E42102F28D688E8F1DC9DA * __this, const RuntimeMethod* method)
{
	((  void (*) (SubsystemDescriptor_1_t0679ED150419770F94E42102F28D688E8F1DC9DA *, const RuntimeMethod*))SubsystemDescriptor_1__ctor_m524E72319B102242E9AE86C5C5A529D6291D8602_gshared)(__this, method);
}
// System.String UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor/Cinfo::get_id()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR String_t* Cinfo_get_id_m998EB4BD213159391A0A66FD6C002C1CE5CD14E8_inline (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, const RuntimeMethod* method);
// System.Type UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor/Cinfo::get_subsystemImplementationType()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Type_t * Cinfo_get_subsystemImplementationType_m63942F2B0929DF14EB7885E7553A9970FBD3E108_inline (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor/Cinfo::get_supportsInstall()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR bool Cinfo_get_supportsInstall_m5FBFD4D2F10A6A46F66F4EFBC61EA14DE0FEED99_inline (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor::set_supportsInstall(System.Boolean)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRSessionSubsystemDescriptor_set_supportsInstall_m010EE3F0CB4B143A90B93C1F10F063FB12546920_inline (XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079 * __this, bool ___value0, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor/Cinfo::get_supportsMatchFrameRate()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR bool Cinfo_get_supportsMatchFrameRate_mE591F09F87EA8F4E5563039C47A6331E6AF31895_inline (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor::set_supportsMatchFrameRate(System.Boolean)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRSessionSubsystemDescriptor_set_supportsMatchFrameRate_mC2B0189D51BF3B64026D01DD6A088052C5D74BFC_inline (XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079 * __this, bool ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor/Cinfo::set_supportsInstall(System.Boolean)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_supportsInstall_mD74EB42C503AC5393E630A56E3AE579FE1558660_inline (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, bool ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor/Cinfo::set_supportsMatchFrameRate(System.Boolean)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_supportsMatchFrameRate_m2B92004D3F2E01EA5DDFBF5F928C5604E68B8D21_inline (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, bool ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor/Cinfo::set_id(System.String)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_id_mC4FF3C524E18065C55B5142D58FBD58A66479A41_inline (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, String_t* ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor/Cinfo::set_subsystemImplementationType(System.Type)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_subsystemImplementationType_m3C759AEC2943DE059B20AA7F5A5B932B7432473C_inline (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, Type_t * ___value0, const RuntimeMethod* method);
// System.Boolean System.Type::op_Inequality(System.Type,System.Type)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Type_op_Inequality_m615014191FB05FD50F63A24EB9A6CCA785E7CEC9 (Type_t * ___left0, Type_t * ___right1, const RuntimeMethod* method);
// System.Int32 UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor/Cinfo::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Cinfo_GetHashCode_m06E1060A5995A0C346AB7C0E56CCAC4BEC758A6E (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor/Cinfo::Equals(UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor/Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_Equals_mC919333F29E857CC3F929451A080AC4A38385E01 (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A  ___other0, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor/Cinfo::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_Equals_m2118FE6BBF6355D645E72BDD6662CF313B8E94EB (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, RuntimeObject * ___obj0, const RuntimeMethod* method);
// UnityEngine.ScreenOrientation UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::get_screenOrientation()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRSessionUpdateParams_get_screenOrientation_m111C145EA6A683F025DF48C6EA355E37D8974183_inline (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::set_screenOrientation(UnityEngine.ScreenOrientation)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRSessionUpdateParams_set_screenOrientation_m7C20FD52988E0F21604700B5CDA93FBA63DD28C6_inline (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, int32_t ___value0, const RuntimeMethod* method);
// UnityEngine.Vector2Int UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::get_screenDimensions()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  XRSessionUpdateParams_get_screenDimensions_m61A9722E272D6292B9C7C093BF7792FB007BF21E_inline (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::set_screenDimensions(UnityEngine.Vector2Int)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRSessionUpdateParams_set_screenDimensions_m41570268847916BA02DD2427BDDB08B3D466A905_inline (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  ___value0, const RuntimeMethod* method);
// System.Int32 UnityEngine.Vector2Int::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Vector2Int_GetHashCode_m73E874F4E94DF3D2603035E2E892873B139A7A9E (Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905 * __this, const RuntimeMethod* method);
// System.Int32 UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRSessionUpdateParams_GetHashCode_m3E0C208F41FAC84F879A073F85FB9DC0F1C09520 (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::Equals(UnityEngine.XR.ARSubsystems.XRSessionUpdateParams)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRSessionUpdateParams_Equals_mFE8BAF000FDC02612C5D563960EB974E510DEAB3 (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16  ___other0, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRSessionUpdateParams_Equals_m415AB0E24C9CF0C013872ED16C571B65DACF24B1 (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, RuntimeObject * ___obj0, const RuntimeMethod* method);
// System.String UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::ToString()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* XRSessionUpdateParams_ToString_m7150FEAE08C59544392C3D47B3CB5AC318B82F4A (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, const RuntimeMethod* method);
// System.Boolean UnityEngine.Vector2Int::Equals(UnityEngine.Vector2Int)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Vector2Int_Equals_m65420C995F326F5C340E4825EA5E16BDE68F5A9C (Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905 * __this, Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  ___other0, const RuntimeMethod* method);
// System.IntPtr UnityEngine.XR.ARSubsystems.XRTextureDescriptor::get_nativeTexture()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR intptr_t XRTextureDescriptor_get_nativeTexture_m83CAA03353C203B7D38618C1963C715F052081F8_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRTextureDescriptor::set_nativeTexture(System.IntPtr)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRTextureDescriptor_set_nativeTexture_mEF92A3E263840B8F428C314323C20A11F896D907_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, intptr_t ___value0, const RuntimeMethod* method);
// System.Int32 UnityEngine.XR.ARSubsystems.XRTextureDescriptor::get_width()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRTextureDescriptor_get_width_m158B2CEE4A0F56DF263BB642F5E4A3C3CF339E0B_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRTextureDescriptor::set_width(System.Int32)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRTextureDescriptor_set_width_m59E159F83238991BAD9838C5835A07E44F6A163E_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, int32_t ___value0, const RuntimeMethod* method);
// System.Int32 UnityEngine.XR.ARSubsystems.XRTextureDescriptor::get_height()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRTextureDescriptor_get_height_mCE50370000BCF4A70B95344A0731A771401C0894_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRTextureDescriptor::set_height(System.Int32)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRTextureDescriptor_set_height_mE690E293BE1FE8009052CC87FA454FB79DE9DF0E_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, int32_t ___value0, const RuntimeMethod* method);
// System.Int32 UnityEngine.XR.ARSubsystems.XRTextureDescriptor::get_mipmapCount()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRTextureDescriptor_get_mipmapCount_m491B149D8BBF148B2030214818E237A28D9B6CC4_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRTextureDescriptor::set_mipmapCount(System.Int32)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRTextureDescriptor_set_mipmapCount_m8CC98FD1B188CA92DE7C1C430BF71E11E7AD7858_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, int32_t ___value0, const RuntimeMethod* method);
// UnityEngine.TextureFormat UnityEngine.XR.ARSubsystems.XRTextureDescriptor::get_format()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRTextureDescriptor_get_format_mA2DA22DC1DEBCAD27A9C69F3374D614DF1C3FA2B_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRTextureDescriptor::set_format(UnityEngine.TextureFormat)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRTextureDescriptor_set_format_m2BEDFB4C31E590B2C2AAE7145AEAE714491E0EA6_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, int32_t ___value0, const RuntimeMethod* method);
// System.Int32 UnityEngine.XR.ARSubsystems.XRTextureDescriptor::get_propertyNameId()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRTextureDescriptor_get_propertyNameId_mA3A29036B96A64D1C4F147678E60E2BFCAAAAFF0_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRTextureDescriptor::set_propertyNameId(System.Int32)
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRTextureDescriptor_set_propertyNameId_m87654C29B3CEFA71D22E9F1323058334E8338B4F_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, int32_t ___value0, const RuntimeMethod* method);
// System.Boolean System.IntPtr::op_Inequality(System.IntPtr,System.IntPtr)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool IntPtr_op_Inequality_mB4886A806009EA825EFCC60CD2A7F6EB8E273A61 (intptr_t ___value10, intptr_t ___value21, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRTextureDescriptor::get_valid()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTextureDescriptor_get_valid_mF060565C5E24FDF97771F6FDA3235562DF01977B (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method);
// System.Boolean System.Int32::Equals(System.Int32)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Int32_Equals_mC8C45B8899F291D55A6152C8FEDB3CFFF181170B (int32_t* __this, int32_t ___obj0, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRTextureDescriptor::hasIdenticalTextureMetadata(UnityEngine.XR.ARSubsystems.XRTextureDescriptor)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTextureDescriptor_hasIdenticalTextureMetadata_mD9C2A76A8B680BB7B2742F82235E40977CD098AE (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD  ___other0, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRTextureDescriptor::Reset()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRTextureDescriptor_Reset_m64A787FBD1F11161369A72A7D61763DDF8D74EBC (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRTextureDescriptor::Equals(UnityEngine.XR.ARSubsystems.XRTextureDescriptor)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTextureDescriptor_Equals_m124C4B8E0370717E0714FB2D28493A77034C6E38 (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD  ___other0, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRTextureDescriptor::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTextureDescriptor_Equals_m8D2E1A6303E60A653F70870CBD04845414F6A0A5 (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, RuntimeObject * ___obj0, const RuntimeMethod* method);
// System.Int32 UnityEngine.XR.ARSubsystems.XRTextureDescriptor::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRTextureDescriptor_GetHashCode_mE61628A57D74C31744B57EBFBE8E8EDFA673B65F (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method);
// System.String System.IntPtr::ToString(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* IntPtr_ToString_m6ADB8DBD989D878D694B4031CC08461B1E2C51FF (intptr_t* __this, String_t* ___format0, const RuntimeMethod* method);
// System.String System.Int32::ToString()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Int32_ToString_m1863896DE712BF97C031D55B12E1583F1982DC02 (int32_t* __this, const RuntimeMethod* method);
// System.String UnityEngine.XR.ARSubsystems.XRTextureDescriptor::ToString()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* XRTextureDescriptor_ToString_mA7C17125D54876E04397C7022031B6A346CA9A7F (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRTrackedImage::.ctor(UnityEngine.XR.ARSubsystems.TrackableId,System.Guid,UnityEngine.Pose,UnityEngine.Vector2,UnityEngine.XR.ARSubsystems.TrackingState,System.IntPtr)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRTrackedImage__ctor_mF31D86D7A523FD9EE7F4166A9ABB04272E93436B (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___trackableId0, Guid_t  ___sourceImageId1, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___pose2, Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  ___size3, int32_t ___trackingState4, intptr_t ___nativePtr5, const RuntimeMethod* method);
// UnityEngine.XR.ARSubsystems.TrackableId UnityEngine.XR.ARSubsystems.XRTrackedImage::get_trackableId()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  XRTrackedImage_get_trackableId_m6EB6DBACC95E5EE2AFEE3CE421F4C123F32E9CB8_inline (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method);
// System.Guid UnityEngine.XR.ARSubsystems.XRTrackedImage::get_sourceImageId()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Guid_t  XRTrackedImage_get_sourceImageId_m402089FA779BB9821B50B23F79579466D895939B_inline (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method);
// UnityEngine.Pose UnityEngine.XR.ARSubsystems.XRTrackedImage::get_pose()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  XRTrackedImage_get_pose_m0566E087CA2DC99DF749E80277510C61DCF13186_inline (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method);
// UnityEngine.Vector2 UnityEngine.XR.ARSubsystems.XRTrackedImage::get_size()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  XRTrackedImage_get_size_m746034D0E2FD28C9E48A90965E4FCD9137988906_inline (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method);
// UnityEngine.XR.ARSubsystems.TrackingState UnityEngine.XR.ARSubsystems.XRTrackedImage::get_trackingState()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRTrackedImage_get_trackingState_mA7177B042E8F9F9B584582970BC5FF0377CE94DB_inline (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method);
// System.IntPtr UnityEngine.XR.ARSubsystems.XRTrackedImage::get_nativePtr()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR intptr_t XRTrackedImage_get_nativePtr_mB44BA43B02762B89091D56F254221F0741808629_inline (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method);
// System.Int32 UnityEngine.Vector2::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Vector2_GetHashCode_m028AB6B14EBC6D668CFA45BF6EDEF17E2C44EA54 (Vector2_tA85D2DD88578276CA8A8796756458277E72D073D * __this, const RuntimeMethod* method);
// System.Int32 UnityEngine.XR.ARSubsystems.XRTrackedImage::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRTrackedImage_GetHashCode_mC1A5AB6C756498852952CB1B9F4F69D1177A02A6 (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method);
// System.Boolean UnityEngine.Vector2::Equals(UnityEngine.Vector2)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Vector2_Equals_mD6BF1A738E3CAF57BB46E604B030C072728F4EEB (Vector2_tA85D2DD88578276CA8A8796756458277E72D073D * __this, Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  ___other0, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRTrackedImage::Equals(UnityEngine.XR.ARSubsystems.XRTrackedImage)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTrackedImage_Equals_m12A588942242306FC770FD88421D00750F22A141 (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  ___other0, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRTrackedImage::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTrackedImage_Equals_m7C7F0B2FC7A6818276C2BC763CF0465333453B9C (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, RuntimeObject * ___obj0, const RuntimeMethod* method);
// UnityEngine.XR.ARSubsystems.TrackableId UnityEngine.XR.ARSubsystems.XRTrackedObject::get_trackableId()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  XRTrackedObject_get_trackableId_mB720981791DE599B20879640517A33BE2FE2D84D_inline (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, const RuntimeMethod* method);
// UnityEngine.Pose UnityEngine.XR.ARSubsystems.XRTrackedObject::get_pose()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  XRTrackedObject_get_pose_mF865EAF61AE8767D6A0CCF59494A51F2D670F603_inline (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, const RuntimeMethod* method);
// UnityEngine.XR.ARSubsystems.TrackingState UnityEngine.XR.ARSubsystems.XRTrackedObject::get_trackingState()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRTrackedObject_get_trackingState_m0BD1D36132D7B57151A4CAE07B94238B2AEF3DED_inline (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, const RuntimeMethod* method);
// System.IntPtr UnityEngine.XR.ARSubsystems.XRTrackedObject::get_nativePtr()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR intptr_t XRTrackedObject_get_nativePtr_mD654B09F24E79E99FA2A6B1A95C4EAFDF09C639F_inline (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, const RuntimeMethod* method);
// System.Guid UnityEngine.XR.ARSubsystems.XRTrackedObject::get_referenceObjectGuid()
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Guid_t  XRTrackedObject_get_referenceObjectGuid_m09514BB6AD9782AF342076F85BB28631C458BDC8_inline (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.XR.ARSubsystems.XRTrackedObject::.ctor(UnityEngine.XR.ARSubsystems.TrackableId,UnityEngine.Pose,UnityEngine.XR.ARSubsystems.TrackingState,System.IntPtr,System.Guid)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRTrackedObject__ctor_m81B6436D0E3BA4E73E1445074972DB81E3D27275 (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___trackableId0, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___pose1, int32_t ___trackingState2, intptr_t ___nativePtr3, Guid_t  ___referenceObjectGuid4, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRTrackedObject::Equals(UnityEngine.XR.ARSubsystems.XRTrackedObject)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTrackedObject_Equals_m925ED652F271F772E282C3621290411A259CBEEE (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260  ___other0, const RuntimeMethod* method);
// System.Boolean UnityEngine.XR.ARSubsystems.XRTrackedObject::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTrackedObject_Equals_mF0CA07E970C48D514F2B9BBEC0FE44F46429C524 (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, RuntimeObject * ___obj0, const RuntimeMethod* method);
// System.Int32 UnityEngine.XR.ARSubsystems.XRTrackedObject::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRTrackedObject_GetHashCode_m2F1509AA89026BB34BFFE2C07529AAB3B5B0A429 (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, const RuntimeMethod* method);
// System.Void System.ThrowHelper::ThrowArgumentOutOfRangeException()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void ThrowHelper_ThrowArgumentOutOfRangeException_mBA2AF20A35144E0C43CD721A22EAC9FCA15D6550 (const RuntimeMethod* method);
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Conversion methods for marshalling of: UnityEngine.XR.ARSubsystems.XRPointCloudData
IL2CPP_EXTERN_C void XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA_marshal_pinvoke(const XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA& unmarshaled, XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA_marshaled_pinvoke& marshaled)
{
	Exception_t* ___m_Positions_0Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field 'm_Positions' of type 'XRPointCloudData'.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___m_Positions_0Exception, NULL);
}
IL2CPP_EXTERN_C void XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA_marshal_pinvoke_back(const XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA_marshaled_pinvoke& marshaled, XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA& unmarshaled)
{
	Exception_t* ___m_Positions_0Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field 'm_Positions' of type 'XRPointCloudData'.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___m_Positions_0Exception, NULL);
}
// Conversion method for clean up from marshalling of: UnityEngine.XR.ARSubsystems.XRPointCloudData
IL2CPP_EXTERN_C void XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA_marshal_pinvoke_cleanup(XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA_marshaled_pinvoke& marshaled)
{
}
// Conversion methods for marshalling of: UnityEngine.XR.ARSubsystems.XRPointCloudData
IL2CPP_EXTERN_C void XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA_marshal_com(const XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA& unmarshaled, XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA_marshaled_com& marshaled)
{
	Exception_t* ___m_Positions_0Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field 'm_Positions' of type 'XRPointCloudData'.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___m_Positions_0Exception, NULL);
}
IL2CPP_EXTERN_C void XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA_marshal_com_back(const XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA_marshaled_com& marshaled, XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA& unmarshaled)
{
	Exception_t* ___m_Positions_0Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field 'm_Positions' of type 'XRPointCloudData'.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___m_Positions_0Exception, NULL);
}
// Conversion method for clean up from marshalling of: UnityEngine.XR.ARSubsystems.XRPointCloudData
IL2CPP_EXTERN_C void XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA_marshal_com_cleanup(XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA_marshaled_com& marshaled)
{
}
// Unity.Collections.NativeArray`1<UnityEngine.Vector3> UnityEngine.XR.ARSubsystems.XRPointCloudData::get_positions()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  XRPointCloudData_get_positions_m2BDA572054D639DB35E9FDA3D15AEF3B7B39D40C (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, const RuntimeMethod* method)
{
	{
		NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  L_0 = __this->get_m_Positions_0();
		return L_0;
	}
}
IL2CPP_EXTERN_C  NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  XRPointCloudData_get_positions_m2BDA572054D639DB35E9FDA3D15AEF3B7B39D40C_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * _thisAdjusted = reinterpret_cast<XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA *>(__this + _offset);
	return XRPointCloudData_get_positions_m2BDA572054D639DB35E9FDA3D15AEF3B7B39D40C_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRPointCloudData::set_positions(Unity.Collections.NativeArray`1<UnityEngine.Vector3>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRPointCloudData_set_positions_m78BB0E1E2A5860DAC6F60D6C9A6A37544FF9880E (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  ___value0, const RuntimeMethod* method)
{
	{
		NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  L_0 = ___value0;
		__this->set_m_Positions_0(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void XRPointCloudData_set_positions_m78BB0E1E2A5860DAC6F60D6C9A6A37544FF9880E_AdjustorThunk (RuntimeObject * __this, NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * _thisAdjusted = reinterpret_cast<XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA *>(__this + _offset);
	XRPointCloudData_set_positions_m78BB0E1E2A5860DAC6F60D6C9A6A37544FF9880E_inline(_thisAdjusted, ___value0, method);
}
// Unity.Collections.NativeArray`1<System.Single> UnityEngine.XR.ARSubsystems.XRPointCloudData::get_confidenceValues()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  XRPointCloudData_get_confidenceValues_m156073A1640F58477DBCBAC6BDA05C3BF866ACE6 (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, const RuntimeMethod* method)
{
	{
		NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  L_0 = __this->get_m_ConfidenceValues_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C  NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  XRPointCloudData_get_confidenceValues_m156073A1640F58477DBCBAC6BDA05C3BF866ACE6_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * _thisAdjusted = reinterpret_cast<XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA *>(__this + _offset);
	return XRPointCloudData_get_confidenceValues_m156073A1640F58477DBCBAC6BDA05C3BF866ACE6_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRPointCloudData::set_confidenceValues(Unity.Collections.NativeArray`1<System.Single>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRPointCloudData_set_confidenceValues_mC837DE6B63BF8CBD8F2480C1A4ED3247AABCB861 (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  ___value0, const RuntimeMethod* method)
{
	{
		NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  L_0 = ___value0;
		__this->set_m_ConfidenceValues_1(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void XRPointCloudData_set_confidenceValues_mC837DE6B63BF8CBD8F2480C1A4ED3247AABCB861_AdjustorThunk (RuntimeObject * __this, NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * _thisAdjusted = reinterpret_cast<XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA *>(__this + _offset);
	XRPointCloudData_set_confidenceValues_mC837DE6B63BF8CBD8F2480C1A4ED3247AABCB861_inline(_thisAdjusted, ___value0, method);
}
// Unity.Collections.NativeArray`1<System.UInt64> UnityEngine.XR.ARSubsystems.XRPointCloudData::get_identifiers()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  XRPointCloudData_get_identifiers_m6AA5FE2F151300371A1F5A8310A49A0D4A35BD23 (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, const RuntimeMethod* method)
{
	{
		NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  L_0 = __this->get_m_Identifiers_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C  NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  XRPointCloudData_get_identifiers_m6AA5FE2F151300371A1F5A8310A49A0D4A35BD23_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * _thisAdjusted = reinterpret_cast<XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA *>(__this + _offset);
	return XRPointCloudData_get_identifiers_m6AA5FE2F151300371A1F5A8310A49A0D4A35BD23_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRPointCloudData::set_identifiers(Unity.Collections.NativeArray`1<System.UInt64>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRPointCloudData_set_identifiers_m550B2B8C6EF821D5BBD47C066FF6C961EF0CA562 (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  ___value0, const RuntimeMethod* method)
{
	{
		NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  L_0 = ___value0;
		__this->set_m_Identifiers_2(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void XRPointCloudData_set_identifiers_m550B2B8C6EF821D5BBD47C066FF6C961EF0CA562_AdjustorThunk (RuntimeObject * __this, NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * _thisAdjusted = reinterpret_cast<XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA *>(__this + _offset);
	XRPointCloudData_set_identifiers_m550B2B8C6EF821D5BBD47C066FF6C961EF0CA562_inline(_thisAdjusted, ___value0, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRPointCloudData::Dispose()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRPointCloudData_Dispose_mDF78595F088472E60327A1D366AA787C68A3EDE3 (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRPointCloudData_Dispose_mDF78595F088472E60327A1D366AA787C68A3EDE3_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 * L_0 = __this->get_address_of_m_Positions_0();
		bool L_1 = NativeArray_1_get_IsCreated_mA83E57BF410BDF5144D71C3C740F1BB04D255073((NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 *)L_0, /*hidden argument*/NativeArray_1_get_IsCreated_mA83E57BF410BDF5144D71C3C740F1BB04D255073_RuntimeMethod_var);
		if (!L_1)
		{
			goto IL_0018;
		}
	}
	{
		NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 * L_2 = __this->get_address_of_m_Positions_0();
		NativeArray_1_Dispose_m87C3176D32CFB61D46241114A0DAA158BA654620((NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 *)L_2, /*hidden argument*/NativeArray_1_Dispose_m87C3176D32CFB61D46241114A0DAA158BA654620_RuntimeMethod_var);
	}

IL_0018:
	{
		NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 * L_3 = __this->get_address_of_m_ConfidenceValues_1();
		bool L_4 = NativeArray_1_get_IsCreated_m871B990AAFB1ACE6BDFE2A895BA589B9E8C3769E((NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 *)L_3, /*hidden argument*/NativeArray_1_get_IsCreated_m871B990AAFB1ACE6BDFE2A895BA589B9E8C3769E_RuntimeMethod_var);
		if (!L_4)
		{
			goto IL_0030;
		}
	}
	{
		NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 * L_5 = __this->get_address_of_m_ConfidenceValues_1();
		NativeArray_1_Dispose_m0A2104A4244A4841BC56C79F09040160C5209941((NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 *)L_5, /*hidden argument*/NativeArray_1_Dispose_m0A2104A4244A4841BC56C79F09040160C5209941_RuntimeMethod_var);
	}

IL_0030:
	{
		NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C * L_6 = __this->get_address_of_m_Identifiers_2();
		bool L_7 = NativeArray_1_get_IsCreated_mC80A890C3A78C986492F0259406F63788CED59A1((NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C *)L_6, /*hidden argument*/NativeArray_1_get_IsCreated_mC80A890C3A78C986492F0259406F63788CED59A1_RuntimeMethod_var);
		if (!L_7)
		{
			goto IL_0048;
		}
	}
	{
		NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C * L_8 = __this->get_address_of_m_Identifiers_2();
		NativeArray_1_Dispose_m922795C73F226D113E9B538E2D95D7423522B868((NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C *)L_8, /*hidden argument*/NativeArray_1_Dispose_m922795C73F226D113E9B538E2D95D7423522B868_RuntimeMethod_var);
	}

IL_0048:
	{
		return;
	}
}
IL2CPP_EXTERN_C  void XRPointCloudData_Dispose_mDF78595F088472E60327A1D366AA787C68A3EDE3_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * _thisAdjusted = reinterpret_cast<XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA *>(__this + _offset);
	XRPointCloudData_Dispose_mDF78595F088472E60327A1D366AA787C68A3EDE3(_thisAdjusted, method);
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRPointCloudData::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRPointCloudData_GetHashCode_mA67A28CD8661AAE597D4466135AF1750D2569409 (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRPointCloudData_GetHashCode_mA67A28CD8661AAE597D4466135AF1750D2569409_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 * L_0 = __this->get_address_of_m_Positions_0();
		int32_t L_1 = NativeArray_1_GetHashCode_m8DD866CBD1A50C075DA392F6DFDE1B9B7767DAD3((NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 *)L_0, /*hidden argument*/NativeArray_1_GetHashCode_m8DD866CBD1A50C075DA392F6DFDE1B9B7767DAD3_RuntimeMethod_var);
		NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 * L_2 = __this->get_address_of_m_ConfidenceValues_1();
		int32_t L_3 = NativeArray_1_GetHashCode_m4A3A547F590E7BCA81A2099A6ECA91A4C9D595F1((NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 *)L_2, /*hidden argument*/NativeArray_1_GetHashCode_m4A3A547F590E7BCA81A2099A6ECA91A4C9D595F1_RuntimeMethod_var);
		NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C * L_4 = __this->get_address_of_m_Identifiers_2();
		int32_t L_5 = NativeArray_1_GetHashCode_m3C1068B41EC1F4520ED44D2FD0E3E44F304099E1((NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C *)L_4, /*hidden argument*/NativeArray_1_GetHashCode_m3C1068B41EC1F4520ED44D2FD0E3E44F304099E1_RuntimeMethod_var);
		return ((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)L_1, (int32_t)((int32_t)486187739))), (int32_t)L_3)), (int32_t)((int32_t)486187739))), (int32_t)L_5));
	}
}
IL2CPP_EXTERN_C  int32_t XRPointCloudData_GetHashCode_mA67A28CD8661AAE597D4466135AF1750D2569409_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * _thisAdjusted = reinterpret_cast<XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA *>(__this + _offset);
	return XRPointCloudData_GetHashCode_mA67A28CD8661AAE597D4466135AF1750D2569409(_thisAdjusted, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRPointCloudData::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRPointCloudData_Equals_mC870B135A9697D1A2AFB40892E70D7D403590E2A (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRPointCloudData_Equals_mC870B135A9697D1A2AFB40892E70D7D403590E2A_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject * L_0 = ___obj0;
		if (!((RuntimeObject *)IsInstSealed((RuntimeObject*)L_0, XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA_il2cpp_TypeInfo_var)))
		{
			goto IL_0015;
		}
	}
	{
		RuntimeObject * L_1 = ___obj0;
		bool L_2 = XRPointCloudData_Equals_mDE5097D689526E5461CBBC48C36E6221F71F7598((XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA *)__this, ((*(XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA *)((XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA *)UnBox(L_1, XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA_il2cpp_TypeInfo_var)))), /*hidden argument*/NULL);
		return L_2;
	}

IL_0015:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool XRPointCloudData_Equals_mC870B135A9697D1A2AFB40892E70D7D403590E2A_AdjustorThunk (RuntimeObject * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * _thisAdjusted = reinterpret_cast<XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA *>(__this + _offset);
	return XRPointCloudData_Equals_mC870B135A9697D1A2AFB40892E70D7D403590E2A(_thisAdjusted, ___obj0, method);
}
// System.String UnityEngine.XR.ARSubsystems.XRPointCloudData::ToString()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* XRPointCloudData_ToString_m3790E45AE87D2C6F63D664EF736F530B7A4FCB4D (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRPointCloudData_ToString_m3790E45AE87D2C6F63D664EF736F530B7A4FCB4D_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 * L_0 = __this->get_address_of_m_Positions_0();
		int32_t L_1 = IL2CPP_NATIVEARRAY_GET_LENGTH(((NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 *)L_0)->___m_Length_1);
		int32_t L_2 = L_1;
		RuntimeObject * L_3 = Box(Int32_t585191389E07734F19F3156FF88FB3EF4800D102_il2cpp_TypeInfo_var, &L_2);
		NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 * L_4 = __this->get_address_of_m_ConfidenceValues_1();
		int32_t L_5 = IL2CPP_NATIVEARRAY_GET_LENGTH(((NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 *)L_4)->___m_Length_1);
		int32_t L_6 = L_5;
		RuntimeObject * L_7 = Box(Int32_t585191389E07734F19F3156FF88FB3EF4800D102_il2cpp_TypeInfo_var, &L_6);
		NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C * L_8 = __this->get_address_of_m_Identifiers_2();
		int32_t L_9 = IL2CPP_NATIVEARRAY_GET_LENGTH(((NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C *)L_8)->___m_Length_1);
		int32_t L_10 = L_9;
		RuntimeObject * L_11 = Box(Int32_t585191389E07734F19F3156FF88FB3EF4800D102_il2cpp_TypeInfo_var, &L_10);
		String_t* L_12 = String_Format_m26BBF75F9609FAD0B39C2242FEBAAD7D68F14D99(_stringLiteralF3CD8D2D9D73EC0CE1DAD87B8E5780B970B7AAAF, L_3, L_7, L_11, /*hidden argument*/NULL);
		return L_12;
	}
}
IL2CPP_EXTERN_C  String_t* XRPointCloudData_ToString_m3790E45AE87D2C6F63D664EF736F530B7A4FCB4D_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * _thisAdjusted = reinterpret_cast<XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA *>(__this + _offset);
	return XRPointCloudData_ToString_m3790E45AE87D2C6F63D664EF736F530B7A4FCB4D(_thisAdjusted, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRPointCloudData::Equals(UnityEngine.XR.ARSubsystems.XRPointCloudData)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRPointCloudData_Equals_mDE5097D689526E5461CBBC48C36E6221F71F7598 (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA  ___other0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRPointCloudData_Equals_mDE5097D689526E5461CBBC48C36E6221F71F7598_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 * L_0 = __this->get_address_of_m_Positions_0();
		XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA  L_1 = ___other0;
		NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  L_2 = L_1.get_m_Positions_0();
		bool L_3 = NativeArray_1_Equals_m2151C41783B1650AC28A718289E8D4FC5DAA2D42((NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74 *)L_0, L_2, /*hidden argument*/NativeArray_1_Equals_m2151C41783B1650AC28A718289E8D4FC5DAA2D42_RuntimeMethod_var);
		if (!L_3)
		{
			goto IL_0038;
		}
	}
	{
		NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 * L_4 = __this->get_address_of_m_ConfidenceValues_1();
		XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA  L_5 = ___other0;
		NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  L_6 = L_5.get_m_ConfidenceValues_1();
		bool L_7 = NativeArray_1_Equals_mEC619E11BC3B323365C4E22C4493BE4F3FF49F27((NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67 *)L_4, L_6, /*hidden argument*/NativeArray_1_Equals_mEC619E11BC3B323365C4E22C4493BE4F3FF49F27_RuntimeMethod_var);
		if (!L_7)
		{
			goto IL_0038;
		}
	}
	{
		NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C * L_8 = __this->get_address_of_m_Identifiers_2();
		XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA  L_9 = ___other0;
		NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  L_10 = L_9.get_m_Identifiers_2();
		bool L_11 = NativeArray_1_Equals_m60C6EC22F3E5BF3FA454EE59E114A0E3610DCC41((NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C *)L_8, L_10, /*hidden argument*/NativeArray_1_Equals_m60C6EC22F3E5BF3FA454EE59E114A0E3610DCC41_RuntimeMethod_var);
		return L_11;
	}

IL_0038:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool XRPointCloudData_Equals_mDE5097D689526E5461CBBC48C36E6221F71F7598_AdjustorThunk (RuntimeObject * __this, XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA  ___other0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * _thisAdjusted = reinterpret_cast<XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA *>(__this + _offset);
	return XRPointCloudData_Equals_mDE5097D689526E5461CBBC48C36E6221F71F7598(_thisAdjusted, ___other0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRPointCloudData::op_Equality(UnityEngine.XR.ARSubsystems.XRPointCloudData,UnityEngine.XR.ARSubsystems.XRPointCloudData)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRPointCloudData_op_Equality_m96CE37D4B2F213B20AE0352C787DBE13BE339ECE (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA  ___lhs0, XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA  ___rhs1, const RuntimeMethod* method)
{
	{
		XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA  L_0 = ___rhs1;
		bool L_1 = XRPointCloudData_Equals_mDE5097D689526E5461CBBC48C36E6221F71F7598((XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return L_1;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRPointCloudData::op_Inequality(UnityEngine.XR.ARSubsystems.XRPointCloudData,UnityEngine.XR.ARSubsystems.XRPointCloudData)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRPointCloudData_op_Inequality_mB515ACB6DD311FB5AF3CA608B7078581AFEDD25B (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA  ___lhs0, XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA  ___rhs1, const RuntimeMethod* method)
{
	{
		XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA  L_0 = ___rhs1;
		bool L_1 = XRPointCloudData_Equals_mDE5097D689526E5461CBBC48C36E6221F71F7598((XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return (bool)((((int32_t)L_1) == ((int32_t)0))? 1 : 0);
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// UnityEngine.XR.ARSubsystems.XRRaycastHit UnityEngine.XR.ARSubsystems.XRRaycastHit::get_defaultValue()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  XRRaycastHit_get_defaultValue_m17AEBDAC971A56C3FC4C7C4E2E14ECC357658DFA (const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRRaycastHit_get_defaultValue_m17AEBDAC971A56C3FC4C7C4E2E14ECC357658DFA_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		IL2CPP_RUNTIME_CLASS_INIT(XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82_il2cpp_TypeInfo_var);
		XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  L_0 = ((XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82_StaticFields*)il2cpp_codegen_static_fields_for(XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82_il2cpp_TypeInfo_var))->get_s_Default_0();
		return L_0;
	}
}
// UnityEngine.XR.ARSubsystems.TrackableId UnityEngine.XR.ARSubsystems.XRRaycastHit::get_trackableId()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  XRRaycastHit_get_trackableId_mAECCB1BE08FB0B5A48CB27D955250FE2068492CF (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = __this->get_m_TrackableId_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C  TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  XRRaycastHit_get_trackableId_mAECCB1BE08FB0B5A48CB27D955250FE2068492CF_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * _thisAdjusted = reinterpret_cast<XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 *>(__this + _offset);
	return XRRaycastHit_get_trackableId_mAECCB1BE08FB0B5A48CB27D955250FE2068492CF_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastHit::set_trackableId(UnityEngine.XR.ARSubsystems.TrackableId)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRRaycastHit_set_trackableId_mD5381CB555237421AA3A1A4F42BDBA66C2CEE77F (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___value0, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = ___value0;
		__this->set_m_TrackableId_1(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void XRRaycastHit_set_trackableId_mD5381CB555237421AA3A1A4F42BDBA66C2CEE77F_AdjustorThunk (RuntimeObject * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * _thisAdjusted = reinterpret_cast<XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 *>(__this + _offset);
	XRRaycastHit_set_trackableId_mD5381CB555237421AA3A1A4F42BDBA66C2CEE77F_inline(_thisAdjusted, ___value0, method);
}
// UnityEngine.Pose UnityEngine.XR.ARSubsystems.XRRaycastHit::get_pose()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  XRRaycastHit_get_pose_mE0B0A754E818C6FF3675A41CA95185A3E608C8C3 (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, const RuntimeMethod* method)
{
	{
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_0 = __this->get_m_Pose_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C  Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  XRRaycastHit_get_pose_mE0B0A754E818C6FF3675A41CA95185A3E608C8C3_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * _thisAdjusted = reinterpret_cast<XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 *>(__this + _offset);
	return XRRaycastHit_get_pose_mE0B0A754E818C6FF3675A41CA95185A3E608C8C3_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastHit::set_pose(UnityEngine.Pose)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRRaycastHit_set_pose_m3E6F13DE1371303DD66CD9D9E8B86500C24C5516 (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___value0, const RuntimeMethod* method)
{
	{
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_0 = ___value0;
		__this->set_m_Pose_2(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void XRRaycastHit_set_pose_m3E6F13DE1371303DD66CD9D9E8B86500C24C5516_AdjustorThunk (RuntimeObject * __this, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * _thisAdjusted = reinterpret_cast<XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 *>(__this + _offset);
	XRRaycastHit_set_pose_m3E6F13DE1371303DD66CD9D9E8B86500C24C5516_inline(_thisAdjusted, ___value0, method);
}
// System.Single UnityEngine.XR.ARSubsystems.XRRaycastHit::get_distance()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR float XRRaycastHit_get_distance_mC748DE6ED96B0C735DCA4AD320FA0BF522246D19 (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, const RuntimeMethod* method)
{
	{
		float L_0 = __this->get_m_Distance_3();
		return L_0;
	}
}
IL2CPP_EXTERN_C  float XRRaycastHit_get_distance_mC748DE6ED96B0C735DCA4AD320FA0BF522246D19_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * _thisAdjusted = reinterpret_cast<XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 *>(__this + _offset);
	return XRRaycastHit_get_distance_mC748DE6ED96B0C735DCA4AD320FA0BF522246D19_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastHit::set_distance(System.Single)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRRaycastHit_set_distance_m53218D1A8CBD8F632F988C439D5F98633A050815 (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, float ___value0, const RuntimeMethod* method)
{
	{
		float L_0 = ___value0;
		__this->set_m_Distance_3(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void XRRaycastHit_set_distance_m53218D1A8CBD8F632F988C439D5F98633A050815_AdjustorThunk (RuntimeObject * __this, float ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * _thisAdjusted = reinterpret_cast<XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 *>(__this + _offset);
	XRRaycastHit_set_distance_m53218D1A8CBD8F632F988C439D5F98633A050815_inline(_thisAdjusted, ___value0, method);
}
// UnityEngine.XR.ARSubsystems.TrackableType UnityEngine.XR.ARSubsystems.XRRaycastHit::get_hitType()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRRaycastHit_get_hitType_m52BBF5DBDE1B3E7E01571EE029F68EB29E240DA6 (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_HitType_4();
		return L_0;
	}
}
IL2CPP_EXTERN_C  int32_t XRRaycastHit_get_hitType_m52BBF5DBDE1B3E7E01571EE029F68EB29E240DA6_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * _thisAdjusted = reinterpret_cast<XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 *>(__this + _offset);
	return XRRaycastHit_get_hitType_m52BBF5DBDE1B3E7E01571EE029F68EB29E240DA6_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastHit::set_hitType(UnityEngine.XR.ARSubsystems.TrackableType)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRRaycastHit_set_hitType_m776B39B9226EB310D47FB6A10BA78844AEC4EE58 (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, int32_t ___value0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___value0;
		__this->set_m_HitType_4(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void XRRaycastHit_set_hitType_m776B39B9226EB310D47FB6A10BA78844AEC4EE58_AdjustorThunk (RuntimeObject * __this, int32_t ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * _thisAdjusted = reinterpret_cast<XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 *>(__this + _offset);
	XRRaycastHit_set_hitType_m776B39B9226EB310D47FB6A10BA78844AEC4EE58_inline(_thisAdjusted, ___value0, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastHit::.ctor(UnityEngine.XR.ARSubsystems.TrackableId,UnityEngine.Pose,System.Single,UnityEngine.XR.ARSubsystems.TrackableType)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRRaycastHit__ctor_m522E98C4B6FD85F8386911C5BC497DE4968E3961 (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___trackableId0, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___pose1, float ___distance2, int32_t ___hitType3, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = ___trackableId0;
		__this->set_m_TrackableId_1(L_0);
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_1 = ___pose1;
		__this->set_m_Pose_2(L_1);
		float L_2 = ___distance2;
		__this->set_m_Distance_3(L_2);
		int32_t L_3 = ___hitType3;
		__this->set_m_HitType_4(L_3);
		return;
	}
}
IL2CPP_EXTERN_C  void XRRaycastHit__ctor_m522E98C4B6FD85F8386911C5BC497DE4968E3961_AdjustorThunk (RuntimeObject * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___trackableId0, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___pose1, float ___distance2, int32_t ___hitType3, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * _thisAdjusted = reinterpret_cast<XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 *>(__this + _offset);
	XRRaycastHit__ctor_m522E98C4B6FD85F8386911C5BC497DE4968E3961(_thisAdjusted, ___trackableId0, ___pose1, ___distance2, ___hitType3, method);
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRRaycastHit::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRRaycastHit_GetHashCode_m74F67C2F858CF9669399A90DC761E0E763C0827F (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, const RuntimeMethod* method)
{
	int32_t V_0 = 0;
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 * L_0 = __this->get_address_of_m_TrackableId_1();
		int32_t L_1 = TrackableId_GetHashCode_m89E7236D11700A1FAF335918CA65CDEB1BF4D973((TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 *)L_0, /*hidden argument*/NULL);
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 * L_2 = __this->get_address_of_m_Pose_2();
		int32_t L_3 = Pose_GetHashCode_m17AC0D28F5BD43DE0CCFA4CC1A870C525E0D6066((Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 *)L_2, /*hidden argument*/NULL);
		float* L_4 = __this->get_address_of_m_Distance_3();
		int32_t L_5 = Single_GetHashCode_m1BC0733E0C3851ED9D1B6C9C0B243BB88BE77AD0((float*)L_4, /*hidden argument*/NULL);
		int32_t L_6 = __this->get_m_HitType_4();
		V_0 = L_6;
		int32_t L_7 = Int32_GetHashCode_m245C424ECE351E5FE3277A88EEB02132DAB8C25A((int32_t*)(&V_0), /*hidden argument*/NULL);
		return ((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)L_1, (int32_t)((int32_t)486187739))), (int32_t)L_3)), (int32_t)((int32_t)486187739))), (int32_t)L_5)), (int32_t)((int32_t)486187739))), (int32_t)L_7));
	}
}
IL2CPP_EXTERN_C  int32_t XRRaycastHit_GetHashCode_m74F67C2F858CF9669399A90DC761E0E763C0827F_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * _thisAdjusted = reinterpret_cast<XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 *>(__this + _offset);
	return XRRaycastHit_GetHashCode_m74F67C2F858CF9669399A90DC761E0E763C0827F(_thisAdjusted, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastHit::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRRaycastHit_Equals_m0A24B5C58B6CA930CDD05F6F17F54FD60DA10DE5 (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRRaycastHit_Equals_m0A24B5C58B6CA930CDD05F6F17F54FD60DA10DE5_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject * L_0 = ___obj0;
		if (((RuntimeObject *)IsInstSealed((RuntimeObject*)L_0, XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82_il2cpp_TypeInfo_var)))
		{
			goto IL_000a;
		}
	}
	{
		return (bool)0;
	}

IL_000a:
	{
		RuntimeObject * L_1 = ___obj0;
		bool L_2 = XRRaycastHit_Equals_m2E3F746F63AC5ED95DF5E79AB43C2DE8A8E42E60((XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 *)__this, ((*(XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 *)((XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 *)UnBox(L_1, XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82_il2cpp_TypeInfo_var)))), /*hidden argument*/NULL);
		return L_2;
	}
}
IL2CPP_EXTERN_C  bool XRRaycastHit_Equals_m0A24B5C58B6CA930CDD05F6F17F54FD60DA10DE5_AdjustorThunk (RuntimeObject * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * _thisAdjusted = reinterpret_cast<XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 *>(__this + _offset);
	return XRRaycastHit_Equals_m0A24B5C58B6CA930CDD05F6F17F54FD60DA10DE5(_thisAdjusted, ___obj0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastHit::Equals(UnityEngine.XR.ARSubsystems.XRRaycastHit)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRRaycastHit_Equals_m2E3F746F63AC5ED95DF5E79AB43C2DE8A8E42E60 (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  ___other0, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 * L_0 = __this->get_address_of_m_TrackableId_1();
		XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  L_1 = ___other0;
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_2 = L_1.get_m_TrackableId_1();
		bool L_3 = TrackableId_Equals_mCE458E0FDCDD6E339FCC1926EE88EB7B3D45F943((TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 *)L_0, L_2, /*hidden argument*/NULL);
		if (!L_3)
		{
			goto IL_0048;
		}
	}
	{
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 * L_4 = __this->get_address_of_m_Pose_2();
		XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  L_5 = ___other0;
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_6 = L_5.get_m_Pose_2();
		bool L_7 = Pose_Equals_m867264C8DF91FF8DC3AD957EF1625902CDEBAEDD((Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 *)L_4, L_6, /*hidden argument*/NULL);
		if (!L_7)
		{
			goto IL_0048;
		}
	}
	{
		float* L_8 = __this->get_address_of_m_Distance_3();
		XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  L_9 = ___other0;
		float L_10 = L_9.get_m_Distance_3();
		bool L_11 = Single_Equals_mCDFA927E712FBA83D076864E16C77E39A6E66FE7((float*)L_8, L_10, /*hidden argument*/NULL);
		if (!L_11)
		{
			goto IL_0048;
		}
	}
	{
		int32_t L_12 = __this->get_m_HitType_4();
		XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  L_13 = ___other0;
		int32_t L_14 = L_13.get_m_HitType_4();
		return (bool)((((int32_t)L_12) == ((int32_t)L_14))? 1 : 0);
	}

IL_0048:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool XRRaycastHit_Equals_m2E3F746F63AC5ED95DF5E79AB43C2DE8A8E42E60_AdjustorThunk (RuntimeObject * __this, XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  ___other0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * _thisAdjusted = reinterpret_cast<XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 *>(__this + _offset);
	return XRRaycastHit_Equals_m2E3F746F63AC5ED95DF5E79AB43C2DE8A8E42E60(_thisAdjusted, ___other0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastHit::op_Equality(UnityEngine.XR.ARSubsystems.XRRaycastHit,UnityEngine.XR.ARSubsystems.XRRaycastHit)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRRaycastHit_op_Equality_m1543C9C16776653F7665569024203E1F54305FC7 (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  ___lhs0, XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  ___rhs1, const RuntimeMethod* method)
{
	{
		XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  L_0 = ___rhs1;
		bool L_1 = XRRaycastHit_Equals_m2E3F746F63AC5ED95DF5E79AB43C2DE8A8E42E60((XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return L_1;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastHit::op_Inequality(UnityEngine.XR.ARSubsystems.XRRaycastHit,UnityEngine.XR.ARSubsystems.XRRaycastHit)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRRaycastHit_op_Inequality_m5533730BFA8AD45F27384E867981EF82A0AB0862 (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  ___lhs0, XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  ___rhs1, const RuntimeMethod* method)
{
	{
		XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  L_0 = ___rhs1;
		bool L_1 = XRRaycastHit_Equals_m2E3F746F63AC5ED95DF5E79AB43C2DE8A8E42E60((XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return (bool)((((int32_t)L_1) == ((int32_t)0))? 1 : 0);
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastHit::.cctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRRaycastHit__cctor_m304374EB65F3AE9EFA5D8418B9CF3CE8A90B752B (const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRRaycastHit__cctor_m304374EB65F3AE9EFA5D8418B9CF3CE8A90B752B_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		IL2CPP_RUNTIME_CLASS_INIT(TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47_il2cpp_TypeInfo_var);
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = TrackableId_get_invalidId_mBE9FA1EC8F2EC1575C1B31666EA928A3382DF1CD_inline(/*hidden argument*/NULL);
		IL2CPP_RUNTIME_CLASS_INIT(Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29_il2cpp_TypeInfo_var);
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_1 = Pose_get_identity_m19458DF84EAADD5E9302CABDC385B97DC91ECCBF(/*hidden argument*/NULL);
		XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  L_2;
		memset((&L_2), 0, sizeof(L_2));
		XRRaycastHit__ctor_m522E98C4B6FD85F8386911C5BC497DE4968E3961((&L_2), L_0, L_1, (0.0f), 0, /*hidden argument*/NULL);
		((XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82_StaticFields*)il2cpp_codegen_static_fields_for(XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82_il2cpp_TypeInfo_var))->set_s_Default_0(L_2);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystem::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRRaycastSubsystem__ctor_mE8BD2BFB3AFD44403F3A663CA5D5AAC707419506 (XRRaycastSubsystem_t1181EA314910ABB4E1F50BF2F7650EC1512A0A20 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRRaycastSubsystem__ctor_mE8BD2BFB3AFD44403F3A663CA5D5AAC707419506_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		XRSubsystem_1__ctor_m9E32FFF260FC17D2C65B47C162DD6B914C8867FB(__this, /*hidden argument*/XRSubsystem_1__ctor_m9E32FFF260FC17D2C65B47C162DD6B914C8867FB_RuntimeMethod_var);
		Provider_t1231BE7466AD9536EA5EB62866FF0B6F07452732 * L_0 = VirtFuncInvoker0< Provider_t1231BE7466AD9536EA5EB62866FF0B6F07452732 * >::Invoke(14 /* UnityEngine.XR.ARSubsystems.XRRaycastSubsystem/Provider UnityEngine.XR.ARSubsystems.XRRaycastSubsystem::CreateProvider() */, __this);
		__this->set_m_Provider_3(L_0);
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystem::OnStart()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRRaycastSubsystem_OnStart_m7B2A704BD7A5EA9FAF5E80D67C83AC90A2B7E0E7 (XRRaycastSubsystem_t1181EA314910ABB4E1F50BF2F7650EC1512A0A20 * __this, const RuntimeMethod* method)
{
	{
		Provider_t1231BE7466AD9536EA5EB62866FF0B6F07452732 * L_0 = __this->get_m_Provider_3();
		NullCheck(L_0);
		VirtActionInvoker0::Invoke(4 /* System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystem/Provider::Start() */, L_0);
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystem::OnStop()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRRaycastSubsystem_OnStop_mB922B7F9B65FF7EC41D3042D161476F5E491573F (XRRaycastSubsystem_t1181EA314910ABB4E1F50BF2F7650EC1512A0A20 * __this, const RuntimeMethod* method)
{
	{
		Provider_t1231BE7466AD9536EA5EB62866FF0B6F07452732 * L_0 = __this->get_m_Provider_3();
		NullCheck(L_0);
		VirtActionInvoker0::Invoke(5 /* System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystem/Provider::Stop() */, L_0);
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystem::OnDestroyed()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRRaycastSubsystem_OnDestroyed_mD476DFCB51E552D3BB2C841CAB96B532D9D77BCC (XRRaycastSubsystem_t1181EA314910ABB4E1F50BF2F7650EC1512A0A20 * __this, const RuntimeMethod* method)
{
	{
		Provider_t1231BE7466AD9536EA5EB62866FF0B6F07452732 * L_0 = __this->get_m_Provider_3();
		NullCheck(L_0);
		VirtActionInvoker0::Invoke(6 /* System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystem/Provider::Destroy() */, L_0);
		return;
	}
}
// Unity.Collections.NativeArray`1<UnityEngine.XR.ARSubsystems.XRRaycastHit> UnityEngine.XR.ARSubsystems.XRRaycastSubsystem::Raycast(UnityEngine.Ray,UnityEngine.XR.ARSubsystems.TrackableType,Unity.Collections.Allocator)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR NativeArray_1_t769CF3061467D3B5B0062090193576AD726411C1  XRRaycastSubsystem_Raycast_mD6335AB75E7AD15295138215F593EAB71754E6FA (XRRaycastSubsystem_t1181EA314910ABB4E1F50BF2F7650EC1512A0A20 * __this, Ray_tE2163D4CB3E6B267E29F8ABE41684490E4A614B2  ___ray0, int32_t ___trackableTypeMask1, int32_t ___allocator2, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRRaycastSubsystem_Raycast_mD6335AB75E7AD15295138215F593EAB71754E6FA_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		Provider_t1231BE7466AD9536EA5EB62866FF0B6F07452732 * L_0 = __this->get_m_Provider_3();
		IL2CPP_RUNTIME_CLASS_INIT(XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82_il2cpp_TypeInfo_var);
		XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  L_1 = XRRaycastHit_get_defaultValue_m17AEBDAC971A56C3FC4C7C4E2E14ECC357658DFA_inline(/*hidden argument*/NULL);
		Ray_tE2163D4CB3E6B267E29F8ABE41684490E4A614B2  L_2 = ___ray0;
		int32_t L_3 = ___trackableTypeMask1;
		int32_t L_4 = ___allocator2;
		NullCheck(L_0);
		NativeArray_1_t769CF3061467D3B5B0062090193576AD726411C1  L_5 = VirtFuncInvoker4< NativeArray_1_t769CF3061467D3B5B0062090193576AD726411C1 , XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 , Ray_tE2163D4CB3E6B267E29F8ABE41684490E4A614B2 , int32_t, int32_t >::Invoke(7 /* Unity.Collections.NativeArray`1<UnityEngine.XR.ARSubsystems.XRRaycastHit> UnityEngine.XR.ARSubsystems.XRRaycastSubsystem/Provider::Raycast(UnityEngine.XR.ARSubsystems.XRRaycastHit,UnityEngine.Ray,UnityEngine.XR.ARSubsystems.TrackableType,Unity.Collections.Allocator) */, L_0, L_1, L_2, L_3, L_4);
		return L_5;
	}
}
// Unity.Collections.NativeArray`1<UnityEngine.XR.ARSubsystems.XRRaycastHit> UnityEngine.XR.ARSubsystems.XRRaycastSubsystem::Raycast(UnityEngine.Vector2,UnityEngine.XR.ARSubsystems.TrackableType,Unity.Collections.Allocator)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR NativeArray_1_t769CF3061467D3B5B0062090193576AD726411C1  XRRaycastSubsystem_Raycast_m46598C4ACA7D6AC6B6DA53A92ED1349F327EC6BF (XRRaycastSubsystem_t1181EA314910ABB4E1F50BF2F7650EC1512A0A20 * __this, Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  ___screenPoint0, int32_t ___trackableTypeMask1, int32_t ___allocator2, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRRaycastSubsystem_Raycast_m46598C4ACA7D6AC6B6DA53A92ED1349F327EC6BF_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		Provider_t1231BE7466AD9536EA5EB62866FF0B6F07452732 * L_0 = __this->get_m_Provider_3();
		IL2CPP_RUNTIME_CLASS_INIT(XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82_il2cpp_TypeInfo_var);
		XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  L_1 = XRRaycastHit_get_defaultValue_m17AEBDAC971A56C3FC4C7C4E2E14ECC357658DFA_inline(/*hidden argument*/NULL);
		Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  L_2 = ___screenPoint0;
		int32_t L_3 = ___trackableTypeMask1;
		int32_t L_4 = ___allocator2;
		NullCheck(L_0);
		NativeArray_1_t769CF3061467D3B5B0062090193576AD726411C1  L_5 = VirtFuncInvoker4< NativeArray_1_t769CF3061467D3B5B0062090193576AD726411C1 , XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 , Vector2_tA85D2DD88578276CA8A8796756458277E72D073D , int32_t, int32_t >::Invoke(8 /* Unity.Collections.NativeArray`1<UnityEngine.XR.ARSubsystems.XRRaycastHit> UnityEngine.XR.ARSubsystems.XRRaycastSubsystem/Provider::Raycast(UnityEngine.XR.ARSubsystems.XRRaycastHit,UnityEngine.Vector2,UnityEngine.XR.ARSubsystems.TrackableType,Unity.Collections.Allocator) */, L_0, L_1, L_2, L_3, L_4);
		return L_5;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystem_Provider::Start()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Provider_Start_mA565B894F858E9176BD59D05BCA2656E89E68184 (Provider_t1231BE7466AD9536EA5EB62866FF0B6F07452732 * __this, const RuntimeMethod* method)
{
	{
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystem_Provider::Stop()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Provider_Stop_mADDEF920450D92ACE94E849F5179B62487BEABE2 (Provider_t1231BE7466AD9536EA5EB62866FF0B6F07452732 * __this, const RuntimeMethod* method)
{
	{
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystem_Provider::Destroy()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Provider_Destroy_mF35AF8F2635ED60E06B102E0AB210D350D0A77D3 (Provider_t1231BE7466AD9536EA5EB62866FF0B6F07452732 * __this, const RuntimeMethod* method)
{
	{
		return;
	}
}
// Unity.Collections.NativeArray`1<UnityEngine.XR.ARSubsystems.XRRaycastHit> UnityEngine.XR.ARSubsystems.XRRaycastSubsystem_Provider::Raycast(UnityEngine.XR.ARSubsystems.XRRaycastHit,UnityEngine.Ray,UnityEngine.XR.ARSubsystems.TrackableType,Unity.Collections.Allocator)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR NativeArray_1_t769CF3061467D3B5B0062090193576AD726411C1  Provider_Raycast_m6063A859AC10ACF7F27A4AE0AAD82C3A0DF831AE (Provider_t1231BE7466AD9536EA5EB62866FF0B6F07452732 * __this, XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  ___defaultRaycastHit0, Ray_tE2163D4CB3E6B267E29F8ABE41684490E4A614B2  ___ray1, int32_t ___trackableTypeMask2, int32_t ___allocator3, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (Provider_Raycast_m6063A859AC10ACF7F27A4AE0AAD82C3A0DF831AE_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 * L_0 = (NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 *)il2cpp_codegen_object_new(NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010_il2cpp_TypeInfo_var);
		NotSupportedException__ctor_mD023A89A5C1F740F43F0A9CD6C49DC21230B3CEE(L_0, _stringLiteral40EB8A6D17A1FA9CEBAFE1F18BB8257C8CE3E703, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_0, Provider_Raycast_m6063A859AC10ACF7F27A4AE0AAD82C3A0DF831AE_RuntimeMethod_var);
	}
}
// Unity.Collections.NativeArray`1<UnityEngine.XR.ARSubsystems.XRRaycastHit> UnityEngine.XR.ARSubsystems.XRRaycastSubsystem_Provider::Raycast(UnityEngine.XR.ARSubsystems.XRRaycastHit,UnityEngine.Vector2,UnityEngine.XR.ARSubsystems.TrackableType,Unity.Collections.Allocator)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR NativeArray_1_t769CF3061467D3B5B0062090193576AD726411C1  Provider_Raycast_mF885EFF5FFD12196F53C9FE405EC523020C35CE2 (Provider_t1231BE7466AD9536EA5EB62866FF0B6F07452732 * __this, XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  ___defaultRaycastHit0, Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  ___screenPoint1, int32_t ___trackableTypeMask2, int32_t ___allocator3, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (Provider_Raycast_mF885EFF5FFD12196F53C9FE405EC523020C35CE2_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 * L_0 = (NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 *)il2cpp_codegen_object_new(NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010_il2cpp_TypeInfo_var);
		NotSupportedException__ctor_mD023A89A5C1F740F43F0A9CD6C49DC21230B3CEE(L_0, _stringLiteralC94A2BBDF45E627BA76EC101C53EBB78511EF25A, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_0, Provider_Raycast_mF885EFF5FFD12196F53C9FE405EC523020C35CE2_RuntimeMethod_var);
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystem_Provider::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Provider__ctor_m1A46FEF4E5C93F21E2C802E59B99507E79C0F2C1 (Provider_t1231BE7466AD9536EA5EB62866FF0B6F07452732 * __this, const RuntimeMethod* method)
{
	{
		Object__ctor_m925ECA5E85CA100E3FB86A4F9E15C120E9A184C0(__this, /*hidden argument*/NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor::get_supportsViewportBasedRaycast()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRRaycastSubsystemDescriptor_get_supportsViewportBasedRaycast_mD71BE0D71A6B3B48DDAB480114F930293C40DF26 (XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7 * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = __this->get_U3CsupportsViewportBasedRaycastU3Ek__BackingField_2();
		return L_0;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor::set_supportsViewportBasedRaycast(System.Boolean)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRRaycastSubsystemDescriptor_set_supportsViewportBasedRaycast_m7A6EBE60F40966D0314C378B1441C7DB41C0720D (XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7 * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsViewportBasedRaycastU3Ek__BackingField_2(L_0);
		return;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor::get_supportsWorldBasedRaycast()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRRaycastSubsystemDescriptor_get_supportsWorldBasedRaycast_m7CDCA8DFD75903B7169A59254A31EBCB1D1962BD (XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7 * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = __this->get_U3CsupportsWorldBasedRaycastU3Ek__BackingField_3();
		return L_0;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor::set_supportsWorldBasedRaycast(System.Boolean)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRRaycastSubsystemDescriptor_set_supportsWorldBasedRaycast_m23E91C1C3684B0FE2AF37D3BE2A79B0D88BFC7B3 (XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7 * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsWorldBasedRaycastU3Ek__BackingField_3(L_0);
		return;
	}
}
// UnityEngine.XR.ARSubsystems.TrackableType UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor::get_supportedTrackableTypes()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRRaycastSubsystemDescriptor_get_supportedTrackableTypes_m02F17127CFA033A9D6D84C7F0D53D0BA3FE379C4 (XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7 * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_U3CsupportedTrackableTypesU3Ek__BackingField_4();
		return L_0;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor::set_supportedTrackableTypes(UnityEngine.XR.ARSubsystems.TrackableType)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRRaycastSubsystemDescriptor_set_supportedTrackableTypes_mA429421E574C9261CFC271AC43A521E43B990DCD (XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7 * __this, int32_t ___value0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___value0;
		__this->set_U3CsupportedTrackableTypesU3Ek__BackingField_4(L_0);
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor::RegisterDescriptor(UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRRaycastSubsystemDescriptor_RegisterDescriptor_mFA32B9879B902AA46943CF8809094299062A41DB (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704  ___cinfo0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRRaycastSubsystemDescriptor_RegisterDescriptor_mFA32B9879B902AA46943CF8809094299062A41DB_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704  L_0 = ___cinfo0;
		XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7 * L_1 = (XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7 *)il2cpp_codegen_object_new(XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7_il2cpp_TypeInfo_var);
		XRRaycastSubsystemDescriptor__ctor_m449AFD6137C639FB21F17C456B20BC954875BC9E(L_1, L_0, /*hidden argument*/NULL);
		IL2CPP_RUNTIME_CLASS_INIT(SubsystemRegistration_t0A22FECC46483ABBFFC039449407F73FF11F5A1A_il2cpp_TypeInfo_var);
		SubsystemRegistration_CreateDescriptor_m90E54AA3B7A6EC958B51A4775DC75DA4149F9DF2(L_1, /*hidden argument*/NULL);
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor::.ctor(UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRRaycastSubsystemDescriptor__ctor_m449AFD6137C639FB21F17C456B20BC954875BC9E (XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7 * __this, Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704  ___cinfo0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRRaycastSubsystemDescriptor__ctor_m449AFD6137C639FB21F17C456B20BC954875BC9E_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		SubsystemDescriptor_1__ctor_mCE93F7D6739E1AADE5429632560447F18FA623F6(__this, /*hidden argument*/SubsystemDescriptor_1__ctor_mCE93F7D6739E1AADE5429632560447F18FA623F6_RuntimeMethod_var);
		String_t* L_0 = Cinfo_get_id_m0418C70EB8FBC60BB9B83053AA5175AEFE31CAF8_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)(&___cinfo0), /*hidden argument*/NULL);
		SubsystemDescriptor_set_id_m3289D5DA5921130F0338C6332907E686E36FF064_inline(__this, L_0, /*hidden argument*/NULL);
		Type_t * L_1 = Cinfo_get_subsystemImplementationType_m0E0FEE226FD08939BF83B7B2644BEC6362BA157B_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)(&___cinfo0), /*hidden argument*/NULL);
		SubsystemDescriptor_set_subsystemImplementationType_m727AB6BED84E57A382B8804F95A75A8012963EE1_inline(__this, L_1, /*hidden argument*/NULL);
		bool L_2 = Cinfo_get_supportsViewportBasedRaycast_m323FE06DA2E4222E6BF4CE89541DB4630CB254B3_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)(&___cinfo0), /*hidden argument*/NULL);
		XRRaycastSubsystemDescriptor_set_supportsViewportBasedRaycast_m7A6EBE60F40966D0314C378B1441C7DB41C0720D_inline(__this, L_2, /*hidden argument*/NULL);
		bool L_3 = Cinfo_get_supportsWorldBasedRaycast_mFBB112B068EE22D519CAC45E35255D6FDACCAE74_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)(&___cinfo0), /*hidden argument*/NULL);
		XRRaycastSubsystemDescriptor_set_supportsWorldBasedRaycast_m23E91C1C3684B0FE2AF37D3BE2A79B0D88BFC7B3_inline(__this, L_3, /*hidden argument*/NULL);
		int32_t L_4 = Cinfo_get_supportedTrackableTypes_mCC57E28DFCE93ECA772B1DE2E3E49AD030D79424_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)(&___cinfo0), /*hidden argument*/NULL);
		XRRaycastSubsystemDescriptor_set_supportedTrackableTypes_mA429421E574C9261CFC271AC43A521E43B990DCD_inline(__this, L_4, /*hidden argument*/NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Conversion methods for marshalling of: UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo
IL2CPP_EXTERN_C void Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704_marshal_pinvoke(const Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704& unmarshaled, Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704_marshaled_pinvoke& marshaled)
{
	Exception_t* ___U3CsubsystemImplementationTypeU3Ek__BackingField_1Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field '<subsystemImplementationType>k__BackingField' of type 'Cinfo': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___U3CsubsystemImplementationTypeU3Ek__BackingField_1Exception, NULL);
}
IL2CPP_EXTERN_C void Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704_marshal_pinvoke_back(const Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704_marshaled_pinvoke& marshaled, Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704& unmarshaled)
{
	Exception_t* ___U3CsubsystemImplementationTypeU3Ek__BackingField_1Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field '<subsystemImplementationType>k__BackingField' of type 'Cinfo': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___U3CsubsystemImplementationTypeU3Ek__BackingField_1Exception, NULL);
}
// Conversion method for clean up from marshalling of: UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo
IL2CPP_EXTERN_C void Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704_marshal_pinvoke_cleanup(Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704_marshaled_pinvoke& marshaled)
{
}
// Conversion methods for marshalling of: UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo
IL2CPP_EXTERN_C void Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704_marshal_com(const Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704& unmarshaled, Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704_marshaled_com& marshaled)
{
	Exception_t* ___U3CsubsystemImplementationTypeU3Ek__BackingField_1Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field '<subsystemImplementationType>k__BackingField' of type 'Cinfo': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___U3CsubsystemImplementationTypeU3Ek__BackingField_1Exception, NULL);
}
IL2CPP_EXTERN_C void Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704_marshal_com_back(const Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704_marshaled_com& marshaled, Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704& unmarshaled)
{
	Exception_t* ___U3CsubsystemImplementationTypeU3Ek__BackingField_1Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field '<subsystemImplementationType>k__BackingField' of type 'Cinfo': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___U3CsubsystemImplementationTypeU3Ek__BackingField_1Exception, NULL);
}
// Conversion method for clean up from marshalling of: UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor/Cinfo
IL2CPP_EXTERN_C void Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704_marshal_com_cleanup(Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704_marshaled_com& marshaled)
{
}
// System.String UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::get_id()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Cinfo_get_id_m0418C70EB8FBC60BB9B83053AA5175AEFE31CAF8 (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, const RuntimeMethod* method)
{
	{
		String_t* L_0 = __this->get_U3CidU3Ek__BackingField_0();
		return L_0;
	}
}
IL2CPP_EXTERN_C  String_t* Cinfo_get_id_m0418C70EB8FBC60BB9B83053AA5175AEFE31CAF8_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * _thisAdjusted = reinterpret_cast<Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *>(__this + _offset);
	return Cinfo_get_id_m0418C70EB8FBC60BB9B83053AA5175AEFE31CAF8_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::set_id(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Cinfo_set_id_mFFFBB447D6D0DF4A27428D414FE23BCCB16D78D1 (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, String_t* ___value0, const RuntimeMethod* method)
{
	{
		String_t* L_0 = ___value0;
		__this->set_U3CidU3Ek__BackingField_0(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void Cinfo_set_id_mFFFBB447D6D0DF4A27428D414FE23BCCB16D78D1_AdjustorThunk (RuntimeObject * __this, String_t* ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * _thisAdjusted = reinterpret_cast<Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *>(__this + _offset);
	Cinfo_set_id_mFFFBB447D6D0DF4A27428D414FE23BCCB16D78D1_inline(_thisAdjusted, ___value0, method);
}
// System.Type UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::get_subsystemImplementationType()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Type_t * Cinfo_get_subsystemImplementationType_m0E0FEE226FD08939BF83B7B2644BEC6362BA157B (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, const RuntimeMethod* method)
{
	{
		Type_t * L_0 = __this->get_U3CsubsystemImplementationTypeU3Ek__BackingField_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C  Type_t * Cinfo_get_subsystemImplementationType_m0E0FEE226FD08939BF83B7B2644BEC6362BA157B_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * _thisAdjusted = reinterpret_cast<Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *>(__this + _offset);
	return Cinfo_get_subsystemImplementationType_m0E0FEE226FD08939BF83B7B2644BEC6362BA157B_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::set_subsystemImplementationType(System.Type)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Cinfo_set_subsystemImplementationType_m00E3D77F5C33C670222169A514C9804886D4FD72 (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, Type_t * ___value0, const RuntimeMethod* method)
{
	{
		Type_t * L_0 = ___value0;
		__this->set_U3CsubsystemImplementationTypeU3Ek__BackingField_1(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void Cinfo_set_subsystemImplementationType_m00E3D77F5C33C670222169A514C9804886D4FD72_AdjustorThunk (RuntimeObject * __this, Type_t * ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * _thisAdjusted = reinterpret_cast<Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *>(__this + _offset);
	Cinfo_set_subsystemImplementationType_m00E3D77F5C33C670222169A514C9804886D4FD72_inline(_thisAdjusted, ___value0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::get_supportsViewportBasedRaycast()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_get_supportsViewportBasedRaycast_m323FE06DA2E4222E6BF4CE89541DB4630CB254B3 (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = __this->get_U3CsupportsViewportBasedRaycastU3Ek__BackingField_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C  bool Cinfo_get_supportsViewportBasedRaycast_m323FE06DA2E4222E6BF4CE89541DB4630CB254B3_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * _thisAdjusted = reinterpret_cast<Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *>(__this + _offset);
	return Cinfo_get_supportsViewportBasedRaycast_m323FE06DA2E4222E6BF4CE89541DB4630CB254B3_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::set_supportsViewportBasedRaycast(System.Boolean)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Cinfo_set_supportsViewportBasedRaycast_mBA63D727FCB8E5FE2EF1BD5CA2192B54768DEF0D (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsViewportBasedRaycastU3Ek__BackingField_2(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void Cinfo_set_supportsViewportBasedRaycast_mBA63D727FCB8E5FE2EF1BD5CA2192B54768DEF0D_AdjustorThunk (RuntimeObject * __this, bool ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * _thisAdjusted = reinterpret_cast<Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *>(__this + _offset);
	Cinfo_set_supportsViewportBasedRaycast_mBA63D727FCB8E5FE2EF1BD5CA2192B54768DEF0D_inline(_thisAdjusted, ___value0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::get_supportsWorldBasedRaycast()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_get_supportsWorldBasedRaycast_mFBB112B068EE22D519CAC45E35255D6FDACCAE74 (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = __this->get_U3CsupportsWorldBasedRaycastU3Ek__BackingField_3();
		return L_0;
	}
}
IL2CPP_EXTERN_C  bool Cinfo_get_supportsWorldBasedRaycast_mFBB112B068EE22D519CAC45E35255D6FDACCAE74_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * _thisAdjusted = reinterpret_cast<Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *>(__this + _offset);
	return Cinfo_get_supportsWorldBasedRaycast_mFBB112B068EE22D519CAC45E35255D6FDACCAE74_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::set_supportsWorldBasedRaycast(System.Boolean)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Cinfo_set_supportsWorldBasedRaycast_mDDA30FB5ADF2800F44467B727F19668AAE05AFEF (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsWorldBasedRaycastU3Ek__BackingField_3(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void Cinfo_set_supportsWorldBasedRaycast_mDDA30FB5ADF2800F44467B727F19668AAE05AFEF_AdjustorThunk (RuntimeObject * __this, bool ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * _thisAdjusted = reinterpret_cast<Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *>(__this + _offset);
	Cinfo_set_supportsWorldBasedRaycast_mDDA30FB5ADF2800F44467B727F19668AAE05AFEF_inline(_thisAdjusted, ___value0, method);
}
// UnityEngine.XR.ARSubsystems.TrackableType UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::get_supportedTrackableTypes()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Cinfo_get_supportedTrackableTypes_mCC57E28DFCE93ECA772B1DE2E3E49AD030D79424 (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_U3CsupportedTrackableTypesU3Ek__BackingField_4();
		return L_0;
	}
}
IL2CPP_EXTERN_C  int32_t Cinfo_get_supportedTrackableTypes_mCC57E28DFCE93ECA772B1DE2E3E49AD030D79424_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * _thisAdjusted = reinterpret_cast<Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *>(__this + _offset);
	return Cinfo_get_supportedTrackableTypes_mCC57E28DFCE93ECA772B1DE2E3E49AD030D79424_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::set_supportedTrackableTypes(UnityEngine.XR.ARSubsystems.TrackableType)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Cinfo_set_supportedTrackableTypes_m91638341F04B3BC3688660AFFE66308C19B13C6B (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, int32_t ___value0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___value0;
		__this->set_U3CsupportedTrackableTypesU3Ek__BackingField_4(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void Cinfo_set_supportedTrackableTypes_m91638341F04B3BC3688660AFFE66308C19B13C6B_AdjustorThunk (RuntimeObject * __this, int32_t ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * _thisAdjusted = reinterpret_cast<Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *>(__this + _offset);
	Cinfo_set_supportedTrackableTypes_m91638341F04B3BC3688660AFFE66308C19B13C6B_inline(_thisAdjusted, ___value0, method);
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Cinfo_GetHashCode_mCC56E718130099F9809415C6C1CE9DC981F06211 (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (Cinfo_GetHashCode_mCC56E718130099F9809415C6C1CE9DC981F06211_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	bool V_0 = false;
	int32_t V_1 = 0;
	int32_t G_B3_0 = 0;
	int32_t G_B5_0 = 0;
	int32_t G_B4_0 = 0;
	int32_t G_B6_0 = 0;
	int32_t G_B6_1 = 0;
	{
		String_t* L_0 = Cinfo_get_id_m0418C70EB8FBC60BB9B83053AA5175AEFE31CAF8_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)__this, /*hidden argument*/NULL);
		if (!L_0)
		{
			goto IL_0015;
		}
	}
	{
		String_t* L_1 = Cinfo_get_id_m0418C70EB8FBC60BB9B83053AA5175AEFE31CAF8_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)__this, /*hidden argument*/NULL);
		NullCheck(L_1);
		int32_t L_2 = VirtFuncInvoker0< int32_t >::Invoke(2 /* System.Int32 System.Object::GetHashCode() */, L_1);
		G_B3_0 = L_2;
		goto IL_0016;
	}

IL_0015:
	{
		G_B3_0 = 0;
	}

IL_0016:
	{
		Type_t * L_3 = Cinfo_get_subsystemImplementationType_m0E0FEE226FD08939BF83B7B2644BEC6362BA157B_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)__this, /*hidden argument*/NULL);
		IL2CPP_RUNTIME_CLASS_INIT(Type_t_il2cpp_TypeInfo_var);
		bool L_4 = Type_op_Equality_m7040622C9E1037EFC73E1F0EDB1DD241282BE3D8(L_3, (Type_t *)NULL, /*hidden argument*/NULL);
		G_B4_0 = ((int32_t)il2cpp_codegen_multiply((int32_t)G_B3_0, (int32_t)((int32_t)486187739)));
		if (L_4)
		{
			G_B5_0 = ((int32_t)il2cpp_codegen_multiply((int32_t)G_B3_0, (int32_t)((int32_t)486187739)));
			goto IL_0037;
		}
	}
	{
		Type_t * L_5 = Cinfo_get_subsystemImplementationType_m0E0FEE226FD08939BF83B7B2644BEC6362BA157B_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)__this, /*hidden argument*/NULL);
		NullCheck(L_5);
		int32_t L_6 = VirtFuncInvoker0< int32_t >::Invoke(2 /* System.Int32 System.Object::GetHashCode() */, L_5);
		G_B6_0 = L_6;
		G_B6_1 = G_B4_0;
		goto IL_0038;
	}

IL_0037:
	{
		G_B6_0 = 0;
		G_B6_1 = G_B5_0;
	}

IL_0038:
	{
		bool L_7 = Cinfo_get_supportsViewportBasedRaycast_m323FE06DA2E4222E6BF4CE89541DB4630CB254B3_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)__this, /*hidden argument*/NULL);
		V_0 = L_7;
		int32_t L_8 = Boolean_GetHashCode_m92C426D44100ED098FEECC96A743C3CB92DFF737((bool*)(&V_0), /*hidden argument*/NULL);
		bool L_9 = Cinfo_get_supportsWorldBasedRaycast_mFBB112B068EE22D519CAC45E35255D6FDACCAE74_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)__this, /*hidden argument*/NULL);
		V_0 = L_9;
		int32_t L_10 = Boolean_GetHashCode_m92C426D44100ED098FEECC96A743C3CB92DFF737((bool*)(&V_0), /*hidden argument*/NULL);
		int32_t L_11 = Cinfo_get_supportedTrackableTypes_mCC57E28DFCE93ECA772B1DE2E3E49AD030D79424_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)__this, /*hidden argument*/NULL);
		V_1 = L_11;
		int32_t L_12 = Int32_GetHashCode_m245C424ECE351E5FE3277A88EEB02132DAB8C25A((int32_t*)(&V_1), /*hidden argument*/NULL);
		return ((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)G_B6_1, (int32_t)G_B6_0)), (int32_t)((int32_t)486187739))), (int32_t)L_8)), (int32_t)((int32_t)486187739))), (int32_t)L_10)), (int32_t)((int32_t)486187739))), (int32_t)L_12));
	}
}
IL2CPP_EXTERN_C  int32_t Cinfo_GetHashCode_mCC56E718130099F9809415C6C1CE9DC981F06211_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * _thisAdjusted = reinterpret_cast<Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *>(__this + _offset);
	return Cinfo_GetHashCode_mCC56E718130099F9809415C6C1CE9DC981F06211(_thisAdjusted, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_Equals_mA57BDAF996011C56A0017EFBBB805683CFA03452 (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (Cinfo_Equals_mA57BDAF996011C56A0017EFBBB805683CFA03452_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject * L_0 = ___obj0;
		if (((RuntimeObject *)IsInstSealed((RuntimeObject*)L_0, Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704_il2cpp_TypeInfo_var)))
		{
			goto IL_000a;
		}
	}
	{
		return (bool)0;
	}

IL_000a:
	{
		RuntimeObject * L_1 = ___obj0;
		bool L_2 = Cinfo_Equals_mC668E44130D6114FFD62DA8470840EE5E39DBBB5((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)__this, ((*(Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)UnBox(L_1, Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704_il2cpp_TypeInfo_var)))), /*hidden argument*/NULL);
		return L_2;
	}
}
IL2CPP_EXTERN_C  bool Cinfo_Equals_mA57BDAF996011C56A0017EFBBB805683CFA03452_AdjustorThunk (RuntimeObject * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * _thisAdjusted = reinterpret_cast<Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *>(__this + _offset);
	return Cinfo_Equals_mA57BDAF996011C56A0017EFBBB805683CFA03452(_thisAdjusted, ___obj0, method);
}
// System.String UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::ToString()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Cinfo_ToString_m9E094799A4E711569550478753C87EE9FC40DC24 (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (Cinfo_ToString_m9E094799A4E711569550478753C87EE9FC40DC24_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		bool L_0 = Cinfo_get_supportsViewportBasedRaycast_m323FE06DA2E4222E6BF4CE89541DB4630CB254B3_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)__this, /*hidden argument*/NULL);
		bool L_1 = L_0;
		RuntimeObject * L_2 = Box(Boolean_tB53F6830F670160873277339AA58F15CAED4399C_il2cpp_TypeInfo_var, &L_1);
		bool L_3 = Cinfo_get_supportsWorldBasedRaycast_mFBB112B068EE22D519CAC45E35255D6FDACCAE74_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)__this, /*hidden argument*/NULL);
		bool L_4 = L_3;
		RuntimeObject * L_5 = Box(Boolean_tB53F6830F670160873277339AA58F15CAED4399C_il2cpp_TypeInfo_var, &L_4);
		String_t* L_6 = String_Format_m19325298DBC61AAC016C16F7B3CF97A8A3DEA34A(_stringLiteralC85048E55E2A449A7E41FC166F56CC576CF57166, L_2, L_5, /*hidden argument*/NULL);
		return L_6;
	}
}
IL2CPP_EXTERN_C  String_t* Cinfo_ToString_m9E094799A4E711569550478753C87EE9FC40DC24_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * _thisAdjusted = reinterpret_cast<Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *>(__this + _offset);
	return Cinfo_ToString_m9E094799A4E711569550478753C87EE9FC40DC24(_thisAdjusted, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::Equals(UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_Equals_mC668E44130D6114FFD62DA8470840EE5E39DBBB5 (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704  ___other0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (Cinfo_Equals_mC668E44130D6114FFD62DA8470840EE5E39DBBB5_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		String_t* L_0 = Cinfo_get_id_m0418C70EB8FBC60BB9B83053AA5175AEFE31CAF8_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)__this, /*hidden argument*/NULL);
		String_t* L_1 = Cinfo_get_id_m0418C70EB8FBC60BB9B83053AA5175AEFE31CAF8_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)(&___other0), /*hidden argument*/NULL);
		bool L_2 = String_Equals_m90EB651A751C3444BADBBD5401109CE05B3E1CFB(L_0, L_1, /*hidden argument*/NULL);
		if (!L_2)
		{
			goto IL_0056;
		}
	}
	{
		Type_t * L_3 = Cinfo_get_subsystemImplementationType_m0E0FEE226FD08939BF83B7B2644BEC6362BA157B_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)__this, /*hidden argument*/NULL);
		Type_t * L_4 = Cinfo_get_subsystemImplementationType_m0E0FEE226FD08939BF83B7B2644BEC6362BA157B_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)(&___other0), /*hidden argument*/NULL);
		IL2CPP_RUNTIME_CLASS_INIT(Type_t_il2cpp_TypeInfo_var);
		bool L_5 = Type_op_Equality_m7040622C9E1037EFC73E1F0EDB1DD241282BE3D8(L_3, L_4, /*hidden argument*/NULL);
		if (!L_5)
		{
			goto IL_0056;
		}
	}
	{
		bool L_6 = Cinfo_get_supportsViewportBasedRaycast_m323FE06DA2E4222E6BF4CE89541DB4630CB254B3_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)__this, /*hidden argument*/NULL);
		bool L_7 = Cinfo_get_supportsViewportBasedRaycast_m323FE06DA2E4222E6BF4CE89541DB4630CB254B3_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)(&___other0), /*hidden argument*/NULL);
		if ((!(((uint32_t)L_6) == ((uint32_t)L_7))))
		{
			goto IL_0056;
		}
	}
	{
		bool L_8 = Cinfo_get_supportsWorldBasedRaycast_mFBB112B068EE22D519CAC45E35255D6FDACCAE74_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)__this, /*hidden argument*/NULL);
		bool L_9 = Cinfo_get_supportsWorldBasedRaycast_mFBB112B068EE22D519CAC45E35255D6FDACCAE74_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)(&___other0), /*hidden argument*/NULL);
		if ((!(((uint32_t)L_8) == ((uint32_t)L_9))))
		{
			goto IL_0056;
		}
	}
	{
		int32_t L_10 = Cinfo_get_supportedTrackableTypes_mCC57E28DFCE93ECA772B1DE2E3E49AD030D79424_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)__this, /*hidden argument*/NULL);
		int32_t L_11 = Cinfo_get_supportedTrackableTypes_mCC57E28DFCE93ECA772B1DE2E3E49AD030D79424_inline((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)(&___other0), /*hidden argument*/NULL);
		return (bool)((((int32_t)L_10) == ((int32_t)L_11))? 1 : 0);
	}

IL_0056:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool Cinfo_Equals_mC668E44130D6114FFD62DA8470840EE5E39DBBB5_AdjustorThunk (RuntimeObject * __this, Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704  ___other0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * _thisAdjusted = reinterpret_cast<Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *>(__this + _offset);
	return Cinfo_Equals_mC668E44130D6114FFD62DA8470840EE5E39DBBB5(_thisAdjusted, ___other0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::op_Equality(UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo,UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_op_Equality_mB012A524026CD2F05A29AB06C6E364D2B2BBD773 (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704  ___lhs0, Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704  ___rhs1, const RuntimeMethod* method)
{
	{
		Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704  L_0 = ___rhs1;
		bool L_1 = Cinfo_Equals_mC668E44130D6114FFD62DA8470840EE5E39DBBB5((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return L_1;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo::op_Inequality(UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo,UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor_Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_op_Inequality_mE9E256858FF976A18E836CDE86AA6AF00DC20828 (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704  ___lhs0, Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704  ___rhs1, const RuntimeMethod* method)
{
	{
		Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704  L_0 = ___rhs1;
		bool L_1 = Cinfo_Equals_mC668E44130D6114FFD62DA8470840EE5E39DBBB5((Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return (bool)((((int32_t)L_1) == ((int32_t)0))? 1 : 0);
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Conversion methods for marshalling of: UnityEngine.XR.ARSubsystems.XRReferenceImage
IL2CPP_EXTERN_C void XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E_marshal_pinvoke(const XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E& unmarshaled, XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E_marshaled_pinvoke& marshaled)
{
	Exception_t* ___m_Texture_5Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field 'm_Texture' of type 'XRReferenceImage': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___m_Texture_5Exception, NULL);
}
IL2CPP_EXTERN_C void XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E_marshal_pinvoke_back(const XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E_marshaled_pinvoke& marshaled, XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E& unmarshaled)
{
	Exception_t* ___m_Texture_5Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field 'm_Texture' of type 'XRReferenceImage': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___m_Texture_5Exception, NULL);
}
// Conversion method for clean up from marshalling of: UnityEngine.XR.ARSubsystems.XRReferenceImage
IL2CPP_EXTERN_C void XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E_marshal_pinvoke_cleanup(XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E_marshaled_pinvoke& marshaled)
{
}
// Conversion methods for marshalling of: UnityEngine.XR.ARSubsystems.XRReferenceImage
IL2CPP_EXTERN_C void XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E_marshal_com(const XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E& unmarshaled, XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E_marshaled_com& marshaled)
{
	Exception_t* ___m_Texture_5Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field 'm_Texture' of type 'XRReferenceImage': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___m_Texture_5Exception, NULL);
}
IL2CPP_EXTERN_C void XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E_marshal_com_back(const XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E_marshaled_com& marshaled, XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E& unmarshaled)
{
	Exception_t* ___m_Texture_5Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field 'm_Texture' of type 'XRReferenceImage': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___m_Texture_5Exception, NULL);
}
// Conversion method for clean up from marshalling of: UnityEngine.XR.ARSubsystems.XRReferenceImage
IL2CPP_EXTERN_C void XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E_marshal_com_cleanup(XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E_marshaled_com& marshaled)
{
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferenceImage::.ctor(UnityEngine.XR.ARSubsystems.SerializableGuid,UnityEngine.XR.ARSubsystems.SerializableGuid,System.Nullable`1<UnityEngine.Vector2>,System.String,UnityEngine.Texture2D)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRReferenceImage__ctor_m6B1ECFC5354FC9FDC73635BF5E693DA33DE02A4B (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  ___guid0, SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  ___textureGuid1, Nullable_1_t2D31B61CB1F8452E868737CCA40082AA2D1F58DC  ___size2, String_t* ___name3, Texture2D_tBBF96AC337723E2EF156DF17E09D4379FD05DE1C * ___texture4, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferenceImage__ctor_m6B1ECFC5354FC9FDC73635BF5E693DA33DE02A4B_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * G_B2_0 = NULL;
	XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * G_B1_0 = NULL;
	Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  G_B3_0;
	memset((&G_B3_0), 0, sizeof(G_B3_0));
	XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * G_B3_1 = NULL;
	{
		SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  L_0 = ___guid0;
		__this->set_m_SerializedGuid_0(L_0);
		SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  L_1 = ___textureGuid1;
		__this->set_m_SerializedTextureGuid_1(L_1);
		bool L_2 = Nullable_1_get_HasValue_m86766498BBF2DC3AB0128F33AC3F200BF93BB1B6_inline((Nullable_1_t2D31B61CB1F8452E868737CCA40082AA2D1F58DC *)(&___size2), /*hidden argument*/Nullable_1_get_HasValue_m86766498BBF2DC3AB0128F33AC3F200BF93BB1B6_RuntimeMethod_var);
		__this->set_m_SpecifySize_3(L_2);
		bool L_3 = Nullable_1_get_HasValue_m86766498BBF2DC3AB0128F33AC3F200BF93BB1B6_inline((Nullable_1_t2D31B61CB1F8452E868737CCA40082AA2D1F58DC *)(&___size2), /*hidden argument*/Nullable_1_get_HasValue_m86766498BBF2DC3AB0128F33AC3F200BF93BB1B6_RuntimeMethod_var);
		G_B1_0 = __this;
		if (L_3)
		{
			G_B2_0 = __this;
			goto IL_002c;
		}
	}
	{
		IL2CPP_RUNTIME_CLASS_INIT(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D_il2cpp_TypeInfo_var);
		Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  L_4 = Vector2_get_zero_mFE0C3213BB698130D6C5247AB4B887A59074D0A8(/*hidden argument*/NULL);
		G_B3_0 = L_4;
		G_B3_1 = G_B1_0;
		goto IL_0033;
	}

IL_002c:
	{
		Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  L_5 = Nullable_1_get_Value_m763182B16D2A96848EAFACFA31AC9985EFCC0F30((Nullable_1_t2D31B61CB1F8452E868737CCA40082AA2D1F58DC *)(&___size2), /*hidden argument*/Nullable_1_get_Value_m763182B16D2A96848EAFACFA31AC9985EFCC0F30_RuntimeMethod_var);
		G_B3_0 = L_5;
		G_B3_1 = G_B2_0;
	}

IL_0033:
	{
		G_B3_1->set_m_Size_2(G_B3_0);
		String_t* L_6 = ___name3;
		__this->set_m_Name_4(L_6);
		Texture2D_tBBF96AC337723E2EF156DF17E09D4379FD05DE1C * L_7 = ___texture4;
		__this->set_m_Texture_5(L_7);
		return;
	}
}
IL2CPP_EXTERN_C  void XRReferenceImage__ctor_m6B1ECFC5354FC9FDC73635BF5E693DA33DE02A4B_AdjustorThunk (RuntimeObject * __this, SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  ___guid0, SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821  ___textureGuid1, Nullable_1_t2D31B61CB1F8452E868737CCA40082AA2D1F58DC  ___size2, String_t* ___name3, Texture2D_tBBF96AC337723E2EF156DF17E09D4379FD05DE1C * ___texture4, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * _thisAdjusted = reinterpret_cast<XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *>(__this + _offset);
	XRReferenceImage__ctor_m6B1ECFC5354FC9FDC73635BF5E693DA33DE02A4B(_thisAdjusted, ___guid0, ___textureGuid1, ___size2, ___name3, ___texture4, method);
}
// System.Guid UnityEngine.XR.ARSubsystems.XRReferenceImage::get_guid()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Guid_t  XRReferenceImage_get_guid_mEFF96705B63F80C7C38125D170F7E62B784AEED2 (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method)
{
	{
		SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821 * L_0 = __this->get_address_of_m_SerializedGuid_0();
		Guid_t  L_1 = SerializableGuid_get_guid_mDD1F60EF61B262769627D5F48F8840285E1986A0((SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821 *)L_0, /*hidden argument*/NULL);
		return L_1;
	}
}
IL2CPP_EXTERN_C  Guid_t  XRReferenceImage_get_guid_mEFF96705B63F80C7C38125D170F7E62B784AEED2_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * _thisAdjusted = reinterpret_cast<XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *>(__this + _offset);
	return XRReferenceImage_get_guid_mEFF96705B63F80C7C38125D170F7E62B784AEED2(_thisAdjusted, method);
}
// System.Guid UnityEngine.XR.ARSubsystems.XRReferenceImage::get_textureGuid()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Guid_t  XRReferenceImage_get_textureGuid_m818FF686F0FAB8AE85630A62E3FA74C2C81C2AC0 (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method)
{
	{
		SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821 * L_0 = __this->get_address_of_m_SerializedTextureGuid_1();
		Guid_t  L_1 = SerializableGuid_get_guid_mDD1F60EF61B262769627D5F48F8840285E1986A0((SerializableGuid_tF7CD988878BEBB3281FA1C06B4457569DFD75821 *)L_0, /*hidden argument*/NULL);
		return L_1;
	}
}
IL2CPP_EXTERN_C  Guid_t  XRReferenceImage_get_textureGuid_m818FF686F0FAB8AE85630A62E3FA74C2C81C2AC0_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * _thisAdjusted = reinterpret_cast<XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *>(__this + _offset);
	return XRReferenceImage_get_textureGuid_m818FF686F0FAB8AE85630A62E3FA74C2C81C2AC0(_thisAdjusted, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferenceImage::get_specifySize()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferenceImage_get_specifySize_m5792CAE7DC7ADB8591E770B1F32D71BCCE9F0597 (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = __this->get_m_SpecifySize_3();
		return L_0;
	}
}
IL2CPP_EXTERN_C  bool XRReferenceImage_get_specifySize_m5792CAE7DC7ADB8591E770B1F32D71BCCE9F0597_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * _thisAdjusted = reinterpret_cast<XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *>(__this + _offset);
	return XRReferenceImage_get_specifySize_m5792CAE7DC7ADB8591E770B1F32D71BCCE9F0597_inline(_thisAdjusted, method);
}
// UnityEngine.Vector2 UnityEngine.XR.ARSubsystems.XRReferenceImage::get_size()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  XRReferenceImage_get_size_m29A6DA526141F214BE2949524305EFE91C07FA32 (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method)
{
	{
		Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  L_0 = __this->get_m_Size_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C  Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  XRReferenceImage_get_size_m29A6DA526141F214BE2949524305EFE91C07FA32_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * _thisAdjusted = reinterpret_cast<XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *>(__this + _offset);
	return XRReferenceImage_get_size_m29A6DA526141F214BE2949524305EFE91C07FA32_inline(_thisAdjusted, method);
}
// System.Single UnityEngine.XR.ARSubsystems.XRReferenceImage::get_width()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR float XRReferenceImage_get_width_m52157ED5E292633BA8BA0CFE7F97A756B6985DB8 (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method)
{
	{
		Vector2_tA85D2DD88578276CA8A8796756458277E72D073D * L_0 = __this->get_address_of_m_Size_2();
		float L_1 = L_0->get_x_0();
		return L_1;
	}
}
IL2CPP_EXTERN_C  float XRReferenceImage_get_width_m52157ED5E292633BA8BA0CFE7F97A756B6985DB8_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * _thisAdjusted = reinterpret_cast<XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *>(__this + _offset);
	return XRReferenceImage_get_width_m52157ED5E292633BA8BA0CFE7F97A756B6985DB8(_thisAdjusted, method);
}
// System.Single UnityEngine.XR.ARSubsystems.XRReferenceImage::get_height()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR float XRReferenceImage_get_height_mA55A287CAA626B1817465DA78D946D3EB82E0D8D (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method)
{
	{
		Vector2_tA85D2DD88578276CA8A8796756458277E72D073D * L_0 = __this->get_address_of_m_Size_2();
		float L_1 = L_0->get_y_1();
		return L_1;
	}
}
IL2CPP_EXTERN_C  float XRReferenceImage_get_height_mA55A287CAA626B1817465DA78D946D3EB82E0D8D_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * _thisAdjusted = reinterpret_cast<XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *>(__this + _offset);
	return XRReferenceImage_get_height_mA55A287CAA626B1817465DA78D946D3EB82E0D8D(_thisAdjusted, method);
}
// System.String UnityEngine.XR.ARSubsystems.XRReferenceImage::get_name()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* XRReferenceImage_get_name_m6DD83ECED755444645A14816918747E2EEAE92A9 (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method)
{
	{
		String_t* L_0 = __this->get_m_Name_4();
		return L_0;
	}
}
IL2CPP_EXTERN_C  String_t* XRReferenceImage_get_name_m6DD83ECED755444645A14816918747E2EEAE92A9_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * _thisAdjusted = reinterpret_cast<XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *>(__this + _offset);
	return XRReferenceImage_get_name_m6DD83ECED755444645A14816918747E2EEAE92A9_inline(_thisAdjusted, method);
}
// UnityEngine.Texture2D UnityEngine.XR.ARSubsystems.XRReferenceImage::get_texture()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Texture2D_tBBF96AC337723E2EF156DF17E09D4379FD05DE1C * XRReferenceImage_get_texture_m97887B57DD747DCE051484D1C97F1240B673FE16 (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method)
{
	{
		Texture2D_tBBF96AC337723E2EF156DF17E09D4379FD05DE1C * L_0 = __this->get_m_Texture_5();
		return L_0;
	}
}
IL2CPP_EXTERN_C  Texture2D_tBBF96AC337723E2EF156DF17E09D4379FD05DE1C * XRReferenceImage_get_texture_m97887B57DD747DCE051484D1C97F1240B673FE16_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * _thisAdjusted = reinterpret_cast<XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *>(__this + _offset);
	return XRReferenceImage_get_texture_m97887B57DD747DCE051484D1C97F1240B673FE16_inline(_thisAdjusted, method);
}
// System.String UnityEngine.XR.ARSubsystems.XRReferenceImage::ToString()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* XRReferenceImage_ToString_m37745D8B95903B29F917CE6E0A141E79E6F4B937 (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferenceImage_ToString_m37745D8B95903B29F917CE6E0A141E79E6F4B937_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	int32_t G_B2_0 = 0;
	ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* G_B2_1 = NULL;
	ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* G_B2_2 = NULL;
	String_t* G_B2_3 = NULL;
	int32_t G_B1_0 = 0;
	ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* G_B1_1 = NULL;
	ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* G_B1_2 = NULL;
	String_t* G_B1_3 = NULL;
	String_t* G_B3_0 = NULL;
	int32_t G_B3_1 = 0;
	ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* G_B3_2 = NULL;
	ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* G_B3_3 = NULL;
	String_t* G_B3_4 = NULL;
	{
		ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* L_0 = (ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A*)(ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A*)SZArrayNew(ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A_il2cpp_TypeInfo_var, (uint32_t)4);
		ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* L_1 = L_0;
		Guid_t  L_2 = XRReferenceImage_get_guid_mEFF96705B63F80C7C38125D170F7E62B784AEED2((XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *)__this, /*hidden argument*/NULL);
		Guid_t  L_3 = L_2;
		RuntimeObject * L_4 = Box(Guid_t_il2cpp_TypeInfo_var, &L_3);
		NullCheck(L_1);
		ArrayElementTypeCheck (L_1, L_4);
		(L_1)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject *)L_4);
		ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* L_5 = L_1;
		Guid_t  L_6 = XRReferenceImage_get_textureGuid_m818FF686F0FAB8AE85630A62E3FA74C2C81C2AC0((XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *)__this, /*hidden argument*/NULL);
		Guid_t  L_7 = L_6;
		RuntimeObject * L_8 = Box(Guid_t_il2cpp_TypeInfo_var, &L_7);
		NullCheck(L_5);
		ArrayElementTypeCheck (L_5, L_8);
		(L_5)->SetAt(static_cast<il2cpp_array_size_t>(1), (RuntimeObject *)L_8);
		ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* L_9 = L_5;
		bool L_10 = __this->get_m_SpecifySize_3();
		G_B1_0 = 2;
		G_B1_1 = L_9;
		G_B1_2 = L_9;
		G_B1_3 = _stringLiteralBAE9CCE613FB80AADF756BAC01EBC9DEF93DA2C0;
		if (L_10)
		{
			G_B2_0 = 2;
			G_B2_1 = L_9;
			G_B2_2 = L_9;
			G_B2_3 = _stringLiteralBAE9CCE613FB80AADF756BAC01EBC9DEF93DA2C0;
			goto IL_0038;
		}
	}
	{
		G_B3_0 = _stringLiteral975146EEDDCE8BEE0A51FB0C58FD4F55FB3E29BF;
		G_B3_1 = G_B1_0;
		G_B3_2 = G_B1_1;
		G_B3_3 = G_B1_2;
		G_B3_4 = G_B1_3;
		goto IL_003d;
	}

IL_0038:
	{
		G_B3_0 = _stringLiteralDA39A3EE5E6B4B0D3255BFEF95601890AFD80709;
		G_B3_1 = G_B2_0;
		G_B3_2 = G_B2_1;
		G_B3_3 = G_B2_2;
		G_B3_4 = G_B2_3;
	}

IL_003d:
	{
		NullCheck(G_B3_2);
		ArrayElementTypeCheck (G_B3_2, G_B3_0);
		(G_B3_2)->SetAt(static_cast<il2cpp_array_size_t>(G_B3_1), (RuntimeObject *)G_B3_0);
		ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* L_11 = G_B3_3;
		Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  L_12 = __this->get_m_Size_2();
		Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  L_13 = L_12;
		RuntimeObject * L_14 = Box(Vector2_tA85D2DD88578276CA8A8796756458277E72D073D_il2cpp_TypeInfo_var, &L_13);
		NullCheck(L_11);
		ArrayElementTypeCheck (L_11, L_14);
		(L_11)->SetAt(static_cast<il2cpp_array_size_t>(3), (RuntimeObject *)L_14);
		String_t* L_15 = String_Format_mA3AC3FE7B23D97F3A5BAA082D25B0E01B341A865(G_B3_4, L_11, /*hidden argument*/NULL);
		return L_15;
	}
}
IL2CPP_EXTERN_C  String_t* XRReferenceImage_ToString_m37745D8B95903B29F917CE6E0A141E79E6F4B937_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * _thisAdjusted = reinterpret_cast<XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *>(__this + _offset);
	return XRReferenceImage_ToString_m37745D8B95903B29F917CE6E0A141E79E6F4B937(_thisAdjusted, method);
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRReferenceImage::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRReferenceImage_GetHashCode_mFBF4D8E0A33B3EFDCB6D6E5A944AB4CF52AAB334 (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method)
{
	Guid_t  V_0;
	memset((&V_0), 0, sizeof(V_0));
	{
		Guid_t  L_0 = XRReferenceImage_get_guid_mEFF96705B63F80C7C38125D170F7E62B784AEED2((XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *)__this, /*hidden argument*/NULL);
		V_0 = L_0;
		int32_t L_1 = Guid_GetHashCode_mEB01C6BA267B1CCD624BCA91D09B803C9B6E5369((Guid_t *)(&V_0), /*hidden argument*/NULL);
		return L_1;
	}
}
IL2CPP_EXTERN_C  int32_t XRReferenceImage_GetHashCode_mFBF4D8E0A33B3EFDCB6D6E5A944AB4CF52AAB334_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * _thisAdjusted = reinterpret_cast<XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *>(__this + _offset);
	return XRReferenceImage_GetHashCode_mFBF4D8E0A33B3EFDCB6D6E5A944AB4CF52AAB334(_thisAdjusted, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferenceImage::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferenceImage_Equals_m90DF560ECB15F4363E40AF3C3A2F82F5E0FD147D (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferenceImage_Equals_m90DF560ECB15F4363E40AF3C3A2F82F5E0FD147D_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject * L_0 = ___obj0;
		if (!((RuntimeObject *)IsInstSealed((RuntimeObject*)L_0, XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E_il2cpp_TypeInfo_var)))
		{
			goto IL_0015;
		}
	}
	{
		RuntimeObject * L_1 = ___obj0;
		bool L_2 = XRReferenceImage_Equals_m5FBBB7CFF96645894AA08221795ABC2A98E4DEF5((XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *)__this, ((*(XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *)((XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *)UnBox(L_1, XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E_il2cpp_TypeInfo_var)))), /*hidden argument*/NULL);
		return L_2;
	}

IL_0015:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool XRReferenceImage_Equals_m90DF560ECB15F4363E40AF3C3A2F82F5E0FD147D_AdjustorThunk (RuntimeObject * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * _thisAdjusted = reinterpret_cast<XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *>(__this + _offset);
	return XRReferenceImage_Equals_m90DF560ECB15F4363E40AF3C3A2F82F5E0FD147D(_thisAdjusted, ___obj0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferenceImage::Equals(UnityEngine.XR.ARSubsystems.XRReferenceImage)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferenceImage_Equals_m5FBBB7CFF96645894AA08221795ABC2A98E4DEF5 (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  ___other0, const RuntimeMethod* method)
{
	Guid_t  V_0;
	memset((&V_0), 0, sizeof(V_0));
	{
		Guid_t  L_0 = XRReferenceImage_get_guid_mEFF96705B63F80C7C38125D170F7E62B784AEED2((XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *)__this, /*hidden argument*/NULL);
		V_0 = L_0;
		Guid_t  L_1 = XRReferenceImage_get_guid_mEFF96705B63F80C7C38125D170F7E62B784AEED2((XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *)(&___other0), /*hidden argument*/NULL);
		bool L_2 = Guid_Equals_mC7FC66A530A8B6FC95E8F5F9E34AE81FD44CD245((Guid_t *)(&V_0), L_1, /*hidden argument*/NULL);
		return L_2;
	}
}
IL2CPP_EXTERN_C  bool XRReferenceImage_Equals_m5FBBB7CFF96645894AA08221795ABC2A98E4DEF5_AdjustorThunk (RuntimeObject * __this, XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  ___other0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * _thisAdjusted = reinterpret_cast<XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *>(__this + _offset);
	return XRReferenceImage_Equals_m5FBBB7CFF96645894AA08221795ABC2A98E4DEF5(_thisAdjusted, ___other0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferenceImage::op_Equality(UnityEngine.XR.ARSubsystems.XRReferenceImage,UnityEngine.XR.ARSubsystems.XRReferenceImage)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferenceImage_op_Equality_mD0355E3DAD36502F8CF26DDDCCBFB5440D41A171 (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  ___lhs0, XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  ___rhs1, const RuntimeMethod* method)
{
	{
		XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  L_0 = ___rhs1;
		bool L_1 = XRReferenceImage_Equals_m5FBBB7CFF96645894AA08221795ABC2A98E4DEF5((XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return L_1;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferenceImage::op_Inequality(UnityEngine.XR.ARSubsystems.XRReferenceImage,UnityEngine.XR.ARSubsystems.XRReferenceImage)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferenceImage_op_Inequality_mDDDE4B2F7C0270E4FE4837E693B36C72892C4297 (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  ___lhs0, XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  ___rhs1, const RuntimeMethod* method)
{
	{
		XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  L_0 = ___rhs1;
		bool L_1 = XRReferenceImage_Equals_m5FBBB7CFF96645894AA08221795ABC2A98E4DEF5((XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return (bool)((((int32_t)L_1) == ((int32_t)0))? 1 : 0);
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Int32 UnityEngine.XR.ARSubsystems.XRReferenceImageLibrary::get_count()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRReferenceImageLibrary_get_count_m031E8D7C22B586EE44B0256912F688867E9DEB84 (XRReferenceImageLibrary_t38B21DC6650EADA892125F045DCBF71EBC3F6A8F * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferenceImageLibrary_get_count_m031E8D7C22B586EE44B0256912F688867E9DEB84_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * L_0 = __this->get_m_Images_6();
		NullCheck(L_0);
		int32_t L_1 = List_1_get_Count_mC10370473AE5833D24A5F9361290A6B38CC73C32_inline(L_0, /*hidden argument*/List_1_get_Count_mC10370473AE5833D24A5F9361290A6B38CC73C32_RuntimeMethod_var);
		return L_1;
	}
}
// System.Collections.Generic.List`1_Enumerator<UnityEngine.XR.ARSubsystems.XRReferenceImage> UnityEngine.XR.ARSubsystems.XRReferenceImageLibrary::GetEnumerator()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Enumerator_t90B18C0FA1516886E459FA26C65233B656F793EB  XRReferenceImageLibrary_GetEnumerator_m7F1F01CC0BD4EF373F5B3766D2D1476901C4DCFD (XRReferenceImageLibrary_t38B21DC6650EADA892125F045DCBF71EBC3F6A8F * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferenceImageLibrary_GetEnumerator_m7F1F01CC0BD4EF373F5B3766D2D1476901C4DCFD_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * L_0 = __this->get_m_Images_6();
		NullCheck(L_0);
		Enumerator_t90B18C0FA1516886E459FA26C65233B656F793EB  L_1 = List_1_GetEnumerator_mD7B1224990EE97B1239AF20ACDA9FC21BCAEEBC5(L_0, /*hidden argument*/List_1_GetEnumerator_mD7B1224990EE97B1239AF20ACDA9FC21BCAEEBC5_RuntimeMethod_var);
		return L_1;
	}
}
// UnityEngine.XR.ARSubsystems.XRReferenceImage UnityEngine.XR.ARSubsystems.XRReferenceImageLibrary::get_Item(System.Int32)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  XRReferenceImageLibrary_get_Item_m9FE52A96359701129EC85E3AFB382E08E7E3B799 (XRReferenceImageLibrary_t38B21DC6650EADA892125F045DCBF71EBC3F6A8F * __this, int32_t ___index0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferenceImageLibrary_get_Item_m9FE52A96359701129EC85E3AFB382E08E7E3B799_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		int32_t L_0 = XRReferenceImageLibrary_get_count_m031E8D7C22B586EE44B0256912F688867E9DEB84(__this, /*hidden argument*/NULL);
		if (L_0)
		{
			goto IL_0013;
		}
	}
	{
		IndexOutOfRangeException_tEC7665FC66525AB6A6916A7EB505E5591683F0CF * L_1 = (IndexOutOfRangeException_tEC7665FC66525AB6A6916A7EB505E5591683F0CF *)il2cpp_codegen_object_new(IndexOutOfRangeException_tEC7665FC66525AB6A6916A7EB505E5591683F0CF_il2cpp_TypeInfo_var);
		IndexOutOfRangeException__ctor_mCCE2EFF47A0ACB4B2636F63140F94FCEA71A9BCA(L_1, _stringLiteral74E2C3E4176EC7B2A7DD081FFCB26DBE573A880E, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_1, XRReferenceImageLibrary_get_Item_m9FE52A96359701129EC85E3AFB382E08E7E3B799_RuntimeMethod_var);
	}

IL_0013:
	{
		int32_t L_2 = ___index0;
		if ((((int32_t)L_2) < ((int32_t)0)))
		{
			goto IL_0020;
		}
	}
	{
		int32_t L_3 = ___index0;
		int32_t L_4 = XRReferenceImageLibrary_get_count_m031E8D7C22B586EE44B0256912F688867E9DEB84(__this, /*hidden argument*/NULL);
		if ((((int32_t)L_3) < ((int32_t)L_4)))
		{
			goto IL_0043;
		}
	}

IL_0020:
	{
		int32_t L_5 = ___index0;
		int32_t L_6 = L_5;
		RuntimeObject * L_7 = Box(Int32_t585191389E07734F19F3156FF88FB3EF4800D102_il2cpp_TypeInfo_var, &L_6);
		int32_t L_8 = XRReferenceImageLibrary_get_count_m031E8D7C22B586EE44B0256912F688867E9DEB84(__this, /*hidden argument*/NULL);
		int32_t L_9 = ((int32_t)il2cpp_codegen_subtract((int32_t)L_8, (int32_t)1));
		RuntimeObject * L_10 = Box(Int32_t585191389E07734F19F3156FF88FB3EF4800D102_il2cpp_TypeInfo_var, &L_9);
		String_t* L_11 = String_Format_m19325298DBC61AAC016C16F7B3CF97A8A3DEA34A(_stringLiteralD60A29563E597D126763623865B515BACF0F6C94, L_7, L_10, /*hidden argument*/NULL);
		IndexOutOfRangeException_tEC7665FC66525AB6A6916A7EB505E5591683F0CF * L_12 = (IndexOutOfRangeException_tEC7665FC66525AB6A6916A7EB505E5591683F0CF *)il2cpp_codegen_object_new(IndexOutOfRangeException_tEC7665FC66525AB6A6916A7EB505E5591683F0CF_il2cpp_TypeInfo_var);
		IndexOutOfRangeException__ctor_mCCE2EFF47A0ACB4B2636F63140F94FCEA71A9BCA(L_12, L_11, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_12, XRReferenceImageLibrary_get_Item_m9FE52A96359701129EC85E3AFB382E08E7E3B799_RuntimeMethod_var);
	}

IL_0043:
	{
		List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * L_13 = __this->get_m_Images_6();
		int32_t L_14 = ___index0;
		NullCheck(L_13);
		XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  L_15 = List_1_get_Item_m0935B6D4B21A74A42474E6C822B5DA4F8A32FE00_inline(L_13, L_14, /*hidden argument*/List_1_get_Item_m0935B6D4B21A74A42474E6C822B5DA4F8A32FE00_RuntimeMethod_var);
		return L_15;
	}
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRReferenceImageLibrary::indexOf(UnityEngine.XR.ARSubsystems.XRReferenceImage)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRReferenceImageLibrary_indexOf_m6E722FBAD970A7FB4C9415EDF6F46BAA25B89116 (XRReferenceImageLibrary_t38B21DC6650EADA892125F045DCBF71EBC3F6A8F * __this, XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  ___referenceImage0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferenceImageLibrary_indexOf_m6E722FBAD970A7FB4C9415EDF6F46BAA25B89116_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * L_0 = __this->get_m_Images_6();
		XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  L_1 = ___referenceImage0;
		NullCheck(L_0);
		int32_t L_2 = List_1_IndexOf_m60FE5836AABB98EF6AD4A55ECB7085D854CA5E87(L_0, L_1, /*hidden argument*/List_1_IndexOf_m60FE5836AABB98EF6AD4A55ECB7085D854CA5E87_RuntimeMethod_var);
		return L_2;
	}
}
// System.Guid UnityEngine.XR.ARSubsystems.XRReferenceImageLibrary::get_guid()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Guid_t  XRReferenceImageLibrary_get_guid_mAE3BC056A0B6817FD14E09D150B561CB468EFCDC (XRReferenceImageLibrary_t38B21DC6650EADA892125F045DCBF71EBC3F6A8F * __this, const RuntimeMethod* method)
{
	{
		uint64_t L_0 = __this->get_m_GuidLow_4();
		uint64_t L_1 = __this->get_m_GuidHigh_5();
		Guid_t  L_2 = GuidUtil_Compose_mF0A2DF31C9F5E45DC7786601C82B926546B021D4(L_0, L_1, /*hidden argument*/NULL);
		return L_2;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferenceImageLibrary::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRReferenceImageLibrary__ctor_m783C31BF895E269078A6F82966CE008024A5450D (XRReferenceImageLibrary_t38B21DC6650EADA892125F045DCBF71EBC3F6A8F * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferenceImageLibrary__ctor_m783C31BF895E269078A6F82966CE008024A5450D_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * L_0 = (List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F *)il2cpp_codegen_object_new(List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F_il2cpp_TypeInfo_var);
		List_1__ctor_m95E964C07FFF3EC898BA56E8B9ADE6E55ECEA0B9(L_0, /*hidden argument*/List_1__ctor_m95E964C07FFF3EC898BA56E8B9ADE6E55ECEA0B9_RuntimeMethod_var);
		__this->set_m_Images_6(L_0);
		ScriptableObject__ctor_m6E2B3821A4A361556FC12E9B1C71E1D5DC002C5B(__this, /*hidden argument*/NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Conversion methods for marshalling of: UnityEngine.XR.ARSubsystems.XRReferenceObject
IL2CPP_EXTERN_C void XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA_marshal_pinvoke(const XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA& unmarshaled, XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA_marshaled_pinvoke& marshaled)
{
	Exception_t* ___m_Entries_3Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field 'm_Entries' of type 'XRReferenceObject'.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___m_Entries_3Exception, NULL);
}
IL2CPP_EXTERN_C void XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA_marshal_pinvoke_back(const XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA_marshaled_pinvoke& marshaled, XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA& unmarshaled)
{
	Exception_t* ___m_Entries_3Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field 'm_Entries' of type 'XRReferenceObject'.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___m_Entries_3Exception, NULL);
}
// Conversion method for clean up from marshalling of: UnityEngine.XR.ARSubsystems.XRReferenceObject
IL2CPP_EXTERN_C void XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA_marshal_pinvoke_cleanup(XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA_marshaled_pinvoke& marshaled)
{
}
// Conversion methods for marshalling of: UnityEngine.XR.ARSubsystems.XRReferenceObject
IL2CPP_EXTERN_C void XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA_marshal_com(const XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA& unmarshaled, XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA_marshaled_com& marshaled)
{
	Exception_t* ___m_Entries_3Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field 'm_Entries' of type 'XRReferenceObject'.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___m_Entries_3Exception, NULL);
}
IL2CPP_EXTERN_C void XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA_marshal_com_back(const XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA_marshaled_com& marshaled, XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA& unmarshaled)
{
	Exception_t* ___m_Entries_3Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field 'm_Entries' of type 'XRReferenceObject'.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___m_Entries_3Exception, NULL);
}
// Conversion method for clean up from marshalling of: UnityEngine.XR.ARSubsystems.XRReferenceObject
IL2CPP_EXTERN_C void XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA_marshal_com_cleanup(XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA_marshaled_com& marshaled)
{
}
// System.String UnityEngine.XR.ARSubsystems.XRReferenceObject::get_name()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* XRReferenceObject_get_name_mB9D4C5BF34D4FF064180412CCE6585867BA98718 (XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * __this, const RuntimeMethod* method)
{
	{
		String_t* L_0 = __this->get_m_Name_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C  String_t* XRReferenceObject_get_name_mB9D4C5BF34D4FF064180412CCE6585867BA98718_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * _thisAdjusted = reinterpret_cast<XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA *>(__this + _offset);
	return XRReferenceObject_get_name_mB9D4C5BF34D4FF064180412CCE6585867BA98718_inline(_thisAdjusted, method);
}
// System.Guid UnityEngine.XR.ARSubsystems.XRReferenceObject::get_guid()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Guid_t  XRReferenceObject_get_guid_mA7BD0F3F54EABC39D19355113087CD4DFF94BE57 (XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * __this, const RuntimeMethod* method)
{
	{
		uint64_t L_0 = __this->get_m_GuidLow_0();
		uint64_t L_1 = __this->get_m_GuidHigh_1();
		Guid_t  L_2 = GuidUtil_Compose_mF0A2DF31C9F5E45DC7786601C82B926546B021D4(L_0, L_1, /*hidden argument*/NULL);
		return L_2;
	}
}
IL2CPP_EXTERN_C  Guid_t  XRReferenceObject_get_guid_mA7BD0F3F54EABC39D19355113087CD4DFF94BE57_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * _thisAdjusted = reinterpret_cast<XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA *>(__this + _offset);
	return XRReferenceObject_get_guid_mA7BD0F3F54EABC39D19355113087CD4DFF94BE57(_thisAdjusted, method);
}
// UnityEngine.XR.ARSubsystems.XRReferenceObjectEntry UnityEngine.XR.ARSubsystems.XRReferenceObject::FindEntry(System.Type)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8 * XRReferenceObject_FindEntry_mBCEBCEF4265B7D210FFA15179493BF8BDBB70C94 (XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * __this, Type_t * ___type0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferenceObject_FindEntry_mBCEBCEF4265B7D210FFA15179493BF8BDBB70C94_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	Enumerator_t8F224602FB8179E9E02E220ED94A72EA075C763F  V_0;
	memset((&V_0), 0, sizeof(V_0));
	XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8 * V_1 = NULL;
	XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8 * V_2 = NULL;
	Exception_t * __last_unhandled_exception = 0;
	NO_UNUSED_WARNING (__last_unhandled_exception);
	Exception_t * __exception_local = 0;
	NO_UNUSED_WARNING (__exception_local);
	void* __leave_targets_storage = alloca(sizeof(int32_t) * 2);
	il2cpp::utils::LeaveTargetStack __leave_targets(__leave_targets_storage);
	NO_UNUSED_WARNING (__leave_targets);
	{
		Type_t * L_0 = ___type0;
		IL2CPP_RUNTIME_CLASS_INIT(Type_t_il2cpp_TypeInfo_var);
		bool L_1 = Type_op_Equality_m7040622C9E1037EFC73E1F0EDB1DD241282BE3D8(L_0, (Type_t *)NULL, /*hidden argument*/NULL);
		if (!L_1)
		{
			goto IL_0014;
		}
	}
	{
		ArgumentNullException_t581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD * L_2 = (ArgumentNullException_t581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD *)il2cpp_codegen_object_new(ArgumentNullException_t581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD_il2cpp_TypeInfo_var);
		ArgumentNullException__ctor_mEE0C0D6FCB2D08CD7967DBB1329A0854BBED49ED(L_2, _stringLiteralD0A3E7F81A9885E99049D1CAE0336D269D5E47A9, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_2, XRReferenceObject_FindEntry_mBCEBCEF4265B7D210FFA15179493BF8BDBB70C94_RuntimeMethod_var);
	}

IL_0014:
	{
		List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE * L_3 = __this->get_m_Entries_3();
		NullCheck(L_3);
		Enumerator_t8F224602FB8179E9E02E220ED94A72EA075C763F  L_4 = List_1_GetEnumerator_mC07E381535940BCF47E02BD6CA84103EBBBB217E(L_3, /*hidden argument*/List_1_GetEnumerator_mC07E381535940BCF47E02BD6CA84103EBBBB217E_RuntimeMethod_var);
		V_0 = L_4;
	}

IL_0020:
	try
	{ // begin try (depth: 1)
		{
			goto IL_0045;
		}

IL_0022:
		{
			XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8 * L_5 = Enumerator_get_Current_mD47F152CC824D5536C956CF7F08372836B4344C6_inline((Enumerator_t8F224602FB8179E9E02E220ED94A72EA075C763F *)(&V_0), /*hidden argument*/Enumerator_get_Current_mD47F152CC824D5536C956CF7F08372836B4344C6_RuntimeMethod_var);
			V_1 = L_5;
			XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8 * L_6 = V_1;
			IL2CPP_RUNTIME_CLASS_INIT(Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0_il2cpp_TypeInfo_var);
			bool L_7 = Object_op_Inequality_m31EF58E217E8F4BDD3E409DEF79E1AEE95874FC1(L_6, (Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0 *)NULL, /*hidden argument*/NULL);
			if (!L_7)
			{
				goto IL_0045;
			}
		}

IL_0033:
		{
			XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8 * L_8 = V_1;
			NullCheck(L_8);
			Type_t * L_9 = Object_GetType_m2E0B62414ECCAA3094B703790CE88CBB2F83EA60(L_8, /*hidden argument*/NULL);
			Type_t * L_10 = ___type0;
			IL2CPP_RUNTIME_CLASS_INIT(Type_t_il2cpp_TypeInfo_var);
			bool L_11 = Type_op_Equality_m7040622C9E1037EFC73E1F0EDB1DD241282BE3D8(L_9, L_10, /*hidden argument*/NULL);
			if (!L_11)
			{
				goto IL_0045;
			}
		}

IL_0041:
		{
			XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8 * L_12 = V_1;
			V_2 = L_12;
			IL2CPP_LEAVE(0x60, FINALLY_0050);
		}

IL_0045:
		{
			bool L_13 = Enumerator_MoveNext_m0B347A783040C9CE4B9C1889A87ED80B3A0DCB8B((Enumerator_t8F224602FB8179E9E02E220ED94A72EA075C763F *)(&V_0), /*hidden argument*/Enumerator_MoveNext_m0B347A783040C9CE4B9C1889A87ED80B3A0DCB8B_RuntimeMethod_var);
			if (L_13)
			{
				goto IL_0022;
			}
		}

IL_004e:
		{
			IL2CPP_LEAVE(0x5E, FINALLY_0050);
		}
	} // end try (depth: 1)
	catch(Il2CppExceptionWrapper& e)
	{
		__last_unhandled_exception = (Exception_t *)e.ex;
		goto FINALLY_0050;
	}

FINALLY_0050:
	{ // begin finally (depth: 1)
		Enumerator_Dispose_m7B6EFE249DF6F43B30BEE8350B87A4231CAA0406((Enumerator_t8F224602FB8179E9E02E220ED94A72EA075C763F *)(&V_0), /*hidden argument*/Enumerator_Dispose_m7B6EFE249DF6F43B30BEE8350B87A4231CAA0406_RuntimeMethod_var);
		IL2CPP_END_FINALLY(80)
	} // end finally (depth: 1)
	IL2CPP_CLEANUP(80)
	{
		IL2CPP_JUMP_TBL(0x60, IL_0060)
		IL2CPP_JUMP_TBL(0x5E, IL_005e)
		IL2CPP_RETHROW_IF_UNHANDLED(Exception_t *)
	}

IL_005e:
	{
		return (XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8 *)NULL;
	}

IL_0060:
	{
		XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8 * L_14 = V_2;
		return L_14;
	}
}
IL2CPP_EXTERN_C  XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8 * XRReferenceObject_FindEntry_mBCEBCEF4265B7D210FFA15179493BF8BDBB70C94_AdjustorThunk (RuntimeObject * __this, Type_t * ___type0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * _thisAdjusted = reinterpret_cast<XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA *>(__this + _offset);
	return XRReferenceObject_FindEntry_mBCEBCEF4265B7D210FFA15179493BF8BDBB70C94(_thisAdjusted, ___type0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferenceObject::Equals(UnityEngine.XR.ARSubsystems.XRReferenceObject)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferenceObject_Equals_m504798221B2E8A72005FD241A5C5E2A063FF37A2 (XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * __this, XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  ___other0, const RuntimeMethod* method)
{
	{
		uint64_t L_0 = __this->get_m_GuidLow_0();
		XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  L_1 = ___other0;
		uint64_t L_2 = L_1.get_m_GuidLow_0();
		if ((!(((uint64_t)L_0) == ((uint64_t)L_2))))
		{
			goto IL_003e;
		}
	}
	{
		uint64_t L_3 = __this->get_m_GuidHigh_1();
		XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  L_4 = ___other0;
		uint64_t L_5 = L_4.get_m_GuidHigh_1();
		if ((!(((uint64_t)L_3) == ((uint64_t)L_5))))
		{
			goto IL_003e;
		}
	}
	{
		String_t* L_6 = __this->get_m_Name_2();
		XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  L_7 = ___other0;
		String_t* L_8 = L_7.get_m_Name_2();
		bool L_9 = String_Equals_m90EB651A751C3444BADBBD5401109CE05B3E1CFB(L_6, L_8, /*hidden argument*/NULL);
		if (!L_9)
		{
			goto IL_003e;
		}
	}
	{
		List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE * L_10 = __this->get_m_Entries_3();
		XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  L_11 = ___other0;
		List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE * L_12 = L_11.get_m_Entries_3();
		return (bool)((((RuntimeObject*)(List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE *)L_10) == ((RuntimeObject*)(List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE *)L_12))? 1 : 0);
	}

IL_003e:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool XRReferenceObject_Equals_m504798221B2E8A72005FD241A5C5E2A063FF37A2_AdjustorThunk (RuntimeObject * __this, XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  ___other0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * _thisAdjusted = reinterpret_cast<XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA *>(__this + _offset);
	return XRReferenceObject_Equals_m504798221B2E8A72005FD241A5C5E2A063FF37A2(_thisAdjusted, ___other0, method);
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRReferenceObject::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRReferenceObject_GetHashCode_mE918BE08147EE066AF8CC5971E4F5A9221881678 (XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * __this, const RuntimeMethod* method)
{
	int32_t G_B2_0 = 0;
	int32_t G_B1_0 = 0;
	int32_t G_B3_0 = 0;
	int32_t G_B3_1 = 0;
	int32_t G_B5_0 = 0;
	int32_t G_B4_0 = 0;
	int32_t G_B6_0 = 0;
	int32_t G_B6_1 = 0;
	{
		uint64_t* L_0 = __this->get_address_of_m_GuidLow_0();
		int32_t L_1 = UInt64_GetHashCode_mCBB4031BF70D0CBD023B4D71F4FEA37BE2C749AD((uint64_t*)L_0, /*hidden argument*/NULL);
		uint64_t* L_2 = __this->get_address_of_m_GuidHigh_1();
		int32_t L_3 = UInt64_GetHashCode_mCBB4031BF70D0CBD023B4D71F4FEA37BE2C749AD((uint64_t*)L_2, /*hidden argument*/NULL);
		String_t* L_4 = __this->get_m_Name_2();
		G_B1_0 = ((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)L_1, (int32_t)((int32_t)486187739))), (int32_t)L_3)), (int32_t)((int32_t)486187739)));
		if (!L_4)
		{
			G_B2_0 = ((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)L_1, (int32_t)((int32_t)486187739))), (int32_t)L_3)), (int32_t)((int32_t)486187739)));
			goto IL_0038;
		}
	}
	{
		String_t* L_5 = __this->get_m_Name_2();
		NullCheck(L_5);
		int32_t L_6 = VirtFuncInvoker0< int32_t >::Invoke(2 /* System.Int32 System.Object::GetHashCode() */, L_5);
		G_B3_0 = L_6;
		G_B3_1 = G_B1_0;
		goto IL_0039;
	}

IL_0038:
	{
		G_B3_0 = 0;
		G_B3_1 = G_B2_0;
	}

IL_0039:
	{
		List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE * L_7 = __this->get_m_Entries_3();
		G_B4_0 = ((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)G_B3_1, (int32_t)G_B3_0)), (int32_t)((int32_t)486187739)));
		if (!L_7)
		{
			G_B5_0 = ((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)G_B3_1, (int32_t)G_B3_0)), (int32_t)((int32_t)486187739)));
			goto IL_0055;
		}
	}
	{
		List_1_t564CABFE377BD9215732520DE10D1FEB72E336AE * L_8 = __this->get_m_Entries_3();
		NullCheck(L_8);
		int32_t L_9 = VirtFuncInvoker0< int32_t >::Invoke(2 /* System.Int32 System.Object::GetHashCode() */, L_8);
		G_B6_0 = L_9;
		G_B6_1 = G_B4_0;
		goto IL_0056;
	}

IL_0055:
	{
		G_B6_0 = 0;
		G_B6_1 = G_B5_0;
	}

IL_0056:
	{
		return ((int32_t)il2cpp_codegen_add((int32_t)G_B6_1, (int32_t)G_B6_0));
	}
}
IL2CPP_EXTERN_C  int32_t XRReferenceObject_GetHashCode_mE918BE08147EE066AF8CC5971E4F5A9221881678_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * _thisAdjusted = reinterpret_cast<XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA *>(__this + _offset);
	return XRReferenceObject_GetHashCode_mE918BE08147EE066AF8CC5971E4F5A9221881678(_thisAdjusted, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferenceObject::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferenceObject_Equals_mC83DEBBA89CBF9EF334B79634C9CA099B166DBC9 (XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferenceObject_Equals_mC83DEBBA89CBF9EF334B79634C9CA099B166DBC9_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject * L_0 = ___obj0;
		if (!((RuntimeObject *)IsInstSealed((RuntimeObject*)L_0, XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA_il2cpp_TypeInfo_var)))
		{
			goto IL_0015;
		}
	}
	{
		RuntimeObject * L_1 = ___obj0;
		bool L_2 = XRReferenceObject_Equals_m504798221B2E8A72005FD241A5C5E2A063FF37A2((XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA *)__this, ((*(XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA *)((XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA *)UnBox(L_1, XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA_il2cpp_TypeInfo_var)))), /*hidden argument*/NULL);
		return L_2;
	}

IL_0015:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool XRReferenceObject_Equals_mC83DEBBA89CBF9EF334B79634C9CA099B166DBC9_AdjustorThunk (RuntimeObject * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * _thisAdjusted = reinterpret_cast<XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA *>(__this + _offset);
	return XRReferenceObject_Equals_mC83DEBBA89CBF9EF334B79634C9CA099B166DBC9(_thisAdjusted, ___obj0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferenceObject::op_Equality(UnityEngine.XR.ARSubsystems.XRReferenceObject,UnityEngine.XR.ARSubsystems.XRReferenceObject)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferenceObject_op_Equality_mA352611067E59B117AB607BAE3CD182706C462D0 (XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  ___lhs0, XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  ___rhs1, const RuntimeMethod* method)
{
	{
		XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  L_0 = ___rhs1;
		bool L_1 = XRReferenceObject_Equals_m504798221B2E8A72005FD241A5C5E2A063FF37A2((XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return L_1;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferenceObject::op_Inequality(UnityEngine.XR.ARSubsystems.XRReferenceObject,UnityEngine.XR.ARSubsystems.XRReferenceObject)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferenceObject_op_Inequality_m14C3E4A6DDD3BD64A168898642FAC1741D542155 (XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  ___lhs0, XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  ___rhs1, const RuntimeMethod* method)
{
	{
		XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  L_0 = ___rhs1;
		bool L_1 = XRReferenceObject_Equals_m504798221B2E8A72005FD241A5C5E2A063FF37A2((XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return (bool)((((int32_t)L_1) == ((int32_t)0))? 1 : 0);
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void UnityEngine.XR.ARSubsystems.XRReferenceObjectEntry::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRReferenceObjectEntry__ctor_mB617B601F6FA34FA4DAB5E9AE50925FCBFB1B6FE (XRReferenceObjectEntry_t15A2A076A0AD9C98A46C957FAE2029EF6FB81FC8 * __this, const RuntimeMethod* method)
{
	{
		ScriptableObject__ctor_m6E2B3821A4A361556FC12E9B1C71E1D5DC002C5B(__this, /*hidden argument*/NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Int32 UnityEngine.XR.ARSubsystems.XRReferenceObjectLibrary::get_count()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRReferenceObjectLibrary_get_count_mAB2D34091CFC65B152FD3FD7095480FF5D6CC99C (XRReferenceObjectLibrary_tC9950535C1214232A691C32EDA0E95C703056260 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferenceObjectLibrary_get_count_mAB2D34091CFC65B152FD3FD7095480FF5D6CC99C_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * L_0 = __this->get_m_ReferenceObjects_6();
		NullCheck(L_0);
		int32_t L_1 = List_1_get_Count_mA303D95DAA9717D8D3148C3F20FA1B2750CE77B4_inline(L_0, /*hidden argument*/List_1_get_Count_mA303D95DAA9717D8D3148C3F20FA1B2750CE77B4_RuntimeMethod_var);
		return L_1;
	}
}
// System.Collections.Generic.List`1_Enumerator<UnityEngine.XR.ARSubsystems.XRReferenceObject> UnityEngine.XR.ARSubsystems.XRReferenceObjectLibrary::GetEnumerator()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Enumerator_t3D111B64EA07FCA753146B875D55422A5191B57F  XRReferenceObjectLibrary_GetEnumerator_m7ECEFBA77B4669ADD3EDCE0841E4FF9C79C3312D (XRReferenceObjectLibrary_tC9950535C1214232A691C32EDA0E95C703056260 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferenceObjectLibrary_GetEnumerator_m7ECEFBA77B4669ADD3EDCE0841E4FF9C79C3312D_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * L_0 = __this->get_m_ReferenceObjects_6();
		NullCheck(L_0);
		Enumerator_t3D111B64EA07FCA753146B875D55422A5191B57F  L_1 = List_1_GetEnumerator_m7ABEC2AD664AC6AC1073E546BAAFC0350626A321(L_0, /*hidden argument*/List_1_GetEnumerator_m7ABEC2AD664AC6AC1073E546BAAFC0350626A321_RuntimeMethod_var);
		return L_1;
	}
}
// UnityEngine.XR.ARSubsystems.XRReferenceObject UnityEngine.XR.ARSubsystems.XRReferenceObjectLibrary::get_Item(System.Int32)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  XRReferenceObjectLibrary_get_Item_m09F2CB2A14D830F37053B8B6E02E9659F3527996 (XRReferenceObjectLibrary_tC9950535C1214232A691C32EDA0E95C703056260 * __this, int32_t ___index0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferenceObjectLibrary_get_Item_m09F2CB2A14D830F37053B8B6E02E9659F3527996_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * L_0 = __this->get_m_ReferenceObjects_6();
		int32_t L_1 = ___index0;
		NullCheck(L_0);
		XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  L_2 = List_1_get_Item_mE2BB4CDB177C0DA1EF1FB0A174459691F207C7DF_inline(L_0, L_1, /*hidden argument*/List_1_get_Item_mE2BB4CDB177C0DA1EF1FB0A174459691F207C7DF_RuntimeMethod_var);
		return L_2;
	}
}
// System.Guid UnityEngine.XR.ARSubsystems.XRReferenceObjectLibrary::get_guid()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Guid_t  XRReferenceObjectLibrary_get_guid_mC10138A0A17DF18E104F14079063BA1C12A3DF87 (XRReferenceObjectLibrary_tC9950535C1214232A691C32EDA0E95C703056260 * __this, const RuntimeMethod* method)
{
	{
		uint64_t L_0 = __this->get_m_GuidLow_4();
		uint64_t L_1 = __this->get_m_GuidHigh_5();
		Guid_t  L_2 = GuidUtil_Compose_mF0A2DF31C9F5E45DC7786601C82B926546B021D4(L_0, L_1, /*hidden argument*/NULL);
		return L_2;
	}
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRReferenceObjectLibrary::indexOf(UnityEngine.XR.ARSubsystems.XRReferenceObject)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRReferenceObjectLibrary_indexOf_mBF56521C68737F5FBBA77684AD76A78E19C5B1F0 (XRReferenceObjectLibrary_tC9950535C1214232A691C32EDA0E95C703056260 * __this, XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  ___referenceObject0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferenceObjectLibrary_indexOf_mBF56521C68737F5FBBA77684AD76A78E19C5B1F0_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * L_0 = __this->get_m_ReferenceObjects_6();
		XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  L_1 = ___referenceObject0;
		NullCheck(L_0);
		int32_t L_2 = List_1_IndexOf_m80147D21315270B53CA3E7AE6FA3B97264231AB2(L_0, L_1, /*hidden argument*/List_1_IndexOf_m80147D21315270B53CA3E7AE6FA3B97264231AB2_RuntimeMethod_var);
		return L_2;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferenceObjectLibrary::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRReferenceObjectLibrary__ctor_mA11977E1ED613E7E7EF79C3F2494E243F619C442 (XRReferenceObjectLibrary_tC9950535C1214232A691C32EDA0E95C703056260 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferenceObjectLibrary__ctor_mA11977E1ED613E7E7EF79C3F2494E243F619C442_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * L_0 = (List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 *)il2cpp_codegen_object_new(List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79_il2cpp_TypeInfo_var);
		List_1__ctor_m370BA08A79B7950CA8E75862CD6F4921BAF6D7AB(L_0, /*hidden argument*/List_1__ctor_m370BA08A79B7950CA8E75862CD6F4921BAF6D7AB_RuntimeMethod_var);
		__this->set_m_ReferenceObjects_6(L_0);
		ScriptableObject__ctor_m6E2B3821A4A361556FC12E9B1C71E1D5DC002C5B(__this, /*hidden argument*/NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// UnityEngine.XR.ARSubsystems.XRReferencePoint UnityEngine.XR.ARSubsystems.XRReferencePoint::get_defaultValue()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  XRReferencePoint_get_defaultValue_mCCFAF4140E24AC2FDF1C8D19043E57B6BFEAC0AD (const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferencePoint_get_defaultValue_mCCFAF4140E24AC2FDF1C8D19043E57B6BFEAC0AD_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		IL2CPP_RUNTIME_CLASS_INIT(XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9_il2cpp_TypeInfo_var);
		XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  L_0 = ((XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9_StaticFields*)il2cpp_codegen_static_fields_for(XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9_il2cpp_TypeInfo_var))->get_s_Default_0();
		return L_0;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePoint::.ctor(UnityEngine.XR.ARSubsystems.TrackableId,UnityEngine.Pose,UnityEngine.XR.ARSubsystems.TrackingState,System.IntPtr)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRReferencePoint__ctor_mF823D8DC414C0B110A0C29F28668C1F3DA2B2997 (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___trackableId0, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___pose1, int32_t ___trackingState2, intptr_t ___nativePtr3, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferencePoint__ctor_mF823D8DC414C0B110A0C29F28668C1F3DA2B2997_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = ___trackableId0;
		__this->set_m_Id_1(L_0);
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_1 = ___pose1;
		__this->set_m_Pose_2(L_1);
		int32_t L_2 = ___trackingState2;
		__this->set_m_TrackingState_3(L_2);
		intptr_t L_3 = ___nativePtr3;
		__this->set_m_NativePtr_4((intptr_t)L_3);
		IL2CPP_RUNTIME_CLASS_INIT(Guid_t_il2cpp_TypeInfo_var);
		Guid_t  L_4 = ((Guid_t_StaticFields*)il2cpp_codegen_static_fields_for(Guid_t_il2cpp_TypeInfo_var))->get_Empty_0();
		__this->set_m_SessionId_5(L_4);
		return;
	}
}
IL2CPP_EXTERN_C  void XRReferencePoint__ctor_mF823D8DC414C0B110A0C29F28668C1F3DA2B2997_AdjustorThunk (RuntimeObject * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___trackableId0, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___pose1, int32_t ___trackingState2, intptr_t ___nativePtr3, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * _thisAdjusted = reinterpret_cast<XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 *>(__this + _offset);
	XRReferencePoint__ctor_mF823D8DC414C0B110A0C29F28668C1F3DA2B2997(_thisAdjusted, ___trackableId0, ___pose1, ___trackingState2, ___nativePtr3, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePoint::.ctor(UnityEngine.XR.ARSubsystems.TrackableId,UnityEngine.Pose,UnityEngine.XR.ARSubsystems.TrackingState,System.IntPtr,System.Guid)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRReferencePoint__ctor_m816965A70CDD2827DE0808A49B135E411E8532BB (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___trackableId0, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___pose1, int32_t ___trackingState2, intptr_t ___nativePtr3, Guid_t  ___sessionId4, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = ___trackableId0;
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_1 = ___pose1;
		int32_t L_2 = ___trackingState2;
		intptr_t L_3 = ___nativePtr3;
		XRReferencePoint__ctor_mF823D8DC414C0B110A0C29F28668C1F3DA2B2997((XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 *)__this, L_0, L_1, L_2, (intptr_t)L_3, /*hidden argument*/NULL);
		Guid_t  L_4 = ___sessionId4;
		__this->set_m_SessionId_5(L_4);
		return;
	}
}
IL2CPP_EXTERN_C  void XRReferencePoint__ctor_m816965A70CDD2827DE0808A49B135E411E8532BB_AdjustorThunk (RuntimeObject * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___trackableId0, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___pose1, int32_t ___trackingState2, intptr_t ___nativePtr3, Guid_t  ___sessionId4, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * _thisAdjusted = reinterpret_cast<XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 *>(__this + _offset);
	XRReferencePoint__ctor_m816965A70CDD2827DE0808A49B135E411E8532BB(_thisAdjusted, ___trackableId0, ___pose1, ___trackingState2, ___nativePtr3, ___sessionId4, method);
}
// UnityEngine.XR.ARSubsystems.TrackableId UnityEngine.XR.ARSubsystems.XRReferencePoint::get_trackableId()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  XRReferencePoint_get_trackableId_m6D53542802F2444CE58861B8868274F9A8296D88 (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = __this->get_m_Id_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C  TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  XRReferencePoint_get_trackableId_m6D53542802F2444CE58861B8868274F9A8296D88_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * _thisAdjusted = reinterpret_cast<XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 *>(__this + _offset);
	return XRReferencePoint_get_trackableId_m6D53542802F2444CE58861B8868274F9A8296D88_inline(_thisAdjusted, method);
}
// UnityEngine.Pose UnityEngine.XR.ARSubsystems.XRReferencePoint::get_pose()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  XRReferencePoint_get_pose_mA4320629B8C7AE23D97FCD8E2C5FB9C9FB6AED9C (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, const RuntimeMethod* method)
{
	{
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_0 = __this->get_m_Pose_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C  Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  XRReferencePoint_get_pose_mA4320629B8C7AE23D97FCD8E2C5FB9C9FB6AED9C_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * _thisAdjusted = reinterpret_cast<XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 *>(__this + _offset);
	return XRReferencePoint_get_pose_mA4320629B8C7AE23D97FCD8E2C5FB9C9FB6AED9C_inline(_thisAdjusted, method);
}
// UnityEngine.XR.ARSubsystems.TrackingState UnityEngine.XR.ARSubsystems.XRReferencePoint::get_trackingState()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRReferencePoint_get_trackingState_mBA0DB4050B734039D22D0ECF69CD6E8896DF52B8 (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_TrackingState_3();
		return L_0;
	}
}
IL2CPP_EXTERN_C  int32_t XRReferencePoint_get_trackingState_mBA0DB4050B734039D22D0ECF69CD6E8896DF52B8_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * _thisAdjusted = reinterpret_cast<XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 *>(__this + _offset);
	return XRReferencePoint_get_trackingState_mBA0DB4050B734039D22D0ECF69CD6E8896DF52B8_inline(_thisAdjusted, method);
}
// System.IntPtr UnityEngine.XR.ARSubsystems.XRReferencePoint::get_nativePtr()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR intptr_t XRReferencePoint_get_nativePtr_mE9EC85AD0E4976145CB0EDC4A74AA5BB076C5789 (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, const RuntimeMethod* method)
{
	{
		intptr_t L_0 = __this->get_m_NativePtr_4();
		return (intptr_t)L_0;
	}
}
IL2CPP_EXTERN_C  intptr_t XRReferencePoint_get_nativePtr_mE9EC85AD0E4976145CB0EDC4A74AA5BB076C5789_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * _thisAdjusted = reinterpret_cast<XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 *>(__this + _offset);
	return XRReferencePoint_get_nativePtr_mE9EC85AD0E4976145CB0EDC4A74AA5BB076C5789_inline(_thisAdjusted, method);
}
// System.Guid UnityEngine.XR.ARSubsystems.XRReferencePoint::get_sessionId()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Guid_t  XRReferencePoint_get_sessionId_m5DCAF1725B8A29481940252D80634C99A3C2F0D0 (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, const RuntimeMethod* method)
{
	{
		Guid_t  L_0 = __this->get_m_SessionId_5();
		return L_0;
	}
}
IL2CPP_EXTERN_C  Guid_t  XRReferencePoint_get_sessionId_m5DCAF1725B8A29481940252D80634C99A3C2F0D0_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * _thisAdjusted = reinterpret_cast<XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 *>(__this + _offset);
	return XRReferencePoint_get_sessionId_m5DCAF1725B8A29481940252D80634C99A3C2F0D0_inline(_thisAdjusted, method);
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRReferencePoint::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRReferencePoint_GetHashCode_mD7BC968C92D3CC25E7D06502570A94B104F9E32C (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 * L_0 = __this->get_address_of_m_Id_1();
		int32_t L_1 = TrackableId_GetHashCode_m89E7236D11700A1FAF335918CA65CDEB1BF4D973((TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 *)L_0, /*hidden argument*/NULL);
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 * L_2 = __this->get_address_of_m_Pose_2();
		int32_t L_3 = Pose_GetHashCode_m17AC0D28F5BD43DE0CCFA4CC1A870C525E0D6066((Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 *)L_2, /*hidden argument*/NULL);
		int32_t* L_4 = __this->get_address_of_m_TrackingState_3();
		int32_t L_5 = Int32_GetHashCode_m245C424ECE351E5FE3277A88EEB02132DAB8C25A((int32_t*)L_4, /*hidden argument*/NULL);
		intptr_t* L_6 = __this->get_address_of_m_NativePtr_4();
		int32_t L_7 = IntPtr_GetHashCode_m0A6AE0C85A4AEEA127235FB5A32056F630E3749A((intptr_t*)L_6, /*hidden argument*/NULL);
		Guid_t * L_8 = __this->get_address_of_m_SessionId_5();
		int32_t L_9 = Guid_GetHashCode_mEB01C6BA267B1CCD624BCA91D09B803C9B6E5369((Guid_t *)L_8, /*hidden argument*/NULL);
		return ((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)L_1, (int32_t)((int32_t)486187739))), (int32_t)L_3)), (int32_t)((int32_t)486187739))), (int32_t)L_5)), (int32_t)((int32_t)486187739))), (int32_t)L_7)), (int32_t)((int32_t)486187739))), (int32_t)L_9));
	}
}
IL2CPP_EXTERN_C  int32_t XRReferencePoint_GetHashCode_mD7BC968C92D3CC25E7D06502570A94B104F9E32C_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * _thisAdjusted = reinterpret_cast<XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 *>(__this + _offset);
	return XRReferencePoint_GetHashCode_mD7BC968C92D3CC25E7D06502570A94B104F9E32C(_thisAdjusted, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePoint::Equals(UnityEngine.XR.ARSubsystems.XRReferencePoint)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferencePoint_Equals_mA58F0C1C266D740037A7D6700857A5E739160AF8 (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  ___other0, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 * L_0 = __this->get_address_of_m_Id_1();
		XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  L_1 = ___other0;
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_2 = L_1.get_m_Id_1();
		bool L_3 = TrackableId_Equals_mCE458E0FDCDD6E339FCC1926EE88EB7B3D45F943((TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 *)L_0, L_2, /*hidden argument*/NULL);
		if (!L_3)
		{
			goto IL_0059;
		}
	}
	{
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 * L_4 = __this->get_address_of_m_Pose_2();
		XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  L_5 = ___other0;
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_6 = L_5.get_m_Pose_2();
		bool L_7 = Pose_Equals_m867264C8DF91FF8DC3AD957EF1625902CDEBAEDD((Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 *)L_4, L_6, /*hidden argument*/NULL);
		if (!L_7)
		{
			goto IL_0059;
		}
	}
	{
		int32_t L_8 = __this->get_m_TrackingState_3();
		XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  L_9 = ___other0;
		int32_t L_10 = L_9.get_m_TrackingState_3();
		if ((!(((uint32_t)L_8) == ((uint32_t)L_10))))
		{
			goto IL_0059;
		}
	}
	{
		intptr_t L_11 = __this->get_m_NativePtr_4();
		XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  L_12 = ___other0;
		intptr_t L_13 = L_12.get_m_NativePtr_4();
		bool L_14 = IntPtr_op_Equality_mEE8D9FD2DFE312BBAA8B4ED3BF7976B3142A5934((intptr_t)L_11, (intptr_t)L_13, /*hidden argument*/NULL);
		if (!L_14)
		{
			goto IL_0059;
		}
	}
	{
		Guid_t * L_15 = __this->get_address_of_m_SessionId_5();
		XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  L_16 = ___other0;
		Guid_t  L_17 = L_16.get_m_SessionId_5();
		bool L_18 = Guid_Equals_mC7FC66A530A8B6FC95E8F5F9E34AE81FD44CD245((Guid_t *)L_15, L_17, /*hidden argument*/NULL);
		return L_18;
	}

IL_0059:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool XRReferencePoint_Equals_mA58F0C1C266D740037A7D6700857A5E739160AF8_AdjustorThunk (RuntimeObject * __this, XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  ___other0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * _thisAdjusted = reinterpret_cast<XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 *>(__this + _offset);
	return XRReferencePoint_Equals_mA58F0C1C266D740037A7D6700857A5E739160AF8(_thisAdjusted, ___other0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePoint::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferencePoint_Equals_mD22BFD6609737E5CC6A31D2C1B519CD5207C89BC (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferencePoint_Equals_mD22BFD6609737E5CC6A31D2C1B519CD5207C89BC_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject * L_0 = ___obj0;
		if (!((RuntimeObject *)IsInstSealed((RuntimeObject*)L_0, XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9_il2cpp_TypeInfo_var)))
		{
			goto IL_0015;
		}
	}
	{
		RuntimeObject * L_1 = ___obj0;
		bool L_2 = XRReferencePoint_Equals_mA58F0C1C266D740037A7D6700857A5E739160AF8((XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 *)__this, ((*(XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 *)((XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 *)UnBox(L_1, XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9_il2cpp_TypeInfo_var)))), /*hidden argument*/NULL);
		return L_2;
	}

IL_0015:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool XRReferencePoint_Equals_mD22BFD6609737E5CC6A31D2C1B519CD5207C89BC_AdjustorThunk (RuntimeObject * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * _thisAdjusted = reinterpret_cast<XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 *>(__this + _offset);
	return XRReferencePoint_Equals_mD22BFD6609737E5CC6A31D2C1B519CD5207C89BC(_thisAdjusted, ___obj0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePoint::op_Equality(UnityEngine.XR.ARSubsystems.XRReferencePoint,UnityEngine.XR.ARSubsystems.XRReferencePoint)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferencePoint_op_Equality_mBE34F72FA0469B0D2B97620A8C7F53C01E85A8BE (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  ___lhs0, XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  ___rhs1, const RuntimeMethod* method)
{
	{
		XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  L_0 = ___rhs1;
		bool L_1 = XRReferencePoint_Equals_mA58F0C1C266D740037A7D6700857A5E739160AF8((XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return L_1;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePoint::op_Inequality(UnityEngine.XR.ARSubsystems.XRReferencePoint,UnityEngine.XR.ARSubsystems.XRReferencePoint)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferencePoint_op_Inequality_m675724084220C79C2F9A35E8D8462DD13146DDD0 (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  ___lhs0, XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  ___rhs1, const RuntimeMethod* method)
{
	{
		XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  L_0 = ___rhs1;
		bool L_1 = XRReferencePoint_Equals_mA58F0C1C266D740037A7D6700857A5E739160AF8((XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return (bool)((((int32_t)L_1) == ((int32_t)0))? 1 : 0);
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePoint::.cctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRReferencePoint__cctor_m3A7635582CA05369AD6537861329B761A106DB82 (const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferencePoint__cctor_m3A7635582CA05369AD6537861329B761A106DB82_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  V_0;
	memset((&V_0), 0, sizeof(V_0));
	{
		il2cpp_codegen_initobj((&V_0), sizeof(XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 ));
		IL2CPP_RUNTIME_CLASS_INIT(TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47_il2cpp_TypeInfo_var);
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = TrackableId_get_invalidId_mBE9FA1EC8F2EC1575C1B31666EA928A3382DF1CD_inline(/*hidden argument*/NULL);
		(&V_0)->set_m_Id_1(L_0);
		IL2CPP_RUNTIME_CLASS_INIT(Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29_il2cpp_TypeInfo_var);
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_1 = Pose_get_identity_m19458DF84EAADD5E9302CABDC385B97DC91ECCBF(/*hidden argument*/NULL);
		(&V_0)->set_m_Pose_2(L_1);
		IL2CPP_RUNTIME_CLASS_INIT(Guid_t_il2cpp_TypeInfo_var);
		Guid_t  L_2 = ((Guid_t_StaticFields*)il2cpp_codegen_static_fields_for(Guid_t_il2cpp_TypeInfo_var))->get_Empty_0();
		(&V_0)->set_m_SessionId_5(L_2);
		XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  L_3 = V_0;
		((XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9_StaticFields*)il2cpp_codegen_static_fields_for(XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9_il2cpp_TypeInfo_var))->set_s_Default_0(L_3);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRReferencePointSubsystem__ctor_mD93381DE24CA18A7BA022014E77BEBA9AA2CD6E9 (XRReferencePointSubsystem_tF175EC0188CC6EFB0A0633BA11FCA4D81A6A88E3 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferencePointSubsystem__ctor_mD93381DE24CA18A7BA022014E77BEBA9AA2CD6E9_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		TrackingSubsystem_2__ctor_mC69AD6AEF92FA2B9D7D5B83C79A418BAB05D0F76(__this, /*hidden argument*/TrackingSubsystem_2__ctor_mC69AD6AEF92FA2B9D7D5B83C79A418BAB05D0F76_RuntimeMethod_var);
		Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 * L_0 = VirtFuncInvoker0< Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 * >::Invoke(15 /* UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem/Provider UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem::CreateProvider() */, __this);
		__this->set_m_Provider_3(L_0);
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem::OnStart()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRReferencePointSubsystem_OnStart_m60B74FB6CD375125B7623A7D0D8D4B7602B53AD8 (XRReferencePointSubsystem_tF175EC0188CC6EFB0A0633BA11FCA4D81A6A88E3 * __this, const RuntimeMethod* method)
{
	{
		Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 * L_0 = __this->get_m_Provider_3();
		NullCheck(L_0);
		VirtActionInvoker0::Invoke(4 /* System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem/Provider::Start() */, L_0);
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem::OnStop()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRReferencePointSubsystem_OnStop_mB170405D1F702B112F844EF70F71B5AA54C345B3 (XRReferencePointSubsystem_tF175EC0188CC6EFB0A0633BA11FCA4D81A6A88E3 * __this, const RuntimeMethod* method)
{
	{
		Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 * L_0 = __this->get_m_Provider_3();
		NullCheck(L_0);
		VirtActionInvoker0::Invoke(5 /* System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem/Provider::Stop() */, L_0);
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem::OnDestroyed()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRReferencePointSubsystem_OnDestroyed_mCC9C916A1FA37AA028305374BF13E962E2260260 (XRReferencePointSubsystem_tF175EC0188CC6EFB0A0633BA11FCA4D81A6A88E3 * __this, const RuntimeMethod* method)
{
	{
		Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 * L_0 = __this->get_m_Provider_3();
		NullCheck(L_0);
		VirtActionInvoker0::Invoke(6 /* System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem/Provider::Destroy() */, L_0);
		return;
	}
}
// UnityEngine.XR.ARSubsystems.TrackableChanges`1<UnityEngine.XR.ARSubsystems.XRReferencePoint> UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem::GetChanges(Unity.Collections.Allocator)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR TrackableChanges_1_t5C8C3FBA23E5BBC147A6991B68520A756EF54FDF  XRReferencePointSubsystem_GetChanges_mE0EC8049CED1EA604A751066DB97430E803BE487 (XRReferencePointSubsystem_tF175EC0188CC6EFB0A0633BA11FCA4D81A6A88E3 * __this, int32_t ___allocator0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferencePointSubsystem_GetChanges_mE0EC8049CED1EA604A751066DB97430E803BE487_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		bool L_0 = VirtFuncInvoker0< bool >::Invoke(9 /* System.Boolean UnityEngine.Subsystem::get_running() */, __this);
		if (L_0)
		{
			goto IL_0013;
		}
	}
	{
		InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * L_1 = (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 *)il2cpp_codegen_object_new(InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var);
		InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706(L_1, _stringLiteralD877CA1B88372977DE10E74458237B9D64D74F9B, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_1, XRReferencePointSubsystem_GetChanges_mE0EC8049CED1EA604A751066DB97430E803BE487_RuntimeMethod_var);
	}

IL_0013:
	{
		Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 * L_2 = __this->get_m_Provider_3();
		IL2CPP_RUNTIME_CLASS_INIT(XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9_il2cpp_TypeInfo_var);
		XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  L_3 = XRReferencePoint_get_defaultValue_mCCFAF4140E24AC2FDF1C8D19043E57B6BFEAC0AD_inline(/*hidden argument*/NULL);
		int32_t L_4 = ___allocator0;
		NullCheck(L_2);
		TrackableChanges_1_t5C8C3FBA23E5BBC147A6991B68520A756EF54FDF  L_5 = VirtFuncInvoker2< TrackableChanges_1_t5C8C3FBA23E5BBC147A6991B68520A756EF54FDF , XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 , int32_t >::Invoke(7 /* UnityEngine.XR.ARSubsystems.TrackableChanges`1<UnityEngine.XR.ARSubsystems.XRReferencePoint> UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem/Provider::GetChanges(UnityEngine.XR.ARSubsystems.XRReferencePoint,Unity.Collections.Allocator) */, L_2, L_3, L_4);
		return L_5;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem::TryAddReferencePoint(UnityEngine.Pose,UnityEngine.XR.ARSubsystems.XRReferencePoint&)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferencePointSubsystem_TryAddReferencePoint_m55C922BC7F9943136A05B7E883D044CFBD5E4B87 (XRReferencePointSubsystem_tF175EC0188CC6EFB0A0633BA11FCA4D81A6A88E3 * __this, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___pose0, XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * ___referencePoint1, const RuntimeMethod* method)
{
	{
		Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 * L_0 = __this->get_m_Provider_3();
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_1 = ___pose0;
		XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * L_2 = ___referencePoint1;
		NullCheck(L_0);
		bool L_3 = VirtFuncInvoker2< bool, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 , XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * >::Invoke(8 /* System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem/Provider::TryAddReferencePoint(UnityEngine.Pose,UnityEngine.XR.ARSubsystems.XRReferencePoint&) */, L_0, L_1, (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 *)L_2);
		return L_3;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem::TryAttachReferencePoint(UnityEngine.XR.ARSubsystems.TrackableId,UnityEngine.Pose,UnityEngine.XR.ARSubsystems.XRReferencePoint&)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferencePointSubsystem_TryAttachReferencePoint_mFC09929BC0AF19465D22E30685A9C213BE434B8E (XRReferencePointSubsystem_tF175EC0188CC6EFB0A0633BA11FCA4D81A6A88E3 * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___trackableToAffix0, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___pose1, XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * ___referencePoint2, const RuntimeMethod* method)
{
	{
		Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 * L_0 = __this->get_m_Provider_3();
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_1 = ___trackableToAffix0;
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_2 = ___pose1;
		XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * L_3 = ___referencePoint2;
		NullCheck(L_0);
		bool L_4 = VirtFuncInvoker3< bool, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 , Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 , XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * >::Invoke(9 /* System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem/Provider::TryAttachReferencePoint(UnityEngine.XR.ARSubsystems.TrackableId,UnityEngine.Pose,UnityEngine.XR.ARSubsystems.XRReferencePoint&) */, L_0, L_1, L_2, (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 *)L_3);
		return L_4;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem::TryRemoveReferencePoint(UnityEngine.XR.ARSubsystems.TrackableId)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferencePointSubsystem_TryRemoveReferencePoint_m3F404A6F9DD63129EE44497554CC59CE8396DC22 (XRReferencePointSubsystem_tF175EC0188CC6EFB0A0633BA11FCA4D81A6A88E3 * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___referencePointId0, const RuntimeMethod* method)
{
	{
		Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 * L_0 = __this->get_m_Provider_3();
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_1 = ___referencePointId0;
		NullCheck(L_0);
		bool L_2 = VirtFuncInvoker1< bool, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  >::Invoke(10 /* System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem/Provider::TryRemoveReferencePoint(UnityEngine.XR.ARSubsystems.TrackableId) */, L_0, L_1);
		return L_2;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem_Provider::Start()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Provider_Start_m5E039CE52C6D7873CAE86F45CC8CCDDD10CFA738 (Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 * __this, const RuntimeMethod* method)
{
	{
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem_Provider::Stop()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Provider_Stop_m00D52CC50FC71C24B3BFC370A592A4BDC3A2E805 (Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 * __this, const RuntimeMethod* method)
{
	{
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem_Provider::Destroy()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Provider_Destroy_m181F91A509C877EEDAA0CDBCA51A03123BFA5DEF (Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 * __this, const RuntimeMethod* method)
{
	{
		return;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem_Provider::TryAddReferencePoint(UnityEngine.Pose,UnityEngine.XR.ARSubsystems.XRReferencePoint&)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Provider_TryAddReferencePoint_m24758900098738D227C197B63C5BB1A4D6E48599 (Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 * __this, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___pose0, XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * ___referencePoint1, const RuntimeMethod* method)
{
	{
		XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * L_0 = ___referencePoint1;
		il2cpp_codegen_initobj(L_0, sizeof(XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 ));
		return (bool)0;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem_Provider::TryAttachReferencePoint(UnityEngine.XR.ARSubsystems.TrackableId,UnityEngine.Pose,UnityEngine.XR.ARSubsystems.XRReferencePoint&)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Provider_TryAttachReferencePoint_m220B48C6D25FCB8C834B21CB58D2C45C87212180 (Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___trackableToAffix0, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___pose1, XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * ___referencePoint2, const RuntimeMethod* method)
{
	{
		XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * L_0 = ___referencePoint2;
		il2cpp_codegen_initobj(L_0, sizeof(XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 ));
		return (bool)0;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem_Provider::TryRemoveReferencePoint(UnityEngine.XR.ARSubsystems.TrackableId)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Provider_TryRemoveReferencePoint_m1DE094DA1EBC860FB65C6B200F2CA79940239A80 (Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___referencePointId0, const RuntimeMethod* method)
{
	{
		return (bool)0;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystem_Provider::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Provider__ctor_mCAD1B3B1509E0232266D304B2CAB429D16C3359E (Provider_t807BCBCDFFF7A549734792EE0C1A0E333431A773 * __this, const RuntimeMethod* method)
{
	{
		Object__ctor_m925ECA5E85CA100E3FB86A4F9E15C120E9A184C0(__this, /*hidden argument*/NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor::get_supportsTrackableAttachments()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRReferencePointSubsystemDescriptor_get_supportsTrackableAttachments_mA94E2928B96D7F5CC69B8D225F8FADDCDC80922D (XRReferencePointSubsystemDescriptor_t5972C5E3DEF485A89A3AA7D9D47CCD8A67FDB74D * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = __this->get_U3CsupportsTrackableAttachmentsU3Ek__BackingField_2();
		return L_0;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor::set_supportsTrackableAttachments(System.Boolean)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRReferencePointSubsystemDescriptor_set_supportsTrackableAttachments_mA4F7709D4C170D414F862490ABBF0090DE46A8AB (XRReferencePointSubsystemDescriptor_t5972C5E3DEF485A89A3AA7D9D47CCD8A67FDB74D * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsTrackableAttachmentsU3Ek__BackingField_2(L_0);
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor::Create(UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRReferencePointSubsystemDescriptor_Create_m4E9D9DF5FCE2FE3F8672653BC733F87D3A4327D0 (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD  ___cinfo0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferencePointSubsystemDescriptor_Create_m4E9D9DF5FCE2FE3F8672653BC733F87D3A4327D0_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD  L_0 = ___cinfo0;
		XRReferencePointSubsystemDescriptor_t5972C5E3DEF485A89A3AA7D9D47CCD8A67FDB74D * L_1 = (XRReferencePointSubsystemDescriptor_t5972C5E3DEF485A89A3AA7D9D47CCD8A67FDB74D *)il2cpp_codegen_object_new(XRReferencePointSubsystemDescriptor_t5972C5E3DEF485A89A3AA7D9D47CCD8A67FDB74D_il2cpp_TypeInfo_var);
		XRReferencePointSubsystemDescriptor__ctor_mB5AD6FF2521FF148612D7662DF93BD6CA68069B2(L_1, L_0, /*hidden argument*/NULL);
		IL2CPP_RUNTIME_CLASS_INIT(SubsystemRegistration_t0A22FECC46483ABBFFC039449407F73FF11F5A1A_il2cpp_TypeInfo_var);
		SubsystemRegistration_CreateDescriptor_m90E54AA3B7A6EC958B51A4775DC75DA4149F9DF2(L_1, /*hidden argument*/NULL);
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor::.ctor(UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRReferencePointSubsystemDescriptor__ctor_mB5AD6FF2521FF148612D7662DF93BD6CA68069B2 (XRReferencePointSubsystemDescriptor_t5972C5E3DEF485A89A3AA7D9D47CCD8A67FDB74D * __this, Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD  ___cinfo0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferencePointSubsystemDescriptor__ctor_mB5AD6FF2521FF148612D7662DF93BD6CA68069B2_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		SubsystemDescriptor_1__ctor_m39ABF0C7663CF93F6B094FB18264078421C6950F(__this, /*hidden argument*/SubsystemDescriptor_1__ctor_m39ABF0C7663CF93F6B094FB18264078421C6950F_RuntimeMethod_var);
		String_t* L_0 = Cinfo_get_id_mB1E35C0B52EEAA8EB934C4D3F02465CF8A752015_inline((Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *)(&___cinfo0), /*hidden argument*/NULL);
		SubsystemDescriptor_set_id_m3289D5DA5921130F0338C6332907E686E36FF064_inline(__this, L_0, /*hidden argument*/NULL);
		Type_t * L_1 = Cinfo_get_subsystemImplementationType_m764697AF3D79BDFA6010287A8B542F9323693096_inline((Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *)(&___cinfo0), /*hidden argument*/NULL);
		SubsystemDescriptor_set_subsystemImplementationType_m727AB6BED84E57A382B8804F95A75A8012963EE1_inline(__this, L_1, /*hidden argument*/NULL);
		bool L_2 = Cinfo_get_supportsTrackableAttachments_m041B030B1BD0114D8FCE9A9F804CFF5984FB07BD_inline((Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *)(&___cinfo0), /*hidden argument*/NULL);
		XRReferencePointSubsystemDescriptor_set_supportsTrackableAttachments_mA4F7709D4C170D414F862490ABBF0090DE46A8AB_inline(__this, L_2, /*hidden argument*/NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Conversion methods for marshalling of: UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor/Cinfo
IL2CPP_EXTERN_C void Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD_marshal_pinvoke(const Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD& unmarshaled, Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD_marshaled_pinvoke& marshaled)
{
	Exception_t* ___U3CsubsystemImplementationTypeU3Ek__BackingField_1Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field '<subsystemImplementationType>k__BackingField' of type 'Cinfo': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___U3CsubsystemImplementationTypeU3Ek__BackingField_1Exception, NULL);
}
IL2CPP_EXTERN_C void Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD_marshal_pinvoke_back(const Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD_marshaled_pinvoke& marshaled, Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD& unmarshaled)
{
	Exception_t* ___U3CsubsystemImplementationTypeU3Ek__BackingField_1Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field '<subsystemImplementationType>k__BackingField' of type 'Cinfo': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___U3CsubsystemImplementationTypeU3Ek__BackingField_1Exception, NULL);
}
// Conversion method for clean up from marshalling of: UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor/Cinfo
IL2CPP_EXTERN_C void Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD_marshal_pinvoke_cleanup(Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD_marshaled_pinvoke& marshaled)
{
}
// Conversion methods for marshalling of: UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor/Cinfo
IL2CPP_EXTERN_C void Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD_marshal_com(const Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD& unmarshaled, Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD_marshaled_com& marshaled)
{
	Exception_t* ___U3CsubsystemImplementationTypeU3Ek__BackingField_1Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field '<subsystemImplementationType>k__BackingField' of type 'Cinfo': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___U3CsubsystemImplementationTypeU3Ek__BackingField_1Exception, NULL);
}
IL2CPP_EXTERN_C void Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD_marshal_com_back(const Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD_marshaled_com& marshaled, Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD& unmarshaled)
{
	Exception_t* ___U3CsubsystemImplementationTypeU3Ek__BackingField_1Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field '<subsystemImplementationType>k__BackingField' of type 'Cinfo': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___U3CsubsystemImplementationTypeU3Ek__BackingField_1Exception, NULL);
}
// Conversion method for clean up from marshalling of: UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor/Cinfo
IL2CPP_EXTERN_C void Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD_marshal_com_cleanup(Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD_marshaled_com& marshaled)
{
}
// System.String UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo::get_id()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Cinfo_get_id_mB1E35C0B52EEAA8EB934C4D3F02465CF8A752015 (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, const RuntimeMethod* method)
{
	{
		String_t* L_0 = __this->get_U3CidU3Ek__BackingField_0();
		return L_0;
	}
}
IL2CPP_EXTERN_C  String_t* Cinfo_get_id_mB1E35C0B52EEAA8EB934C4D3F02465CF8A752015_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * _thisAdjusted = reinterpret_cast<Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *>(__this + _offset);
	return Cinfo_get_id_mB1E35C0B52EEAA8EB934C4D3F02465CF8A752015_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo::set_id(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Cinfo_set_id_m9CAE75E21B0DAE38A8619D1B04D17EDEC81E97D7 (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, String_t* ___value0, const RuntimeMethod* method)
{
	{
		String_t* L_0 = ___value0;
		__this->set_U3CidU3Ek__BackingField_0(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void Cinfo_set_id_m9CAE75E21B0DAE38A8619D1B04D17EDEC81E97D7_AdjustorThunk (RuntimeObject * __this, String_t* ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * _thisAdjusted = reinterpret_cast<Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *>(__this + _offset);
	Cinfo_set_id_m9CAE75E21B0DAE38A8619D1B04D17EDEC81E97D7_inline(_thisAdjusted, ___value0, method);
}
// System.Type UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo::get_subsystemImplementationType()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Type_t * Cinfo_get_subsystemImplementationType_m764697AF3D79BDFA6010287A8B542F9323693096 (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, const RuntimeMethod* method)
{
	{
		Type_t * L_0 = __this->get_U3CsubsystemImplementationTypeU3Ek__BackingField_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C  Type_t * Cinfo_get_subsystemImplementationType_m764697AF3D79BDFA6010287A8B542F9323693096_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * _thisAdjusted = reinterpret_cast<Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *>(__this + _offset);
	return Cinfo_get_subsystemImplementationType_m764697AF3D79BDFA6010287A8B542F9323693096_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo::set_subsystemImplementationType(System.Type)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Cinfo_set_subsystemImplementationType_m9D5112215C6766E6561E42DA858B7F3D72F0005E (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, Type_t * ___value0, const RuntimeMethod* method)
{
	{
		Type_t * L_0 = ___value0;
		__this->set_U3CsubsystemImplementationTypeU3Ek__BackingField_1(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void Cinfo_set_subsystemImplementationType_m9D5112215C6766E6561E42DA858B7F3D72F0005E_AdjustorThunk (RuntimeObject * __this, Type_t * ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * _thisAdjusted = reinterpret_cast<Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *>(__this + _offset);
	Cinfo_set_subsystemImplementationType_m9D5112215C6766E6561E42DA858B7F3D72F0005E_inline(_thisAdjusted, ___value0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo::get_supportsTrackableAttachments()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_get_supportsTrackableAttachments_m041B030B1BD0114D8FCE9A9F804CFF5984FB07BD (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = __this->get_U3CsupportsTrackableAttachmentsU3Ek__BackingField_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C  bool Cinfo_get_supportsTrackableAttachments_m041B030B1BD0114D8FCE9A9F804CFF5984FB07BD_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * _thisAdjusted = reinterpret_cast<Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *>(__this + _offset);
	return Cinfo_get_supportsTrackableAttachments_m041B030B1BD0114D8FCE9A9F804CFF5984FB07BD_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo::set_supportsTrackableAttachments(System.Boolean)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Cinfo_set_supportsTrackableAttachments_mC0A061EDA609485B0E83FC7857E8573C93F38FD7 (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsTrackableAttachmentsU3Ek__BackingField_2(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void Cinfo_set_supportsTrackableAttachments_mC0A061EDA609485B0E83FC7857E8573C93F38FD7_AdjustorThunk (RuntimeObject * __this, bool ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * _thisAdjusted = reinterpret_cast<Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *>(__this + _offset);
	Cinfo_set_supportsTrackableAttachments_mC0A061EDA609485B0E83FC7857E8573C93F38FD7_inline(_thisAdjusted, ___value0, method);
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Cinfo_GetHashCode_mA1002FD6C9DDB0C39442B7692A7CDB5562C61086 (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (Cinfo_GetHashCode_mA1002FD6C9DDB0C39442B7692A7CDB5562C61086_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	int32_t G_B3_0 = 0;
	int32_t G_B5_0 = 0;
	int32_t G_B4_0 = 0;
	int32_t G_B6_0 = 0;
	int32_t G_B6_1 = 0;
	{
		String_t* L_0 = Cinfo_get_id_mB1E35C0B52EEAA8EB934C4D3F02465CF8A752015_inline((Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *)__this, /*hidden argument*/NULL);
		if (!L_0)
		{
			goto IL_0015;
		}
	}
	{
		String_t* L_1 = Cinfo_get_id_mB1E35C0B52EEAA8EB934C4D3F02465CF8A752015_inline((Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *)__this, /*hidden argument*/NULL);
		NullCheck(L_1);
		int32_t L_2 = VirtFuncInvoker0< int32_t >::Invoke(2 /* System.Int32 System.Object::GetHashCode() */, L_1);
		G_B3_0 = L_2;
		goto IL_0016;
	}

IL_0015:
	{
		G_B3_0 = 0;
	}

IL_0016:
	{
		Type_t * L_3 = Cinfo_get_subsystemImplementationType_m764697AF3D79BDFA6010287A8B542F9323693096_inline((Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *)__this, /*hidden argument*/NULL);
		IL2CPP_RUNTIME_CLASS_INIT(Type_t_il2cpp_TypeInfo_var);
		bool L_4 = Type_op_Equality_m7040622C9E1037EFC73E1F0EDB1DD241282BE3D8(L_3, (Type_t *)NULL, /*hidden argument*/NULL);
		G_B4_0 = ((int32_t)il2cpp_codegen_multiply((int32_t)G_B3_0, (int32_t)((int32_t)486187739)));
		if (L_4)
		{
			G_B5_0 = ((int32_t)il2cpp_codegen_multiply((int32_t)G_B3_0, (int32_t)((int32_t)486187739)));
			goto IL_0037;
		}
	}
	{
		Type_t * L_5 = Cinfo_get_subsystemImplementationType_m764697AF3D79BDFA6010287A8B542F9323693096_inline((Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *)__this, /*hidden argument*/NULL);
		NullCheck(L_5);
		int32_t L_6 = VirtFuncInvoker0< int32_t >::Invoke(2 /* System.Int32 System.Object::GetHashCode() */, L_5);
		G_B6_0 = L_6;
		G_B6_1 = G_B4_0;
		goto IL_0038;
	}

IL_0037:
	{
		G_B6_0 = 0;
		G_B6_1 = G_B5_0;
	}

IL_0038:
	{
		return ((int32_t)il2cpp_codegen_add((int32_t)G_B6_1, (int32_t)G_B6_0));
	}
}
IL2CPP_EXTERN_C  int32_t Cinfo_GetHashCode_mA1002FD6C9DDB0C39442B7692A7CDB5562C61086_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * _thisAdjusted = reinterpret_cast<Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *>(__this + _offset);
	return Cinfo_GetHashCode_mA1002FD6C9DDB0C39442B7692A7CDB5562C61086(_thisAdjusted, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_Equals_mC210DE78D45CD980BF7B5A186241CFC5CC243D2E (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (Cinfo_Equals_mC210DE78D45CD980BF7B5A186241CFC5CC243D2E_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject * L_0 = ___obj0;
		if (((RuntimeObject *)IsInstSealed((RuntimeObject*)L_0, Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD_il2cpp_TypeInfo_var)))
		{
			goto IL_000a;
		}
	}
	{
		return (bool)0;
	}

IL_000a:
	{
		RuntimeObject * L_1 = ___obj0;
		bool L_2 = Cinfo_Equals_m7D9C657E628D2DFC390587FBE0BDFEB1D5CDC92D((Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *)__this, ((*(Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *)((Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *)UnBox(L_1, Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD_il2cpp_TypeInfo_var)))), /*hidden argument*/NULL);
		return L_2;
	}
}
IL2CPP_EXTERN_C  bool Cinfo_Equals_mC210DE78D45CD980BF7B5A186241CFC5CC243D2E_AdjustorThunk (RuntimeObject * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * _thisAdjusted = reinterpret_cast<Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *>(__this + _offset);
	return Cinfo_Equals_mC210DE78D45CD980BF7B5A186241CFC5CC243D2E(_thisAdjusted, ___obj0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo::Equals(UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_Equals_m7D9C657E628D2DFC390587FBE0BDFEB1D5CDC92D (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD  ___other0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (Cinfo_Equals_m7D9C657E628D2DFC390587FBE0BDFEB1D5CDC92D_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		String_t* L_0 = Cinfo_get_id_mB1E35C0B52EEAA8EB934C4D3F02465CF8A752015_inline((Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *)__this, /*hidden argument*/NULL);
		String_t* L_1 = Cinfo_get_id_mB1E35C0B52EEAA8EB934C4D3F02465CF8A752015_inline((Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *)(&___other0), /*hidden argument*/NULL);
		bool L_2 = String_Equals_m90EB651A751C3444BADBBD5401109CE05B3E1CFB(L_0, L_1, /*hidden argument*/NULL);
		if (!L_2)
		{
			goto IL_0027;
		}
	}
	{
		Type_t * L_3 = Cinfo_get_subsystemImplementationType_m764697AF3D79BDFA6010287A8B542F9323693096_inline((Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *)__this, /*hidden argument*/NULL);
		Type_t * L_4 = Cinfo_get_subsystemImplementationType_m764697AF3D79BDFA6010287A8B542F9323693096_inline((Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *)(&___other0), /*hidden argument*/NULL);
		IL2CPP_RUNTIME_CLASS_INIT(Type_t_il2cpp_TypeInfo_var);
		bool L_5 = Type_op_Equality_m7040622C9E1037EFC73E1F0EDB1DD241282BE3D8(L_3, L_4, /*hidden argument*/NULL);
		return L_5;
	}

IL_0027:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool Cinfo_Equals_m7D9C657E628D2DFC390587FBE0BDFEB1D5CDC92D_AdjustorThunk (RuntimeObject * __this, Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD  ___other0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * _thisAdjusted = reinterpret_cast<Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *>(__this + _offset);
	return Cinfo_Equals_m7D9C657E628D2DFC390587FBE0BDFEB1D5CDC92D(_thisAdjusted, ___other0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo::op_Equality(UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo,UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_op_Equality_mE1F2D9B59D8CE1464461320ED675AF909582610A (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD  ___lhs0, Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD  ___rhs1, const RuntimeMethod* method)
{
	{
		Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD  L_0 = ___rhs1;
		bool L_1 = Cinfo_Equals_m7D9C657E628D2DFC390587FBE0BDFEB1D5CDC92D((Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return L_1;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo::op_Inequality(UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo,UnityEngine.XR.ARSubsystems.XRReferencePointSubsystemDescriptor_Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_op_Inequality_m434589D57A438F397CD363A5E2CB491095725A53 (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD  ___lhs0, Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD  ___rhs1, const RuntimeMethod* method)
{
	{
		Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD  L_0 = ___rhs1;
		bool L_1 = Cinfo_Equals_m7D9C657E628D2DFC390587FBE0BDFEB1D5CDC92D((Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return (bool)((((int32_t)L_1) == ((int32_t)0))? 1 : 0);
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.IntPtr UnityEngine.XR.ARSubsystems.XRSessionSubsystem::get_nativePtr()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR intptr_t XRSessionSubsystem_get_nativePtr_m0F00EE85A23E2FBE08AE83393F4C7DC97C22366B (XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0 * __this, const RuntimeMethod* method)
{
	{
		Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * L_0 = __this->get_m_Provider_3();
		NullCheck(L_0);
		intptr_t L_1 = VirtFuncInvoker0< intptr_t >::Invoke(11 /* System.IntPtr UnityEngine.XR.ARSubsystems.XRSessionSubsystem/Provider::get_nativePtr() */, L_0);
		return (intptr_t)L_1;
	}
}
// System.Guid UnityEngine.XR.ARSubsystems.XRSessionSubsystem::get_sessionId()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Guid_t  XRSessionSubsystem_get_sessionId_m830EF72639051E3486DA85FE6E4EF1C9AD3481E1 (XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0 * __this, const RuntimeMethod* method)
{
	{
		Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * L_0 = __this->get_m_Provider_3();
		NullCheck(L_0);
		Guid_t  L_1 = VirtFuncInvoker0< Guid_t  >::Invoke(16 /* System.Guid UnityEngine.XR.ARSubsystems.XRSessionSubsystem/Provider::get_sessionId() */, L_0);
		return L_1;
	}
}
// UnityEngine.XR.ARSubsystems.Promise`1<UnityEngine.XR.ARSubsystems.SessionAvailability> UnityEngine.XR.ARSubsystems.XRSessionSubsystem::GetAvailabilityAsync()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Promise_1_t7E6FC51116CF1FB8FFF9D4D6621E5F7231AE9A6B * XRSessionSubsystem_GetAvailabilityAsync_mE1444BD33C0A1EAD4982FC0AE64D1251635487ED (XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0 * __this, const RuntimeMethod* method)
{
	{
		Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * L_0 = __this->get_m_Provider_3();
		NullCheck(L_0);
		Promise_1_t7E6FC51116CF1FB8FFF9D4D6621E5F7231AE9A6B * L_1 = VirtFuncInvoker0< Promise_1_t7E6FC51116CF1FB8FFF9D4D6621E5F7231AE9A6B * >::Invoke(12 /* UnityEngine.XR.ARSubsystems.Promise`1<UnityEngine.XR.ARSubsystems.SessionAvailability> UnityEngine.XR.ARSubsystems.XRSessionSubsystem/Provider::GetAvailabilityAsync() */, L_0);
		return L_1;
	}
}
// UnityEngine.XR.ARSubsystems.Promise`1<UnityEngine.XR.ARSubsystems.SessionInstallationStatus> UnityEngine.XR.ARSubsystems.XRSessionSubsystem::InstallAsync()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Promise_1_tE8B65103AAD59FA1265B1977CE59DC6DA5220626 * XRSessionSubsystem_InstallAsync_m35E08EF7130491F2E498C990109FA7323A2ABCCC (XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRSessionSubsystem_InstallAsync_m35E08EF7130491F2E498C990109FA7323A2ABCCC_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079 * L_0 = Subsystem_1_get_SubsystemDescriptor_mD9F3F4ECB180FF40D21E57235A6D775BDB014D9B(__this, /*hidden argument*/Subsystem_1_get_SubsystemDescriptor_mD9F3F4ECB180FF40D21E57235A6D775BDB014D9B_RuntimeMethod_var);
		NullCheck(L_0);
		bool L_1 = XRSessionSubsystemDescriptor_get_supportsInstall_m2AA89682007FE1D8BB811FD152DE326FF7BB5A99_inline(L_0, /*hidden argument*/NULL);
		if (L_1)
		{
			goto IL_0018;
		}
	}
	{
		NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 * L_2 = (NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 *)il2cpp_codegen_object_new(NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010_il2cpp_TypeInfo_var);
		NotSupportedException__ctor_mD023A89A5C1F740F43F0A9CD6C49DC21230B3CEE(L_2, _stringLiteral39BF4FB45444043D19EFF93848D9F1A0BEA05065, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_2, XRSessionSubsystem_InstallAsync_m35E08EF7130491F2E498C990109FA7323A2ABCCC_RuntimeMethod_var);
	}

IL_0018:
	{
		Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * L_3 = __this->get_m_Provider_3();
		NullCheck(L_3);
		Promise_1_tE8B65103AAD59FA1265B1977CE59DC6DA5220626 * L_4 = VirtFuncInvoker0< Promise_1_tE8B65103AAD59FA1265B1977CE59DC6DA5220626 * >::Invoke(13 /* UnityEngine.XR.ARSubsystems.Promise`1<UnityEngine.XR.ARSubsystems.SessionInstallationStatus> UnityEngine.XR.ARSubsystems.XRSessionSubsystem/Provider::InstallAsync() */, L_3);
		return L_4;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRSessionSubsystem__ctor_m2817E6FDD974187708CFD270DE4C6D3132774648 (XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRSessionSubsystem__ctor_m2817E6FDD974187708CFD270DE4C6D3132774648_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		XRSubsystem_1__ctor_m3D9DD5E9EFF90C8BD6855FB1E5FED02E410B19E6(__this, /*hidden argument*/XRSubsystem_1__ctor_m3D9DD5E9EFF90C8BD6855FB1E5FED02E410B19E6_RuntimeMethod_var);
		Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * L_0 = VirtFuncInvoker0< Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * >::Invoke(14 /* UnityEngine.XR.ARSubsystems.XRSessionSubsystem/Provider UnityEngine.XR.ARSubsystems.XRSessionSubsystem::CreateProvider() */, __this);
		__this->set_m_Provider_3(L_0);
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem::OnStart()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRSessionSubsystem_OnStart_mCE65AF851F73EEE76E058AD66DA6E3E355C3FFCB (XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0 * __this, const RuntimeMethod* method)
{
	{
		Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * L_0 = __this->get_m_Provider_3();
		NullCheck(L_0);
		VirtActionInvoker0::Invoke(4 /* System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem/Provider::Resume() */, L_0);
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem::Reset()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRSessionSubsystem_Reset_mA6596EEA8C670E2561986B54BB34E0F15A0D5836 (XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0 * __this, const RuntimeMethod* method)
{
	{
		Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * L_0 = __this->get_m_Provider_3();
		NullCheck(L_0);
		VirtActionInvoker0::Invoke(8 /* System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem/Provider::Reset() */, L_0);
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem::OnStop()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRSessionSubsystem_OnStop_mC995FD70B27E089A98FEE62DAFB6642521F90F33 (XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0 * __this, const RuntimeMethod* method)
{
	{
		Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * L_0 = __this->get_m_Provider_3();
		NullCheck(L_0);
		VirtActionInvoker0::Invoke(5 /* System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem/Provider::Pause() */, L_0);
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem::OnDestroyed()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRSessionSubsystem_OnDestroyed_mE818CF27CDBBA6E22713A186B50C7BED4A77CBCA (XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0 * __this, const RuntimeMethod* method)
{
	{
		Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * L_0 = __this->get_m_Provider_3();
		NullCheck(L_0);
		VirtActionInvoker0::Invoke(7 /* System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem/Provider::Destroy() */, L_0);
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem::Update(UnityEngine.XR.ARSubsystems.XRSessionUpdateParams)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRSessionSubsystem_Update_m168E0641976ED5CD2084BD32692044A2D361B945 (XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0 * __this, XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16  ___updateParams0, const RuntimeMethod* method)
{
	{
		Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * L_0 = __this->get_m_Provider_3();
		XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16  L_1 = ___updateParams0;
		NullCheck(L_0);
		VirtActionInvoker1< XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16  >::Invoke(6 /* System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem/Provider::Update(UnityEngine.XR.ARSubsystems.XRSessionUpdateParams) */, L_0, L_1);
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem::OnApplicationPause()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRSessionSubsystem_OnApplicationPause_mE53A32290C253F808E5B14A11B7917286E86B08A (XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0 * __this, const RuntimeMethod* method)
{
	{
		Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * L_0 = __this->get_m_Provider_3();
		NullCheck(L_0);
		VirtActionInvoker0::Invoke(9 /* System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem/Provider::OnApplicationPause() */, L_0);
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem::OnApplicationResume()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRSessionSubsystem_OnApplicationResume_m4103D0866A4152C9A52E52A306D85EBAD8405F2F (XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0 * __this, const RuntimeMethod* method)
{
	{
		Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * L_0 = __this->get_m_Provider_3();
		NullCheck(L_0);
		VirtActionInvoker0::Invoke(10 /* System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem/Provider::OnApplicationResume() */, L_0);
		return;
	}
}
// UnityEngine.XR.ARSubsystems.TrackingState UnityEngine.XR.ARSubsystems.XRSessionSubsystem::get_trackingState()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRSessionSubsystem_get_trackingState_m6CEDC16CB9B224A0302A83BC2C22FC4C0905EB30 (XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0 * __this, const RuntimeMethod* method)
{
	{
		Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * L_0 = __this->get_m_Provider_3();
		NullCheck(L_0);
		int32_t L_1 = VirtFuncInvoker0< int32_t >::Invoke(14 /* UnityEngine.XR.ARSubsystems.TrackingState UnityEngine.XR.ARSubsystems.XRSessionSubsystem/Provider::get_trackingState() */, L_0);
		return L_1;
	}
}
// UnityEngine.XR.ARSubsystems.NotTrackingReason UnityEngine.XR.ARSubsystems.XRSessionSubsystem::get_notTrackingReason()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRSessionSubsystem_get_notTrackingReason_m2425113BCCDD44CEF92AA9A045C002CAF981B6D7 (XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0 * __this, const RuntimeMethod* method)
{
	{
		Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * L_0 = __this->get_m_Provider_3();
		NullCheck(L_0);
		int32_t L_1 = VirtFuncInvoker0< int32_t >::Invoke(15 /* UnityEngine.XR.ARSubsystems.NotTrackingReason UnityEngine.XR.ARSubsystems.XRSessionSubsystem/Provider::get_notTrackingReason() */, L_0);
		return L_1;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystem::get_matchFrameRate()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRSessionSubsystem_get_matchFrameRate_m1A5153DC073507175C7D0E47FC649DBB690FEEEC (XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0 * __this, const RuntimeMethod* method)
{
	{
		Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * L_0 = __this->get_m_Provider_3();
		NullCheck(L_0);
		bool L_1 = VirtFuncInvoker0< bool >::Invoke(17 /* System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystem/Provider::get_matchFrameRate() */, L_0);
		return L_1;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem::set_matchFrameRate(System.Boolean)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRSessionSubsystem_set_matchFrameRate_m5AE7252065F093953386D31D79BCA9C1C7BC256A (XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0 * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * L_0 = __this->get_m_Provider_3();
		bool L_1 = ___value0;
		NullCheck(L_0);
		VirtActionInvoker1< bool >::Invoke(18 /* System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem/Provider::set_matchFrameRate(System.Boolean) */, L_0, L_1);
		return;
	}
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRSessionSubsystem::get_frameRate()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRSessionSubsystem_get_frameRate_m9C029A08839E039C3459DF5CE4A5E5CFAD3DC7DE (XRSessionSubsystem_t9B9C16B4BDB611559FB6FA728BE399001E47EFF0 * __this, const RuntimeMethod* method)
{
	{
		Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * L_0 = __this->get_m_Provider_3();
		NullCheck(L_0);
		int32_t L_1 = VirtFuncInvoker0< int32_t >::Invoke(19 /* System.Int32 UnityEngine.XR.ARSubsystems.XRSessionSubsystem/Provider::get_frameRate() */, L_0);
		return L_1;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem_Provider::Resume()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Provider_Resume_m8CAB34CF062DD4D2BE34F606AA14F9F78BB6904E (Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * __this, const RuntimeMethod* method)
{
	{
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem_Provider::Pause()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Provider_Pause_m29AA9017C4F53BEC8B24467A7C469512F421A97A (Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * __this, const RuntimeMethod* method)
{
	{
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem_Provider::Update(UnityEngine.XR.ARSubsystems.XRSessionUpdateParams)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Provider_Update_m3C7FA8783EB5ED76A0A022CB87D840921D94941E (Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * __this, XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16  ___updateParams0, const RuntimeMethod* method)
{
	{
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem_Provider::Destroy()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Provider_Destroy_m0BFF3C770F8D3DDEDB8BBE1E3E540B33722CF8AC (Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * __this, const RuntimeMethod* method)
{
	{
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem_Provider::Reset()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Provider_Reset_m43A334CBFFF6A9D9FC404A0FD5DC05A41CE9CAE1 (Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * __this, const RuntimeMethod* method)
{
	{
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem_Provider::OnApplicationPause()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Provider_OnApplicationPause_m02759BAEDA12223C11B002BE7CEAA14C3308C4AD (Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * __this, const RuntimeMethod* method)
{
	{
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem_Provider::OnApplicationResume()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Provider_OnApplicationResume_mD415E31314285E9277FF8C9B5F3B39E34417B736 (Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * __this, const RuntimeMethod* method)
{
	{
		return;
	}
}
// System.IntPtr UnityEngine.XR.ARSubsystems.XRSessionSubsystem_Provider::get_nativePtr()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR intptr_t Provider_get_nativePtr_mEEB293C5FF1BBA91207EABA24752E9DF7A715609 (Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (Provider_get_nativePtr_mEEB293C5FF1BBA91207EABA24752E9DF7A715609_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		return (intptr_t)(0);
	}
}
// UnityEngine.XR.ARSubsystems.Promise`1<UnityEngine.XR.ARSubsystems.SessionAvailability> UnityEngine.XR.ARSubsystems.XRSessionSubsystem_Provider::GetAvailabilityAsync()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Promise_1_t7E6FC51116CF1FB8FFF9D4D6621E5F7231AE9A6B * Provider_GetAvailabilityAsync_m9CC6F74169601931E94DA1177C34A543048B0A01 (Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (Provider_GetAvailabilityAsync_m9CC6F74169601931E94DA1177C34A543048B0A01_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		Promise_1_t7E6FC51116CF1FB8FFF9D4D6621E5F7231AE9A6B * L_0 = Promise_1_CreateResolvedPromise_m36CC1CB8A654C9849F7BB9FE6C69A725C78CC01D(0, /*hidden argument*/Promise_1_CreateResolvedPromise_m36CC1CB8A654C9849F7BB9FE6C69A725C78CC01D_RuntimeMethod_var);
		return L_0;
	}
}
// UnityEngine.XR.ARSubsystems.Promise`1<UnityEngine.XR.ARSubsystems.SessionInstallationStatus> UnityEngine.XR.ARSubsystems.XRSessionSubsystem_Provider::InstallAsync()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Promise_1_tE8B65103AAD59FA1265B1977CE59DC6DA5220626 * Provider_InstallAsync_m074282442B17B260C246A17F8D7E1C4E947A6017 (Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (Provider_InstallAsync_m074282442B17B260C246A17F8D7E1C4E947A6017_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		Promise_1_tE8B65103AAD59FA1265B1977CE59DC6DA5220626 * L_0 = Promise_1_CreateResolvedPromise_m33AA652B2B2460A4777ED6EA73923156280826AA(4, /*hidden argument*/Promise_1_CreateResolvedPromise_m33AA652B2B2460A4777ED6EA73923156280826AA_RuntimeMethod_var);
		return L_0;
	}
}
// UnityEngine.XR.ARSubsystems.TrackingState UnityEngine.XR.ARSubsystems.XRSessionSubsystem_Provider::get_trackingState()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Provider_get_trackingState_m9E8D77D2BA6BD8F3508CFA530482C6D790DCFF35 (Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * __this, const RuntimeMethod* method)
{
	{
		return (int32_t)(0);
	}
}
// UnityEngine.XR.ARSubsystems.NotTrackingReason UnityEngine.XR.ARSubsystems.XRSessionSubsystem_Provider::get_notTrackingReason()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Provider_get_notTrackingReason_m3EF060E8F7A23B6CD196E1B7FCF2F34457B065AA (Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * __this, const RuntimeMethod* method)
{
	{
		return (int32_t)(6);
	}
}
// System.Guid UnityEngine.XR.ARSubsystems.XRSessionSubsystem_Provider::get_sessionId()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Guid_t  Provider_get_sessionId_mD3AED2746B5F6920BD6BCC4234D6B9189CCF4BFA (Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (Provider_get_sessionId_mD3AED2746B5F6920BD6BCC4234D6B9189CCF4BFA_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		IL2CPP_RUNTIME_CLASS_INIT(Guid_t_il2cpp_TypeInfo_var);
		Guid_t  L_0 = ((Guid_t_StaticFields*)il2cpp_codegen_static_fields_for(Guid_t_il2cpp_TypeInfo_var))->get_Empty_0();
		return L_0;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystem_Provider::get_matchFrameRate()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Provider_get_matchFrameRate_mCAB407F0D2F5D39966F0BD7544211C44CBCCC4EB (Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * __this, const RuntimeMethod* method)
{
	{
		return (bool)0;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem_Provider::set_matchFrameRate(System.Boolean)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Provider_set_matchFrameRate_mFB25AA4BD319D4968EEB29FB2A8EA8B79D4ED031 (Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * __this, bool ___value0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (Provider_set_matchFrameRate_mFB25AA4BD319D4968EEB29FB2A8EA8B79D4ED031_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		bool L_0 = ___value0;
		if (!L_0)
		{
			goto IL_000e;
		}
	}
	{
		NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 * L_1 = (NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 *)il2cpp_codegen_object_new(NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010_il2cpp_TypeInfo_var);
		NotSupportedException__ctor_mD023A89A5C1F740F43F0A9CD6C49DC21230B3CEE(L_1, _stringLiteralB803153B51A277297BD55B487E4635F22FE4D888, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_1, Provider_set_matchFrameRate_mFB25AA4BD319D4968EEB29FB2A8EA8B79D4ED031_RuntimeMethod_var);
	}

IL_000e:
	{
		return;
	}
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRSessionSubsystem_Provider::get_frameRate()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Provider_get_frameRate_m55D5E1C5FDF9ADAB43DE7E3F74C7A463ECA63F33 (Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (Provider_get_frameRate_m55D5E1C5FDF9ADAB43DE7E3F74C7A463ECA63F33_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 * L_0 = (NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 *)il2cpp_codegen_object_new(NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010_il2cpp_TypeInfo_var);
		NotSupportedException__ctor_mD023A89A5C1F740F43F0A9CD6C49DC21230B3CEE(L_0, _stringLiteral8D5D999C478CCB89B73D744A0781362458C30380, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_0, Provider_get_frameRate_m55D5E1C5FDF9ADAB43DE7E3F74C7A463ECA63F33_RuntimeMethod_var);
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystem_Provider::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Provider__ctor_m890EE1FF005D8F1C10E3F1F4B0B9C37D9ECD29DA (Provider_t5EF338E7DB92D0B2D3E462849519A630DB97E980 * __this, const RuntimeMethod* method)
{
	{
		Object__ctor_m925ECA5E85CA100E3FB86A4F9E15C120E9A184C0(__this, /*hidden argument*/NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor::get_supportsInstall()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRSessionSubsystemDescriptor_get_supportsInstall_m2AA89682007FE1D8BB811FD152DE326FF7BB5A99 (XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079 * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = __this->get_U3CsupportsInstallU3Ek__BackingField_2();
		return L_0;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor::set_supportsInstall(System.Boolean)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRSessionSubsystemDescriptor_set_supportsInstall_m010EE3F0CB4B143A90B93C1F10F063FB12546920 (XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079 * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsInstallU3Ek__BackingField_2(L_0);
		return;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor::get_supportsMatchFrameRate()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRSessionSubsystemDescriptor_get_supportsMatchFrameRate_m66DA7D5EE88322AF2EE5FC3B1BF8203115C2CA8F (XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079 * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = __this->get_U3CsupportsMatchFrameRateU3Ek__BackingField_3();
		return L_0;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor::set_supportsMatchFrameRate(System.Boolean)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRSessionSubsystemDescriptor_set_supportsMatchFrameRate_mC2B0189D51BF3B64026D01DD6A088052C5D74BFC (XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079 * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsMatchFrameRateU3Ek__BackingField_3(L_0);
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor::RegisterDescriptor(UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRSessionSubsystemDescriptor_RegisterDescriptor_m7A9F84E8A57323CDB5DC415BA05E72D6A72025E4 (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A  ___cinfo0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRSessionSubsystemDescriptor_RegisterDescriptor_m7A9F84E8A57323CDB5DC415BA05E72D6A72025E4_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A  L_0 = ___cinfo0;
		XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079 * L_1 = (XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079 *)il2cpp_codegen_object_new(XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079_il2cpp_TypeInfo_var);
		XRSessionSubsystemDescriptor__ctor_mF2A65C6A814FB2D22D5ED1608E5EFD5B0CD9A6E2(L_1, L_0, /*hidden argument*/NULL);
		IL2CPP_RUNTIME_CLASS_INIT(SubsystemRegistration_t0A22FECC46483ABBFFC039449407F73FF11F5A1A_il2cpp_TypeInfo_var);
		SubsystemRegistration_CreateDescriptor_m90E54AA3B7A6EC958B51A4775DC75DA4149F9DF2(L_1, /*hidden argument*/NULL);
		return;
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor::.ctor(UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRSessionSubsystemDescriptor__ctor_mF2A65C6A814FB2D22D5ED1608E5EFD5B0CD9A6E2 (XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079 * __this, Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A  ___cinfo0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRSessionSubsystemDescriptor__ctor_mF2A65C6A814FB2D22D5ED1608E5EFD5B0CD9A6E2_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		SubsystemDescriptor_1__ctor_mE7F10A17EF25AE3D50D0B7D5CAA54D07FE709FAB(__this, /*hidden argument*/SubsystemDescriptor_1__ctor_mE7F10A17EF25AE3D50D0B7D5CAA54D07FE709FAB_RuntimeMethod_var);
		String_t* L_0 = Cinfo_get_id_m998EB4BD213159391A0A66FD6C002C1CE5CD14E8_inline((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)(&___cinfo0), /*hidden argument*/NULL);
		SubsystemDescriptor_set_id_m3289D5DA5921130F0338C6332907E686E36FF064_inline(__this, L_0, /*hidden argument*/NULL);
		Type_t * L_1 = Cinfo_get_subsystemImplementationType_m63942F2B0929DF14EB7885E7553A9970FBD3E108_inline((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)(&___cinfo0), /*hidden argument*/NULL);
		SubsystemDescriptor_set_subsystemImplementationType_m727AB6BED84E57A382B8804F95A75A8012963EE1_inline(__this, L_1, /*hidden argument*/NULL);
		bool L_2 = Cinfo_get_supportsInstall_m5FBFD4D2F10A6A46F66F4EFBC61EA14DE0FEED99_inline((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)(&___cinfo0), /*hidden argument*/NULL);
		XRSessionSubsystemDescriptor_set_supportsInstall_m010EE3F0CB4B143A90B93C1F10F063FB12546920_inline(__this, L_2, /*hidden argument*/NULL);
		bool L_3 = Cinfo_get_supportsMatchFrameRate_mE591F09F87EA8F4E5563039C47A6331E6AF31895_inline((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)(&___cinfo0), /*hidden argument*/NULL);
		XRSessionSubsystemDescriptor_set_supportsMatchFrameRate_mC2B0189D51BF3B64026D01DD6A088052C5D74BFC_inline(__this, L_3, /*hidden argument*/NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Conversion methods for marshalling of: UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor/Cinfo
IL2CPP_EXTERN_C void Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A_marshal_pinvoke(const Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A& unmarshaled, Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A_marshaled_pinvoke& marshaled)
{
	Exception_t* ___U3CsubsystemImplementationTypeU3Ek__BackingField_3Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field '<subsystemImplementationType>k__BackingField' of type 'Cinfo': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___U3CsubsystemImplementationTypeU3Ek__BackingField_3Exception, NULL);
}
IL2CPP_EXTERN_C void Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A_marshal_pinvoke_back(const Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A_marshaled_pinvoke& marshaled, Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A& unmarshaled)
{
	Exception_t* ___U3CsubsystemImplementationTypeU3Ek__BackingField_3Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field '<subsystemImplementationType>k__BackingField' of type 'Cinfo': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___U3CsubsystemImplementationTypeU3Ek__BackingField_3Exception, NULL);
}
// Conversion method for clean up from marshalling of: UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor/Cinfo
IL2CPP_EXTERN_C void Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A_marshal_pinvoke_cleanup(Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A_marshaled_pinvoke& marshaled)
{
}
// Conversion methods for marshalling of: UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor/Cinfo
IL2CPP_EXTERN_C void Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A_marshal_com(const Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A& unmarshaled, Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A_marshaled_com& marshaled)
{
	Exception_t* ___U3CsubsystemImplementationTypeU3Ek__BackingField_3Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field '<subsystemImplementationType>k__BackingField' of type 'Cinfo': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___U3CsubsystemImplementationTypeU3Ek__BackingField_3Exception, NULL);
}
IL2CPP_EXTERN_C void Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A_marshal_com_back(const Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A_marshaled_com& marshaled, Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A& unmarshaled)
{
	Exception_t* ___U3CsubsystemImplementationTypeU3Ek__BackingField_3Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field '<subsystemImplementationType>k__BackingField' of type 'Cinfo': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___U3CsubsystemImplementationTypeU3Ek__BackingField_3Exception, NULL);
}
// Conversion method for clean up from marshalling of: UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor/Cinfo
IL2CPP_EXTERN_C void Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A_marshal_com_cleanup(Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A_marshaled_com& marshaled)
{
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo::get_supportsInstall()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_get_supportsInstall_m5FBFD4D2F10A6A46F66F4EFBC61EA14DE0FEED99 (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = __this->get_U3CsupportsInstallU3Ek__BackingField_0();
		return L_0;
	}
}
IL2CPP_EXTERN_C  bool Cinfo_get_supportsInstall_m5FBFD4D2F10A6A46F66F4EFBC61EA14DE0FEED99_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * _thisAdjusted = reinterpret_cast<Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *>(__this + _offset);
	return Cinfo_get_supportsInstall_m5FBFD4D2F10A6A46F66F4EFBC61EA14DE0FEED99_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo::set_supportsInstall(System.Boolean)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Cinfo_set_supportsInstall_mD74EB42C503AC5393E630A56E3AE579FE1558660 (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsInstallU3Ek__BackingField_0(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void Cinfo_set_supportsInstall_mD74EB42C503AC5393E630A56E3AE579FE1558660_AdjustorThunk (RuntimeObject * __this, bool ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * _thisAdjusted = reinterpret_cast<Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *>(__this + _offset);
	Cinfo_set_supportsInstall_mD74EB42C503AC5393E630A56E3AE579FE1558660_inline(_thisAdjusted, ___value0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo::get_supportsMatchFrameRate()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_get_supportsMatchFrameRate_mE591F09F87EA8F4E5563039C47A6331E6AF31895 (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = __this->get_U3CsupportsMatchFrameRateU3Ek__BackingField_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C  bool Cinfo_get_supportsMatchFrameRate_mE591F09F87EA8F4E5563039C47A6331E6AF31895_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * _thisAdjusted = reinterpret_cast<Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *>(__this + _offset);
	return Cinfo_get_supportsMatchFrameRate_mE591F09F87EA8F4E5563039C47A6331E6AF31895_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo::set_supportsMatchFrameRate(System.Boolean)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Cinfo_set_supportsMatchFrameRate_m2B92004D3F2E01EA5DDFBF5F928C5604E68B8D21 (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsMatchFrameRateU3Ek__BackingField_1(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void Cinfo_set_supportsMatchFrameRate_m2B92004D3F2E01EA5DDFBF5F928C5604E68B8D21_AdjustorThunk (RuntimeObject * __this, bool ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * _thisAdjusted = reinterpret_cast<Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *>(__this + _offset);
	Cinfo_set_supportsMatchFrameRate_m2B92004D3F2E01EA5DDFBF5F928C5604E68B8D21_inline(_thisAdjusted, ___value0, method);
}
// System.String UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo::get_id()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Cinfo_get_id_m998EB4BD213159391A0A66FD6C002C1CE5CD14E8 (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, const RuntimeMethod* method)
{
	{
		String_t* L_0 = __this->get_U3CidU3Ek__BackingField_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C  String_t* Cinfo_get_id_m998EB4BD213159391A0A66FD6C002C1CE5CD14E8_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * _thisAdjusted = reinterpret_cast<Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *>(__this + _offset);
	return Cinfo_get_id_m998EB4BD213159391A0A66FD6C002C1CE5CD14E8_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo::set_id(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Cinfo_set_id_mC4FF3C524E18065C55B5142D58FBD58A66479A41 (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, String_t* ___value0, const RuntimeMethod* method)
{
	{
		String_t* L_0 = ___value0;
		__this->set_U3CidU3Ek__BackingField_2(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void Cinfo_set_id_mC4FF3C524E18065C55B5142D58FBD58A66479A41_AdjustorThunk (RuntimeObject * __this, String_t* ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * _thisAdjusted = reinterpret_cast<Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *>(__this + _offset);
	Cinfo_set_id_mC4FF3C524E18065C55B5142D58FBD58A66479A41_inline(_thisAdjusted, ___value0, method);
}
// System.Type UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo::get_subsystemImplementationType()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Type_t * Cinfo_get_subsystemImplementationType_m63942F2B0929DF14EB7885E7553A9970FBD3E108 (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, const RuntimeMethod* method)
{
	{
		Type_t * L_0 = __this->get_U3CsubsystemImplementationTypeU3Ek__BackingField_3();
		return L_0;
	}
}
IL2CPP_EXTERN_C  Type_t * Cinfo_get_subsystemImplementationType_m63942F2B0929DF14EB7885E7553A9970FBD3E108_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * _thisAdjusted = reinterpret_cast<Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *>(__this + _offset);
	return Cinfo_get_subsystemImplementationType_m63942F2B0929DF14EB7885E7553A9970FBD3E108_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo::set_subsystemImplementationType(System.Type)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Cinfo_set_subsystemImplementationType_m3C759AEC2943DE059B20AA7F5A5B932B7432473C (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, Type_t * ___value0, const RuntimeMethod* method)
{
	{
		Type_t * L_0 = ___value0;
		__this->set_U3CsubsystemImplementationTypeU3Ek__BackingField_3(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void Cinfo_set_subsystemImplementationType_m3C759AEC2943DE059B20AA7F5A5B932B7432473C_AdjustorThunk (RuntimeObject * __this, Type_t * ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * _thisAdjusted = reinterpret_cast<Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *>(__this + _offset);
	Cinfo_set_subsystemImplementationType_m3C759AEC2943DE059B20AA7F5A5B932B7432473C_inline(_thisAdjusted, ___value0, method);
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Cinfo_GetHashCode_m06E1060A5995A0C346AB7C0E56CCAC4BEC758A6E (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (Cinfo_GetHashCode_m06E1060A5995A0C346AB7C0E56CCAC4BEC758A6E_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	bool V_0 = false;
	int32_t G_B3_0 = 0;
	int32_t G_B5_0 = 0;
	int32_t G_B4_0 = 0;
	int32_t G_B6_0 = 0;
	int32_t G_B6_1 = 0;
	{
		String_t* L_0 = Cinfo_get_id_m998EB4BD213159391A0A66FD6C002C1CE5CD14E8_inline((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)__this, /*hidden argument*/NULL);
		if (L_0)
		{
			goto IL_000b;
		}
	}
	{
		G_B3_0 = 0;
		goto IL_0016;
	}

IL_000b:
	{
		String_t* L_1 = Cinfo_get_id_m998EB4BD213159391A0A66FD6C002C1CE5CD14E8_inline((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)__this, /*hidden argument*/NULL);
		NullCheck(L_1);
		int32_t L_2 = VirtFuncInvoker0< int32_t >::Invoke(2 /* System.Int32 System.Object::GetHashCode() */, L_1);
		G_B3_0 = L_2;
	}

IL_0016:
	{
		Type_t * L_3 = Cinfo_get_subsystemImplementationType_m63942F2B0929DF14EB7885E7553A9970FBD3E108_inline((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)__this, /*hidden argument*/NULL);
		IL2CPP_RUNTIME_CLASS_INIT(Type_t_il2cpp_TypeInfo_var);
		bool L_4 = Type_op_Inequality_m615014191FB05FD50F63A24EB9A6CCA785E7CEC9(L_3, (Type_t *)NULL, /*hidden argument*/NULL);
		G_B4_0 = ((int32_t)il2cpp_codegen_multiply((int32_t)G_B3_0, (int32_t)((int32_t)486187739)));
		if (L_4)
		{
			G_B5_0 = ((int32_t)il2cpp_codegen_multiply((int32_t)G_B3_0, (int32_t)((int32_t)486187739)));
			goto IL_002d;
		}
	}
	{
		G_B6_0 = 0;
		G_B6_1 = G_B4_0;
		goto IL_0038;
	}

IL_002d:
	{
		Type_t * L_5 = Cinfo_get_subsystemImplementationType_m63942F2B0929DF14EB7885E7553A9970FBD3E108_inline((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)__this, /*hidden argument*/NULL);
		NullCheck(L_5);
		int32_t L_6 = VirtFuncInvoker0< int32_t >::Invoke(2 /* System.Int32 System.Object::GetHashCode() */, L_5);
		G_B6_0 = L_6;
		G_B6_1 = G_B5_0;
	}

IL_0038:
	{
		bool L_7 = Cinfo_get_supportsInstall_m5FBFD4D2F10A6A46F66F4EFBC61EA14DE0FEED99_inline((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)__this, /*hidden argument*/NULL);
		V_0 = L_7;
		int32_t L_8 = Boolean_GetHashCode_m92C426D44100ED098FEECC96A743C3CB92DFF737((bool*)(&V_0), /*hidden argument*/NULL);
		bool L_9 = Cinfo_get_supportsMatchFrameRate_mE591F09F87EA8F4E5563039C47A6331E6AF31895_inline((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)__this, /*hidden argument*/NULL);
		V_0 = L_9;
		int32_t L_10 = Boolean_GetHashCode_m92C426D44100ED098FEECC96A743C3CB92DFF737((bool*)(&V_0), /*hidden argument*/NULL);
		return ((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)G_B6_1, (int32_t)G_B6_0)), (int32_t)((int32_t)486187739))), (int32_t)L_8)), (int32_t)((int32_t)486187739))), (int32_t)L_10));
	}
}
IL2CPP_EXTERN_C  int32_t Cinfo_GetHashCode_m06E1060A5995A0C346AB7C0E56CCAC4BEC758A6E_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * _thisAdjusted = reinterpret_cast<Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *>(__this + _offset);
	return Cinfo_GetHashCode_m06E1060A5995A0C346AB7C0E56CCAC4BEC758A6E(_thisAdjusted, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_Equals_m2118FE6BBF6355D645E72BDD6662CF313B8E94EB (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (Cinfo_Equals_m2118FE6BBF6355D645E72BDD6662CF313B8E94EB_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject * L_0 = ___obj0;
		if (((RuntimeObject *)IsInstSealed((RuntimeObject*)L_0, Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A_il2cpp_TypeInfo_var)))
		{
			goto IL_000a;
		}
	}
	{
		return (bool)0;
	}

IL_000a:
	{
		RuntimeObject * L_1 = ___obj0;
		bool L_2 = Cinfo_Equals_mC919333F29E857CC3F929451A080AC4A38385E01((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)__this, ((*(Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)UnBox(L_1, Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A_il2cpp_TypeInfo_var)))), /*hidden argument*/NULL);
		return L_2;
	}
}
IL2CPP_EXTERN_C  bool Cinfo_Equals_m2118FE6BBF6355D645E72BDD6662CF313B8E94EB_AdjustorThunk (RuntimeObject * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * _thisAdjusted = reinterpret_cast<Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *>(__this + _offset);
	return Cinfo_Equals_m2118FE6BBF6355D645E72BDD6662CF313B8E94EB(_thisAdjusted, ___obj0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo::Equals(UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_Equals_mC919333F29E857CC3F929451A080AC4A38385E01 (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A  ___other0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (Cinfo_Equals_mC919333F29E857CC3F929451A080AC4A38385E01_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		String_t* L_0 = Cinfo_get_id_m998EB4BD213159391A0A66FD6C002C1CE5CD14E8_inline((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)__this, /*hidden argument*/NULL);
		String_t* L_1 = Cinfo_get_id_m998EB4BD213159391A0A66FD6C002C1CE5CD14E8_inline((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)(&___other0), /*hidden argument*/NULL);
		bool L_2 = String_Equals_m90EB651A751C3444BADBBD5401109CE05B3E1CFB(L_0, L_1, /*hidden argument*/NULL);
		if (!L_2)
		{
			goto IL_0047;
		}
	}
	{
		Type_t * L_3 = Cinfo_get_subsystemImplementationType_m63942F2B0929DF14EB7885E7553A9970FBD3E108_inline((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)__this, /*hidden argument*/NULL);
		Type_t * L_4 = Cinfo_get_subsystemImplementationType_m63942F2B0929DF14EB7885E7553A9970FBD3E108_inline((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)(&___other0), /*hidden argument*/NULL);
		IL2CPP_RUNTIME_CLASS_INIT(Type_t_il2cpp_TypeInfo_var);
		bool L_5 = Type_op_Equality_m7040622C9E1037EFC73E1F0EDB1DD241282BE3D8(L_3, L_4, /*hidden argument*/NULL);
		if (!L_5)
		{
			goto IL_0047;
		}
	}
	{
		bool L_6 = Cinfo_get_supportsInstall_m5FBFD4D2F10A6A46F66F4EFBC61EA14DE0FEED99_inline((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)__this, /*hidden argument*/NULL);
		bool L_7 = Cinfo_get_supportsInstall_m5FBFD4D2F10A6A46F66F4EFBC61EA14DE0FEED99_inline((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)(&___other0), /*hidden argument*/NULL);
		if ((!(((uint32_t)L_6) == ((uint32_t)L_7))))
		{
			goto IL_0047;
		}
	}
	{
		bool L_8 = Cinfo_get_supportsMatchFrameRate_mE591F09F87EA8F4E5563039C47A6331E6AF31895_inline((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)__this, /*hidden argument*/NULL);
		bool L_9 = Cinfo_get_supportsMatchFrameRate_mE591F09F87EA8F4E5563039C47A6331E6AF31895_inline((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)(&___other0), /*hidden argument*/NULL);
		return (bool)((((int32_t)L_8) == ((int32_t)L_9))? 1 : 0);
	}

IL_0047:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool Cinfo_Equals_mC919333F29E857CC3F929451A080AC4A38385E01_AdjustorThunk (RuntimeObject * __this, Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A  ___other0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * _thisAdjusted = reinterpret_cast<Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *>(__this + _offset);
	return Cinfo_Equals_mC919333F29E857CC3F929451A080AC4A38385E01(_thisAdjusted, ___other0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo::op_Equality(UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo,UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_op_Equality_mCDF32E8CB0B505E476F92FD9A3FAAD6C46BF20DA (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A  ___lhs0, Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A  ___rhs1, const RuntimeMethod* method)
{
	{
		Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A  L_0 = ___rhs1;
		bool L_1 = Cinfo_Equals_mC919333F29E857CC3F929451A080AC4A38385E01((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return L_1;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo::op_Inequality(UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo,UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor_Cinfo)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Cinfo_op_Inequality_mA173B6B47123F27CE69E7C18FECEC6871CB08A99 (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A  ___lhs0, Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A  ___rhs1, const RuntimeMethod* method)
{
	{
		Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A  L_0 = ___rhs1;
		bool L_1 = Cinfo_Equals_mC919333F29E857CC3F929451A080AC4A38385E01((Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return (bool)((((int32_t)L_1) == ((int32_t)0))? 1 : 0);
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// UnityEngine.ScreenOrientation UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::get_screenOrientation()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRSessionUpdateParams_get_screenOrientation_m111C145EA6A683F025DF48C6EA355E37D8974183 (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_U3CscreenOrientationU3Ek__BackingField_0();
		return L_0;
	}
}
IL2CPP_EXTERN_C  int32_t XRSessionUpdateParams_get_screenOrientation_m111C145EA6A683F025DF48C6EA355E37D8974183_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * _thisAdjusted = reinterpret_cast<XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *>(__this + _offset);
	return XRSessionUpdateParams_get_screenOrientation_m111C145EA6A683F025DF48C6EA355E37D8974183_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::set_screenOrientation(UnityEngine.ScreenOrientation)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRSessionUpdateParams_set_screenOrientation_m7C20FD52988E0F21604700B5CDA93FBA63DD28C6 (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, int32_t ___value0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___value0;
		__this->set_U3CscreenOrientationU3Ek__BackingField_0(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void XRSessionUpdateParams_set_screenOrientation_m7C20FD52988E0F21604700B5CDA93FBA63DD28C6_AdjustorThunk (RuntimeObject * __this, int32_t ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * _thisAdjusted = reinterpret_cast<XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *>(__this + _offset);
	XRSessionUpdateParams_set_screenOrientation_m7C20FD52988E0F21604700B5CDA93FBA63DD28C6_inline(_thisAdjusted, ___value0, method);
}
// UnityEngine.Vector2Int UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::get_screenDimensions()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  XRSessionUpdateParams_get_screenDimensions_m61A9722E272D6292B9C7C093BF7792FB007BF21E (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, const RuntimeMethod* method)
{
	{
		Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  L_0 = __this->get_U3CscreenDimensionsU3Ek__BackingField_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C  Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  XRSessionUpdateParams_get_screenDimensions_m61A9722E272D6292B9C7C093BF7792FB007BF21E_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * _thisAdjusted = reinterpret_cast<XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *>(__this + _offset);
	return XRSessionUpdateParams_get_screenDimensions_m61A9722E272D6292B9C7C093BF7792FB007BF21E_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::set_screenDimensions(UnityEngine.Vector2Int)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRSessionUpdateParams_set_screenDimensions_m41570268847916BA02DD2427BDDB08B3D466A905 (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  ___value0, const RuntimeMethod* method)
{
	{
		Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  L_0 = ___value0;
		__this->set_U3CscreenDimensionsU3Ek__BackingField_1(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void XRSessionUpdateParams_set_screenDimensions_m41570268847916BA02DD2427BDDB08B3D466A905_AdjustorThunk (RuntimeObject * __this, Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * _thisAdjusted = reinterpret_cast<XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *>(__this + _offset);
	XRSessionUpdateParams_set_screenDimensions_m41570268847916BA02DD2427BDDB08B3D466A905_inline(_thisAdjusted, ___value0, method);
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRSessionUpdateParams_GetHashCode_m3E0C208F41FAC84F879A073F85FB9DC0F1C09520 (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, const RuntimeMethod* method)
{
	int32_t V_0 = 0;
	Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  V_1;
	memset((&V_1), 0, sizeof(V_1));
	{
		int32_t L_0 = XRSessionUpdateParams_get_screenOrientation_m111C145EA6A683F025DF48C6EA355E37D8974183_inline((XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *)__this, /*hidden argument*/NULL);
		V_0 = L_0;
		int32_t L_1 = Int32_GetHashCode_m245C424ECE351E5FE3277A88EEB02132DAB8C25A((int32_t*)(&V_0), /*hidden argument*/NULL);
		Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  L_2 = XRSessionUpdateParams_get_screenDimensions_m61A9722E272D6292B9C7C093BF7792FB007BF21E_inline((XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *)__this, /*hidden argument*/NULL);
		V_1 = L_2;
		int32_t L_3 = Vector2Int_GetHashCode_m73E874F4E94DF3D2603035E2E892873B139A7A9E((Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905 *)(&V_1), /*hidden argument*/NULL);
		return ((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)L_1, (int32_t)((int32_t)486187739))), (int32_t)L_3));
	}
}
IL2CPP_EXTERN_C  int32_t XRSessionUpdateParams_GetHashCode_m3E0C208F41FAC84F879A073F85FB9DC0F1C09520_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * _thisAdjusted = reinterpret_cast<XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *>(__this + _offset);
	return XRSessionUpdateParams_GetHashCode_m3E0C208F41FAC84F879A073F85FB9DC0F1C09520(_thisAdjusted, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRSessionUpdateParams_Equals_m415AB0E24C9CF0C013872ED16C571B65DACF24B1 (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRSessionUpdateParams_Equals_m415AB0E24C9CF0C013872ED16C571B65DACF24B1_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject * L_0 = ___obj0;
		if (((RuntimeObject *)IsInstSealed((RuntimeObject*)L_0, XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16_il2cpp_TypeInfo_var)))
		{
			goto IL_000a;
		}
	}
	{
		return (bool)0;
	}

IL_000a:
	{
		RuntimeObject * L_1 = ___obj0;
		bool L_2 = XRSessionUpdateParams_Equals_mFE8BAF000FDC02612C5D563960EB974E510DEAB3((XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *)__this, ((*(XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *)((XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *)UnBox(L_1, XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16_il2cpp_TypeInfo_var)))), /*hidden argument*/NULL);
		return L_2;
	}
}
IL2CPP_EXTERN_C  bool XRSessionUpdateParams_Equals_m415AB0E24C9CF0C013872ED16C571B65DACF24B1_AdjustorThunk (RuntimeObject * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * _thisAdjusted = reinterpret_cast<XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *>(__this + _offset);
	return XRSessionUpdateParams_Equals_m415AB0E24C9CF0C013872ED16C571B65DACF24B1(_thisAdjusted, ___obj0, method);
}
// System.String UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::ToString()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* XRSessionUpdateParams_ToString_m7150FEAE08C59544392C3D47B3CB5AC318B82F4A (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRSessionUpdateParams_ToString_m7150FEAE08C59544392C3D47B3CB5AC318B82F4A_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		int32_t L_0 = XRSessionUpdateParams_get_screenOrientation_m111C145EA6A683F025DF48C6EA355E37D8974183_inline((XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *)__this, /*hidden argument*/NULL);
		int32_t L_1 = L_0;
		RuntimeObject * L_2 = Box(ScreenOrientation_t4AB8E2E02033B0EAEA0260B05B1D88DA8058BB51_il2cpp_TypeInfo_var, &L_1);
		Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  L_3 = XRSessionUpdateParams_get_screenDimensions_m61A9722E272D6292B9C7C093BF7792FB007BF21E_inline((XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *)__this, /*hidden argument*/NULL);
		Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  L_4 = L_3;
		RuntimeObject * L_5 = Box(Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905_il2cpp_TypeInfo_var, &L_4);
		String_t* L_6 = String_Format_m19325298DBC61AAC016C16F7B3CF97A8A3DEA34A(_stringLiteralB39A7E18F4E7B76A8C729E54E8852F98B5EA76F3, L_2, L_5, /*hidden argument*/NULL);
		return L_6;
	}
}
IL2CPP_EXTERN_C  String_t* XRSessionUpdateParams_ToString_m7150FEAE08C59544392C3D47B3CB5AC318B82F4A_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * _thisAdjusted = reinterpret_cast<XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *>(__this + _offset);
	return XRSessionUpdateParams_ToString_m7150FEAE08C59544392C3D47B3CB5AC318B82F4A(_thisAdjusted, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::Equals(UnityEngine.XR.ARSubsystems.XRSessionUpdateParams)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRSessionUpdateParams_Equals_mFE8BAF000FDC02612C5D563960EB974E510DEAB3 (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16  ___other0, const RuntimeMethod* method)
{
	Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  V_0;
	memset((&V_0), 0, sizeof(V_0));
	{
		int32_t L_0 = XRSessionUpdateParams_get_screenOrientation_m111C145EA6A683F025DF48C6EA355E37D8974183_inline((XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *)__this, /*hidden argument*/NULL);
		int32_t L_1 = XRSessionUpdateParams_get_screenOrientation_m111C145EA6A683F025DF48C6EA355E37D8974183_inline((XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *)(&___other0), /*hidden argument*/NULL);
		if ((!(((uint32_t)L_0) == ((uint32_t)L_1))))
		{
			goto IL_0025;
		}
	}
	{
		Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  L_2 = XRSessionUpdateParams_get_screenDimensions_m61A9722E272D6292B9C7C093BF7792FB007BF21E_inline((XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *)__this, /*hidden argument*/NULL);
		V_0 = L_2;
		Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  L_3 = XRSessionUpdateParams_get_screenDimensions_m61A9722E272D6292B9C7C093BF7792FB007BF21E_inline((XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *)(&___other0), /*hidden argument*/NULL);
		bool L_4 = Vector2Int_Equals_m65420C995F326F5C340E4825EA5E16BDE68F5A9C((Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905 *)(&V_0), L_3, /*hidden argument*/NULL);
		return L_4;
	}

IL_0025:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool XRSessionUpdateParams_Equals_mFE8BAF000FDC02612C5D563960EB974E510DEAB3_AdjustorThunk (RuntimeObject * __this, XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16  ___other0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * _thisAdjusted = reinterpret_cast<XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *>(__this + _offset);
	return XRSessionUpdateParams_Equals_mFE8BAF000FDC02612C5D563960EB974E510DEAB3(_thisAdjusted, ___other0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::op_Equality(UnityEngine.XR.ARSubsystems.XRSessionUpdateParams,UnityEngine.XR.ARSubsystems.XRSessionUpdateParams)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRSessionUpdateParams_op_Equality_mEB57CF7E4D66886BF9EE3FF1BBF7D1B73E63608B (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16  ___lhs0, XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16  ___rhs1, const RuntimeMethod* method)
{
	{
		XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16  L_0 = ___rhs1;
		bool L_1 = XRSessionUpdateParams_Equals_mFE8BAF000FDC02612C5D563960EB974E510DEAB3((XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return L_1;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRSessionUpdateParams::op_Inequality(UnityEngine.XR.ARSubsystems.XRSessionUpdateParams,UnityEngine.XR.ARSubsystems.XRSessionUpdateParams)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRSessionUpdateParams_op_Inequality_mD00004C24603E13FC7D4F0239F812446EE21FF75 (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16  ___lhs0, XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16  ___rhs1, const RuntimeMethod* method)
{
	{
		XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16  L_0 = ___rhs1;
		bool L_1 = XRSessionUpdateParams_Equals_mFE8BAF000FDC02612C5D563960EB974E510DEAB3((XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return (bool)((((int32_t)L_1) == ((int32_t)0))? 1 : 0);
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.IntPtr UnityEngine.XR.ARSubsystems.XRTextureDescriptor::get_nativeTexture()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR intptr_t XRTextureDescriptor_get_nativeTexture_m83CAA03353C203B7D38618C1963C715F052081F8 (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method)
{
	{
		intptr_t L_0 = __this->get_m_NativeTexture_0();
		return (intptr_t)L_0;
	}
}
IL2CPP_EXTERN_C  intptr_t XRTextureDescriptor_get_nativeTexture_m83CAA03353C203B7D38618C1963C715F052081F8_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * _thisAdjusted = reinterpret_cast<XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *>(__this + _offset);
	return XRTextureDescriptor_get_nativeTexture_m83CAA03353C203B7D38618C1963C715F052081F8_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRTextureDescriptor::set_nativeTexture(System.IntPtr)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRTextureDescriptor_set_nativeTexture_mEF92A3E263840B8F428C314323C20A11F896D907 (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, intptr_t ___value0, const RuntimeMethod* method)
{
	{
		intptr_t L_0 = ___value0;
		__this->set_m_NativeTexture_0((intptr_t)L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void XRTextureDescriptor_set_nativeTexture_mEF92A3E263840B8F428C314323C20A11F896D907_AdjustorThunk (RuntimeObject * __this, intptr_t ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * _thisAdjusted = reinterpret_cast<XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *>(__this + _offset);
	XRTextureDescriptor_set_nativeTexture_mEF92A3E263840B8F428C314323C20A11F896D907_inline(_thisAdjusted, ___value0, method);
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRTextureDescriptor::get_width()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRTextureDescriptor_get_width_m158B2CEE4A0F56DF263BB642F5E4A3C3CF339E0B (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_Width_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C  int32_t XRTextureDescriptor_get_width_m158B2CEE4A0F56DF263BB642F5E4A3C3CF339E0B_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * _thisAdjusted = reinterpret_cast<XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *>(__this + _offset);
	return XRTextureDescriptor_get_width_m158B2CEE4A0F56DF263BB642F5E4A3C3CF339E0B_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRTextureDescriptor::set_width(System.Int32)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRTextureDescriptor_set_width_m59E159F83238991BAD9838C5835A07E44F6A163E (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, int32_t ___value0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___value0;
		__this->set_m_Width_1(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void XRTextureDescriptor_set_width_m59E159F83238991BAD9838C5835A07E44F6A163E_AdjustorThunk (RuntimeObject * __this, int32_t ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * _thisAdjusted = reinterpret_cast<XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *>(__this + _offset);
	XRTextureDescriptor_set_width_m59E159F83238991BAD9838C5835A07E44F6A163E_inline(_thisAdjusted, ___value0, method);
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRTextureDescriptor::get_height()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRTextureDescriptor_get_height_mCE50370000BCF4A70B95344A0731A771401C0894 (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_Height_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C  int32_t XRTextureDescriptor_get_height_mCE50370000BCF4A70B95344A0731A771401C0894_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * _thisAdjusted = reinterpret_cast<XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *>(__this + _offset);
	return XRTextureDescriptor_get_height_mCE50370000BCF4A70B95344A0731A771401C0894_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRTextureDescriptor::set_height(System.Int32)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRTextureDescriptor_set_height_mE690E293BE1FE8009052CC87FA454FB79DE9DF0E (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, int32_t ___value0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___value0;
		__this->set_m_Height_2(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void XRTextureDescriptor_set_height_mE690E293BE1FE8009052CC87FA454FB79DE9DF0E_AdjustorThunk (RuntimeObject * __this, int32_t ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * _thisAdjusted = reinterpret_cast<XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *>(__this + _offset);
	XRTextureDescriptor_set_height_mE690E293BE1FE8009052CC87FA454FB79DE9DF0E_inline(_thisAdjusted, ___value0, method);
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRTextureDescriptor::get_mipmapCount()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRTextureDescriptor_get_mipmapCount_m491B149D8BBF148B2030214818E237A28D9B6CC4 (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_MipmapCount_3();
		return L_0;
	}
}
IL2CPP_EXTERN_C  int32_t XRTextureDescriptor_get_mipmapCount_m491B149D8BBF148B2030214818E237A28D9B6CC4_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * _thisAdjusted = reinterpret_cast<XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *>(__this + _offset);
	return XRTextureDescriptor_get_mipmapCount_m491B149D8BBF148B2030214818E237A28D9B6CC4_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRTextureDescriptor::set_mipmapCount(System.Int32)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRTextureDescriptor_set_mipmapCount_m8CC98FD1B188CA92DE7C1C430BF71E11E7AD7858 (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, int32_t ___value0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___value0;
		__this->set_m_MipmapCount_3(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void XRTextureDescriptor_set_mipmapCount_m8CC98FD1B188CA92DE7C1C430BF71E11E7AD7858_AdjustorThunk (RuntimeObject * __this, int32_t ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * _thisAdjusted = reinterpret_cast<XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *>(__this + _offset);
	XRTextureDescriptor_set_mipmapCount_m8CC98FD1B188CA92DE7C1C430BF71E11E7AD7858_inline(_thisAdjusted, ___value0, method);
}
// UnityEngine.TextureFormat UnityEngine.XR.ARSubsystems.XRTextureDescriptor::get_format()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRTextureDescriptor_get_format_mA2DA22DC1DEBCAD27A9C69F3374D614DF1C3FA2B (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_Format_4();
		return L_0;
	}
}
IL2CPP_EXTERN_C  int32_t XRTextureDescriptor_get_format_mA2DA22DC1DEBCAD27A9C69F3374D614DF1C3FA2B_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * _thisAdjusted = reinterpret_cast<XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *>(__this + _offset);
	return XRTextureDescriptor_get_format_mA2DA22DC1DEBCAD27A9C69F3374D614DF1C3FA2B_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRTextureDescriptor::set_format(UnityEngine.TextureFormat)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRTextureDescriptor_set_format_m2BEDFB4C31E590B2C2AAE7145AEAE714491E0EA6 (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, int32_t ___value0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___value0;
		__this->set_m_Format_4(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void XRTextureDescriptor_set_format_m2BEDFB4C31E590B2C2AAE7145AEAE714491E0EA6_AdjustorThunk (RuntimeObject * __this, int32_t ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * _thisAdjusted = reinterpret_cast<XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *>(__this + _offset);
	XRTextureDescriptor_set_format_m2BEDFB4C31E590B2C2AAE7145AEAE714491E0EA6_inline(_thisAdjusted, ___value0, method);
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRTextureDescriptor::get_propertyNameId()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRTextureDescriptor_get_propertyNameId_mA3A29036B96A64D1C4F147678E60E2BFCAAAAFF0 (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_PropertyNameId_5();
		return L_0;
	}
}
IL2CPP_EXTERN_C  int32_t XRTextureDescriptor_get_propertyNameId_mA3A29036B96A64D1C4F147678E60E2BFCAAAAFF0_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * _thisAdjusted = reinterpret_cast<XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *>(__this + _offset);
	return XRTextureDescriptor_get_propertyNameId_mA3A29036B96A64D1C4F147678E60E2BFCAAAAFF0_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRTextureDescriptor::set_propertyNameId(System.Int32)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRTextureDescriptor_set_propertyNameId_m87654C29B3CEFA71D22E9F1323058334E8338B4F (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, int32_t ___value0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___value0;
		__this->set_m_PropertyNameId_5(L_0);
		return;
	}
}
IL2CPP_EXTERN_C  void XRTextureDescriptor_set_propertyNameId_m87654C29B3CEFA71D22E9F1323058334E8338B4F_AdjustorThunk (RuntimeObject * __this, int32_t ___value0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * _thisAdjusted = reinterpret_cast<XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *>(__this + _offset);
	XRTextureDescriptor_set_propertyNameId_m87654C29B3CEFA71D22E9F1323058334E8338B4F_inline(_thisAdjusted, ___value0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRTextureDescriptor::get_valid()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTextureDescriptor_get_valid_mF060565C5E24FDF97771F6FDA3235562DF01977B (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRTextureDescriptor_get_valid_mF060565C5E24FDF97771F6FDA3235562DF01977B_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		intptr_t L_0 = __this->get_m_NativeTexture_0();
		bool L_1 = IntPtr_op_Inequality_mB4886A806009EA825EFCC60CD2A7F6EB8E273A61((intptr_t)L_0, (intptr_t)(0), /*hidden argument*/NULL);
		if (!L_1)
		{
			goto IL_0025;
		}
	}
	{
		int32_t L_2 = __this->get_m_Width_1();
		if ((((int32_t)L_2) <= ((int32_t)0)))
		{
			goto IL_0025;
		}
	}
	{
		int32_t L_3 = __this->get_m_Height_2();
		return (bool)((((int32_t)L_3) > ((int32_t)0))? 1 : 0);
	}

IL_0025:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool XRTextureDescriptor_get_valid_mF060565C5E24FDF97771F6FDA3235562DF01977B_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * _thisAdjusted = reinterpret_cast<XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *>(__this + _offset);
	return XRTextureDescriptor_get_valid_mF060565C5E24FDF97771F6FDA3235562DF01977B(_thisAdjusted, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRTextureDescriptor::hasIdenticalTextureMetadata(UnityEngine.XR.ARSubsystems.XRTextureDescriptor)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTextureDescriptor_hasIdenticalTextureMetadata_mD9C2A76A8B680BB7B2742F82235E40977CD098AE (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD  ___other0, const RuntimeMethod* method)
{
	{
		int32_t* L_0 = __this->get_address_of_m_Width_1();
		XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD  L_1 = ___other0;
		int32_t L_2 = L_1.get_m_Width_1();
		bool L_3 = Int32_Equals_mC8C45B8899F291D55A6152C8FEDB3CFFF181170B((int32_t*)L_0, L_2, /*hidden argument*/NULL);
		if (!L_3)
		{
			goto IL_0048;
		}
	}
	{
		int32_t* L_4 = __this->get_address_of_m_Height_2();
		XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD  L_5 = ___other0;
		int32_t L_6 = L_5.get_m_Height_2();
		bool L_7 = Int32_Equals_mC8C45B8899F291D55A6152C8FEDB3CFFF181170B((int32_t*)L_4, L_6, /*hidden argument*/NULL);
		if (!L_7)
		{
			goto IL_0048;
		}
	}
	{
		int32_t* L_8 = __this->get_address_of_m_MipmapCount_3();
		XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD  L_9 = ___other0;
		int32_t L_10 = L_9.get_m_MipmapCount_3();
		bool L_11 = Int32_Equals_mC8C45B8899F291D55A6152C8FEDB3CFFF181170B((int32_t*)L_8, L_10, /*hidden argument*/NULL);
		if (!L_11)
		{
			goto IL_0048;
		}
	}
	{
		int32_t L_12 = __this->get_m_Format_4();
		XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD  L_13 = ___other0;
		int32_t L_14 = L_13.get_m_Format_4();
		return (bool)((((int32_t)L_12) == ((int32_t)L_14))? 1 : 0);
	}

IL_0048:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool XRTextureDescriptor_hasIdenticalTextureMetadata_mD9C2A76A8B680BB7B2742F82235E40977CD098AE_AdjustorThunk (RuntimeObject * __this, XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD  ___other0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * _thisAdjusted = reinterpret_cast<XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *>(__this + _offset);
	return XRTextureDescriptor_hasIdenticalTextureMetadata_mD9C2A76A8B680BB7B2742F82235E40977CD098AE(_thisAdjusted, ___other0, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRTextureDescriptor::Reset()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRTextureDescriptor_Reset_m64A787FBD1F11161369A72A7D61763DDF8D74EBC (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRTextureDescriptor_Reset_m64A787FBD1F11161369A72A7D61763DDF8D74EBC_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		__this->set_m_NativeTexture_0((intptr_t)(0));
		__this->set_m_Width_1(0);
		__this->set_m_Height_2(0);
		__this->set_m_MipmapCount_3(0);
		__this->set_m_Format_4(0);
		__this->set_m_PropertyNameId_5(0);
		return;
	}
}
IL2CPP_EXTERN_C  void XRTextureDescriptor_Reset_m64A787FBD1F11161369A72A7D61763DDF8D74EBC_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * _thisAdjusted = reinterpret_cast<XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *>(__this + _offset);
	XRTextureDescriptor_Reset_m64A787FBD1F11161369A72A7D61763DDF8D74EBC(_thisAdjusted, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRTextureDescriptor::Equals(UnityEngine.XR.ARSubsystems.XRTextureDescriptor)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTextureDescriptor_Equals_m124C4B8E0370717E0714FB2D28493A77034C6E38 (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD  ___other0, const RuntimeMethod* method)
{
	{
		XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD  L_0 = ___other0;
		bool L_1 = XRTextureDescriptor_hasIdenticalTextureMetadata_mD9C2A76A8B680BB7B2742F82235E40977CD098AE((XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *)__this, L_0, /*hidden argument*/NULL);
		if (!L_1)
		{
			goto IL_002e;
		}
	}
	{
		int32_t* L_2 = __this->get_address_of_m_PropertyNameId_5();
		XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD  L_3 = ___other0;
		int32_t L_4 = L_3.get_m_PropertyNameId_5();
		bool L_5 = Int32_Equals_mC8C45B8899F291D55A6152C8FEDB3CFFF181170B((int32_t*)L_2, L_4, /*hidden argument*/NULL);
		if (!L_5)
		{
			goto IL_002e;
		}
	}
	{
		intptr_t L_6 = __this->get_m_NativeTexture_0();
		XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD  L_7 = ___other0;
		intptr_t L_8 = L_7.get_m_NativeTexture_0();
		bool L_9 = IntPtr_op_Equality_mEE8D9FD2DFE312BBAA8B4ED3BF7976B3142A5934((intptr_t)L_6, (intptr_t)L_8, /*hidden argument*/NULL);
		return L_9;
	}

IL_002e:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool XRTextureDescriptor_Equals_m124C4B8E0370717E0714FB2D28493A77034C6E38_AdjustorThunk (RuntimeObject * __this, XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD  ___other0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * _thisAdjusted = reinterpret_cast<XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *>(__this + _offset);
	return XRTextureDescriptor_Equals_m124C4B8E0370717E0714FB2D28493A77034C6E38(_thisAdjusted, ___other0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRTextureDescriptor::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTextureDescriptor_Equals_m8D2E1A6303E60A653F70870CBD04845414F6A0A5 (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRTextureDescriptor_Equals_m8D2E1A6303E60A653F70870CBD04845414F6A0A5_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject * L_0 = ___obj0;
		if (!((RuntimeObject *)IsInstSealed((RuntimeObject*)L_0, XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD_il2cpp_TypeInfo_var)))
		{
			goto IL_0015;
		}
	}
	{
		RuntimeObject * L_1 = ___obj0;
		bool L_2 = XRTextureDescriptor_Equals_m124C4B8E0370717E0714FB2D28493A77034C6E38((XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *)__this, ((*(XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *)((XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *)UnBox(L_1, XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD_il2cpp_TypeInfo_var)))), /*hidden argument*/NULL);
		return L_2;
	}

IL_0015:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool XRTextureDescriptor_Equals_m8D2E1A6303E60A653F70870CBD04845414F6A0A5_AdjustorThunk (RuntimeObject * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * _thisAdjusted = reinterpret_cast<XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *>(__this + _offset);
	return XRTextureDescriptor_Equals_m8D2E1A6303E60A653F70870CBD04845414F6A0A5(_thisAdjusted, ___obj0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRTextureDescriptor::op_Equality(UnityEngine.XR.ARSubsystems.XRTextureDescriptor,UnityEngine.XR.ARSubsystems.XRTextureDescriptor)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTextureDescriptor_op_Equality_mBEE6E663B93B3648626DAACC5D0AD1F2C0B76847 (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD  ___lhs0, XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD  ___rhs1, const RuntimeMethod* method)
{
	{
		XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD  L_0 = ___rhs1;
		bool L_1 = XRTextureDescriptor_Equals_m124C4B8E0370717E0714FB2D28493A77034C6E38((XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return L_1;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRTextureDescriptor::op_Inequality(UnityEngine.XR.ARSubsystems.XRTextureDescriptor,UnityEngine.XR.ARSubsystems.XRTextureDescriptor)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTextureDescriptor_op_Inequality_m1373FCEBF131F54B4A61398DE7034853861E9EAE (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD  ___lhs0, XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD  ___rhs1, const RuntimeMethod* method)
{
	{
		XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD  L_0 = ___rhs1;
		bool L_1 = XRTextureDescriptor_Equals_m124C4B8E0370717E0714FB2D28493A77034C6E38((XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return (bool)((((int32_t)L_1) == ((int32_t)0))? 1 : 0);
	}
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRTextureDescriptor::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRTextureDescriptor_GetHashCode_mE61628A57D74C31744B57EBFBE8E8EDFA673B65F (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method)
{
	int32_t V_0 = 0;
	{
		intptr_t* L_0 = __this->get_address_of_m_NativeTexture_0();
		int32_t L_1 = IntPtr_GetHashCode_m0A6AE0C85A4AEEA127235FB5A32056F630E3749A((intptr_t*)L_0, /*hidden argument*/NULL);
		int32_t* L_2 = __this->get_address_of_m_Width_1();
		int32_t L_3 = Int32_GetHashCode_m245C424ECE351E5FE3277A88EEB02132DAB8C25A((int32_t*)L_2, /*hidden argument*/NULL);
		int32_t* L_4 = __this->get_address_of_m_Height_2();
		int32_t L_5 = Int32_GetHashCode_m245C424ECE351E5FE3277A88EEB02132DAB8C25A((int32_t*)L_4, /*hidden argument*/NULL);
		int32_t* L_6 = __this->get_address_of_m_MipmapCount_3();
		int32_t L_7 = Int32_GetHashCode_m245C424ECE351E5FE3277A88EEB02132DAB8C25A((int32_t*)L_6, /*hidden argument*/NULL);
		int32_t L_8 = __this->get_m_Format_4();
		V_0 = L_8;
		int32_t L_9 = Int32_GetHashCode_m245C424ECE351E5FE3277A88EEB02132DAB8C25A((int32_t*)(&V_0), /*hidden argument*/NULL);
		int32_t* L_10 = __this->get_address_of_m_PropertyNameId_5();
		int32_t L_11 = Int32_GetHashCode_m245C424ECE351E5FE3277A88EEB02132DAB8C25A((int32_t*)L_10, /*hidden argument*/NULL);
		return ((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)486187739), (int32_t)((int32_t)486187739))), (int32_t)L_1)), (int32_t)((int32_t)486187739))), (int32_t)L_3)), (int32_t)((int32_t)486187739))), (int32_t)L_5)), (int32_t)((int32_t)486187739))), (int32_t)L_7)), (int32_t)((int32_t)486187739))), (int32_t)L_9)), (int32_t)((int32_t)486187739))), (int32_t)L_11));
	}
}
IL2CPP_EXTERN_C  int32_t XRTextureDescriptor_GetHashCode_mE61628A57D74C31744B57EBFBE8E8EDFA673B65F_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * _thisAdjusted = reinterpret_cast<XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *>(__this + _offset);
	return XRTextureDescriptor_GetHashCode_mE61628A57D74C31744B57EBFBE8E8EDFA673B65F(_thisAdjusted, method);
}
// System.String UnityEngine.XR.ARSubsystems.XRTextureDescriptor::ToString()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* XRTextureDescriptor_ToString_mA7C17125D54876E04397C7022031B6A346CA9A7F (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRTextureDescriptor_ToString_mA7C17125D54876E04397C7022031B6A346CA9A7F_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* L_0 = (ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A*)(ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A*)SZArrayNew(ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A_il2cpp_TypeInfo_var, (uint32_t)6);
		ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* L_1 = L_0;
		intptr_t* L_2 = __this->get_address_of_m_NativeTexture_0();
		String_t* L_3 = IntPtr_ToString_m6ADB8DBD989D878D694B4031CC08461B1E2C51FF((intptr_t*)L_2, _stringLiteral7C920AC9C27322B466EC79E3F70C59D0EB2E27E3, /*hidden argument*/NULL);
		NullCheck(L_1);
		ArrayElementTypeCheck (L_1, L_3);
		(L_1)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject *)L_3);
		ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* L_4 = L_1;
		int32_t* L_5 = __this->get_address_of_m_Width_1();
		String_t* L_6 = Int32_ToString_m1863896DE712BF97C031D55B12E1583F1982DC02((int32_t*)L_5, /*hidden argument*/NULL);
		NullCheck(L_4);
		ArrayElementTypeCheck (L_4, L_6);
		(L_4)->SetAt(static_cast<il2cpp_array_size_t>(1), (RuntimeObject *)L_6);
		ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* L_7 = L_4;
		int32_t* L_8 = __this->get_address_of_m_Height_2();
		String_t* L_9 = Int32_ToString_m1863896DE712BF97C031D55B12E1583F1982DC02((int32_t*)L_8, /*hidden argument*/NULL);
		NullCheck(L_7);
		ArrayElementTypeCheck (L_7, L_9);
		(L_7)->SetAt(static_cast<il2cpp_array_size_t>(2), (RuntimeObject *)L_9);
		ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* L_10 = L_7;
		int32_t* L_11 = __this->get_address_of_m_MipmapCount_3();
		String_t* L_12 = Int32_ToString_m1863896DE712BF97C031D55B12E1583F1982DC02((int32_t*)L_11, /*hidden argument*/NULL);
		NullCheck(L_10);
		ArrayElementTypeCheck (L_10, L_12);
		(L_10)->SetAt(static_cast<il2cpp_array_size_t>(3), (RuntimeObject *)L_12);
		ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* L_13 = L_10;
		int32_t* L_14 = __this->get_address_of_m_Format_4();
		RuntimeObject * L_15 = Box(TextureFormat_t7C6B5101554065C47682E592D1E26079D4EC2DCE_il2cpp_TypeInfo_var, L_14);
		NullCheck(L_15);
		String_t* L_16 = VirtFuncInvoker0< String_t* >::Invoke(3 /* System.String System.Object::ToString() */, L_15);
		*L_14 = *(int32_t*)UnBox(L_15);
		NullCheck(L_13);
		ArrayElementTypeCheck (L_13, L_16);
		(L_13)->SetAt(static_cast<il2cpp_array_size_t>(4), (RuntimeObject *)L_16);
		ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* L_17 = L_13;
		int32_t* L_18 = __this->get_address_of_m_PropertyNameId_5();
		String_t* L_19 = Int32_ToString_m1863896DE712BF97C031D55B12E1583F1982DC02((int32_t*)L_18, /*hidden argument*/NULL);
		NullCheck(L_17);
		ArrayElementTypeCheck (L_17, L_19);
		(L_17)->SetAt(static_cast<il2cpp_array_size_t>(5), (RuntimeObject *)L_19);
		String_t* L_20 = String_Format_mA3AC3FE7B23D97F3A5BAA082D25B0E01B341A865(_stringLiteral4A5C099E77D1F0180583C811D9E0FFDBBD8056EE, L_17, /*hidden argument*/NULL);
		return L_20;
	}
}
IL2CPP_EXTERN_C  String_t* XRTextureDescriptor_ToString_mA7C17125D54876E04397C7022031B6A346CA9A7F_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * _thisAdjusted = reinterpret_cast<XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD *>(__this + _offset);
	return XRTextureDescriptor_ToString_mA7C17125D54876E04397C7022031B6A346CA9A7F(_thisAdjusted, method);
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void UnityEngine.XR.ARSubsystems.XRTrackedImage::.ctor(UnityEngine.XR.ARSubsystems.TrackableId,System.Guid,UnityEngine.Pose,UnityEngine.Vector2,UnityEngine.XR.ARSubsystems.TrackingState,System.IntPtr)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRTrackedImage__ctor_mF31D86D7A523FD9EE7F4166A9ABB04272E93436B (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___trackableId0, Guid_t  ___sourceImageId1, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___pose2, Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  ___size3, int32_t ___trackingState4, intptr_t ___nativePtr5, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = ___trackableId0;
		__this->set_m_Id_1(L_0);
		Guid_t  L_1 = ___sourceImageId1;
		__this->set_m_SourceImageId_2(L_1);
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_2 = ___pose2;
		__this->set_m_Pose_3(L_2);
		Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  L_3 = ___size3;
		__this->set_m_Size_4(L_3);
		int32_t L_4 = ___trackingState4;
		__this->set_m_TrackingState_5(L_4);
		intptr_t L_5 = ___nativePtr5;
		__this->set_m_NativePtr_6((intptr_t)L_5);
		return;
	}
}
IL2CPP_EXTERN_C  void XRTrackedImage__ctor_mF31D86D7A523FD9EE7F4166A9ABB04272E93436B_AdjustorThunk (RuntimeObject * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___trackableId0, Guid_t  ___sourceImageId1, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___pose2, Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  ___size3, int32_t ___trackingState4, intptr_t ___nativePtr5, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * _thisAdjusted = reinterpret_cast<XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 *>(__this + _offset);
	XRTrackedImage__ctor_mF31D86D7A523FD9EE7F4166A9ABB04272E93436B(_thisAdjusted, ___trackableId0, ___sourceImageId1, ___pose2, ___size3, ___trackingState4, ___nativePtr5, method);
}
// UnityEngine.XR.ARSubsystems.XRTrackedImage UnityEngine.XR.ARSubsystems.XRTrackedImage::get_defaultValue()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  XRTrackedImage_get_defaultValue_mC27C0C8BAC99DFBD1900C92FBA0D4940D86468EE (const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRTrackedImage_get_defaultValue_mC27C0C8BAC99DFBD1900C92FBA0D4940D86468EE_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		IL2CPP_RUNTIME_CLASS_INIT(XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8_il2cpp_TypeInfo_var);
		XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  L_0 = ((XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8_StaticFields*)il2cpp_codegen_static_fields_for(XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8_il2cpp_TypeInfo_var))->get_s_Default_0();
		return L_0;
	}
}
// UnityEngine.XR.ARSubsystems.TrackableId UnityEngine.XR.ARSubsystems.XRTrackedImage::get_trackableId()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  XRTrackedImage_get_trackableId_m6EB6DBACC95E5EE2AFEE3CE421F4C123F32E9CB8 (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = __this->get_m_Id_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C  TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  XRTrackedImage_get_trackableId_m6EB6DBACC95E5EE2AFEE3CE421F4C123F32E9CB8_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * _thisAdjusted = reinterpret_cast<XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 *>(__this + _offset);
	return XRTrackedImage_get_trackableId_m6EB6DBACC95E5EE2AFEE3CE421F4C123F32E9CB8_inline(_thisAdjusted, method);
}
// System.Guid UnityEngine.XR.ARSubsystems.XRTrackedImage::get_sourceImageId()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Guid_t  XRTrackedImage_get_sourceImageId_m402089FA779BB9821B50B23F79579466D895939B (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method)
{
	{
		Guid_t  L_0 = __this->get_m_SourceImageId_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C  Guid_t  XRTrackedImage_get_sourceImageId_m402089FA779BB9821B50B23F79579466D895939B_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * _thisAdjusted = reinterpret_cast<XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 *>(__this + _offset);
	return XRTrackedImage_get_sourceImageId_m402089FA779BB9821B50B23F79579466D895939B_inline(_thisAdjusted, method);
}
// UnityEngine.Pose UnityEngine.XR.ARSubsystems.XRTrackedImage::get_pose()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  XRTrackedImage_get_pose_m0566E087CA2DC99DF749E80277510C61DCF13186 (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method)
{
	{
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_0 = __this->get_m_Pose_3();
		return L_0;
	}
}
IL2CPP_EXTERN_C  Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  XRTrackedImage_get_pose_m0566E087CA2DC99DF749E80277510C61DCF13186_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * _thisAdjusted = reinterpret_cast<XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 *>(__this + _offset);
	return XRTrackedImage_get_pose_m0566E087CA2DC99DF749E80277510C61DCF13186_inline(_thisAdjusted, method);
}
// UnityEngine.Vector2 UnityEngine.XR.ARSubsystems.XRTrackedImage::get_size()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  XRTrackedImage_get_size_m746034D0E2FD28C9E48A90965E4FCD9137988906 (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method)
{
	{
		Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  L_0 = __this->get_m_Size_4();
		return L_0;
	}
}
IL2CPP_EXTERN_C  Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  XRTrackedImage_get_size_m746034D0E2FD28C9E48A90965E4FCD9137988906_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * _thisAdjusted = reinterpret_cast<XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 *>(__this + _offset);
	return XRTrackedImage_get_size_m746034D0E2FD28C9E48A90965E4FCD9137988906_inline(_thisAdjusted, method);
}
// UnityEngine.XR.ARSubsystems.TrackingState UnityEngine.XR.ARSubsystems.XRTrackedImage::get_trackingState()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRTrackedImage_get_trackingState_mA7177B042E8F9F9B584582970BC5FF0377CE94DB (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_TrackingState_5();
		return L_0;
	}
}
IL2CPP_EXTERN_C  int32_t XRTrackedImage_get_trackingState_mA7177B042E8F9F9B584582970BC5FF0377CE94DB_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * _thisAdjusted = reinterpret_cast<XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 *>(__this + _offset);
	return XRTrackedImage_get_trackingState_mA7177B042E8F9F9B584582970BC5FF0377CE94DB_inline(_thisAdjusted, method);
}
// System.IntPtr UnityEngine.XR.ARSubsystems.XRTrackedImage::get_nativePtr()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR intptr_t XRTrackedImage_get_nativePtr_mB44BA43B02762B89091D56F254221F0741808629 (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method)
{
	{
		intptr_t L_0 = __this->get_m_NativePtr_6();
		return (intptr_t)L_0;
	}
}
IL2CPP_EXTERN_C  intptr_t XRTrackedImage_get_nativePtr_mB44BA43B02762B89091D56F254221F0741808629_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * _thisAdjusted = reinterpret_cast<XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 *>(__this + _offset);
	return XRTrackedImage_get_nativePtr_mB44BA43B02762B89091D56F254221F0741808629_inline(_thisAdjusted, method);
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRTrackedImage::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRTrackedImage_GetHashCode_mC1A5AB6C756498852952CB1B9F4F69D1177A02A6 (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 * L_0 = __this->get_address_of_m_Id_1();
		int32_t L_1 = TrackableId_GetHashCode_m89E7236D11700A1FAF335918CA65CDEB1BF4D973((TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 *)L_0, /*hidden argument*/NULL);
		Guid_t * L_2 = __this->get_address_of_m_SourceImageId_2();
		int32_t L_3 = Guid_GetHashCode_mEB01C6BA267B1CCD624BCA91D09B803C9B6E5369((Guid_t *)L_2, /*hidden argument*/NULL);
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 * L_4 = __this->get_address_of_m_Pose_3();
		int32_t L_5 = Pose_GetHashCode_m17AC0D28F5BD43DE0CCFA4CC1A870C525E0D6066((Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 *)L_4, /*hidden argument*/NULL);
		Vector2_tA85D2DD88578276CA8A8796756458277E72D073D * L_6 = __this->get_address_of_m_Size_4();
		int32_t L_7 = Vector2_GetHashCode_m028AB6B14EBC6D668CFA45BF6EDEF17E2C44EA54((Vector2_tA85D2DD88578276CA8A8796756458277E72D073D *)L_6, /*hidden argument*/NULL);
		int32_t* L_8 = __this->get_address_of_m_TrackingState_5();
		int32_t L_9 = Int32_GetHashCode_m245C424ECE351E5FE3277A88EEB02132DAB8C25A((int32_t*)L_8, /*hidden argument*/NULL);
		return ((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)L_1, (int32_t)((int32_t)486187739))), (int32_t)L_3)), (int32_t)((int32_t)486187739))), (int32_t)L_5)), (int32_t)((int32_t)486187739))), (int32_t)L_7)), (int32_t)((int32_t)486187739))), (int32_t)L_9));
	}
}
IL2CPP_EXTERN_C  int32_t XRTrackedImage_GetHashCode_mC1A5AB6C756498852952CB1B9F4F69D1177A02A6_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * _thisAdjusted = reinterpret_cast<XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 *>(__this + _offset);
	return XRTrackedImage_GetHashCode_mC1A5AB6C756498852952CB1B9F4F69D1177A02A6(_thisAdjusted, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRTrackedImage::Equals(UnityEngine.XR.ARSubsystems.XRTrackedImage)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTrackedImage_Equals_m12A588942242306FC770FD88421D00750F22A141 (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  ___other0, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 * L_0 = __this->get_address_of_m_Id_1();
		XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  L_1 = ___other0;
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_2 = L_1.get_m_Id_1();
		bool L_3 = TrackableId_Equals_mCE458E0FDCDD6E339FCC1926EE88EB7B3D45F943((TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 *)L_0, L_2, /*hidden argument*/NULL);
		if (!L_3)
		{
			goto IL_005b;
		}
	}
	{
		Guid_t * L_4 = __this->get_address_of_m_SourceImageId_2();
		XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  L_5 = ___other0;
		Guid_t  L_6 = L_5.get_m_SourceImageId_2();
		bool L_7 = Guid_Equals_mC7FC66A530A8B6FC95E8F5F9E34AE81FD44CD245((Guid_t *)L_4, L_6, /*hidden argument*/NULL);
		if (!L_7)
		{
			goto IL_005b;
		}
	}
	{
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 * L_8 = __this->get_address_of_m_Pose_3();
		XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  L_9 = ___other0;
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_10 = L_9.get_m_Pose_3();
		bool L_11 = Pose_Equals_m867264C8DF91FF8DC3AD957EF1625902CDEBAEDD((Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29 *)L_8, L_10, /*hidden argument*/NULL);
		if (!L_11)
		{
			goto IL_005b;
		}
	}
	{
		Vector2_tA85D2DD88578276CA8A8796756458277E72D073D * L_12 = __this->get_address_of_m_Size_4();
		XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  L_13 = ___other0;
		Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  L_14 = L_13.get_m_Size_4();
		bool L_15 = Vector2_Equals_mD6BF1A738E3CAF57BB46E604B030C072728F4EEB((Vector2_tA85D2DD88578276CA8A8796756458277E72D073D *)L_12, L_14, /*hidden argument*/NULL);
		if (!L_15)
		{
			goto IL_005b;
		}
	}
	{
		int32_t L_16 = __this->get_m_TrackingState_5();
		XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  L_17 = ___other0;
		int32_t L_18 = L_17.get_m_TrackingState_5();
		return (bool)((((int32_t)L_16) == ((int32_t)L_18))? 1 : 0);
	}

IL_005b:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool XRTrackedImage_Equals_m12A588942242306FC770FD88421D00750F22A141_AdjustorThunk (RuntimeObject * __this, XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  ___other0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * _thisAdjusted = reinterpret_cast<XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 *>(__this + _offset);
	return XRTrackedImage_Equals_m12A588942242306FC770FD88421D00750F22A141(_thisAdjusted, ___other0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRTrackedImage::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTrackedImage_Equals_m7C7F0B2FC7A6818276C2BC763CF0465333453B9C (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRTrackedImage_Equals_m7C7F0B2FC7A6818276C2BC763CF0465333453B9C_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject * L_0 = ___obj0;
		if (!((RuntimeObject *)IsInstSealed((RuntimeObject*)L_0, XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8_il2cpp_TypeInfo_var)))
		{
			goto IL_0015;
		}
	}
	{
		RuntimeObject * L_1 = ___obj0;
		bool L_2 = XRTrackedImage_Equals_m12A588942242306FC770FD88421D00750F22A141((XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 *)__this, ((*(XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 *)((XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 *)UnBox(L_1, XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8_il2cpp_TypeInfo_var)))), /*hidden argument*/NULL);
		return L_2;
	}

IL_0015:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool XRTrackedImage_Equals_m7C7F0B2FC7A6818276C2BC763CF0465333453B9C_AdjustorThunk (RuntimeObject * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * _thisAdjusted = reinterpret_cast<XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 *>(__this + _offset);
	return XRTrackedImage_Equals_m7C7F0B2FC7A6818276C2BC763CF0465333453B9C(_thisAdjusted, ___obj0, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRTrackedImage::op_Equality(UnityEngine.XR.ARSubsystems.XRTrackedImage,UnityEngine.XR.ARSubsystems.XRTrackedImage)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTrackedImage_op_Equality_m8C52E2C73BB01445DA64A954189A25E1C6B162AA (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  ___lhs0, XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  ___rhs1, const RuntimeMethod* method)
{
	{
		XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  L_0 = ___rhs1;
		bool L_1 = XRTrackedImage_Equals_m12A588942242306FC770FD88421D00750F22A141((XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return L_1;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRTrackedImage::op_Inequality(UnityEngine.XR.ARSubsystems.XRTrackedImage,UnityEngine.XR.ARSubsystems.XRTrackedImage)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTrackedImage_op_Inequality_mDC56A7B7605F26C5D1D049FE1D68D4463155F847 (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  ___lhs0, XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  ___rhs1, const RuntimeMethod* method)
{
	{
		XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  L_0 = ___rhs1;
		bool L_1 = XRTrackedImage_Equals_m12A588942242306FC770FD88421D00750F22A141((XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return (bool)((((int32_t)L_1) == ((int32_t)0))? 1 : 0);
	}
}
// System.Void UnityEngine.XR.ARSubsystems.XRTrackedImage::.cctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRTrackedImage__cctor_m4D42652FA025B44DA4EEAF27F15B77E11DAF4614 (const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRTrackedImage__cctor_m4D42652FA025B44DA4EEAF27F15B77E11DAF4614_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  V_0;
	memset((&V_0), 0, sizeof(V_0));
	{
		il2cpp_codegen_initobj((&V_0), sizeof(XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 ));
		IL2CPP_RUNTIME_CLASS_INIT(TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47_il2cpp_TypeInfo_var);
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = TrackableId_get_invalidId_mBE9FA1EC8F2EC1575C1B31666EA928A3382DF1CD_inline(/*hidden argument*/NULL);
		(&V_0)->set_m_Id_1(L_0);
		IL2CPP_RUNTIME_CLASS_INIT(Guid_t_il2cpp_TypeInfo_var);
		Guid_t  L_1 = ((Guid_t_StaticFields*)il2cpp_codegen_static_fields_for(Guid_t_il2cpp_TypeInfo_var))->get_Empty_0();
		(&V_0)->set_m_SourceImageId_2(L_1);
		IL2CPP_RUNTIME_CLASS_INIT(Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29_il2cpp_TypeInfo_var);
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_2 = Pose_get_identity_m19458DF84EAADD5E9302CABDC385B97DC91ECCBF(/*hidden argument*/NULL);
		(&V_0)->set_m_Pose_3(L_2);
		XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8  L_3 = V_0;
		((XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8_StaticFields*)il2cpp_codegen_static_fields_for(XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8_il2cpp_TypeInfo_var))->set_s_Default_0(L_3);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// UnityEngine.XR.ARSubsystems.XRTrackedObject UnityEngine.XR.ARSubsystems.XRTrackedObject::get_defaultValue()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260  XRTrackedObject_get_defaultValue_m4623361129019EE5722A95C580171705EA1F3901 (const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRTrackedObject_get_defaultValue_m4623361129019EE5722A95C580171705EA1F3901_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		IL2CPP_RUNTIME_CLASS_INIT(XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260_il2cpp_TypeInfo_var);
		XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260  L_0 = ((XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260_StaticFields*)il2cpp_codegen_static_fields_for(XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260_il2cpp_TypeInfo_var))->get_s_Default_5();
		return L_0;
	}
}
// UnityEngine.XR.ARSubsystems.TrackableId UnityEngine.XR.ARSubsystems.XRTrackedObject::get_trackableId()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  XRTrackedObject_get_trackableId_mB720981791DE599B20879640517A33BE2FE2D84D (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = __this->get_m_TrackableId_0();
		return L_0;
	}
}
IL2CPP_EXTERN_C  TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  XRTrackedObject_get_trackableId_mB720981791DE599B20879640517A33BE2FE2D84D_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * _thisAdjusted = reinterpret_cast<XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 *>(__this + _offset);
	return XRTrackedObject_get_trackableId_mB720981791DE599B20879640517A33BE2FE2D84D_inline(_thisAdjusted, method);
}
// UnityEngine.Pose UnityEngine.XR.ARSubsystems.XRTrackedObject::get_pose()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  XRTrackedObject_get_pose_mF865EAF61AE8767D6A0CCF59494A51F2D670F603 (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, const RuntimeMethod* method)
{
	{
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_0 = __this->get_m_Pose_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C  Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  XRTrackedObject_get_pose_mF865EAF61AE8767D6A0CCF59494A51F2D670F603_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * _thisAdjusted = reinterpret_cast<XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 *>(__this + _offset);
	return XRTrackedObject_get_pose_mF865EAF61AE8767D6A0CCF59494A51F2D670F603_inline(_thisAdjusted, method);
}
// UnityEngine.XR.ARSubsystems.TrackingState UnityEngine.XR.ARSubsystems.XRTrackedObject::get_trackingState()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRTrackedObject_get_trackingState_m0BD1D36132D7B57151A4CAE07B94238B2AEF3DED (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_TrackingState_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C  int32_t XRTrackedObject_get_trackingState_m0BD1D36132D7B57151A4CAE07B94238B2AEF3DED_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * _thisAdjusted = reinterpret_cast<XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 *>(__this + _offset);
	return XRTrackedObject_get_trackingState_m0BD1D36132D7B57151A4CAE07B94238B2AEF3DED_inline(_thisAdjusted, method);
}
// System.IntPtr UnityEngine.XR.ARSubsystems.XRTrackedObject::get_nativePtr()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR intptr_t XRTrackedObject_get_nativePtr_mD654B09F24E79E99FA2A6B1A95C4EAFDF09C639F (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, const RuntimeMethod* method)
{
	{
		intptr_t L_0 = __this->get_m_NativePtr_3();
		return (intptr_t)L_0;
	}
}
IL2CPP_EXTERN_C  intptr_t XRTrackedObject_get_nativePtr_mD654B09F24E79E99FA2A6B1A95C4EAFDF09C639F_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * _thisAdjusted = reinterpret_cast<XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 *>(__this + _offset);
	return XRTrackedObject_get_nativePtr_mD654B09F24E79E99FA2A6B1A95C4EAFDF09C639F_inline(_thisAdjusted, method);
}
// System.Guid UnityEngine.XR.ARSubsystems.XRTrackedObject::get_referenceObjectGuid()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Guid_t  XRTrackedObject_get_referenceObjectGuid_m09514BB6AD9782AF342076F85BB28631C458BDC8 (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, const RuntimeMethod* method)
{
	{
		Guid_t  L_0 = __this->get_m_ReferenceObjectGuid_4();
		return L_0;
	}
}
IL2CPP_EXTERN_C  Guid_t  XRTrackedObject_get_referenceObjectGuid_m09514BB6AD9782AF342076F85BB28631C458BDC8_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * _thisAdjusted = reinterpret_cast<XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 *>(__this + _offset);
	return XRTrackedObject_get_referenceObjectGuid_m09514BB6AD9782AF342076F85BB28631C458BDC8_inline(_thisAdjusted, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRTrackedObject::.ctor(UnityEngine.XR.ARSubsystems.TrackableId,UnityEngine.Pose,UnityEngine.XR.ARSubsystems.TrackingState,System.IntPtr,System.Guid)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRTrackedObject__ctor_m81B6436D0E3BA4E73E1445074972DB81E3D27275 (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___trackableId0, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___pose1, int32_t ___trackingState2, intptr_t ___nativePtr3, Guid_t  ___referenceObjectGuid4, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = ___trackableId0;
		__this->set_m_TrackableId_0(L_0);
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_1 = ___pose1;
		__this->set_m_Pose_1(L_1);
		int32_t L_2 = ___trackingState2;
		__this->set_m_TrackingState_2(L_2);
		intptr_t L_3 = ___nativePtr3;
		__this->set_m_NativePtr_3((intptr_t)L_3);
		Guid_t  L_4 = ___referenceObjectGuid4;
		__this->set_m_ReferenceObjectGuid_4(L_4);
		return;
	}
}
IL2CPP_EXTERN_C  void XRTrackedObject__ctor_m81B6436D0E3BA4E73E1445074972DB81E3D27275_AdjustorThunk (RuntimeObject * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___trackableId0, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___pose1, int32_t ___trackingState2, intptr_t ___nativePtr3, Guid_t  ___referenceObjectGuid4, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * _thisAdjusted = reinterpret_cast<XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 *>(__this + _offset);
	XRTrackedObject__ctor_m81B6436D0E3BA4E73E1445074972DB81E3D27275(_thisAdjusted, ___trackableId0, ___pose1, ___trackingState2, ___nativePtr3, ___referenceObjectGuid4, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRTrackedObject::Equals(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTrackedObject_Equals_mF0CA07E970C48D514F2B9BBEC0FE44F46429C524 (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRTrackedObject_Equals_mF0CA07E970C48D514F2B9BBEC0FE44F46429C524_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject * L_0 = ___obj0;
		if (!((RuntimeObject *)IsInstSealed((RuntimeObject*)L_0, XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260_il2cpp_TypeInfo_var)))
		{
			goto IL_0015;
		}
	}
	{
		RuntimeObject * L_1 = ___obj0;
		bool L_2 = XRTrackedObject_Equals_m925ED652F271F772E282C3621290411A259CBEEE((XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 *)__this, ((*(XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 *)((XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 *)UnBox(L_1, XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260_il2cpp_TypeInfo_var)))), /*hidden argument*/NULL);
		return L_2;
	}

IL_0015:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool XRTrackedObject_Equals_mF0CA07E970C48D514F2B9BBEC0FE44F46429C524_AdjustorThunk (RuntimeObject * __this, RuntimeObject * ___obj0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * _thisAdjusted = reinterpret_cast<XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 *>(__this + _offset);
	return XRTrackedObject_Equals_mF0CA07E970C48D514F2B9BBEC0FE44F46429C524(_thisAdjusted, ___obj0, method);
}
// System.Int32 UnityEngine.XR.ARSubsystems.XRTrackedObject::GetHashCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XRTrackedObject_GetHashCode_m2F1509AA89026BB34BFFE2C07529AAB3B5B0A429 (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 * L_0 = __this->get_address_of_m_TrackableId_0();
		int32_t L_1 = TrackableId_GetHashCode_m89E7236D11700A1FAF335918CA65CDEB1BF4D973((TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 *)L_0, /*hidden argument*/NULL);
		return L_1;
	}
}
IL2CPP_EXTERN_C  int32_t XRTrackedObject_GetHashCode_m2F1509AA89026BB34BFFE2C07529AAB3B5B0A429_AdjustorThunk (RuntimeObject * __this, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * _thisAdjusted = reinterpret_cast<XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 *>(__this + _offset);
	return XRTrackedObject_GetHashCode_m2F1509AA89026BB34BFFE2C07529AAB3B5B0A429(_thisAdjusted, method);
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRTrackedObject::op_Equality(UnityEngine.XR.ARSubsystems.XRTrackedObject,UnityEngine.XR.ARSubsystems.XRTrackedObject)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTrackedObject_op_Equality_m06220676F2AB319883E5895019E7010622DE9583 (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260  ___lhs0, XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260  ___rhs1, const RuntimeMethod* method)
{
	{
		XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260  L_0 = ___rhs1;
		bool L_1 = XRTrackedObject_Equals_m925ED652F271F772E282C3621290411A259CBEEE((XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return L_1;
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRTrackedObject::op_Inequality(UnityEngine.XR.ARSubsystems.XRTrackedObject,UnityEngine.XR.ARSubsystems.XRTrackedObject)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTrackedObject_op_Inequality_m05EF7C266FC336DCCA28A984954021CE67818E40 (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260  ___lhs0, XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260  ___rhs1, const RuntimeMethod* method)
{
	{
		XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260  L_0 = ___rhs1;
		bool L_1 = XRTrackedObject_Equals_m925ED652F271F772E282C3621290411A259CBEEE((XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 *)(&___lhs0), L_0, /*hidden argument*/NULL);
		return (bool)((((int32_t)L_1) == ((int32_t)0))? 1 : 0);
	}
}
// System.Boolean UnityEngine.XR.ARSubsystems.XRTrackedObject::Equals(UnityEngine.XR.ARSubsystems.XRTrackedObject)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XRTrackedObject_Equals_m925ED652F271F772E282C3621290411A259CBEEE (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260  ___other0, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 * L_0 = __this->get_address_of_m_TrackableId_0();
		XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260  L_1 = ___other0;
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_2 = L_1.get_m_TrackableId_0();
		bool L_3 = TrackableId_Equals_mCE458E0FDCDD6E339FCC1926EE88EB7B3D45F943((TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47 *)L_0, L_2, /*hidden argument*/NULL);
		return L_3;
	}
}
IL2CPP_EXTERN_C  bool XRTrackedObject_Equals_m925ED652F271F772E282C3621290411A259CBEEE_AdjustorThunk (RuntimeObject * __this, XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260  ___other0, const RuntimeMethod* method)
{
	int32_t _offset = 1;
	XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * _thisAdjusted = reinterpret_cast<XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 *>(__this + _offset);
	return XRTrackedObject_Equals_m925ED652F271F772E282C3621290411A259CBEEE(_thisAdjusted, ___other0, method);
}
// System.Void UnityEngine.XR.ARSubsystems.XRTrackedObject::.cctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XRTrackedObject__cctor_mF6797A036790C2B6133B8B8A44C64B49FDBFF296 (const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRTrackedObject__cctor_mF6797A036790C2B6133B8B8A44C64B49FDBFF296_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260  V_0;
	memset((&V_0), 0, sizeof(V_0));
	{
		il2cpp_codegen_initobj((&V_0), sizeof(XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 ));
		IL2CPP_RUNTIME_CLASS_INIT(TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47_il2cpp_TypeInfo_var);
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = TrackableId_get_invalidId_mBE9FA1EC8F2EC1575C1B31666EA928A3382DF1CD_inline(/*hidden argument*/NULL);
		(&V_0)->set_m_TrackableId_0(L_0);
		IL2CPP_RUNTIME_CLASS_INIT(Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29_il2cpp_TypeInfo_var);
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_1 = Pose_get_identity_m19458DF84EAADD5E9302CABDC385B97DC91ECCBF(/*hidden argument*/NULL);
		(&V_0)->set_m_Pose_1(L_1);
		IL2CPP_RUNTIME_CLASS_INIT(Guid_t_il2cpp_TypeInfo_var);
		Guid_t  L_2 = ((Guid_t_StaticFields*)il2cpp_codegen_static_fields_for(Guid_t_il2cpp_TypeInfo_var))->get_Empty_0();
		(&V_0)->set_m_ReferenceObjectGuid_4(L_2);
		XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260  L_3 = V_0;
		((XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260_StaticFields*)il2cpp_codegen_static_fields_for(XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260_il2cpp_TypeInfo_var))->set_s_Default_5(L_3);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  XRPointCloudData_get_positions_m2BDA572054D639DB35E9FDA3D15AEF3B7B39D40C_inline (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, const RuntimeMethod* method)
{
	{
		NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  L_0 = __this->get_m_Positions_0();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRPointCloudData_set_positions_m78BB0E1E2A5860DAC6F60D6C9A6A37544FF9880E_inline (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  ___value0, const RuntimeMethod* method)
{
	{
		NativeArray_1_t20B0E97D613353CCC2889E9B3D97A5612374FE74  L_0 = ___value0;
		__this->set_m_Positions_0(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  XRPointCloudData_get_confidenceValues_m156073A1640F58477DBCBAC6BDA05C3BF866ACE6_inline (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, const RuntimeMethod* method)
{
	{
		NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  L_0 = __this->get_m_ConfidenceValues_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRPointCloudData_set_confidenceValues_mC837DE6B63BF8CBD8F2480C1A4ED3247AABCB861_inline (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  ___value0, const RuntimeMethod* method)
{
	{
		NativeArray_1_t2F93EF84A543D826D53EFEAFE52F5C42392D0D67  L_0 = ___value0;
		__this->set_m_ConfidenceValues_1(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  XRPointCloudData_get_identifiers_m6AA5FE2F151300371A1F5A8310A49A0D4A35BD23_inline (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, const RuntimeMethod* method)
{
	{
		NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  L_0 = __this->get_m_Identifiers_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRPointCloudData_set_identifiers_m550B2B8C6EF821D5BBD47C066FF6C961EF0CA562_inline (XRPointCloudData_t790D531E8C51EE83A08A457B436F969CBF9439BA * __this, NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  ___value0, const RuntimeMethod* method)
{
	{
		NativeArray_1_t910D4CAEE55F7F4F3664BD3DB14835AB6D8F5F3C  L_0 = ___value0;
		__this->set_m_Identifiers_2(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  XRRaycastHit_get_trackableId_mAECCB1BE08FB0B5A48CB27D955250FE2068492CF_inline (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = __this->get_m_TrackableId_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRRaycastHit_set_trackableId_mD5381CB555237421AA3A1A4F42BDBA66C2CEE77F_inline (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  ___value0, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = ___value0;
		__this->set_m_TrackableId_1(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  XRRaycastHit_get_pose_mE0B0A754E818C6FF3675A41CA95185A3E608C8C3_inline (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, const RuntimeMethod* method)
{
	{
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_0 = __this->get_m_Pose_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRRaycastHit_set_pose_m3E6F13DE1371303DD66CD9D9E8B86500C24C5516_inline (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  ___value0, const RuntimeMethod* method)
{
	{
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_0 = ___value0;
		__this->set_m_Pose_2(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR float XRRaycastHit_get_distance_mC748DE6ED96B0C735DCA4AD320FA0BF522246D19_inline (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, const RuntimeMethod* method)
{
	{
		float L_0 = __this->get_m_Distance_3();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRRaycastHit_set_distance_m53218D1A8CBD8F632F988C439D5F98633A050815_inline (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, float ___value0, const RuntimeMethod* method)
{
	{
		float L_0 = ___value0;
		__this->set_m_Distance_3(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRRaycastHit_get_hitType_m52BBF5DBDE1B3E7E01571EE029F68EB29E240DA6_inline (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_HitType_4();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRRaycastHit_set_hitType_m776B39B9226EB310D47FB6A10BA78844AEC4EE58_inline (XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82 * __this, int32_t ___value0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___value0;
		__this->set_m_HitType_4(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  TrackableId_get_invalidId_mBE9FA1EC8F2EC1575C1B31666EA928A3382DF1CD_inline (const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (TrackableId_get_invalidId_mBE9FA1EC8F2EC1575C1B31666EA928A3382DF1CDUnity_XR_ARSubsystems1_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		IL2CPP_RUNTIME_CLASS_INIT(TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47_il2cpp_TypeInfo_var);
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = ((TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47_StaticFields*)il2cpp_codegen_static_fields_for(TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47_il2cpp_TypeInfo_var))->get_s_InvalidId_0();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  XRRaycastHit_get_defaultValue_m17AEBDAC971A56C3FC4C7C4E2E14ECC357658DFA_inline (const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRRaycastHit_get_defaultValue_m17AEBDAC971A56C3FC4C7C4E2E14ECC357658DFAUnity_XR_ARSubsystems1_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		IL2CPP_RUNTIME_CLASS_INIT(XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82_il2cpp_TypeInfo_var);
		XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82  L_0 = ((XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82_StaticFields*)il2cpp_codegen_static_fields_for(XRRaycastHit_t2DE122E601B75E2070D4A542A13E2486DEE60E82_il2cpp_TypeInfo_var))->get_s_Default_0();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR String_t* Cinfo_get_id_m0418C70EB8FBC60BB9B83053AA5175AEFE31CAF8_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, const RuntimeMethod* method)
{
	{
		String_t* L_0 = __this->get_U3CidU3Ek__BackingField_0();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void SubsystemDescriptor_set_id_m3289D5DA5921130F0338C6332907E686E36FF064_inline (SubsystemDescriptor_t889FE96B75F157379A8337541A119F08DD9C75D4 * __this, String_t* ___value0, const RuntimeMethod* method)
{
	{
		String_t* L_0 = ___value0;
		__this->set_U3CidU3Ek__BackingField_0(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Type_t * Cinfo_get_subsystemImplementationType_m0E0FEE226FD08939BF83B7B2644BEC6362BA157B_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, const RuntimeMethod* method)
{
	{
		Type_t * L_0 = __this->get_U3CsubsystemImplementationTypeU3Ek__BackingField_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void SubsystemDescriptor_set_subsystemImplementationType_m727AB6BED84E57A382B8804F95A75A8012963EE1_inline (SubsystemDescriptor_t889FE96B75F157379A8337541A119F08DD9C75D4 * __this, Type_t * ___value0, const RuntimeMethod* method)
{
	{
		Type_t * L_0 = ___value0;
		__this->set_U3CsubsystemImplementationTypeU3Ek__BackingField_1(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR bool Cinfo_get_supportsViewportBasedRaycast_m323FE06DA2E4222E6BF4CE89541DB4630CB254B3_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = __this->get_U3CsupportsViewportBasedRaycastU3Ek__BackingField_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRRaycastSubsystemDescriptor_set_supportsViewportBasedRaycast_m7A6EBE60F40966D0314C378B1441C7DB41C0720D_inline (XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7 * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsViewportBasedRaycastU3Ek__BackingField_2(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR bool Cinfo_get_supportsWorldBasedRaycast_mFBB112B068EE22D519CAC45E35255D6FDACCAE74_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = __this->get_U3CsupportsWorldBasedRaycastU3Ek__BackingField_3();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRRaycastSubsystemDescriptor_set_supportsWorldBasedRaycast_m23E91C1C3684B0FE2AF37D3BE2A79B0D88BFC7B3_inline (XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7 * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsWorldBasedRaycastU3Ek__BackingField_3(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t Cinfo_get_supportedTrackableTypes_mCC57E28DFCE93ECA772B1DE2E3E49AD030D79424_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_U3CsupportedTrackableTypesU3Ek__BackingField_4();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRRaycastSubsystemDescriptor_set_supportedTrackableTypes_mA429421E574C9261CFC271AC43A521E43B990DCD_inline (XRRaycastSubsystemDescriptor_tDC6E6E465FE3E4D385429760EF0C84832A09A3D7 * __this, int32_t ___value0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___value0;
		__this->set_U3CsupportedTrackableTypesU3Ek__BackingField_4(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_id_mFFFBB447D6D0DF4A27428D414FE23BCCB16D78D1_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, String_t* ___value0, const RuntimeMethod* method)
{
	{
		String_t* L_0 = ___value0;
		__this->set_U3CidU3Ek__BackingField_0(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_subsystemImplementationType_m00E3D77F5C33C670222169A514C9804886D4FD72_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, Type_t * ___value0, const RuntimeMethod* method)
{
	{
		Type_t * L_0 = ___value0;
		__this->set_U3CsubsystemImplementationTypeU3Ek__BackingField_1(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_supportsViewportBasedRaycast_mBA63D727FCB8E5FE2EF1BD5CA2192B54768DEF0D_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsViewportBasedRaycastU3Ek__BackingField_2(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_supportsWorldBasedRaycast_mDDA30FB5ADF2800F44467B727F19668AAE05AFEF_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsWorldBasedRaycastU3Ek__BackingField_3(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_supportedTrackableTypes_m91638341F04B3BC3688660AFFE66308C19B13C6B_inline (Cinfo_t4760A1C9784E5D057E5B5D48B3C4C0B905471704 * __this, int32_t ___value0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___value0;
		__this->set_U3CsupportedTrackableTypesU3Ek__BackingField_4(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR bool XRReferenceImage_get_specifySize_m5792CAE7DC7ADB8591E770B1F32D71BCCE9F0597_inline (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = __this->get_m_SpecifySize_3();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  XRReferenceImage_get_size_m29A6DA526141F214BE2949524305EFE91C07FA32_inline (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method)
{
	{
		Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  L_0 = __this->get_m_Size_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR String_t* XRReferenceImage_get_name_m6DD83ECED755444645A14816918747E2EEAE92A9_inline (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method)
{
	{
		String_t* L_0 = __this->get_m_Name_4();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Texture2D_tBBF96AC337723E2EF156DF17E09D4379FD05DE1C * XRReferenceImage_get_texture_m97887B57DD747DCE051484D1C97F1240B673FE16_inline (XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E * __this, const RuntimeMethod* method)
{
	{
		Texture2D_tBBF96AC337723E2EF156DF17E09D4379FD05DE1C * L_0 = __this->get_m_Texture_5();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR String_t* XRReferenceObject_get_name_mB9D4C5BF34D4FF064180412CCE6585867BA98718_inline (XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA * __this, const RuntimeMethod* method)
{
	{
		String_t* L_0 = __this->get_m_Name_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  XRReferencePoint_get_trackableId_m6D53542802F2444CE58861B8868274F9A8296D88_inline (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = __this->get_m_Id_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  XRReferencePoint_get_pose_mA4320629B8C7AE23D97FCD8E2C5FB9C9FB6AED9C_inline (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, const RuntimeMethod* method)
{
	{
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_0 = __this->get_m_Pose_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRReferencePoint_get_trackingState_mBA0DB4050B734039D22D0ECF69CD6E8896DF52B8_inline (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_TrackingState_3();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR intptr_t XRReferencePoint_get_nativePtr_mE9EC85AD0E4976145CB0EDC4A74AA5BB076C5789_inline (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, const RuntimeMethod* method)
{
	{
		intptr_t L_0 = __this->get_m_NativePtr_4();
		return (intptr_t)L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Guid_t  XRReferencePoint_get_sessionId_m5DCAF1725B8A29481940252D80634C99A3C2F0D0_inline (XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9 * __this, const RuntimeMethod* method)
{
	{
		Guid_t  L_0 = __this->get_m_SessionId_5();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  XRReferencePoint_get_defaultValue_mCCFAF4140E24AC2FDF1C8D19043E57B6BFEAC0AD_inline (const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (XRReferencePoint_get_defaultValue_mCCFAF4140E24AC2FDF1C8D19043E57B6BFEAC0ADUnity_XR_ARSubsystems1_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		IL2CPP_RUNTIME_CLASS_INIT(XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9_il2cpp_TypeInfo_var);
		XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9  L_0 = ((XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9_StaticFields*)il2cpp_codegen_static_fields_for(XRReferencePoint_tA8592C08A27EC91D9B1FB3B083C95C5D372FF1F9_il2cpp_TypeInfo_var))->get_s_Default_0();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR String_t* Cinfo_get_id_mB1E35C0B52EEAA8EB934C4D3F02465CF8A752015_inline (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, const RuntimeMethod* method)
{
	{
		String_t* L_0 = __this->get_U3CidU3Ek__BackingField_0();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Type_t * Cinfo_get_subsystemImplementationType_m764697AF3D79BDFA6010287A8B542F9323693096_inline (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, const RuntimeMethod* method)
{
	{
		Type_t * L_0 = __this->get_U3CsubsystemImplementationTypeU3Ek__BackingField_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR bool Cinfo_get_supportsTrackableAttachments_m041B030B1BD0114D8FCE9A9F804CFF5984FB07BD_inline (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = __this->get_U3CsupportsTrackableAttachmentsU3Ek__BackingField_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRReferencePointSubsystemDescriptor_set_supportsTrackableAttachments_mA4F7709D4C170D414F862490ABBF0090DE46A8AB_inline (XRReferencePointSubsystemDescriptor_t5972C5E3DEF485A89A3AA7D9D47CCD8A67FDB74D * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsTrackableAttachmentsU3Ek__BackingField_2(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_id_m9CAE75E21B0DAE38A8619D1B04D17EDEC81E97D7_inline (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, String_t* ___value0, const RuntimeMethod* method)
{
	{
		String_t* L_0 = ___value0;
		__this->set_U3CidU3Ek__BackingField_0(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_subsystemImplementationType_m9D5112215C6766E6561E42DA858B7F3D72F0005E_inline (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, Type_t * ___value0, const RuntimeMethod* method)
{
	{
		Type_t * L_0 = ___value0;
		__this->set_U3CsubsystemImplementationTypeU3Ek__BackingField_1(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_supportsTrackableAttachments_mC0A061EDA609485B0E83FC7857E8573C93F38FD7_inline (Cinfo_t763E336A62E286B348AB9B084829CFD16A32D7AD * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsTrackableAttachmentsU3Ek__BackingField_2(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR bool XRSessionSubsystemDescriptor_get_supportsInstall_m2AA89682007FE1D8BB811FD152DE326FF7BB5A99_inline (XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079 * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = __this->get_U3CsupportsInstallU3Ek__BackingField_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR String_t* Cinfo_get_id_m998EB4BD213159391A0A66FD6C002C1CE5CD14E8_inline (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, const RuntimeMethod* method)
{
	{
		String_t* L_0 = __this->get_U3CidU3Ek__BackingField_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Type_t * Cinfo_get_subsystemImplementationType_m63942F2B0929DF14EB7885E7553A9970FBD3E108_inline (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, const RuntimeMethod* method)
{
	{
		Type_t * L_0 = __this->get_U3CsubsystemImplementationTypeU3Ek__BackingField_3();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR bool Cinfo_get_supportsInstall_m5FBFD4D2F10A6A46F66F4EFBC61EA14DE0FEED99_inline (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = __this->get_U3CsupportsInstallU3Ek__BackingField_0();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRSessionSubsystemDescriptor_set_supportsInstall_m010EE3F0CB4B143A90B93C1F10F063FB12546920_inline (XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079 * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsInstallU3Ek__BackingField_2(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR bool Cinfo_get_supportsMatchFrameRate_mE591F09F87EA8F4E5563039C47A6331E6AF31895_inline (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = __this->get_U3CsupportsMatchFrameRateU3Ek__BackingField_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRSessionSubsystemDescriptor_set_supportsMatchFrameRate_mC2B0189D51BF3B64026D01DD6A088052C5D74BFC_inline (XRSessionSubsystemDescriptor_tAB6680BDBC0B281B15C5C9E6F4DA6810CFFBA079 * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsMatchFrameRateU3Ek__BackingField_3(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_supportsInstall_mD74EB42C503AC5393E630A56E3AE579FE1558660_inline (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsInstallU3Ek__BackingField_0(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_supportsMatchFrameRate_m2B92004D3F2E01EA5DDFBF5F928C5604E68B8D21_inline (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CsupportsMatchFrameRateU3Ek__BackingField_1(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_id_mC4FF3C524E18065C55B5142D58FBD58A66479A41_inline (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, String_t* ___value0, const RuntimeMethod* method)
{
	{
		String_t* L_0 = ___value0;
		__this->set_U3CidU3Ek__BackingField_2(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void Cinfo_set_subsystemImplementationType_m3C759AEC2943DE059B20AA7F5A5B932B7432473C_inline (Cinfo_t0D2C3593DAA5642EF84F81D756DBDDACC2E27C1A * __this, Type_t * ___value0, const RuntimeMethod* method)
{
	{
		Type_t * L_0 = ___value0;
		__this->set_U3CsubsystemImplementationTypeU3Ek__BackingField_3(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRSessionUpdateParams_get_screenOrientation_m111C145EA6A683F025DF48C6EA355E37D8974183_inline (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_U3CscreenOrientationU3Ek__BackingField_0();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRSessionUpdateParams_set_screenOrientation_m7C20FD52988E0F21604700B5CDA93FBA63DD28C6_inline (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, int32_t ___value0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___value0;
		__this->set_U3CscreenOrientationU3Ek__BackingField_0(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  XRSessionUpdateParams_get_screenDimensions_m61A9722E272D6292B9C7C093BF7792FB007BF21E_inline (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, const RuntimeMethod* method)
{
	{
		Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  L_0 = __this->get_U3CscreenDimensionsU3Ek__BackingField_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRSessionUpdateParams_set_screenDimensions_m41570268847916BA02DD2427BDDB08B3D466A905_inline (XRSessionUpdateParams_tAA765EB179BD3BAB22FA143AF178D328B30EAD16 * __this, Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  ___value0, const RuntimeMethod* method)
{
	{
		Vector2Int_t339DA203C037FA6BCFC926C36DC2194D52D5F905  L_0 = ___value0;
		__this->set_U3CscreenDimensionsU3Ek__BackingField_1(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR intptr_t XRTextureDescriptor_get_nativeTexture_m83CAA03353C203B7D38618C1963C715F052081F8_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method)
{
	{
		intptr_t L_0 = __this->get_m_NativeTexture_0();
		return (intptr_t)L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRTextureDescriptor_set_nativeTexture_mEF92A3E263840B8F428C314323C20A11F896D907_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, intptr_t ___value0, const RuntimeMethod* method)
{
	{
		intptr_t L_0 = ___value0;
		__this->set_m_NativeTexture_0((intptr_t)L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRTextureDescriptor_get_width_m158B2CEE4A0F56DF263BB642F5E4A3C3CF339E0B_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_Width_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRTextureDescriptor_set_width_m59E159F83238991BAD9838C5835A07E44F6A163E_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, int32_t ___value0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___value0;
		__this->set_m_Width_1(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRTextureDescriptor_get_height_mCE50370000BCF4A70B95344A0731A771401C0894_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_Height_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRTextureDescriptor_set_height_mE690E293BE1FE8009052CC87FA454FB79DE9DF0E_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, int32_t ___value0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___value0;
		__this->set_m_Height_2(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRTextureDescriptor_get_mipmapCount_m491B149D8BBF148B2030214818E237A28D9B6CC4_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_MipmapCount_3();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRTextureDescriptor_set_mipmapCount_m8CC98FD1B188CA92DE7C1C430BF71E11E7AD7858_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, int32_t ___value0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___value0;
		__this->set_m_MipmapCount_3(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRTextureDescriptor_get_format_mA2DA22DC1DEBCAD27A9C69F3374D614DF1C3FA2B_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_Format_4();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRTextureDescriptor_set_format_m2BEDFB4C31E590B2C2AAE7145AEAE714491E0EA6_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, int32_t ___value0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___value0;
		__this->set_m_Format_4(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRTextureDescriptor_get_propertyNameId_mA3A29036B96A64D1C4F147678E60E2BFCAAAAFF0_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_PropertyNameId_5();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR void XRTextureDescriptor_set_propertyNameId_m87654C29B3CEFA71D22E9F1323058334E8338B4F_inline (XRTextureDescriptor_t56503F48CEBC183AF26EE86935E918F31D09E9FD * __this, int32_t ___value0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___value0;
		__this->set_m_PropertyNameId_5(L_0);
		return;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  XRTrackedImage_get_trackableId_m6EB6DBACC95E5EE2AFEE3CE421F4C123F32E9CB8_inline (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = __this->get_m_Id_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Guid_t  XRTrackedImage_get_sourceImageId_m402089FA779BB9821B50B23F79579466D895939B_inline (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method)
{
	{
		Guid_t  L_0 = __this->get_m_SourceImageId_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  XRTrackedImage_get_pose_m0566E087CA2DC99DF749E80277510C61DCF13186_inline (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method)
{
	{
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_0 = __this->get_m_Pose_3();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  XRTrackedImage_get_size_m746034D0E2FD28C9E48A90965E4FCD9137988906_inline (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method)
{
	{
		Vector2_tA85D2DD88578276CA8A8796756458277E72D073D  L_0 = __this->get_m_Size_4();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRTrackedImage_get_trackingState_mA7177B042E8F9F9B584582970BC5FF0377CE94DB_inline (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_TrackingState_5();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR intptr_t XRTrackedImage_get_nativePtr_mB44BA43B02762B89091D56F254221F0741808629_inline (XRTrackedImage_t178EACF5BFA4228DF4EB1899685C533F3F296AA8 * __this, const RuntimeMethod* method)
{
	{
		intptr_t L_0 = __this->get_m_NativePtr_6();
		return (intptr_t)L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  XRTrackedObject_get_trackableId_mB720981791DE599B20879640517A33BE2FE2D84D_inline (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, const RuntimeMethod* method)
{
	{
		TrackableId_tA7E19AFE62176E25E3759548887E9068E1E4AE47  L_0 = __this->get_m_TrackableId_0();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  XRTrackedObject_get_pose_mF865EAF61AE8767D6A0CCF59494A51F2D670F603_inline (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, const RuntimeMethod* method)
{
	{
		Pose_t2997DE3CB3863E4D78FCF42B46FC481818823F29  L_0 = __this->get_m_Pose_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR int32_t XRTrackedObject_get_trackingState_m0BD1D36132D7B57151A4CAE07B94238B2AEF3DED_inline (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_TrackingState_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR intptr_t XRTrackedObject_get_nativePtr_mD654B09F24E79E99FA2A6B1A95C4EAFDF09C639F_inline (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, const RuntimeMethod* method)
{
	{
		intptr_t L_0 = __this->get_m_NativePtr_3();
		return (intptr_t)L_0;
	}
}
IL2CPP_EXTERN_C inline  IL2CPP_METHOD_ATTR Guid_t  XRTrackedObject_get_referenceObjectGuid_m09514BB6AD9782AF342076F85BB28631C458BDC8_inline (XRTrackedObject_tF08D3523D17E214EC3D7DBE8ED5BFE220BAAE260 * __this, const RuntimeMethod* method)
{
	{
		Guid_t  L_0 = __this->get_m_ReferenceObjectGuid_4();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline IL2CPP_METHOD_ATTR bool Nullable_1_get_HasValue_m86766498BBF2DC3AB0128F33AC3F200BF93BB1B6_gshared_inline (Nullable_1_t2D31B61CB1F8452E868737CCA40082AA2D1F58DC * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = (bool)__this->get_has_value_1();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline IL2CPP_METHOD_ATTR int32_t List_1_get_Count_mC10370473AE5833D24A5F9361290A6B38CC73C32_gshared_inline (List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = (int32_t)__this->get__size_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline IL2CPP_METHOD_ATTR XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  List_1_get_Item_m0935B6D4B21A74A42474E6C822B5DA4F8A32FE00_gshared_inline (List_1_t13506773B6C03CE4C5D47127B81B3031675EE51F * __this, int32_t ___index0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___index0;
		int32_t L_1 = (int32_t)__this->get__size_2();
		if ((!(((uint32_t)L_0) >= ((uint32_t)L_1))))
		{
			goto IL_000e;
		}
	}
	{
		ThrowHelper_ThrowArgumentOutOfRangeException_mBA2AF20A35144E0C43CD721A22EAC9FCA15D6550(/*hidden argument*/NULL);
	}

IL_000e:
	{
		XRReferenceImageU5BU5D_t1EEAB0EDA5828C38140B4D8D48E11247C4789BCE* L_2 = (XRReferenceImageU5BU5D_t1EEAB0EDA5828C38140B4D8D48E11247C4789BCE*)__this->get__items_1();
		int32_t L_3 = ___index0;
		XRReferenceImage_t5A53387AC6253D5D3DFD62BC583A45BBDFC1347E  L_4 = IL2CPP_ARRAY_UNSAFE_LOAD((XRReferenceImageU5BU5D_t1EEAB0EDA5828C38140B4D8D48E11247C4789BCE*)L_2, (int32_t)L_3);
		return L_4;
	}
}
IL2CPP_EXTERN_C inline IL2CPP_METHOD_ATTR RuntimeObject * Enumerator_get_Current_mD7829C7E8CFBEDD463B15A951CDE9B90A12CC55C_gshared_inline (Enumerator_tE0C99528D3DCE5863566CE37BD94162A4C0431CD * __this, const RuntimeMethod* method)
{
	{
		RuntimeObject * L_0 = (RuntimeObject *)__this->get_current_3();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline IL2CPP_METHOD_ATTR int32_t List_1_get_Count_mA303D95DAA9717D8D3148C3F20FA1B2750CE77B4_gshared_inline (List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = (int32_t)__this->get__size_2();
		return L_0;
	}
}
IL2CPP_EXTERN_C inline IL2CPP_METHOD_ATTR XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  List_1_get_Item_mE2BB4CDB177C0DA1EF1FB0A174459691F207C7DF_gshared_inline (List_1_tBCCE21E4A74A6344171668FB521DE76F9004EE79 * __this, int32_t ___index0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___index0;
		int32_t L_1 = (int32_t)__this->get__size_2();
		if ((!(((uint32_t)L_0) >= ((uint32_t)L_1))))
		{
			goto IL_000e;
		}
	}
	{
		ThrowHelper_ThrowArgumentOutOfRangeException_mBA2AF20A35144E0C43CD721A22EAC9FCA15D6550(/*hidden argument*/NULL);
	}

IL_000e:
	{
		XRReferenceObjectU5BU5D_t0AF9158E0468305ABEAA331C8925977310FBD498* L_2 = (XRReferenceObjectU5BU5D_t0AF9158E0468305ABEAA331C8925977310FBD498*)__this->get__items_1();
		int32_t L_3 = ___index0;
		XRReferenceObject_t4DAB86DD7C1E4245ABD473120BD1625265465BAA  L_4 = IL2CPP_ARRAY_UNSAFE_LOAD((XRReferenceObjectU5BU5D_t0AF9158E0468305ABEAA331C8925977310FBD498*)L_2, (int32_t)L_3);
		return L_4;
	}
}
