using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dice : MonoBehaviour, ISelectable
{
    [SerializeField] float _torqueMinimum = .1f;
    [SerializeField] float _torqueMaximum = 2;
    [SerializeField] float _throwStrengthMinimum = 8;
    [SerializeField] float _throwStrengthMaximum = 12;
    [SerializeField] TextMeshProUGUI _textBox;
    [SerializeField] float xRotationFrequency = 1.2f;
    [SerializeField] float yRotationFrequency = 1.5f;
    [SerializeField] float zRotationFrequency = 1.3f;
    [SerializeField] float xRotationAmplitude = 20;
    [SerializeField] float yRotationAmplitude = 20;
    [SerializeField] float zRotationAmplitude = 20;
    Rigidbody _rb;

    private bool isSelected = false;
    private float timeCounter = 0;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

    }

    void Update()
    {

        if (isSelected)
        {
            timeCounter += Time.deltaTime;

            // Use sine waves to determine rotation speed and direction
            float xRotation = Mathf.Sin(timeCounter * xRotationFrequency) * xRotationAmplitude; // These numbers can be adjusted
            float yRotation = Mathf.Sin(timeCounter * yRotationFrequency) * yRotationAmplitude; // to change the rotation behavior
            float zRotation = Mathf.Sin(timeCounter * zRotationFrequency) * zRotationAmplitude;

            transform.Rotate(new Vector3(xRotation, yRotation, zRotation) * Time.deltaTime);
        }
    }
    
        
    public void RollTheDice()
    {
        if (true)
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

    public void OnSelected()
    {
        isSelected = true;
        Debug.Log("Selected!!!");
    }

    public void OnDeselected()
    {
        isSelected = false;
        Debug.Log("Deselected!!!");
        RollTheDice();
    }

}
