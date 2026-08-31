using System;
namespace NullGarel.ByteGaffer;

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
partial class TranscoderSettingAttribute(string settingName) : Attribute
{
    public string SettingName { get; } = settingName;
}