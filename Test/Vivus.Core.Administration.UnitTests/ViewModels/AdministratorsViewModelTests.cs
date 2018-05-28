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
    /// Represents a testing class for the administration viewmodel.
    /// </summary>
    [TestClass]
    public class AdministratorsViewModelTests
    {
        private UnitOfWork unitOfWork;
        private Mock<IApplicationViewModel<Administrator>> appViewModel;
        private DispatcherWrapper dispatcherWrapper;
        private Security security;
        private ParentPage parentPage;
        private AdministratorsViewModel viewModel;

        /// <summary>
        /// Called before each test. Puts the viewmodel in a clean state.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            unitOfWork = new UnitOfWork();
            appViewModel = new Mock<IApplicationViewModel<Administrator>>();
            dispatcherWrapper = new DispatcherWrapper();
            parentPage = new ParentPage();
            security = new Security();
            viewModel = new AdministratorsViewModel(unitOfWork, appViewModel.Object, dispatcherWrapper, security)
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

            Assert.AreEqual(0, viewModel.Administrators.Count);
        }

        // Convention: MethodsName_Scenario_ExpectedBehaviour
        // Generic scenario: WhenCalled
        [TestMethod]
        public void AddModify_OnAdd_ReturnsOne()
        {
            viewModel.Email = "you@email.com";
            viewModel.Person.FirstName = "FirstName";
            viewModel.Person.LastName = "LastName";
            viewModel.Person.BirthDate = "1/1/1";
            viewModel.Person.NationalIdentificationNumber = "1234567890123";
            viewModel.Person.PhoneNumber = "012345678";
            viewModel.Person.Gender = new VivusDataModels.BasicEntity<string>(0, "Gender");
            viewModel.Address.County = new VivusDataModels.BasicEntity<string>(0, "County");
            viewModel.Address.City = "City";
            viewModel.Address.StreetName = "Streetname";
            viewModel.Address.StreetNumber = "StreetNumber";

            viewModel.AddModifyAsync().Wait();

            Assert.AreEqual(1, viewModel.Administrators.Count);
        }
    }
}
