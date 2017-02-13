using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ConsoleApplication1;
using ConsoleApplication1.GameModel;
using ConsoleApplication1.GameModel.Actions;
using ConsoleApplication1.Parsing;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ParserTests
    {
        private Parser _parser;
        private MockFileSystem _fs;
        private MockFileData _mockFileData;
        private const string AdvenDat = @"C:\dev\GamePorts\ColossalCave\ConsoleApplication1\adven.dat";
        private const string Rn = "\r\n";

        private static string FirstAndSecondSection => "1	Long form" + Rn +
                                                       "-1" + Rn +
                                                       "1	Short form" + Rn +
                                                       "-1" + Rn;

        private static string SimpleLocationMap => "1	2	1	2	3";


        [SetUp]
        public void SetUp()
        {
            _fs = new MockFileSystem();
            _parser = new Parser(_fs);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void Parse_PassedNullEmptyOrWhitespaceThrows(string dataPath)
        {
            Assert.Throws<ArgumentNullException>(() => _parser.Parse(dataPath));
        }

        [Test]
        public void Parse_ProvidedFile_ParserKnowsHowBigTheFileIs()
        {
            FileDataIs("1 SOME TEXT");

            var gameWorld = _parser.Parse(AdvenDat);

            Assert.That(gameWorld.RawLength, Is.EqualTo(1));
        }

        [Test]
        public void Parse_ProvidedFile_DividerNotPresentInChunks()
        {
            FileDataIs("1	Blah." + Rn +
                       "-1");

            var gameWorld = _parser.Parse(AdvenDat);

            Assert.That(gameWorld.Locations.Count, Is.EqualTo(1));
        }

        [Test]
        public void Parse_GivenLongFormDescriptions_Detects()
        {
            FileDataIs(@"1	YOU ARE STANDING AT THE END OF A ROAD BEFORE A SMALL BRICK BUILDING." + Rn +
                       "-1" + Rn +
                       "1	YOU'RE AT END OF ROAD AGAIN." + Rn +
                       "-1" + Rn +
                       "1	2	2	44	29");

            var gameWorld = _parser.Parse(AdvenDat);

            Assert.That(gameWorld.Locations.Count, Is.EqualTo(1));
            Assert.That(gameWorld.Locations.First().Value.Description, Is.EqualTo("YOU ARE STANDING AT THE END OF A ROAD BEFORE A SMALL BRICK BUILDING."));
        }

        [Test]
        public void Parse_GivenLongFormDescriptionsSpanningManyLines_Detects()
        {
            FileDataIs("1	PREAMBLE." + Rn +
                       "1	POSTAMBLE." + Rn +
                       "-1" + Rn +
                       "1	YOU'RE AT END OF ROAD AGAIN." + Rn +
                       "-1" + Rn +
                       "1	2	2	44	29");

            var gameWorld = _parser.Parse(AdvenDat);

            Assert.That(gameWorld.Locations.Count, Is.EqualTo(1));
            Assert.That(gameWorld.Locations.First().Value.Description, Is.EqualTo("PREAMBLE.\r\nPOSTAMBLE."));
        }

        [Test]
        public void Parse_ShortFormPresent_MapsToLongDescription()
        {
            FileDataIs("1	Long form" + Rn +
                       "-1" + Rn +
                       "1	Short form" + Rn +
                       "-1" + Rn + SimpleLocationMap);

            var gameWorld = _parser.Parse(AdvenDat);

            Assert.That(gameWorld.Locations.First().Value.ShortForm, Is.EqualTo("Short form"));
        }

        [Test]
        public void Parse_DirectionsPresent_AssignsThemToLocations()
        {
            FileDataIs(FirstAndSecondSection +
                       "1	2	1	2	3");
                      //loc, dest, verb, verb, verb

            var gameWorld = _parser.Parse(AdvenDat);
            var locationDescription = gameWorld.Locations.First().Value;

            Assert.That(locationDescription.Actions[0].TargetId, Is.EqualTo(2));
            Assert.That(locationDescription.Actions[0].Triggers[0].Id, Is.EqualTo(1));
            Assert.That(locationDescription.Actions[0].Triggers[1].Id, Is.EqualTo(2));
            Assert.That(locationDescription.Actions[0].Triggers[2].Id, Is.EqualTo(3));
        }

        [TestCase(1)]
        [TestCase(300)]
        public void Parse_TargetIdLessThan300_AssignedToLocation(int targetLocationId)
        {
            FileDataIs(FirstAndSecondSection +
                       $"1	{targetLocationId}	1	2	3");
                      //loc, dest, verb, verb, verb

            var gameWorld = _parser.Parse(AdvenDat);
            var locationDescription = gameWorld.Locations.First().Value;

            Assert.That(locationDescription.Actions[0].TargetId, Is.EqualTo(targetLocationId));
            Assert.That(locationDescription.Actions[0], Is.TypeOf<Navigate>());
        }

        [Test]
        public void Parse_TargetIdGreaterThan300ButLessThan500_IsAJump()
        {
            FileDataIs(FirstAndSecondSection +
                       $"1	301	1	2	3");
                      //loc, dest, verb, verb, verb

            var gameWorld = _parser.Parse(AdvenDat);
            var locationDescription = gameWorld.Locations.First().Value;

            Assert.That(locationDescription.Actions[0].TargetId, Is.EqualTo(1));
            Assert.That(locationDescription.Actions[0], Is.TypeOf<ComputedGoTo>());
        }

        [Test]
        public void Parse_TargetIdGreaterThan500_IsAMessage()
        {
            FileDataIs(FirstAndSecondSection +
                       $"1	501	1	2	3");
                      //loc, dest, verb, verb, verb

            var gameWorld = _parser.Parse(AdvenDat);
            var locationDescription = gameWorld.Locations.First().Value;

            Assert.That(locationDescription.Actions[0].TargetId, Is.EqualTo(1));
            Assert.That(locationDescription.Actions[0], Is.TypeOf<Message>());
        }

        [Test]
        public void Parse_DestinationDivisibleByOneThousand_NoContraints()
        {
            FileDataIs(FirstAndSecondSection +
                       $"1	501	1	2	3");
                      //loc, dest, verb, verb, verb

            var gameWorld = _parser.Parse(AdvenDat);
            var locationDescription = gameWorld.Locations.First().Value;

            Assert.That(locationDescription.Actions[0].Action, Is.TypeOf<NavigateAction>());
        }

        [Test]
        public void Parse_DestinationDividedBy1000Between0And100_ProbabilityConstraint()
        {
            FileDataIs(FirstAndSecondSection +
                       $"1	50005	1	2	3");
                      //loc, dest, verb, verb, verb

            var gameWorld = _parser.Parse(AdvenDat);
            var locationDescription = gameWorld.Locations.First().Value;
            var action = locationDescription.GetAction<NavigateAction>(0);

            Assert.That(action.Constraints[0], Is.TypeOf<PercentageConstraint>());
        }

        [Test]
        public void Parse_DestinationDividedBy1000is100_ForbiddenToDwarfs()
        {
            FileDataIs(FirstAndSecondSection +
                       $"1	100000	1	2	3");
                      //loc, dest, verb, verb, verb

            var gameWorld = _parser.Parse(AdvenDat);
            var locationDescription = gameWorld.Locations.First().Value;
            var action = locationDescription.GetAction<NavigateAction>(0);

            Assert.That(action.Constraints[0], Is.TypeOf<ForbiddenToDwarfsConstraint>());
        }

        [Test]
        public void Parse_DestinationDividedBy1000Between100And200_InventoryConstraint()
        {
            FileDataIs(FirstAndSecondSection +
                       $"1	110000	1	2	3");
                      //loc, dest, verb, verb, verb

            var gameWorld = _parser.Parse(AdvenDat);
            var locationDescription = gameWorld.Locations.First().Value;
            var action = locationDescription.GetAction<NavigateAction>(0);
            
            Assert.That(((InventoryConstraint)action.Constraints[0]).ItemId, Is.EqualTo(10));
        }

        [Test]
        public void Parse_DestinationDividedBy1000Between200And300_PresentConstraint()
        {
            FileDataIs(FirstAndSecondSection +
                       $"1	201000	1	2	3");
                      //loc, dest, verb, verb, verb

            var gameWorld = _parser.Parse(AdvenDat);
            var locationDescription = gameWorld.Locations.First().Value;
            var action = locationDescription.GetAction<NavigateAction>(0);
            
            Assert.That(((ItemOrRoomPresentConstraint)action.Constraints[0]).ItemId, Is.EqualTo(1));
        }

        [TestCase("301", 0)]
        [TestCase("401", 1)]
        [TestCase("501", 2)]
        public void Parse_DestinationDividedBy1000Between300And400_RemainderMustNotBeZero(string mValue, int moduloExpectation)
        {
            FileDataIs(FirstAndSecondSection +
                       $"1	{mValue}000	1	2	3");
                      //loc, dest, verb, verb, verb

            var gameWorld = _parser.Parse(AdvenDat);
            var locationDescription = gameWorld.Locations.First().Value;
            var action = locationDescription.GetAction<NavigateAction>(0);

            Assert.That(((ModuloConstraint)action.Constraints[0]).ModuloResultMustNotBeEqualTo, Is.EqualTo(moduloExpectation));
        }

        private void FileDataIs(string contents)
        {
            _mockFileData = new MockFileData(contents);
            _fs.AddFile(AdvenDat, _mockFileData);
        }
    }
}
