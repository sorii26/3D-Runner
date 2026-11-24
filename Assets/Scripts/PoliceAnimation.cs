using UnityEngine;

public class PoliceAnimation : MonoBehaviour
{
    public Transform leftArm;
    public Transform rightArm;
    public Transform leftLeg;
    public Transform rightLeg;

    public float animationSpeed = 12f;
    public float swingAmount = 35f;

    void Update()
    {
        float swing = Mathf.Sin(Time.time
            * animationSpeed) * swingAmount;

        leftArm.localRotation =
            Quaternion.Euler(swing, 0, 0);

        rightArm.localRotation =
            Quaternion.Euler(-swing, 0, 0);

        leftLeg.localRotation =
            Quaternion.Euler(-swing, 0, 0);

        rightLeg.localRotation =
            Quaternion.Euler(swing, 0, 0);
    }
}