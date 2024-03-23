using UnityEngine;

public class AutoRiggerTest : MonoBehaviour
{
    [SerializeField] private GameObject sampleAvatar;
    [SerializeField] private AutoRigger autoRigger;

    // Start is called before the first frame update
    void Start()
    {
        GameObject avatar = Instantiate(sampleAvatar, transform.position, Quaternion.identity);
        avatar.transform.SetParent(transform);
        autoRigger.SetupRig(avatar);
    }
}
