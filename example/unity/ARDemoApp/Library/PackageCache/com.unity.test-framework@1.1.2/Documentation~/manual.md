# Unity Test Framework manual

This is the manual for the Unity Test Framework (UTF):

## **Introduction**

* [Unity Test Framework overview](./index.md)
* [Edit Mode vs. Play Mode tests](edit-mode-vs-play-mode-tests.md)

## **Getting started**

* [Getting started with UTF](./getting-started.md)
  * Workflows:
    * [How to create a new test assembly](./workflow-create-test-assembly.md)
    * [How to create a test](./workflow-create-test.md)
    * [How to run a test](workflow-run-test.md)
    * [How to create a Play Mode test](./workflow-create-playmode-test.md)
    * [How to run a Play Mode test as standalone](./workflow-run-playmode-test-standalone.md)
* [Resources](./resources.md)

## Extending UTF

* [Extending UTF](./extending.md)
  * Workflows:
    * [How to split the build and run process for standalone Play Mode tests](./reference-attribute-testplayerbuildmodifier.md#split-build-and-run-for-player-mode-tests)
    * [How to run tests programmatically](./extension-run-tests.md)
    * [How to get test results](./extension-get-test-results.md)
    * [How to retrieve the list of tests](./extension-retrieve-test-list.md)

## Reference

* [Running tests from the command-line](./reference-command-line.md)
* [UnityTest attribute](./reference-attribute-unitytest.md)
* [Setup and cleanup at build time](./reference-setup-and-cleanup.md)
  * [IPrebuildSetup](./reference-setup-and-cleanup.md#iprebuildsetup)
  * [IPostBuildCleanup](./reference-setup-and-cleanup.md#ipostbuildcleanup)
* [Actions outside of tests](./reference-actions-outside-tests.md) 
  * [Action execution order](./reference-actions-outside-tests.md#action-execution-order)
  * [UnitySetUp and UnityTearDown](./reference-actions-outside-tests.md#unitysetup-and-unityteardown)
  * [OuterUnityTestAction](./reference-actions-outside-tests.md#outerunitytestaction)
  * [Domain Reloads](./reference-actions-outside-tests.md#domain-reloads)
* [Custom attributes](./reference-custom-attributes.md)
  * [ConditionalIgnore attribute](./reference-attribute-conditionalignore.md)
  * [PostBuildCleanup attribute](./reference-setup-and-cleanup.md#prebuildsetup-and-postbuildcleanup)
  * [PrebuildSetup attribute](./reference-setup-and-cleanup.md#prebuildsetup-and-postbuildcleanup)
  * [TestMustExpectAllLogs attribute](./reference-attribute-testmustexpectalllogs.md)
  * [TestPlayerBuildModifier attribute](./reference-attribute-testplayerbuildmodifier.md)
  * [TestRunCallback attribute](./reference-attribute-testruncallback.md)
  * [UnityPlatform attribute](./reference-attribute-unityplatform.md)
  * [UnitySetUp attribute](./reference-actions-outside-tests.md#unitysetup-and-unityteardown)
  * [UnityTearDown attribute](./reference-actions-outside-tests.md#unitysetup-and-unityteardown)
  * [UnityTest attribute](./reference-attribute-unitytest.md)
* [Custom equality comparers](./reference-custom-equality-comparers.md)
  * [ColorEqualityComparer](./reference-comparer-color.md)
  * [FloatEqualityComparer](./reference-comparer-float.md)
  * [QuaternionEqualityComparer](./reference-comparer-quaternion.md)
  * [Vector2EqualityComparer](./reference-comparer-vector2.md)
  * [Vector3EqualityComparer](./reference-comparer-vector3.md)
  * [Vector4EqualityComparer](./reference-comparer-vector4.md)
  * [Custom equality comparers with equals operator](./reference-comparer-equals.md)
  * [Test Utils](./reference-test-utils.md)
* [Custom yield instructions](./reference-custom-yield-instructions.md)
  * [IEditModeTestYieldInstruction](./reference-custom-yield-instructions.md#IEditModeTestYieldInstruction)
  * [EnterPlayMode](./reference-custom-yield-instructions.md#enterplaymode)
  * [ExitPlayMode](./reference-custom-yield-instructions.md#exitplaymode)
* [Custom assertion](./reference-custom-assertion.md)
  * [LogAssert](./reference-custom-assertion.md#logassert)
* [Custom constraints](./reference-custom-constraints.md) 
  * [Is](./reference-custom-constraints.md#is)
* [Parameterized tests](./reference-tests-parameterized.md)
* [MonoBehaviour tests](./reference-tests-monobehaviour.md)
  * [MonoBehaviourTest&lt;T&gt;](./reference-tests-monobehaviour.md#monobehaviourtestt)
  * [IMonoBehaviourTest](./reference-tests-monobehaviour.md#imonobehaviourtest)

* [TestRunnerApi](./reference-test-runner-api.md)
    * [ExecutionSettings](./reference-execution-settings.md)
    * [Filter](./reference-filter.md)
    * [ITestRunSettings](./reference-itest-run-settings.md)
    * [ICallbacks](./reference-icallbacks.md)
    * [IErrorCallbacks](./reference-ierror-callbacks.md)