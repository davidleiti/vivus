using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vivus.Core.DCPersonnel.UnitTests.ViewModels
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Vivus.Core.DCPersonnel.ViewModels;
    using VivusDataModels = Core.DataModels;
    using Vivus.Core.Model;
    using Vivus.Core.UnitTests.Dependencies;
    using Vivus.Core.UnitTests.Dependencies.UoW;
    using Vivus.Core.ViewModels.Base;

    [TestClass]
    public class BloodDonationRequestsViewModelTest
    {
        private UnitOfWork unitOfWork;
        private Mock<IApplicationViewModel<DCPersonnel>> appViewModel;
        private DispatcherWrapper dispatcherWrapper;
        private Security security;
        private ParentPage parentPage;
        private BloodDonationRequestsViewModel viewModel;

        /// <summary>
        /// Called before each test. Puts the viewmodel in a clean state.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            unitOfWork = new UnitOfWork();
            appViewModel = new Mock<IApplicationViewModel<DCPersonnel>>();
            dispatcherWrapper = new DispatcherWrapper();
            parentPage = new ParentPage();
            security = new Security();
            viewModel = new BloodDonationRequestsViewModel(unitOfWork, appViewModel.Object, dispatcherWrapper, security)
            {
                Parentpage = parentPage
            };
            
        }

        // Convention: MethodsName_Scenario_ExpectedBehaviour
        // Generic scenario: WhenCalled
        [TestMethod]
        public void ApproveOrRejectDonation_OnNoSelection_ReturnsAll()
        {
            viewModel.BloodDonationRequestItems.Add(new BloodDonationRequestItem
            {
                Id = 0,
                FullName = "Full Name",
                Age = 20,
                NationalIdentificationNumber = ""
            });

            viewModel.ApproveOrRejectDonation(true).Wait();

            Assert.AreEqual(1, viewModel.BloodDonationRequestItems.Count);
        }

        // Convention: MethodsName_Scenario_ExpectedBehaviour
        // Generic scenario: WhenCalled
        [TestMethod]
        public void ApproveOrRejectDonation_OnSelected_ReturnsZero()
        {
            viewModel.BloodDonationRequestItems.Clear();
            viewModel.BloodDonationRequestItems.Add(new BloodDonationRequestItem
            {
                Id = 0,
                FullName = "Full Name",
                Age = 20,
                NationalIdentificationNumber = ""
            });
            viewModel.Messages = "aaaaaaaaaaaaaa";
            viewModel.SelectedBloodDonationRequestItem = viewModel.BloodDonationRequestItems[0];
            
            viewModel.ApproveOrRejectDonation(true).Wait();

            Assert.AreEqual(0, viewModel.Errors);
        }
    }
}
