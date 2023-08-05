//日本語コメント可
using System;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace ActorBehaviour
{
    namespace Jump
    {
        public class ActorAir
        {
            [Tooltip("")]
            IInputPlayer _owner = null;
            [Tooltip("")]
            bool _keyLook = false;
            [Tooltip("")]
            bool _isJump = false;
            [Tooltip("")]
            float _timeInAir = 0f;
            [Tooltip("")]
            ActorVec _actorFallJudge = ActorVec.None;

            [Tooltip("")]
            StateOfPlayer stateOfPlayer => _owner.CurrentLocation.Value;
            public bool KeyLook => _keyLook;

            public ActorAir(IInputPlayer owner)
            {
                _owner = owner;
                ValueWatcher();
            }

            void ValueWatcher()
            {
                _owner.IsJump
                    .Subscribe(isJump =>
                    {
                        if (isJump)
                        {
                            _isJump = !_keyLook;
                            if (_isJump && stateOfPlayer == StateOfPlayer.OnGround)
                            {
                                _actorFallJudge = ActorVec.Up;
                            }
                        }
                        else
                        {
                            _isJump = false;
                            _keyLook = false;
                        }
                    });
                _owner.CurrentLocation
                    .Subscribe(location =>
                    {
                        switch (location)
                        {
                            case StateOfPlayer.OnGround:
                                _timeInAir = 0f;
                                _keyLook = true;
                                break;
                            case StateOfPlayer.GripingWall:
                                _timeInAir = 0f;
                                _keyLook = true;
                                break;
                            default:
                                break;
                        }
                    });
            }

            /// <summary>
            /// ジャンプ力と落下策度を計算(毎フレーム計算)
            /// </summary>
            /// <param name="jumoPawer"></param>
            /// <param name="fallSpeed"></param>
            /// <param name="jumpLowerLimit"></param>
            /// <returns></returns>
            public Vector3 ActorVectorInAir(float jumoPawer, float fallSpeed = 120f, float jumpLowerLimit = 0.03f)
            {
                Vector3 ActorVertical = Vector3.zero;
                switch (_actorFallJudge)
                {
                    case ActorVec.Up:
                        _timeInAir += Time.deltaTime;
                        if (_isJump || jumpLowerLimit > _timeInAir)
                        {
                            ActorVertical.y = jumoPawer;
                            ActorVertical.y -= (fallSpeed * Mathf.Pow(_timeInAir, 2));
                        }
                        else
                        {
                            _timeInAir += Time.deltaTime; // 落下を早める
                            ActorVertical.y = jumoPawer;
                            ActorVertical.y -= (fallSpeed * Mathf.Pow(_timeInAir, 2));
                        }

                        if (0f > ActorVertical.y)
                        {
                            _actorFallJudge = ActorVec.Down;
                            ActorVertical.y = 0f;
                            _timeInAir = 0.1f;
                        }
                        break;
                    case ActorVec.Down:
                        _timeInAir += Time.deltaTime;

                        //ActorVertical.y = 0f;
                        ActorVertical.y = -(fallSpeed * Mathf.Pow(_timeInAir, 2));
                        if (ActorVertical.y < -(fallSpeed / 2))
                        {
                            ActorVertical.y = -(fallSpeed / 2);
                        }
                        Debug.Log(ActorVertical.y);
                        break;

                    default:
                        break;
                }
                //Debug.Log(ActorVertical);
                return ActorVertical;
            }
        }
    }
    namespace Move
    {
        public class ActorMove
        {
            [Tooltip("")]
            bool _ableJumpInput = true;
            [Tooltip("")]
            Vector2 _velo = Vector2.zero;
            [Tooltip("Playerの向き")]
            ActorVec _actorVec = ActorVec.Right;

            /// <summary>
            /// プレイヤーの回転
            /// </summary>
            /// <param name="turnSpeed"></param>
            /// <param name="rb"></param>
            /// <param name="rotVector"></param>
            /// <returns></returns>
            public ActorVec ActorRotateMethod(float turnSpeed, Transform targetObject, Vector2 rotVector)
            {
                //プレイヤーの方向転換
                if (rotVector.x == -1)
                {
                    Quaternion rotationLeft = Quaternion.LookRotation(Vector3.left);
                    targetObject.rotation = Quaternion.Slerp(targetObject.rotation, rotationLeft, Time.deltaTime * turnSpeed);
                    _actorVec = ActorVec.Left;
                }
                else if (rotVector.x == 1)
                {
                    Quaternion rotationRight = Quaternion.LookRotation(Vector3.right);
                    targetObject.rotation = Quaternion.Slerp(targetObject.rotation, rotationRight, Time.deltaTime * turnSpeed);
                    _actorVec = ActorVec.Right;
                }
                else if (rotVector.y == 1)
                {
                    Quaternion rotationUp = Quaternion.LookRotation(Vector3.zero);
                    targetObject.rotation = Quaternion.Slerp(targetObject.rotation, rotationUp, Time.deltaTime * turnSpeed);
                    _actorVec = ActorVec.Up;
                }
                return _actorVec;
            }
            /// <summary>
            /// プレイヤーの移動(RaycastHit版)
            /// </summary>
            /// <param name="h"></param>
            /// <param name="moveSpeed"></param>
            /// <param name="hitNormal">地面の法線ベクトル</param>
            /// <returns></returns>
            public Vector3 ActorMoveMethod(float h, float moveSpeed, RaycastHit hitNormal)
            {
                Vector3 velo = Vector3.zero;
                if (hitNormal.collider)
                {
                    Vector3 normalVector = hitNormal.normal;
                    Vector3 onPlane = Vector3.ProjectOnPlane(new Vector3(h, 0f, 0f), normalVector);
                    _velo.x = onPlane.x * moveSpeed;
                    _velo.y = onPlane.y * moveSpeed;
                    //Debug.Log(onPlane.y);
                    if (Mathf.Abs(onPlane.y) <= 0.01f || Mathf.Abs(onPlane.y) > 1f && _ableJumpInput)
                    {
                        velo = new Vector3(_velo.x, 0f, 0f);
                    }
                    else if (Mathf.Abs(onPlane.y) > 0.01f && _ableJumpInput)
                    {
                        velo = new Vector3(_velo.x, _velo.y, 0f);
                    }
                }
                else
                {
                    velo = new Vector3(h * moveSpeed, 0f, 0f);
                }
                return velo;
            }

            /// <summary>
            /// プレイヤーの移動(RaycastHit版)
            /// </summary>
            /// <param name="h">入力方向</param>
            /// <param name="moveSpeed">移動速度</param>
            /// <param name="normalVector">地面の法線ベクトル</param>
            /// <returns></returns>
            public Vector3 ActorMoveMethod(float h, float moveSpeed, Vector3 normalVector, bool ableInput = true)
            {
                if (!ableInput) return Vector3.zero;

                Vector3 velo = Vector3.zero;
                Vector3 onPlane = Vector3.ProjectOnPlane(new Vector3(h, 0f, 0f), normalVector);
                _velo.x = onPlane.x * moveSpeed;
                _velo.y = onPlane.y * moveSpeed;
                //Debug.Log(onPlane.y);
                if (Mathf.Abs(onPlane.y) <= 0.01f || Mathf.Abs(onPlane.y) > 1f && _ableJumpInput)
                {
                    velo = new Vector3(_velo.x, 0f, 0f);
                }
                else if (Mathf.Abs(onPlane.y) > 0.01f && _ableJumpInput)
                {
                    velo = new Vector3(_velo.x, _velo.y, 0f);
                }
                return velo;
            }
            /// <summary>
            /// プレイヤーのダッシュ
            /// </summary>
            /// <param name="DashPower"></param>
            /// <returns></returns>
            public Vector3 DodgeVec(Vector2 currentVec, float DashPower = 10f)
            {
                return new Vector3(currentVec.x, currentVec.y, 0f) * DashPower;
            }
        }
    }
    namespace Wall
    {
        public class ActorWall
        {
            [Tooltip("")]
            BoolReactiveProperty _clinbing = new();
            [Tooltip("")]
            bool _slideWall = false;
            [Tooltip("")]
            bool _ableJumpInput = true;
            [Tooltip("")]
            Vector2 _velo = Vector2.zero;

            public IReadOnlyReactiveProperty<bool> Climbing => _clinbing;

            /// <summary>
            /// 壁張り付き中の挙動
            /// </summary>
            /// <param name="jump"></param>
            /// <param name="wallSlideSpeed"></param>
            /// <param name="wallJumpPower"></param>
            /// <param name="rb"></param>
            /// <param name="hitInfo"></param>
            /// <param name="stateOfPlayer"></param>
            public void ActorSlideWall(float wallSlideSpeed, Rigidbody rb, RaycastHit hitInfo, StateOfPlayer stateOfPlayer)
            {
                switch (stateOfPlayer)
                {
                    case StateOfPlayer.GripingWall:
                        if (!_slideWall)
                        {
                            rb.isKinematic = true;
                            _slideWall = true;
                            rb.isKinematic = false;
                        }
                        rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
                        break;
                    default:
                        if (_slideWall)
                        {
                            _slideWall = false;
                        }
                        break;
                }
            }



            /// <summary>
            /// 壁のよじ登り
            /// </summary>
            /// <param name="rb"></param>
            /// <param name="hitInfo"></param>
            /// <param name="duration"></param>
            public void ClimbWall(Rigidbody rb, RaycastHit hitInfo, float duration = 0.5f)
            {
                //float time = 0;
                //Vector3 startPoint = rb.transform.position;

                //_animator.SetBool("Climbing", true);
                if (!_clinbing.Value && hitInfo.collider.TryGetComponent(out BoxCollider col1))
                {
                    _slideWall = false;
                    Vector3 endPoint = hitInfo.transform.position + new Vector3(0f, col1.size.y * hitInfo.transform.localScale.y, 0f);

                    rb.transform.DOMove(endPoint, duration)
                        .OnStart(() =>
                        {
                            rb.isKinematic = true;
                            _clinbing.Value = true;
                            _ableJumpInput = false;
                        })
                        .OnComplete(() =>
                        {
                            rb.transform.position = endPoint;
                            _clinbing.Value = false;
                            _ableJumpInput = true;
                            rb.isKinematic = false;
                        });
                }

                //while (time < duration)
                //{
                //    rb.transform.position = Vector3.Lerp(startPoint, endPoint, time / duration);
                //    time += Time.deltaTime;
                //}
                //_animator.SetBool("Climbing", false);
                //_animator.SetBool("WallGrip", false);
                //rb.transform.position = endPoint;
                //_clinbing = false;
                //_ableInput = true;
                //rb.isKinematic = false;
            }
        }
    }
}
