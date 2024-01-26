using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class KeyManager : MonoBehaviour
{
    [Header("Key spawning")]
    [SerializeField] private KeyController keyPrefab;
    [SerializeField] private List<KeyController> keys;
    [SerializeField] private Transform keyParent;

    [Header("Game stages")]
    [SerializeField] private List<GameStages> gameStages;

    [Header("Key detection")]
    [SerializeField] private Transform checkPoint;

    [SerializeField] private float perfectKeyDistance;
    [SerializeField] private int perfectScore;

    [SerializeField] private float allowKeyPress;
    [SerializeField] private int passableScore;
    [SerializeField] private int punishmentScore;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image satisfactionBar;
    [SerializeField] private int scoreMapper = 300;
    [SerializeField] private Color[] barColors;

    private float lastSpawnTime = 0;
    private int currentStage;
    private KeyController currentKey;

    public int Score { get; private set; } = 100;

    private void Awake()
    {
        keys = new List<KeyController>();
    }

    private void Start()
    {
        UpdateScore(0);
    }

    private void Update()
    {
        var gameStage = gameStages[currentStage];
        
        if (lastSpawnTime +  gameStage.spawnRate < Time.time)
        {
            SpawnKey();
        }

        foreach (var key in keys)
        {
            key.Move();
        }

        if (keys.Count > 0)
        {
            var leadingKey = keys[0];

            CheckKey(leadingKey);
        }

        if (Score >= gameStage.transitionScore && currentStage + 1 < gameStages.Count)
        {
            currentStage++;
            foreach (var key in keys)
            {
                key.MovementFactor = gameStages[currentStage].movementMult;
            }
        }
    }

    private void CheckKey(KeyController leadingKey)
    {
        var leadingDistance = Vector3.Distance(leadingKey.selfRef.position, checkPoint.position);

        if (leadingDistance <= allowKeyPress)
        {
            currentKey = leadingKey;
            leadingKey.CanBePressed();

            if (leadingDistance < perfectKeyDistance)
            {
                leadingKey.Perfect();
            }
            else if (leadingKey.selfRef.position.x < checkPoint.position.x)
            {
                leadingKey.TooLate();
                UpdateScore(punishmentScore);
                RemoveKey(leadingKey);
            }
        }
    }

    private void RemoveKey(KeyController leadingKey)
    {
        currentKey = null;
        Destroy(leadingKey.gameObject, 0.1f);
        keys.RemoveAt(0);
    }

    private void SpawnKey()
    {
        var newKey = Instantiate(keyPrefab, keyParent);
        newKey.SetValue((KeyValue)Random.Range(0, gameStages[currentStage].numberOfKeys));
        newKey.MovementFactor = gameStages[currentStage].movementMult;
        keys.Add(newKey);
        lastSpawnTime = Time.time;
    }


    public void UpKeyPressed(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (currentKey != null)
        {
            CheckKeyValue(KeyValue.Up);
        }
    }

    public void DownKeyPressed(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (currentKey != null)
        {
            CheckKeyValue(KeyValue.Down);
        }
    }

    public void LeftKeyPressed(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (currentKey != null)
        {
            CheckKeyValue(KeyValue.Left);
        }
    }

    public void RightKeyPressed(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (currentKey != null)
            {
                CheckKeyValue(KeyValue.Right);
            }
        }
    }

    private void CheckKeyValue(KeyValue val)
    {
        if (currentKey.value != val) return;

        switch (currentKey.State)
        {
            case KeyState.Waiting:
                break;
            case KeyState.Passable:
                UpdateScore(passableScore);
                break;
            case KeyState.Perfect:
                UpdateScore(perfectScore);
                break;
            case KeyState.Late:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        RemoveKey(currentKey);
    }


    private void UpdateScore(int diff)
    {
        Score += diff;
        scoreText.text = Score + "";
        var fraction = 1f * Score / scoreMapper;
        satisfactionBar.fillAmount = fraction;
        
        if(fraction <= 0.3f)
        {
            satisfactionBar.color = barColors[0];
        }
        else if (fraction <= 0.6f)
        {
            satisfactionBar.color = barColors[1];
        }
        else if (fraction <= 0.8f)
        {
            satisfactionBar.color = barColors[2];
        }
        else 
        {
            satisfactionBar.color = barColors[3];
        }
    }
}

[Serializable]
public class GameStages
{
    public int numberOfKeys;
    public float spawnRate;
    public float movementMult = 1;
    public int transitionScore;
}