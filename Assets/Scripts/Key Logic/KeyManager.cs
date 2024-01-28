using System;
using System.Collections;
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
    [SerializeField] private Sprite[] barSpites;
    [SerializeField] private Image[] emojis;
    [SerializeField] private Sprite[] blankEmoji;
    [SerializeField] private float difficultyFactor = 1f;
    [SerializeField] private float lossingFraction;

    [Header("SFX")]
    [SerializeField] private AudioClip[] keySounds;
    [SerializeField] private CinematicPlayer endingScenePlayer;
    
    private float lastSpawnTime = 0;
    public int currentStage { get; private set; }
    public bool RoundEnded { get; set; }
    private KeyController currentKey;

    public int Score { get; private set; } = 100;
    public int RoundMaxScore { get; private set; } = 100;
    private int currentCombo = 0;
    private int soundIndex;

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
        if(RoundEnded) return;
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
                PlayKeySound();
                UpdateScore(passableScore);
                break;
            case KeyState.Perfect:
                PlayKeySound();
                UpdateScore(perfectScore);
                break;
            case KeyState.Late:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        RemoveKey(currentKey);
    }

    private void PlayKeySound()
    {
        AudioSource.PlayClipAtPoint(keySounds[soundIndex], Vector3.zero);
        soundIndex = 1 - soundIndex;
    }


    private void UpdateScore(int diff)
    {
        if(RoundEnded) return;
        
        Score += diff;
        RoundMaxScore = Mathf.Max(Score, RoundMaxScore);
        currentCombo = diff != 0 && Score == RoundMaxScore ? currentCombo + 1 : 0;
        scoreText.text = "Max score: " + RoundMaxScore;
        var fraction = 1f * Score / RoundMaxScore - lossingFraction;
        satisfactionBar.fillAmount = fraction + 0.05f * currentCombo;

        UpdateEmojis(satisfactionBar.fillAmount);
        

        if (fraction <= 0)
        {
            RoundEnded = true;
            StartCoroutine(EndRound());
        }

        if (Score > 300)
        {
            difficultyFactor = Mathf.Pow(1.01f, (Score - 300) / 10);
        }
    }

    private IEnumerator EndRound()
    {
        yield return endingScenePlayer.PlayVideo();
        // yield return new WaitForSeconds(1.5f);
        OnRoundEnd?.Invoke(RoundMaxScore);
    }

    private void UpdateEmojis(float fraction)
    {
        var inc = 1f / (barSpites.Length - 1);
        var currentOne = -1;
        for (int i = 0; i < emojis.Length; i++)
        {
            emojis[i].sprite = blankEmoji[i];

            if (inc * i <= fraction && fraction <= (i + 1) * inc)
            {
                currentOne = i;
            }
        }

        if(currentOne != -1)
            emojis[currentOne].sprite = barSpites[currentOne];

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