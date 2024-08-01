using MarketSimulator.Market;

namespace MarketSimulatorTest
{
    public class Tests
    {
        private readonly Market _market = new Market();
        private List<Seller> _sellers;
        private List<Buyer> _buyers;

        [SetUp]
        public void Setup()
        {
            _sellers = new List<Seller>
            {
                new Seller("Tom LLC"),
                new Seller("Jerry LLC"),
            };
            _buyers = new List<Buyer>
            {
                new Buyer("John Doe"),
                new Buyer("Jane Doe"),
            };

            _market.AddSellers(_sellers);
            _market.AddBuyers(_buyers);
        }

        [Test]
        public void PerformActions_Should_UpdateInflationHistory()
        {
            _market.PerformActions();
            
            Assert.That(MarketState.InflationHistory, Has.Count.EqualTo(7));
        }

        [Test]
        public void PerformActions_Should_UpdateIncomeHistory()
        {
            var seller = new Seller("Seller1");
            var buyer = new Buyer("Buyer1");

            _market.AddSeller(seller);
            _market.AddBuyer(buyer);
            
            _market.PerformActions();
            
            Assert.That(CentralBank.IncomeHistory, Has.Count.GreaterThan(0));
        }

        [Test]
        public void AddSeller_Should_AddSellerToMarket()
        {
            var seller = new Seller("Seller1");
            
            _market.AddSeller(seller);
            
            Assert.That(Market.Sellers, Has.Count.EqualTo(7));
            Assert.That(Market.Sellers.Last(), Is.EqualTo(seller));
        }

        [Test]
        public void AddBuyer_Should_AddBuyerToMarket()
        {
            var buyer = new Buyer("Buyer1");

            _market.AddBuyer(buyer);
            
            Assert.That(Market.Buyers, Has.Count.EqualTo(3));
            Assert.That(Market.Buyers.Last(), Is.EqualTo(buyer));
        }

        [Test]
        public void AddSellers_Should_AddMultipleSellersToMarket()
        {
            var sellers = new[]
            {
                new Seller("Seller1"),
                new Seller("Seller2"),
                new Seller("Seller3")
            };
            
            _market.AddSellers(sellers);
            
            Assert.That(Market.Sellers, Has.Count.EqualTo(12));
        }

        [Test]
        public void AddBuyers_Should_AddMultipleBuyersToMarket()
        {
            var buyers = new[]
            {
                new Buyer("Buyer1"),
                new Buyer("Buyer2"),
                new Buyer("Buyer3")
            };
            
            _market.AddBuyers(buyers);
            
            Assert.That(Market.Buyers, Has.Count.EqualTo(8));
        }
        
        [Test]
        public void PerformActions_Should_UpdateBuyersDesiredProducts()
        {
            var buyer = new Buyer("Buyer1");
            
            _market.PerformActions();
            
            Assert.That(_buyers[0].DesiredProducts, Has.Count.GreaterThan(0));
            Assert.That(_buyers[1].DesiredProducts, Has.Count.GreaterThan(0));
        }

        [Test]
        public void PerformActions_Should_UpdateSellersProducts()
        {
            var seller = new Seller("Seller1");
            
            _market.AddSeller(seller);
            _market.PerformActions();
            
            Assert.Multiple(() =>
            {
                Assert.That(_sellers[0].Products, Has.Count.GreaterThan(0));
                Assert.That(_sellers[1].Products, Has.Count.GreaterThan(0));
                Assert.That(seller.Products, Has.Count.GreaterThan(0));
            });
        }

        [Test]
        public void PerformActions_Should_UpdateBuyersMoney()
        {
            var buyer = new Buyer("Buyer1");
            
            _market.AddBuyer(buyer);
            
            _market.PerformActions();
            
            Assert.That(_buyers[0].Money, Is.GreaterThan(0));
            Assert.That(_buyers[1].Money, Is.GreaterThan(0));
            Assert.That(buyer.Money, Is.GreaterThan(0));
        }
        
        [Test]
        public void PerformActions_Should_UpdateBuyersIncome()
        {
            _market.PerformActions();

            var income = _buyers[0].Income;
            
            _market.PerformActions();
            
            Assert.That(_buyers[0].Income, Is.Not.EqualTo(income));
        }
    }
}
