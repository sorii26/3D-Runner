using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

public static class BankHallwayBuilder
{
    const string Folder = "Assets/BankHallways/Generated";
    static Material marble, dark, gold, green, white, black;
    [MenuItem("Tools/Money Heist/Build Bank Hallways")]
    public static void Build()
    {
        if (!AssetDatabase.IsValidFolder(Folder)) AssetDatabase.CreateFolder("Assets/BankHallways", "Generated");
        marble = Mat("Marble", new Color(.83f,.79f,.68f));
        dark = Mat("DarkGreen", new Color(.035f,.13f,.11f));
        gold = Mat("Brass", new Color(.72f,.48f,.16f), .65f);
        green = Mat("GreenPanel", new Color(.09f,.24f,.19f));
        white = Mat("Ivory", new Color(.96f,.91f,.79f));
        black = Mat("DarkStone", new Color(.18f,.20f,.19f));
        Make("StraightHallway",0); Make("LeftHallway",-1); Make("RightHallway",1);
        AssetDatabase.SaveAssets(); AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Bank hallways", "Three prefabs created in " + Folder + ". Check Console for any missing gameplay scripts.", "OK");
    }
    static Material Mat(string name, Color color, float metallic=0)
    {
        string path=Folder+"/"+name+".mat";
        Material m=AssetDatabase.LoadAssetAtPath<Material>(path);
        if(!m){ Shader s=Shader.Find("Universal Render Pipeline/Lit"); if(!s || UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline==null) s=Shader.Find("Standard"); m=new Material(s); AssetDatabase.CreateAsset(m,path); }
        m.color=color; if(m.HasProperty("_Metallic"))m.SetFloat("_Metallic",metallic); if(m.HasProperty("_Smoothness"))m.SetFloat("_Smoothness",.45f); if(m.HasProperty("_Glossiness"))m.SetFloat("_Glossiness",.45f); EditorUtility.SetDirty(m); return m;
    }
    static GameObject Cube(Transform p,string n,Vector3 pos,Vector3 scale,Material m,bool solid=false)
    {
        GameObject o=GameObject.CreatePrimitive(PrimitiveType.Cube); o.name=n; o.transform.SetParent(p,false); o.transform.localPosition=pos; o.transform.localScale=scale; o.GetComponent<Renderer>().sharedMaterial=m; if(!solid) UnityEngine.Object.DestroyImmediate(o.GetComponent<Collider>()); return o;
    }
    static Vector3 V(float x,float y,float z){return new Vector3(x,y,z);}
    static Transform Point(Transform p,string n,Vector3 pos,float yaw=0){var o=new GameObject(n).transform;o.SetParent(p,false);o.localPosition=pos;o.localRotation=Quaternion.Euler(0,yaw,0);return o;}
    static void Floor(Transform p,string n,float x,float z,float w,float l)
    {
        Cube(p,n,V(x,-.5f,z),V(w,1,l),marble,true);
        // Inlaid stone grid, flush enough to avoid gameplay collisions.
        for(float a=x-w/2+1.5f;a<x+w/2-.1f;a+=3) for(float b=z-l/2+1.5f;b<z+l/2-.1f;b+=3){float sx=Mathf.Min(2.94f,2*(x+w/2-a));float sz=Mathf.Min(2.94f,2*(z+l/2-b));Cube(p,"MarbleInlay",V(a,.004f,b),V(sx,.008f,sz),((Mathf.RoundToInt((a-x+w/2)/3)+Mathf.RoundToInt((b-z+l/2)/3))%2==0)?white:marble);}
    }
    static void Wall(Transform p,string n,float x,float z,float w,float l)
    {
        var a=Point(p,n,V(x,0,z)); Cube(a,"Wall",V(0,2,0),V(w,4,l),dark,true);
        bool alongZ=l>w; float len=alongZ?l:w;
        foreach(float y in new[]{.22f,1.05f,3.65f,3.94f}) Cube(a,"BrassTrim",V(0,y,0),V(w+(alongZ?.06f:0),.07f,l+(alongZ?0:.06f)),gold);
        for(float t=-len/2+1.5f;t<len/2-.3f;t+=3){float panel=Mathf.Min(2.4f,2*(len/2-t));Cube(a,"WallPanel",alongZ?V(0,2.25f,t):V(t,2.25f,0),alongZ?V(w+.025f,2.1f,panel):V(panel,2.1f,l+.025f),green);Cube(a,"Pilaster",alongZ?V(0,2,t-1.25f):V(t-1.25f,2,0),alongZ?V(w+.13f,3.9f,.16f):V(.16f,3.9f,l+.13f),gold);}
    }
    static Component Script(GameObject o,string name){var t=TypeCache.GetTypesDerivedFrom<MonoBehaviour>().FirstOrDefault(q=>q.Name==name);if(t==null){Debug.LogWarning("Bank hallway: missing "+name);return null;}return o.AddComponent(t);}
    static void Set(Component c,string field,UnityEngine.Object value){if(!c)return;var s=new SerializedObject(c);var p=s.FindProperty(field);if(p!=null){p.objectReferenceValue=value;s.ApplyModifiedPropertiesWithoutUndo();}else Debug.LogWarning("Missing field "+field);}
    static void Trigger(Transform t,Vector3 size){var b=t.gameObject.AddComponent<BoxCollider>();b.size=size;b.isTrigger=true;var r=t.gameObject.AddComponent<Rigidbody>();r.isKinematic=true;r.useGravity=false;}
    static void Make(string name,int d)
    {
        var root=new GameObject(name);var p=root.transform;
        try{
        Transform end;
        if(d==0){Floor(p,"Floor",0,10,9,20);Wall(p,"LeftWall",-4.75f,10,.5f,20);Wall(p,"RightWall",4.75f,10,.5f,20);end=Point(p,"EndPoint",V(0,0,20));}
        else{
            Floor(p,"EntranceFloor",0,5,9,10);Floor(p,"CornerFloor",0,14.5f,9,9);Floor(p,"ExitFloor",d*9.5f,14.5f,10,9);
            Wall(p,"OuterSideWall",-d*4.75f,9.5f,.5f,19);Wall(p,"OuterBackWall",d*4.75f,19.25f,19.5f,.5f);Wall(p,"InnerSideWall",d*4.75f,5,.5f,10);Wall(p,"InnerExitWall",d*9.75f,9.75f,9.5f,.5f);
            var turn=Point(p,"TurnPoint",V(0,0,14.5f),d*90);var zone=Point(p,"TurnTrigger",V(0,2,10));Trigger(zone,V(9,5,9));var tc=Script(zone.gameObject,"TurnTrigger");Set(tc,"turnPoint",turn);if(tc){var so=new SerializedObject(tc);var prop=so.FindProperty("turnDirection");if(prop!=null){prop.intValue=d;so.ApplyModifiedPropertiesWithoutUndo();}}
            end=Point(p,"EndPoint",V(d*14.5f,0,14.5f),d*90);
        }
        var exit=Point(end,"ExitTrigger",V(0,2,-1));Trigger(exit,V(9,4,.5f));Script(exit.gameObject,"TileExit");Set(Script(root,"HallwayTile"),"endPoint",end);
        // Decorative ceiling-height light fixtures; no solid geometry in the running lanes.
        foreach(float z in (d==0?new[]{5f,15f}:new[]{5f,14.5f})){
            Cube(p,"LightHousing",V(0,3.95f,z),V(2.4f,.12f,.6f),gold);Cube(p,"LightDiffuser",V(0,3.85f,z),V(2.1f,.1f,.4f),white);
            var lamp=Point(p,"WarmPointLight",V(0,3.5f,z));var light=lamp.gameObject.AddComponent<Light>();light.type=LightType.Point;light.color=new Color(1,.83f,.59f);light.range=10;light.intensity=2;light.shadows=LightShadows.None;
        }
        if(d!=0){Cube(p,"VaultSignBacking",V(0,2.8f,18.94f),V(3.5f,.7f,.1f),black);var sign=Point(p,"VaultSign",V(0,2.8f,18.86f));var tm=sign.gameObject.AddComponent<TextMesh>();tm.text="VAULT";tm.anchor=TextAnchor.MiddleCenter;tm.alignment=TextAlignment.Center;tm.characterSize=.24f;tm.fontSize=64;tm.color=new Color(1,.8f,.4f);}
        string path=AssetDatabase.GenerateUniqueAssetPath(Folder+"/"+name+".prefab");PrefabUtility.SaveAsPrefabAsset(root,path);
        }finally{UnityEngine.Object.DestroyImmediate(root);}
    }
}
