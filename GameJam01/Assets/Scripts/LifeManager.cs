using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class LifeManager : NetworkBehaviour {
  [Header("Life Values")]
  public float lifeValue = 1;
  public float lifeMax = 1;

  [HideInInspector()]
  public float lifePercent = 1.0f;

  [Header("Properties"), Tooltip("Is the gameobject will be destroy upon death or just in a 'death' state")]
  public bool isImmortal;

  [Tooltip("(optional) The animator of the attached gameObject. If defined, the LifeManager will try to trigger the 'BEIGN_HIT' animation if it exists")]
  public Animator entityAnimator;

  private int damageDuration = 0;
  private float damagePerSec = 0;

  // Use this for initialization
  void Start() {
    InitializeLife(lifeMax, isImmortal);
  }

  private void FixedUpdate() {
    if(damageDuration < 0) {
      if(damagePerSec < 0) {

      }
    }
  }

  //Hit and heal functions
  public void Hit(float damage) {
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

  public void ApplyLifeDebuff(float damage, int duration) {
    damagePerSec = damage;
    damageDuration = duration;
    Invoke("DealDebuffDamage", 1);
  }

  public void DealDebuffDamage() {
    Hit(damagePerSec);
    if(damageDuration > 0) {
      damageDuration--;
      Invoke("DealDebuffDamage", 1);
    }
  }

  public void Heal(float value) {
    if ((lifeValue + value) > lifeMax) {
      lifeValue = lifeMax;
      lifePercent = 100.0f;
    } else {
      lifeValue += value;
      UpdateLifePercent();
    }
  }

  // Setters
  void InitializeLife(float lifeMaxValue, bool isImmortalValue) {
    isImmortal = isImmortalValue;
    lifeMax = lifeMaxValue;
    lifeValue = lifeMaxValue;
    lifePercent = 100.0f;
  }

  void ChangeLifeMax(float lifeMaxValue) {
    lifeMax = lifeMaxValue;
    UpdateLifePercent();
  }

  private void UpdateLifePercent() {
    lifePercent = (float)lifeValue / (float)lifeMax * 100f;
  }
}
