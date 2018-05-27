using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Vivus.Core.DataModels;
using Vivus.Core.Donor.ViewModels;
using Vivus.Core.UnitTests.Dependencies;
using Vivus.Core.UnitTests.Dependencies.UoW;
using Vivus.Core.ViewModels.Base;
using VivusDataModels = Vivus.Core.DataModels;


namespace Vivus.Core.Donor.UnitTests.ViewModels
{
    [TestClass]
    public class ProfileViewModelTests
    {
        private UnitOfWork unitOfWork;
        private Mock<IApllicationViewModel<Model.Donor>> appViewModel;
        private Core.UnitTests.Dependencies.DispatcherWrapper dispatcherWrapper;
        private Security.Security security;
        private ParentPage parentPage;
        private ProfileViewModel viewModel;
        Model.Donor donor;

        /// <summary>
        /// Called before each test. Puts the viewmodel in a clean state.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            unitOfWork = new UnitOfWork();
            appViewModel = new Mock<IApllicationViewModel<Model.Donor>>();
            dispatcherWrapper = new Core.UnitTests.Dependencies.DispatcherWrapper();
            parentPage = new ParentPage();
            security = new Security.Security();
            viewModel = new ProfileViewModel(unitOfWork, appViewModel.Object, dispatcherWrapper, security)
            {
                ParentPage = parentPage
            };
            donor = new Model.Donor();
        }
     
        [TestMethod]
        public void Update_OnUpdate_DonorUpdated()
        {
            viewModel.Email = "you@email.com";
            viewModel.SelectedDonationCenter = new BasicEntity<string>(10, "DonationCenter");

            viewModel.Person.FirstName = "FirstName";
            viewModel.Person.LastName = "LastName";
            viewModel.Person.BirthDate = "1/1/1";
            viewModel.Person.NationalIdentificationNumber = "1234567890123";
            viewModel.Person.PhoneNumber = "012345678";
            viewModel.Person.Gender = new BasicEntity<string>(0, "Gender");

            viewModel.IdentificationCardAddress.County = new BasicEntity<string>(0, "County");
            viewModel.IdentificationCardAddress.City = "City";
            viewModel.IdentificationCardAddress.StreetName = "Streetname";
            viewModel.IdentificationCardAddress.StreetNumber = "StreetNumber";

            viewModel.ResidenceAddress.County = new BasicEntity<string>(0, "County");
            viewModel.ResidenceAddress.City = "City";
            viewModel.ResidenceAddress.StreetName = "Streetname";
            viewModel.ResidenceAddress.StreetNumber = "StreetNumber";

            viewModel.UpdatePublicAsync().Wait();

            donor = unitOfWork.Persons[appViewModel.Object.User.PersonID].Donor;
            Assert.AreEqual("LastName", donor.Person.LastName);
        }
    }
}
