using Mono.Cecil;
using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using Mono.Cecil.Cil;
using UnityEngine;

public struct MockInstruction
{
    public OpCode OpCode;
    public object Operand;

    public MockInstruction(OpCode opCode) : this(opCode, null)
    {
    }

    public MockInstruction(OpCode opCode, object operand)
    {
        OpCode = opCode;
        Operand = operand;
    }

    public override string ToString()
    {
        if (Operand == null)
        {
            return OpCode.ToString();
        }

        if (Operand is String && OpCode == OpCodes.Ldstr)
        {
            return OpCode.ToString() + "\t\"" + Operand + " \"";
        }

        return OpCode.ToString() + "\t" + Operand;
    }
}

public static class WeaverILMatcher
{
    static Dictionary<string, AssemblyDefinition> AssemblyDefinitions;
    static AssemblyDefinition AssemblyCSharp;
    static AssemblyDefinition AssemblyCSharpEditortestable;

    static TypeDefinition GetTypeDef(Type type)
    {
        if (AssemblyDefinitions == null)
        {
            AssemblyDefinitions = new Dictionary<string, AssemblyDefinition>();
        }

        AssemblyDefinition asmDef;

        if (AssemblyDefinitions.TryGetValue(type.Assembly.Location, out asmDef) == false)
        {
            // read up assembly
            asmDef = AssemblyDefinition.ReadAssembly(type.Assembly.Location);

            // add to lookup so we don't need to read it every time
            AssemblyDefinitions.Add(type.Assembly.Location, asmDef);
        }

        var typeDef = asmDef.MainModule.Types.FirstOrDefault(x => x.FullName == type.FullName);

        Assert.NotNull(typeDef, "Could not find TypeDefinition for {0}", type);

        return typeDef;
    }

    public static void MatchMethodIL(Type type, string method, int parameterCount, params MockInstruction[] match)
    {
        MatchMethodIL(type, method, false, parameterCount, 1, match);
    }

    public static void MatchMethodIL(Type type, string method, bool hasReturnValue, int parameterCount, int matchCount, params MockInstruction[] match)
    {
        var typeDef = GetTypeDef(type);
        var methodDef = typeDef.Methods.FirstOrDefault(x => x.Name == method && (x.ReturnType.Name != "Void") == hasReturnValue && x.Parameters.Count == parameterCount);

        Assert.NotNull(methodDef, "Could not find method {0} on type {1}", method, type);

        var matches = 0;
        var instructions = methodDef.Body.Instructions.ToArray();

        for (int i = 0; i < instructions.Length; ++i)
        {
            if (instructions[i].OpCode == match[0].OpCode)
            {
                if (MatchInstructions(instructions, i, match))
                {
                    // -1: means at least once, and don't check specific count
                    if (matchCount == -1)
                    {
                        return;
                    }

                    ++matches;
                }
            }
        }

        if (matches != matchCount)
        {
            Assert.Fail("Method {0} on type {1} did not match IL pattern exactly {3} times:\r\n{2}", method, type, string.Join("\r\n", match.Select(x => x.ToString()).ToArray()), matchCount);
        }
    }

    static bool MatchInstructions(Instruction[] instructions, int i, MockInstruction[] match)
    {
        for (int m = 0; m < match.Length; ++m)
        {
            // il out of bounds
            if (m + i >= instructions.Length)
            {
                return false;
            }

            var i_in = instructions[m + i];
            var m_in = match[m];

            // miss-matching opcode
            if (i_in.OpCode != m_in.OpCode)
            {
                return false;
            }

            // special case when we pass a null-operand match value we only require the opcode to match
            if (m_in.Operand == null)
            {
                continue;
            }

            // miss-matching operand, expected an operand value but got none
            if (i_in.Operand == null && m_in.Operand != null)
            {
                return false;
            }

            // special case for "call/callvirt/newobj" verification where we match against a string instead of the actual MemberReference object
            if ((m_in.OpCode == OpCodes.Call || m_in.OpCode == OpCodes.Callvirt || m_in.OpCode == OpCodes.Newobj || m_in.OpCode == OpCodes.Isinst || m_in.OpCode == OpCodes.Stfld || m_in.OpCode == OpCodes.Ldfld) && m_in.Operand is string)
            {
                if (i_in.Operand.ToString() != (string)m_in.Operand)
                {
                    return false;
                }
            }

            // miss-matching operand type
            if (i_in.Operand.GetType() != m_in.Operand.GetType())
            {
                continue;
            }

            // miss-matching operand value
            if (i_in.Operand.Equals(m_in.Operand) == false)
            {
                return false;
            }
        }

        return true;
    }
}
