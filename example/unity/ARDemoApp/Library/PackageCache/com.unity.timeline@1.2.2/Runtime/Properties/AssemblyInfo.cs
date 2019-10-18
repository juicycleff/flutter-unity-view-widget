using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("UnityEngine.Timeline")]
[assembly: AssemblyDescription("Unity Timeline")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Unity Technologies")]
[assembly: AssemblyProduct("UnityEngine.Timeline")]
[assembly: AssemblyCopyright("Copyright � 2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: InternalsVisibleTo("Unity.Timeline.Editor")]
[assembly: ComVisible(false)]
#if UNITY_EDITOR // RuntimeEditor version
[assembly: Guid("844F8153-91DB-42D4-9455-CBF1A7BFC434")]
#else // Runtime version
[assembly: Guid("6A10B290-9283-487F-913B-00D94CD3FAF5")]
#endif
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: InternalsVisibleTo("Assembly-CSharp-testable")]
[assembly: InternalsVisibleTo("Assembly-CSharp-Editor-testable")]
[assembly: InternalsVisibleTo("Unity.Timeline.EditorTests")]
[assembly: InternalsVisibleTo("Unity.Timeline.Tests")]
[assembly: InternalsVisibleTo("Unity.Timeline.Tests.Common")]
[assembly: InternalsVisibleTo("Unity.Timeline.Tests.Performance")]
[assembly: InternalsVisibleTo("Unity.Timeline.Tests.Performance.Editor")]
