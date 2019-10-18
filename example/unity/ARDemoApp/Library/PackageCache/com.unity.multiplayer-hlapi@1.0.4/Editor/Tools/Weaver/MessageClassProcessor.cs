using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Unity.UNetWeaver
{
    class MessageClassProcessor
    {
        TypeDefinition m_td;
        Weaver m_Weaver;

        public MessageClassProcessor(TypeDefinition td, Weaver weaver)
        {
            m_td = td;
            m_Weaver = weaver;
            m_Weaver.DLog(td, "MessageClassProcessor for " + td.Name);
        }

        public void Process()
        {
            m_Weaver.DLog(m_td, "MessageClassProcessor Start");

            m_Weaver.ResetRecursionCount();

            GenerateSerialization();
            if (m_Weaver.fail)
            {
                return;
            }

            GenerateDeSerialization();
            m_Weaver.DLog(m_td, "MessageClassProcessor Done");
        }

        void GenerateSerialization()
        {
            m_Weaver.DLog(m_td, "  GenerateSerialization");
            foreach (var m in m_td.Methods)
            {
                if (m.Name == "Serialize")
                    return;
            }

            if (m_td.Fields.Count == 0)
            {
                return;
            }

            // check for self-referencing types
            foreach (var field in m_td.Fields)
            {
                if (field.FieldType.FullName == m_td.FullName)
                {
                    m_Weaver.fail = true;
                    Log.Error("GenerateSerialization for " + m_td.Name + " [" + field.FullName + "]. [MessageBase] member cannot be self referencing.");
                    return;
                }
            }

            MethodDefinition serializeFunc = new MethodDefinition("Serialize", MethodAttributes.Public |
                    MethodAttributes.Virtual |
                    MethodAttributes.HideBySig,
                    m_Weaver.voidType);

            serializeFunc.Parameters.Add(new ParameterDefinition("writer", ParameterAttributes.None, m_Weaver.m_ScriptDef.MainModule.ImportReference(m_Weaver.NetworkWriterType)));
            ILProcessor serWorker = serializeFunc.Body.GetILProcessor();

            foreach (var field in m_td.Fields)
            {
                if (field.IsStatic || field.IsPrivate || field.IsSpecialName)
                    continue;

                if (field.FieldType.Resolve().HasGenericParameters)
                {
                    m_Weaver.fail = true;
                    Log.Error("GenerateSerialization for " + m_td.Name + " [" + field.FieldType + "/" + field.FieldType.FullName + "]. [MessageBase] member cannot have generic parameters.");
                    return;
                }

                if (field.FieldType.Resolve().IsInterface)
                {
                    m_Weaver.fail = true;
                    Log.Error("GenerateSerialization for " + m_td.Name + " [" + field.FieldType + "/" + field.FieldType.FullName + "]. [MessageBase] member cannot be an interface.");
                    return;
                }

                MethodReference writeFunc = m_Weaver.GetWriteFunc(field.FieldType);
                if (writeFunc != null)
                {
                    serWorker.Append(serWorker.Create(OpCodes.Ldarg_1));
                    serWorker.Append(serWorker.Create(OpCodes.Ldarg_0));
                    serWorker.Append(serWorker.Create(OpCodes.Ldfld, field));
                    serWorker.Append(serWorker.Create(OpCodes.Call, writeFunc));
                }
                else
                {
                    m_Weaver.fail = true;
                    Log.Error("GenerateSerialization for " + m_td.Name + " unknown type [" + field.FieldType + "/" + field.FieldType.FullName + "]. [MessageBase] member variables must be basic types.");
                    return;
                }
            }
            serWorker.Append(serWorker.Create(OpCodes.Ret));

            m_td.Methods.Add(serializeFunc);
        }

        void GenerateDeSerialization()
        {
            m_Weaver.DLog(m_td, "  GenerateDeserialization");
            foreach (var m in m_td.Methods)
            {
                if (m.Name == "Deserialize")
                    return;
            }

            if (m_td.Fields.Count == 0)
            {
                return;
            }

            MethodDefinition serializeFunc = new MethodDefinition("Deserialize", MethodAttributes.Public |
                    MethodAttributes.Virtual |
                    MethodAttributes.HideBySig,
                    m_Weaver.voidType);

            serializeFunc.Parameters.Add(new ParameterDefinition("reader", ParameterAttributes.None, m_Weaver.m_ScriptDef.MainModule.ImportReference(m_Weaver.NetworkReaderType)));
            ILProcessor serWorker = serializeFunc.Body.GetILProcessor();

            foreach (var field in m_td.Fields)
            {
                if (field.IsStatic || field.IsPrivate || field.IsSpecialName)
                    continue;

                MethodReference readerFunc = m_Weaver.GetReadFunc(field.FieldType);
                if (readerFunc != null)
                {
                    serWorker.Append(serWorker.Create(OpCodes.Ldarg_0));
                    serWorker.Append(serWorker.Create(OpCodes.Ldarg_1));
                    serWorker.Append(serWorker.Create(OpCodes.Call, readerFunc));
                    serWorker.Append(serWorker.Create(OpCodes.Stfld, field));
                }
                else
                {
                    m_Weaver.fail = true;
                    Log.Error("GenerateDeSerialization for " + m_td.Name + " unknown type [" + field.FieldType + "]. [SyncVar] member variables must be basic types.");
                    return;
                }
            }
            serWorker.Append(serWorker.Create(OpCodes.Ret));

            m_td.Methods.Add(serializeFunc);
        }
    }
}
