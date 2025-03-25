using TMPro;
using UnityEngine;

namespace Hunter
{
    public class TextDamage : MonoBehaviour
    {
        public Transform target;

        public RectTransform textParent;

        public TextMeshProUGUI[] texts;
        public TextMeshProUGUI[] textCrits;
        public TextMeshProUGUI[] textCombos;
        public TextMeshProUGUI[] textMisses;

        RectTransform[] textRects;
        RectTransform[] textCritRects;
        RectTransform[] textComboRects;
        RectTransform[] textMissRects;

        public void Awake()
        {
            textRects = new RectTransform[texts.Length];
            textCritRects = new RectTransform[textCrits.Length];
            textComboRects = new RectTransform[textCombos.Length];
            textMissRects = new RectTransform[textMisses.Length];

            for (int i = 0; i < texts.Length; i++)
            {
                textRects[i] = texts[i].GetComponent<RectTransform>();
            }

            for (int i = 0; i < textCrits.Length; i++)
            {
                textCritRects[i] = textCrits[i].GetComponent<RectTransform>();
            }
            
            for (int i = 0; i < textCombos.Length; i++)
            {
                textComboRects[i] = textCombos[i].GetComponent<RectTransform>();
            }
            
            for (int i = 0; i < textMisses.Length; i++)
            {
                textMissRects[i] = textMisses[i].GetComponent<RectTransform>();
            }
        }

        int indexDamage;
        int indexDamageCrit;
        int indexCombo;
        int indexMiss;

        public void Update()
        {
            textParent.position = UIInGame.instance.virtualCam.cam.WorldToScreenPoint(target.position);
        }

        public void ShowDamage(int damage)
        {
            textRects[indexDamage].gameObject.SetActive(false);
            textRects[indexDamage].gameObject.SetActive(true);

            textRects[indexDamage].anchoredPosition = new Vector2(Random.Range(-35f, 35f), Random.Range(0, 55));

            texts[indexDamage].text = damage.ToString();
            indexDamage++;
            if (indexDamage == texts.Length) indexDamage = 0;
        }

        public void ShowDamageCrit(int damage)
        {
            textCritRects[indexDamageCrit].gameObject.SetActive(false);
            textCritRects[indexDamageCrit].gameObject.SetActive(true);

            textCritRects[indexDamageCrit].anchoredPosition = new Vector2(Random.Range(-35f, 35f), Random.Range(0, 55));

            textCrits[indexDamageCrit].text = "<sprite=0>" + damage.ToString();
            indexDamageCrit++;
            if (indexDamageCrit == textCrits.Length) indexDamageCrit = 0;
        }
        
        public void ShowCombo()
        {
            textComboRects[indexCombo].gameObject.SetActive(false);
            textComboRects[indexCombo].gameObject.SetActive(true);

            textComboRects[indexCombo].anchoredPosition = new Vector2(Random.Range(-35f, 35f), Random.Range(15, 55));

            textCombos[indexCombo].text = "Combo";
            indexCombo++;
            if (indexCombo == textCombos.Length) indexCombo = 0;
        }
        
        public void ShowMiss()
        {
            textMissRects[indexMiss].gameObject.SetActive(false);
            textMissRects[indexMiss].gameObject.SetActive(true);

            textMissRects[indexMiss].anchoredPosition = new Vector2(Random.Range(-55f, 55f), Random.Range(0, 55));

            textMisses[indexMiss].text = "Miss";
            indexMiss++;
            if (indexMiss == textMisses.Length) indexMiss = 0;
        }
    }
}
