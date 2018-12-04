//using DynamicExpresso;
//using NSerialProtocol;
//using NSerialProtocol.EventArgs;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;

//namespace ExpTree
//{
//    public class Program
//    {
//        static void Main(string[] args)
//        {
//            //Do(() => MessageBox.Show("Hello, Do!")).Compile()();

//            //When(() => 4 < 5);

//            // Proof of concept: change 6 to 4 or similar.
//            Extensions.If(() => 6 < 5).Then(() => MessageBox.Show("Hello, Do!"))();

//            FakeProtocol fakeProtocol = new FakeProtocol();
//        }

//        public static void Exp1()
//        {
//            Func<int, bool> func = num => num < 5;
//            Expression<Func<int, bool>> expression = (num) => func(num);

//            var compiled = expression.Compile();

//            bool result = compiled.Invoke(4);
//        }

//        public static void Exp2()
//        {
//            Func<int, bool> func = num =>
//            {
//                return num < 5;
//            };

//            Expression<Func<int, bool>> expression = (num) => func(num);

//            var compiled = expression.Compile();

//            bool result = compiled.Invoke(4);
//        }

//        public static void Exp3()
//        {
//            Action action = () => MessageBox.Show("Hello, expressions!");

//            Func<int, bool> func = num =>
//            {
//                bool b = num < 5;

//                if (b)
//                {
//                    action.Invoke();
//                }

//                return b;
//            };

//            Expression<Func<int, bool>> expression = (num) => func(num);

//            var compiled = expression.Compile();

//            bool result = compiled.Invoke(4);
//        }
//    }

//    public static class Extensions
//    {
//        public static Func<bool> If(Func<bool> isTrue)
//        {
//            Expression<Func<bool>> expression = () => isTrue();

//            return expression.Compile();
//        }

//        public static Action Then(this Func<bool> predicate, Action action)
//        {
//            Expression<Action> expression = () => IfThen(predicate, action);

//            return expression.Compile();
//        }

//        private static void IfThen(Func<bool> predicate, Action action)
//        {
//            if (predicate())
//                action();
//        }
//    }

//    public class FakeProtocol
//    {
//        public delegate void SerialFrameParsedEventHandler(object sender, SerialFrameParsedEventArgs e);
//        public delegate void SerialFrameErrorEventHandler(object sender, SerialFrameErrorEventArgs e);
//        public delegate SerialFrame SerialFrameReceivedEventHandler(object sender, SerialFrameReceivedEventArgs e);
//        //public delegate void SerialPacketReceivedEventHandler(object sender, SerialPacketReceivedEventArgs e);

//        public event SerialFrameParsedEventHandler SerialFrameParsed;
//        public event SerialFrameErrorEventHandler SerialFrameError;
//        public event SerialFrameReceivedEventHandler SerialFrameReceived;

//        public EventRouter<SerialFrameReceivedEventArgs> FrameReceivedEventRouter;

//        public FakeProtocol()
//        {
//            FrameReceivedEventRouter = new EventRouter<SerialFrameReceivedEventArgs>(this);
//        }

//        public SerialFrame WaitFrame()
//        {
//            return new SerialFrame();
//        }


//    }

//    public static class ProtocolExtensions
//    {
//        public static Func<SerialFrame, bool> If2(this FakeProtocol protocol, Func<SerialFrame, bool> isTrue)
//        {
//            Expression<Func<SerialFrame, bool>> expression = (s) => isTrue(s);

//            Func<SerialFrame, bool> func = expression.Compile();

//            //protocol.SerialFrameReceived += (s, e) => func(); ;

//            return expression.Compile();
//        }

//        public static Action Then2(this Func<bool> predicate, Action action)
//        {
//            Expression<Action> expression = () => IfThen(predicate, action);

//            return expression.Compile();
//        }

//        private static void IfThen(Func<bool> predicate, Action action)
//        {
//            if (predicate())
//                action();
//        }

//        public static void WaitForFrame(this FakeProtocol protocol, Type frameType)
//        {
//            protocol.WaitFrame();
//        }
//    }

//    public class EventRouter<T>
//    {
//        public List<Action<object, T>> Routes { get; } = new List<Action<object, T>>();

//        public EventRouter(FakeProtocol protocol)
//        {
//            //protocol.SerialFrameReceived += (s, e) => { };
//        }

//        private void OnSerialFrameReceived(object sender, T eventArgs)
//        {
//            for (int i = 0; i < Routes.Count; i++)
//            {
//                Routes[i].Invoke(sender, eventArgs);
//            }
//        }

//        public Action AddRoute()
//        {
//            throw new Exception();
//        }
//    }
//}
