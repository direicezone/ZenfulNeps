using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;

namespace TestProject1
{
	public class BalanceCalculator
	{
		ITransactionRetriever _retriever;

		public BalanceCalculator(ITransactionRetriever retriever)
		{
			_retriever = retriever;
		}

		public decimal GetBalance(decimal startBalance, int period)
		{
			try
			{
				decimal[] transactions = _retriever.GetTransactions(period);
				return transactions.Sum() + startBalance;
			}
			catch (ArgumentException)
			{
				return startBalance;
			}
		}

		public decimal GetBalance(decimal startBalance, int startPeriod, int endPeriod)
		{
			decimal runningTotal = startBalance;
			for (int period = startPeriod; period <= endPeriod; period++)
			{
				runningTotal = GetBalance(runningTotal, period);
			}
			return runningTotal;
		}
	}

	public interface ITransactionRetriever
	{
		decimal[] GetTransactions(int period);
	}
}
