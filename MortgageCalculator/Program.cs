using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MortgageCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Mortgage Calculator");

            Console.Write("What is your buyer's name? ");
            string buyerName = Console.ReadLine();

            double[] loanOfficerInput = LoanOfficerInput();
            Console.WriteLine();

            double downPayment = loanOfficerInput[0];
            double purchasePrice = loanOfficerInput[1];
            double marketValue = loanOfficerInput[2];
            double buyerIncome = loanOfficerInput[3];
            double hoaFees = loanOfficerInput[4];
            double interestRate = loanOfficerInput[5];
            double paymentsPerYear = 12;

            if (downPayment/purchasePrice < .10)
            {
                Console.WriteLine("Down payment not enough.");
                Console.ReadLine();
            }
            else
            {
                double loanAmount = DetermineLoanValue(loanOfficerInput[1], loanOfficerInput[0]);
                Console.WriteLine("Loan Amount: " + loanAmount);
                Console.WriteLine();

                Console.Write("Will it be a fixed 15 or 30 year loan? ");
                double loanLength = LoanPeriodInput();
                Console.WriteLine();

                double loanInsurance = DetermineLoanInsurance(purchasePrice, loanAmount, marketValue, downPayment);
                Console.WriteLine("Annual Loan Insurance: " + loanInsurance);
                Console.WriteLine();

                double escrow = evaluateEscrow(marketValue);
                Console.WriteLine("Annual Escrow: " + escrow);
                Console.WriteLine();

                double monthlyPayment = MonthlyPayment(loanAmount, interestRate, paymentsPerYear, loanLength, escrow, hoaFees, loanInsurance);
                Console.WriteLine("Monthly Payment: " + monthlyPayment);
                Console.WriteLine();

                ApproveOrDeny(monthlyPayment, buyerIncome, buyerName);
                Console.ReadLine();
            }
        }
        static void ApproveOrDeny(double monthlyPayment, double buyerIncome, string buyerName)
        {
            double monthlyIncome = buyerIncome / 12;
            double percentageDifference = monthlyPayment / monthlyIncome;

            if(percentageDifference <= .25)
            {
                Console.WriteLine("Loan approved, Congratulations {0}!", buyerName);
                Console.WriteLine("Your payment will be: {0:c}", monthlyPayment);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Loan denied");
            }
        }
        static double MonthlyPayment(double loanAmount, double interestRate, double paymentsPerYear, double loanLength, double escrow, double hoaFees, double loanInsurance)
        {
            double monthlyInterest = interestRate / paymentsPerYear;
            double loanInMonths = loanLength * paymentsPerYear;
            double baseMonthlyPayment = loanAmount * ((monthlyInterest
                * Math.Pow((1 + monthlyInterest), loanInMonths))
                / (Math.Pow(1 + monthlyInterest, loanInMonths) - 1));
            double monthlyPayment = baseMonthlyPayment + (escrow/paymentsPerYear) + (loanInsurance/paymentsPerYear) + hoaFees;
            return monthlyPayment;
        }
        static double evaluateEscrow(double marketValue)
        {
            double escrowPercentage = .02;
            double escrow = marketValue * (escrowPercentage);
            return escrow;
            
        }

        static double DetermineLoanValue(double purchasePrice, double downPayment)
        {
            double loanAmount;
            double loanBaseAmount;
            double originationFeeRate = .01;
            double closingCosts = 2500;

            loanBaseAmount = purchasePrice - downPayment;
            double originationFee = loanBaseAmount * originationFeeRate;
            loanAmount = loanBaseAmount + originationFee + closingCosts;
            return loanAmount;
        }

        static double[] LoanOfficerInput()
        {
            double[] loanOfficerInput = new double[6];
            Console.WriteLine("Please enter the following: ");
            Console.Write("Down Payment: ");
            double downPayment = Convert.ToDouble(Console.ReadLine());
            loanOfficerInput[0] = downPayment;
            Console.Write("Purchase Price: ");
            double purchasePrice = Convert.ToDouble(Console.ReadLine());
            loanOfficerInput[1] = purchasePrice;
            Console.Write("Market Value: ");
            double marketValue = Convert.ToDouble(Console.ReadLine());
            loanOfficerInput[2] = marketValue;
            Console.Write("Buyer Income: ");
            double buyerIncome = Convert.ToDouble(Console.ReadLine());
            loanOfficerInput[3] = buyerIncome;
            Console.Write("HOA Fees: ");
            double hoaFees = Convert.ToDouble(Console.ReadLine());
            loanOfficerInput[4] = hoaFees;
            Console.Write("Interest Rate: ");
            double interestRate = Convert.ToDouble(Console.ReadLine());
            loanOfficerInput[5] = interestRate;

            Console.WriteLine($"You entered the following, Down Payment: {downPayment}, Purchase Price: {purchasePrice},\nMarket Value: {marketValue}, Buyer Income: {buyerIncome}, HOA Fees: {hoaFees}.");
            return loanOfficerInput;
        }
        public static double LoanPeriodInput()
        {
          double loanLength = Convert.ToDouble(Console.ReadLine());
          
              if (loanLength == 15)
              {
                  Console.WriteLine("You have selected a {0} year loan.", loanLength);
              }
              else if (loanLength == 30)
              {
                  Console.WriteLine("You have selected a {0} year loan.", loanLength);
              }
          return loanLength;
            
        }
        static double DetermineLoanInsurance(double purchasePrice, double loanAmount, double marketValue, double downPayment)
        {
            double marketDifference = purchasePrice - marketValue;
            double downPaymentRequired = (purchasePrice/10) + marketDifference;
            double loanInsurance = loanAmount * .01;
            if (downPayment < downPaymentRequired)
            {
                Console.WriteLine("You will need loan insurance");
                return loanInsurance;
            }
            else
            {
                Console.WriteLine("Loan insurance not required");
                return 0;
            }
        }
    }
}
