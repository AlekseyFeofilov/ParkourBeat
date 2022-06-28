using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class PreMainScene : MonoBehaviour
    {
        [Header("Component")] [SerializeField] private GameObject warning;
        [SerializeField] private GameObject company;

        private bool flag = false;

        private void Awake()
        {
            if (warning != null && company != null)
            {
                var mes = Start();
            }
            else
            {
                Debug.Log("Допущена ошибка");
            }
        }

        private IEnumerator Start()
        {
            var colorThreeObjectA = warning.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().color.a;
            colorThreeObjectA = 0;

            yield return new WaitForSeconds(5f);
            warning.transform.GetChild(2).gameObject.SetActive(true);

            while (colorThreeObjectA < 142.0)
            {
                colorThreeObjectA += 5;

                yield return new WaitForSeconds(0.01f);
            }
        }

        private void Update()
        {
            //показываем "компанию"
            if (Input.anyKey)
            {
                if (!flag)
                {
                    warning.SetActive(false);
                    StartCoroutine(Next());
                    flag = true;
                }
            }
        }

        private IEnumerator Next()
        {
            var color = company.GetComponent<RawImage>().color;
            color.a = 0;

            company.SetActive(true);

            while (color.a < 256)
            {
                color.a = color.a + 1;
                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(3f);

            SceneManager.LoadScene("Main Menu");
        }
    }
}