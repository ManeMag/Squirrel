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
    }
}
