using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

public static class PoliceBuilder {
 const string Folder="Assets/HeistPolice/Generated";
 static Material navy,blue,skin,black,gold,white;
 static Material Mat(string name, Color color) {
  string path=Folder+"/"+name+".mat";
  var m=AssetDatabase.LoadAssetAtPath<Material>(path); if(m!=null)return m;
  m=new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard")); m.color=color; AssetDatabase.CreateAsset(m,path); return m;
 }
 static GameObject Part(Transform parent,string name,PrimitiveType type,Vector3 pos,Vector3 scale,Material mat) {
  var g=GameObject.CreatePrimitive(type); g.name=name; g.transform.SetParent(parent,false); g.transform.localPosition=pos; g.transform.localScale=scale; g.GetComponent<Renderer>().sharedMaterial=mat; UnityEngine.Object.DestroyImmediate(g.GetComponent<Collider>()); return g;
 }
 static Transform Pivot(Transform parent,string name,Vector3 pos) {var g=new GameObject(name);g.transform.SetParent(parent,false);g.transform.localPosition=pos;return g.transform;}
 static void Ball(Transform p,string n,float x,float y,float z,float sx,float sy,float sz,Material m){Part(p,n,PrimitiveType.Sphere,new Vector3(x,y,z),new Vector3(sx,sy,sz),m);}
 static void Box(Transform p,string n,float x,float y,float z,float sx,float sy,float sz,Material m){Part(p,n,PrimitiveType.Cube,new Vector3(x,y,z),new Vector3(sx,sy,sz),m);}
 [MenuItem("Tools/Money Heist/Build Police Prefabs")]
 public static void Build(){
  if(!AssetDatabase.IsValidFolder(Folder))AssetDatabase.CreateFolder("Assets/HeistPolice","Generated");
  navy=Mat("Navy",new Color(.045f,.09f,.19f));blue=Mat("UniformBlue",new Color(.13f,.29f,.52f));skin=Mat("Skin",new Color(.92f,.65f,.43f));black=Mat("Boots",new Color(.035f,.04f,.05f));gold=Mat("Badge",new Color(.95f,.68f,.18f));white=Mat("Eyes",new Color(.98f,.95f,.87f));
  var root=new GameObject("StationaryPolice");var model=Pivot(root.transform,"PoliceModel",Vector3.zero);model.localRotation=Quaternion.Euler(0,180,0);
  Ball(model,"Torso",0,.78f,0,.63f,.65f,.42f,blue);Box(model,"Belt",0,.55f,0,.55f,.10f,.40f,black);Box(model,"Buckle",0,.55f,.22f,.13f,.09f,.04f,gold);Box(model,"Badge",-.17f,.91f,.22f,.10f,.13f,.035f,gold);Box(model,"Tie",0,.92f,.225f,.07f,.24f,.025f,navy);
  var head=Pivot(model,"Head",new Vector3(0,1.34f,0));Ball(head,"Face",0,0,0,.76f,.69f,.65f,skin);Ball(head,"Nose",0,-.02f,.32f,.13f,.13f,.13f,skin);Box(head,"Mouth",0,-.16f,.30f,.16f,.025f,.025f,navy);
  foreach(int s in new[]{-1,1}){Ball(head,"Ear",s*.36f,-.02f,0,.13f,.19f,.14f,skin);Ball(head,"Eye",s*.145f,.035f,.293f,.15f,.18f,.075f,white);Ball(head,"Pupil",s*.145f,.035f,.333f,.075f,.105f,.035f,black);Box(head,"Brow",s*.145f,.15f,.30f,.17f,.045f,.045f,navy);}
  Ball(head,"Cap",0,.30f,0,.85f,.29f,.73f,navy);Ball(head,"Visor",0,.22f,.27f,.67f,.065f,.42f,black);Box(head,"CapBadge",0,.32f,.35f,.12f,.12f,.035f,gold);
  foreach(int s in new[]{-1,1}){string side=s<0?"Left":"Right";var arm=Pivot(model,side+"Arm",new Vector3(s*.34f,.98f,0));Ball(arm,"Sleeve",s*.035f,-.14f,0,.22f,.37f,.24f,blue);Ball(arm,"Hand",s*.04f,-.32f,0,.19f,.20f,.20f,skin);var leg=Pivot(model,side+"Leg",new Vector3(s*.16f,.52f,0));Ball(leg,"Trouser",0,-.19f,0,.25f,.43f,.27f,navy);Ball(leg,"Boot",0,-.40f,.075f,.28f,.23f,.43f,black);}
  var col=root.AddComponent<CapsuleCollider>();col.center=new Vector3(0,.9f,0);col.height=1.8f;col.radius=.45f;col.isTrigger=true;var rb=root.AddComponent<Rigidbody>();rb.isKinematic=true;rb.useGravity=false;
  Add(root,"Obstacle");PrefabUtility.SaveAsPrefabAsset(root,Folder+"/StationaryPolice.prefab");root.name="MovingPolice";Add(root,"PoliceMovement");PrefabUtility.SaveAsPrefabAsset(root,Folder+"/MovingPolice.prefab");UnityEngine.Object.DestroyImmediate(root);AssetDatabase.SaveAssets();Debug.Log("Police prefabs created in "+Folder);
 }
 static void Add(GameObject g,string name){var t=TypeCache.GetTypesDerivedFrom<MonoBehaviour>().FirstOrDefault(x=>x.Name==name);if(t!=null)g.AddComponent(t);else Debug.LogWarning("Attach "+name+" manually when available.");}
}
