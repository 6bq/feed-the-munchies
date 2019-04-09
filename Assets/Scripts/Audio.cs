using UnityEngine;

public class Audio : MonoBehaviour {

    public static Audio instance;
    public AudioClip[] clips;

    public void Awake() {
        instance = this;
    }

    public static void Play(Sounds sound) {
        GameObject slave = new GameObject();
        slave.AddComponent<AudioSource>().clip = instance.clips[(int)sound];
        slave.GetComponent<AudioSource>().Play();
        Destroy(slave,instance.clips[(int)sound].length);
    }

}

public enum Sounds {
    Eat,
    Scissors
}
