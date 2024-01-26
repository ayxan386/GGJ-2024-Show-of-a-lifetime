using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyManager : MonoBehaviour
{
    [SerializeField] private float spawnRate;
    [SerializeField] private KeyController keyPrefab;
    [SerializeField] private List<KeyController> keys;
    [SerializeField] private Transform keyParent;

    [Header("Key detection")]
    [SerializeField] private Transform checkPoint;

    [SerializeField] private float perfectKeyDistance;
    [SerializeField] private int perfectScore;

    [SerializeField] private float allowKeyPress;
    [SerializeField] private int passableScore;
    [SerializeField] private int punishmentScore;

    private float lastSpawnTime = 0;
    private KeyController currentKey;

    public int Score { get; private set; }

    private void Awake()
    {
        keys = new List<KeyController>();
    }

    void Update()
    {
        if (lastSpawnTime + spawnRate < Time.time)
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
                Score += passableScore;
                break;
            case KeyState.Perfect:
                Score += perfectScore;
                break;
            case KeyState.Late:
                Score += punishmentScore;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        RemoveKey(currentKey);
    }
}