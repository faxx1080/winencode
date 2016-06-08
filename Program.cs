using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Foundation;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Media.Transcoding;

namespace winencode
{
    class Program
    {
        static Windows.Media.MediaProperties.MediaEncodingProfile _Profile;
        static Windows.Storage.StorageFile _InputFile = null;
        static Windows.Storage.StorageFile _OutputFile = null;
        static Windows.Media.Transcoding.MediaTranscoder _Transcoder = new Windows.Media.Transcoding.MediaTranscoder();
        
        static void Main(string[] args)
        {
            
            GetProfile();
            Task<IAsyncOperation<StorageFile>> task2 = new Task<IAsyncOperation<StorageFile>>(() =>
            {
                return StorageFile.GetFileFromPathAsync(args[0]);
            });
            task2.RunSynchronously();
            StorageFile _InputFile = task2.Result.GetResults();
            // This is going to be nasty.
            // Output can only be to known directories.

            var openPicker = new Windows.Storage.Pickers.FolderPicker();

            //var task = Task.Factory.StartNew<IAsyncOperation<StorageFolder>>(() => openPicker.PickSingleFolderAsync());
            //task.Wait();
            //var res = task.Result.GetResults();
            var res = KnownFolders.MusicLibrary;

            // var listToken = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(res);

            var task3 = Task.Factory.StartNew<IAsyncOperation<StorageFile>>(() => res.CreateFileAsync(args[1], CreationCollisionOption.GenerateUniqueName));
            task3.Wait();
            _OutputFile = task3.Result.GetResults();

            var task4 = Task.Factory.StartNew<IAsyncOperation<PrepareTranscodeResult>>(() => _Transcoder.PrepareFileTranscodeAsync(_InputFile, _OutputFile, _Profile));
            task4.Wait();
            var r4 = task4.Result.GetResults();

            if (r4.CanTranscode)
            {
                var task5 = Task.Factory.StartNew(() => r4.TranscodeAsync());
                task5.Wait();
            }

        }
        
        static void GetProfile()
        {
            try
            {
                _Profile = new MediaEncodingProfile();
                /*
                "3GP"	3GP file.
                "AC3"	AC-3 audio.
                "ADTS"	Audio Data Transport Stream (ADTS) stream.
                "MP3"	MPEG Audio Layer-3 (MP3).
                "MPEG2PS"	MPEG-2 program stream.
                "MPEG2TS"	MPEG-2 transport stream.
                "MPEG4"	MP4 file container.
                 */
                _Profile.Container.Subtype = "MPEG4";
                _Profile.Audio.BitsPerSample = UInt32.Parse("16");
                _Profile.Audio.SampleRate = UInt32.Parse("44100");
                _Profile.Audio.Bitrate = UInt32.Parse("256000");
                _Profile.Audio.ChannelCount = UInt32.Parse("2");
                _Profile.Audio.Subtype = "AAC";
                /*
                "AAC"	Advanced Audio Coding (AAC). The stream can contain either raw
                        AAC data or AAC data in an Audio Data Transport Stream (ADTS) stream.
                "AC3"	Dolby Digital audio (AC-3).
                "EAC3"	Dolby Digital Plus audio (E-AC-3).
                "MP3"	MPEG Audio Layer-3 (MP3).
                "MPEG"	MPEG-1 audio payload.
                "PCM"	Uncompressed 16-bit PCM audio.
                "Float"	Uncompressed 32-bit float PCM audio.
                "WMA8"	Windows Media Audio 8 codec, Windows Media Audio 9 codec, or Windows Media Audio 9.1 codec.
                "WMA9"	Windows Media Audio 9 Professional codec or Windows Media Audio 9.1 Professional codec.
                "ADTS"	Audio Data Transport Stream
                "AACADTS"	Advanced Audio Coding (AAC) audio in Audio Data Transport Stream (ADTS) format.
                "AMRNB"	Adaptive Multi-Rate audio codec (AMR-NB)
                "AWRWB"	Adaptive Multi-Rate Wideband audio codec (AMR-WB)
                 */
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception.Message);
                Environment.Exit(1);
            }
        }
    }
}
