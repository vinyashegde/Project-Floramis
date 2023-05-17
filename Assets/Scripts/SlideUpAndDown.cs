using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SlideUpAndDown : MonoBehaviour
{
    public RectTransform uiVerticalRectangle;
    public float slideDuration = 0.5f;

    private bool isSliding = false;
    public bool isShown = false;

    private PanAndZoom zoomAndPanScript;
    private bool zoomAndPanEnabled = true;

    void Start()
    {
        zoomAndPanScript = Camera.main.GetComponent<PanAndZoom>();
    }

    public void OnButtonClick()
    {
        if (!isSliding)
        {
            isSliding = true;
            StartCoroutine(Slide(isShown ? -uiVerticalRectangle.rect.height : uiVerticalRectangle.rect.height));
            isShown = !isShown;
        }
    }

    private IEnumerator Slide(float targetPosition)
    {
        float elapsedTime = 0f;
        Vector2 startingPosition = uiVerticalRectangle.anchoredPosition;
        Vector2 targetPositionVector = new Vector2(startingPosition.x, startingPosition.y + targetPosition);

        while (elapsedTime < slideDuration)
        {
            uiVerticalRectangle.anchoredPosition = Vector2.Lerp(startingPosition, targetPositionVector, (elapsedTime / slideDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        uiVerticalRectangle.anchoredPosition = targetPositionVector;
        isSliding = false;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if(isShown==true)
        {
            zoomAndPanScript.enabled = false;
            zoomAndPanEnabled = false;
        }
        else{
             zoomAndPanScript.enabled = true;
            zoomAndPanEnabled = true;
        }
    }
}
