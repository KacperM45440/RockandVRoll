using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PositionKeeper : MonoBehaviour
{
    public InputActionReference stoppedRotating = null;
    public Transform cameraPos;
    public Rigidbody rigidbodyRef;
    private Vector2 thumbAxis;

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
    public void Rotate(InputAction.CallbackContext obj)
    {
        thumbAxis.x = obj.ReadValue<float>();
        StartCoroutine(CheckRotation());
    }

    // Kiedy gracz obraca kamere, zmienia swoje wzgledne polozenie w swiecie ktore nie pokrywa sie z jego colliderem
    // Jest przez to w stanie o wiele latwiej wchodzic w obiekty (np: stoly) oraz clipowac przez sciane
    // Aby zredukowac czestotliwosc wystepowania tego problemu, przesuwamy collider gracza z powrotem do obecnej pozycji glowy gdy konczy obrot
    // Za zakonczenie obrotu uznajemy sytuacje, w ktorej w krotkim odstepnie czasu nie zauwazamy zmiany w rotacji rigidbody; zbieranie inputu prosto z galki nie dzialalo zbyt dobrze
    private IEnumerator CheckRotation()
    {
        float x = rigidbodyRef.transform.rotation.y;
        yield return new WaitForSeconds(0.1f);
        float y = rigidbodyRef.transform.rotation.y;

        bool grounded = transform.GetChild(0).GetComponent<GroundDetector>().isGrounded;

        if (x.Equals(y) && grounded)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(cameraPos.localPosition.x, transform.localPosition.y, cameraPos.localPosition.z), 1);
        }
    }
}
