using _game.rnk.Scripts.battleSystem;
using _game.tinypack.source.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _game.rnk.Scripts
{
    public class BattleHUD: MonoBehaviour
    {
        public TMP_Text RollButtonText;
        public TMP_Text EndTurnButtonText;
        public Button RollButton;
        public Button EndTurnButton;
        public SpriteVFX meleeHit;
        public SpriteVFX armorHit;
        public SpriteVFX healHit;
        public Transform enemiesRoot;
        EnemyView enemyViewPrefab;

        void Awake()
        {
            enemyViewPrefab = "prefab/EnemyView".Load<EnemyView>();
        }

        void Start()
        {
            EndTurnButton.onClick.AddListener(OnClickEndTurn);
            RollButton.onClick.AddListener(OnClickRollButton);
        }

        void OnDestroy()
        {
            EndTurnButton.onClick.RemoveAllListeners();
            RollButton.onClick.RemoveAllListeners();
        }
        
        public EnemyView CreateEnemyView(EnemyState enemyState)
        {
            var enemy = Instantiate(enemyViewPrefab, enemiesRoot);
            enemy.SetState(enemyState);
            return enemy;
        }
        
        public void PunchEndTurn()
        {
            G.ui.Punch(EndTurnButton.transform);
        }
    
        public void PunchRollButton()
        {
            G.ui.Punch(RollButton.transform);
        }
        
        public void DisableHud()
        {
            G.battle.rollDicesZone.canDrag = false;
            EndTurnButton.interactable = false;
            RollButton.interactable = false;
        }

        public void EnableHud()
        {
            G.battle.rollDicesZone.canDrag = true;
            EndTurnButton.interactable = true;
            RollButton.interactable = true;
        }
        
        void OnClickEndTurn()
        {
            PunchEndTurn();
            G.battle.EndTurnButton();
        }
    
        void OnClickRollButton()
        {
            PunchRollButton();
            G.battle.ReRollButton();
        }
        public void DisableRerollButton()
        {
            RollButton.interactable = false;
        }
    }
}