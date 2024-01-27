using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    [SerializeField] private DroppedItem itemPrefab;
    [SerializeField] private List<GameStages> gameStages;
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;
    [SerializeField] private Transform itemParent;

    private int stageIndex => KeyManager.Instance.currentStage < gameStages.Count
        ? KeyManager.Instance.currentStage
        : gameStages.Count - 1;

    private IEnumerator Start()
    {
        while (true)
        {
            var currentStage = gameStages[stageIndex];
            yield return new WaitForSeconds(currentStage.spawnRate);
            for (int i = 0; i < currentStage.numberOfKeys; i++)
            {
                var randomPos = Vector3.Lerp(leftLimit.position, rightLimit.position, Random.value);
                var newItem = Instantiate(itemPrefab, randomPos, Quaternion.identity, itemParent);
                newItem.SetSpeedMult(currentStage.movementMult);
            }
        }
    }
}