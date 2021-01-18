using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Controller : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField]
	private Image 		bckgCtrl;
	[SerializeField]
	private Image 		botCtrl;
	[SerializeField]
	private float		sensibility = 0.5f;
	[SerializeField]
	private Sprite[] 	controllerImages;
	private bool  		locked;

	public Vector3 inputDir;

	private void Start()
	{
		bckgCtrl = GetComponent<Image>();
		botCtrl = transform.GetChild(1).GetComponent<Image>();
	}

	public virtual void OnDrag(PointerEventData ped)
	{
		Vector2 pos;

		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bckgCtrl.rectTransform, ped.position, ped.pressEventCamera, out pos))
		{
			pos.x = (pos.x / bckgCtrl.rectTransform.sizeDelta.x);
			pos.y = (pos.y / bckgCtrl.rectTransform.sizeDelta.y);

			inputDir = new Vector3(pos.x * 2, pos.y * 2, 0);
			inputDir = (inputDir.magnitude > 1) ? inputDir.normalized : inputDir; 

			botCtrl.rectTransform.anchoredPosition = new Vector3(inputDir.x * (bckgCtrl.rectTransform.sizeDelta.x / 4), inputDir.y * (bckgCtrl.rectTransform.sizeDelta.y / 4));
		}

		//atualiza as imagens
		if (inputDir.y < -sensibility ) 
		{
			botCtrl.sprite = controllerImages[1]; //alavanca para baixo
		}
		else if (inputDir.x > sensibility+0.3 )
		{
			botCtrl.sprite = controllerImages[2]; //alavanca para direita
		}
		else if (inputDir.x < -sensibility+-0.3 )
		{
			botCtrl.sprite = controllerImages[3]; //alavanca para esquerda
		}
		else
		{
			botCtrl.sprite = controllerImages[0]; //alavanca no centro
		}


	}

	public virtual void OnPointerDown(PointerEventData ped)
	{
		OnDrag(ped);
	}

	public virtual void OnPointerUp(PointerEventData ped)
	{
		inputDir = Vector3.zero;
		botCtrl.rectTransform.anchoredPosition = Vector3.zero;
		botCtrl.sprite = controllerImages[0];
	}

	public float GetHorizontal()
	{
		if (locked) return 0.0f;

		if (inputDir.x > sensibility )
			return 1f;
		else if (inputDir.x < -sensibility )
			return -1f;
		else
			return 0.0f;
	}

	public float GetVertical()
	{
		if (locked) return 0.0f;

		if (inputDir.y > sensibility )
		{
			return 1f;
		}
		else if (inputDir.y < -sensibility )
		{
			return -1f;
		}
		else
		{
			return 0.0f;
		}
			
	}

	public void LockController ()
	{
		inputDir = Vector3.zero;
		locked = true;
	}

	public void LockController (float time)
	{
		LockController();
		StartCoroutine(UnlockControllerAfterTime (time));
	}

	public void UnlockController ()
	{
		locked = false;
	}

	IEnumerator UnlockControllerAfterTime (float time)
	{
		yield return new WaitForSeconds(time);
		UnlockController ();
	}
   
}