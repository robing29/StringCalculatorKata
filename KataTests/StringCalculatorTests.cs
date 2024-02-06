using StringCalculatorKata;
using System.Net.NetworkInformation;

namespace KataTests
{
    [TestClass]
    public class StringCalculatorTests
    {
        private StringCalculator createDefaultStringCalculator()
        {
            return new StringCalculatorKata.StringCalculator();
        }

        [TestMethod]
        public void Add_EmptyString_ReturnZero()
        {
            //Arrange
            StringCalculator stringCalculator = createDefaultStringCalculator();

            //Act
            int result = stringCalculator.Add("");

            //Assert
            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void Add_SingleNumber_ReturnSingleNumber()
        {
            //Arrange
            StringCalculator stringCalculator = createDefaultStringCalculator();

            //Act
            int actual = stringCalculator.Add("0");

            //Assert
            Assert.AreEqual(actual,0);
        }

        [TestMethod]
        [DataRow("0,1",1)]
        [DataRow(" 1,2",3)]
        [DataRow(" 3,5",8)]
        [DataRow("1000, 1001", 1000)]
        [DataRow("1001, 1002", 0)]
        [DataRow("1000, 1000", 2000)]
        public void Add_TwoNumbers_ReturnResult(string input, int expected)
        {
            //Arrange
            StringCalculator stringCalculator = createDefaultStringCalculator();

            //Act
            int actual = stringCalculator.Add(input);

            //Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        [DataRow("0,1,2", 3)]
        [DataRow(" 1,2,3,4", 10)]
        [DataRow(" 3,5,6,7,8", 29)]
        [DataRow("1001, 1000, 2000", 1000)]
        [DataRow("5, 5, 2000", 10)]
        [DataRow("//[---]\n1---2000---1000", 1001)]
        [DataRow("//[***]\n1***2000***1000", 1001)]
        public void Add_ThreeOrMoreNumbers_ReturnResult(string input, int expected)
        {
            //Arrange
            StringCalculator stringCalculator = createDefaultStringCalculator();

            //Act
            int actual = stringCalculator.Add(input);

            //Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        [DataRow("0\n1,2", 3)]
        [DataRow(" 1,2\n3", 6)]
        [DataRow(" 3,5\n6,7\n8", 29)]
        public void Add_ThreeNumbersWithNewLineDelimiter_ReturnResult(string input, int expected)
        {
            //Arrange
            StringCalculator stringCalculator = createDefaultStringCalculator();

            //Act
            int actual = stringCalculator.Add(input);

            //Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        [DataRow("//;\n1;2", 3)]
        [DataRow("//.\n2.2", 4)]
        [DataRow("//-\n4-4", 8)]
        public void Add_TwoNumbersWithCustomDelimiter_ReturnResult(string input, int expected)
        {
            //Arrange
            StringCalculator stringCalculator = createDefaultStringCalculator();

            //Act
            int actual = stringCalculator.Add(input);

            //Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        [DataRow("//;\n1;-2")]
        [DataRow("//.\n2.-1")]
        [DataRow("-1")]
        [DataRow("-3,3")]
        public void Add_NegativeNumber_ThrowsException(string input)
        {
            //Arrange
            StringCalculator stringCalculator = createDefaultStringCalculator();

            //Act
            Action action = () => stringCalculator.Add(input);

            //Assert
            Assert.ThrowsException<Exception>(action);
        }

        [TestMethod]
        [DataRow("//;\n1;-2","-2")]
        [DataRow("//.\n2.-1","-1")]
        [DataRow("-1","-1")]
        [DataRow("-3,3","-3")]
        [DataRow("3,-3","-3")]
        public void Add_NegativeNumbers_ThrowExceptionAndContainFirstNegativeNumber(string input, string firstnegnumber)
        {
            // Arrange
            StringCalculator stringCalculator = createDefaultStringCalculator();

            // Act and Assert
            try
            {
                stringCalculator.Add(input);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception ex)
            {
                // Verify the exception message
                Assert.IsTrue(ex.Message.Contains(firstnegnumber));
            }
        }

        [TestMethod]
        [DataRow("//;\n1;-2;-3;-4", "-2,-3,-4")]
        [DataRow("//.\n2.-1.-3", "-1,-3")]
        [DataRow("-1,-3", "-1,-3")]
        [DataRow("-3,3,-4", "-3,-4")]
        [DataRow("3,-3,-4,-5", "-3,-4,-5")]
        public void Add_NegativeNumbers_ThrowsExceptionAndContainsAllNegativeNumbers(string input, string expectedNegativeNumbers)
        {
            // Arrange
            StringCalculator stringCalculator = createDefaultStringCalculator();

            // Act and Assert
            try
            {
                stringCalculator.Add(input);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception ex)
            {
                // Verify the exception message
                string actualMessage = ex.Message;

                // Split the expected and actual negative numbers
                string[] expectedNegativeNumbersArray = expectedNegativeNumbers.Split(',');
                string[] actualNegativeNumbersArray = actualMessage.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                                   .Where(s => s.StartsWith("-"))
                                                                   .ToArray();

                // Check if all expected negative numbers are present in the actual message
                CollectionAssert.AreEquivalent(expectedNegativeNumbersArray, actualNegativeNumbersArray,
                    $"Expected: {expectedNegativeNumbers}, Actual: {actualMessage}");
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(6)]
        public void GetCalledCount_ReturnsCorrectCountAfterAddCalls(int addCallCount)
        {
            // Arrange
            StringCalculator stringCalculator = createDefaultStringCalculator();

            // Act
            for (int i = 0; i < addCallCount; i++)
            {
                stringCalculator.Add("");
            }

            // Assert
            Assert.AreEqual(addCallCount, stringCalculator.GetCalledCount());
        }

        [TestMethod]
        public void OccuredEvent()
        {
            //Arrange
            StringCalculator stringCalculator = createDefaultStringCalculator();

            string receivedinput = null;
            int receivedResult = 0;

            //Act
            stringCalculator.AddOccured += (input, result) =>
            {
                receivedinput = input;
                receivedResult = result;
            };
            stringCalculator.Add("1,2");

            //Assert
            Assert.AreEqual("1,2", receivedinput);
            Assert.AreEqual(3, receivedResult);

            
        } 
    }
}