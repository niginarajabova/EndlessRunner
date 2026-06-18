using UnityEngine;

public class ItemData : MonoBehaviour
{
    [Tooltip("Bu buyumni yig'ish uchun beriladigan ball")]
    public int scoreValue = 3;

    [Tooltip("Yig'ilganda paydo bo'ladigan vizual effekt prefabi")]
    public GameObject collectEffect;
}
