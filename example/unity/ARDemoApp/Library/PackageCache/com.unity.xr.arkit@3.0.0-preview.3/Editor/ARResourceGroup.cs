using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor.XR.ARKit
{
    internal class ARResourceGroup
    {
        public string name { get; set; }

        public ARResourceGroup(string name)
        {
            this.name = name;
        }

        public void AddResource(ARResource resource)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");

            if (m_Resources.Contains(resource))
                throw new InvalidOperationException(string.Format("Duplicate resource '{0}` in group '{1}'", resource.name, name));

            m_Resources.Add(resource);
        }

        internal void Write(string pathToAssetCatalog)
        {
            string path = Path.Combine(pathToAssetCatalog, name + ".arresourcegroup");
            Directory.CreateDirectory(path);

            // Build the contents json and write each resource to disk
            var contents = new Json.ResourceGroup
            {
                info = new Json.AuthorInfo
                {
                    version = 1,
                    author = "unity"
                },
                resources = new Json.Filename[m_Resources.Count]
            };

            for (int i = 0; i < m_Resources.Count; ++i)
            {
                var resource = m_Resources[i];
                contents.resources[i].filename = resource.filename;
                resource.Write(path);
            }

            // Finally, write out the json contents
            File.WriteAllText(Path.Combine(path, "Contents.json"), JsonUtility.ToJson(contents));
        }

        List<ARResource> m_Resources = new List<ARResource>();
    }
}
