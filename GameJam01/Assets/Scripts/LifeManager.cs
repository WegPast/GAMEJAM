using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class LifeManager : NetworkBehaviour {
  [Header("Life Values")]
  public int lifeValue = 1;
  public int lifeMax = 1;

  [HideInInspector()]
  public float lifePercent = 1.0f;

  [Header("Properties"), Tooltip("Is the gameobject will be destroy upon death or just in a 'death' state")]
  public bool isImmortal;

  [Tooltip("(optional) The animator of the attached gameObject. If defined, the LifeManager will try to trigger the 'BEIGN_HIT' animation if it exists")]
  public Animator entityAnimator;

  // Use this for initialization
  void Start() {
    InitializeLife(lifeMax, isImmortal);
  }

  //Hit and heal functions
  public void Hit(int damage) {
    if (entityAnimator) {
      AnimatorControllerParameter[] parameters = entityAnimator.parameters;
      foreach (var param in parameters) {
        if(param.name == "BEING_HIT") {
          entityAnimator.SetTrigger(param.name);
          break;
        }
      }
    }
    lifeValue = (damage > lifeValue) ? 0 : lifeValue - damage;
    UpdateLifePercent();
  }

  public void Heal(int value) {
    if ((lifeValue + value) > lifeMax) {
      lifeValue = lifeMax;
      lifePercent = 100.0f;
    } else {
      lifeValue += value;
      UpdateLifePercent();
    }
  }

  // Setters
  void InitializeLife(int lifeMaxValue, bool isImmortalValue) {
    isImmortal = isImmortalValue;
    lifeMax = lifeMaxValue;
    lifeValue = lifeMaxValue;
    lifePercent = 100.0f;
  }

  void ChangeLifeMax(int lifeMaxValue) {
    lifeMax = lifeMaxValue;
    UpdateLifePercent();
  }

  private void UpdateLifePercent() {
    lifePercent = (float)lifeValue / (float)lifeMax * 100f;
  }
}
