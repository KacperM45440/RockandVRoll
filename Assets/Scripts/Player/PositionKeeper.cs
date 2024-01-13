using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PositionKeeper : MonoBehaviour
{
    public InputActionReference stoppedRotating = null;
    public Transform tempTransform;
    public Transform cameraPos;
    public GameObject cameraOffset;
    public Rigidbody rigidbodyRef;

    private void Awake()
    {
        stoppedRotating.action.performed += Rotate;
    }
    private void OnEnable()
    {
        stoppedRotating.action.Enable();
    }
    private void OnDisable()
    {
        stoppedRotating.action.Disable();
    }
    // Kiedy gracz obraca kamere, zmienia swoje wzgledne polozenie w swiecie ktore nie pokrywa sie z jego colliderem
    // Jest przez to w stanie o wiele latwiej wchodzic w obiekty (np: stoly) oraz clipowac przez sciane
    // Aby zredukowac czestotliwosc wystepowania tego problemu, przesuwamy collider gracza z powrotem do obecnej pozycji glowy gdy konczy obrot
    // Za zakonczenie obrotu uznajemy sytuacje, w ktorej w krotkim odstepnie czasu nie zauwazamy zmiany w rotacji rigidbody; zbieranie inputu prosto z galki nie dzialalo zbyt dobrze
    public void Rotate(InputAction.CallbackContext obj)
    {
        if (obj.ReadValue<Vector2>().y != 0)
        {
            return;
        }

        Vector3 targetPos = cameraPos.transform.position;
        foreach (Transform child in transform) 
        {
            child.parent = tempTransform;
        }

        transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        foreach (Transform child in tempTransform)
        {
            child.parent = transform;
        }
        
        cameraPos.rotation = Quaternion.Euler(cameraPos.rotation.x, cameraPos.rotation.y + GetComponent<ActionBasedSnapTurnProvider>().turnAmount, cameraPos.rotation.z);
    }
}
