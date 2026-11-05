using UnityEngine;
using UnityEngine.UI;

public class StaminaMeter : MonoBehaviour
{
    [SerializeField] private Image barFrame;
    [SerializeField] private Image bar;
    [SerializeField] private PlayerMovement playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        barFrame.gameObject.SetActive(false);
        bar.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (playerMovement.CurrentState == MovementState.Climbing)
        {
            barFrame.gameObject.SetActive(true);
            bar.gameObject.SetActive(true);
            bar.fillAmount = playerMovement.timeNormalized;

            if (playerMovement.CanClimb == false)
            {
                barFrame.gameObject.SetActive(false);
                bar.gameObject.SetActive(false);
            }
        }
        else
        {
            bar.fillAmount = playerMovement.StaminaNormalized;
        }

        if (playerMovement.SprintHeld == true)
        {
            barFrame.gameObject.SetActive(true);
            bar.gameObject.SetActive(true);
        }
        if (playerMovement.StaminaNormalized == 1 && playerMovement.CurrentState != MovementState.Climbing)
        {
            barFrame.gameObject.SetActive(false);
            bar.gameObject.SetActive(false);
        }

    }
}
