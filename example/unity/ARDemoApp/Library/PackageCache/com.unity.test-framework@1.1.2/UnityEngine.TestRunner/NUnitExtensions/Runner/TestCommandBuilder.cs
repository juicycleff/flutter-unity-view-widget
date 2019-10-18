using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;
using UnityEngine.TestTools;

namespace UnityEngine.TestRunner.NUnitExtensions.Runner
{
    internal static class TestCommandBuilder
    {
        public static TestCommand BuildTestCommand(TestMethod test, ITestFilter filter)
        {
            if (test.RunState != RunState.Runnable &&
                !(test.RunState == RunState.Explicit && filter.IsExplicitMatch(test)))
            {
                return new SkipCommand(test);
            }

            var testReturnsIEnumerator = test.Method.ReturnType.Type == typeof(IEnumerator); 
            
            TestCommand command;
            if (!testReturnsIEnumerator)
            {
                command = new TestMethodCommand(test);    
            }
            else
            {
                command = new EnumerableTestMethodCommand(test);
            }
            
            command = new UnityLogCheckDelegatingCommand(command);
            foreach (var wrapper in test.Method.GetCustomAttributes<IWrapTestMethod>(true))
            {
                command = wrapper.Wrap(command);
                if (command == null)
                {
                    var message = String.Format("IWrapTestMethod implementation '{0}' returned null as command.",
                        wrapper.GetType().FullName);
                    return new FailCommand(test, ResultState.Failure, message);
                }

                if (testReturnsIEnumerator && !(command is IEnumerableTestMethodCommand))
                {
                    command = TryReplaceWithEnumerableCommand(command);
                    if (command != null)
                    {
                        continue;
                    }
                    
                    var message = String.Format("'{0}' is not supported on {1} as it does not handle returning IEnumerator.",
                        wrapper.GetType().FullName,
                        GetTestBuilderName(test));
                    return new FailCommand(test, ResultState.Failure, message);
                }
            }

            command = new UnityEngine.TestTools.TestActionCommand(command);
            command = new UnityEngine.TestTools.SetUpTearDownCommand(command);
            
            if (!testReturnsIEnumerator)
            {
                command = new ImmediateEnumerableCommand(command);    
            }
            
            foreach (var wrapper in test.Method.GetCustomAttributes<IWrapSetUpTearDown>(true))
            {
                command = wrapper.Wrap(command);
                if (command == null)
                {
                    var message = String.Format("IWrapSetUpTearDown implementation '{0}' returned null as command.",
                        wrapper.GetType().FullName);
                    return new FailCommand(test, ResultState.Failure, message);
                }
                
                if (testReturnsIEnumerator && !(command is IEnumerableTestMethodCommand))
                {
                    command = TryReplaceWithEnumerableCommand(command);
                    if (command != null)
                    {
                        continue;
                    }
                    
                    var message = String.Format("'{0}' is not supported on {1} as it does not handle returning IEnumerator.",
                        wrapper.GetType().FullName,
                        GetTestBuilderName(test));
                    return new FailCommand(test, ResultState.Failure, message);
                }
            }

            command = new EnumerableSetUpTearDownCommand(command);
            command = new OuterUnityTestActionCommand(command);

            IApplyToContext[] changes = test.Method.GetCustomAttributes<IApplyToContext>(true);
            if (changes.Length > 0)
            {
                command = new EnumerableApplyChangesToContextCommand(command, changes);
            }

            return command;
        }

        private static string GetTestBuilderName(TestMethod testMethod)
        {
            return new[]
            {
                testMethod.Method.GetCustomAttributes<ITestBuilder>(true).Select(attribute => attribute.GetType().Name),
                testMethod.Method.GetCustomAttributes<ISimpleTestBuilder>(true).Select(attribute => attribute.GetType().Name)
            }.SelectMany(v => v).FirstOrDefault();
        }

        private static TestCommand TryReplaceWithEnumerableCommand(TestCommand command)
        {
            switch (command.GetType().Name)
            {
                case nameof(RepeatAttribute.RepeatedTestCommand):
                    return new EnumerableRepeatedTestCommand(command as RepeatAttribute.RepeatedTestCommand);
                case nameof(RetryAttribute.RetryCommand):
                    return new EnumerableRetryTestCommand(command as RetryAttribute.RetryCommand);
                default:
                    return null;
            }
        }
    }
}