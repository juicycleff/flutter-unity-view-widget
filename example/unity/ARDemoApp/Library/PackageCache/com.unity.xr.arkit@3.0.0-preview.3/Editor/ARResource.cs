namespace UnityEditor.XR.ARKit
{
    internal abstract class ARResource
    {
        public abstract string extension { get; }

        public string name { get; protected set; }

        public string filename
        {
            get { return name + "." + extension; }
        }

        public abstract void Write(string pathToResourceGroup);
    }
}
