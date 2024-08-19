using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonGameActionsUIRegistrar : MonoBehaviour
{
	private AdInitiator _adInitiator = null;

	private void OnEnable()
	{
		if (_adInitiator == null)
		{
			_adInitiator = FindObjectOfType<AdInitiator>();
		}	
		_adInitiator.NonGameActionOccured();
	}
}
