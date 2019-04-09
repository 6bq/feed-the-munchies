using UnityEngine;

public class EventManager : MonoBehaviour {

    public static void NotifySpiderDrop(Spider spidy) {

        Munchie[] munchies = FindObjectsOfType<Munchie>();

        Munchie closestMunchie = null;

        if (munchies.Length >= 1) closestMunchie = munchies[0];

        //Xpos of spider
        float spiderX = spidy.transform.position.x;

        //Get the closest munchie
        foreach (Munchie m in munchies) {
            if ( !m.hasTarget && Mathf.Abs(m.transform.position.x - spiderX) < Mathf.Abs(closestMunchie.transform.position.x - spiderX)) { //Closest munchie so far
                closestMunchie = m;
            }
        }

        if (closestMunchie && !closestMunchie.hasTarget) {

            if ((closestMunchie.transform.localScale.x == 1 && closestMunchie.transform.position.x <= spiderX) || //Munchies can only see ahead of them!
            (closestMunchie.transform.localScale.x == -1 && closestMunchie.transform.position.x > spiderX)) {
                closestMunchie.setTarget(spidy.transform);
                spidy.Claim(closestMunchie.transform.position.y );
            }
        }

    }

    public static void RemoveSpiders() {
        foreach (Spider s in FindObjectsOfType<Spider>())
            if (s.OnGround)
                s.Die();
    }


}
