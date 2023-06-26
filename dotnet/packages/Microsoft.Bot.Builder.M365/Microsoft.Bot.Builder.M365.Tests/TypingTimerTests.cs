﻿using Microsoft.Bot.Schema;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Bot.Builder.M365.Tests
{
    public class TypingTimerTests
    {

        [Fact]
        public void Start_MessageActivityType()
        {
            // Arrange
            var botAdapterStub = Mock.Of<BotAdapter>();
            var turnContextMock = new Mock<TurnContext>(botAdapterStub, new Activity { Type = ActivityTypes.Message });

            int timerDelay = 1000;
            TypingTimer typingTimer = new TypingTimer(timerDelay);

            // Act
            typingTimer.Start(turnContextMock.Object);

            // Assert
            Assert.True(typingTimer.IsRunning());
        }

        [Fact]
        public void Start_DoubleStart_ShouldFail()
        {
            // Arrange
            var botAdapterStub = Mock.Of<BotAdapter>();
            var turnContextMock = new Mock<TurnContext>(botAdapterStub, new Activity { Type = ActivityTypes.Message });

            int timerDelay = 1000;
            TypingTimer typingTimer = new TypingTimer(timerDelay);

            // Act
            typingTimer.Start(turnContextMock.Object);

            // Assert
            Assert.False(typingTimer.Start(turnContextMock.Object));
        }

        [Fact]
        public void Start_NotMessageActivityType()
        {
            // Arrange
            var botAdapterStub = Mock.Of<BotAdapter>();
            var notMessageActivityType = ActivityTypes.Invoke;
            var turnContextMock = new Mock<TurnContext>(botAdapterStub, new Activity { Type = notMessageActivityType });

            int timerDelay = 1000;
            TypingTimer typingTimer = new TypingTimer(timerDelay);

            // Act
            typingTimer.Start(turnContextMock.Object);

            // Assert
            Assert.False(typingTimer.IsRunning());
        }

        [Fact]
        public void Start_Registers_OnSendActivites_EventHandler()
        {
            // Arrange
            var botAdapterStub = Mock.Of<BotAdapter>();
            var turnContextMock = new Mock<ITurnContext>();
            turnContextMock.Setup(tc => tc.Activity).Returns(new Activity { Type = ActivityTypes.Message });
            turnContextMock.Setup(tc => tc.OnSendActivities(It.IsAny<SendActivitiesHandler>()));

            int timerDelay = 1000;
            TypingTimer typingTimer = new TypingTimer(timerDelay);

            // Act
            typingTimer.Start(turnContextMock.Object);

            // Assert
            turnContextMock.Verify(tc => tc.OnSendActivities(It.IsAny<SendActivitiesHandler>()), Times.Once);

        }

        [Fact]
        public void Start_ShouldSendTypingActivity_OneAtATime()
        {
            // Arrange
            var botAdapterStub = Mock.Of<BotAdapter>();
            var turnContextMock = new Mock<ITurnContext>();
            turnContextMock.Setup(tc => tc.Activity).Returns(new Activity { Type = ActivityTypes.Message });
            turnContextMock.Setup(tc => tc.OnSendActivities(It.IsAny<SendActivitiesHandler>()));
            turnContextMock.Setup(tc => tc.SendActivityAsync(It.IsAny<Activity>(), It.IsAny<CancellationToken>())).Callback(() =>
            {
                // Blocking the thread for 2 seconds to simulate a long running operation
                Thread.Sleep(2000);
            });

            // Sending typing activity 10 times per second
            int timerDelay = 100;
            TypingTimer typingTimer = new TypingTimer(timerDelay);

            // Act
            typingTimer.Start(turnContextMock.Object);

            // In the mean time, simulating sending 10 typing activities
            Thread.Sleep(1000);

            // Assert
            // Only one typing timer is sent
            turnContextMock.Verify(tc => tc.SendActivityAsync(It.IsAny<Activity>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Dispose_ShouldResetProperties()
        {
            // Arrange
            var botAdapterStub = Mock.Of<BotAdapter>();
            var turnContextMock = new Mock<TurnContext>(botAdapterStub, new Activity { Type = ActivityTypes.Message });

            int timerDelay = 1000;
            TypingTimer typingTimer = new TypingTimer(timerDelay);

            // Act
            typingTimer.Start(turnContextMock.Object);
            typingTimer.Dispose();

            // Assert
            Assert.False(typingTimer.IsRunning());
        }
    }
}
