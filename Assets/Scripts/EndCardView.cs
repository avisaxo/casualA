using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCardView : MonoBehaviour
{
    public Animator animator;
    private static readonly int CloseShopDoor = Animator.StringToHash("CloseShopDoor");
    
    public void ShowEndCard()
    {
        gameObject.SetActive(true);
        animator.SetTrigger(CloseShopDoor);
        StartCoroutine(ShowShopScene());
    }

    private IEnumerator ShowShopScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(2);
    }
}
