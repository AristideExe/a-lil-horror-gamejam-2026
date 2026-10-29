
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewSuspect",
    menuName = "Scriptable Objects/Suspect"
)]
public class SuspectData : ScriptableObject
{
    // The fisrt one is the truth the other one are lie
    [Header(">===== EXPESSION ======<")]
    public float sad;
    public float angry;
    public float happy;
    public float surprise;
    public int status = 0; // Calm / Disturbed / Hostile


    public Sprite picture;

    [Header(">====== MODEL ======<")]
    public int hair_id;
    public int hair_color;
    public int shirt_color;
    public int pants_color;
    public int scary_color;

    public string c_ID; // case_ID

    [Header(">====== ID ======<")]
    public string s_ID;
    public string[] s_name;
    public string[] s_firstName;
    public string[] s_pseudo;
    public string[] s_age;
    public string[] s_sex;
    public string[] s_work;


    [Header(">====== Suspect ID ======<")]
    public string[] s_height;
    public string[] s_weight;

    [Header(">====== Alibi ======<")]
    [TextArea(3, 8)]
    public string[] s_alibi;

    [Header(">====== Interrogation ======<")]
    public Dictionary<QuestionChoice, string> allQuestions = new();

    public SuspectChoice s_choice;
    public Level s_level;
}


[Serializable]
public class Questions
{
    [TextArea(2, 4)]
    public QuestionChoice question;

    [TextArea(2, 5)]
    public string answer;

    [Tooltip(">====== Suspect patience impact ======<")]
    public float patienceImpact = 0f;
}

public enum QuestionChoice
{
    Name,
    FirstName,
    Pseudo,
    Age,
    Sex,
    Work,
    Height,
    Weight,
    Alibi
}

public enum SuspectChoice
{
    Truth,
    Lie
}

public enum Level
{
    Easy, // 3-4 lies
    Normal, // 2-3 lies
    Hard, // 1-2 lies
    Expert // 1 lie
}

