using _game.rnk.Scripts;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.inventory;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public BattleHUD battle;
    public UITooltip tooltip;
    public Transform charactersRoot;
    public InventoryScreen inventory;
    
    CharacterView characterViewPrefab;
    
    void Awake()
    {
        G.hud = this;
        
        tooltip.Hide();
        battle.gameObject.SetActive(false);
        
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
    public void ShowBattleHud()
    {
        battle.gameObject.SetActive(true);
    }
    
    public void HideBattleHud()
    {
        battle.gameObject.SetActive(false);
    }
    
    public void ShowInventory(CharacterState characterState)
    {
        inventory.gameObject.SetActive(true);
        inventory.SetState(characterState);
    }
    
    public void HideInventory()
    {
        inventory.FreeState();
        inventory.gameObject.SetActive(false);
    }
}