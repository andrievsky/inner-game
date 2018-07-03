using System;
using Code.Common.StateMachine;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace Code.Editor.Tests.StateMachine
{
    public class StateMachineTest
    {
        class EmptyTestState : AbstractState
        {
            public override void DidEnter(IState state)
            {  
            }

            public override void Update(int deltaTime)
            {   
            }

            public override void WillExit(IState state)
            {   
            }
        }
        
        [Test]
        public void InitTest()
        {
            var sm = new SyncStateMachine();
            var state1 = new Mock<IState>();
            var state2 = new Mock<IState>();
            sm.Init(new[] {state1.Object, state2.Object});
            state1.Verify(s => s.DidEnter(null), Times.Once());
            state1.VerifyAll();
            Assert.AreEqual(state1.Object, sm.CurrentState);
        }
        
        [Test]
        public void EnterTest()
        {
            var sm = new SyncStateMachine();
            var state1 = new Mock<IState>();
            state1.Setup(s => s.IsValidNextState(It.IsAny<Type>())).Returns(true);
            var state2 = new EmptyTestState();
            sm.Init(new[] {state1.Object, state2});
            Assert.AreEqual(state1.Object, sm.CurrentState);
            sm.Enter(typeof(EmptyTestState));
            Assert.AreEqual(state2, sm.CurrentState);
            state1.Verify(s => s.DidEnter(null), Times.Once());
            state1.Verify(s => s.WillExit(state2), Times.Once());
            state1.VerifyAll();
        }
        
        [Test]
        public void ChangeStateTest()
        {
            var sm = new SyncStateMachine();
            var state1 = new Mock<IState>();
            state1.Setup(s => s.IsValidNextState(It.IsAny<Type>())).Returns(true);
            var state2 = new EmptyTestState();
            sm.Init(new[] {state1.Object, state2});
            Assert.AreEqual(state1.Object, sm.CurrentState);
            sm.Enter(typeof(EmptyTestState));
            Assert.AreEqual(state2, sm.CurrentState);
            sm.Enter(state1.Object.GetType());
            Assert.AreEqual(state1.Object, sm.CurrentState);
            state1.Verify(s => s.DidEnter(null), Times.Once());
            state1.Verify(s => s.DidEnter(state2), Times.Once());
            state1.Verify(s => s.WillExit(state2), Times.Once());
            state1.VerifyAll();
        }
    }
}