using DefaultNamespace;
using GameScene;
using UnityEngine;
using UnityEngine.EventSystems;

public class TimerPauseUI : MonoBehaviour, IPointerClickHandler
{
    private GameManager _gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _gameManager.PauseGame();
    }
}
