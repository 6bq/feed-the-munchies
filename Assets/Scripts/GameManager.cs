using UnityEngine;

using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject spiderStringPrefab;
    public static bool spawnSpiders = true;

    public static GameManager instance;

    private bool[] spiderStringSlots;
    private Vector3[] spiderStringPositions;

    public void Start() {
        instance = this;

        spiderStringPositions = new Vector3[] {
            new Vector3(-3.5F,11,0),
            new Vector3(-2.5F,11,0),
            new Vector3(-1.5F,11,0),
            new Vector3(0,11,0),
            new Vector3(1.5F,11,0),
            new Vector3(2.5F,11,0),
            new Vector3(3.5F,11,0)
        };

        spiderStringSlots = new bool[spiderStringPositions.Length];

        StartCoroutine(SpawnSpiderRoutine());
    }

    private IEnumerator SpawnSpiderRoutine() {
        while (spawnSpiders) {

            int randomint = Random.Range(0,spiderStringSlots.Length);

            if (!spiderStringSlots[randomint]) {
                spiderStringPrefab.GetComponent<SpiderString>().index = randomint;
                Instantiate(spiderStringPrefab,spiderStringPositions[randomint],Quaternion.identity);
                spiderStringSlots[randomint] = true;
            }

            yield return new WaitForSeconds(Random.Range(1,3));
        }
    }

    public void FreeSlot(int index) {
        spiderStringSlots[index] = false;
    }

}
