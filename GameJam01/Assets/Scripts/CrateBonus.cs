using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CrateBonus : NetworkBehaviour
{

  [
      Header("Time before despawn"),
      Tooltip("Nombre de secondes avant la disparition de la crate"),
      Range(0, 10)
  ]
  public int secondesBeforeDespawn;

  public enum LootType
  {
    repairBonus,
    bigRepairBonus,
    hugeRepairBonus,
    lifeBoostBonus,
    bigLifeBoostBonus,
    hugeLifeBoostBonus,
    movementSpeedBonus,
    bigMovementSpeedBonus,
    hugeMovementSpeedBonus,
    damageSpeedBonus,
    bigDamageSpeedBonus,
    hugeDamageSpeedBonus
  }

  [Header("Sprites for each bonus type")]
  public Sprite[] iconeTable;

  [Header("Prefabs for each rarity of loot")]
  public LootType[] commonLoot;
  public LootType[] rareLoot;
  public LootType[] legendaryLoot;

  [Header("Chance for each rarity")]
  public int commonChance = 70;
  public int rareChance = 25;
  public int legendaryChance = 5;


  private int lootIndexInStock;
  private GameObject lootTypeIcon;

  // Use this for initialization
  void Start() {
    // Au start on determine ce qui sera le type de bonus.
    Destroy(this.gameObject, secondesBeforeDespawn);
    lootIndexInStock = GetRandomLoot();
    lootTypeIcon = transform.Find("Icon").gameObject;
    SetLootIcon();
  }

  public void SetLootIcon() {
    lootTypeIcon.GetComponent<SpriteRenderer>().sprite = iconeTable[lootIndexInStock];
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    // On détruit la caisse quand le joueur passe dessus.
    if (collision.gameObject.GetComponent<PlayerControl>()) {
      PlayerControl playerControl = collision.gameObject.GetComponent<PlayerControl>();
      Destroy(gameObject);
    }
  }

  private LootType GetRandomLoot() {
    int rarity = Random.Range(0, 100);
    if (rarity > 0 && rarity <= commonChance) { // COMMON
      int lootIndex = Random.Range(0, commonLoot.Length);
      if (commonLoot[lootIndex] != null) {
        return commonLoot[lootIndex];
      } else {
        throw new System.Exception("No loot in this commonLoots slot");
      }
    } else if (rarity > commonChance && rarity <= rareChance) { // RARE
      int lootIndex = Random.Range(0, rareLoot.Length);
      if (rareLoot[lootIndex] != null) {
        return rareLoot[lootIndex];
      } else {
        throw new System.Exception("No loot in this rareLoots slot");
      }
    } else { // LEGENDARY !
      int lootIndex = Random.Range(0, legendaryLoot.Length);
      if (legendaryLoot[lootIndex] != null) {
        return legendaryLoot[lootIndex];
      } else {
        throw new System.Exception("No loot in this legendaryLoots slot");
      }
    }
  }
}
