using System;
using System.Collections;
using System.Collections.Generic;
using ConnectThePops.Data;
using ConnectThePops.Models;
using ConnectThePops.Settings;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace ConnectThePops.Views
{
    public class PopView : MonoBehaviour, IPoolable<PopsData, IMemoryPool>
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private int  _value;
        public int Value => _value;
        private IMemoryPool _pool;
        public SlotModel CurrentSlot{ get; private set; }
        public bool IsTapped{ get; private set; }
        public Color PopColor{ get; private set; }
        
        
        #region Injection

        private GameSettings _gameSettings;

        [Inject]
        private void Construct(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        #endregion

        public void OnSpawned(PopsData data, IMemoryPool pool)
        {
            _value = data.popValue;
            _pool = pool;
            _spriteRenderer.sprite = data.popSprite;
            PopColor = data.popColor;
        }

        public void SetCurrentSlot(SlotModel slot)
        {
            CurrentSlot = slot;
        }

        public void SetPositionImmediate(Vector2 position)
        {
            transform.position = position;
        }

        public void Tapped()
        {
            IsTapped = true;
            transform.DOScale(Vector2.one * .55f, .05f);
        }

        public void Release()
        {
            IsTapped = false;
            transform.DOScale(Vector2.one / 2, .05f);
        }

        public void Merge(SlotModel slot)
        {
            transform.DOMove(slot.WorldPos, _gameSettings.PopsMoveTime).OnComplete(() => { _pool.Despawn(this);});
            CurrentSlot.SetEmpty();
        }

        public async UniTask MoveToNewSlot(SlotModel slot, float delayTime)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
            transform.DOMove(slot.WorldPos, _gameSettings.PopsMoveTime);
            SetCurrentSlot(slot);
        }

        public async UniTask SetNewData(PopsData newData, float delayTime)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
            _value = newData.popValue;
            _spriteRenderer.sprite = newData.popSprite;
            PopColor = newData.popColor;
            transform.DOScale(Vector2.one * .65f, .1f).OnComplete(() => { transform.DOScale(Vector2.one * .5f, .1f); });
        }
        
        public void OnDespawned()
        {
            IsTapped = false;
        }

        public class Factory : PlaceholderFactory<PopsData, PopView>
        {
        }

        public class Pool : MonoPoolableMemoryPool<PopsData, IMemoryPool, PopView>
        {
        }
    }
}
