using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLHandAnim : MonoBehaviour
{
    public Animator leftHandAnimator;

    private bool gripping = false;
    private bool holding = false;
    private bool objectIsOnWay = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            gripping = true;
            //Debug.Log("gripping");
        }
        else {
            gripping = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            holding = true;
            //Debug.Log("holding");
        }
        else {
            holding = false;
            /*
            if (!leftHandAnimator.GetBool("grabStartPlays")) {
                leftHandAnimator.SetBool("grabEndPlays", true);
            }
            */
        }
        if (objectIsOnWay) {
            //Debug.Log("gripped");
            leftHandAnimator.SetBool("telekinesis", gripping);
        }
        if (objectIsOnWay)
        {
            //Debug.Log("holdingForReal");
            leftHandAnimator.SetBool("grabbing", holding);
        }
    }
}
