using UnityEngine;
using UnityEngine.UI;

public class Munchie : MonoBehaviour {

    [SerializeField] protected Vector3 direction = new Vector3(1,0,0);
    [SerializeField] protected float moveSpeed = 3;
    public Transform armature;

    public Text debugPercentage;
    public Renderer meshRenderer;
    private Material mat;

    //Honger en kleuren
    public static readonly short hungerPerSecond = 7;
    public static readonly float NORMAL_HUE = 0.35F;
    public static readonly short MAX_HUE_HUNGRY = 100, MAX_HUNGRY = 125;
    protected float hungry = 0;

    public Transform spiderTarget = null;

    public GameObject bloodParticles;
    
    private Animator animator;

    public Image lifeImage;

    void Start() {
        //Verkrijg componenten
        mat = meshRenderer.material;
        animator = GetComponent<Animator>();

        //Kies een willekeurige richting
        int randomint = Random.Range(0,2);
        if (randomint == 0) {
            transform.localScale = new Vector3(-1,1,1);
        }

        //Krijg een random positie op de X axis
        transform.position = new Vector3(Random.Range(-7,7),transform.position.y,transform.position.z);

    }

    void Update() {

        if (spiderTarget != null) { //Als we een target hebben
            //Bewegen we er naartoe
            if (transform.position.x < spiderTarget.position.x - 0.1F) { //Links van ons
                transform.localScale = new Vector3(1,1,1);
            }
            else if (transform.position.x > spiderTarget.position.x + 0.1F) {
                transform.localScale = new Vector3(-1,1,1);
            }
            else { //Onder de spin
                direction = Vector3.zero;
                animator.SetBool("wait",true);

                if (spiderTarget.transform.position.y <= transform.position.y + 0.5F) { //Eet de spin
                    EatSpider();
                    animator.SetBool("wait",false);
                }
            }

        }

        transform.Translate(direction * moveSpeed * Time.deltaTime);

        if (hungry < MAX_HUNGRY) {
            hungry += hungerPerSecond * Time.deltaTime;

            if (hungry <= MAX_HUE_HUNGRY) {
                float hue = NORMAL_HUE - (NORMAL_HUE / MAX_HUE_HUNGRY * hungry);
                mat.color = Color.HSVToRGB(hue,1,1);
                lifeImage.color = Color.HSVToRGB(hue,1,1);
            }

            /*DEBUG*/
            debugPercentage.text = Mathf.RoundToInt(hungry).ToString() + "%";
        }

        // Screen Wrap
        if (transform.position.x < -7)
            transform.position = new Vector3(7,transform.position.y,transform.position.z);
        else if (transform.position.x > 7)
            transform.position = new Vector3(-7,transform.position.y,transform.position.z);

       /*DEBUG*/if (Input.GetKeyDown(KeyCode.Space)) debugPercentage.enabled = !debugPercentage.enabled;

    }

    public bool hasTarget {
        get { return (spiderTarget != null); }
    }

    public void setTarget(Transform t) {
        spiderTarget = t;
    }

    private void EatSpider() {
        if (spiderTarget.GetComponent<Spider>().OnGround) {
            //Als de spin op de grond is, mogen we hem niet eten
            print("Spin is op de grond");
            Die();
            spiderTarget.GetComponent<Spider>().Die();
        }
        else { //Eet de spin
            hungry = 0;
            //Geluidseffect
            Audio.Play(Sounds.Eat);
            //Particles
            Destroy(Instantiate(bloodParticles,spiderTarget.transform.position,Quaternion.identity,transform),1);
            //Vernietig de spin
            spiderTarget.GetComponent<Spider>().Die();
            spiderTarget.parent = transform;
            spiderTarget.transform.localPosition = Vector3.zero;
            spiderTarget = null;
            direction = Vector3.right;

            //Draai om
            transform.localScale = new Vector3(-transform.localScale.x,1,1);
        }

    }

    public void Die() {
        print(name + "IK ben dood");
        lifeImage.enabled = false;
        Destroy(gameObject);
    }

}