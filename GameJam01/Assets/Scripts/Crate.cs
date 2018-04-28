using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Crate : NetworkBehaviour
{

  [
      Header("Time before despawn"),
      Tooltip("Nombre de secondes avant la disparition de la crate"),
      Range(0, 10)
  ]
  public int secondesBeforeDespawn;

  public int commonChance = 70, rareChance = 25, legendaryChance = 5;

  public GameObject[] commonWeapons;
  public GameObject[] rareWeapons;
  public GameObject[] legendaryWeapons;

  private GameObject weaponInStock;

  // Use this for initialization
  void Start() {
    // Au start on determine ce qui sera le type de bonus.
    Destroy(this.gameObject, secondesBeforeDespawn);
    weaponInStock = GetRandomWeapon();
  }

  // Update is called once per frame
  void Update() {

  }

  private void OnTriggerEnter2D(Collider2D collision) {
    // On détruit la caisse quand le joueur passe dessus.
    if (collision.gameObject.GetComponent<PlayerControl>()) {
      collision.gameObject.GetComponent<PlayerControl>().ChangeGunWeapon(weaponInStock);
      Destroy(gameObject);
    }
  }

  private GameObject GetRandomWeapon() {
    int rarity = Random.Range(0, 100);
    if (rarity > 0 && rarity <= commonChance) { // COMMON
      int weaponIndex = Random.Range(0, commonWeapons.Length);
      if (commonWeapons[weaponIndex] != null) {
        return commonWeapons[weaponIndex];
      } else {
        throw new System.Exception("No weapon in this commonWeapons slot");
      }
    } else if (rarity > commonChance && rarity <= rareChance) { // RARE
      int weaponIndex = Random.Range(0, rareWeapons.Length);
      if (rareWeapons[weaponIndex] != null) {
        return rareWeapons[weaponIndex];
      } else {
        throw new System.Exception("No weapon in this rareWeapons slot");
      }
    } else { // LEGENDARY !
      int weaponIndex = Random.Range(0, legendaryWeapons.Length);
      if (legendaryWeapons[weaponIndex] != null) {
        return legendaryWeapons[weaponIndex];
      } else {
        throw new System.Exception("No weapon in this legendaryWeapons slot");
      }
    }
  }
}
