using _game.rnk.Scripts;
using _game.rnk.Scripts.battleSystem;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public BattleHUD battlehud;
    public UITooltip tooltip;
    public Transform charactersRoot;

    CharacterView characterViewPrefab;
    
    void Awake()
    {
        G.hud = this;
        G.hud.tooltip.Hide();
        
        characterViewPrefab = "prefab/TimurCharacterView".Load<CharacterView>();
    }

    public void Init()
    {
        foreach (var characterState in G.run.characters)
        {
            if (characterState.view == null)
                CreateCharacterView(characterState);
        }
    }
    
    public CharacterView CreateCharacterView(CharacterState characterState)
    {
        var character = Instantiate(characterViewPrefab, charactersRoot);
        character.SetState(characterState);
        return character;
    }
    
    public static Vector2 MousePositionToCanvasPosition(Canvas canvas, RectTransform rectTransform)
    {
        Vector2 localPoint;
        Vector2 screenPosition = Input.mousePosition;
        Camera uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            screenPosition,
            uiCamera,
            out localPoint
        );

        return localPoint;
    }

    public Vector2 MousePos()
    {
        return MousePositionToCanvasPosition(G.hud.GetComponentInParent<Canvas>(), G.hud.GetComponent<RectTransform>());
    }
}