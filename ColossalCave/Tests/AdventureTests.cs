using System.Collections.Generic;
using System.Linq;
using ConsoleApplication1.GameModel;
using ConsoleApplication1.GameModel.Actions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class AdventureTests
    {
        private Adventure _adv;

        [SetUp]
        public void SetUp()
        {
            _adv = new Adventure
            {
                Locations = new Dictionary<int, Location>
                {
                    {
                        1, new Location(1).WithActions(a => a.Add(new Command
                        {
                            TargetId = 2,
                            Triggers = new List<string> {"Move"},
                            Action = new NavigateAction()
                        }))
                    },
                    {
                        2, new Location(2).WithActions(a => a.Add(new Command
                        {
                            TargetId = 1,
                            Triggers = new List<string> {"Back"},
                            Action = new NavigateAction()
                        }))
                    },
                }
            }.StartGame();
        }

        [Test]
        public void ProcessInput_ValidInput_InvokesAppropriateAction()
        {
            _adv.ProcessInput("Move");

            Assert.That(_adv.CurrentLocation, Is.EqualTo(2));
        }

        [Test]
        public void ProcessInput_InvalidInput_DoesNotUnderstand()
        {
            var output = _adv.ProcessInput("Rubbish");

            Assert.That(output.First(), Is.EqualTo("I don't understand that."));
        }
    }
}