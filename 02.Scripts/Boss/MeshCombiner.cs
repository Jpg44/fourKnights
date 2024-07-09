using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MeshCombiner : MonoBehaviour
{
    public GameObject Parent;
    public Material Material;
    public bool DeactivateParentAfterMerge = true;
    public bool DestroyParentAfterMerge = false;

    [ContextMenu("Merge")]
    public void MergeMeshes()
    {
        MeshFilter[] meshFilters = Parent.GetComponentsInChildren<MeshFilter>();
        List<CombineInstance> combineList = new List<CombineInstance>();

        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i].sharedMesh != null)
            {
                CombineInstance combineInstance = new CombineInstance
                {
                    mesh = meshFilters[i].sharedMesh,
                    transform = meshFilters[i].transform.localToWorldMatrix
                };
                combineList.Add(combineInstance);
            }
        }

        GameObject combinedObject = new GameObject("Combined Mesh");
        combinedObject.AddComponent<MeshFilter>();
        combinedObject.AddComponent<MeshRenderer>();
        Mesh combinedMesh = new Mesh { indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
        combinedMesh.CombineMeshes(combineList.ToArray(), true, true);
        combinedObject.GetComponent<MeshFilter>().sharedMesh = combinedMesh;
        combinedObject.GetComponent<MeshRenderer>().material = Material;

#if UNITY_EDITOR
        // Save the combined mesh as an asset
        string assetPath = "Assets/CombinedMesh.asset";
        AssetDatabase.CreateAsset(combinedMesh, assetPath);
        AssetDatabase.SaveAssets();

        // Convert to prefab
        string prefabPath = "Assets/CombinedMesh.prefab";
        PrefabUtility.SaveAsPrefabAsset(combinedObject, prefabPath);
#endif

        if (DeactivateParentAfterMerge)
        {
            Parent.SetActive(false);
        }

        if (DestroyParentAfterMerge)
        {
            Destroy(Parent);
        }

        // Optionally destroy the temporary combined object
        DestroyImmediate(combinedObject);
    }
}
