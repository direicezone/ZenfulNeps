using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TestProject1
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class UnitTest1
	{
		private readonly Mock<ITransactionRetriever> _mockRetriever;
		public UnitTest1()
		{
			_mockRetriever = new Mock<ITransactionRetriever>();
		}

		//Todo: Something here
		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion 

		[TestMethod]
		public void TestMethod1()
		{
			_mockRetriever.Setup(m => m.GetTransactions(It.IsAny<int>())).Returns(new decimal[] { 10, 20, 30, 40 });
			var calculator = new BalanceCalculator(_mockRetriever.Object);
			Assert.AreEqual(105, calculator.GetBalance(5, 1));
		}		
		
		[TestMethod]
		public void TestMatchingRulesFor2()
		{
			_mockRetriever.Setup(m => m.GetTransactions(It.Is<int>(i => i == 2))).Returns(new decimal[] { 10, 20, 30, 40 });
			var calculator = new BalanceCalculator(_mockRetriever.Object);
			Assert.AreEqual(105, calculator.GetBalance(5, 2));
		}		
		
		[TestMethod]
		public void TestMethodThrowException()
		{
			_mockRetriever.Setup(m => m.GetTransactions(It.IsAny<int>())).Throws(new ArgumentException());
			var calculator = new BalanceCalculator(_mockRetriever.Object);
			Assert.AreEqual(105, calculator.GetBalance(105, 0));
		}

		[TestMethod]
		public void TotalIsCorrectForMultipleMatchingPeriodsWithTransactions2()
		{
			var mockRetriever = new Mock<ITransactionRetriever>();
			mockRetriever.Setup(m => m.GetTransactions(It.Is<int>(i => i == 1 || i == 2))).Returns(
				new decimal[] { 1, 2, 3, 4 });
			mockRetriever.Setup(m => m.GetTransactions(3)).Returns(new decimal[] { 5, 6, 7 });
			var calculator = new BalanceCalculator(mockRetriever.Object);
			Assert.AreEqual(43, calculator.GetBalance(5, 1, 3));
		}

		[TestMethod]
		public void TestCallToGetBalanceMethodHasBeenMade()
		{
			var calculator = new BalanceCalculator(_mockRetriever.Object);
			calculator.GetBalance(0, 1);
			_mockRetriever.Verify(m => m.GetTransactions(It.IsAny<int>()));
		}

		[TestMethod]
		public void TestNumberOfCallToGetBalanceMethod()
		{
			var calculator = new BalanceCalculator(_mockRetriever.Object);
			calculator.GetBalance(0, 1, 3);
			_mockRetriever.Verify(m => m.GetTransactions(It.IsAny<int>()), Times.Exactly(3));
		}

		[TestMethod]
		public void TotalIsCorrectForMultiplePeriodsWithTransactions3()
		{
			var mockRetriever = new Mock<ITransactionRetriever>();
			mockRetriever.Setup(m => m.GetTransactions(1)).Returns(new decimal[] { 1, 2, 3, 4 });
			mockRetriever.Setup(m => m.GetTransactions(2)).Returns(new decimal[] { 5, 6, 7 });
			mockRetriever.Setup(m => m.GetTransactions(3)).Returns(new decimal[] { 8, 9, 10 });
			//Should remove forth expectation as it is unnecessary to the test...or....
			//...change the Assert to expect return value of 83 and 4 end periods
			//mockRetriever.Setup(m => m.GetTransactions(4)).Returns(new decimal[] { 11, 12 });
			var calculator = new BalanceCalculator(mockRetriever.Object);
			Assert.AreEqual(60, calculator.GetBalance(5, 1, 3));
			mockRetriever.VerifyAll();
		}
	}
}
