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

struct VirtActionInvoker0
{
	typedef void (*Action)(void*, const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeObject* obj)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		((Action)invokeData.methodPtr)(obj, invokeData.method);
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
struct GenericVirtActionInvoker0
{
	typedef void (*Action)(void*, const RuntimeMethod*);

	static inline void Invoke (const RuntimeMethod* method, RuntimeObject* obj)
	{
		VirtualInvokeData invokeData;
		il2cpp_codegen_get_generic_virtual_invoke_data(method, obj, &invokeData);
		((Action)invokeData.methodPtr)(obj, invokeData.method);
	}
};
template <typename T1>
struct GenericVirtActionInvoker1
{
	typedef void (*Action)(void*, T1, const RuntimeMethod*);

	static inline void Invoke (const RuntimeMethod* method, RuntimeObject* obj, T1 p1)
	{
		VirtualInvokeData invokeData;
		il2cpp_codegen_get_generic_virtual_invoke_data(method, obj, &invokeData);
		((Action)invokeData.methodPtr)(obj, p1, invokeData.method);
	}
};
struct InterfaceActionInvoker0
{
	typedef void (*Action)(void*, const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeClass* declaringInterface, RuntimeObject* obj)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_interface_invoke_data(slot, obj, declaringInterface);
		((Action)invokeData.methodPtr)(obj, invokeData.method);
	}
};
template <typename T1>
struct InterfaceActionInvoker1
{
	typedef void (*Action)(void*, T1, const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeClass* declaringInterface, RuntimeObject* obj, T1 p1)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_interface_invoke_data(slot, obj, declaringInterface);
		((Action)invokeData.methodPtr)(obj, p1, invokeData.method);
	}
};
struct GenericInterfaceActionInvoker0
{
	typedef void (*Action)(void*, const RuntimeMethod*);

	static inline void Invoke (const RuntimeMethod* method, RuntimeObject* obj)
	{
		VirtualInvokeData invokeData;
		il2cpp_codegen_get_generic_interface_invoke_data(method, obj, &invokeData);
		((Action)invokeData.methodPtr)(obj, invokeData.method);
	}
};
template <typename T1>
struct GenericInterfaceActionInvoker1
{
	typedef void (*Action)(void*, T1, const RuntimeMethod*);

	static inline void Invoke (const RuntimeMethod* method, RuntimeObject* obj, T1 p1)
	{
		VirtualInvokeData invokeData;
		il2cpp_codegen_get_generic_interface_invoke_data(method, obj, &invokeData);
		((Action)invokeData.methodPtr)(obj, p1, invokeData.method);
	}
};

// System.AsyncCallback
struct AsyncCallback_t3F3DA3BEDAEE81DD1D24125DF8EB30E85EE14DA4;
// System.Attribute
struct Attribute_tF048C13FB3C8CFCC53F82290E4A3F621089F9A74;
// System.Byte[]
struct ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821;
// System.Char[]
struct CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2;
// System.Collections.Generic.Dictionary`2<System.Int16,UnityEngine.Networking.NetworkConnection/PacketStat>
struct Dictionary_2_t12C98B97230995F2189EDF313C5425F959276BD5;
// System.Collections.Generic.Dictionary`2<System.Int16,UnityEngine.Networking.NetworkMessageDelegate>
struct Dictionary_2_tC04929123F1414CEE8F4F8F7A76CC39B1BBDFFC3;
// System.Collections.Generic.HashSet`1<System.Int32>
struct HashSet_1_tD16423F193A61077DD5FE7C8517877716AAFF11E;
// System.Collections.Generic.HashSet`1<UnityEngine.Networking.NetworkIdentity>
struct HashSet_1_t11FE623F2912D3D97F683A80F888A092B31918D6;
// System.Collections.Generic.HashSet`1<UnityEngine.Networking.NetworkInstanceId>
struct HashSet_1_t04AFB5FB47684F4B89636F9B0A465D7E869FFA50;
// System.Collections.Generic.List`1<UnityEngine.Networking.LocalClient/InternalMsg>
struct List_1_t163FEDD599DBF83121B7665744F773B0D0A681AB;
// System.Collections.Generic.List`1<UnityEngine.Networking.NetworkClient>
struct List_1_t51C4494E1C3EF6D60D76BC46D51A3376405941EE;
// System.Collections.Generic.List`1<UnityEngine.Networking.NetworkConnection>
struct List_1_t7A006745EA41BB1EE354777CCAE13C252BF8240D;
// System.Collections.Generic.List`1<UnityEngine.Networking.PlayerController>
struct List_1_t5761C753EAD0AAC9E064F2B3B61ED2992111850F;
// System.Collections.Generic.Stack`1<UnityEngine.Networking.LocalClient/InternalMsg>
struct Stack_1_tA8312A4E405D252C827FE2E3FCF65714642C8C47;
// System.Delegate
struct Delegate_t;
// System.DelegateData
struct DelegateData_t1BF9F691B56DAE5F8C28C5E084FDE94F15F27BBE;
// System.Delegate[]
struct DelegateU5BU5D_tDFCDEE2A6322F96C0FE49AF47E9ADB8C4B294E86;
// System.IAsyncResult
struct IAsyncResult_t8E194308510B375B42432981AE5E7488C458D598;
// System.Net.EndPoint
struct EndPoint_tD87FCEF2780A951E8CE8D808C345FBF2C088D980;
// System.Reflection.MethodInfo
struct MethodInfo_t;
// System.String
struct String_t;
// System.Text.Encoding
struct Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4;
// System.Type
struct Type_t;
// System.UInt32[]
struct UInt32U5BU5D_t9AA834AF2940E75BBF8E3F08FF0D20D266DB71CB;
// System.Void
struct Void_t22962CB4C05B1D89B55A6E1139F0E87A90987017;
// UnityEngine.GameObject
struct GameObject_tBD1244AD56B4E59AAD76E5E7C9282EC5CE434F0F;
// UnityEngine.Networking.ChannelBuffer[]
struct ChannelBufferU5BU5D_t75CDA99AB4F27F49A1DAA287CF43B1132505E6FA;
// UnityEngine.Networking.HostTopology
struct HostTopology_tD01D253330A0DAA736EDFC67EE9585C363FA9B0E;
// UnityEngine.Networking.LocalClient
struct LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86;
// UnityEngine.Networking.MessageBase
struct MessageBase_t2EA42B01AD6A5F36EAF84BE623801951B9F55416;
// UnityEngine.Networking.NetBuffer
struct NetBuffer_t2BA43CF3688776F372BECD54D28F90CB0559B36C;
// UnityEngine.Networking.NetworkConnection
struct NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA;
// UnityEngine.Networking.NetworkMessage
struct NetworkMessage_tCD66E2AE395A185EFE622EBB5497C95F6754685C;
// UnityEngine.Networking.NetworkMessageHandlers
struct NetworkMessageHandlers_tA7BB2E51BDBD8ECE976AD44F1B634F40EA9807D4;
// UnityEngine.Networking.NetworkReader
struct NetworkReader_t7011A2F66F461EA5D4413F3979F1F3244D82FD12;
// UnityEngine.Networking.NetworkScene
struct NetworkScene_t67A8AC9779C203B146A8723FA561736890CA9A40;
// UnityEngine.Networking.NetworkServer
struct NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1;
// UnityEngine.Networking.NetworkServer/ServerSimpleWrapper
struct ServerSimpleWrapper_t1ECF42A66748FA970402440F00E743DB5E2AAA32;
// UnityEngine.Networking.NetworkSystem.CRCMessage
struct CRCMessage_t7F44D52B267C35387F0D7AD0D9098D579ECF61FA;
// UnityEngine.Networking.NetworkSystem.RemovePlayerMessage
struct RemovePlayerMessage_t51B0D9BCA3C2B4FD772A2972588CC0915FD4CEBF;
// UnityEngine.Networking.NetworkWriter
struct NetworkWriter_t9BE861BDE3F59F374D83A1E4CC697C73003FF030;
// UnityEngine.Networking.SyncVarAttribute
struct SyncVarAttribute_tD57FE395DED8D547F0200B7F50F36DFA27C6BF3A;
// UnityEngine.Networking.TargetRpcAttribute
struct TargetRpcAttribute_t7B515CB5DD6D609483DFC4ACC89D00B00C9EAE03;
// UnityEngine.Networking.ULocalConnectionToClient
struct ULocalConnectionToClient_t7AF7EBF2BEC3714F75EF894035BFAE9E6F9561A8;
// UnityEngine.Networking.ULocalConnectionToServer
struct ULocalConnectionToServer_tE6E34057F329C3E0E703C6F095DF82B0270557B8;
// UnityEngine.Networking.UnSpawnDelegate
struct UnSpawnDelegate_tDC1AD5AA3602EB703F4FA34792B4D4075582AE19;

IL2CPP_EXTERN_C RuntimeClass* Debug_t7B5FCB117E2FD63B6838BC52821B252E2BFB61C4_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* LogFilter_t5202A297E770086F7954B8D6703BAC03C22654ED_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C String_t* _stringLiteral0C84AC39FC120C1E579C09FEAA062CA04994E08F;
IL2CPP_EXTERN_C String_t* _stringLiteral13F4B303180B12CAF8F069E93D7C4FAF969D3BC4;
IL2CPP_EXTERN_C String_t* _stringLiteral8A90E4187AA462FD7BCD9E2521B1C4F09372DBAF;
IL2CPP_EXTERN_C const uint32_t ULocalConnectionToClient__ctor_mCF552AD09DD71A741FD56B4429F49D44F0238AE2_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t ULocalConnectionToServer_SendBytes_m0DD7C930ED218BD442317922C19250C58E7D319B_MetadataUsageId;
IL2CPP_EXTERN_C const uint32_t ULocalConnectionToServer__ctor_m98B9D5A9A831E4DAF3C504FA93965B4563C0E981_MetadataUsageId;
struct Delegate_t_marshaled_com;
struct Delegate_t_marshaled_pinvoke;

struct ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821;
struct DelegateU5BU5D_tDFCDEE2A6322F96C0FE49AF47E9ADB8C4B294E86;

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


// System.Attribute
struct  Attribute_tF048C13FB3C8CFCC53F82290E4A3F621089F9A74  : public RuntimeObject
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

// UnityEngine.Networking.MessageBase
struct  MessageBase_t2EA42B01AD6A5F36EAF84BE623801951B9F55416  : public RuntimeObject
{
public:

public:
};


// UnityEngine.Networking.NetworkServer
struct  NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1  : public RuntimeObject
{
public:
	// System.Boolean UnityEngine.Networking.NetworkServer::m_LocalClientActive
	bool ___m_LocalClientActive_4;
	// System.Collections.Generic.List`1<UnityEngine.Networking.NetworkConnection> UnityEngine.Networking.NetworkServer::m_LocalConnectionsFakeList
	List_1_t7A006745EA41BB1EE354777CCAE13C252BF8240D * ___m_LocalConnectionsFakeList_5;
	// UnityEngine.Networking.ULocalConnectionToClient UnityEngine.Networking.NetworkServer::m_LocalConnection
	ULocalConnectionToClient_t7AF7EBF2BEC3714F75EF894035BFAE9E6F9561A8 * ___m_LocalConnection_6;
	// UnityEngine.Networking.NetworkScene UnityEngine.Networking.NetworkServer::m_NetworkScene
	NetworkScene_t67A8AC9779C203B146A8723FA561736890CA9A40 * ___m_NetworkScene_7;
	// System.Collections.Generic.HashSet`1<System.Int32> UnityEngine.Networking.NetworkServer::m_ExternalConnections
	HashSet_1_tD16423F193A61077DD5FE7C8517877716AAFF11E * ___m_ExternalConnections_8;
	// UnityEngine.Networking.NetworkServer_ServerSimpleWrapper UnityEngine.Networking.NetworkServer::m_SimpleServerSimple
	ServerSimpleWrapper_t1ECF42A66748FA970402440F00E743DB5E2AAA32 * ___m_SimpleServerSimple_9;
	// System.Single UnityEngine.Networking.NetworkServer::m_MaxDelay
	float ___m_MaxDelay_10;
	// System.Collections.Generic.HashSet`1<UnityEngine.Networking.NetworkInstanceId> UnityEngine.Networking.NetworkServer::m_RemoveList
	HashSet_1_t04AFB5FB47684F4B89636F9B0A465D7E869FFA50 * ___m_RemoveList_11;
	// System.Int32 UnityEngine.Networking.NetworkServer::m_RemoveListCount
	int32_t ___m_RemoveListCount_12;

public:
	inline static int32_t get_offset_of_m_LocalClientActive_4() { return static_cast<int32_t>(offsetof(NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1, ___m_LocalClientActive_4)); }
	inline bool get_m_LocalClientActive_4() const { return ___m_LocalClientActive_4; }
	inline bool* get_address_of_m_LocalClientActive_4() { return &___m_LocalClientActive_4; }
	inline void set_m_LocalClientActive_4(bool value)
	{
		___m_LocalClientActive_4 = value;
	}

	inline static int32_t get_offset_of_m_LocalConnectionsFakeList_5() { return static_cast<int32_t>(offsetof(NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1, ___m_LocalConnectionsFakeList_5)); }
	inline List_1_t7A006745EA41BB1EE354777CCAE13C252BF8240D * get_m_LocalConnectionsFakeList_5() const { return ___m_LocalConnectionsFakeList_5; }
	inline List_1_t7A006745EA41BB1EE354777CCAE13C252BF8240D ** get_address_of_m_LocalConnectionsFakeList_5() { return &___m_LocalConnectionsFakeList_5; }
	inline void set_m_LocalConnectionsFakeList_5(List_1_t7A006745EA41BB1EE354777CCAE13C252BF8240D * value)
	{
		___m_LocalConnectionsFakeList_5 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_LocalConnectionsFakeList_5), (void*)value);
	}

	inline static int32_t get_offset_of_m_LocalConnection_6() { return static_cast<int32_t>(offsetof(NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1, ___m_LocalConnection_6)); }
	inline ULocalConnectionToClient_t7AF7EBF2BEC3714F75EF894035BFAE9E6F9561A8 * get_m_LocalConnection_6() const { return ___m_LocalConnection_6; }
	inline ULocalConnectionToClient_t7AF7EBF2BEC3714F75EF894035BFAE9E6F9561A8 ** get_address_of_m_LocalConnection_6() { return &___m_LocalConnection_6; }
	inline void set_m_LocalConnection_6(ULocalConnectionToClient_t7AF7EBF2BEC3714F75EF894035BFAE9E6F9561A8 * value)
	{
		___m_LocalConnection_6 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_LocalConnection_6), (void*)value);
	}

	inline static int32_t get_offset_of_m_NetworkScene_7() { return static_cast<int32_t>(offsetof(NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1, ___m_NetworkScene_7)); }
	inline NetworkScene_t67A8AC9779C203B146A8723FA561736890CA9A40 * get_m_NetworkScene_7() const { return ___m_NetworkScene_7; }
	inline NetworkScene_t67A8AC9779C203B146A8723FA561736890CA9A40 ** get_address_of_m_NetworkScene_7() { return &___m_NetworkScene_7; }
	inline void set_m_NetworkScene_7(NetworkScene_t67A8AC9779C203B146A8723FA561736890CA9A40 * value)
	{
		___m_NetworkScene_7 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_NetworkScene_7), (void*)value);
	}

	inline static int32_t get_offset_of_m_ExternalConnections_8() { return static_cast<int32_t>(offsetof(NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1, ___m_ExternalConnections_8)); }
	inline HashSet_1_tD16423F193A61077DD5FE7C8517877716AAFF11E * get_m_ExternalConnections_8() const { return ___m_ExternalConnections_8; }
	inline HashSet_1_tD16423F193A61077DD5FE7C8517877716AAFF11E ** get_address_of_m_ExternalConnections_8() { return &___m_ExternalConnections_8; }
	inline void set_m_ExternalConnections_8(HashSet_1_tD16423F193A61077DD5FE7C8517877716AAFF11E * value)
	{
		___m_ExternalConnections_8 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_ExternalConnections_8), (void*)value);
	}

	inline static int32_t get_offset_of_m_SimpleServerSimple_9() { return static_cast<int32_t>(offsetof(NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1, ___m_SimpleServerSimple_9)); }
	inline ServerSimpleWrapper_t1ECF42A66748FA970402440F00E743DB5E2AAA32 * get_m_SimpleServerSimple_9() const { return ___m_SimpleServerSimple_9; }
	inline ServerSimpleWrapper_t1ECF42A66748FA970402440F00E743DB5E2AAA32 ** get_address_of_m_SimpleServerSimple_9() { return &___m_SimpleServerSimple_9; }
	inline void set_m_SimpleServerSimple_9(ServerSimpleWrapper_t1ECF42A66748FA970402440F00E743DB5E2AAA32 * value)
	{
		___m_SimpleServerSimple_9 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_SimpleServerSimple_9), (void*)value);
	}

	inline static int32_t get_offset_of_m_MaxDelay_10() { return static_cast<int32_t>(offsetof(NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1, ___m_MaxDelay_10)); }
	inline float get_m_MaxDelay_10() const { return ___m_MaxDelay_10; }
	inline float* get_address_of_m_MaxDelay_10() { return &___m_MaxDelay_10; }
	inline void set_m_MaxDelay_10(float value)
	{
		___m_MaxDelay_10 = value;
	}

	inline static int32_t get_offset_of_m_RemoveList_11() { return static_cast<int32_t>(offsetof(NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1, ___m_RemoveList_11)); }
	inline HashSet_1_t04AFB5FB47684F4B89636F9B0A465D7E869FFA50 * get_m_RemoveList_11() const { return ___m_RemoveList_11; }
	inline HashSet_1_t04AFB5FB47684F4B89636F9B0A465D7E869FFA50 ** get_address_of_m_RemoveList_11() { return &___m_RemoveList_11; }
	inline void set_m_RemoveList_11(HashSet_1_t04AFB5FB47684F4B89636F9B0A465D7E869FFA50 * value)
	{
		___m_RemoveList_11 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_RemoveList_11), (void*)value);
	}

	inline static int32_t get_offset_of_m_RemoveListCount_12() { return static_cast<int32_t>(offsetof(NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1, ___m_RemoveListCount_12)); }
	inline int32_t get_m_RemoveListCount_12() const { return ___m_RemoveListCount_12; }
	inline int32_t* get_address_of_m_RemoveListCount_12() { return &___m_RemoveListCount_12; }
	inline void set_m_RemoveListCount_12(int32_t value)
	{
		___m_RemoveListCount_12 = value;
	}
};

struct NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1_StaticFields
{
public:
	// System.Boolean UnityEngine.Networking.NetworkServer::s_Active
	bool ___s_Active_0;
	// UnityEngine.Networking.NetworkServer modreq(System.Runtime.CompilerServices.IsVolatile) UnityEngine.Networking.NetworkServer::s_Instance
	NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 * ___s_Instance_1;
	// System.Object UnityEngine.Networking.NetworkServer::s_Sync
	RuntimeObject * ___s_Sync_2;
	// System.Boolean UnityEngine.Networking.NetworkServer::m_DontListen
	bool ___m_DontListen_3;
	// System.UInt16 UnityEngine.Networking.NetworkServer::maxPacketSize
	uint16_t ___maxPacketSize_14;
	// UnityEngine.Networking.NetworkSystem.RemovePlayerMessage UnityEngine.Networking.NetworkServer::s_RemovePlayerMessage
	RemovePlayerMessage_t51B0D9BCA3C2B4FD772A2972588CC0915FD4CEBF * ___s_RemovePlayerMessage_15;

public:
	inline static int32_t get_offset_of_s_Active_0() { return static_cast<int32_t>(offsetof(NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1_StaticFields, ___s_Active_0)); }
	inline bool get_s_Active_0() const { return ___s_Active_0; }
	inline bool* get_address_of_s_Active_0() { return &___s_Active_0; }
	inline void set_s_Active_0(bool value)
	{
		___s_Active_0 = value;
	}

	inline static int32_t get_offset_of_s_Instance_1() { return static_cast<int32_t>(offsetof(NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1_StaticFields, ___s_Instance_1)); }
	inline NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 * get_s_Instance_1() const { return ___s_Instance_1; }
	inline NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 ** get_address_of_s_Instance_1() { return &___s_Instance_1; }
	inline void set_s_Instance_1(NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 * value)
	{
		___s_Instance_1 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___s_Instance_1), (void*)value);
	}

	inline static int32_t get_offset_of_s_Sync_2() { return static_cast<int32_t>(offsetof(NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1_StaticFields, ___s_Sync_2)); }
	inline RuntimeObject * get_s_Sync_2() const { return ___s_Sync_2; }
	inline RuntimeObject ** get_address_of_s_Sync_2() { return &___s_Sync_2; }
	inline void set_s_Sync_2(RuntimeObject * value)
	{
		___s_Sync_2 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___s_Sync_2), (void*)value);
	}

	inline static int32_t get_offset_of_m_DontListen_3() { return static_cast<int32_t>(offsetof(NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1_StaticFields, ___m_DontListen_3)); }
	inline bool get_m_DontListen_3() const { return ___m_DontListen_3; }
	inline bool* get_address_of_m_DontListen_3() { return &___m_DontListen_3; }
	inline void set_m_DontListen_3(bool value)
	{
		___m_DontListen_3 = value;
	}

	inline static int32_t get_offset_of_maxPacketSize_14() { return static_cast<int32_t>(offsetof(NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1_StaticFields, ___maxPacketSize_14)); }
	inline uint16_t get_maxPacketSize_14() const { return ___maxPacketSize_14; }
	inline uint16_t* get_address_of_maxPacketSize_14() { return &___maxPacketSize_14; }
	inline void set_maxPacketSize_14(uint16_t value)
	{
		___maxPacketSize_14 = value;
	}

	inline static int32_t get_offset_of_s_RemovePlayerMessage_15() { return static_cast<int32_t>(offsetof(NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1_StaticFields, ___s_RemovePlayerMessage_15)); }
	inline RemovePlayerMessage_t51B0D9BCA3C2B4FD772A2972588CC0915FD4CEBF * get_s_RemovePlayerMessage_15() const { return ___s_RemovePlayerMessage_15; }
	inline RemovePlayerMessage_t51B0D9BCA3C2B4FD772A2972588CC0915FD4CEBF ** get_address_of_s_RemovePlayerMessage_15() { return &___s_RemovePlayerMessage_15; }
	inline void set_s_RemovePlayerMessage_15(RemovePlayerMessage_t51B0D9BCA3C2B4FD772A2972588CC0915FD4CEBF * value)
	{
		___s_RemovePlayerMessage_15 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___s_RemovePlayerMessage_15), (void*)value);
	}
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


// System.Byte
struct  Byte_tF87C579059BD4633E6840EBBBEEF899C6E33EF07 
{
public:
	// System.Byte System.Byte::m_value
	uint8_t ___m_value_0;

public:
	inline static int32_t get_offset_of_m_value_0() { return static_cast<int32_t>(offsetof(Byte_tF87C579059BD4633E6840EBBBEEF899C6E33EF07, ___m_value_0)); }
	inline uint8_t get_m_value_0() const { return ___m_value_0; }
	inline uint8_t* get_address_of_m_value_0() { return &___m_value_0; }
	inline void set_m_value_0(uint8_t value)
	{
		___m_value_0 = value;
	}
};


// System.Decimal
struct  Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8 
{
public:
	// System.Int32 System.Decimal::flags
	int32_t ___flags_14;
	// System.Int32 System.Decimal::hi
	int32_t ___hi_15;
	// System.Int32 System.Decimal::lo
	int32_t ___lo_16;
	// System.Int32 System.Decimal::mid
	int32_t ___mid_17;

public:
	inline static int32_t get_offset_of_flags_14() { return static_cast<int32_t>(offsetof(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8, ___flags_14)); }
	inline int32_t get_flags_14() const { return ___flags_14; }
	inline int32_t* get_address_of_flags_14() { return &___flags_14; }
	inline void set_flags_14(int32_t value)
	{
		___flags_14 = value;
	}

	inline static int32_t get_offset_of_hi_15() { return static_cast<int32_t>(offsetof(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8, ___hi_15)); }
	inline int32_t get_hi_15() const { return ___hi_15; }
	inline int32_t* get_address_of_hi_15() { return &___hi_15; }
	inline void set_hi_15(int32_t value)
	{
		___hi_15 = value;
	}

	inline static int32_t get_offset_of_lo_16() { return static_cast<int32_t>(offsetof(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8, ___lo_16)); }
	inline int32_t get_lo_16() const { return ___lo_16; }
	inline int32_t* get_address_of_lo_16() { return &___lo_16; }
	inline void set_lo_16(int32_t value)
	{
		___lo_16 = value;
	}

	inline static int32_t get_offset_of_mid_17() { return static_cast<int32_t>(offsetof(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8, ___mid_17)); }
	inline int32_t get_mid_17() const { return ___mid_17; }
	inline int32_t* get_address_of_mid_17() { return &___mid_17; }
	inline void set_mid_17(int32_t value)
	{
		___mid_17 = value;
	}
};

struct Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8_StaticFields
{
public:
	// System.UInt32[] System.Decimal::Powers10
	UInt32U5BU5D_t9AA834AF2940E75BBF8E3F08FF0D20D266DB71CB* ___Powers10_6;
	// System.Decimal System.Decimal::Zero
	Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  ___Zero_7;
	// System.Decimal System.Decimal::One
	Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  ___One_8;
	// System.Decimal System.Decimal::MinusOne
	Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  ___MinusOne_9;
	// System.Decimal System.Decimal::MaxValue
	Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  ___MaxValue_10;
	// System.Decimal System.Decimal::MinValue
	Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  ___MinValue_11;
	// System.Decimal System.Decimal::NearNegativeZero
	Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  ___NearNegativeZero_12;
	// System.Decimal System.Decimal::NearPositiveZero
	Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  ___NearPositiveZero_13;

public:
	inline static int32_t get_offset_of_Powers10_6() { return static_cast<int32_t>(offsetof(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8_StaticFields, ___Powers10_6)); }
	inline UInt32U5BU5D_t9AA834AF2940E75BBF8E3F08FF0D20D266DB71CB* get_Powers10_6() const { return ___Powers10_6; }
	inline UInt32U5BU5D_t9AA834AF2940E75BBF8E3F08FF0D20D266DB71CB** get_address_of_Powers10_6() { return &___Powers10_6; }
	inline void set_Powers10_6(UInt32U5BU5D_t9AA834AF2940E75BBF8E3F08FF0D20D266DB71CB* value)
	{
		___Powers10_6 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___Powers10_6), (void*)value);
	}

	inline static int32_t get_offset_of_Zero_7() { return static_cast<int32_t>(offsetof(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8_StaticFields, ___Zero_7)); }
	inline Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  get_Zero_7() const { return ___Zero_7; }
	inline Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8 * get_address_of_Zero_7() { return &___Zero_7; }
	inline void set_Zero_7(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  value)
	{
		___Zero_7 = value;
	}

	inline static int32_t get_offset_of_One_8() { return static_cast<int32_t>(offsetof(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8_StaticFields, ___One_8)); }
	inline Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  get_One_8() const { return ___One_8; }
	inline Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8 * get_address_of_One_8() { return &___One_8; }
	inline void set_One_8(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  value)
	{
		___One_8 = value;
	}

	inline static int32_t get_offset_of_MinusOne_9() { return static_cast<int32_t>(offsetof(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8_StaticFields, ___MinusOne_9)); }
	inline Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  get_MinusOne_9() const { return ___MinusOne_9; }
	inline Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8 * get_address_of_MinusOne_9() { return &___MinusOne_9; }
	inline void set_MinusOne_9(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  value)
	{
		___MinusOne_9 = value;
	}

	inline static int32_t get_offset_of_MaxValue_10() { return static_cast<int32_t>(offsetof(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8_StaticFields, ___MaxValue_10)); }
	inline Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  get_MaxValue_10() const { return ___MaxValue_10; }
	inline Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8 * get_address_of_MaxValue_10() { return &___MaxValue_10; }
	inline void set_MaxValue_10(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  value)
	{
		___MaxValue_10 = value;
	}

	inline static int32_t get_offset_of_MinValue_11() { return static_cast<int32_t>(offsetof(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8_StaticFields, ___MinValue_11)); }
	inline Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  get_MinValue_11() const { return ___MinValue_11; }
	inline Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8 * get_address_of_MinValue_11() { return &___MinValue_11; }
	inline void set_MinValue_11(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  value)
	{
		___MinValue_11 = value;
	}

	inline static int32_t get_offset_of_NearNegativeZero_12() { return static_cast<int32_t>(offsetof(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8_StaticFields, ___NearNegativeZero_12)); }
	inline Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  get_NearNegativeZero_12() const { return ___NearNegativeZero_12; }
	inline Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8 * get_address_of_NearNegativeZero_12() { return &___NearNegativeZero_12; }
	inline void set_NearNegativeZero_12(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  value)
	{
		___NearNegativeZero_12 = value;
	}

	inline static int32_t get_offset_of_NearPositiveZero_13() { return static_cast<int32_t>(offsetof(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8_StaticFields, ___NearPositiveZero_13)); }
	inline Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  get_NearPositiveZero_13() const { return ___NearPositiveZero_13; }
	inline Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8 * get_address_of_NearPositiveZero_13() { return &___NearPositiveZero_13; }
	inline void set_NearPositiveZero_13(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  value)
	{
		___NearPositiveZero_13 = value;
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

// System.Int16
struct  Int16_t823A20635DAF5A3D93A1E01CFBF3CBA27CF00B4D 
{
public:
	// System.Int16 System.Int16::m_value
	int16_t ___m_value_0;

public:
	inline static int32_t get_offset_of_m_value_0() { return static_cast<int32_t>(offsetof(Int16_t823A20635DAF5A3D93A1E01CFBF3CBA27CF00B4D, ___m_value_0)); }
	inline int16_t get_m_value_0() const { return ___m_value_0; }
	inline int16_t* get_address_of_m_value_0() { return &___m_value_0; }
	inline void set_m_value_0(int16_t value)
	{
		___m_value_0 = value;
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


// UnityEngine.Networking.SyncVarAttribute
struct  SyncVarAttribute_tD57FE395DED8D547F0200B7F50F36DFA27C6BF3A  : public Attribute_tF048C13FB3C8CFCC53F82290E4A3F621089F9A74
{
public:
	// System.String UnityEngine.Networking.SyncVarAttribute::hook
	String_t* ___hook_0;

public:
	inline static int32_t get_offset_of_hook_0() { return static_cast<int32_t>(offsetof(SyncVarAttribute_tD57FE395DED8D547F0200B7F50F36DFA27C6BF3A, ___hook_0)); }
	inline String_t* get_hook_0() const { return ___hook_0; }
	inline String_t** get_address_of_hook_0() { return &___hook_0; }
	inline void set_hook_0(String_t* value)
	{
		___hook_0 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___hook_0), (void*)value);
	}
};


// UnityEngine.Networking.TargetRpcAttribute
struct  TargetRpcAttribute_t7B515CB5DD6D609483DFC4ACC89D00B00C9EAE03  : public Attribute_tF048C13FB3C8CFCC53F82290E4A3F621089F9A74
{
public:
	// System.Int32 UnityEngine.Networking.TargetRpcAttribute::channel
	int32_t ___channel_0;

public:
	inline static int32_t get_offset_of_channel_0() { return static_cast<int32_t>(offsetof(TargetRpcAttribute_t7B515CB5DD6D609483DFC4ACC89D00B00C9EAE03, ___channel_0)); }
	inline int32_t get_channel_0() const { return ___channel_0; }
	inline int32_t* get_address_of_channel_0() { return &___channel_0; }
	inline void set_channel_0(int32_t value)
	{
		___channel_0 = value;
	}
};


// UnityEngine.Networking.UIntFloat
struct  UIntFloat_tFF4D5273EEDE59506E38E1C3A3932139C4EACBE0 
{
public:
	union
	{
		#pragma pack(push, tp, 1)
		struct
		{
			// System.Single UnityEngine.Networking.UIntFloat::floatValue
			float ___floatValue_0;
		};
		#pragma pack(pop, tp)
		struct
		{
			float ___floatValue_0_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			// System.UInt32 UnityEngine.Networking.UIntFloat::intValue
			uint32_t ___intValue_1;
		};
		#pragma pack(pop, tp)
		struct
		{
			uint32_t ___intValue_1_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			// System.Double UnityEngine.Networking.UIntFloat::doubleValue
			double ___doubleValue_2;
		};
		#pragma pack(pop, tp)
		struct
		{
			double ___doubleValue_2_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			// System.UInt64 UnityEngine.Networking.UIntFloat::longValue
			uint64_t ___longValue_3;
		};
		#pragma pack(pop, tp)
		struct
		{
			uint64_t ___longValue_3_forAlignmentOnly;
		};
	};

public:
	inline static int32_t get_offset_of_floatValue_0() { return static_cast<int32_t>(offsetof(UIntFloat_tFF4D5273EEDE59506E38E1C3A3932139C4EACBE0, ___floatValue_0)); }
	inline float get_floatValue_0() const { return ___floatValue_0; }
	inline float* get_address_of_floatValue_0() { return &___floatValue_0; }
	inline void set_floatValue_0(float value)
	{
		___floatValue_0 = value;
	}

	inline static int32_t get_offset_of_intValue_1() { return static_cast<int32_t>(offsetof(UIntFloat_tFF4D5273EEDE59506E38E1C3A3932139C4EACBE0, ___intValue_1)); }
	inline uint32_t get_intValue_1() const { return ___intValue_1; }
	inline uint32_t* get_address_of_intValue_1() { return &___intValue_1; }
	inline void set_intValue_1(uint32_t value)
	{
		___intValue_1 = value;
	}

	inline static int32_t get_offset_of_doubleValue_2() { return static_cast<int32_t>(offsetof(UIntFloat_tFF4D5273EEDE59506E38E1C3A3932139C4EACBE0, ___doubleValue_2)); }
	inline double get_doubleValue_2() const { return ___doubleValue_2; }
	inline double* get_address_of_doubleValue_2() { return &___doubleValue_2; }
	inline void set_doubleValue_2(double value)
	{
		___doubleValue_2 = value;
	}

	inline static int32_t get_offset_of_longValue_3() { return static_cast<int32_t>(offsetof(UIntFloat_tFF4D5273EEDE59506E38E1C3A3932139C4EACBE0, ___longValue_3)); }
	inline uint64_t get_longValue_3() const { return ___longValue_3; }
	inline uint64_t* get_address_of_longValue_3() { return &___longValue_3; }
	inline void set_longValue_3(uint64_t value)
	{
		___longValue_3 = value;
	}
};


// System.Delegate
struct  Delegate_t  : public RuntimeObject
{
public:
	// System.IntPtr System.Delegate::method_ptr
	Il2CppMethodPointer ___method_ptr_0;
	// System.IntPtr System.Delegate::invoke_impl
	intptr_t ___invoke_impl_1;
	// System.Object System.Delegate::m_target
	RuntimeObject * ___m_target_2;
	// System.IntPtr System.Delegate::method
	intptr_t ___method_3;
	// System.IntPtr System.Delegate::delegate_trampoline
	intptr_t ___delegate_trampoline_4;
	// System.IntPtr System.Delegate::extra_arg
	intptr_t ___extra_arg_5;
	// System.IntPtr System.Delegate::method_code
	intptr_t ___method_code_6;
	// System.Reflection.MethodInfo System.Delegate::method_info
	MethodInfo_t * ___method_info_7;
	// System.Reflection.MethodInfo System.Delegate::original_method_info
	MethodInfo_t * ___original_method_info_8;
	// System.DelegateData System.Delegate::data
	DelegateData_t1BF9F691B56DAE5F8C28C5E084FDE94F15F27BBE * ___data_9;
	// System.Boolean System.Delegate::method_is_virtual
	bool ___method_is_virtual_10;

public:
	inline static int32_t get_offset_of_method_ptr_0() { return static_cast<int32_t>(offsetof(Delegate_t, ___method_ptr_0)); }
	inline Il2CppMethodPointer get_method_ptr_0() const { return ___method_ptr_0; }
	inline Il2CppMethodPointer* get_address_of_method_ptr_0() { return &___method_ptr_0; }
	inline void set_method_ptr_0(Il2CppMethodPointer value)
	{
		___method_ptr_0 = value;
	}

	inline static int32_t get_offset_of_invoke_impl_1() { return static_cast<int32_t>(offsetof(Delegate_t, ___invoke_impl_1)); }
	inline intptr_t get_invoke_impl_1() const { return ___invoke_impl_1; }
	inline intptr_t* get_address_of_invoke_impl_1() { return &___invoke_impl_1; }
	inline void set_invoke_impl_1(intptr_t value)
	{
		___invoke_impl_1 = value;
	}

	inline static int32_t get_offset_of_m_target_2() { return static_cast<int32_t>(offsetof(Delegate_t, ___m_target_2)); }
	inline RuntimeObject * get_m_target_2() const { return ___m_target_2; }
	inline RuntimeObject ** get_address_of_m_target_2() { return &___m_target_2; }
	inline void set_m_target_2(RuntimeObject * value)
	{
		___m_target_2 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_target_2), (void*)value);
	}

	inline static int32_t get_offset_of_method_3() { return static_cast<int32_t>(offsetof(Delegate_t, ___method_3)); }
	inline intptr_t get_method_3() const { return ___method_3; }
	inline intptr_t* get_address_of_method_3() { return &___method_3; }
	inline void set_method_3(intptr_t value)
	{
		___method_3 = value;
	}

	inline static int32_t get_offset_of_delegate_trampoline_4() { return static_cast<int32_t>(offsetof(Delegate_t, ___delegate_trampoline_4)); }
	inline intptr_t get_delegate_trampoline_4() const { return ___delegate_trampoline_4; }
	inline intptr_t* get_address_of_delegate_trampoline_4() { return &___delegate_trampoline_4; }
	inline void set_delegate_trampoline_4(intptr_t value)
	{
		___delegate_trampoline_4 = value;
	}

	inline static int32_t get_offset_of_extra_arg_5() { return static_cast<int32_t>(offsetof(Delegate_t, ___extra_arg_5)); }
	inline intptr_t get_extra_arg_5() const { return ___extra_arg_5; }
	inline intptr_t* get_address_of_extra_arg_5() { return &___extra_arg_5; }
	inline void set_extra_arg_5(intptr_t value)
	{
		___extra_arg_5 = value;
	}

	inline static int32_t get_offset_of_method_code_6() { return static_cast<int32_t>(offsetof(Delegate_t, ___method_code_6)); }
	inline intptr_t get_method_code_6() const { return ___method_code_6; }
	inline intptr_t* get_address_of_method_code_6() { return &___method_code_6; }
	inline void set_method_code_6(intptr_t value)
	{
		___method_code_6 = value;
	}

	inline static int32_t get_offset_of_method_info_7() { return static_cast<int32_t>(offsetof(Delegate_t, ___method_info_7)); }
	inline MethodInfo_t * get_method_info_7() const { return ___method_info_7; }
	inline MethodInfo_t ** get_address_of_method_info_7() { return &___method_info_7; }
	inline void set_method_info_7(MethodInfo_t * value)
	{
		___method_info_7 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___method_info_7), (void*)value);
	}

	inline static int32_t get_offset_of_original_method_info_8() { return static_cast<int32_t>(offsetof(Delegate_t, ___original_method_info_8)); }
	inline MethodInfo_t * get_original_method_info_8() const { return ___original_method_info_8; }
	inline MethodInfo_t ** get_address_of_original_method_info_8() { return &___original_method_info_8; }
	inline void set_original_method_info_8(MethodInfo_t * value)
	{
		___original_method_info_8 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___original_method_info_8), (void*)value);
	}

	inline static int32_t get_offset_of_data_9() { return static_cast<int32_t>(offsetof(Delegate_t, ___data_9)); }
	inline DelegateData_t1BF9F691B56DAE5F8C28C5E084FDE94F15F27BBE * get_data_9() const { return ___data_9; }
	inline DelegateData_t1BF9F691B56DAE5F8C28C5E084FDE94F15F27BBE ** get_address_of_data_9() { return &___data_9; }
	inline void set_data_9(DelegateData_t1BF9F691B56DAE5F8C28C5E084FDE94F15F27BBE * value)
	{
		___data_9 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___data_9), (void*)value);
	}

	inline static int32_t get_offset_of_method_is_virtual_10() { return static_cast<int32_t>(offsetof(Delegate_t, ___method_is_virtual_10)); }
	inline bool get_method_is_virtual_10() const { return ___method_is_virtual_10; }
	inline bool* get_address_of_method_is_virtual_10() { return &___method_is_virtual_10; }
	inline void set_method_is_virtual_10(bool value)
	{
		___method_is_virtual_10 = value;
	}
};

// Native definition for P/Invoke marshalling of System.Delegate
struct Delegate_t_marshaled_pinvoke
{
	intptr_t ___method_ptr_0;
	intptr_t ___invoke_impl_1;
	Il2CppIUnknown* ___m_target_2;
	intptr_t ___method_3;
	intptr_t ___delegate_trampoline_4;
	intptr_t ___extra_arg_5;
	intptr_t ___method_code_6;
	MethodInfo_t * ___method_info_7;
	MethodInfo_t * ___original_method_info_8;
	DelegateData_t1BF9F691B56DAE5F8C28C5E084FDE94F15F27BBE * ___data_9;
	int32_t ___method_is_virtual_10;
};
// Native definition for COM marshalling of System.Delegate
struct Delegate_t_marshaled_com
{
	intptr_t ___method_ptr_0;
	intptr_t ___invoke_impl_1;
	Il2CppIUnknown* ___m_target_2;
	intptr_t ___method_3;
	intptr_t ___delegate_trampoline_4;
	intptr_t ___extra_arg_5;
	intptr_t ___method_code_6;
	MethodInfo_t * ___method_info_7;
	MethodInfo_t * ___original_method_info_8;
	DelegateData_t1BF9F691B56DAE5F8C28C5E084FDE94F15F27BBE * ___data_9;
	int32_t ___method_is_virtual_10;
};

// UnityEngine.Networking.NetworkClient_ConnectState
struct  ConnectState_t36DA0E4226FF2170489050E79863831D4DBCB31A 
{
public:
	// System.Int32 UnityEngine.Networking.NetworkClient_ConnectState::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(ConnectState_t36DA0E4226FF2170489050E79863831D4DBCB31A, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};


// UnityEngine.Networking.NetworkError
struct  NetworkError_t2F4C5EEB3EF2313DB6E035334EC2D73885BDEDEC 
{
public:
	// System.Int32 UnityEngine.Networking.NetworkError::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(NetworkError_t2F4C5EEB3EF2313DB6E035334EC2D73885BDEDEC, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};


// UnityEngine.Networking.NetworkWriter
struct  NetworkWriter_t9BE861BDE3F59F374D83A1E4CC697C73003FF030  : public RuntimeObject
{
public:
	// UnityEngine.Networking.NetBuffer UnityEngine.Networking.NetworkWriter::m_Buffer
	NetBuffer_t2BA43CF3688776F372BECD54D28F90CB0559B36C * ___m_Buffer_1;

public:
	inline static int32_t get_offset_of_m_Buffer_1() { return static_cast<int32_t>(offsetof(NetworkWriter_t9BE861BDE3F59F374D83A1E4CC697C73003FF030, ___m_Buffer_1)); }
	inline NetBuffer_t2BA43CF3688776F372BECD54D28F90CB0559B36C * get_m_Buffer_1() const { return ___m_Buffer_1; }
	inline NetBuffer_t2BA43CF3688776F372BECD54D28F90CB0559B36C ** get_address_of_m_Buffer_1() { return &___m_Buffer_1; }
	inline void set_m_Buffer_1(NetBuffer_t2BA43CF3688776F372BECD54D28F90CB0559B36C * value)
	{
		___m_Buffer_1 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_Buffer_1), (void*)value);
	}
};

struct NetworkWriter_t9BE861BDE3F59F374D83A1E4CC697C73003FF030_StaticFields
{
public:
	// System.Text.Encoding UnityEngine.Networking.NetworkWriter::s_Encoding
	Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * ___s_Encoding_2;
	// System.Byte[] UnityEngine.Networking.NetworkWriter::s_StringWriteBuffer
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___s_StringWriteBuffer_3;
	// UnityEngine.Networking.UIntFloat UnityEngine.Networking.NetworkWriter::s_FloatConverter
	UIntFloat_tFF4D5273EEDE59506E38E1C3A3932139C4EACBE0  ___s_FloatConverter_4;

public:
	inline static int32_t get_offset_of_s_Encoding_2() { return static_cast<int32_t>(offsetof(NetworkWriter_t9BE861BDE3F59F374D83A1E4CC697C73003FF030_StaticFields, ___s_Encoding_2)); }
	inline Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * get_s_Encoding_2() const { return ___s_Encoding_2; }
	inline Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 ** get_address_of_s_Encoding_2() { return &___s_Encoding_2; }
	inline void set_s_Encoding_2(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * value)
	{
		___s_Encoding_2 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___s_Encoding_2), (void*)value);
	}

	inline static int32_t get_offset_of_s_StringWriteBuffer_3() { return static_cast<int32_t>(offsetof(NetworkWriter_t9BE861BDE3F59F374D83A1E4CC697C73003FF030_StaticFields, ___s_StringWriteBuffer_3)); }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* get_s_StringWriteBuffer_3() const { return ___s_StringWriteBuffer_3; }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821** get_address_of_s_StringWriteBuffer_3() { return &___s_StringWriteBuffer_3; }
	inline void set_s_StringWriteBuffer_3(ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* value)
	{
		___s_StringWriteBuffer_3 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___s_StringWriteBuffer_3), (void*)value);
	}

	inline static int32_t get_offset_of_s_FloatConverter_4() { return static_cast<int32_t>(offsetof(NetworkWriter_t9BE861BDE3F59F374D83A1E4CC697C73003FF030_StaticFields, ___s_FloatConverter_4)); }
	inline UIntFloat_tFF4D5273EEDE59506E38E1C3A3932139C4EACBE0  get_s_FloatConverter_4() const { return ___s_FloatConverter_4; }
	inline UIntFloat_tFF4D5273EEDE59506E38E1C3A3932139C4EACBE0 * get_address_of_s_FloatConverter_4() { return &___s_FloatConverter_4; }
	inline void set_s_FloatConverter_4(UIntFloat_tFF4D5273EEDE59506E38E1C3A3932139C4EACBE0  value)
	{
		___s_FloatConverter_4 = value;
	}
};


// UnityEngine.Networking.UIntDecimal
struct  UIntDecimal_t7F0C0B7CE4190086DE8B448A03A75C133BA118BA 
{
public:
	union
	{
		#pragma pack(push, tp, 1)
		struct
		{
			// System.UInt64 UnityEngine.Networking.UIntDecimal::longValue1
			uint64_t ___longValue1_0;
		};
		#pragma pack(pop, tp)
		struct
		{
			uint64_t ___longValue1_0_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___longValue2_1_OffsetPadding[8];
			// System.UInt64 UnityEngine.Networking.UIntDecimal::longValue2
			uint64_t ___longValue2_1;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___longValue2_1_OffsetPadding_forAlignmentOnly[8];
			uint64_t ___longValue2_1_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			// System.Decimal UnityEngine.Networking.UIntDecimal::decimalValue
			Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  ___decimalValue_2;
		};
		#pragma pack(pop, tp)
		struct
		{
			Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  ___decimalValue_2_forAlignmentOnly;
		};
	};

public:
	inline static int32_t get_offset_of_longValue1_0() { return static_cast<int32_t>(offsetof(UIntDecimal_t7F0C0B7CE4190086DE8B448A03A75C133BA118BA, ___longValue1_0)); }
	inline uint64_t get_longValue1_0() const { return ___longValue1_0; }
	inline uint64_t* get_address_of_longValue1_0() { return &___longValue1_0; }
	inline void set_longValue1_0(uint64_t value)
	{
		___longValue1_0 = value;
	}

	inline static int32_t get_offset_of_longValue2_1() { return static_cast<int32_t>(offsetof(UIntDecimal_t7F0C0B7CE4190086DE8B448A03A75C133BA118BA, ___longValue2_1)); }
	inline uint64_t get_longValue2_1() const { return ___longValue2_1; }
	inline uint64_t* get_address_of_longValue2_1() { return &___longValue2_1; }
	inline void set_longValue2_1(uint64_t value)
	{
		___longValue2_1 = value;
	}

	inline static int32_t get_offset_of_decimalValue_2() { return static_cast<int32_t>(offsetof(UIntDecimal_t7F0C0B7CE4190086DE8B448A03A75C133BA118BA, ___decimalValue_2)); }
	inline Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  get_decimalValue_2() const { return ___decimalValue_2; }
	inline Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8 * get_address_of_decimalValue_2() { return &___decimalValue_2; }
	inline void set_decimalValue_2(Decimal_t44EE9DA309A1BF848308DE4DDFC070CAE6D95EE8  value)
	{
		___decimalValue_2 = value;
	}
};


// UnityEngine.Networking.Version
struct  Version_tD63B9838033C89FE5C3252E9CB83CA44B92D63CD 
{
public:
	// System.Int32 UnityEngine.Networking.Version::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(Version_tD63B9838033C89FE5C3252E9CB83CA44B92D63CD, ___value___2)); }
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

// System.MulticastDelegate
struct  MulticastDelegate_t  : public Delegate_t
{
public:
	// System.Delegate[] System.MulticastDelegate::delegates
	DelegateU5BU5D_tDFCDEE2A6322F96C0FE49AF47E9ADB8C4B294E86* ___delegates_11;

public:
	inline static int32_t get_offset_of_delegates_11() { return static_cast<int32_t>(offsetof(MulticastDelegate_t, ___delegates_11)); }
	inline DelegateU5BU5D_tDFCDEE2A6322F96C0FE49AF47E9ADB8C4B294E86* get_delegates_11() const { return ___delegates_11; }
	inline DelegateU5BU5D_tDFCDEE2A6322F96C0FE49AF47E9ADB8C4B294E86** get_address_of_delegates_11() { return &___delegates_11; }
	inline void set_delegates_11(DelegateU5BU5D_tDFCDEE2A6322F96C0FE49AF47E9ADB8C4B294E86* value)
	{
		___delegates_11 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___delegates_11), (void*)value);
	}
};

// Native definition for P/Invoke marshalling of System.MulticastDelegate
struct MulticastDelegate_t_marshaled_pinvoke : public Delegate_t_marshaled_pinvoke
{
	Delegate_t_marshaled_pinvoke** ___delegates_11;
};
// Native definition for COM marshalling of System.MulticastDelegate
struct MulticastDelegate_t_marshaled_com : public Delegate_t_marshaled_com
{
	Delegate_t_marshaled_com** ___delegates_11;
};

// UnityEngine.GameObject
struct  GameObject_tBD1244AD56B4E59AAD76E5E7C9282EC5CE434F0F  : public Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0
{
public:

public:
};


// UnityEngine.Networking.NetworkClient
struct  NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F  : public RuntimeObject
{
public:
	// System.Type UnityEngine.Networking.NetworkClient::m_NetworkConnectionClass
	Type_t * ___m_NetworkConnectionClass_0;
	// UnityEngine.Networking.HostTopology UnityEngine.Networking.NetworkClient::m_HostTopology
	HostTopology_tD01D253330A0DAA736EDFC67EE9585C363FA9B0E * ___m_HostTopology_4;
	// System.Int32 UnityEngine.Networking.NetworkClient::m_HostPort
	int32_t ___m_HostPort_5;
	// System.Boolean UnityEngine.Networking.NetworkClient::m_UseSimulator
	bool ___m_UseSimulator_6;
	// System.Int32 UnityEngine.Networking.NetworkClient::m_SimulatedLatency
	int32_t ___m_SimulatedLatency_7;
	// System.Single UnityEngine.Networking.NetworkClient::m_PacketLoss
	float ___m_PacketLoss_8;
	// System.String UnityEngine.Networking.NetworkClient::m_ServerIp
	String_t* ___m_ServerIp_9;
	// System.Int32 UnityEngine.Networking.NetworkClient::m_ServerPort
	int32_t ___m_ServerPort_10;
	// System.Int32 UnityEngine.Networking.NetworkClient::m_ClientId
	int32_t ___m_ClientId_11;
	// System.Int32 UnityEngine.Networking.NetworkClient::m_ClientConnectionId
	int32_t ___m_ClientConnectionId_12;
	// System.Int32 UnityEngine.Networking.NetworkClient::m_StatResetTime
	int32_t ___m_StatResetTime_13;
	// System.Net.EndPoint UnityEngine.Networking.NetworkClient::m_RemoteEndPoint
	EndPoint_tD87FCEF2780A951E8CE8D808C345FBF2C088D980 * ___m_RemoteEndPoint_14;
	// UnityEngine.Networking.NetworkMessageHandlers UnityEngine.Networking.NetworkClient::m_MessageHandlers
	NetworkMessageHandlers_tA7BB2E51BDBD8ECE976AD44F1B634F40EA9807D4 * ___m_MessageHandlers_16;
	// UnityEngine.Networking.NetworkConnection UnityEngine.Networking.NetworkClient::m_Connection
	NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA * ___m_Connection_17;
	// System.Byte[] UnityEngine.Networking.NetworkClient::m_MsgBuffer
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___m_MsgBuffer_18;
	// UnityEngine.Networking.NetworkReader UnityEngine.Networking.NetworkClient::m_MsgReader
	NetworkReader_t7011A2F66F461EA5D4413F3979F1F3244D82FD12 * ___m_MsgReader_19;
	// UnityEngine.Networking.NetworkClient_ConnectState UnityEngine.Networking.NetworkClient::m_AsyncConnect
	int32_t ___m_AsyncConnect_20;
	// System.String UnityEngine.Networking.NetworkClient::m_RequestedServerHost
	String_t* ___m_RequestedServerHost_21;

public:
	inline static int32_t get_offset_of_m_NetworkConnectionClass_0() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F, ___m_NetworkConnectionClass_0)); }
	inline Type_t * get_m_NetworkConnectionClass_0() const { return ___m_NetworkConnectionClass_0; }
	inline Type_t ** get_address_of_m_NetworkConnectionClass_0() { return &___m_NetworkConnectionClass_0; }
	inline void set_m_NetworkConnectionClass_0(Type_t * value)
	{
		___m_NetworkConnectionClass_0 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_NetworkConnectionClass_0), (void*)value);
	}

	inline static int32_t get_offset_of_m_HostTopology_4() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F, ___m_HostTopology_4)); }
	inline HostTopology_tD01D253330A0DAA736EDFC67EE9585C363FA9B0E * get_m_HostTopology_4() const { return ___m_HostTopology_4; }
	inline HostTopology_tD01D253330A0DAA736EDFC67EE9585C363FA9B0E ** get_address_of_m_HostTopology_4() { return &___m_HostTopology_4; }
	inline void set_m_HostTopology_4(HostTopology_tD01D253330A0DAA736EDFC67EE9585C363FA9B0E * value)
	{
		___m_HostTopology_4 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_HostTopology_4), (void*)value);
	}

	inline static int32_t get_offset_of_m_HostPort_5() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F, ___m_HostPort_5)); }
	inline int32_t get_m_HostPort_5() const { return ___m_HostPort_5; }
	inline int32_t* get_address_of_m_HostPort_5() { return &___m_HostPort_5; }
	inline void set_m_HostPort_5(int32_t value)
	{
		___m_HostPort_5 = value;
	}

	inline static int32_t get_offset_of_m_UseSimulator_6() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F, ___m_UseSimulator_6)); }
	inline bool get_m_UseSimulator_6() const { return ___m_UseSimulator_6; }
	inline bool* get_address_of_m_UseSimulator_6() { return &___m_UseSimulator_6; }
	inline void set_m_UseSimulator_6(bool value)
	{
		___m_UseSimulator_6 = value;
	}

	inline static int32_t get_offset_of_m_SimulatedLatency_7() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F, ___m_SimulatedLatency_7)); }
	inline int32_t get_m_SimulatedLatency_7() const { return ___m_SimulatedLatency_7; }
	inline int32_t* get_address_of_m_SimulatedLatency_7() { return &___m_SimulatedLatency_7; }
	inline void set_m_SimulatedLatency_7(int32_t value)
	{
		___m_SimulatedLatency_7 = value;
	}

	inline static int32_t get_offset_of_m_PacketLoss_8() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F, ___m_PacketLoss_8)); }
	inline float get_m_PacketLoss_8() const { return ___m_PacketLoss_8; }
	inline float* get_address_of_m_PacketLoss_8() { return &___m_PacketLoss_8; }
	inline void set_m_PacketLoss_8(float value)
	{
		___m_PacketLoss_8 = value;
	}

	inline static int32_t get_offset_of_m_ServerIp_9() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F, ___m_ServerIp_9)); }
	inline String_t* get_m_ServerIp_9() const { return ___m_ServerIp_9; }
	inline String_t** get_address_of_m_ServerIp_9() { return &___m_ServerIp_9; }
	inline void set_m_ServerIp_9(String_t* value)
	{
		___m_ServerIp_9 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_ServerIp_9), (void*)value);
	}

	inline static int32_t get_offset_of_m_ServerPort_10() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F, ___m_ServerPort_10)); }
	inline int32_t get_m_ServerPort_10() const { return ___m_ServerPort_10; }
	inline int32_t* get_address_of_m_ServerPort_10() { return &___m_ServerPort_10; }
	inline void set_m_ServerPort_10(int32_t value)
	{
		___m_ServerPort_10 = value;
	}

	inline static int32_t get_offset_of_m_ClientId_11() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F, ___m_ClientId_11)); }
	inline int32_t get_m_ClientId_11() const { return ___m_ClientId_11; }
	inline int32_t* get_address_of_m_ClientId_11() { return &___m_ClientId_11; }
	inline void set_m_ClientId_11(int32_t value)
	{
		___m_ClientId_11 = value;
	}

	inline static int32_t get_offset_of_m_ClientConnectionId_12() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F, ___m_ClientConnectionId_12)); }
	inline int32_t get_m_ClientConnectionId_12() const { return ___m_ClientConnectionId_12; }
	inline int32_t* get_address_of_m_ClientConnectionId_12() { return &___m_ClientConnectionId_12; }
	inline void set_m_ClientConnectionId_12(int32_t value)
	{
		___m_ClientConnectionId_12 = value;
	}

	inline static int32_t get_offset_of_m_StatResetTime_13() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F, ___m_StatResetTime_13)); }
	inline int32_t get_m_StatResetTime_13() const { return ___m_StatResetTime_13; }
	inline int32_t* get_address_of_m_StatResetTime_13() { return &___m_StatResetTime_13; }
	inline void set_m_StatResetTime_13(int32_t value)
	{
		___m_StatResetTime_13 = value;
	}

	inline static int32_t get_offset_of_m_RemoteEndPoint_14() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F, ___m_RemoteEndPoint_14)); }
	inline EndPoint_tD87FCEF2780A951E8CE8D808C345FBF2C088D980 * get_m_RemoteEndPoint_14() const { return ___m_RemoteEndPoint_14; }
	inline EndPoint_tD87FCEF2780A951E8CE8D808C345FBF2C088D980 ** get_address_of_m_RemoteEndPoint_14() { return &___m_RemoteEndPoint_14; }
	inline void set_m_RemoteEndPoint_14(EndPoint_tD87FCEF2780A951E8CE8D808C345FBF2C088D980 * value)
	{
		___m_RemoteEndPoint_14 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_RemoteEndPoint_14), (void*)value);
	}

	inline static int32_t get_offset_of_m_MessageHandlers_16() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F, ___m_MessageHandlers_16)); }
	inline NetworkMessageHandlers_tA7BB2E51BDBD8ECE976AD44F1B634F40EA9807D4 * get_m_MessageHandlers_16() const { return ___m_MessageHandlers_16; }
	inline NetworkMessageHandlers_tA7BB2E51BDBD8ECE976AD44F1B634F40EA9807D4 ** get_address_of_m_MessageHandlers_16() { return &___m_MessageHandlers_16; }
	inline void set_m_MessageHandlers_16(NetworkMessageHandlers_tA7BB2E51BDBD8ECE976AD44F1B634F40EA9807D4 * value)
	{
		___m_MessageHandlers_16 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_MessageHandlers_16), (void*)value);
	}

	inline static int32_t get_offset_of_m_Connection_17() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F, ___m_Connection_17)); }
	inline NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA * get_m_Connection_17() const { return ___m_Connection_17; }
	inline NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA ** get_address_of_m_Connection_17() { return &___m_Connection_17; }
	inline void set_m_Connection_17(NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA * value)
	{
		___m_Connection_17 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_Connection_17), (void*)value);
	}

	inline static int32_t get_offset_of_m_MsgBuffer_18() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F, ___m_MsgBuffer_18)); }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* get_m_MsgBuffer_18() const { return ___m_MsgBuffer_18; }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821** get_address_of_m_MsgBuffer_18() { return &___m_MsgBuffer_18; }
	inline void set_m_MsgBuffer_18(ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* value)
	{
		___m_MsgBuffer_18 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_MsgBuffer_18), (void*)value);
	}

	inline static int32_t get_offset_of_m_MsgReader_19() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F, ___m_MsgReader_19)); }
	inline NetworkReader_t7011A2F66F461EA5D4413F3979F1F3244D82FD12 * get_m_MsgReader_19() const { return ___m_MsgReader_19; }
	inline NetworkReader_t7011A2F66F461EA5D4413F3979F1F3244D82FD12 ** get_address_of_m_MsgReader_19() { return &___m_MsgReader_19; }
	inline void set_m_MsgReader_19(NetworkReader_t7011A2F66F461EA5D4413F3979F1F3244D82FD12 * value)
	{
		___m_MsgReader_19 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_MsgReader_19), (void*)value);
	}

	inline static int32_t get_offset_of_m_AsyncConnect_20() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F, ___m_AsyncConnect_20)); }
	inline int32_t get_m_AsyncConnect_20() const { return ___m_AsyncConnect_20; }
	inline int32_t* get_address_of_m_AsyncConnect_20() { return &___m_AsyncConnect_20; }
	inline void set_m_AsyncConnect_20(int32_t value)
	{
		___m_AsyncConnect_20 = value;
	}

	inline static int32_t get_offset_of_m_RequestedServerHost_21() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F, ___m_RequestedServerHost_21)); }
	inline String_t* get_m_RequestedServerHost_21() const { return ___m_RequestedServerHost_21; }
	inline String_t** get_address_of_m_RequestedServerHost_21() { return &___m_RequestedServerHost_21; }
	inline void set_m_RequestedServerHost_21(String_t* value)
	{
		___m_RequestedServerHost_21 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_RequestedServerHost_21), (void*)value);
	}
};

struct NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F_StaticFields
{
public:
	// System.Collections.Generic.List`1<UnityEngine.Networking.NetworkClient> UnityEngine.Networking.NetworkClient::s_Clients
	List_1_t51C4494E1C3EF6D60D76BC46D51A3376405941EE * ___s_Clients_2;
	// System.Boolean UnityEngine.Networking.NetworkClient::s_IsActive
	bool ___s_IsActive_3;
	// UnityEngine.Networking.NetworkSystem.CRCMessage UnityEngine.Networking.NetworkClient::s_CRCMessage
	CRCMessage_t7F44D52B267C35387F0D7AD0D9098D579ECF61FA * ___s_CRCMessage_15;

public:
	inline static int32_t get_offset_of_s_Clients_2() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F_StaticFields, ___s_Clients_2)); }
	inline List_1_t51C4494E1C3EF6D60D76BC46D51A3376405941EE * get_s_Clients_2() const { return ___s_Clients_2; }
	inline List_1_t51C4494E1C3EF6D60D76BC46D51A3376405941EE ** get_address_of_s_Clients_2() { return &___s_Clients_2; }
	inline void set_s_Clients_2(List_1_t51C4494E1C3EF6D60D76BC46D51A3376405941EE * value)
	{
		___s_Clients_2 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___s_Clients_2), (void*)value);
	}

	inline static int32_t get_offset_of_s_IsActive_3() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F_StaticFields, ___s_IsActive_3)); }
	inline bool get_s_IsActive_3() const { return ___s_IsActive_3; }
	inline bool* get_address_of_s_IsActive_3() { return &___s_IsActive_3; }
	inline void set_s_IsActive_3(bool value)
	{
		___s_IsActive_3 = value;
	}

	inline static int32_t get_offset_of_s_CRCMessage_15() { return static_cast<int32_t>(offsetof(NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F_StaticFields, ___s_CRCMessage_15)); }
	inline CRCMessage_t7F44D52B267C35387F0D7AD0D9098D579ECF61FA * get_s_CRCMessage_15() const { return ___s_CRCMessage_15; }
	inline CRCMessage_t7F44D52B267C35387F0D7AD0D9098D579ECF61FA ** get_address_of_s_CRCMessage_15() { return &___s_CRCMessage_15; }
	inline void set_s_CRCMessage_15(CRCMessage_t7F44D52B267C35387F0D7AD0D9098D579ECF61FA * value)
	{
		___s_CRCMessage_15 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___s_CRCMessage_15), (void*)value);
	}
};


// UnityEngine.Networking.NetworkConnection
struct  NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA  : public RuntimeObject
{
public:
	// UnityEngine.Networking.ChannelBuffer[] UnityEngine.Networking.NetworkConnection::m_Channels
	ChannelBufferU5BU5D_t75CDA99AB4F27F49A1DAA287CF43B1132505E6FA* ___m_Channels_0;
	// System.Collections.Generic.List`1<UnityEngine.Networking.PlayerController> UnityEngine.Networking.NetworkConnection::m_PlayerControllers
	List_1_t5761C753EAD0AAC9E064F2B3B61ED2992111850F * ___m_PlayerControllers_1;
	// UnityEngine.Networking.NetworkMessage UnityEngine.Networking.NetworkConnection::m_NetMsg
	NetworkMessage_tCD66E2AE395A185EFE622EBB5497C95F6754685C * ___m_NetMsg_2;
	// System.Collections.Generic.HashSet`1<UnityEngine.Networking.NetworkIdentity> UnityEngine.Networking.NetworkConnection::m_VisList
	HashSet_1_t11FE623F2912D3D97F683A80F888A092B31918D6 * ___m_VisList_3;
	// UnityEngine.Networking.NetworkWriter UnityEngine.Networking.NetworkConnection::m_Writer
	NetworkWriter_t9BE861BDE3F59F374D83A1E4CC697C73003FF030 * ___m_Writer_4;
	// System.Collections.Generic.Dictionary`2<System.Int16,UnityEngine.Networking.NetworkMessageDelegate> UnityEngine.Networking.NetworkConnection::m_MessageHandlersDict
	Dictionary_2_tC04929123F1414CEE8F4F8F7A76CC39B1BBDFFC3 * ___m_MessageHandlersDict_5;
	// UnityEngine.Networking.NetworkMessageHandlers UnityEngine.Networking.NetworkConnection::m_MessageHandlers
	NetworkMessageHandlers_tA7BB2E51BDBD8ECE976AD44F1B634F40EA9807D4 * ___m_MessageHandlers_6;
	// System.Collections.Generic.HashSet`1<UnityEngine.Networking.NetworkInstanceId> UnityEngine.Networking.NetworkConnection::m_ClientOwnedObjects
	HashSet_1_t04AFB5FB47684F4B89636F9B0A465D7E869FFA50 * ___m_ClientOwnedObjects_7;
	// UnityEngine.Networking.NetworkMessage UnityEngine.Networking.NetworkConnection::m_MessageInfo
	NetworkMessage_tCD66E2AE395A185EFE622EBB5497C95F6754685C * ___m_MessageInfo_8;
	// UnityEngine.Networking.NetworkError UnityEngine.Networking.NetworkConnection::error
	int32_t ___error_10;
	// System.Int32 UnityEngine.Networking.NetworkConnection::hostId
	int32_t ___hostId_11;
	// System.Int32 UnityEngine.Networking.NetworkConnection::connectionId
	int32_t ___connectionId_12;
	// System.Boolean UnityEngine.Networking.NetworkConnection::isReady
	bool ___isReady_13;
	// System.String UnityEngine.Networking.NetworkConnection::address
	String_t* ___address_14;
	// System.Single UnityEngine.Networking.NetworkConnection::lastMessageTime
	float ___lastMessageTime_15;
	// System.Boolean UnityEngine.Networking.NetworkConnection::logNetworkMessages
	bool ___logNetworkMessages_16;
	// System.Collections.Generic.Dictionary`2<System.Int16,UnityEngine.Networking.NetworkConnection_PacketStat> UnityEngine.Networking.NetworkConnection::m_PacketStats
	Dictionary_2_t12C98B97230995F2189EDF313C5425F959276BD5 * ___m_PacketStats_17;
	// System.Boolean UnityEngine.Networking.NetworkConnection::m_Disposed
	bool ___m_Disposed_18;

public:
	inline static int32_t get_offset_of_m_Channels_0() { return static_cast<int32_t>(offsetof(NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA, ___m_Channels_0)); }
	inline ChannelBufferU5BU5D_t75CDA99AB4F27F49A1DAA287CF43B1132505E6FA* get_m_Channels_0() const { return ___m_Channels_0; }
	inline ChannelBufferU5BU5D_t75CDA99AB4F27F49A1DAA287CF43B1132505E6FA** get_address_of_m_Channels_0() { return &___m_Channels_0; }
	inline void set_m_Channels_0(ChannelBufferU5BU5D_t75CDA99AB4F27F49A1DAA287CF43B1132505E6FA* value)
	{
		___m_Channels_0 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_Channels_0), (void*)value);
	}

	inline static int32_t get_offset_of_m_PlayerControllers_1() { return static_cast<int32_t>(offsetof(NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA, ___m_PlayerControllers_1)); }
	inline List_1_t5761C753EAD0AAC9E064F2B3B61ED2992111850F * get_m_PlayerControllers_1() const { return ___m_PlayerControllers_1; }
	inline List_1_t5761C753EAD0AAC9E064F2B3B61ED2992111850F ** get_address_of_m_PlayerControllers_1() { return &___m_PlayerControllers_1; }
	inline void set_m_PlayerControllers_1(List_1_t5761C753EAD0AAC9E064F2B3B61ED2992111850F * value)
	{
		___m_PlayerControllers_1 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_PlayerControllers_1), (void*)value);
	}

	inline static int32_t get_offset_of_m_NetMsg_2() { return static_cast<int32_t>(offsetof(NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA, ___m_NetMsg_2)); }
	inline NetworkMessage_tCD66E2AE395A185EFE622EBB5497C95F6754685C * get_m_NetMsg_2() const { return ___m_NetMsg_2; }
	inline NetworkMessage_tCD66E2AE395A185EFE622EBB5497C95F6754685C ** get_address_of_m_NetMsg_2() { return &___m_NetMsg_2; }
	inline void set_m_NetMsg_2(NetworkMessage_tCD66E2AE395A185EFE622EBB5497C95F6754685C * value)
	{
		___m_NetMsg_2 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_NetMsg_2), (void*)value);
	}

	inline static int32_t get_offset_of_m_VisList_3() { return static_cast<int32_t>(offsetof(NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA, ___m_VisList_3)); }
	inline HashSet_1_t11FE623F2912D3D97F683A80F888A092B31918D6 * get_m_VisList_3() const { return ___m_VisList_3; }
	inline HashSet_1_t11FE623F2912D3D97F683A80F888A092B31918D6 ** get_address_of_m_VisList_3() { return &___m_VisList_3; }
	inline void set_m_VisList_3(HashSet_1_t11FE623F2912D3D97F683A80F888A092B31918D6 * value)
	{
		___m_VisList_3 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_VisList_3), (void*)value);
	}

	inline static int32_t get_offset_of_m_Writer_4() { return static_cast<int32_t>(offsetof(NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA, ___m_Writer_4)); }
	inline NetworkWriter_t9BE861BDE3F59F374D83A1E4CC697C73003FF030 * get_m_Writer_4() const { return ___m_Writer_4; }
	inline NetworkWriter_t9BE861BDE3F59F374D83A1E4CC697C73003FF030 ** get_address_of_m_Writer_4() { return &___m_Writer_4; }
	inline void set_m_Writer_4(NetworkWriter_t9BE861BDE3F59F374D83A1E4CC697C73003FF030 * value)
	{
		___m_Writer_4 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_Writer_4), (void*)value);
	}

	inline static int32_t get_offset_of_m_MessageHandlersDict_5() { return static_cast<int32_t>(offsetof(NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA, ___m_MessageHandlersDict_5)); }
	inline Dictionary_2_tC04929123F1414CEE8F4F8F7A76CC39B1BBDFFC3 * get_m_MessageHandlersDict_5() const { return ___m_MessageHandlersDict_5; }
	inline Dictionary_2_tC04929123F1414CEE8F4F8F7A76CC39B1BBDFFC3 ** get_address_of_m_MessageHandlersDict_5() { return &___m_MessageHandlersDict_5; }
	inline void set_m_MessageHandlersDict_5(Dictionary_2_tC04929123F1414CEE8F4F8F7A76CC39B1BBDFFC3 * value)
	{
		___m_MessageHandlersDict_5 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_MessageHandlersDict_5), (void*)value);
	}

	inline static int32_t get_offset_of_m_MessageHandlers_6() { return static_cast<int32_t>(offsetof(NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA, ___m_MessageHandlers_6)); }
	inline NetworkMessageHandlers_tA7BB2E51BDBD8ECE976AD44F1B634F40EA9807D4 * get_m_MessageHandlers_6() const { return ___m_MessageHandlers_6; }
	inline NetworkMessageHandlers_tA7BB2E51BDBD8ECE976AD44F1B634F40EA9807D4 ** get_address_of_m_MessageHandlers_6() { return &___m_MessageHandlers_6; }
	inline void set_m_MessageHandlers_6(NetworkMessageHandlers_tA7BB2E51BDBD8ECE976AD44F1B634F40EA9807D4 * value)
	{
		___m_MessageHandlers_6 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_MessageHandlers_6), (void*)value);
	}

	inline static int32_t get_offset_of_m_ClientOwnedObjects_7() { return static_cast<int32_t>(offsetof(NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA, ___m_ClientOwnedObjects_7)); }
	inline HashSet_1_t04AFB5FB47684F4B89636F9B0A465D7E869FFA50 * get_m_ClientOwnedObjects_7() const { return ___m_ClientOwnedObjects_7; }
	inline HashSet_1_t04AFB5FB47684F4B89636F9B0A465D7E869FFA50 ** get_address_of_m_ClientOwnedObjects_7() { return &___m_ClientOwnedObjects_7; }
	inline void set_m_ClientOwnedObjects_7(HashSet_1_t04AFB5FB47684F4B89636F9B0A465D7E869FFA50 * value)
	{
		___m_ClientOwnedObjects_7 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_ClientOwnedObjects_7), (void*)value);
	}

	inline static int32_t get_offset_of_m_MessageInfo_8() { return static_cast<int32_t>(offsetof(NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA, ___m_MessageInfo_8)); }
	inline NetworkMessage_tCD66E2AE395A185EFE622EBB5497C95F6754685C * get_m_MessageInfo_8() const { return ___m_MessageInfo_8; }
	inline NetworkMessage_tCD66E2AE395A185EFE622EBB5497C95F6754685C ** get_address_of_m_MessageInfo_8() { return &___m_MessageInfo_8; }
	inline void set_m_MessageInfo_8(NetworkMessage_tCD66E2AE395A185EFE622EBB5497C95F6754685C * value)
	{
		___m_MessageInfo_8 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_MessageInfo_8), (void*)value);
	}

	inline static int32_t get_offset_of_error_10() { return static_cast<int32_t>(offsetof(NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA, ___error_10)); }
	inline int32_t get_error_10() const { return ___error_10; }
	inline int32_t* get_address_of_error_10() { return &___error_10; }
	inline void set_error_10(int32_t value)
	{
		___error_10 = value;
	}

	inline static int32_t get_offset_of_hostId_11() { return static_cast<int32_t>(offsetof(NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA, ___hostId_11)); }
	inline int32_t get_hostId_11() const { return ___hostId_11; }
	inline int32_t* get_address_of_hostId_11() { return &___hostId_11; }
	inline void set_hostId_11(int32_t value)
	{
		___hostId_11 = value;
	}

	inline static int32_t get_offset_of_connectionId_12() { return static_cast<int32_t>(offsetof(NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA, ___connectionId_12)); }
	inline int32_t get_connectionId_12() const { return ___connectionId_12; }
	inline int32_t* get_address_of_connectionId_12() { return &___connectionId_12; }
	inline void set_connectionId_12(int32_t value)
	{
		___connectionId_12 = value;
	}

	inline static int32_t get_offset_of_isReady_13() { return static_cast<int32_t>(offsetof(NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA, ___isReady_13)); }
	inline bool get_isReady_13() const { return ___isReady_13; }
	inline bool* get_address_of_isReady_13() { return &___isReady_13; }
	inline void set_isReady_13(bool value)
	{
		___isReady_13 = value;
	}

	inline static int32_t get_offset_of_address_14() { return static_cast<int32_t>(offsetof(NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA, ___address_14)); }
	inline String_t* get_address_14() const { return ___address_14; }
	inline String_t** get_address_of_address_14() { return &___address_14; }
	inline void set_address_14(String_t* value)
	{
		___address_14 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___address_14), (void*)value);
	}

	inline static int32_t get_offset_of_lastMessageTime_15() { return static_cast<int32_t>(offsetof(NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA, ___lastMessageTime_15)); }
	inline float get_lastMessageTime_15() const { return ___lastMessageTime_15; }
	inline float* get_address_of_lastMessageTime_15() { return &___lastMessageTime_15; }
	inline void set_lastMessageTime_15(float value)
	{
		___lastMessageTime_15 = value;
	}

	inline static int32_t get_offset_of_logNetworkMessages_16() { return static_cast<int32_t>(offsetof(NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA, ___logNetworkMessages_16)); }
	inline bool get_logNetworkMessages_16() const { return ___logNetworkMessages_16; }
	inline bool* get_address_of_logNetworkMessages_16() { return &___logNetworkMessages_16; }
	inline void set_logNetworkMessages_16(bool value)
	{
		___logNetworkMessages_16 = value;
	}

	inline static int32_t get_offset_of_m_PacketStats_17() { return static_cast<int32_t>(offsetof(NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA, ___m_PacketStats_17)); }
	inline Dictionary_2_t12C98B97230995F2189EDF313C5425F959276BD5 * get_m_PacketStats_17() const { return ___m_PacketStats_17; }
	inline Dictionary_2_t12C98B97230995F2189EDF313C5425F959276BD5 ** get_address_of_m_PacketStats_17() { return &___m_PacketStats_17; }
	inline void set_m_PacketStats_17(Dictionary_2_t12C98B97230995F2189EDF313C5425F959276BD5 * value)
	{
		___m_PacketStats_17 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_PacketStats_17), (void*)value);
	}

	inline static int32_t get_offset_of_m_Disposed_18() { return static_cast<int32_t>(offsetof(NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA, ___m_Disposed_18)); }
	inline bool get_m_Disposed_18() const { return ___m_Disposed_18; }
	inline bool* get_address_of_m_Disposed_18() { return &___m_Disposed_18; }
	inline void set_m_Disposed_18(bool value)
	{
		___m_Disposed_18 = value;
	}
};


// System.AsyncCallback
struct  AsyncCallback_t3F3DA3BEDAEE81DD1D24125DF8EB30E85EE14DA4  : public MulticastDelegate_t
{
public:

public:
};


// UnityEngine.Networking.LocalClient
struct  LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86  : public NetworkClient_t33B95FF43955FEC9083CA7222A143777B8B79F0F
{
public:
	// System.Collections.Generic.List`1<UnityEngine.Networking.LocalClient_InternalMsg> UnityEngine.Networking.LocalClient::m_InternalMsgs
	List_1_t163FEDD599DBF83121B7665744F773B0D0A681AB * ___m_InternalMsgs_23;
	// System.Collections.Generic.List`1<UnityEngine.Networking.LocalClient_InternalMsg> UnityEngine.Networking.LocalClient::m_InternalMsgs2
	List_1_t163FEDD599DBF83121B7665744F773B0D0A681AB * ___m_InternalMsgs2_24;
	// System.Collections.Generic.Stack`1<UnityEngine.Networking.LocalClient_InternalMsg> UnityEngine.Networking.LocalClient::m_FreeMessages
	Stack_1_tA8312A4E405D252C827FE2E3FCF65714642C8C47 * ___m_FreeMessages_25;
	// UnityEngine.Networking.NetworkServer UnityEngine.Networking.LocalClient::m_LocalServer
	NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 * ___m_LocalServer_26;
	// System.Boolean UnityEngine.Networking.LocalClient::m_Connected
	bool ___m_Connected_27;
	// UnityEngine.Networking.NetworkMessage UnityEngine.Networking.LocalClient::s_InternalMessage
	NetworkMessage_tCD66E2AE395A185EFE622EBB5497C95F6754685C * ___s_InternalMessage_28;

public:
	inline static int32_t get_offset_of_m_InternalMsgs_23() { return static_cast<int32_t>(offsetof(LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86, ___m_InternalMsgs_23)); }
	inline List_1_t163FEDD599DBF83121B7665744F773B0D0A681AB * get_m_InternalMsgs_23() const { return ___m_InternalMsgs_23; }
	inline List_1_t163FEDD599DBF83121B7665744F773B0D0A681AB ** get_address_of_m_InternalMsgs_23() { return &___m_InternalMsgs_23; }
	inline void set_m_InternalMsgs_23(List_1_t163FEDD599DBF83121B7665744F773B0D0A681AB * value)
	{
		___m_InternalMsgs_23 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_InternalMsgs_23), (void*)value);
	}

	inline static int32_t get_offset_of_m_InternalMsgs2_24() { return static_cast<int32_t>(offsetof(LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86, ___m_InternalMsgs2_24)); }
	inline List_1_t163FEDD599DBF83121B7665744F773B0D0A681AB * get_m_InternalMsgs2_24() const { return ___m_InternalMsgs2_24; }
	inline List_1_t163FEDD599DBF83121B7665744F773B0D0A681AB ** get_address_of_m_InternalMsgs2_24() { return &___m_InternalMsgs2_24; }
	inline void set_m_InternalMsgs2_24(List_1_t163FEDD599DBF83121B7665744F773B0D0A681AB * value)
	{
		___m_InternalMsgs2_24 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_InternalMsgs2_24), (void*)value);
	}

	inline static int32_t get_offset_of_m_FreeMessages_25() { return static_cast<int32_t>(offsetof(LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86, ___m_FreeMessages_25)); }
	inline Stack_1_tA8312A4E405D252C827FE2E3FCF65714642C8C47 * get_m_FreeMessages_25() const { return ___m_FreeMessages_25; }
	inline Stack_1_tA8312A4E405D252C827FE2E3FCF65714642C8C47 ** get_address_of_m_FreeMessages_25() { return &___m_FreeMessages_25; }
	inline void set_m_FreeMessages_25(Stack_1_tA8312A4E405D252C827FE2E3FCF65714642C8C47 * value)
	{
		___m_FreeMessages_25 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_FreeMessages_25), (void*)value);
	}

	inline static int32_t get_offset_of_m_LocalServer_26() { return static_cast<int32_t>(offsetof(LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86, ___m_LocalServer_26)); }
	inline NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 * get_m_LocalServer_26() const { return ___m_LocalServer_26; }
	inline NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 ** get_address_of_m_LocalServer_26() { return &___m_LocalServer_26; }
	inline void set_m_LocalServer_26(NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 * value)
	{
		___m_LocalServer_26 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_LocalServer_26), (void*)value);
	}

	inline static int32_t get_offset_of_m_Connected_27() { return static_cast<int32_t>(offsetof(LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86, ___m_Connected_27)); }
	inline bool get_m_Connected_27() const { return ___m_Connected_27; }
	inline bool* get_address_of_m_Connected_27() { return &___m_Connected_27; }
	inline void set_m_Connected_27(bool value)
	{
		___m_Connected_27 = value;
	}

	inline static int32_t get_offset_of_s_InternalMessage_28() { return static_cast<int32_t>(offsetof(LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86, ___s_InternalMessage_28)); }
	inline NetworkMessage_tCD66E2AE395A185EFE622EBB5497C95F6754685C * get_s_InternalMessage_28() const { return ___s_InternalMessage_28; }
	inline NetworkMessage_tCD66E2AE395A185EFE622EBB5497C95F6754685C ** get_address_of_s_InternalMessage_28() { return &___s_InternalMessage_28; }
	inline void set_s_InternalMessage_28(NetworkMessage_tCD66E2AE395A185EFE622EBB5497C95F6754685C * value)
	{
		___s_InternalMessage_28 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___s_InternalMessage_28), (void*)value);
	}
};


// UnityEngine.Networking.ULocalConnectionToClient
struct  ULocalConnectionToClient_t7AF7EBF2BEC3714F75EF894035BFAE9E6F9561A8  : public NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA
{
public:
	// UnityEngine.Networking.LocalClient UnityEngine.Networking.ULocalConnectionToClient::m_LocalClient
	LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86 * ___m_LocalClient_19;

public:
	inline static int32_t get_offset_of_m_LocalClient_19() { return static_cast<int32_t>(offsetof(ULocalConnectionToClient_t7AF7EBF2BEC3714F75EF894035BFAE9E6F9561A8, ___m_LocalClient_19)); }
	inline LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86 * get_m_LocalClient_19() const { return ___m_LocalClient_19; }
	inline LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86 ** get_address_of_m_LocalClient_19() { return &___m_LocalClient_19; }
	inline void set_m_LocalClient_19(LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86 * value)
	{
		___m_LocalClient_19 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_LocalClient_19), (void*)value);
	}
};


// UnityEngine.Networking.ULocalConnectionToServer
struct  ULocalConnectionToServer_tE6E34057F329C3E0E703C6F095DF82B0270557B8  : public NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA
{
public:
	// UnityEngine.Networking.NetworkServer UnityEngine.Networking.ULocalConnectionToServer::m_LocalServer
	NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 * ___m_LocalServer_19;

public:
	inline static int32_t get_offset_of_m_LocalServer_19() { return static_cast<int32_t>(offsetof(ULocalConnectionToServer_tE6E34057F329C3E0E703C6F095DF82B0270557B8, ___m_LocalServer_19)); }
	inline NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 * get_m_LocalServer_19() const { return ___m_LocalServer_19; }
	inline NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 ** get_address_of_m_LocalServer_19() { return &___m_LocalServer_19; }
	inline void set_m_LocalServer_19(NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 * value)
	{
		___m_LocalServer_19 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___m_LocalServer_19), (void*)value);
	}
};


// UnityEngine.Networking.UnSpawnDelegate
struct  UnSpawnDelegate_tDC1AD5AA3602EB703F4FA34792B4D4075582AE19  : public MulticastDelegate_t
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// System.Byte[]
struct ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821  : public RuntimeArray
{
public:
	ALIGN_FIELD (8) uint8_t m_Items[1];

public:
	inline uint8_t GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline uint8_t* GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, uint8_t value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
	}
	inline uint8_t GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline uint8_t* GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, uint8_t value)
	{
		m_Items[index] = value;
	}
};
// System.Delegate[]
struct DelegateU5BU5D_tDFCDEE2A6322F96C0FE49AF47E9ADB8C4B294E86  : public RuntimeArray
{
public:
	ALIGN_FIELD (8) Delegate_t * m_Items[1];

public:
	inline Delegate_t * GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline Delegate_t ** GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, Delegate_t * value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)m_Items + index, (void*)value);
	}
	inline Delegate_t * GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline Delegate_t ** GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, Delegate_t * value)
	{
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)m_Items + index, (void*)value);
	}
};



// System.Void System.Attribute::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Attribute__ctor_m45CAD4B01265CC84CC5A84F62EE2DBE85DE89EC0 (Attribute_tF048C13FB3C8CFCC53F82290E4A3F621089F9A74 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.NetworkConnection::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void NetworkConnection__ctor_m963910FF246B2657557F235695D9F18D548072AA (NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA * __this, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.LocalClient::InvokeHandlerOnClient(System.Int16,UnityEngine.Networking.MessageBase,System.Int32)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void LocalClient_InvokeHandlerOnClient_mC7039A6DA140D8F7B003604B90D275B97103A4FC (LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86 * __this, int16_t ___msgType0, MessageBase_t2EA42B01AD6A5F36EAF84BE623801951B9F55416 * ___msg1, int32_t ___channelId2, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.LocalClient::InvokeBytesOnClient(System.Byte[],System.Int32)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void LocalClient_InvokeBytesOnClient_mD21304A3A48F5A9379B35CBAB52E4EBDE4669618 (LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86 * __this, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___buffer0, int32_t ___channelId1, const RuntimeMethod* method);
// System.Byte[] UnityEngine.Networking.NetworkWriter::AsArray()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* NetworkWriter_AsArray_m6942FACAAA317851E142E290225FBD255ABF418D (NetworkWriter_t9BE861BDE3F59F374D83A1E4CC697C73003FF030 * __this, const RuntimeMethod* method);
// System.Boolean UnityEngine.Networking.NetworkServer::InvokeHandlerOnServer(UnityEngine.Networking.ULocalConnectionToServer,System.Int16,UnityEngine.Networking.MessageBase,System.Int32)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool NetworkServer_InvokeHandlerOnServer_mC2E8DFDCBD8FCB291D059A7BE1BC51F53BDA655B (NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 * __this, ULocalConnectionToServer_tE6E34057F329C3E0E703C6F095DF82B0270557B8 * ___conn0, int16_t ___msgType1, MessageBase_t2EA42B01AD6A5F36EAF84BE623801951B9F55416 * ___msg2, int32_t ___channelId3, const RuntimeMethod* method);
// System.Boolean UnityEngine.Networking.LogFilter::get_logError()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool LogFilter_get_logError_m782345C28E87D1E15882B5804CB36D5006389095 (const RuntimeMethod* method);
// System.Void UnityEngine.Debug::LogError(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Debug_LogError_m3BCF9B78263152261565DCA9DB7D55F0C391ED29 (RuntimeObject * ___message0, const RuntimeMethod* method);
// System.Boolean UnityEngine.Networking.NetworkServer::InvokeBytes(UnityEngine.Networking.ULocalConnectionToServer,System.Byte[],System.Int32,System.Int32)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool NetworkServer_InvokeBytes_m87C095B5C8756CCB6B1B4B11D9C819D2A2FBD554 (NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 * __this, ULocalConnectionToServer_tE6E34057F329C3E0E703C6F095DF82B0270557B8 * ___conn0, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___buffer1, int32_t ___numBytes2, int32_t ___channelId3, const RuntimeMethod* method);
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void UnityEngine.Networking.SyncVarAttribute::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void SyncVarAttribute__ctor_m353A6BC0E192AB7DBFC8F43F37ED2105E93A1775 (SyncVarAttribute_tD57FE395DED8D547F0200B7F50F36DFA27C6BF3A * __this, const RuntimeMethod* method)
{
	{
		Attribute__ctor_m45CAD4B01265CC84CC5A84F62EE2DBE85DE89EC0(__this, /*hidden argument*/NULL);
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
// System.Void UnityEngine.Networking.TargetRpcAttribute::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void TargetRpcAttribute__ctor_m5D47418DA5A7FA5AEDCC8E203F9436599AD04F2A (TargetRpcAttribute_t7B515CB5DD6D609483DFC4ACC89D00B00C9EAE03 * __this, const RuntimeMethod* method)
{
	{
		Attribute__ctor_m45CAD4B01265CC84CC5A84F62EE2DBE85DE89EC0(__this, /*hidden argument*/NULL);
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
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// UnityEngine.Networking.LocalClient UnityEngine.Networking.ULocalConnectionToClient::get_localClient()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86 * ULocalConnectionToClient_get_localClient_mFA1151CD224CF848FF175CE075EBECAA82C118E4 (ULocalConnectionToClient_t7AF7EBF2BEC3714F75EF894035BFAE9E6F9561A8 * __this, const RuntimeMethod* method)
{
	{
		LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86 * L_0 = __this->get_m_LocalClient_19();
		return L_0;
	}
}
// System.Void UnityEngine.Networking.ULocalConnectionToClient::.ctor(UnityEngine.Networking.LocalClient)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void ULocalConnectionToClient__ctor_mCF552AD09DD71A741FD56B4429F49D44F0238AE2 (ULocalConnectionToClient_t7AF7EBF2BEC3714F75EF894035BFAE9E6F9561A8 * __this, LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86 * ___localClient0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (ULocalConnectionToClient__ctor_mCF552AD09DD71A741FD56B4429F49D44F0238AE2_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		NetworkConnection__ctor_m963910FF246B2657557F235695D9F18D548072AA(__this, /*hidden argument*/NULL);
		((NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA *)__this)->set_address_14(_stringLiteral0C84AC39FC120C1E579C09FEAA062CA04994E08F);
		LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86 * L_0 = ___localClient0;
		__this->set_m_LocalClient_19(L_0);
		return;
	}
}
// System.Boolean UnityEngine.Networking.ULocalConnectionToClient::Send(System.Int16,UnityEngine.Networking.MessageBase)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool ULocalConnectionToClient_Send_mD185E65B90EF6053558337C7BFCF20D149AD5FD2 (ULocalConnectionToClient_t7AF7EBF2BEC3714F75EF894035BFAE9E6F9561A8 * __this, int16_t ___msgType0, MessageBase_t2EA42B01AD6A5F36EAF84BE623801951B9F55416 * ___msg1, const RuntimeMethod* method)
{
	{
		LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86 * L_0 = __this->get_m_LocalClient_19();
		int16_t L_1 = ___msgType0;
		MessageBase_t2EA42B01AD6A5F36EAF84BE623801951B9F55416 * L_2 = ___msg1;
		NullCheck(L_0);
		LocalClient_InvokeHandlerOnClient_mC7039A6DA140D8F7B003604B90D275B97103A4FC(L_0, L_1, L_2, 0, /*hidden argument*/NULL);
		return (bool)1;
	}
}
// System.Boolean UnityEngine.Networking.ULocalConnectionToClient::SendUnreliable(System.Int16,UnityEngine.Networking.MessageBase)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool ULocalConnectionToClient_SendUnreliable_m0936671CCBC1A3B0E519ABFEF5139ED274D69CD6 (ULocalConnectionToClient_t7AF7EBF2BEC3714F75EF894035BFAE9E6F9561A8 * __this, int16_t ___msgType0, MessageBase_t2EA42B01AD6A5F36EAF84BE623801951B9F55416 * ___msg1, const RuntimeMethod* method)
{
	{
		LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86 * L_0 = __this->get_m_LocalClient_19();
		int16_t L_1 = ___msgType0;
		MessageBase_t2EA42B01AD6A5F36EAF84BE623801951B9F55416 * L_2 = ___msg1;
		NullCheck(L_0);
		LocalClient_InvokeHandlerOnClient_mC7039A6DA140D8F7B003604B90D275B97103A4FC(L_0, L_1, L_2, 1, /*hidden argument*/NULL);
		return (bool)1;
	}
}
// System.Boolean UnityEngine.Networking.ULocalConnectionToClient::SendByChannel(System.Int16,UnityEngine.Networking.MessageBase,System.Int32)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool ULocalConnectionToClient_SendByChannel_m53558B620A8270E0BB2D7271C45C4F18DC830AF8 (ULocalConnectionToClient_t7AF7EBF2BEC3714F75EF894035BFAE9E6F9561A8 * __this, int16_t ___msgType0, MessageBase_t2EA42B01AD6A5F36EAF84BE623801951B9F55416 * ___msg1, int32_t ___channelId2, const RuntimeMethod* method)
{
	{
		LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86 * L_0 = __this->get_m_LocalClient_19();
		int16_t L_1 = ___msgType0;
		MessageBase_t2EA42B01AD6A5F36EAF84BE623801951B9F55416 * L_2 = ___msg1;
		int32_t L_3 = ___channelId2;
		NullCheck(L_0);
		LocalClient_InvokeHandlerOnClient_mC7039A6DA140D8F7B003604B90D275B97103A4FC(L_0, L_1, L_2, L_3, /*hidden argument*/NULL);
		return (bool)1;
	}
}
// System.Boolean UnityEngine.Networking.ULocalConnectionToClient::SendBytes(System.Byte[],System.Int32,System.Int32)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool ULocalConnectionToClient_SendBytes_m3AE08F6D87E0DFC3DD6D7461EA2CD999B4D4220D (ULocalConnectionToClient_t7AF7EBF2BEC3714F75EF894035BFAE9E6F9561A8 * __this, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___bytes0, int32_t ___numBytes1, int32_t ___channelId2, const RuntimeMethod* method)
{
	{
		LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86 * L_0 = __this->get_m_LocalClient_19();
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_1 = ___bytes0;
		int32_t L_2 = ___channelId2;
		NullCheck(L_0);
		LocalClient_InvokeBytesOnClient_mD21304A3A48F5A9379B35CBAB52E4EBDE4669618(L_0, L_1, L_2, /*hidden argument*/NULL);
		return (bool)1;
	}
}
// System.Boolean UnityEngine.Networking.ULocalConnectionToClient::SendWriter(UnityEngine.Networking.NetworkWriter,System.Int32)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool ULocalConnectionToClient_SendWriter_m793F7E1A1432DA674CB0A5BE68293EF8E80D7EF7 (ULocalConnectionToClient_t7AF7EBF2BEC3714F75EF894035BFAE9E6F9561A8 * __this, NetworkWriter_t9BE861BDE3F59F374D83A1E4CC697C73003FF030 * ___writer0, int32_t ___channelId1, const RuntimeMethod* method)
{
	{
		LocalClient_tCEC0096B13C433140FD4C09424CE345B28FE3C86 * L_0 = __this->get_m_LocalClient_19();
		NetworkWriter_t9BE861BDE3F59F374D83A1E4CC697C73003FF030 * L_1 = ___writer0;
		NullCheck(L_1);
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_2 = NetworkWriter_AsArray_m6942FACAAA317851E142E290225FBD255ABF418D(L_1, /*hidden argument*/NULL);
		int32_t L_3 = ___channelId1;
		NullCheck(L_0);
		LocalClient_InvokeBytesOnClient_mD21304A3A48F5A9379B35CBAB52E4EBDE4669618(L_0, L_2, L_3, /*hidden argument*/NULL);
		return (bool)1;
	}
}
// System.Void UnityEngine.Networking.ULocalConnectionToClient::GetStatsOut(System.Int32&,System.Int32&,System.Int32&,System.Int32&)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void ULocalConnectionToClient_GetStatsOut_m845BD674A16CF2373B032ADE3693158060E5692D (ULocalConnectionToClient_t7AF7EBF2BEC3714F75EF894035BFAE9E6F9561A8 * __this, int32_t* ___numMsgs0, int32_t* ___numBufferedMsgs1, int32_t* ___numBytes2, int32_t* ___lastBufferedPerSecond3, const RuntimeMethod* method)
{
	{
		int32_t* L_0 = ___numMsgs0;
		*((int32_t*)L_0) = (int32_t)0;
		int32_t* L_1 = ___numBufferedMsgs1;
		*((int32_t*)L_1) = (int32_t)0;
		int32_t* L_2 = ___numBytes2;
		*((int32_t*)L_2) = (int32_t)0;
		int32_t* L_3 = ___lastBufferedPerSecond3;
		*((int32_t*)L_3) = (int32_t)0;
		return;
	}
}
// System.Void UnityEngine.Networking.ULocalConnectionToClient::GetStatsIn(System.Int32&,System.Int32&)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void ULocalConnectionToClient_GetStatsIn_m557424F7D968448B3AA392C1ACCF1EF8A7FC4B62 (ULocalConnectionToClient_t7AF7EBF2BEC3714F75EF894035BFAE9E6F9561A8 * __this, int32_t* ___numMsgs0, int32_t* ___numBytes1, const RuntimeMethod* method)
{
	{
		int32_t* L_0 = ___numMsgs0;
		*((int32_t*)L_0) = (int32_t)0;
		int32_t* L_1 = ___numBytes1;
		*((int32_t*)L_1) = (int32_t)0;
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
// System.Void UnityEngine.Networking.ULocalConnectionToServer::.ctor(UnityEngine.Networking.NetworkServer)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void ULocalConnectionToServer__ctor_m98B9D5A9A831E4DAF3C504FA93965B4563C0E981 (ULocalConnectionToServer_tE6E34057F329C3E0E703C6F095DF82B0270557B8 * __this, NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 * ___localServer0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (ULocalConnectionToServer__ctor_m98B9D5A9A831E4DAF3C504FA93965B4563C0E981_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		NetworkConnection__ctor_m963910FF246B2657557F235695D9F18D548072AA(__this, /*hidden argument*/NULL);
		((NetworkConnection_t56E90DAE06B07A4A3233611CC9C0CBCD0A1CAFBA *)__this)->set_address_14(_stringLiteral8A90E4187AA462FD7BCD9E2521B1C4F09372DBAF);
		NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 * L_0 = ___localServer0;
		__this->set_m_LocalServer_19(L_0);
		return;
	}
}
// System.Boolean UnityEngine.Networking.ULocalConnectionToServer::Send(System.Int16,UnityEngine.Networking.MessageBase)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool ULocalConnectionToServer_Send_m8A764DD79C6DF286758E68194D4BBAA5DC9CA4A2 (ULocalConnectionToServer_tE6E34057F329C3E0E703C6F095DF82B0270557B8 * __this, int16_t ___msgType0, MessageBase_t2EA42B01AD6A5F36EAF84BE623801951B9F55416 * ___msg1, const RuntimeMethod* method)
{
	{
		NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 * L_0 = __this->get_m_LocalServer_19();
		int16_t L_1 = ___msgType0;
		MessageBase_t2EA42B01AD6A5F36EAF84BE623801951B9F55416 * L_2 = ___msg1;
		NullCheck(L_0);
		bool L_3 = NetworkServer_InvokeHandlerOnServer_mC2E8DFDCBD8FCB291D059A7BE1BC51F53BDA655B(L_0, __this, L_1, L_2, 0, /*hidden argument*/NULL);
		return L_3;
	}
}
// System.Boolean UnityEngine.Networking.ULocalConnectionToServer::SendUnreliable(System.Int16,UnityEngine.Networking.MessageBase)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool ULocalConnectionToServer_SendUnreliable_m59BDF4E98E520A24155F36780712B409C7F1CF85 (ULocalConnectionToServer_tE6E34057F329C3E0E703C6F095DF82B0270557B8 * __this, int16_t ___msgType0, MessageBase_t2EA42B01AD6A5F36EAF84BE623801951B9F55416 * ___msg1, const RuntimeMethod* method)
{
	{
		NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 * L_0 = __this->get_m_LocalServer_19();
		int16_t L_1 = ___msgType0;
		MessageBase_t2EA42B01AD6A5F36EAF84BE623801951B9F55416 * L_2 = ___msg1;
		NullCheck(L_0);
		bool L_3 = NetworkServer_InvokeHandlerOnServer_mC2E8DFDCBD8FCB291D059A7BE1BC51F53BDA655B(L_0, __this, L_1, L_2, 1, /*hidden argument*/NULL);
		return L_3;
	}
}
// System.Boolean UnityEngine.Networking.ULocalConnectionToServer::SendByChannel(System.Int16,UnityEngine.Networking.MessageBase,System.Int32)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool ULocalConnectionToServer_SendByChannel_m8FF0E370EC6754DF0AAE25D99CBD49006F2AB92F (ULocalConnectionToServer_tE6E34057F329C3E0E703C6F095DF82B0270557B8 * __this, int16_t ___msgType0, MessageBase_t2EA42B01AD6A5F36EAF84BE623801951B9F55416 * ___msg1, int32_t ___channelId2, const RuntimeMethod* method)
{
	{
		NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 * L_0 = __this->get_m_LocalServer_19();
		int16_t L_1 = ___msgType0;
		MessageBase_t2EA42B01AD6A5F36EAF84BE623801951B9F55416 * L_2 = ___msg1;
		int32_t L_3 = ___channelId2;
		NullCheck(L_0);
		bool L_4 = NetworkServer_InvokeHandlerOnServer_mC2E8DFDCBD8FCB291D059A7BE1BC51F53BDA655B(L_0, __this, L_1, L_2, L_3, /*hidden argument*/NULL);
		return L_4;
	}
}
// System.Boolean UnityEngine.Networking.ULocalConnectionToServer::SendBytes(System.Byte[],System.Int32,System.Int32)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool ULocalConnectionToServer_SendBytes_m0DD7C930ED218BD442317922C19250C58E7D319B (ULocalConnectionToServer_tE6E34057F329C3E0E703C6F095DF82B0270557B8 * __this, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___bytes0, int32_t ___numBytes1, int32_t ___channelId2, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (ULocalConnectionToServer_SendBytes_m0DD7C930ED218BD442317922C19250C58E7D319B_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		int32_t L_0 = ___numBytes1;
		if ((((int32_t)L_0) > ((int32_t)0)))
		{
			goto IL_0017;
		}
	}
	{
		IL2CPP_RUNTIME_CLASS_INIT(LogFilter_t5202A297E770086F7954B8D6703BAC03C22654ED_il2cpp_TypeInfo_var);
		bool L_1 = LogFilter_get_logError_m782345C28E87D1E15882B5804CB36D5006389095(/*hidden argument*/NULL);
		if (!L_1)
		{
			goto IL_0015;
		}
	}
	{
		IL2CPP_RUNTIME_CLASS_INIT(Debug_t7B5FCB117E2FD63B6838BC52821B252E2BFB61C4_il2cpp_TypeInfo_var);
		Debug_LogError_m3BCF9B78263152261565DCA9DB7D55F0C391ED29(_stringLiteral13F4B303180B12CAF8F069E93D7C4FAF969D3BC4, /*hidden argument*/NULL);
	}

IL_0015:
	{
		return (bool)0;
	}

IL_0017:
	{
		NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 * L_2 = __this->get_m_LocalServer_19();
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_3 = ___bytes0;
		int32_t L_4 = ___numBytes1;
		int32_t L_5 = ___channelId2;
		NullCheck(L_2);
		bool L_6 = NetworkServer_InvokeBytes_m87C095B5C8756CCB6B1B4B11D9C819D2A2FBD554(L_2, __this, L_3, L_4, L_5, /*hidden argument*/NULL);
		return L_6;
	}
}
// System.Boolean UnityEngine.Networking.ULocalConnectionToServer::SendWriter(UnityEngine.Networking.NetworkWriter,System.Int32)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool ULocalConnectionToServer_SendWriter_m7D87B54975237D4849CBE2C0AAC47D416DBE80DE (ULocalConnectionToServer_tE6E34057F329C3E0E703C6F095DF82B0270557B8 * __this, NetworkWriter_t9BE861BDE3F59F374D83A1E4CC697C73003FF030 * ___writer0, int32_t ___channelId1, const RuntimeMethod* method)
{
	{
		NetworkServer_tFD62C268FC6F01624A5989BFC1D0DD689A66B4A1 * L_0 = __this->get_m_LocalServer_19();
		NetworkWriter_t9BE861BDE3F59F374D83A1E4CC697C73003FF030 * L_1 = ___writer0;
		NullCheck(L_1);
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_2 = NetworkWriter_AsArray_m6942FACAAA317851E142E290225FBD255ABF418D(L_1, /*hidden argument*/NULL);
		NetworkWriter_t9BE861BDE3F59F374D83A1E4CC697C73003FF030 * L_3 = ___writer0;
		NullCheck(L_3);
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_4 = NetworkWriter_AsArray_m6942FACAAA317851E142E290225FBD255ABF418D(L_3, /*hidden argument*/NULL);
		NullCheck(L_4);
		int32_t L_5 = ___channelId1;
		NullCheck(L_0);
		bool L_6 = NetworkServer_InvokeBytes_m87C095B5C8756CCB6B1B4B11D9C819D2A2FBD554(L_0, __this, L_2, (((int16_t)((int16_t)(((int32_t)((int32_t)(((RuntimeArray*)L_4)->max_length))))))), L_5, /*hidden argument*/NULL);
		return L_6;
	}
}
// System.Void UnityEngine.Networking.ULocalConnectionToServer::GetStatsOut(System.Int32&,System.Int32&,System.Int32&,System.Int32&)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void ULocalConnectionToServer_GetStatsOut_m9CABB2E65C1EF8CE32576CF91FF57156C11960FD (ULocalConnectionToServer_tE6E34057F329C3E0E703C6F095DF82B0270557B8 * __this, int32_t* ___numMsgs0, int32_t* ___numBufferedMsgs1, int32_t* ___numBytes2, int32_t* ___lastBufferedPerSecond3, const RuntimeMethod* method)
{
	{
		int32_t* L_0 = ___numMsgs0;
		*((int32_t*)L_0) = (int32_t)0;
		int32_t* L_1 = ___numBufferedMsgs1;
		*((int32_t*)L_1) = (int32_t)0;
		int32_t* L_2 = ___numBytes2;
		*((int32_t*)L_2) = (int32_t)0;
		int32_t* L_3 = ___lastBufferedPerSecond3;
		*((int32_t*)L_3) = (int32_t)0;
		return;
	}
}
// System.Void UnityEngine.Networking.ULocalConnectionToServer::GetStatsIn(System.Int32&,System.Int32&)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void ULocalConnectionToServer_GetStatsIn_m334AA73B5B48095ABE01E8DFB346A424C60FA6EE (ULocalConnectionToServer_tE6E34057F329C3E0E703C6F095DF82B0270557B8 * __this, int32_t* ___numMsgs0, int32_t* ___numBytes1, const RuntimeMethod* method)
{
	{
		int32_t* L_0 = ___numMsgs0;
		*((int32_t*)L_0) = (int32_t)0;
		int32_t* L_1 = ___numBytes1;
		*((int32_t*)L_1) = (int32_t)0;
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
// System.Void UnityEngine.Networking.UnSpawnDelegate::.ctor(System.Object,System.IntPtr)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void UnSpawnDelegate__ctor_m1710488EC44693AF807968468030E8193F4CBF4C (UnSpawnDelegate_tDC1AD5AA3602EB703F4FA34792B4D4075582AE19 * __this, RuntimeObject * ___object0, intptr_t ___method1, const RuntimeMethod* method)
{
	__this->set_method_ptr_0(il2cpp_codegen_get_method_pointer((RuntimeMethod*)___method1));
	__this->set_method_3(___method1);
	__this->set_m_target_2(___object0);
}
// System.Void UnityEngine.Networking.UnSpawnDelegate::Invoke(UnityEngine.GameObject)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void UnSpawnDelegate_Invoke_m1F65543C3375DC627429071E48586FDB9B8036E3 (UnSpawnDelegate_tDC1AD5AA3602EB703F4FA34792B4D4075582AE19 * __this, GameObject_tBD1244AD56B4E59AAD76E5E7C9282EC5CE434F0F * ___spawned0, const RuntimeMethod* method)
{
	DelegateU5BU5D_tDFCDEE2A6322F96C0FE49AF47E9ADB8C4B294E86* delegateArrayToInvoke = __this->get_delegates_11();
	Delegate_t** delegatesToInvoke;
	il2cpp_array_size_t length;
	if (delegateArrayToInvoke != NULL)
	{
		length = delegateArrayToInvoke->max_length;
		delegatesToInvoke = reinterpret_cast<Delegate_t**>(delegateArrayToInvoke->GetAddressAtUnchecked(0));
	}
	else
	{
		length = 1;
		delegatesToInvoke = reinterpret_cast<Delegate_t**>(&__this);
	}

	for (il2cpp_array_size_t i = 0; i < length; i++)
	{
		Delegate_t* currentDelegate = delegatesToInvoke[i];
		Il2CppMethodPointer targetMethodPointer = currentDelegate->get_method_ptr_0();
		RuntimeObject* targetThis = currentDelegate->get_m_target_2();
		RuntimeMethod* targetMethod = (RuntimeMethod*)(currentDelegate->get_method_3());
		if (!il2cpp_codegen_method_is_virtual(targetMethod))
		{
			il2cpp_codegen_raise_execution_engine_exception_if_method_is_not_found(targetMethod);
		}
		bool ___methodIsStatic = MethodIsStatic(targetMethod);
		int ___parameterCount = il2cpp_codegen_method_parameter_count(targetMethod);
		if (___methodIsStatic)
		{
			if (___parameterCount == 1)
			{
				// open
				typedef void (*FunctionPointerType) (GameObject_tBD1244AD56B4E59AAD76E5E7C9282EC5CE434F0F *, const RuntimeMethod*);
				((FunctionPointerType)targetMethodPointer)(___spawned0, targetMethod);
			}
			else
			{
				// closed
				typedef void (*FunctionPointerType) (void*, GameObject_tBD1244AD56B4E59AAD76E5E7C9282EC5CE434F0F *, const RuntimeMethod*);
				((FunctionPointerType)targetMethodPointer)(targetThis, ___spawned0, targetMethod);
			}
		}
		else if (___parameterCount != 1)
		{
			// open
			if (il2cpp_codegen_method_is_virtual(targetMethod) && !il2cpp_codegen_object_is_of_sealed_type(targetThis) && il2cpp_codegen_delegate_has_invoker((Il2CppDelegate*)__this))
			{
				if (il2cpp_codegen_method_is_generic_instance(targetMethod))
				{
					if (il2cpp_codegen_method_is_interface_method(targetMethod))
						GenericInterfaceActionInvoker0::Invoke(targetMethod, ___spawned0);
					else
						GenericVirtActionInvoker0::Invoke(targetMethod, ___spawned0);
				}
				else
				{
					if (il2cpp_codegen_method_is_interface_method(targetMethod))
						InterfaceActionInvoker0::Invoke(il2cpp_codegen_method_get_slot(targetMethod), il2cpp_codegen_method_get_declaring_type(targetMethod), ___spawned0);
					else
						VirtActionInvoker0::Invoke(il2cpp_codegen_method_get_slot(targetMethod), ___spawned0);
				}
			}
			else
			{
				typedef void (*FunctionPointerType) (GameObject_tBD1244AD56B4E59AAD76E5E7C9282EC5CE434F0F *, const RuntimeMethod*);
				((FunctionPointerType)targetMethodPointer)(___spawned0, targetMethod);
			}
		}
		else
		{
			// closed
			if (il2cpp_codegen_method_is_virtual(targetMethod) && !il2cpp_codegen_object_is_of_sealed_type(targetThis) && il2cpp_codegen_delegate_has_invoker((Il2CppDelegate*)__this))
			{
				if (targetThis == NULL)
				{
					typedef void (*FunctionPointerType) (GameObject_tBD1244AD56B4E59AAD76E5E7C9282EC5CE434F0F *, const RuntimeMethod*);
					((FunctionPointerType)targetMethodPointer)(___spawned0, targetMethod);
				}
				else if (il2cpp_codegen_method_is_generic_instance(targetMethod))
				{
					if (il2cpp_codegen_method_is_interface_method(targetMethod))
						GenericInterfaceActionInvoker1< GameObject_tBD1244AD56B4E59AAD76E5E7C9282EC5CE434F0F * >::Invoke(targetMethod, targetThis, ___spawned0);
					else
						GenericVirtActionInvoker1< GameObject_tBD1244AD56B4E59AAD76E5E7C9282EC5CE434F0F * >::Invoke(targetMethod, targetThis, ___spawned0);
				}
				else
				{
					if (il2cpp_codegen_method_is_interface_method(targetMethod))
						InterfaceActionInvoker1< GameObject_tBD1244AD56B4E59AAD76E5E7C9282EC5CE434F0F * >::Invoke(il2cpp_codegen_method_get_slot(targetMethod), il2cpp_codegen_method_get_declaring_type(targetMethod), targetThis, ___spawned0);
					else
						VirtActionInvoker1< GameObject_tBD1244AD56B4E59AAD76E5E7C9282EC5CE434F0F * >::Invoke(il2cpp_codegen_method_get_slot(targetMethod), targetThis, ___spawned0);
				}
			}
			else
			{
				typedef void (*FunctionPointerType) (void*, GameObject_tBD1244AD56B4E59AAD76E5E7C9282EC5CE434F0F *, const RuntimeMethod*);
				((FunctionPointerType)targetMethodPointer)(targetThis, ___spawned0, targetMethod);
			}
		}
	}
}
// System.IAsyncResult UnityEngine.Networking.UnSpawnDelegate::BeginInvoke(UnityEngine.GameObject,System.AsyncCallback,System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* UnSpawnDelegate_BeginInvoke_m2C05956085CADC92B10D0D1533803739ED62F0E7 (UnSpawnDelegate_tDC1AD5AA3602EB703F4FA34792B4D4075582AE19 * __this, GameObject_tBD1244AD56B4E59AAD76E5E7C9282EC5CE434F0F * ___spawned0, AsyncCallback_t3F3DA3BEDAEE81DD1D24125DF8EB30E85EE14DA4 * ___callback1, RuntimeObject * ___object2, const RuntimeMethod* method)
{
	void *__d_args[2] = {0};
	__d_args[0] = ___spawned0;
	return (RuntimeObject*)il2cpp_codegen_delegate_begin_invoke((RuntimeDelegate*)__this, __d_args, (RuntimeDelegate*)___callback1, (RuntimeObject*)___object2);
}
// System.Void UnityEngine.Networking.UnSpawnDelegate::EndInvoke(System.IAsyncResult)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void UnSpawnDelegate_EndInvoke_mBCDA7B633E76A6E1E2D1EE281097E3AC4C21A878 (UnSpawnDelegate_tDC1AD5AA3602EB703F4FA34792B4D4075582AE19 * __this, RuntimeObject* ___result0, const RuntimeMethod* method)
{
	il2cpp_codegen_delegate_end_invoke((Il2CppAsyncResult*) ___result0, 0);
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
#ifdef __clang__
#pragma clang diagnostic pop
#endif
