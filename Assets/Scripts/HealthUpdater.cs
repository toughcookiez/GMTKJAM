using UnityEngine;

public class HealthUpdater : MonoBehaviour
{
    [SerializeField] private PlayerController controller;

    [SerializeField] private GameObject[] Hearts;

    private void Update()
    {
        if (controller.health == 3)
        {
            foreach (var item in Hearts)
            {
                item.SetActive(true);
            }
        }
        else if (controller.health == 2)
        {
            Hearts[2].SetActive(false);
        }
        else if (controller.health == 1)
        {
            Hearts[2].SetActive(false);
            Hearts[1].SetActive(false);
        }
        else
        {
            Hearts[2].SetActive(false);
            Hearts[1].SetActive(false);
            Hearts[0].SetActive(false);
        }

    }
}
