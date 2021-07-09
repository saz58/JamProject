using System.IO;
using Artees.UnitySemVer;
using UnityEditor;
using UnityEngine;

namespace GT
{
    [CreateAssetMenu(fileName = "SemVersion", menuName = "ScriptableObjects/SemVersion", order = 1)]
    public class AppSemanticVersion : ScriptableObject
    {
        private static string AssetName => "SemVersion";
        private const string AssetExtension = ".asset";

        public SemVer semVer = new SemVer {major = 0, minor = 0, patch = 0};

        public string PackVer => semVer.ToString().Replace(".", "_");

        private static AppSemanticVersion _instance;

        public static AppSemanticVersion Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = Resources.Load(AssetName) as AppSemanticVersion;

                return _instance;
            }
        }

#if UNITY_EDITOR
        private static AppSemanticVersion Create()
        {
            if (_instance != null) return _instance;
            // If not found, autocreate the asset object.
            _instance = CreateInstance<AppSemanticVersion>();
            var fullPath = Path.Combine(Path.Combine("Assets", "Resources"), AssetName + AssetExtension);
            AssetDatabase.CreateAsset(_instance, fullPath);

            return _instance;
        }

        public void Save()
        {
            EditorUtility.SetDirty(Instance);
        }
#endif
    }
}