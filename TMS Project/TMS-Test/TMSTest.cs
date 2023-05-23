using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS_Project;
using NUnit.Framework;
using System.IO;

namespace TMS_Test
{
	/// <summary>
	/// Test suite for the Buyer class and its functions
	/// </summary>
    [TestFixture]
    public class BuyerTest
    {
		/// <summary>
		/// Sample structure for test function
		/// MUST FOLLOW NAMING CONVENTION where:
		/// - WhatIsBeingTested is the name of the function, method or class being
		/// tested; i.e. ValidTriangle
		/// - InputValues refers to the actual input values that will be applied for this
		/// test; i.e. Input60and60and60
		/// - ExpectedOutcomes would detail the output value(s) that are expected;
		/// i.e. ExpectValidTriangle
		/// 
		/// Steps for testing:
		/// - Run tests with Test | Test Explorer
		/// or 
		/// - Build Solution to create .dll to use with NUnit UI
		/// - run NUnit UI
		/// - within UI:
		/// -> File | Open and load the .dll
		/// -> Click on the green play button to run tests		
		/// </summary>
		///
		/*
		[Test]
        public void WhatIsBeingTest_InputValues_ExpectedOutcomes()
        {
			// Arrange
			
			 * int firstAngle = 60;
			 * int secondAngle = 60;
			 * int thirdAngle = 60;
			 * 
			 * string expected = "The triangle is valid";
			 

			// Act
			
			 * string actual = Triangle.ValidTriangle(firstAngle, secondAngle, thirdAngle);
			 

			// Assert
			
			 * assert.AreEqual(expected, actual);
		}
		*/
		[Test]
		public void BuyerGetCustomers_InputMacDonglesandBigOilandMalMart_ExpectGetCustomerID()
		{
			// Arrange
			TMS_Project.Buyer buyer = new TMS_Project.Buyer();
			uint[] validIDs = { 2, 4, 7 };
			uint[] actual = new uint[3];
			string[] names = { "MacDongles", "Big Oil", "MalMart" };
			// Act
			for (int i = 0; i < names.Length; i++)
			{
				actual[i] = buyer.getCustomers(names[i]);
			}
			// Assert
			for (int i = 0; i < validIDs.Length; i++)
			{
				Assert.AreEqual(validIDs[i], actual[i]);
			}
		}
	}

	/// <summary>
	/// Test suite for the Admin class and its functions
	/// </summary>
	[TestFixture]
	public class AdminTest
	{
		[Test]
		public void AdminCreateBackup_InputFileName_ExpectFilePath()
		{
			// Arrange
			string path = Path.Combine(Directory.GetParent(
					System.IO.Directory.GetCurrentDirectory()).
					Parent.Parent.Parent.FullName);
			string fileName = DateTime.Now.ToString("d") +
								"_" + "tms_schema.sql";
			string filePath = path + "\\Backup\\" + fileName;


			// Act
			TMS_Project.Admin.CreateBackUp();

			// Assert
			Assert.IsTrue(File.Exists(filePath));	
		}
	}

	/// <summary>
	/// Test suite for the Order class and its functions
	/// </summary>
	[TestFixture]
	public class OrderTest
	{

	}

	/// <summary>
	/// Test suite for the Planner class and its functions
	/// </summary>
	[TestFixture]
	public class PlannerTest
	{

	}
}
