using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleState
{
    //ステート実行クラス
    public class StateProcessor
    {
        private BubbleState _state;
        public BubbleState state
        {
            set { _state = value; }
            get { return _state; }
        }

        public void StateUpdate()
        {
            _state.StateUpdate();
        }

        public void ChangeState(BubbleState state)
        {

        }
    }

    public abstract class BubbleState : MonoBehaviour
    {
        //各ステートが始まってからのフレーム数
        protected int _stateTimer;
        public virtual void StateStart()
        {

        }
        public virtual void StateUpdate()
        {
        }
        //ステート名取得
        public abstract string GetStateName();
    }

    public class FloatingState:BubbleState
    {
        public override void StateStart()
        {
            _stateTimer = 0;
        }
        public override void StateUpdate()
        {
            ++_stateTimer;
        }
        public override string GetStateName()
        {
            return "Floating";
        }
    }
    public class BurstState:BubbleState
    {
        public override void StateStart()
        {
            _stateTimer = 0;
        }
        public override void StateUpdate()
        {
            ++_stateTimer;
        }

        public override string GetStateName()
        {
            return "Burst";
        }
    }
}