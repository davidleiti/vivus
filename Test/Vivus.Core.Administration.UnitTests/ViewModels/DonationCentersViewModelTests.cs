namespace Vivus.Core.Administration.UnitTests.ViewModels
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Vivus.Core.Administration.ViewModels;
    using VivusDataModels = Core.DataModels;
    using Vivus.Core.Model;
    using Vivus.Core.UnitTests.Dependencies;
    using Vivus.Core.UnitTests.Dependencies.UoW;
    using Vivus.Core.ViewModels.Base;

    /// <summary>
    /// Summary description for DonationCentersViewModelTests
    /// </summary>
    [TestClass]
    public class DonationCentersViewModelTests
    {
        private UnitOfWork unitOfWork;
        private Mock<IApplicationViewModel<Administrator>> appViewModel;
        private DispatcherWrapper dispatcherWrapper;
        private Security security;
        private ParentPage parentPage;
        private DonationCentersViewModel viewModel;

        public void Setup()
        {
            unitOfWork = new UnitOfWork();
            appViewModel = new Mock<IApplicationViewModel<Administrator>>();
            dispatcherWrapper = new DispatcherWrapper();
            parentPage = new ParentPage();
            security = new Security();
            viewModel = new DonationCentersViewModel(unitOfWork, appViewModel.Object, dispatcherWrapper, security)
            {
                ParentPage = parentPage
            };
        }

        // Convention: MethodsName_Scenario_ExpectedBehaviour
        // Generic scenario: WhenCalled
        [TestMethod]
        public void AddModify_OnAddEmpty_ReturnsZero()
        {
            viewModel.AddModifyAsync().Wait();

            Assert.AreEqual(0, viewModel.DonationCenters.Count);
        }

        // Convention: MethodsName_Scenario_ExpectedBehaviour
        // Generic scenario: WhenCalled
        [TestMethod]
        public void AddModify_OnAdd_ReturnsOne()
        {
            viewModel.DonationCenterName = "DonationCenter1";

            viewModel.ResidencyAddress.County = new VivusDataModels.BasicEntity<string>(0, "County");
            viewModel.ResidencyAddress.City = "City";
            viewModel.ResidencyAddress.StreetName = "Streetname";
            viewModel.ResidencyAddress.StreetNumber = "StreetNumber";

            viewModel.AddModifyAsync().Wait();

            Assert.AreEqual(1, viewModel.DonationCenters.Count);
        }

    }
}
