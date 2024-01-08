using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    #region fields
    #endregion

    #region methods
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Continue(0.5f));
            GetComponent<Image>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            SceneManager.LoadScene("Loading");
        }
    }

    IEnumerator Continue(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    #endregion
}
