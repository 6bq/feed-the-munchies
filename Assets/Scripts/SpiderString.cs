using UnityEngine;
using System.Collections;

public class SpiderString : MonoBehaviour {

    public GameObject spiderPrefab;
    private GameObject spider;

    public int index;

    private float speed;

	void Start () {
        //Voeg een spin toe aan het laatste stukje touw
        spider = Instantiate(spiderPrefab) as GameObject;
        spider.transform.position = transform.GetChild(transform.childCount - 1).transform.position;
        spider.transform.parent = transform.GetChild(transform.childCount - 1);

        speed = Random.Range(0.1F,0.7F);
	}

    void Update() {
        if (transform.position.y > 5.25F)
            transform.position += Vector3.down * speed * Time.deltaTime;
    }

    public void Cut() {
        GameManager.instance.FreeSlot(index);

        Audio.Play(Sounds.Scissors);

        //Laat de spin los
        spider.GetComponent<Spider>().Drop();
        spider.transform.parent = null;

        //Vernietig dit stukje touw
        Destroy(gameObject);
    }
	
}
