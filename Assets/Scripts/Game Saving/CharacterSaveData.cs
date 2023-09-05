using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PH{
[System.Serializable]
public class CharacterSaveData
{
    [Header("Scene Index")]
    public int sceneIndex;

    [Header("Character Name")]
   public string characterName = "Character";

   [Header("Time Played")]
   public float secondsPlayed;


   [Header("World Coordinates")]
   public float xPosition;
   public float yPosition;
   public float zPosition;

    [Header("Resources")]
    public int currentHealth;
    public float currentStamina;

   [Header("Rotation")]
   public float xRotation;
   [Header("Stats")]
   public int vitality;
   public int endurance;


}}