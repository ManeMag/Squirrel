using FluentAssertions;
using NetArchTest.Rules;

namespace Squirrel.UnitTests
{
    public class ArchitectureTests
    {
        private const string BusinessLayerNamespace = "Squirrel";
        private const string DataAccessLayerNamespace = "DataAccess";

        [Fact]
        public void DataAccessLayer_ShouldNotHaveDependencies_OnOtherProjects()
        {
            var assembly = typeof(DataAccess.AssemblyReference).Assembly;

            var otherProjects = new[] { BusinessLayerNamespace };

            var result = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void RequestClasses_EndWith_Request()
        {
            var result = Types
                    .InNamespace(BusinessLayerNamespace + ".Requests")
                    .Should()
                    .HaveNameEndingWith("Request")
                    .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void ResponseClasses_EndWith_Response()
        {
            var result = Types
                    .InNamespace(BusinessLayerNamespace + ".Responce")
                    .Should()
                    .HaveNameEndingWith("Responce")
                    .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }
    }
}
