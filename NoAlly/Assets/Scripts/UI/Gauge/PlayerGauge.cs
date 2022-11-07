using UnityEngine;
using UnityEngine.UI;
using UniRx;


    public class PlayerGauge : SingletonBehaviour<PlayerGauge>
    {
        [SerializeField, Tooltip("�I�u�W�F�N�g�̕K�E�Z�Q�[�W�̏��")]
        float _maxSAP = 20;
        [SerializeField, Tooltip("�I�u�W�F�N�g��HP�̏��")]
        float _maxHP = 20;
        [SerializeField, Tooltip("�v���C���[�̃_���[�W�T�E���h")]
        AudioClip _damageSound;

        [Tooltip("�����h���")]
        float _rigitDefensePercentage = 0f;
        [Tooltip("���h���")]
        float _fireDifansePercentage = 0f;
        [Tooltip("�d�C�h���")]
        float _elekeDifansePercentage = 0f;
        [Tooltip("�X���h���")]
        float _frozenDifansePercentage = 0f;
        [Tooltip("�I�u�W�F�N�g�̌��݂̕K�E�Z�Q�[�W")]
        FloatReactiveProperty _sap = null;
        [Tooltip("�I�u�W�F�N�g�̌��݂�HP")]
        FloatReactiveProperty _hp = null;
        [Tooltip("�I�u�W�F�N�g�̐�������")]
        bool _living = true;
        [Tooltip("Animator�̊i�[�ϐ�")]
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
        //�_���[�W�v�Z
        public void DamageMethod(float weaponPower, float firePower, float elekePower, float frozenPower)
        {
            var damage = weaponPower * _rigitDefensePercentage
                + firePower * _fireDifansePercentage
                + elekePower * _elekeDifansePercentage
                + frozenPower * _frozenDifansePercentage;
            _hp.Value -= damage;

            //��������
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



