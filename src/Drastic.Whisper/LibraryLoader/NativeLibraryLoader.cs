// Licensed under the MIT license: https://opensource.org/licenses/MIT

using System.Runtime.InteropServices;
using Drastic.Whisper.Native;

namespace Drastic.Whisper.LibraryLoader;

public static class NativeLibraryLoader
{
    private static ILibraryLoader? defaultLibraryLoader = null;

    /// <summary>
    /// Sets the library loader used to load the native libraries. Overwrite this only if you want some custom loading.
    /// </summary>
    /// <param name="libraryLoader">The library loader to be used.</param>
    /// <remarks>
    /// It needs to be set before the first <seealso cref="WhisperFactory"/> is created, otherwise it won't have any effect.
    /// </remarks>
    public static void SetLibraryLoader(ILibraryLoader libraryLoader)
    {
        defaultLibraryLoader = libraryLoader;
    }

    internal static LoadResult LoadNativeLibrary()
    {

        // The library should already be linked and loaded.
#if IOS || MACCATALYST || TVOS || ANDROID
        return LoadResult.Success;
#endif

        var assemblySearchPath = GetAssemblyPath();

        var path = string.Empty;

        var platformPath = string.Empty;

        var platformExtension = string.Empty;

        var libraryName = "whisper";
        ILibraryLoader? libraryLoader = null;

        var architecture = RuntimeInformation.OSArchitecture.ToString().ToLower();

#if MACOS
        platformExtension = "dylib";
        libraryName = "libwhisper";
        libraryLoader = new MacOsLibraryLoader();
        platformPath = "macos";
#elif WINDOWS
        platformPath = $"win-{architecture}";
        platformExtension = "dll";
        libraryLoader = new WindowsLibraryLoader();
#endif

#if NETSTANDARD
        architecture = RuntimeInformation.OSArchitecture switch
        {
            Architecture.X64 => "x64",
            Architecture.X86 => "x86",
            Architecture.Arm => "arm",
            Architecture.Arm64 => "arm64",
            _ => throw new PlatformNotSupportedException($"Unsupported OS platform, architecture: {RuntimeInformation.OSArchitecture}")
        };

        var platform = Environment.OSVersion.Platform.ToString().ToLower();

        platformExtension = platform switch
        {
            "win" => "dll",
            "linux" => "so",
            "osx" => "dylib",
            _ => throw new PlatformNotSupportedException($"Unsupported OS platform, architecture: {RuntimeInformation.OSArchitecture}")
        };

        platformPath = $"{platform}-{architecture}";

        libraryLoader = platform switch
        {
            "win" => new WindowsLibraryLoader(),
            "osx" => new MacOsLibraryLoader(),
            "linux" => new LinuxLibraryLoader(),
            _ => throw new PlatformNotSupportedException($"Currently {platform} platform is not supported")
        };
#endif

        path = Path.Combine(assemblySearchPath, "runtimes", platformPath, $"{libraryName}.{platformExtension}");

        if (defaultLibraryLoader != null)
        {
            return defaultLibraryLoader.OpenLibrary(path);
        }

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Native Library not found in path {path}. Probably it is not supported yet.");
        }

        var result = libraryLoader?.OpenLibrary(path);

        return result ?? LoadResult.Failure("No Library Loader.");
    }

    private static string GetAssemblyPath()
    {
        return new[]
        {
            AppDomain.CurrentDomain.RelativeSearchPath,
            Path.GetDirectoryName(typeof(NativeMethods).Assembly.Location),
            Path.GetDirectoryName(Environment.GetCommandLineArgs()[0])
        }.Where(it => !string.IsNullOrEmpty(it)).FirstOrDefault() ?? string.Empty;
    }
}
