using DG.Tweening;
using UnityEngine;

public class BackGroundGradient : MonoBehaviour
{
    public bool isStop = false;
    
    [SerializeField] private Renderer grad;
    private Sequence _colorChangeCeq;
    private void Start()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        if(isStop)
            return;
        _colorChangeCeq = DOTween.Sequence();
        
        var background = new Color(
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f)
        );
        
        _colorChangeCeq.Append(grad.material.DOColor(background, 1f)).OnComplete(ChangeColor);
        
    }
}
