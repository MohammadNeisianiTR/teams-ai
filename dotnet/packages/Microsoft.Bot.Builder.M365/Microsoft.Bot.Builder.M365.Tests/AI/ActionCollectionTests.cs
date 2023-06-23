﻿using Microsoft.Bot.Builder.M365.AI.Action;

namespace Microsoft.Bot.Builder.M365.Tests.AI
{
    public class ActionCollectionTests
    {
        [Fact]
        public void Test_Simple()
        {
            // Arrange
            IActionCollection<TurnState> actionCollection = new ActionCollection<TurnState>();
            string name = "action";
            ActionHandler<TurnState> handler = (turnContext, turnState, data, action) => Task.FromResult(true);
            bool allowOverrides = true;

            // Act
            actionCollection.SetAction(name, handler, allowOverrides);
            ActionEntry<TurnState> entry = actionCollection.GetAction(name);

            // Assert
            Assert.True(actionCollection.HasAction(name));
            Assert.NotNull(entry);
            Assert.Equal(name, entry.Name);
            Assert.Equal(handler, entry.Handler);
            Assert.Equal(allowOverrides, entry.AllowOverrides);
        }

        [Fact]
        public void Test_Set_NonOverridable_Action_Throws_Exception()
        {
            // Arrange
            IActionCollection<TurnState> actionCollection = new ActionCollection<TurnState>();
            string name = "action";
            ActionHandler<TurnState> handler = (turnContext, turnState, data, action) => Task.FromResult(true);
            bool allowOverrides = false;
            actionCollection.SetAction(name, handler, allowOverrides);

            // Act
            var func = () => actionCollection.SetAction(name, handler, allowOverrides);

            // Assert
            Exception ex = Assert.Throws<ArgumentException>(() => func());
            Assert.Equal($"Action {name} already exists and does not allow overrides", ex.Message);
        }

        [Fact]
        public void Test_Get_NonExistent_Action()
        {
            // Arrange
            IActionCollection<TurnState> actionCollection = new ActionCollection<TurnState>();
            var nonExistentAction = "non existent action";

            // Act
            var func = () => actionCollection.GetAction(nonExistentAction);

            // Assert
            Exception ex = Assert.Throws<ArgumentException>(() => func());
            Assert.Equal($"`{nonExistentAction}` action does not exist", ex.Message);
        }

        [Fact]
        public void Test_HasAction_False()
        {
            // Arrange
            IActionCollection<TurnState> actionCollection = new ActionCollection<TurnState>();
            var nonExistentAction = "non existent action";

            // Act
            bool hasAction = actionCollection.HasAction(nonExistentAction);

            // Assert
            Assert.False(hasAction);
        }
    }
}