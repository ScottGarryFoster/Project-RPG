using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voruko;

public class BasicTree : MonoBehaviour
{
    [Header("Links")]
    [Tooltip("Drag the player in here must have inventory attributes")]
    public GameObject player;
    [Tooltip("Drag the tree when grown in here")]
    public GameObject Tree;
    [Tooltip("Drag the stump when grown in here")]
    public GameObject Stump;

    [Header("Meta Data")]
    [Tooltip("How quickly should we heal (in seconds)")]
    public int RegrowthTime;

    private MovementController playerMovementCont;
    private Attributes_Inventory playerInventory;

    private bool CurrentState = false;//False means is full tree, true means is stump
    private int Health = 5;

    private float RegrowTimer = 5;
    // Start is called before the first frame update
    void Start()
    {
        playerMovementCont = player.GetComponent<MovementController>();
        playerInventory = player.GetComponent<Attributes_Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLookOfTree();
        UpdateRegrowTree();
    }

    void UpdateLookOfTree()
    {
        if (CurrentState && Tree.activeSelf)
        {
            Tree.SetActive(false);
            Stump.SetActive(true);
        }
        else if (!CurrentState && Stump.activeSelf)
        {
            Tree.SetActive(true);
            Stump.SetActive(false);
        }
    }

    void UpdateRegrowTree()
    {
        if (Health >= 5) return;//Already healed
        RegrowTimer += Time.deltaTime;
        if (RegrowTimer < RegrowthTime) return;
        RegrowTimer -= RegrowthTime;
        Health += 1;
        Debug.Log("Current health: " + Health);
        if (Health == 5) CurrentState = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        if (!AreLookingAtTree()) return;
        if (Input.GetButtonUp("Fire1")) HitTree();

    }

    void HitTree()
    {
        if (CurrentState) return;//If we're a stump
        if (playerInventory == null) return; //We don't have a link to the player Inventory
        GameObject weapon = playerMovementCont.getWeapon();
        if (weapon == null) return;
        Attributes_Weapon weaponAtt = weapon.GetComponent<Attributes_Weapon>();
        if (weaponAtt == null) return;
        Health -= 1;
        playerInventory.giveOakWood(Random.Range(1, 3));
        Debug.Log("Yh");
        if (Health == 0) CurrentState = true;
    }

    bool AreLookingAtTree()
    {
        Vector3 dirFromAtoB = (Tree.transform.position - player.transform.position).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, player.transform.forward);

        if (dotProd > 0.9)
            return true;
        return false;
    }
}
