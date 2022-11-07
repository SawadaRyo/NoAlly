using UnityEngine;
using UnityEngine.UI;
using UniRx;


    public class PlayerGauge : SingletonBehaviour<PlayerGauge>
    {
        [SerializeField, Tooltip("オブジェクトの必殺技ゲージの上限")]
        float _maxSAP = 20;
        [SerializeField, Tooltip("オブジェクトのHPの上限")]
        float _maxHP = 20;
        [SerializeField, Tooltip("プレイヤーのダメージサウンド")]
        AudioClip _damageSound;

        [Tooltip("物理防御力")]
        float _rigitDefensePercentage = 0f;
        [Tooltip("炎防御力")]
        float _fireDifansePercentage = 0f;
        [Tooltip("電気防御力")]
        float _elekeDifansePercentage = 0f;
        [Tooltip("氷結防御力")]
        float _frozenDifansePercentage = 0f;
        [Tooltip("オブジェクトの現在の必殺技ゲージ")]
        FloatReactiveProperty _sap = null;
        [Tooltip("オブジェクトの現在のHP")]
        FloatReactiveProperty _hp = null;
        [Tooltip("オブジェクトの生死判定")]
        bool _living = true;
        [Tooltip("Animatorの格納変数")]
        Animator _animator;

        public bool Living => _living;
        public float MaxHP => _maxHP;
        public float MaxSAP => _maxSAP;
        public IReadOnlyReactiveProperty<float> SAP => _sap;
        public IReadOnlyReactiveProperty<float> HP => _hp;

        void Awake()
        {
            _animator = gameObject.GetComponent<Animator>();
        }
        public void Init()
        {
            _hp = new FloatReactiveProperty(_maxHP);
            _sap = new FloatReactiveProperty(0);
            _living = true;
            _animator = GetComponentInParent<Animator>();
        }

        void Update()
        {
            if (_living)
            {
                return;
            }
            else
            {
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
                _animator.SetBool("Death", true);
            }
        }
        //ダメージ計算
        public void DamageMethod(float weaponPower, float firePower, float elekePower, float frozenPower)
        {
            var damage = weaponPower * _rigitDefensePercentage
                + firePower * _fireDifansePercentage
                + elekePower * _elekeDifansePercentage
                + frozenPower * _frozenDifansePercentage;
            _hp.Value -= damage;

            //生死判定
            if (_hp.Value <= 0)
            {
                _living = false;
            }
            else return;
        }


        public void HPPuls(int hpPuls)
        {
            _hp.Value += hpPuls;
            if (_hp.Value > _maxHP)
            {
                _hp.Value = _maxHP;
            }
        }
        public void SAPPuls(int sapPuls)
        {
            _sap.Value += sapPuls;
            if (_sap.Value > _maxSAP)
            {
                _sap.Value = _maxSAP;
            }
        }

        public void UseSAP(float useSAP)
        {
            _sap.Value -= useSAP;
        }

        private void OnDisable()
        {
            _hp.Dispose();
            _sap.Dispose();
        }
    }



