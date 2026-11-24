using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public static class RobberBuilder
{
    [Serializable] class Data { public Part[] parts; public Group[] groups; }
    [Serializable] class Group { public string name; public float[] pivot; }
    [Serializable] class Part { public string name, group; public float[] vertices, normals; public int[] triangles, color; }
    const string Folder = "Assets/HeistRobber";
    static Vector3 Vec(float[] a, int i) { return new Vector3(a[i], a[i+1], a[i+2]); }
    [MenuItem("Tools/Money Heist/Build Robber Prefab")]
    public static void Build()
    {
        var data = JsonUtility.FromJson<Data>(System.IO.File.ReadAllText(Folder + "/RobberData.json"));
        string output = AssetDatabase.GenerateUniqueAssetPath(Folder + "/Generated");
        AssetDatabase.CreateFolder(Folder, System.IO.Path.GetFileName(output));
        var root = new GameObject("RobberModel");
        var groups = new Dictionary<string, Transform>();
        var materials = new Dictionary<string, Material>();
        try {
            foreach (var g in data.groups) {
                var obj = new GameObject(g.name);
                obj.transform.SetParent(root.transform, false);
                obj.transform.localPosition = Vec(g.pivot, 0);
                groups.Add(g.name, obj.transform);
            }
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline == null || shader == null)
                shader = Shader.Find("Standard");
            if (shader == null) throw new Exception("No compatible shader found.");
            int index = 0;
            foreach (var p in data.parts) {
                var obj = new GameObject(p.name);
                obj.transform.SetParent(groups[p.group], false);
                var mesh = new Mesh { name = p.name };
                var vertices = new Vector3[p.vertices.Length / 3];
                var normals = new Vector3[vertices.Length];
                for (int i = 0; i < vertices.Length; i++) { vertices[i] = Vec(p.vertices, i*3); normals[i] = Vec(p.normals, i*3); }
                mesh.vertices = vertices; mesh.triangles = p.triangles; mesh.normals = normals; mesh.RecalculateBounds();
                AssetDatabase.CreateAsset(mesh, output + "/Mesh_" + index++ + ".asset");
                obj.AddComponent<MeshFilter>().sharedMesh = mesh;
                string key = string.Join("_", p.color);
                if (!materials.TryGetValue(key, out Material mat)) {
                    mat = new Material(shader);
                    mat.color = new Color32((byte)p.color[0], (byte)p.color[1], (byte)p.color[2], 255);
                    AssetDatabase.CreateAsset(mat, output + "/Color_" + key + ".mat");
                    materials.Add(key, mat);
                }
                obj.AddComponent<MeshRenderer>().sharedMaterial = mat;
            }
            var prefab = PrefabUtility.SaveAsPrefabAsset(root, output + "/RobberModel.prefab");
            AssetDatabase.SaveAssets();
            Selection.activeObject = prefab;
            EditorGUIUtility.PingObject(prefab);
        } finally { UnityEngine.Object.DestroyImmediate(root); }
    }
}
