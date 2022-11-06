using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NSubstitute;
using Squirrel.Extensions;

namespace Squirrel.UnitTests
{
    public class ExtensionsTests
    {
        private const string OriginalMessage = "Password does not contain letters";
        private const string ExpectedMessage = "В пароле нету букв";

        private readonly IStringLocalizer _localizer;

        public ExtensionsTests()
        {
            _localizer = Substitute.For<IStringLocalizer>();

            var localizedString = new LocalizedString(OriginalMessage, ExpectedMessage);
            
            _localizer[OriginalMessage].Returns(localizedString);
        }

        [Fact]
        public void Using_ShouldProduceExpectedString()
        {
            OriginalMessage.Using(_localizer).Should().Be(ExpectedMessage);
        }

        [Fact]
        public void ToBadRequestUsing_ShouldProduceTheSameBadRequestObjectResult()
        {
            var expected = new BadRequestObjectResult(new[] { _localizer[OriginalMessage].Value });

            var actual = OriginalMessage.ToBadRequestUsing(_localizer);

            actual.StatusCode.Should().Be(expected.StatusCode);
            (actual.Value as string).Should().Be(expected.Value as string);
        }
    }
}