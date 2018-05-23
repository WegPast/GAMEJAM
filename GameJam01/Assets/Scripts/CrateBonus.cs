﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CrateBonus : NetworkBehaviour
{

  public enum LootRarity
  {
    common, rare, legendary
  }

  [System.Serializable]
  public class Loot
  {
    [Tooltip("Name : The name of the loot")]
    public string lootName = "New Loot";
    [Tooltip("Icon : The loot's sprite, if has to have any...")]
    public Sprite lootIcon = null;
    //public LootType lootType = null;
    [Tooltip("Duration : 0f = immediate use, xf = last x seconds")]
    public float lootDuration = 0f;
    [Tooltip("Credits : Add X Credits to the player's Credits")]
    public int lootCreditValue = 0;
    [Tooltip("Damage Increase : Multiply by X points the current damage of each player's ship projectiles")]
    public float lootDamageIncreasedValue = 0;
    [Tooltip("Max Life Bonus : Add X points to the max life of the player's ship")]
    public int lootMaxLifeBonusValue = 0;
    [Tooltip("Repair : Add X points of the player's ship current life (not max life)")]
    public int lootRepairValue = 0;
    [Tooltip("Chance : the percentage of chance the loot appear")]
    public LootRarity lootRarity = LootRarity.common;
  }

  public Loot[] lootTable;

  [
      Header("Time before despawn"),
      Tooltip("Nombre de secondes avant la disparition de la crate"),
      Range(0, 10)
  ]
  public int secondesBeforeDespawn;

  [Header("Chance for each rarity")]
  public int commonChance = 70;
  public int rareChance = 25;
  public int legendaryChance = 5;


  private List<Loot> commonLoot;
  private List<Loot> rareLoot;
  private List<Loot> legendaryLoot;

  private Loot lootInStock;
  private GameObject lootTypeIcon;


  void Start() {
    commonLoot = GetAllLootByRarity(LootRarity.common);
    rareLoot = GetAllLootByRarity(LootRarity.rare);
    legendaryLoot = GetAllLootByRarity(LootRarity.legendary);

    Destroy(this.gameObject, secondesBeforeDespawn);

    // Au start on determine ce qui sera le type de bonus.
    lootInStock = GetRandomLoot();
    lootTypeIcon = transform.Find("Icon").gameObject;
    SetLootIcon();
  }

  public List<Loot> GetAllLootByRarity(LootRarity rarity) {
    List<Loot> resultLootList = new List<Loot>();
    foreach (var loot in lootTable) {
      if(loot.lootRarity == rarity) {
        resultLootList.Add(loot);
      }
    }
    return resultLootList;
  }

  public void SetLootIcon() {
    if (lootInStock.lootIcon != null) {
      lootTypeIcon.GetComponent<SpriteRenderer>().sprite = lootInStock.lootIcon;
    }
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    // On détruit la caisse quand le joueur passe dessus.
    if (collision.gameObject.GetComponent<PlayerControl>()) {
      ApplyLootBehaviour(collision.gameObject.GetComponent<PlayerControl>());
    }
  }

  private void ApplyLootBehaviour(PlayerControl playerControlCollided) {

    PlayerControl playerControl = playerControlCollided;

    // the bonus repair the player's ship
    if (lootInStock.lootRepairValue != 0) {
      playerControl.GetComponent<LifeManager>().Heal(lootInStock.lootRepairValue);
    }


    // When all what the bonus was meant to do is done, destroy it :
    Destroy(gameObject);
  }

  private Loot GetRandomLoot() {
    int rarity = Random.Range(0, 100);
    if (rarity > 0 && rarity <= commonChance) { // COMMON
      int lootIndex = Random.Range(0, commonLoot.Count);
      if (commonLoot[lootIndex] != null) {
        return commonLoot[lootIndex];
      } else {
        throw new System.Exception("No loot in this commonLoots slot");
      }
    } else if (rarity > commonChance && rarity <= rareChance) { // RARE
      int lootIndex = Random.Range(0, rareLoot.Count);
      if (rareLoot[lootIndex] != null) {
        return rareLoot[lootIndex];
      } else {
        throw new System.Exception("No loot in this rareLoots slot");
      }
    } else { // LEGENDARY !
      int lootIndex = Random.Range(0, legendaryLoot.Count);
      if (legendaryLoot[lootIndex] != null) {
        return legendaryLoot[lootIndex];
      } else {
        throw new System.Exception("No loot in this legendaryLoots slot");
      }
    }
  }
}
