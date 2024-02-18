using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] float _torqueMinimum = .1f;
    [SerializeField] float _torqueMaximum = 2;
    [SerializeField] float _throwStrengthMinimum = 8;
    [SerializeField] float _throwStrengthMaximum = 12;
    [SerializeField] TextMeshProUGUI _textBox;
    Rigidbody _rb;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

    }
        
    public void RollTheDice()
    {
        if (_rb.angularVelocity.sqrMagnitude < 1)
        {
            _rb.AddForce(Vector3.up * Random.Range(_throwStrengthMinimum, _throwStrengthMaximum), ForceMode.Impulse);

            _rb.AddTorque(transform.forward * Random.Range(_torqueMinimum, _torqueMaximum)
                + transform.up * Random.Range(_torqueMinimum, _torqueMaximum)
                + transform.right * Random.Range(_torqueMinimum, _torqueMaximum));

            _textBox.text = "";

            StartCoroutine(WaitForStop());
        }
    }

    IEnumerator WaitForStop()
    {
        yield return new WaitForFixedUpdate();

        while (_rb.angularVelocity.sqrMagnitude > 1)
        {
            yield return null;
        }

        CheckRoll();
    }

    public void CheckRoll()
    {
        /* y 1==2 -1==5
         * x 1==1 -1==6
         * z 1==4 -1==3
         */

        float yDot, xDot, zDot;
        int rollValue = -1;

        yDot = Mathf.Round(Vector3.Dot(transform.up.normalized, Vector3.up));
        xDot = Mathf.Round(Vector3.Dot(transform.forward.normalized, Vector3.up));
        zDot = Mathf.Round(Vector3.Dot(transform.right.normalized, Vector3.up));

        switch(yDot)
        {
            case 1:
                rollValue = 2;
                break;
            case -1:
                rollValue = 5;
                break;
        }
        switch (xDot)
        {
            case 1:
                rollValue = 1;
                break;
            case -1:
                rollValue = 6;
                break;
        }
        switch (zDot)
        {
            case 1:
                rollValue = 4;
                break;
            case -1:
                rollValue = 3;
                break;
        }
        _textBox.text = rollValue.ToString();
    }

}
