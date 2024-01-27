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
    [Header("Scoring")]
    [SerializeField] private int passableScore;
    [SerializeField] private float punishmentScore;
    [SerializeField] private float itemCollisionScore;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image satisfactionBar;
    [SerializeField] private Color[] barColors;
    [SerializeField] private float difficultyFactor = 1f;
    [SerializeField] private float lossingFraction;

    private float lastSpawnTime = 0;
    public int currentStage { get; private set; }
    private KeyController currentKey;

    public int Score { get; private set; } = 100;
    public int RoundMaxScore { get; private set; } = 100;

    public static KeyManager Instance { get; private set; }
    
    public static event Action<int> OnRoundEnd; 

    private void Awake()
    {
        keys = new List<KeyController>();
        Instance = this;
    }

    private void Start()
    {
        UpdateScore(0);
    }

    private void Update()
    {
        var gameStage = gameStages[currentStage];

        if (lastSpawnTime + gameStage.spawnRate / difficultyFactor < Time.time)
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
                key.MovementFactor = gameStages[currentStage].movementMult * difficultyFactor;
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
                UpdateScore((int)Mathf.Floor(Score * punishmentScore));
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
        newKey.MovementFactor = gameStages[currentStage].movementMult * difficultyFactor;
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

    public void ItemDropped()
    {
        UpdateScore((int)(Score * itemCollisionScore));
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
        RoundMaxScore = Mathf.Max(Score, RoundMaxScore);
        scoreText.text = "Max score: " + RoundMaxScore;
        var fraction = 1f * Score / RoundMaxScore;
        satisfactionBar.fillAmount = fraction;

        if (fraction <= 0.3f)
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

        if (fraction <= lossingFraction)
        {
            OnRoundEnd?.Invoke(RoundMaxScore);
        }

        if (Score > 300)
        {
            difficultyFactor = Mathf.Pow(1.01f, (Score - 300) / 10);
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