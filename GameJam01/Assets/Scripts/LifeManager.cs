using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class LifeManager : NetworkBehaviour
{
  [Header("Life Values")]
  public int lifeValue = 1;
  public int lifeMax = 1;

  [HideInInspector()]
  public float lifePercent = 1.0f;

  [Header("Properties"),Tooltip("Is the gameobject will be destroy upon death or just in a 'death' state")]
  public bool isImmortal;

  // Use this for initialization
  void Start()
  {
    InitializeLife(lifeMax, isImmortal);
  }

  // Update is called once per frame
  void Update()
  {
  }

  //Hit and heal functions
  public void Hit(int damage) {
    this.lifeValue = (damage > this.lifeValue) ? 0 : this.lifeValue - damage;
    this.lifePercent = (float) this.lifeValue / (float)this.lifeMax * 100f;
  }

  void Heal(int value)
  {
    this.lifeValue += value;
    this.lifePercent = (this.lifeValue / this.lifeMax) * 100;
  }

  // Setters
  void InitializeLife(int lifeMax, bool isImmortal)
  {
    this.isImmortal = isImmortal;
    this.lifeMax = lifeMax;
    this.lifeValue = lifeMax;
    this.lifePercent = 100.0f;
  }

  void ChangeLifeMax(int lifeMax)
  {
    this.lifeMax = lifeMax;
    this.lifePercent = (this.lifeValue / this.lifeMax) * 100;
  }

}
