using System;
using System.Reflection;

namespace UnityEngine.TestTools.Utils
{
    internal class AssemblyWrapper : IAssemblyWrapper
    {
        public AssemblyWrapper(Assembly assembly)
        {
            Assembly = assembly;
        }

        public Assembly Assembly { get; }

        public virtual string Location
        {
            get
            {
                //Some platforms dont support this
                throw new NotImplementedException();
            }
        }

        public virtual AssemblyName[] GetReferencedAssemblies()
        {
            //Some platforms dont support this
            throw new NotImplementedException();
        }
    }
}
