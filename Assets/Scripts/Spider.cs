using UnityEngine;
using System.Collections;

public class Spider : MonoBehaviour {

    public static readonly float fallSpeed = 2;
    private bool dropped = false;
    private bool claimed = false;
    private bool onGround = false, dead =false;

    public static float SPIDER_LIFETIME = 5;

    private float bottomY = 1.285F;

    private Animator animator;

    public void Claim(float y) {
        claimed = true;
        bottomY = y;
    }

    public bool IsClaimed {
        get { return claimed; }
    }

    public bool OnGround {
        get { return onGround; }
    }

    void Start() {
        animator = GetComponent<Animator>();
    }
	
	void Update () {
        if (dead)
            return;

        if (dropped && transform.position.y > bottomY) {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            if (!IsClaimed)
                EventManager.NotifySpiderDrop(this);
        }else if (!onGround && transform.position.y < bottomY){
            //We zijn op de grond beland
            onGround = true;
            animator.SetBool("dropped",false);
            animator.SetBool("onground",true);
        }

        if (onGround || !dropped) {
            //Kill munchies die de dichtbij komen
            foreach(Munchie m in FindObjectsOfType<Munchie>()) {
                if (Vector3.Distance(transform.position,m.transform.position) < 0.5F) {
                    m.Die();
                    Die(); // < Ga zelf ook dood
                }
            }
        }

    }

    public void Die() {
        if (isDropped)
            StartCoroutine(DieRoutine());
        else //Neem het touw mee
            Destroy(transform.parent.parent.gameObject);

        dead = true;
    }

    public void Drop() {
        dropped = true;
        StartCoroutine(SpiderLifetimeRoutine());
        animator.SetBool("dropped",true);
    }

    public bool isDropped {
        get { return dropped; }
    }

    IEnumerator DieRoutine() {
        animator.SetTrigger("die");
        yield return new WaitForSeconds(0.1F);
        Destroy(gameObject);
    }

    IEnumerator SpiderLifetimeRoutine() {
        yield return new WaitForSeconds(SPIDER_LIFETIME);
        Die();
    }

}
