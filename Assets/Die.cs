using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Die : MonoBehaviour
{
  public void ChangeScene()
  {
    SceneManager.LoadScene("SampleScene");
  }

}
