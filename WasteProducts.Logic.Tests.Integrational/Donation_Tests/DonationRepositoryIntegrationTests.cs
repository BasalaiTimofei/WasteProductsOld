using Ninject;
using NUnit.Framework;
using WasteProducts.DataAccess.Repositories.Donations;
using WasteProducts.DataAccess.Common.Repositories.Donations;
using WasteProducts.DataAccess.Common.Models.Donations;
using System;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Tests.Donation_Tests
{
    [TestFixture]
    class DonationRepositoryIntegrationTests
    {
        private readonly StandardKernel _kernel = new StandardKernel();
        private readonly IDonationRepository _donationRepository;

        public DonationRepositoryIntegrationTests()
        {
            _kernel.Load(new DataAccess.InjectorModule(), new Logic.InjectorModule());
            _donationRepository = _kernel.Get<IDonationRepository>();
            ((DonationRepository)_donationRepository).RecreateTestDatabase();
        }

        [Test]
        public async Task _00CreateNewDonationAsync()
        {
            DonationDB donation = new DonationDB()
            {
                Currency = "USD",
                Date = DateTime.Now,
                Fee = 0.14M,
                Gross = 1.0M,
                TransactionId = "1",
                Donor = CreateDonorWithLondonAddress()
            };
            await AddAndAssertAsync(donation).ConfigureAwait(false);
        }

        [Test]
        public async Task _01CreateNewDonationFromSameDonorAsync()
        {
            DonationDB donation = new DonationDB()
            {
                Currency = "USD",
                Date = DateTime.Now,
                Fee = 0.15M,
                Gross = 2.0M,
                TransactionId = "2",
                Donor = CreateDonorWithLondonAddress()
            };
            await AddAndAssertAsync(donation).ConfigureAwait(false);
        }

        [Test]
        public async Task _02CreateNewDonationFromChangedDonorWithUnmodifiedAddressAsync()
        {
            DonorDB donor = CreateDonorWithLondonAddress();
            donor.IsVerified = true;
            DonationDB donation = new DonationDB()
            {
                Currency = "USD",
                Date = DateTime.Now,
                Fee = 0.15M,
                Gross = 3.0M,
                TransactionId = "3",
                Donor = donor
            };
            await AddAndAssertAsync(donation).ConfigureAwait(false);
        }

        [Test]
        public async Task _03CreateNewDonationFromDonorWithNewAddress_OldAddressIsNotUsedAsync()
        {
            DonorDB donor = CreateDonorWithLondonAddress();
            donor.Address = CreateAmsterdamAddress();
            DonationDB donation = new DonationDB()
            {
                Currency = "USD",
                Date = DateTime.Now,
                Fee = 0.15M,
                Gross = 4.0M,
                TransactionId = "4",
                Donor = donor
            };
            await AddAndAssertAsync(donation).ConfigureAwait(false);
        }

        [Test]
        public async Task _04CreateNewDonationFromOtherDonorWithSameAddressAsync()
        {
            DonationDB donation = new DonationDB()
            {
                Currency = "USD",
                Date = DateTime.Now,
                Fee = 0.15M,
                Gross = 5.0M,
                TransactionId = "5",
                Donor = CreateDonorWithAmsterdamAddress()
            };
            await AddAndAssertAsync(donation).ConfigureAwait(false);
        }

        [Test]
        public async Task _05CreateNewDonationFromDonorWithNewAddress_OldAddressIsUsedAsync()
        {
            DonorDB donor = CreateDonorWithLondonAddress();
            DonationDB donation = new DonationDB()
            {
                Currency = "USD",
                Date = DateTime.Now,
                Fee = 0.15M,
                Gross = 6.0M,
                TransactionId = "6",
                Donor = donor
            };
            await AddAndAssertAsync(donation).ConfigureAwait(false);
        }

        [Test]
        public async Task _06CreateNewDonationFromDonorWithChangedButExistAddress_OldAddressIsNotUsedAsync()
        {
            DonorDB donor = CreateDonorWithAmsterdamAddress();
            donor.Address = CreateLondonAddress();
            DonationDB donation = new DonationDB()
            {
                Currency = "USD",
                Date = DateTime.Now,
                Fee = 0.15M,
                Gross = 7.0M,
                TransactionId = "7",
                Donor = donor
            };
            await AddAndAssertAsync(donation).ConfigureAwait(false);
        }

        [OneTimeTearDown]
        public void LastTearDown()
        {
            _donationRepository?.Dispose();
            _kernel?.Dispose();
        }

        private AddressDB CreateAmsterdamAddress()
        {
            const string AMSTERDAM = "Amsterdam";
            return new AddressDB()
            {
                City = AMSTERDAM,
                State = AMSTERDAM,
                Country = "Netherlands",
                IsConfirmed = false,
                Name = "Inc.",
                Street = "Avenue",
                Zip = "600100"
            };
        }

        private AddressDB CreateLondonAddress()
        {
            const string LONDON = "London";
            return new AddressDB()
            {
                City = LONDON,
                Country = "United Kingdom",
                IsConfirmed = true,
                Name = "Test name",
                State = LONDON,
                Street = "Down Town",
                Zip = "100300"
            };
        }

        private DonorDB CreateDonorWithLondonAddress()
        {
            
            return new DonorDB()
            {
                Email = "JohnNewton@gmail.com",
                FirstName = "John",
                LastName = "Newton",
                Id = "1",
                IsVerified = false,
                Address = CreateLondonAddress()
            };
        }

        private DonorDB CreateDonorWithAmsterdamAddress()
        {
            return new DonorDB()
            {
                Email = "TomSnow@gmail.com",
                FirstName = "Tom",
                LastName = "Snow",
                Id = "2",
                IsVerified = false,
                Address = CreateAmsterdamAddress()
            };
        }

        private async Task AddAndAssertAsync(DonationDB donation)
        {
            await _donationRepository.AddAsync(donation).ConfigureAwait(false);
            Assert.IsTrue(await _donationRepository.ContainsAsync(donation.TransactionId).ConfigureAwait(false));
        }
    }
}