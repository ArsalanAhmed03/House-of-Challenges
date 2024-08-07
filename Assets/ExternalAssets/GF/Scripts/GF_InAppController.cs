using UnityEngine;
using System.Collections;

public class GF_InAppController : MonoBehaviour
{

	public static GF_InAppController Instance { get; private set; }


	void Awake()
	{

		if (Instance != null)
		{
			DestroyImmediate(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}
}
