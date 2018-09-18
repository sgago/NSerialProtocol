namespace NSerialProtocolUnitTests
{
    using NSerialProtocol;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class FieldUnitTests
    {
        /// <summary>
        /// Verifies the Index accessors get and set the correct value.
        /// </summary>
        [TestCase]
        public void Index_Accessors_Test()
        {
            int expected = 99;
            int actual = 0;

            Field field = new Field(0, 10, "");

            field.Index = expected;
            actual = field.Index;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                nameof(Field.Index), actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the Length accessors get and set the correct value.
        /// </summary>
        [TestCase]
        public void Length_Accessors_Test()
        {
            int expected = 111;
            int actual = 0;

            Field field = new Field(0, 10, "");

            field.Length = expected;
            actual = field.Length;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                nameof(Field.Length), actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the KnownLength accessors get and set the correct value.
        /// </summary>
        [TestCase]
        public void KnownLength_Accessors_Test()
        {
            bool expected = true;
            bool actual = false;

            Field field = new Field(0, 10, "");

            field.KnownLength = expected;
            actual = field.KnownLength;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                nameof(Field.KnownLength), actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the ValueFunc accessors get and set the correct value.
        /// </summary>
        [TestCase]
        public void ValueFunc_Accessors_Test()
        {
            Func<string> expected = () =>
            {
                return "hello unit testing";
            };

            Func<string> actual = null;

            Field field = new Field(0, 10, "");

            field.ValueFunc = expected;
            actual = field.ValueFunc;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                nameof(Field.ValueFunc), actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the Value accessors get and set the correct value.
        /// </summary>
        [TestCase]
        public void Value_Accessors_Test()
        {
            string expected = "hello world";
            string actual = null;

            Field field = new Field(0, 10, "");

            field.Value = expected;
            actual = field.Value;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                nameof(Field.Value), actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the Order accessors get and set the correct value.
        /// </summary>
        [TestCase]
        public void Order_Accessors_Test()
        {
            int expected = 13;
            int actual = 0;

            Field field = new Field(0, 10, "");

            field.Order = expected;
            actual = field.Order;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                nameof(Field.Order), actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }
    }
}
