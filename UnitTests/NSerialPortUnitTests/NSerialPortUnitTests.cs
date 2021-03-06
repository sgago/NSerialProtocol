﻿// Naming standard = [UnitOfWork_StateUnderTest_ExpectedBehavior]

namespace NSerialPortUnitTests
{
    using NativeMethods;
    using NSerialPort;
    using NSubstitute;
    using NUnit.Framework;
    using SerialPortFix;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.IO.Ports;
    using System.Text;
    using static NSerialPort.NSerialPort;

    /// <summary>
    /// Contains unit tests for NSerialPort.
    /// </summary>
    [TestFixture]
    public class NSerialPortUnitTests
    {
        private static readonly IEnumerable propertyNames = new List<string>
        {
            nameof(NSerialPort.BaudRate),
            nameof(NSerialPort.BreakState),
            nameof(NSerialPort.DataBits),
            nameof(NSerialPort.DiscardNull),
            nameof(NSerialPort.DtrEnable),
            nameof(NSerialPort.Encoding),
            nameof(NSerialPort.Handshake),
            nameof(NSerialPort.IsOpen),
            nameof(NSerialPort.NewLine),
            nameof(NSerialPort.Parity),
            nameof(NSerialPort.ParityReplace),
            nameof(NSerialPort.PortName),
            nameof(NSerialPort.ReadBufferSize),
            nameof(NSerialPort.ReadTimeout),
            nameof(NSerialPort.ReceivedBytesThreshold),
            nameof(NSerialPort.RtsEnable),
            nameof(NSerialPort.StopBits),
            nameof(NSerialPort.WriteBufferSize),
            nameof(NSerialPort.WriteTimeout),
        };

        private static IEnumerable GetPropertyTestCaseData(string propertyName)
        {
            foreach (string name in propertyNames)
            {
                yield return new TestCaseData(name).Returns(name == propertyName);
            }
        }

        /// <summary>
        /// Verifies the BaseStream get accessor returns the correct reference
        /// from the BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void BaseStream_GetAccessor_ReturnsCorrectReference()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            serialPortSub.BaseStream.Returns(new MemoryStream());

            Stream expected = serialPortSub.BaseStream;
            Stream actual = nSerialPort.BaseStream;

            Assert.That(actual, Is.SameAs(expected),
                "BaseStream references were not the same.");
        }

        /// <summary>
        /// Verifies the BaudRate get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void BaudRate_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            int expected = 123;
            int actual = 0;

            serialPortSub.BaudRate = expected;
            actual = nSerialPort.BaudRate;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "BaudRate", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the BaudRate set accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void BaudRate_SetAccessor_SetsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            int expected = int.MaxValue;
            int actual = 0;

            nSerialPort.BaudRate = expected;
            actual = serialPortSub.BaudRate;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "BaudRate", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Test cases for BaudRate OnPropertyChanged event.
        /// </summary>
        public static IEnumerable BaudRate_OnPropertyChanged_TestCases
        {
            get
            {
                return GetPropertyTestCaseData(nameof(NSerialPort.BaudRate));
            }
        }

        /// <summary>
        /// Verifies the correct events are raised when BaudRate property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(BaudRate_OnPropertyChanged_TestCases))]
        public bool BaudRate_OnPropertyChanged_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.BaudRate))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.BaudRate = 100;

            return wasCalled;
        }

        /// <summary>
        /// Verifies the BreakState get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void BreakState_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            bool expected = true;
            bool actual = false;

            serialPortSub.BreakState = expected;
            actual = nSerialPort.BreakState;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "BreakState", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the BreakState set accessor sets the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void BreakState_SetAccessor_SetsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            bool expected = true;
            bool actual = false;

            nSerialPort.BreakState = expected;
            actual = serialPortSub.BreakState;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "BreakState", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Test cases for BreakState OnPropertyChanged event.
        /// </summary>
        public static IEnumerable BreakState_OnPropertyChanged_TestCases
        {
            get
            {
                return GetPropertyTestCaseData(nameof(NSerialPort.BreakState));
            }
        }

        /// <summary>
        /// Verifies the correct events are raised when BreakState property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(BreakState_OnPropertyChanged_TestCases))]
        public bool BreakState_OnPropertyChanged_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.BreakState))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.BreakState = true;

            return wasCalled;
        }

        /// <summary>
        /// Verifies the BytesToRead get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void BytesToRead_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            int expected = 99;
            int actual = 0;

            serialPortSub.BytesToRead.Returns(expected);
            actual = nSerialPort.BytesToRead;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "BytesToRead", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the BytesToWrite get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void BytesToWrite_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            int expected = 101;
            int actual = 0;

            serialPortSub.BytesToWrite.Returns(expected);
            actual = nSerialPort.BytesToWrite;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "BytesToWrite", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the CDHolding get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void CDHolding_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            bool expected = true;
            bool actual = false;

            serialPortSub.CDHolding.Returns(expected);
            actual = nSerialPort.CDHolding;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "CDHolding", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the CtsHolding get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void CtsHolding_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            bool expected = true;
            bool actual = false;

            serialPortSub.CtsHolding.Returns(expected);
            actual = nSerialPort.CtsHolding;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "CtsHolding", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the DataBits get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void DataBits_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            int expected = 73;
            int actual = 0;

            serialPortSub.DataBits = expected;
            actual = nSerialPort.DataBits;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "DataBits", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the DataBits set accessor returns the correct value from
        /// the BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void DataBits_SetAccessor_SetsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            int expected = 24;
            int actual = 0;

            nSerialPort.DataBits = expected;
            actual = serialPortSub.DataBits;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "DataBits", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Test cases for DataBits OnPropertyChanged event.
        /// </summary>
        public static IEnumerable DataBits_OnPropertyChanged_TestCases
        {
            get
            {
                return GetPropertyTestCaseData(nameof(NSerialPort.DataBits));
            }
        }

        /// <summary>
        /// Verifies the correct events are raised when DataBits property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(DataBits_OnPropertyChanged_TestCases))]
        public bool DataBits_OnPropertyChanged_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.DataBits))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.DataBits = 7;

            return wasCalled;
        }

        /// <summary>
        /// Verifies the DiscardNull get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void DiscardNull_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            bool expected = true;
            bool actual = false;

            serialPortSub.DiscardNull = expected;
            actual = nSerialPort.DiscardNull;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "DiscardNull", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the DiscardNull set accessor returns the correct value from
        /// the BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void DiscardNull_SetAccessor_SetsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            bool expected = true;
            bool actual = false;

            nSerialPort.DiscardNull = expected;
            actual = serialPortSub.DiscardNull;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "DiscardNull", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Test cases for DiscardNull OnPropertyChanged event.
        /// </summary>
        public static IEnumerable DiscardNull_OnPropertyChanged_TestCases
        {
            get
            {
                return GetPropertyTestCaseData(nameof(NSerialPort.DiscardNull));
            }
        }

        /// <summary>
        /// Verifies the correct events are raised when DiscardNull property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(DiscardNull_OnPropertyChanged_TestCases))]
        public bool DiscardNull_OnPropertyChanged_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.DiscardNull))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.DiscardNull = true;

            return wasCalled;
        }

        /// <summary>
        /// Verifies the DsrHolding get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void DsrHolding_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            bool expected = true;
            bool actual = false;

            serialPortSub.DsrHolding.Returns(expected);
            actual = nSerialPort.DsrHolding;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "DsrHolding", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the DtrEnable get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void DtrEnable_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            bool expected = true;
            bool actual = false;

            serialPortSub.DtrEnable = expected;
            actual = nSerialPort.DtrEnable;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "DtrEnable", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the DtrEnable set accessor returns the correct value from
        /// the BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void DtrEnable_SetAccessor_SetsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            bool expected = true;
            bool actual = false;

            nSerialPort.DtrEnable = expected;
            actual = serialPortSub.DtrEnable;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "DtrEnable", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Test cases for DtrEnable OnPropertyChanged event.
        /// </summary>
        public static IEnumerable DtrEnable_OnPropertyChanged_TestCases
        {
            get
            {
                return GetPropertyTestCaseData(nameof(NSerialPort.DtrEnable));
            }
        }

        /// <summary>
        /// Verifies the correct events are raised when DtrEnable property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(DtrEnable_OnPropertyChanged_TestCases))]
        public bool DtrEnable_OnPropertyChanged_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.DtrEnable))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.DtrEnable = true;

            return wasCalled;
        }

        /// <summary>
        /// Verifies the Encoding get accessor returns the correct reference from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void Encoding_GetAccessor_ReturnsCorrectReference()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            serialPortSub.Encoding.Returns(Encoding.ASCII);

            Encoding expected = serialPortSub.Encoding;
            Encoding actual = nSerialPort.Encoding;

            Assert.That(actual, Is.SameAs(expected),
                "Encoding references were not the same.");
        }

        /// <summary>
        /// Verifies the Encoding set accessor sets the correct BaseSerialPort object
        /// reference.
        /// </summary>
        [TestCase]
        public void Encoding_SetAccessor_SetsCorrectReference()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            Encoding expected = Encoding.BigEndianUnicode;
            nSerialPort.Encoding = expected;
            Encoding actual = serialPortSub.Encoding;

            Assert.That(actual, Is.SameAs(expected),
                "Encoding references were not the same.");
        }

        /// <summary>
        /// Test cases for Encoding OnPropertyChanged event.
        /// </summary>
        public static IEnumerable Encoding_OnPropertyChanged_TestCases
        {
            get
            {
                return GetPropertyTestCaseData(nameof(NSerialPort.Encoding));
            }
        }

        /// <summary>
        /// Verifies the correct events are raised when Encoding property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(Encoding_OnPropertyChanged_TestCases))]
        public bool Encoding_OnPropertyChanged_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.Encoding))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.Encoding = Encoding.UTF8;

            return wasCalled;
        }

        /// <summary>
        /// Verifies the Handshake get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void Handshake_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            Handshake expected = Handshake.XOnXOff;
            Handshake actual = Handshake.None;

            serialPortSub.Handshake = expected;
            actual = nSerialPort.Handshake;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "Handshake", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the Handshake set accessor sets the correct value in the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void Handshake_SetAccessor_SetsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            Handshake expected = Handshake.XOnXOff;
            Handshake actual = Handshake.None;

            nSerialPort.Handshake = expected;
            actual = serialPortSub.Handshake;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "Handshake", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Test cases for Handshake OnPropertyChanged event.
        /// </summary>
        public static IEnumerable Handshake_OnPropertyChanged_TestCases
        {
            get
            {
                return GetPropertyTestCaseData(nameof(NSerialPort.Handshake));
            }
        }

        /// <summary>
        /// Verifies the correct events are raised when Handshake property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(Handshake_OnPropertyChanged_TestCases))]
        public bool Handshake_OnPropertyChanged_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.Handshake))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.Handshake = Handshake.RequestToSendXOnXOff;

            return wasCalled;
        }

        /// <summary>
        /// Verifies the IsOpen get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void IsOpen_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            bool expected = true;
            bool actual = false;

            serialPortSub.IsOpen.Returns(expected);
            actual = nSerialPort.IsOpen;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "IsOpen", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Test cases for IsOpen OnPropertyChanged event.
        /// </summary>
        public static IEnumerable IsOpen_OnPropertyChanged_TestCases
        {
            get
            {
                return GetPropertyTestCaseData(nameof(NSerialPort.IsOpen));
            }
        }

        /// <summary>
        /// Verifies the correct events are raised when IsOpen property changes when the
        /// Close method is called.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(IsOpen_OnPropertyChanged_TestCases))]
        public bool IsOpen_CloseCalled_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.IsOpen))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.Close();

            return wasCalled;
        }

        /// <summary>
        /// Verifies the correct events are raised when IsOpen property changes when the
        /// Open method is called.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(IsOpen_OnPropertyChanged_TestCases))]
        public bool IsOpen_OpenCalled_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.IsOpen))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.Open();

            return wasCalled;
        }

        /// <summary>
        /// Verifies the NewLine get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void NewLine_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            string expected = "newline";
            string actual = "abc";

            serialPortSub.NewLine = expected;
            actual = nSerialPort.NewLine;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "NewLine", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the NewLine set accessor sets the correct BaseSerialPort object value.
        /// </summary>
        [TestCase]
        public void NewLine_SetAccessor_SetsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            string expected = "newline";
            string actual = "abc";

            nSerialPort.NewLine = expected;
            actual = serialPortSub.NewLine;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "NewLine", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Test cases for NewLine OnPropertyChanged event.
        /// </summary>
        public static IEnumerable NewLine_OnPropertyChanged_TestCases
        {
            get
            {
                return GetPropertyTestCaseData(nameof(NSerialPort.NewLine));
            }
        }

        /// <summary>
        /// Verifies the correct events are raised when NewLine property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(NewLine_OnPropertyChanged_TestCases))]
        public bool NewLine_OnPropertyChanged_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.NewLine))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.NewLine = "\r\n";

            return wasCalled;
        }

        /// <summary>
        /// Verifies the Parity get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void Parity_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            Parity expected = Parity.Mark;
            Parity actual = Parity.None;

            serialPortSub.Parity = expected;
            actual = nSerialPort.Parity;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "Parity", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the Parity set accessor sets the correct value in the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void Parity_SetAccessor_SetsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            Parity expected = Parity.Space;
            Parity actual = Parity.None;

            nSerialPort.Parity = expected;
            actual = serialPortSub.Parity;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "Parity", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Test cases for Parity OnPropertyChanged event.
        /// </summary>
        public static IEnumerable Parity_OnPropertyChanged_TestCases
        {
            get
            {
                return GetPropertyTestCaseData(nameof(NSerialPort.Parity));
            }
        }

        /// <summary>
        /// Verifies the correct events are raised when Parity property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(Parity_OnPropertyChanged_TestCases))]
        public bool Parity_OnPropertyChanged_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.Parity))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.Parity = Parity.Mark;

            return wasCalled;
        }

        /// <summary>
        /// Verifies the ParityReplace get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void ParityReplace_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            byte expected = 33;
            byte actual = 0;

            serialPortSub.ParityReplace = expected;
            actual = nSerialPort.ParityReplace;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "ParityReplace", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the ParityReplacce set accessor sets the correct value
        /// in the BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void ParityReplace_SetAccessor_SetsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            byte expected = 55;
            byte actual = 0;

            nSerialPort.ParityReplace = expected;
            actual = serialPortSub.ParityReplace;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "ParityReplace", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Test cases for ParityReplace OnPropertyChanged event.
        /// </summary>
        public static IEnumerable ParityReplace_OnPropertyChanged_TestCases
        {
            get
            {
                return GetPropertyTestCaseData(nameof(NSerialPort.ParityReplace));
            }
        }

        /// <summary>
        /// Verifies the correct events are raised when ParityReplace property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(ParityReplace_OnPropertyChanged_TestCases))]
        public bool ParityReplace_OnPropertyChanged_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.ParityReplace))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.ParityReplace = 10;

            return wasCalled;
        }

        /// <summary>
        /// Verifies the PortName get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void PortName_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            string expected = "COM9";
            string actual = "abc";

            serialPortSub.PortName = expected;
            actual = nSerialPort.PortName;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "PortName", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the PortName set accessor sets the correct value in the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void PortName_SetAccessor_SetsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            string expected = "COM7";
            string actual = "abc";

            nSerialPort.PortName = expected;
            actual = serialPortSub.PortName;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "PortName", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Test cases for PortName OnPropertyChanged event.
        /// </summary>
        public static IEnumerable PortName_OnPropertyChanged_TestCases
        {
            get
            {
                return GetPropertyTestCaseData(nameof(NSerialPort.PortName));
            }
        }

        /// <summary>
        /// Verifies the correct events are raised when PortName property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(PortName_OnPropertyChanged_TestCases))]
        public bool PortName_OnPropertyChanged_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.PortName))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.PortName = "COM10";

            return wasCalled;
        }

        /// <summary>
        /// Verifies the ReadBufferSize get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void ReadBufferSize_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            int expected = 100;
            int actual = 0;

            serialPortSub.ReadBufferSize = expected;
            actual = nSerialPort.ReadBufferSize;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "ReadBufferSize", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the ReadBufferSize set accessor sets the correct BaseSerialPort
        /// value.
        /// </summary>
        [TestCase]
        public void ReadBufferSize_SetAccessor_SetsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            int expected = 77;
            int actual = 0;

            nSerialPort.ReadBufferSize = expected;
            actual = serialPortSub.ReadBufferSize;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "ReadBufferSize", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Test cases for ReadBufferSize OnPropertyChanged event.
        /// </summary>
        public static IEnumerable ReadBufferSize_OnPropertyChanged_TestCases
        {
            get
            {
                return GetPropertyTestCaseData(nameof(NSerialPort.ReadBufferSize));
            }
        }

        /// <summary>
        /// Verifies the correct events are raised when ReadBufferSize property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(ReadBufferSize_OnPropertyChanged_TestCases))]
        public bool ReadBufferSize_OnPropertyChanged_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.ReadBufferSize))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.ReadBufferSize = 3;

            return wasCalled;
        }

        /// <summary>
        /// Verifies the ReadTimeout get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void ReadTimeout_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            int expected = 3;
            int actual = 0;

            serialPortSub.ReadTimeout = expected;
            actual = nSerialPort.ReadTimeout;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "ReadTimeout", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the ReadTimeout set accessor sets the correct BaseSerialPort
        /// value.
        /// </summary>
        [TestCase]
        public void ReadTimeout_SetAccessor_SetsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            int expected = 2;
            int actual = 0;

            nSerialPort.ReadTimeout = expected;
            actual = serialPortSub.ReadTimeout;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "ReadTimeout", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Test cases for ReadTimeout OnPropertyChanged event.
        /// </summary>
        public static IEnumerable ReadTimeout_OnPropertyChanged_TestCases
        {
            get
            {
                return GetPropertyTestCaseData(nameof(NSerialPort.ReadTimeout));
            }
        }

        /// <summary>
        /// Verifies the correct events are raised when ReadTimeout property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(ReadTimeout_OnPropertyChanged_TestCases))]
        public bool ReadTimeout_OnPropertyChanged_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.ReadTimeout))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.ReadTimeout = 32;

            return wasCalled;
        }

        /// <summary>
        /// Verifies the ReceivedBytesTheshold get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void ReceivedBytesThreshold_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            int expected = 5;
            int actual = 0;

            serialPortSub.ReceivedBytesThreshold = expected;
            actual = nSerialPort.ReceivedBytesThreshold;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "ReceivedBytesThreshold", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the ReceivedBytesThreshold set accessor sets the correct
        /// BaseSerialPort object value.
        /// </summary>
        [TestCase]
        public void ReceivedBytesThreshold_SetAccessor_SetsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            int expected = 8;
            int actual = 0;

            nSerialPort.ReceivedBytesThreshold = expected;
            actual = serialPortSub.ReceivedBytesThreshold;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "ReceivedBytesThreshold", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Test cases for ReceivedBytesThreshold OnPropertyChanged event.
        /// </summary>
        public static IEnumerable ReceivedBytesThreshold_OnPropertyChanged_TestCases
        {
            get
            {
                return GetPropertyTestCaseData(nameof(NSerialPort.ReceivedBytesThreshold));
            }
        }

        /// <summary>
        /// Verifies the correct events are raised when ReceivedBytesThreshold property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(ReceivedBytesThreshold_OnPropertyChanged_TestCases))]
        public bool ReceivedBytesThreshold_OnPropertyChanged_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.ReceivedBytesThreshold))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.ReceivedBytesThreshold = 128;

            return wasCalled;
        }

        /// <summary>
        /// Verifies the RtsEnable get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void RtsEnable_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            bool expected = true;
            bool actual = false;

            serialPortSub.RtsEnable = expected;
            actual = nSerialPort.RtsEnable;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "RtsEnable", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the RtsEnable set accessor sets the correct BaseSerialPort
        /// object value.
        /// </summary>
        [TestCase]
        public void RtsEnable_SetAccessor_SetsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            bool expected = true;
            bool actual = false;

            nSerialPort.RtsEnable = expected;
            actual = serialPortSub.RtsEnable;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "RtsEnable", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Test cases for RtsEnable OnPropertyChanged event.
        /// </summary>
        public static IEnumerable RtsEnable_OnPropertyChanged_TestCases
        {
            get
            {
                return GetPropertyTestCaseData(nameof(NSerialPort.RtsEnable));
            }
        }

        /// <summary>
        /// Verifies the correct events are raised when RtsEnable property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(RtsEnable_OnPropertyChanged_TestCases))]
        public bool RtsEnable_OnPropertyChanged_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.RtsEnable))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.RtsEnable = true;

            return wasCalled;
        }

        /// <summary>
        /// Verifies the StopBits get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void StopBits_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            StopBits expected = StopBits.OnePointFive;
            StopBits actual = StopBits.None;

            serialPortSub.StopBits = expected;
            actual = nSerialPort.StopBits;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "StopBits", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the StopBits set accessor sets the correct BaseSerialPort
        /// object value.
        /// </summary>
        [TestCase]
        public void StopBits_SetAccessor_SetsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            StopBits expected = StopBits.Two;
            StopBits actual = StopBits.None;

            nSerialPort.StopBits = expected;
            actual = serialPortSub.StopBits;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "StopBits", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Test cases for StopBits OnPropertyChanged event.
        /// </summary>
        public static IEnumerable StopBits_OnPropertyChanged_TestCases
        {
            get
            {
                return GetPropertyTestCaseData(nameof(NSerialPort.StopBits));
            }
        }

        /// <summary>
        /// Verifies the correct events are raised when StopBits property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(StopBits_OnPropertyChanged_TestCases))]
        public bool StopBits_OnPropertyChanged_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.StopBits))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.StopBits = StopBits.OnePointFive;

            return wasCalled;
        }

        /// <summary>
        /// Verifies the WriteBufferSize get accessor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void WriteBufferSize_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            int expected = 4;
            int actual = 0;

            serialPortSub.WriteBufferSize = expected;
            actual = nSerialPort.WriteBufferSize;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "WriteBufferSize", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the WriteBufferSize set accessor sets the correct BaseSerialPort
        /// object value.
        /// </summary>
        [TestCase]
        public void WriteBufferSize_SetAccessor_SetsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            int expected = 88;
            int actual = 0;

            nSerialPort.WriteBufferSize = expected;
            actual = serialPortSub.WriteBufferSize;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "WriteBufferSize", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Test cases for WriteBufferSize OnPropertyChanged event.
        /// </summary>
        public static IEnumerable WriteBufferSize_OnPropertyChanged_TestCases
        {
            get
            {
                return GetPropertyTestCaseData(nameof(NSerialPort.WriteBufferSize));
            }
        }

        /// <summary>
        /// Verifies the correct events are raised when WriteBufferSize property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(WriteBufferSize_OnPropertyChanged_TestCases))]
        public bool WriteBufferSize_OnPropertyChanged_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.WriteBufferSize))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.WriteBufferSize = 987;

            return wasCalled;
        }

        /// <summary>
        /// Verifies the WriteTimeout get accesssor returns the correct value from the
        /// BaseSerialPort object.
        /// </summary>
        [TestCase]
        public void WriteTimeout_GetAccessor_ReturnsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            int expected = 4;
            int actual = 0;

            serialPortSub.WriteTimeout = expected;
            actual = nSerialPort.WriteTimeout;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "WriteTimeout", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Verifies the WriteTimeout set accessor sets the correct BaseSerialPort
        /// object value.
        /// </summary>
        [TestCase]
        public void WriteTimeout_SetAccessor_SetsCorrectValue()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            int expected = 88;
            int actual = 0;

            nSerialPort.WriteTimeout = expected;
            actual = serialPortSub.WriteTimeout;

            string errorMessage = string.Format("{0} was \"{1}\" and should be \"{2}\"",
                "WriteTimeout", actual, expected);

            Assert.That(actual, Is.EqualTo(expected), errorMessage);
        }

        /// <summary>
        /// Test cases for WriteTimeout OnPropertyChanged event.
        /// </summary>
        public static IEnumerable WriteTimeout_OnPropertyChanged_TestCases
        {
            get
            {
                return GetPropertyTestCaseData(nameof(NSerialPort.WriteTimeout));
            }
        }

        /// <summary>
        /// Verifies the correct events are raised when WriteBufferSize property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property to verify.</param>
        /// <returns>True if the PropertyChanged event was raised; otherwise, false.</returns>
        [Test]
        [TestCaseSource(nameof(WriteTimeout_OnPropertyChanged_TestCases))]
        public bool WriteTimeout_OnPropertyChanged_Test(string propertyName)
        {
            bool wasCalled = false;
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            (nSerialPort as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
            {
                if (propertyName == nameof(nSerialPort.WriteTimeout))
                {
                    wasCalled = true;
                }
            };

            nSerialPort.WriteTimeout = 456;

            return wasCalled;
        }

        /// <summary>
        /// Verifies that a COMStat struct is returned.
        /// </summary>
        [TestCase]
        public void GetComStat_ReturnsStruct_Test()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            Dcb dcb = new Dcb();
            ComStat comStat = nSerialPort.GetComStat(ref dcb);

            Assert.That(comStat, Is.EqualTo(default(ComStat)));
        }

        /// <summary>
        /// Verifies that the COMStat struct fills in the DCB struct.
        /// </summary>
        [TestCase]
        public void GetComStat_PopulatesDcb()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            Dcb dcb = new Dcb();
            ComStat comStat = nSerialPort.GetComStat(ref dcb);

            Assert.That(dcb, Is.EqualTo(default(Dcb)));
        }

        /// <summary>
        /// Verifies that the close method is called to release unmanaged resources.
        /// </summary>
        [TestCase]
        public void Close_IsCalled_Test()
        {
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            INSerialPort nSerialPort = new NSerialPort(serialPortSub);

            nSerialPort.Close();

            serialPortSub.Received().Close();
        }






        /*
         * TODO: Need to test the following
         *  - If the new DataReceived event is called
         *  - If the new LineReceived event is called
         *  - if the old events are still being called
         *  - If lines are being read from the serial port properly
         *  - If the tranceive methods works
         */

        //[TestCase]
        //public void ErrorReceived_Event_IsRaised()
        //{
        //    bool wasCalled = false;
        //    ISerialPort serialPortSub = Substitute.For<ISerialPort>();
        //    INSerialPort nSerialPort = new NSerialPort(serialPortSub);

        //    nSerialPort.ErrorReceived += (sender, e) => wasCalled = true;

        //    serialPortSub.ErrorReceived += Raise.Event<SerialErrorReceivedEventHandler>(this, System.EventArgs.Empty);

        //    Assert.That(wasCalled, Is.True);
        //}
        

        //[TestCase]
        //public void PinChanged_Event_IsRaised()
        //{
        //    bool wasCalled = false;
        //    ISerialPort serialPortSub = Substitute.For<ISerialPort>();
        //    INSerialPort nSerialPort = new NSerialPort(serialPortSub);

        //    nSerialPort.PinChanged += (sender, e) => wasCalled = true;

        //    serialPortSub.PinChanged += Raise.Event<SerialPinChangedEventHandler>(this, System.EventArgs.Empty);

        //    Assert.That(wasCalled, Is.True);
        //}






    }
}
