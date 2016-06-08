# Windows Built-in Encoders
## ..in a console app

This is a quickly written, proof of concept to see that native Windows Media Encoders can be called outside UWP.

Reference: [UWP Samples](https://github.com/Microsoft/Windows-universal-samples/tree/master/Samples/MediaTranscoding)

Unfortunately, it turns out that UWP can't write to arbitrary files without a `FilePicker` (because if Store apps could clutter the Desktop/more sensitive places, there goes walled garden security), so a command line interface is out. I could have written to temp, then streamed the result out to stdout or a file, but disk use doubles.

But still, cool to see Windows 10 actually includes an AC3 Encoder/Decoder, even though it won't play DVDs out of the box.

Oh and you can pass FLAC as input.

Usage: `winencode [file] [outputname.ext]`

Always writes to MusicLibrary root because of above reasons.