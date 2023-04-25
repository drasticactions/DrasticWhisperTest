// Licensed under the MIT license: https://opensource.org/licenses/MIT

namespace Drastic.Whisper.LibraryLoader;

public interface ILibraryLoader
{
    LoadResult OpenLibrary(string filename);
}
