using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    [SerializeField] private GameObject coinsParent;
    [SerializeField] private TextMeshProUGUI coinCounterText;
    [SerializeField] private GameObject rewardButton;
    [SerializeField] private AudioSource coinDropSound;
    [SerializeField] private AudioSource coinCollectSound;
    private Vector3[] initialPos;
    private Quaternion[] initialRot;
    private Vector2 targetCoinPos;

    void Start()
    {
        // coins go to target coin position
        targetCoinPos = new Vector2(12f, 670f);

        // get initial positions and rotations values of coins
        initialPos = new Vector3[coinsParent.transform.childCount];
        initialRot = new Quaternion[coinsParent.transform.childCount];

        for (int i = 0; i < coinsParent.transform.childCount; i++)
        {
            initialPos[i] = coinsParent.transform.GetChild(i).position;
            initialRot[i] = coinsParent.transform.GetChild(i).rotation;
        }
    }

    private void ResetPos()
    {
        // set coins positions and rotations values to reset
        for (int i = 0; i < coinsParent.transform.childCount; i++)
        {
            coinsParent.transform.GetChild(i).position = initialPos[i];
            coinsParent.transform.GetChild(i).rotation = initialRot[i];
        }
    }

    public void CollectCoins()
    {
        // reset coins positions and rotations
        ResetPos();

        // delay after each coin
        float delay = 0f;

        // activate coins parent
        coinsParent.SetActive(true);

        // disable button after clicked
        rewardButton.GetComponent<Button>().interactable = false;

        // play coin drop sound
        coinDropSound.Play();

        // collect coins
        for (int i = 0; i < coinsParent.transform.childCount; i++)
        {
            // these values can be adjusted according to the desired effect
            var coinChild = coinsParent.transform.GetChild(i);
            coinChild.DOScale(1f, 0.3f)
                .SetDelay(delay).SetEase(Ease.OutBack);
            coinChild.GetComponent<RectTransform>().DOAnchorPos(targetCoinPos, 0.8f)
                .SetDelay(delay + 0.5f).SetEase(Ease.InBack);
            coinChild.DORotate(Vector3.zero, 0.5f)
                .SetDelay(delay + 0.5f).SetEase(Ease.Flash);
            coinChild.DOScale(0f, 0.3f)
                .SetDelay(delay + 1.8f).SetEase(Ease.OutBack);
            delay += 0.1f;
        }

        // start coroutine for counter text
        StartCoroutine(CountCoins());
    }

    IEnumerator CountCoins()
    {
        yield return new WaitForSecondsRealtime(1.4f);

        float timer = 0f;
        float counter = int.Parse(coinCounterText.text);

        for (int i = 0; i < coinsParent.transform.childCount; i++)
        {
            coinCounterText.text = (++counter).ToString();

            coinCollectSound.Play();

            timer += 0.03f;

            yield return new WaitForSecondsRealtime(timer);
        }
    }
}
