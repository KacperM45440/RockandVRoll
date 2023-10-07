using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectsReferenceManager : MonoBehaviour
{
    public static ObjectsReferenceManager instance;

    public XRDirectInteractor directLeftRef;
    public XRDirectInteractor directRightRef;
    public HandPhysics physicsLeftRef;
    public HandPhysics physicsRightRef;
    public Transform physicsLeftHand;
    public Transform physicsRightHand;
    public Transform handParentLeft;
    public Transform handParentRight;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
}
