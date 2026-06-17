using UnityEngine;

public class ThiefSpawner : MonoBehaviour
{
    [Header("Рассыпание сладостей")]
    public GameObject[] itemsToDrop;

    [Header("Частицы")]
    public ParticleSystem sugarParticles;

    private float laneDistance = 4f;
    private float groundY = 1.0f;
    private float startThiefZ;
    private float startThiefRotY;
    private Vector3 startThiefScale;

    void UpdateThiefPosition(PlayerMovement player)
    {
        float hover = Mathf.Sin(Time.time * 3f) * 0.3f;
        float dynamicZ = startThiefZ + Mathf.Sin(Time.time * 0.7f) * 2.5f;
        float thiefX = Mathf.Lerp(transform.localPosition.x, player.targetX, Time.deltaTime * 5f);
        
        transform.localPosition = new Vector3(thiefX, 2.5f + hover, dynamicZ);
        transform.localScale = startThiefScale;

        if (!player.isGrounded)
            transform.Rotate(Vector3.right, 500f * Time.deltaTime);
        else
        {
            float tilt = (player.targetX - transform.localPosition.x) * 15f;
            Quaternion targetRot = Quaternion.Euler(0, startThiefRotY, tilt);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * 10f);
        }
    }

    void UpdateParticles(PlayerMovement player)
    {
        if (sugarParticles != null)
        {
            var emission = sugarParticles.emission;
            emission.rateOverTime = player.playerSpeed * 2f;
        }
    }
}