using UnityEngine;

public class RobberAnimation : MonoBehaviour
{
    public Transform leftArm;
    public Transform rightArm;
    public Transform leftLeg;
    public Transform rightLeg;
    public Transform moneyBag;

    public float animationSpeed = 12f;
    public float swingAmount = 35f;
    public float bagBounce = 0.06f;

    private Vector3 bagStartPosition;
    private Quaternion bagStartRotation;

    private Quaternion leftArmStart;
    private Quaternion rightArmStart;
    private Quaternion leftLegStart;
    private Quaternion rightLegStart;

    void Start()
    {
        bagStartPosition = moneyBag.localPosition;
        bagStartRotation = moneyBag.localRotation;

        leftArmStart = leftArm.localRotation;
        rightArmStart = rightArm.localRotation;
        leftLegStart = leftLeg.localRotation;
        rightLegStart = rightLeg.localRotation;
    }

    void Update()
    {
        float swing = Mathf.Sin(
            Time.time * animationSpeed
        ) * swingAmount;

        // Swing arms
        leftArm.localRotation = leftArmStart *
            Quaternion.Euler(swing, 0, 0);

        rightArm.localRotation = rightArmStart *
            Quaternion.Euler(-swing, 0, 0);

        // Swing legs in opposite directions
        leftLeg.localRotation = leftLegStart *
            Quaternion.Euler(-swing, 0, 0);

        rightLeg.localRotation = rightLegStart *
            Quaternion.Euler(swing, 0, 0);

        // Bounce the money bag
        float bounce = Mathf.Abs(
            Mathf.Sin(Time.time * animationSpeed)
        ) * bagBounce;

        moneyBag.localPosition = bagStartPosition +
            new Vector3(0, bounce, 0);

        // Sway the money bag
        moneyBag.localRotation = bagStartRotation *
            Quaternion.Euler(0, 0, swing * 0.2f);
    }
}
