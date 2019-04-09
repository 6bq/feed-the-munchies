using UnityEngine;

public class CameraClick : MonoBehaviour {
	
	void Update () {
        //Click

        if (Input.GetMouseButton(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray,out hit,1000)) {
                
                if (hit.transform.CompareTag("String") && hit.transform.GetComponent<SpiderString>()) { //String geraakt, knip het lijntje door
                    hit.transform.GetComponent<SpiderString>().Cut();
                }

            }

        }

    }
}
