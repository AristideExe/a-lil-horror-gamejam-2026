using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



namespace CSVGeneration
{
    public class CSVGenerator
    {
        [MenuItem("Generate/Selected")]
        public static void GenerateSuspect()
        {
            if (Selection.activeObject == null) return;

            string path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), AssetDatabase.GetAssetPath(Selection.activeObject));

            if (!IsCSVFile(path))
            {
                Debug.LogError("Not a CSV File.");
                return;
            }

            List<Dictionary<string, object>> rawCSVData = CSVReading.CSVReader.Read(path);

            if (rawCSVData.Count > 0)
            {
                bool _casesGeneration = EditorUtility.DisplayDialog(
                    "Generation",
                    "What are you generating ?",
                    "Cases", "Suspects"
                );
                string thing =  _casesGeneration? "case" : "suspect";
                bool confirmed = EditorUtility.DisplayDialog(
                    $"Generating {thing}",
                    $"Are you sure you want to create {rawCSVData.Count} {thing} ?",
                    "YES", "NO"
                );

                if (confirmed)
                { 
                    if(_casesGeneration)
                        PerformGeneration(rawCSVData, true, false);
                    else
                        PerformGeneration(rawCSVData, false, true);
                }
            }
            else
            {
                Debug.LogError("No entries read from CSV.");
                return;
            }
        }

        public static void PerformGeneration(List<Dictionary<string, object>> csvData, bool _casesGeneration, bool _suspectsGeneration)
        {
            if  (!System.IO.Directory.Exists("Assets/ScriptableObjects"))
                System.IO.Directory.CreateDirectory("Assets/ScriptableObjects");

            for (int i = 0; i < csvData.Count; i++)
            {
                // if (i > 8) break;

                Dictionary<string, object> item = csvData[i];

                if (_casesGeneration)
                {
                    int id;
                    string context, facts;
                    SuspectData[] suspects;

                    id = int.Parse(item["id"].ToString());
                    string split = "&";
                    context = item["context"].ToString().Replace(split, "\n");
                    facts = item["facts"].ToString().Replace(split, "\n");

                    // Get suspects
                    string pathToSuspect = "Assets/ScriptableObjects/Suspects";

                    // Récupère tous les GUID des assets de ce type dans le dossier
                    string[] guids = AssetDatabase.FindAssets("t:SuspectData", new[] { pathToSuspect });
                    
                    SuspectData[] allSuspects = new SuspectData[guids.Length];

                    for (int j = 0; j < guids.Length; j++)
                    {
                        string cheminAsset = AssetDatabase.GUIDToAssetPath(guids[j]);
                        allSuspects[j] = AssetDatabase.LoadAssetAtPath<SuspectData>(cheminAsset);
                        Debug.Log(allSuspects[j]);
                    }

                    List<SuspectData> suspectsList = new();
                    foreach (SuspectData suspect in allSuspects)
                    {
                        if (int.Parse(suspect.c_ID) == id)
                            suspectsList.Add(suspect);
                    }
                    suspects = suspectsList.ToArray();

                    CreateScriptableObjectCase(id, context, facts, suspects);
                }
                if (_suspectsGeneration)
                {
                    Sprite picture;
                    GameObject[] models;
                    string s_ID, c_ID;
                    string[] s_name, s_firstName, s_pseudo, s_age, s_sex, s_work, s_height, s_weight, s_alibi;
                    int hair_id, hair_color, shirt_color, pants_color, scary_color;

                    c_ID = item["c_ID"].ToString();
                    s_ID = item["s_ID"].ToString();
                    string split = "&";
                    s_name = item["s_name"].ToString().Split(split);
                    s_firstName = item["s_firstName"].ToString().Split(split);
                    s_pseudo = item["s_pseudo"].ToString().Split(split);
                    s_age = item["s_age"].ToString().Split(split);
                    s_sex = item["s_sex"].ToString().Split(split);
                    s_work = item["s_work"].ToString().Split(split);
                    s_height = item["s_height"].ToString().Split(split);
                    s_weight = item["s_weight"].ToString().Split(split);
                    s_alibi = item["s_alibi"].ToString().Split(split);

                    hair_id = int.Parse(item["hair_id"].ToString());
                    hair_color = int.Parse(item["hair_color"].ToString());
                    shirt_color = int.Parse(item["shirt_color"].ToString());
                    pants_color = int.Parse(item["pants_color"].ToString());
                    scary_color = int.Parse(item["scary_color"].ToString());

                    ID suspectID = SuspectGeneration.Instance.CreateID(s_sex[0], hair_id, hair_color, shirt_color, pants_color, scary_color);

                    picture = suspectID.picture;

                    models = suspectID.models;
                    UnityEngine.Object.DestroyImmediate(models[0].gameObject);
                    UnityEngine.Object.DestroyImmediate(models[1].gameObject);

                    CreateScriptableObjectSuspect(
                        hair_id, hair_color, shirt_color, pants_color, scary_color,
                        s_ID,
                        picture,
                        s_ID, s_name, s_firstName, s_pseudo,
                        s_age, s_sex, s_work, s_height,
                        s_weight, s_alibi
                    );
                }
                
            }
        }

        private static void CreateScriptableObjectSuspect(
            int hair_id, int hair_color, int shirt_color, int pants_color, int scary_color,
            string c_ID,
            Sprite picture,
            string s_ID, string[] s_name, string[] s_firstName, string[] s_pseudo,
            string[] s_age, string[] s_sex, string[] s_work, string[] s_height,
            string[] s_weight, string[] s_alibi)
        {
            SuspectData suspect =  ScriptableObject.CreateInstance<SuspectData>();

            suspect.picture = picture;
            suspect.hair_color = hair_color;
            suspect.hair_id = hair_id;
            suspect.shirt_color = shirt_color;
            suspect.pants_color = pants_color;
            suspect.scary_color = scary_color;
            suspect.c_ID = c_ID;
            suspect.s_ID = s_ID;
            suspect.s_name = s_name;
            suspect.s_firstName = s_firstName;
            suspect.s_pseudo = s_pseudo;
            suspect.s_age = s_age;
            suspect.s_sex = s_sex;
            suspect.s_work = s_work;
            suspect.s_height = s_height;
            suspect.s_weight = s_weight;
            suspect.s_alibi = s_alibi;

            // Create the scriptable object
            string assetPath = $"Assets/ScriptableObjects/Suspects/suspect_{s_ID}.asset";
            AssetDatabase.CreateAsset(suspect, assetPath);

            picture.texture.name = $"texture_{s_ID}";
            picture.name = $"sprite_{s_ID}";
                
            AssetDatabase.AddObjectToAsset(picture.texture, suspect);
            AssetDatabase.AddObjectToAsset(picture, suspect);
                
            suspect.picture = picture;


            EditorUtility.SetDirty(suspect);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateScriptableObjectCase(int id, string context, string facts, SuspectData[] suspects)
        {
            CaseData _case =  ScriptableObject.CreateInstance<CaseData>();

            _case.id = id;
            _case.context = context;
            _case.facts = facts;
            _case.suspects = suspects;

            AssetDatabase.CreateAsset(_case, $"Assets/ScriptableObjects/Cases/case_{id}.asset");
            AssetDatabase.SaveAssets();
        }

        private static bool IsCSVFile(string fullPath)
        {
            return fullPath.ToLower().EndsWith(".csv");            
        }
    }
}