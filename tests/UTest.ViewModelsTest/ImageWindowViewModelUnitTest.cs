using FluentAssertions;
using ImageLyre.UI.ViewModels;
using Xunit;

namespace UTest.ViewModelsTest
{
    public class ImageWindowViewModelUnitTest
    {
        [Fact]
        public void EqualsTest1()
        {
            var vm1 = new ImageViewModel();
            var vm2 = new ImageViewModel();
            vm1.Equals(vm2).Should().BeFalse();
        }

        [Fact]
        public void EqualsTest2()
        {
            var path1 = @"assets\3d44.jpg";
            var path2 = @"assets\24d5.jpg";
            var vm1 = new ImageViewModel();
            vm1.Read(path1);
            var vm2 = new ImageViewModel();
            vm2.Read(path2);
            vm1.Equals(vm2).Should().BeFalse();
        }

        [Fact]
        public void EqualsTest3()
        {
            var path1 = @"assets\3d44.jpg";
            var path2 = @"assets\24d5.jpg";
            var vm1 = new ImageViewModel();
            vm1.Read(path2);
            var vm2 = new ImageViewModel();
            vm2.Read(path2);
            vm1.Equals(vm2).Should().BeTrue();
        }

        [Fact]
        public void EqualsTest4()
        {
            var path1 = @"assets\3d44.jpg";
            var path2 = @"assets\24d5.jpg";
            var vm1 = new ImageViewModel();
            vm1.Read(path1);
            var vm2 = new ImageViewModel();
            vm2.Read(path1);
            vm1.Equals(vm2).Should().BeTrue();
        }
    }
}