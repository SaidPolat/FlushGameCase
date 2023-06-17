using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public Animator animator;

    public float speed;
    public VariableJoystick variableJoystick;
    public Transform gemStackPoint;

    public List<GameObject> carriedGems;
    public List<GemController> collectedGemsProperties;

    [HideInInspector] public Vector3 lastPos;
    Vector3 rotationVector;
    CharacterController characterController;


    void Start()
    {
        characterController = GetComponent<CharacterController>();  
    }

    void Update()
    {
        if (variableJoystick.isDragging)
        {
            characterController.Move(new Vector3(variableJoystick.Horizontal * speed * Time.deltaTime, 0, variableJoystick.Vertical * speed * Time.deltaTime));
         
            rotationVector = new Vector3(0, (variableJoystick.Vertical < 0) ? 180 - (variableJoystick.Horizontal * 90.0f) : (variableJoystick.Horizontal * 90.0f),
                0);

            transform.rotation = Quaternion.Euler(rotationVector);
        }
        else
        {
            rotationVector = new Vector3(0, (variableJoystick.LastRotation.y < 0) ? 180 - (variableJoystick.LastRotation.x * 90.0f) : (variableJoystick.LastRotation.x * 90.0f), 0);

            transform.position = lastPos;
            transform.rotation = Quaternion.Euler(rotationVector);
        }
    }

    private void OnTriggerEnter(Collider other)
    {      
        switch (other.tag)
        {
            case "Gem": 
                GemController gemController = other.GetComponent<GemController>();
                if(other.transform.localScale.x > 0.25 / 0.39f)
                {
                    int type = gemController.GemColor.GetHashCode();
                    //Debug.Log("type: " + type);
                    StackGems(gemController, type);
                }
                break;
            case "SellZone":
                if (SellGemsSignature != null) StopCoroutine(SellGemsSignature);
                SellGemsSignature = SellGems();
                StartCoroutine(SellGemsSignature);
                break;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SellZone"))
        {
            Debug.Log("Stop coroutine");
            if (SellGemsSignature != null) StopCoroutine(SellGemsSignature);
        }
    }

    public void StackGems(GemController gem, int gemType)
    {
        Vector3 tempPos = gemStackPoint.position;

        gem.SpawnNewGemForTile();

        gem.gameObject.transform.DOMove(gemStackPoint.position + new Vector3(0, carriedGems.Count * 0.6f, 0), 0.1f).OnComplete(() =>
        {
            var newGem = Instantiate(GameManager.Instance.GemList.GemModels[gemType].collectedGemPrefab, gemStackPoint.transform);
            newGem.transform.DOLocalMove(new Vector3(0, carriedGems.Count * 0.6f, 0), 0);
            newGem.transform.DOScale(.4f, 0);

            GemController newGemController = newGem.GetComponent<GemController>();

            newGemController.GemName = gem.GemName;
            newGemController.GemStartPrice = gem.GemStartPrice;
            newGemController.ScaleBeforeCollect = gem.transform.localScale.x * 0.39f;
            //Debug.Log("Collected gems scale: " + newGemController.scaleBeforeCollect);

            carriedGems.Add(newGem);
            GameManager.Instance.GemList.GemModels[gemType].collectedGemCount++;
            UIManager.Instance.UpdateCollectedGemUI(gemType, GameManager.Instance.GemList.GemModels[gemType].collectedGemCount);

            gem.transform.DOKill();
            Destroy(gem.gameObject);
        });
    }

    public IEnumerator SellGemsSignature;
    public IEnumerator SellGems()
    {
        if(carriedGems.Count > 0)
        {
            for (int i = carriedGems.Count - 1; i >= 0; i--)
            {
                var gem = carriedGems[i];

                gem.GetComponent<GemController>().EarnMoneyFromSale();
               
                gem.SetActive(false);
                carriedGems.Remove(gem);

                Destroy(gem);

                yield return new WaitForSeconds(0.1f);
            }
        }
        
    }
}
