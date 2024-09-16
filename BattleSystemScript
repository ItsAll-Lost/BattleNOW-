using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, NEXT}
public class BattleSystemScript : MonoBehaviour
{
    public GameObject[] unitPrefab;
    public GameObject[] enemyPrefabs;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    private UnitScript playerUnit;
    private UnitScript enemyUnit;

    public TMP_Text dialogueText;
    public TMP_Text roundText;


    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public GameObject playerPlat;
    public GameObject enemyPlat;

    public Button attackButton;
    public Button healButton;
    public Button dodgeButton;
    public Button swapButton;

    private int currentPlayerIndex = 0;  


    public BattleState state;

    private int round = 1;  
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());

        swapButton.onClick.AddListener(OnSwapPlayerUnit);
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(unitPrefab[0], playerBattleStation);
        playerUnit = playerGO.GetComponent<UnitScript>();

        SpawnRandomEnemy();

        dialogueText.text = "A " + enemyUnit.unitName + " Approaches...";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();

        if (unitPrefab.Length == 1)
        {
            swapButton.interactable = false;
        }
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHp(enemyUnit.currentHp);
        dialogueText.text = "The Attack Is Successful!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            enemyHUD.SetHp(enemyUnit.currentHp = 0);
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            SetButtonsInteractable(false);
            dialogueText.text = "You Deal " + playerUnit.damage + " Damage...";
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        TogglePlatforms();
        dialogueText.text = enemyUnit.unitName + " Attacks!";


        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHp(playerUnit.currentHp);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You Won The Battle!";
            roundText.text = "Conquered Rounds: " + round;


            StartCoroutine(WaitBeforeNextRound());
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You Were Defeated! You Fell On Round " + round + ".";
            SetButtonsInteractable(false);
        }
    }

    IEnumerator WaitBeforeNextRound()
    {
        yield return new WaitForSeconds(2f); 

        round++; 
        state = BattleState.NEXT; 
        NextTurn(); 
    }

    void NextTurn()
    {
        if (round == 5 || round == 10 || round == 15)
        {
            dialogueText.text = "Checkpoint Reached At Round " + round + "!";
        }

        if (state == BattleState.NEXT)
        {
            SpawnRandomEnemy();
            dialogueText.text = "Round " + round + " ,A " + enemyUnit.unitName + " Approaches...";

            enemyHUD.SetHUD(enemyUnit);

            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose An Action:";
        SetButtonsInteractable(true);
        TogglePlatforms();

        if (unitPrefab.Length == 1)
        {
            swapButton.interactable = false;
        }
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(20);

        state = BattleState.ENEMYTURN;

        playerHUD.SetHp(playerUnit.currentHp);
        dialogueText.text = "You Healed For 20.";

        yield return new WaitForSeconds(2f);

        SetButtonsInteractable(false);
        StartCoroutine(EnemyTurn());
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
        SetButtonsInteractable(false);
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
        SetButtonsInteractable(false);
    }

  
    public void OnDodgeButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerDodge());
        SetButtonsInteractable(false);
    }

    IEnumerator PlayerDodge()
    {
        state = BattleState.ENEMYTURN;

        bool dodgeSuccessful = AttemptDodge();

        yield return new WaitForSeconds(1f);

        if (dodgeSuccessful)
        {
            dialogueText.text = "The " + enemyUnit.unitName + "'s Attack Missed!";
            playerUnit.Heal(10);

            yield return new WaitForSeconds(1f);


            playerHUD.SetHp(playerUnit.currentHp);
            dialogueText.text = "You Recovered For 10.";

            yield return new WaitForSeconds(2f);


            state = BattleState.PLAYERTURN;
            PlayerTurn();  
        }
        else
        {
            dialogueText.text = "Dodge Failed!";
            yield return new WaitForSeconds(1f);
            StartCoroutine(EnemyTurn());
        }
    }

    bool AttemptDodge()
    {
        int maxRoll = 10;
        int roll = Random.Range(1, maxRoll + 1);

        if (playerUnit.speed >= roll)  
        {
            return true; 
        }

        return false;  
    }

    public void OnSwapPlayerUnit()
    {
        if (unitPrefab.Length <= 1)
        {
            swapButton.interactable = false;
            return;
        }

        int oldPlayerIndex = currentPlayerIndex;

        List<GameObject> remainingUnits = new List<GameObject>(unitPrefab);
        remainingUnits.RemoveAt(oldPlayerIndex);  

        if (remainingUnits.Count == 0)
        {
            swapButton.interactable = false;
            return;
        }

        int newUnitIndex = Random.Range(0, remainingUnits.Count);

        Destroy(playerUnit.gameObject);

        GameObject playerGO = Instantiate(remainingUnits[newUnitIndex], playerBattleStation);
        playerUnit = playerGO.GetComponent<UnitScript>();

        playerHUD.SetHUD(playerUnit);

        unitPrefab = remainingUnits.ToArray();

        currentPlayerIndex = Mathf.Min(newUnitIndex, unitPrefab.Length - 1);

        if (unitPrefab.Length == 1)
        {
            swapButton.interactable = false;
        }

        dialogueText.text = playerUnit.unitName + " Is Your New Unit!";
    }

    void SetButtonsInteractable(bool interactable)
    {
        attackButton.interactable = interactable;
        healButton.interactable = interactable;
        dodgeButton.interactable = interactable;  
    }

    void SpawnRandomEnemy()
    {
        if (enemyUnit != null)
        {
            Destroy(enemyUnit.gameObject);
        }

        int randomIndex;

        if (round <= 5)
        {
            randomIndex = Random.Range(0, 4);
        }
        else if (round <= 14)
        {
            randomIndex = Random.Range(0, 7);
        }
        else
        {
            randomIndex = Random.Range(0, enemyPrefabs.Length);
        }

        GameObject enemyGO = Instantiate(enemyPrefabs[randomIndex], enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<UnitScript>();
    }

    void TogglePlatforms()
    {
        if (state == BattleState.PLAYERTURN)
        {
            playerPlat.SetActive(true);
            enemyPlat.SetActive(false);
        }
        else if (state == BattleState.ENEMYTURN)
        {
            playerPlat.SetActive(false);
            enemyPlat.SetActive(true);
        }
    }
}
