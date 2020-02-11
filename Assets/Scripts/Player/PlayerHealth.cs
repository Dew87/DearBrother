using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
	public void TakeDamage()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
