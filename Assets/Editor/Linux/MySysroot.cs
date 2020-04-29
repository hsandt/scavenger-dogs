// Fix for Sysroot + clang not found issue on Release Build on Linux
// https://forum.unity.com/threads/unity-2019-3-linux-il2cpp-player-can-only-be-built-with-linux-error.822210/#post-5631169

#if UNITY_EDITOR_LINUX

using System;
using System.Collections.Generic;

namespace UnityEditor.LinuxStandalone
{
    public class MySysroot : Sysroot
    {
        public override string Name           => "MySysroot";
        public override string HostPlatform   => "linux";
        public override string HostArch       => "x86_64";
        public override string TargetPlatform => "linux";
        public override string TargetArch     => "x86_64";
 
        public override bool Initialize() { return true; }
 
        public override IEnumerable<string> GetIl2CppArguments()
        {
            yield return "--sysroot-path=/";
            yield return "--tool-chain-path=/usr/lib/llvm-6.0";
        }
    }
}

#endif
