using System.Collections;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private TeamReturnPositions redTeamPositions;
    [SerializeField] private TeamReturnPositions blueTeamPositions;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plate") || other.CompareTag("Pan") || other.CompareTag("Pot"))
        {
            Ingredient ingredient = other.GetComponent<Ingredient>();
            if (ingredient != null)
            {
                StartCoroutine(DeactivateAndRespawn(ingredient));
            }
        }
        else if (other.CompareTag("Ingredient"))
        {
            Ingredient ingredient = other.GetComponent<Ingredient>();
            if (ingredient != null)
            {
                Destroy(ingredient.gameObject);  // 요리 재료 파괴
            }
        }
    }

    private IEnumerator DeactivateAndRespawn(Ingredient ingredient)
    {
        ingredient.gameObject.SetActive(false);  // 재료 비활성화
        yield return new WaitForSeconds(3.0f);

        Transform returnPosition = GetReturnPosition(ingredient.team, ingredient.gameObject.tag);

        if (returnPosition != null)
        {
            ingredient.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ingredient.transform.rotation = Quaternion.Euler(0, 0 ,0);
            // ingredient.transform.GetChild(0).GetComponent<BoxCollider>().size *= 1.02f;
            ingredient.transform.SetParent(returnPosition.parent);
            // ingredient.transform.localPosition = returnPosition.localPosition + new Vector3(0.072f, 0.006f, 0.024f);
            
            // pan, pot는 위치 다름
            if(ingredient.CompareTag("Plate"))
            {
                ingredient.transform.localPosition = new Vector3(0.072f, 0.006f, 0.024f);
            }
            else
            {
                ingredient.transform.localPosition = new Vector3(0.0f, 0.006f, 0.0f);
            }

            ingredient.transform.SetSiblingIndex(2);  // 두 번째 자식으로 설정
            ingredient.gameObject.SetActive(true);  // 재료 활성화
            ingredient.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;  // 위치 고정
            returnPosition.parent.transform.GetChild(0).GetComponent<ObjectHighlight>().onSomething = true;
            Debug.Log($"{ingredient.gameObject.tag}가 {returnPosition.position} 위치로 반환되었습니다.");
        }
        else
        {
            Debug.LogWarning($"{ingredient.gameObject.tag}의 반환 위치를 찾을 수 없습니다.");
        }
    }

    private Transform GetReturnPosition(Ingredient.Team team, string objectType)
    {
        TeamReturnPositions teamPositions = team == Ingredient.Team.Red ? redTeamPositions : blueTeamPositions;
        Transform[] positions = null;

        switch (objectType)
        {
            case "Pan":
                positions = teamPositions.panPositions;
                break;
            case "Pot":
                positions = teamPositions.potPositions;
                break;
            case "Plate":
                positions = teamPositions.platePositions;
                break;
        }

        if (positions != null)
        {
            foreach (Transform position in positions)
            {
                if (position.parent.childCount == 2 && position.parent.gameObject.name.Contains("Plate"))
                {
                    return position;
                } 
                else if(position.parent.childCount == 3 && (position.parent.gameObject.name.Contains("Pot") || position.parent.gameObject.name.Contains("Pan")))
                {
                    return position;
                }
            }
        }

        return null;
    }
}
