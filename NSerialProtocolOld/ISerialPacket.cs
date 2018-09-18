using NByteStuff;
using NFec;
using System;
using System.Collections.Generic;

namespace NSerialProtocol
{
    public interface ISerialPacket
    {
        IDictionary<string, IField> Fields { get; }

        int Length { get; }
        int MaxLength { get; }
        string Value { get; }
        string RawValue { get; }

        IField StartFlag { get; }
        IField EndFlag { get; }
        IField DynamicLength { get; }
        IField Ecc { get; }

        IDynamicLength DynamicLengthConverter { get; }
        IFec Fec { get; }
        IByteStuff ByteStuff { get; }

        void AddField(string key, IField field);
        void RemoveField(string key);

        void SetStartFlag(string value);
        void SetEndFlag(string value);
        void SetPayload(int startIndex);

        //void SetDynamicLength(int startIndex);
        //void SetDynamicLength(int startIndex, int numberLength,
        //    IDynamicLength dynamicLength);
        //void SetDynamicLength(int startIndex, int numberLength,
        //    Func<int, string> convertIntToStringFunc,
        //    Func<string, int> convertStringToIntFunc);

        void SetDynamicLength(int startIndex, string formatSpecifier);

        void SetFec(IFec fec, int eccStartIndex, int eccLength);

        void SetByteStuff(IByteStuff byteStuff);

        string ComputeEcc();

        bool HasValidEcc(IFec fec);

        ISerialPacket ParseFrame(string frame);
    }
}
