using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using ConsoleApplication1;
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
            Assert.That(gameWorld.Locations.First().Value.ToString(), Is.EqualTo("YOU ARE STANDING AT THE END OF A ROAD BEFORE A SMALL BRICK BUILDING."));
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
            Assert.That(gameWorld.Locations.First().Value.ToString(), Is.EqualTo("PREAMBLE.\r\nPOSTAMBLE."));
        }

        [Test]
        public void Parse_ShortFormPresent_MapsToLongDescription()
        {
            FileDataIs("1	Long form" + Rn +
                       "-1" + Rn +
                       "1	Short form" + Rn +
                       "-1" + Rn +
                       "1	2	2	44	29");

            var gameWorld = _parser.Parse(AdvenDat);

            Assert.That(gameWorld.Locations.First().Value.ShortForm, Is.EqualTo("Short form"));
        }

        [Test]
        public void Parse_DirectionsPresent_AssignsThemToLocations()
        {
            FileDataIs("1	A" + Rn +
                       "2	B" + Rn +
                       "-1" + Rn +
                       "1	A" + Rn +
                       "-1" + Rn +
                       "1	2	1	2	3");
                      //loc, dest, verb, verb, verb

            var gameWorld = _parser.Parse(AdvenDat);
            var locationDescription = gameWorld.Locations.First().Value;

            Assert.That(locationDescription.Paths[0].LocationId, Is.EqualTo(2));
            Assert.That(locationDescription.Paths[0].Action[0], Is.EqualTo("1"));
            Assert.That(locationDescription.Paths[0].Action[1], Is.EqualTo("2"));
            Assert.That(locationDescription.Paths[0].Action[2], Is.EqualTo("3"));
        }

        private void FileDataIs(string contents)
        {
            _mockFileData = new MockFileData(contents);
            _fs.AddFile(AdvenDat, _mockFileData);
        }
    }
}
