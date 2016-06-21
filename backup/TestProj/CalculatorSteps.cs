using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TechTalk.SpecFlow;

namespace TestProj
{
    [Binding]
    public class CalculatorSteps
    {
        [Given(@"I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(int p0)
        {
            Calculator.Enter(p0);
        }
        
        [Given(@"I have also entered (.*) into the calculator")]
        public void GivenIHaveAlsoEnteredIntoTheCalculator(int p0)
        {
            Calculator.Enter(p0);
        }
        
        [When(@"I press add")]
        public void WhenIPressAdd()
        {
            Calculator.Add();
        }
        
        [Then(@"the result should be (.*) on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(int p0)
        {
            Assert.Fail("These two numbers are not the same: ", Calculator.LastResult, p0);
        }
    }

    public class Calculator {

        private static readonly List<int> buffer = new List<int>();

        public static int LastResult = 0;

        public static void Enter(int arg)
        {
            buffer.Add(arg);
        }

        public static int Add()
        {
            LastResult = 0;
            foreach (var i in buffer)
                LastResult += i;
            buffer.Clear();
            return LastResult;
        }
    }


}
