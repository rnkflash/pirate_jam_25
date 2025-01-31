using System;
using System.Linq;
using _game.HeroInfo;
using _game.Inventory;
using UnityEngine;

namespace _game.rnk.Scripts.inventory
{
    public class InventoryScreen : MonoBehaviour
    {
        public Transform itemsPanel;
        public ItemSlot[] slots;
        
        [NonSerialized] public InventoryItem inventoryItemPrefab;
        
        [NonSerialized] public CharacterState state;

        [SerializeField] private InfoView _infoView;
        [SerializeField] private InventoryView _inventoryView;

        void Awake()
        {
            inventoryItemPrefab = "prefab/InventoryItem".Load<InventoryItem>();

            for (int i = 0; i < slots.Length; i++)
            {
                var slot = slots[i];
                slot.index = i;
                slot.onItemPut += OnItemPutInSlot;
                slot.onItemRemoved += OnItemRemovedFromSlot;
            }
        }

        void OnDestroy()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                var slot = slots[i];
                slot.Free();
                slot.onItemPut -= OnItemPutInSlot;
                slot.onItemRemoved -= OnItemRemovedFromSlot;
            }
        }
        
        void OnItemPutInSlot(InventoryItem item, int index)
        {
            G.run.inventory.Remove(item.state);
            item.state.slot = index;
            state.diceStates.First().artefacts.Add(item.state);
        }
        
        void OnItemRemovedFromSlot(InventoryItem item, int index)
        {
            state.diceStates.First().artefacts.Remove(item.state);
            G.run.inventory.Add(item.state);
        }

        public void SetState(CharacterState characterState)
        {
            state = characterState;

            var dice = state.diceStates.First();
            var faces = dice.faces;
            for (int i = 0; i < faces.Length; i++)
            {
                var face = faces[i];
                var slot = slots[i];
                slot.SetFace(face);
            }

            foreach (var artefactState in dice.artefacts)
            {
                var item = CreateInventoryItem(artefactState, slots[artefactState.slot].slot.transform);
                slots[artefactState.slot].slot.Claim(item.draggable, true);
            }

            foreach (var artefactState in G.run.inventory)
            {
                CreateInventoryItem(artefactState, itemsPanel.transform);
            }
            
            _infoView.SetState(characterState);
            _inventoryView.SetState(characterState);
        }
        public void FreeState()
        {
            _infoView.Hide();
            _inventoryView.Hide();
            foreach (var slot in slots)
            {
                slot.Free();
            }
            
            foreach(Transform child in itemsPanel.transform)
                Destroy(child.gameObject);
            
            itemsPanel.DetachChildren();
            
            state = null;
        }

        InventoryItem CreateInventoryItem(ArtefactState artefactState, Transform parent)
        {
            var item = Instantiate(inventoryItemPrefab, parent);
            item.SetState(artefactState);
            return item;
        }
    }
}