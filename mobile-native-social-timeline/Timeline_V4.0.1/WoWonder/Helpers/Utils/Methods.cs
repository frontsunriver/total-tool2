using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AFollestad.MaterialDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Database;
using Android.Graphics;
using Android.Media;
using Android.Net;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using AndroidHUD;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using AndroidX.Lifecycle;
using AndroidX.RecyclerView.Widget;
using Java.IO;
using Java.Lang;
using Java.Security;
using Newtonsoft.Json;
using WoWonder.Helpers.Controller;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Message;
using Calendar = Android.Icu.Util.Calendar;
using ClipboardManager = Android.Content.ClipboardManager;
using Console = System.Console;
using Environment = System.Environment;
using Exception = System.Exception;
using File = Java.IO.File;
using Process = Android.OS.Process;
using Random = System.Random;
using Stream = System.IO.Stream;
using Thread = System.Threading.Thread;
using Uri = Android.Net.Uri;
using MimeTypeMap = WoWonderClient.MimeTypeMap;
using IOException = System.IO.IOException;
using Notification = Android.App.Notification;
using TransportType = Android.Net.TransportType;

namespace WoWonder.Helpers.Utils
{
    public static partial class Methods
    {
        //########################## IMethods Application Version 3.0 ##########################
    }

    public static partial class Methods
    {
        #region Methods

        //Checks for Internet connection 
        public static bool CheckConnectivity()
        {
            try
            {
                ConnectivityManager cm = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
                switch ((int)Build.VERSION.SdkInt)
                {
                    case <= 25:
                        {
                            #pragma warning disable 618
                            var activeNetwork = cm?.ActiveNetworkInfo;
                            #pragma warning restore 618
                            if (activeNetwork != null)
                            {
                                #pragma warning disable 618
                                bool isOnline = activeNetwork.IsConnected;
                                #pragma warning restore 618
                                return isOnline;
                            }

                            break;
                        }
                    default:
                        {
                            NetworkCapabilities capabilities = cm.GetNetworkCapabilities(cm.ActiveNetwork);
                            if (capabilities != null)
                            {
                                if (capabilities.HasTransport(TransportType.Cellular) || capabilities.HasTransport(TransportType.Wifi) || capabilities.HasTransport(TransportType.Ethernet) || capabilities.HasTransport(TransportType.Vpn))
                                    return true;
                            }

                            break;
                        }
                }

                return false;
            }
            catch (Exception exception)
            {
                DisplayReportResultTrack(exception);
                return false;
            }
        }

        public static void SetFocusable(View v)
        {
            try
            {
                switch (v)
                {
                    case null:
                        return;
                }
                v.Focusable = true;
                v.FocusableInTouchMode = true;
                v.ClearFocus();
                switch ((int)Build.VERSION.SdkInt)
                {
                    case >= 23:
                        v.SetFocusable(ViewFocusability.NotFocusable);
                        break;
                }
            }
            catch (Exception e)
            {
                DisplayReportResultTrack(e);
            }
        }

        public static void SetColorEditText(EditText v, Color color)
        {
            try
            {
                switch (v)
                {
                    case null:
                        return;
                    default:
                        v.SetTextColor(color);
                        v.SetHintTextColor(Color.ParseColor("#444444"));
                        break;
                }
            }
            catch (Exception e)
            {
                DisplayReportResultTrack(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        public static void SetMargin(View v, int left, int top, int right, int bottom)
        {
            try
            {
                switch (v)
                {
                    case null:
                        return;
                    default:
                        switch (v.LayoutParameters)
                        {
                            case RecyclerView.LayoutParams parameter2:
                                parameter2.SetMargins(left, top, right, bottom); // left, top, right, bottom
                                v.LayoutParameters = parameter2;
                                break;
                            case RelativeLayout.LayoutParams parameter:
                                parameter.SetMargins(left, top, right, bottom); // left, top, right, bottom
                                v.LayoutParameters = parameter;
                                break;
                            case LinearLayout.LayoutParams parameter3:
                                parameter3.SetMargins(left, top, right, bottom); // left, top, right, bottom
                                v.LayoutParameters = parameter3;
                                break;
                        }

                        break;
                }
            }
            catch (Exception e)
            {
                DisplayReportResultTrack(e);
            }
        }

        public static void GenerateNoteOnSD(string sBody)
        {
            try
            {
                string personalFolder = Path.AndroidDcimFolder;

                var root = new File(personalFolder, ".WoWLU");
                if (!root.Exists())
                    root.Mkdirs();

                File file = new File(root, ".LU.txt");
                if (file.Exists())
                {
                    // set to true if you want to append contents to text file
                    // set to false if you want to remove preivous content of text file
                    FileWriter textFileWriter = new FileWriter(file, false);

                    BufferedWriter outWriter = new BufferedWriter(textFileWriter);

                    // create the content string
                    string contentString = new string(sBody);

                    // write the updated content
                    outWriter.Write(contentString);
                    outWriter.Close();

                    Console.WriteLine("File was updated.");
                }
                else
                {
                    Console.WriteLine("Cannot update.File does not exist.");

                    FileWriter writer = new FileWriter(file);
                    writer.Append(sBody);
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (IOException e)
            {
                DisplayReportResultTrack(e);
            }
        }

        public static string ReadNoteOnSD()
        {
            try
            {
                //Find the directory for the SD Card using the API
                //*Don't* hardcode "/sdcard"
                string personalFolder = Path.AndroidDcimFolder;

                var root = new File(personalFolder, ".WoWLU");
                if (!root.Exists())
                    root.Mkdirs();

                //Get the text file
                File file = new File(root, ".LU.txt");
                if (file.Exists())
                {
                    //Read text from file
                    var text = new StringBuilder();
                    try
                    {
                        BufferedReader br = new BufferedReader(new FileReader(file));
                        string line;

                        while ((line = br.ReadLine()) != null)
                        {
                            text.Append(line);
                        }
                        br.Close();
                    }
                    catch (IOException e)
                    {
                        //You'll need to add proper error handling here
                        DisplayReportResultTrack(e);
                    }

                    //Set the text
                    return text.ToString();
                }
                return "";
            }
            catch (IOException e)
            {
                DisplayReportResultTrack(e);
                return "";
            }
        }

        public static string DeleteNoteOnSD()
        {
            try
            {
                //Find the directory for the SD Card using the API
                //*Don't* hardcode "/sdcard"
                string personalFolder = Path.AndroidDcimFolder;

                var root = new File(personalFolder, ".WoWLU");
                if (!root.Exists())
                    root.Mkdirs();

                //Get the text file
                File file = new File(root, ".LU.txt");
                if (file.Exists())
                {
                    file.Delete();
                    if (file.Exists())
                    {
                        file.CanonicalFile.Delete();
                        if (file.Exists())
                        {
                            Application.Context.DeleteFile(file.Name);
                        }
                    }
                }
                return "";
            }
            catch (IOException e)
            {
                DisplayReportResultTrack(e);
                return "";
            }
        }


        public static void Set_SoundPlay(string typeUri)
        {
            try
            {
                //Type_uri >>  mystic_call - Popup_GetMesseges - Popup_SendMesseges 
                var uri = Uri.Parse("android.resource://" + Application.Context.PackageName + "/raw/" +
                                    typeUri);

                RingtoneManager.GetRingtone(Application.Context, uri).Play();
                //RingtoneManager.GetRingtone(Application.Context, uri).Play();
            }
            catch (Exception exception)
            {
                DisplayReportResultTrack(exception);
            }
        }

        public static void DisplayReportResult(Activity activityContext, dynamic respond, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            string errorText;
            switch (respond)
            {
                case ErrorObject error:
                    {
                        errorText = error.Error.ErrorText;

                        if (errorText.Contains("Invalid or expired access_token") || errorText.Contains("No session sent") || errorText.Contains("Not authorized"))
                            ApiRequest.Logout(activityContext);
                        break;
                    }
                default:
                    errorText = respond.ToString();
                    break;
            }

            System.Diagnostics.Trace.WriteLine("ReportMode >> message: " + errorText);
            System.Diagnostics.Trace.WriteLine("ReportMode >> member name: " + memberName);
            System.Diagnostics.Trace.WriteLine("ReportMode >> source file path: " + sourceFilePath);
            System.Diagnostics.Trace.WriteLine("ReportMode >> source line number: " + sourceLineNumber);

            switch (AppSettings.SetApisReportMode)
            {
                case true when !errorText.Contains("com.android.okhttp"):
                    DialogPopup.InvokeAndShowDialog(activityContext, "ReportMode", errorText, "Close");
                    break;
            }
            //Crashes.TrackError(new Exception(errorText));
            //Analytics.TrackEvent(errorText);

            throw new Exception(errorText);
        }

        public static void DisplayAndHudErrorResult(Activity activityContext, dynamic respond, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            string errorText = respond.ToString();
            try
            {
                switch (respond)
                {
                    case ErrorObject error:
                        {
                            errorText = error.Error.ErrorText;

                            if (errorText.Contains("Invalid or expired access_token") || errorText.Contains("No session sent") || errorText.Contains("Not authorized"))
                                ApiRequest.Logout(activityContext);
                            break;
                        }
                    default:
                        errorText = respond.ToString();
                        break;
                }
                //Show a Error 
                AndHUD.Shared.ShowError(activityContext, errorText, MaskType.Clear, TimeSpan.FromSeconds(1));

                System.Diagnostics.Trace.WriteLine("ReportMode >> message: " + errorText);
                System.Diagnostics.Trace.WriteLine("ReportMode >> member name: " + memberName);
                System.Diagnostics.Trace.WriteLine("ReportMode >> source file path: " + sourceFilePath);
                System.Diagnostics.Trace.WriteLine("ReportMode >> source line number: " + sourceLineNumber);
                //Crashes.TrackError(new Exception(errorText));
                //Analytics.TrackEvent(errorText);
            }
            catch (Exception ex)
            {
                DisplayReportResultTrack(ex);
                AndHUD.Shared.Dismiss(activityContext);
            }

            switch (AppSettings.SetApisReportMode)
            {
                case true:
                    DialogPopup.InvokeAndShowDialog(activityContext, "ReportMode", errorText, "Close");
                    //throw new Exception(errorText);
                    break;
            }
        }

        public static void DisplayReportResultTrack(Exception exception, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            try
            {
                System.Diagnostics.Trace.WriteLine("ReportMode >> message: " + exception.Message + " \n  " + exception.StackTrace);
                System.Diagnostics.Trace.WriteLine("ReportMode >> member name: " + memberName);
                System.Diagnostics.Trace.WriteLine("ReportMode >> source file path: " + sourceFilePath);
                System.Diagnostics.Trace.WriteLine("ReportMode >> source line number: " + sourceLineNumber);

                string text = "ReportMode >> message: " + exception.Message + " \n  " + exception.StackTrace;
                text += "\n \n ReportMode >> member name: " + memberName;
                text += "\n \n ReportMode >> source file path: " + sourceFilePath;
                text += "\n \n ReportMode >> source line number: " + sourceLineNumber;

                switch (AppSettings.SetApisReportMode)
                {
                    case true when !exception.Message.Contains("com.android.okhttp"):
                        DialogPopup.InvokeAndShowDialog(MainApplication.GetInstance().Activity, "ReportMode", text, "Close");
                        break;
                }
                //Crashes.TrackError(exception);
                //Analytics.TrackEvent(text);
            }
            catch (Exception xx)
            {
                DisplayReportResultTrack(xx);
            }
        }


        public static void CopyToClipboard(Activity activityContext, string text)
        {
            try
            {
                var clipboardManager = (ClipboardManager)activityContext.GetSystemService(Context.ClipboardService);

                var clipData = ClipData.NewPlainText("text", text);
                clipboardManager.PrimaryClip = clipData;

                //Toast.MakeText(activityContext, activityContext.GetText(Resource.String.Lbl_Text_copied), ToastLength.Short)?.Show();
            }
            catch (Exception exception)
            {
                DisplayReportResultTrack(exception);
            }
        }

        public static string GetTimestamp(DateTime value)
        {
            try
            {
                Console.WriteLine(value);
                return DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            }
            catch (Exception e)
            {
                Toast.MakeText(Application.Context, e.Message, ToastLength.Short)?.Show();
                DisplayReportResultTrack(e);
                return DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            }
        }

        public static byte[] ConvertFileToByteArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using MemoryStream ms = new MemoryStream();
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                ms.Write(buffer, 0, read);
            return ms.ToArray();
        }

        #endregion

        #region Audio Record & Play

        public class AudioRecorderAndPlayer
        {
            public MediaPlayer Player { get; private set; }
            private static MediaPlayer PlayerStatic { get; set; }

            private readonly string SoundFile;
            private string AudioFileFullPathReleased;
            private readonly File SoundFileFullPath;
            private MediaRecorder Recorder;

            public AudioRecorderAndPlayer(string id)
            {
                try
                {
                    Player = InitializeMediaPlayer();
                    Path.Chack_MyFolder();
                    //_audio.3gp
                    SoundFile = GetTimestamp(DateTime.Now) + "_audio.wav";
                    Console.WriteLine("audio File Name Released : " + SoundFile);
                    SoundFileFullPath = new File(Path.FolderDcimSound + "/" + id + "/" + SoundFile);

                    var dir = Path.FolderDcimSound + "/" + id;
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }

            public static MediaPlayer InitializeMediaPlayer()
            {
                try
                {
                    var player = new MediaPlayer();
                    player.SetAudioAttributes(new AudioAttributes.Builder()?.SetUsage(AudioUsageKind.Media)?.SetContentType(AudioContentType.Music)?.Build());

                    //if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    //{
                    //    player.SetAudioAttributes(new AudioAttributes.Builder()
                    //        .SetUsage(AudioUsageKind.Media)
                    //        .SetContentType(AudioContentType.Music)
                    //        .SetLegacyStreamType(Android.Media.Stream.Music)
                    //        .Build());
                    //}
                    //else
                    //{
                    //    #pragma warning disable 618
                    //    player.SetAudioStreamType(Android.Media.Stream.Music);
                    //    #pragma warning restore 618
                    //} 

                    return player;
                }
                catch (Exception e)
                {
                    DisplayReportResultTrack(e);
                    return null!;
                }
            }

            public static string Get_MediaFileDuration(string path)
            {
                MediaPlayer mp = InitializeMediaPlayer();

                try
                {
                    if (path.Contains("http"))
                    {
                        mp?.SetDataSource(Application.Context, Uri.Parse(path));
                        mp?.PrepareAsync();
                    }
                    else
                    {
                        File file2 = new File(path);
                        var photoUri = FileProvider.GetUriForFile(Application.Context, Application.Context.PackageName + ".fileprovider", file2);
                        mp?.SetDataSource(Application.Context, photoUri);
                        mp?.PrepareAsync();
                    }

                    return mp?.Duration != 0 ? mp?.Duration.ToString() : "00";
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                    return "0";
                }
                finally
                {
                    mp?.Release();
                }
            }

            public void StartRecording()
            {
                try
                {
                    if (Recorder != null)
                        StopRecording();

                    Recorder = new MediaRecorder();
                    Recorder.SetAudioSource(AudioSource.Mic);
                    Recorder.SetOutputFormat(OutputFormat.ThreeGpp);
                    Recorder.SetAudioEncoder(AudioEncoder.AmrNb);
                    Recorder.SetOutputFile(SoundFileFullPath.AbsolutePath);

                    try
                    {
                        Recorder.Prepare();
                    }
                    catch (IOException e)
                    {
                        DisplayReportResultTrack(e);
                    }

                    Recorder.Start();
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }

            public void StopRecording()
            {
                try
                {
                    if (Recorder != null)
                    {
                        Recorder.Release();
                        Recorder = null;
                    }

                    AudioFileFullPathReleased = SoundFileFullPath.AbsolutePath;
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }

            public string GetRecorded_Sound_Path()
            {
                if (System.IO.File.Exists(SoundFileFullPath.AbsolutePath))
                {
                    return SoundFileFullPath.AbsolutePath;
                }

                return string.Empty;
            }

            public static string Check_Sound_File_if_Exits(string folderName, string soundFile)
            {
                var soundFileFullPath = Path.AndroidDcimFolder + "/" + folderName + "/" + soundFile;
                if (System.IO.File.Exists(soundFileFullPath))
                {
                    return soundFileFullPath;
                }

                return "File Dont Exists";
            }

            public Stream GetSound_as_Stream(string path)
            {
                if (System.IO.File.Exists(path))
                {
                    byte[] databyte = System.IO.File.ReadAllBytes(path);
                    Console.WriteLine(databyte);
                    Stream stream = System.IO.File.OpenRead(path);

                    return stream;
                }

                return null!;
            }

            public string Delete_Sound_Path(string path)
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);

                    return "Deleted";
                }

                return "Not exits";
            }

            public static void PlayAudioFromAsset(string fileName, string typeVolume = "right")
            {
                try
                {
                    PlayerStatic = InitializeMediaPlayer();
                    var fd = Application.Context?.Assets?.OpenFd(fileName);

                    PlayerStatic.Prepared += (s, e) =>
                    {
                        try
                        {
                            PlayerStatic.Start();
                        }
                        catch (Exception exception)
                        {
                            DisplayReportResultTrack(exception);
                        }
                    };

                    switch (typeVolume)
                    {
                        case "left":
                            PlayerStatic.SetAudioAttributes(new AudioAttributes.Builder()?.SetUsage(AudioUsageKind.VoiceCommunication)?.SetContentType(AudioContentType.Music)?.Build());
                            PlayerStatic.Looping = true;
                            break;
                    }

                    PlayerStatic.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
                    PlayerStatic.PrepareAsync();
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }

            public static void StopAudioFromAsset()
            {
                try
                {
                    switch (PlayerStatic.IsPlaying)
                    {
                        case true:
                            PlayerStatic.Stop();
                            break;
                    }
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }

            public void PlayAudioFromPath(string audioPath)
            {
                try
                {
                    Player.Completion += (sender, e) => { Player.Reset(); };

                    Player.SetDataSource(audioPath);
                    Player.PrepareAsync();
                    Player.Prepared += (s, e) => { Player.Start(); };
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }


            public void StopAudioPlay()
            {
                switch (Player.IsPlaying)
                {
                    case true:
                        Player.Stop();
                        break;
                }
            }

            public void PauseAudioPlay()
            {
                switch (Player.IsPlaying)
                {
                    case true:
                        Player.Pause();
                        break;
                }
            }

            public static string GetTimeString(string millisString)
            {
                try
                {
                    string finalTimerString = "";
                    string secondsString, minutsString;

                    var millis = Convert.ToInt32(millisString);

                    int hours = (int)(millis / (1000 * 60 * 60));
                    int minutes = (int)(millis % (1000 * 60 * 60) / (1000 * 60));
                    int seconds = (int)(millis % (1000 * 60 * 60) % (1000 * 60) / 1000);

                    finalTimerString = hours switch
                    {
                        // Add hours if there
                        > 0 => hours + ":",
                        _ => finalTimerString
                    };

                    secondsString = seconds switch
                    {
                        // Prepending 0 to seconds if it is one digit
                        < 10 => "0" + seconds,
                        _ => "" + seconds
                    };

                    minutsString = minutes switch
                    {
                        < 10 => "0" + minutes,
                        _ => "" + minutes
                    };

                    finalTimerString = finalTimerString + minutsString + ":" + secondsString;

                    return finalTimerString;
                }
                catch (Exception e)
                {
                    DisplayReportResultTrack(e);
                    return "";
                }
            }
        }

        #endregion

        #region Images And video

        public static class MultiMedia
        {
            public static void Save_Images_CostomName(string savedfoldername, string fileUrl, string typeimage, string imageid)
            {
                try
                {
                    string filename = imageid + "_" + typeimage + ".jpg";
                    string filePath = System.IO.Path.Combine(savedfoldername);
                    string mediaFile = filePath + "/" + filename;

                    if (!System.IO.File.Exists(mediaFile))
                    {
                        if (!Directory.Exists(filePath))
                            Directory.CreateDirectory(filePath);

                        using WebClient web = new WebClient();
                        web.DownloadDataAsync(new System.Uri(fileUrl), mediaFile);

                        web.DownloadDataCompleted += (s, e) =>
                        {
                            try
                            {
                                System.IO.File.WriteAllBytes(mediaFile, e.Result);
                            }
                            catch (Exception exception)
                            {
                                DisplayReportResultTrack(exception);
                            }
                        };
                    }
                }
                catch (Exception e)
                {
                    DisplayReportResultTrack(e);
                }
            }

            public static string Get_Images_CostomName(string savedfoldername, string typeimage, string imageid)
            {
                try
                {
                    string filename = imageid + "_" + typeimage + ".jpg";

                    string fileUrl = GetMediaFrom_Disk(savedfoldername, filename);
                    return fileUrl;
                }
                catch (Exception e)
                {
                    DisplayReportResultTrack(e);
                    return "File Dont Exists";
                }
            }

            public static string GetMediaFrom_Disk(string foldername, string filename)
            {
                try
                {
                    string file = foldername + "/" + filename;
                    if (System.IO.File.Exists(file))
                    {
                        FileInfo fi = new FileInfo(file);
                        Console.WriteLine("size" + fi.Length);
                        FileInfo fileVol = new FileInfo(file);
                        string fileLength = fileVol.Length.ToString();
                        Console.WriteLine("size" + fileLength);
                        return file;
                    }

                    return "File Dont Exists";
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                    return "File Dont Exists";
                }
            }

            public static string GetMediaFrom_Gallery(string foldername, string filename)
            {
                try
                {
                    string filePath = System.IO.Path.Combine(foldername);
                    string mediaFile = filePath + "/" + filename;

                    File file = new File(foldername, filename);
                    if (file.Exists())
                    {
                        return mediaFile;
                    }

                    //if (System.IO.File.Exists(mediaFile))
                    //{
                    //    return mediaFile;
                    //}

                    return "File Dont Exists";
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                    return "File Dont Exists";
                }
            }

            public static void DeleteMediaFrom_Disk(string path)
            {
                try
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }

            public static string CheckFileIfExits(string filepath)
            {
                try
                {
                    if (System.IO.File.Exists(filepath))
                    {
                        return filepath;
                    }

                    return "File Dont Exists";
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                    return "File Dont Exists";
                }
            }

            public static string CopyMediaFileTo(string pathOfFile, string toFolderName, bool saveOnPersonalFolder = true, bool saveOnGallaryFolder = false)
            {
                //Change the file name to new unique name
                string fileName = pathOfFile.Contains("/")
                    ? pathOfFile.Split('/').Last()
                    : pathOfFile.Split('\\').Last();
                string extension = fileName.Split('.').Last();
                fileName = fileName.Split('.').First();
                fileName = fileName.Replace(fileName, GetTimestamp(DateTime.Now)) + "." + extension;

                string newFolderPath = System.IO.Path.Combine(toFolderName);
                string copyFileFullPath = newFolderPath + "/" + fileName;

                switch (saveOnPersonalFolder)
                {
                    case true:
                        {
                            if (!Directory.Exists(newFolderPath))
                                Directory.CreateDirectory(newFolderPath);

                            if (System.IO.File.Exists(pathOfFile))
                            {
                                System.IO.File.Copy(pathOfFile, copyFileFullPath);
                                return copyFileFullPath;
                            }

                            return "Path File Dont exits";
                        }
                }

                switch (saveOnGallaryFolder)
                {
                    case true:
                        {
                            newFolderPath = System.IO.Path.Combine(toFolderName);
                            copyFileFullPath = newFolderPath + "/" + fileName;

                            if (!Directory.Exists(newFolderPath))
                                Directory.CreateDirectory(newFolderPath);

                            if (System.IO.File.Exists(pathOfFile))
                            {
                                System.IO.File.Copy(pathOfFile, copyFileFullPath);
                                //var mediaScanIntent = new Intent(Intent?.ActionMediaScannerScanFile);
                                //mediaScanIntent?.SetData(Uri.FromFile(new File(copyFileFullPath)));
                                //Application.Context.SendBroadcast(mediaScanIntent);

                                // Tell the media scanner about the new file so that it is
                                // immediately available to the user.
                                MediaScannerConnection.ScanFile(Application.Context, new[] { pathOfFile }, null, null);


                                return copyFileFullPath;
                            }

                            //File.Copy(pathOfFile, CopyFileFullPath);
                            return "Path File Dont exits";
                        }
                    default:
                        return "Path File Dont exits";
                }
            }

            public static void DownloadMediaTo_DiskAsync(string savedfoldername, string url)
            {
                try
                {
                    if (url.Contains("http"))
                    {
                        string filename = url.Split('/').Last();
                        string filePath = System.IO.Path.Combine(savedfoldername);
                        string mediaFile = filePath + "/" + filename;

                        if (!Directory.Exists(filePath))
                            Directory.CreateDirectory(filePath);

                        if (!System.IO.File.Exists(mediaFile))
                        {
                            WebClient webClient = new WebClient();

                            webClient.DownloadDataAsync(new System.Uri(url), mediaFile);

                            webClient.DownloadDataCompleted += (s, e) =>
                            {
                                try
                                {
                                    System.IO.File.WriteAllBytes(mediaFile, e.Result);
                                }
                                catch (Exception exception)
                                {
                                    DisplayReportResultTrack(exception);
                                }
                            };
                        }
                    }
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }

            public static void DownloadMediaTo_GalleryAsync(string savedfoldername, string url)
            {
                try
                {
                    string filename = url.Split('/').Last();
                    string filePath = System.IO.Path.Combine(savedfoldername);
                    string mediaFile = filePath + "/" + filename;

                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);

                    if (!System.IO.File.Exists(mediaFile))
                    {
                        WebClient webClient = new WebClient();

                        webClient.DownloadDataAsync(new System.Uri(url));
                        webClient.DownloadDataCompleted += (s, e) =>
                        {
                            try
                            {
                                System.IO.File.WriteAllBytes(mediaFile, e.Result);
                                //var mediaScanIntent = new Intent(Intent?.ActionMediaScannerScanFile);
                                //mediaScanIntent?.SetData(Uri.FromFile(new File(mediaFile)));
                                //Application.Context.SendBroadcast(mediaScanIntent);

                                // Tell the media scanner about the new file so that it is
                                // immediately available to the user.
                                MediaScannerConnection.ScanFile(Application.Context, new[] { mediaFile }, null, null);
                            }
                            catch (Exception exception)
                            {
                                DisplayReportResultTrack(exception);
                            }

                        };
                    }
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }

            public static bool IsCameraAvailable()
            {
                PackageManager pm = Application.Context.PackageManager;
                if (pm.HasSystemFeature(PackageManager.FeatureCamera))
                    return true;

                return false;
            }

            public static Bitmap Retrieve_VideoFrame_AsBitmap(Context context, string mediaFile, ThumbnailKind thumbnailKind = ThumbnailKind.MiniKind)
            {
                switch (mediaFile)
                {
                    case null:
                        return null!;
                    default:
                        try
                        {
                            Bitmap bitmap;
                            switch ((int)Build.VERSION.SdkInt)
                            {
                                case >= 29 when mediaFile.Contains("http://") || mediaFile.Contains("https://"):
                                    //bitmap = context.ContentResolver?.LoadThumbnail(Uri.Parse(mediaFile), new Size(200, 200), null);
#pragma warning disable 618
                                    bitmap = ThumbnailUtils.CreateVideoThumbnail(mediaFile, thumbnailKind);
#pragma warning restore 618
                                    break;
                                case >= 29:
                                    {
                                        File file2 = new File(mediaFile);
                                        bitmap = ThumbnailUtils.CreateVideoThumbnail(file2, new Size(200, 200), null);
                                        break;
                                    }
                                default:
                                    {
                                        var filepath = AttachmentFiles.GetActualPathFromFile(context, Uri.Parse(mediaFile));
                                        if (filepath != null)
                                        {
#pragma warning disable 618
                                            bitmap = ThumbnailUtils.CreateVideoThumbnail(filepath, thumbnailKind);
#pragma warning restore 618
                                            return bitmap;
                                        }

#pragma warning disable 618
                                        bitmap = ThumbnailUtils.CreateVideoThumbnail(mediaFile, thumbnailKind);
#pragma warning restore 618
                                        break;
                                    }
                            }

                            return bitmap;
                        }
                        catch (Exception exception)
                        {
                            DisplayReportResultTrack(exception);
                            return null!;
                        }
                }
            }

            public static string Export_Bitmap_As_Image(Bitmap bitmap, string filename, string pathTofolder)
            {
                try
                {
                    if (!Directory.Exists(pathTofolder))
                        Directory.CreateDirectory(pathTofolder);

                    string filePath = System.IO.Path.Combine(pathTofolder);
                    string mediaFile = filePath + "/" + filename + ".png";
                    var stream = new FileStream(mediaFile, FileMode.Create);
                    bitmap?.Compress(Bitmap.CompressFormat.Png, 100, stream);
                    stream.Close();

                    return mediaFile;
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                    return "";
                }
            }

            public static Stream GetMedia_as_Stream(string path)
            {
                try
                {
                    byte[] datass = System.IO.File.ReadAllBytes(path);
                    Console.WriteLine(datass);
                    Stream dsd = System.IO.File.OpenRead(path);
                    return dsd;
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                    return null!;
                }
            }

            public static void image_compression(string path)
            {
                try
                {
                    string anyString = System.IO.File.ReadAllText(path);
                    CompressStringToFile("new.gz", anyString);
                }
                catch (Exception exception) // Couldn't compress.
                {
                    DisplayReportResultTrack(exception);
                }
            }

            public static void CompressStringToFile(string fileName, string value)
            {
                try
                {
                    string temp = System.IO.Path.GetTempFileName();
                    System.IO.File.WriteAllText(temp, value);
                    byte[] b;
                    using (FileStream f = new FileStream(temp, FileMode.Open))
                    {
                        b = new byte[f.Length];
                        f.Read(b, 0, (int)f.Length);
                    }

                    using FileStream f2 = new FileStream(fileName, FileMode.Create);
                    using GZipStream gz = new GZipStream(f2, CompressionMode.Compress, false);
                    gz.Write(b, 0, b.Length);
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }
        }

        #endregion

        #region Contacts

        public static class PhoneContactManager
        {
            public class UserContact
            {
                public string PhoneNumber { get; set; }
                public string UserDisplayName { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
            }

            public static IEnumerable<UserContact> GetAllContacts()
            {
                var phoneContactsList = new ObservableCollection<UserContact>();
                using var phones = Application.Context.ContentResolver?.Query(ContactsContract.CommonDataKinds.Phone.ContentUri, null, null, null, null);
                if (phones != null)
                {
                    while (phones.MoveToNext())
                    {
                        try
                        {
                            string name = phones.GetString(phones.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName));
                            string phoneNumber = phones.GetString(phones.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number));

                            string[] words = name?.Split(' ');
                            var contact = new UserContact
                            {
                                FirstName = words?[0],
                                LastName = words?.Length > 1 ? words[1] : "",
                                UserDisplayName = name,
                                PhoneNumber = phoneNumber?.Replace("+", "00").Replace("-", "").Replace(" ", "")
                            };

                            var check = phoneContactsList.FirstOrDefault(a => a.PhoneNumber == contact.PhoneNumber);
                            switch (check)
                            {
                                case null:
                                    phoneContactsList.Add(contact);
                                    break;
                            }
                        }
                        catch (Exception exception)
                        {
                            //something wrong with one contact, may be display name is completely empty, decide what to do
                            DisplayReportResultTrack(exception);
                        }
                    }

                    phones.Close();
                }

                // if we get here, we can't access the contacts. Consider throwing an exception to display to the user

                return phoneContactsList;
            }

            public static UserContact Get_ContactInfoBy_Id(string fromUriId)
            {
                try
                {
                    //var uri = ContactsContract.Contacts.ContentUri;
                    var contacts = Application.Context.ContentResolver.Query(ContactsContract.CommonDataKinds.Phone.ContentUri, null, "_id = ?", new[] { fromUriId }, null);
                    if (contacts != null)
                    {
                        UserContact userContact = new UserContact();
                        contacts.MoveToFirst();
                        string displayName = contacts.GetString(contacts.GetColumnIndex("display_name"));
                        int indexNumber = contacts.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number);


                        string mobileNumber = contacts.GetString(indexNumber);

                        userContact.PhoneNumber = mobileNumber;
                        userContact.UserDisplayName = displayName;

                        //var columnNames = contacts.GetColumnNames();
                        //foreach (var columnName in columnNames)
                        //{
                        //    int index = contacts.GetColumnIndex(columnName);
                        //    var value = contacts.GetString(index);
                        //    Console.WriteLine("Allen >> index = {0}, value = {1}", index, value);
                        //}

                        return string.IsNullOrEmpty(mobileNumber) switch
                        {
                            false => userContact,
                            _ => null!
                        };
                    }
                    else
                    {
                        return null!;
                    }
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                    return null!;
                }
            }

            public static void InsertContact(string fisrtName, string lastName, string number, string email,
                string company)
            {
                List<ContentProviderOperation> ops = new List<ContentProviderOperation>();
                try
                {
                    ContentProviderOperation.Builder builder = ContentProviderOperation.NewInsert(ContactsContract.RawContacts.ContentUri);
                    builder.WithValue(ContactsContract.RawContacts.InterfaceConsts.AccountType, null);
                    builder.WithValue(ContactsContract.RawContacts.InterfaceConsts.AccountName, null);
                    ops.Add(builder.Build());

                    //Name
                    builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
                    builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, 0);
                    builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype, ContactsContract.CommonDataKinds.StructuredName.ContentItemType);
                    builder.WithValue(ContactsContract.CommonDataKinds.StructuredName.FamilyName, lastName);
                    builder.WithValue(ContactsContract.CommonDataKinds.StructuredName.GivenName, fisrtName);
                    ops.Add(builder.Build());

                    //Number
                    builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
                    builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, 0);
                    builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                        ContactsContract.CommonDataKinds.Phone.ContentItemType);
                    builder.WithValue(ContactsContract.CommonDataKinds.Phone.Number, number);
                    builder.WithValue(ContactsContract.CommonDataKinds.Phone.InterfaceConsts.Type,
                        ContactsContract.CommonDataKinds.Phone.InterfaceConsts.TypeCustom);
                    builder.WithValue(ContactsContract.CommonDataKinds.Phone.InterfaceConsts.Label, "Work");
                    ops.Add(builder.Build());

                    //Email
                    builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
                    builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, 0);
                    builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                        ContactsContract.CommonDataKinds.Email.ContentItemType);
                    builder.WithValue(ContactsContract.CommonDataKinds.Email.InterfaceConsts.Data, email);
                    builder.WithValue(ContactsContract.CommonDataKinds.Email.InterfaceConsts.Type,
                        ContactsContract.CommonDataKinds.Email.InterfaceConsts.TypeCustom);
                    builder.WithValue(ContactsContract.CommonDataKinds.Email.InterfaceConsts.Label, "Work");
                    ops.Add(builder.Build());

                    //Company
                    builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
                    builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, 0);
                    builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                        ContactsContract.CommonDataKinds.Organization.ContentItemType);
                    builder.WithValue(ContactsContract.CommonDataKinds.Organization.InterfaceConsts.Data, company);
                    builder.WithValue(ContactsContract.CommonDataKinds.Organization.InterfaceConsts.Type,
                        ContactsContract.CommonDataKinds.Organization.InterfaceConsts.TypeCustom);
                    builder.WithValue(ContactsContract.CommonDataKinds.Organization.InterfaceConsts.Label, "Work");
                    ops.Add(builder.Build());


                    try
                    {
                        //ContentProviderResult[] res = Application.Context.ContentResolver.ApplyBatch(ContactsContract.Authority,ops);

                        Toast.MakeText(Application.Context, "Done contacted added", ToastLength.Short)?.Show();
                    }
                    catch (Exception exception)
                    {
                        Toast.MakeText(Application.Context, "Error ", ToastLength.Long)?.Show();
                        DisplayReportResultTrack(exception);
                    }
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }
        }

        #endregion

        #region String 

        public static class FunString
        {
            //========================= Variables =========================
            private static readonly Random Random = new Random();

            //========================= Functions =========================

            //creat new Random String Session 
            public static string RandomString(int length)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXdsdaawerthklmnbvcxer46gfdsYZ0123456789";
                return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
            }

            //creat new Random Color
            public static string RandomColor()
            {
                string color = "";
                int b;
                b = Random.Next(1, 11);
                color = b switch
                {
                    1 => "#c62828",
                    2 => "#AD1457",
                    3 => "#6A1B9A",
                    4 => "#4527A0",
                    5 => "#283593",
                    6 => "#1565C0",
                    7 => "#00838F",
                    8 => "#2E7D32",
                    9 => "#9E9D24",
                    10 => "#FF8F00",
                    11 => "#D84315",
                    _ => color
                };

                return color;
            }

            public static string GetoLettersfromString(string key)
            {
                try
                {
                    var string1 = key.Split(' ').First();
                    var string2 = key.Split(' ').Last();

                    if (string1 != string2)
                    {
                        string substring1 = string1.Substring(0, 1);
                        string substring2 = string2.Substring(0, 1);
                        var result = substring1 + substring2;
                        return result.ToUpper();
                    }
                    else
                    {
                        string substring1 = string1.Substring(0, 2);

                        var result = substring1;
                        return result.ToUpper();
                    }
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                    return "";
                }
            }

            public static string Format_byte_size(string filepath)
            {
                try
                {
                    /*
                    * var size = new FileInfo(filepath).Length;
                    * double totalSize = size / 1024.0F / 1024.0F;
                    * string sizeFile = totalSize.ToString("0.### KB"); 
                    */

                    string[] sizes = { "B", "KB", "MB", "GB", "TB" };
                    double len = new FileInfo(filepath).Length;
                    int order = 0;
                    while (len >= 1024 && order < sizes.Length - 1)
                    {
                        order++;
                        len /= 1024;
                    }

                    // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
                    // show a single decimal place, and no space.
                    string result = $"{len:0.##} {sizes[order]}";
                    return result;
                }
                catch (Exception e)
                {
                    DisplayReportResultTrack(e);
                    return "0B";
                }
            }

            public static string UppercaseFirst(string s)
            {
                // Check for empty string.
                if (string.IsNullOrEmpty(s))
                {
                    return string.Empty;
                }

                // Return char and concat substring.
                return char.ToUpper(s[0]) + s.Substring(1);
            }

            public static string TrimTo(string str, int maxLength)
            {
                try
                {
                    if (str.Length <= maxLength)
                    {
                        return str;
                    }

                    switch (str.Length)
                    {
                        case > 35:
                            {
                                var remove = str.Remove(0, 10);
                                return remove;
                            }
                    }

                    switch (str.Length)
                    {
                        case > 65:
                            {
                                var remove = str.Remove(0, 30);
                                return remove;
                            }
                    }

                    switch (str.Length)
                    {
                        case > 85:
                            {
                                var remove = str.Remove(0, 50);
                                return remove;
                            }
                    }

                    switch (str.Length)
                    {
                        case > 105:
                            {
                                var remove = str.Remove(0, 70);
                                return remove;
                            }
                        default:
                            return str.Substring(maxLength - 17, maxLength);
                    }
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                    return str.Substring(maxLength - 17, maxLength);
                }
            }

            //SubString Cut Of
            public static string SubStringCutOf(string s, int x)
            {
                try
                {
                    if (!string.IsNullOrEmpty(s) && s.Length > x)
                    {
                        string substring = s.Substring(0, x);
                        return substring + "...";
                    }

                    return s;
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                    return s;
                }
            }

            //Null Remover >> return Empty
            public static string StringNullRemover(string s)
            {
                try
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        s = "Empty";
                    }

                    return s;
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                    return s;
                }
            }

            //De code
            public static string DecodeString(string content)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(content) || string.IsNullOrEmpty(content))
                        return "";

                    //const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";
                    const string stripFormatting = @"<[^>]*(>|$)";
                    const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";
                    var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
                    var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
                    //var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

                    System.Text.StringBuilder builder = new System.Text.StringBuilder(content);
                    builder.Replace(":)", "\ud83d\ude0a")
                       .Replace(";)", "\ud83d\ude09")
                       .Replace("0)", "\ud83d\ude07")
                       .Replace("(<", "\ud83d\ude02")
                       .Replace(":D", "\ud83d\ude01")
                       .Replace("*_*", "\ud83d\ude0d")
                       .Replace("(<", "\ud83d\ude02")
                       .Replace("<3", "\ud83d\u2764")
                       .Replace("/_)", "\ud83d\ude0f")
                       .Replace("-_-", "\ud83d\ude11")
                       .Replace(":-/", "\ud83d\ude15")
                       .Replace(":*", "\ud83d\ude18")
                       .Replace(":_p", "\ud83d\ude1b")
                       .Replace(":p", "\ud83d\ude1c")
                       .Replace("x(", "\ud83d\ude20")
                       .Replace("X(", "\ud83d\ude21")
                       .Replace(":_(", "\ud83d\ude22")
                       .Replace("<5", "\ud83d\u2B50")
                       .Replace(":0", "\ud83d\ude31")
                       .Replace("B)", "\ud83d\ude0e")
                       .Replace("o(", "\ud83d\ude27")
                       .Replace("</3", "\uD83D\uDC94")
                       .Replace(":o", "\ud83d\ude26")
                       .Replace("o(", "\ud83d\ude27")
                       .Replace(":__(", "\ud83d\ude2d")
                       .Replace("!_", "\uD83D\u2757")
                       .Replace("<br> <br>", "\n")
                       .Replace("<br />", "\n")
                       .Replace("[/a]", "/")
                       .Replace("[a]", "")
                       .Replace("%3A", ":")
                       .Replace("%2F", "/")
                       .Replace("%3F", "?")
                       .Replace("%3D", "=")
                       .Replace("<a href=", "")
                       .Replace("target=", "")
                       .Replace("_blank", "")
                       //.Replace(@"""", "")
                       .Replace("</a>", "")
                       .Replace("\'", "'")
                       .Replace("class=hash", "")
                       .Replace("rel=nofollow>", "")
                       .Replace("<p>", "")
                       .Replace("</p>", "")
                       .Replace("</body>", "")
                       .Replace("<body>", "")
                       .Replace("<div>", "")
                       .Replace("<div ", "")
                       .Replace("</div>", "")
                       .Replace("&#039;", "'")
                       .Replace("&amp;", "&")
                       .Replace("&lt;", "<")
                       .Replace("&gt;", ">")
                       .Replace("&le;", "≤")
                       .Replace("&ge;", "≥")
                       .Replace("<iframe", "")
                       .Replace("</iframe>", "")
                       .Replace("<table", "")
                       .Replace("<ul>", "")
                       .Replace("<li>", "")
                       .Replace("&nbsp;", "")
                       .Replace("&amp;nbsp;&lt;/p&gt;&lt;p&gt;", "\r\n")
                       .Replace("&amp;", "&")
                       .Replace("&quot;", "")
                       .Replace("&apos;", "")
                       .Replace("&cent;", "¢")
                       .Replace("&pound;", "£")
                       .Replace("&yen;", "¥")
                       .Replace("&euro;", "€")
                       .Replace("&copy;", "©")
                       .Replace("&reg;", "®")
                       .Replace("<b>", "")
                       .Replace("<u>", "")
                       .Replace("<i>", "")
                       .Replace("</i>", "")
                       .Replace("</u>", "")
                       .Replace("</b>", "")
                       .Replace("<br>", "\n")
                       .Replace("</li>", "")
                       .Replace("</ul>", "")
                       .Replace("</table>", " ")
                       .Replace("a&#768;", "")
                       .Replace("a&#769;", "")
                       .Replace("a&#770;", "")
                       .Replace("a&#771;", "")
                       .Replace("O&#768;", "")
                       .Replace("O&#769;", "")
                       .Replace("O&#770;", "")
                       .Replace("O&#771;", "")
                       .Replace("</table>", "")
                       .Replace("&bull;", "•")
                       .Replace("&hellip;", "…")
                       .Replace("&prime;", "′")
                       .Replace("&Prime;", "″")
                       .Replace("&oline;", "‾")
                       .Replace("&frasl;", "⁄")
                       .Replace("&weierp;", "℘")
                       .Replace("&image;", "ℑ")
                       .Replace("&real;", "ℜ")
                       .Replace("&trade;", "™")
                       .Replace("&alefsym;", "ℵ")
                       .Replace("&larr;", "←")
                       .Replace("&uarr;", "↑")
                       .Replace("&rarr;", "→")
                       .Replace("&darr;", "↓")
                       .Replace("&barr;", "↔")
                       .Replace("&crarr;", "↵")
                       .Replace("&lArr;", "⇐")
                       .Replace("&uArr;", "⇑")
                       .Replace("&rArr;", "⇒")
                       .Replace("&dArr;", "⇓")
                       .Replace("&hArr;", "⇔")
                       .Replace("&forall;", "∀")
                       .Replace("&part;", "∂")
                       .Replace("&exist;", "∃")
                       .Replace("&empty;", "∅")
                       .Replace("&nabla;", "∇")
                       .Replace("&isin;", "∈")
                       .Replace("&notin;", "∉")
                       .Replace("&ni;", "∋")
                       .Replace("&prod;", "∏")
                       .Replace("&sum;", "∑")
                       .Replace("&minus;", "−")
                       .Replace("&lowast", "∗")
                       .Replace("&radic;", "√")
                       .Replace("&prop;", "∝")
                       .Replace("&infin;", "∞")
                       .Replace("&OEig;", "Œ")
                       .Replace("&oelig;", "œ")
                       .Replace("&Yuml;", "Ÿ")
                       .Replace("&spades;", "♠")
                       .Replace("&clubs;", "♣")
                       .Replace("&hearts;", "♥")
                       .Replace("&diams;", "♦")
                       .Replace("&thetasym;", "ϑ")
                       .Replace("&upsih;", "ϒ")
                       .Replace("&piv;", "ϖ")
                       .Replace("&Scaron;", "Š")
                       .Replace("&scaron;", "š")
                       .Replace("&ang;", "∠")
                       .Replace("&and;", "∧")
                       .Replace("&or;", "∨")
                       .Replace("&cap;", "∩")
                       .Replace("&cup;", "∪")
                       .Replace("&int;", "∫")
                       .Replace("&there4;", "∴")
                       .Replace("&sim;", "∼")
                       .Replace("&cong;", "≅")
                       .Replace("&asymp;", "≈")
                       .Replace("&ne;", "≠")
                       .Replace("&equiv;", "≡")
                       .Replace("&le;", "≤")
                       .Replace("&ge;", "≥")
                       .Replace("&sub;", "⊂")
                       .Replace("&sup;", "⊃")
                       .Replace("&nsub;", "⊄")
                       .Replace("&sube;", "⊆")
                       .Replace("&supe;", "⊇")
                       .Replace("&oplus;", "⊕")
                       .Replace("&otimes;", "⊗")
                       .Replace("&perp;", "⊥")
                       .Replace("&sdot;", "⋅")
                       .Replace("&lcell;", "⌈")
                       .Replace("&rcell;", "⌉")
                       .Replace("&lfloor;", "⌊")
                       .Replace("&rfloor;", "⌋")
                       .Replace("&lang;", "⟨")
                       .Replace("&rang;", "⟩")
                       .Replace("&loz;", "◊")
                       .Replace("\u0024", "$")
                       .Replace("\u20AC", "€")
                       .Replace("\u00A3", "£")
                       .Replace("\u00A5", "¥")
                       .Replace("\u00A2", "¢")
                       .Replace("\u20B9", "₹")
                       .Replace("\u20A8", "₨")
                       .Replace("\u20B1", "₱")
                       .Replace("\u20A9", "₩")
                       .Replace("\u0E3F", "฿")
                       .Replace("\u20AB", "₫")
                       .Replace("\u20AA", "₪")
                       .Replace("&#36;", "$")
                       .Replace("&#8364;", "€")
                       .Replace("&#163;", "£")
                       .Replace("&#165;", "¥")
                       .Replace("&#162;", "¢")
                       .Replace("&#8377;", "₹")
                       .Replace("&#8360;", "₨")
                       .Replace("&#8369;", "₱")
                       .Replace("&#8361;", "₩")
                       .Replace("&#3647;", "฿")
                       .Replace("&#8363;", "₫")
                       .Replace("&#8362;", "₪")
                       .Replace("</table>", " ");

                    var text = builder.ToString();

                    //Decode html specific characters
                    text = WebUtility.HtmlDecode(text);

                    //Remove tag whitespace/line breaks
                    //text = tagWhiteSpaceRegex.Replace(text, "><");

                    text = Regex.Replace(text, @"^\s*$\n", string.Empty, RegexOptions.Multiline).TrimEnd();

                    //Replace <br /> with line breaks
                    text = lineBreakRegex.Replace(text, Environment.NewLine);

                    //Strip formatting
                    text = stripFormattingRegex.Replace(text, string.Empty);

                    return text;
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                    return "";
                }
            }

            //String format numbers to millions, thousands with rounding
            public static string FormatPriceValue(long num)
            {
                try
                {
                    return num switch
                    {
                        >= 100000000 => ((num >= 10050000 ? num - 500000 : num) / 1000000D).ToString("#M"),
                        >= 10000000 => ((num >= 10500000 ? num - 50000 : num) / 1000000D).ToString("0.#M"),
                        >= 1000000 => ((num >= 1005000 ? num - 5000 : num) / 1000000D).ToString("0.##M"),
                        >= 100000 => ((num >= 100500 ? num - 500 : num) / 1000D).ToString("0.k"),
                        >= 10000 => ((num >= 10550 ? num - 50 : num) / 1000D).ToString("0.#k"),
                        _ => num >= 1000
                            ? ((num >= 1005 ? num - 5 : num) / 1000D).ToString("0.##k")
                            : num.ToString("#,0")
                    };
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                    return num.ToString();
                }
            }

            public static bool IsEmailValid(string emailAddress)
            {
                try
                {
                    if (string.IsNullOrEmpty(emailAddress) || string.IsNullOrWhiteSpace(emailAddress))
                        return false;

                    MailAddress m = new MailAddress(emailAddress);
                    Console.WriteLine(m);
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            public static bool IsUrlValid(string url)
            {
                try
                {
                    string pattern =
                        @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
                    Regex reg = new Regex(pattern, RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

                    Match m = reg.Match(url);
                    while (m.Success)
                    {
                        //do things with your matching text 
                        m.NextMatch();
                        break;
                    }

                    if (reg.IsMatch(url))
                    {
                        //var ss = "http://" + m.Value;
                        return true;
                    }

                    return false;
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                    return false;
                }
            }

            public static bool IsPhoneNumber(string number)
            {
                return Regex.Match(number, @"^(\+?)([0-9]{9,20}?)$").Success;
            }


            // Functions convert color RGB to HEX
            public static string ConvertColorRgBtoHex(string color)
            {
                //to rgba => string.Format("rgba({0}, {1}, {2}, {3});", color_red, color_green, color_blue, color_alpha);
                try
                {
                    if (color.Contains("rgb"))
                    {
                        var regex = new Regex(@"([0-9]+)");
                        string colorData = color;

                        var matches = regex.Matches(colorData);

                        var colorRed = Convert.ToInt32(matches[0].ToString());
                        var colorGreen = Convert.ToInt32(matches[1].ToString());
                        var colorBlue = Convert.ToInt32(matches[2].ToString());
                        var colorAlpha = Convert.ToInt16(matches[3].ToString());
                        var hex = $"#{colorRed:X2}{colorGreen:X2}{colorBlue:X2}";
                        Console.WriteLine(colorAlpha);
                        return hex;
                    }

                    return AppSettings.MainColor;
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                    return AppSettings.MainColor;
                }
            }

            public static bool OnlyHexInString(string color)
            {
                try
                {
                    if (color.Contains("rgba"))
                    {
                        var regex = new Regex(@"([0-9]+)");
                        string colorData = color;

                        var matches = regex.Matches(colorData);

                        var colorRed = Convert.ToInt32(matches[0].ToString());
                        var colorGreen = Convert.ToInt32(matches[1].ToString());
                        var colorBlue = Convert.ToInt32(matches[2].ToString());
                        var colorAlpha = Convert.ToInt16(matches[3].ToString());
                        var hex = $"#{colorAlpha:X2}{colorRed:X2}{colorGreen:X2}{colorBlue:X2}";
                        Console.WriteLine(hex);
                        return true;
                    }

                    if (color.Contains("rgb"))
                    {
                        var regex = new Regex(@"([0-9]+)");
                        string colorData = color;

                        var matches = regex.Matches(colorData);
                        var colorRed = Convert.ToInt32(matches[0].ToString());
                        var colorGreen = Convert.ToInt32(matches[1].ToString());
                        var colorBlue = Convert.ToInt32(matches[2].ToString());
                        var colorAlpha = Convert.ToInt16(00);
                        var hex = $"#{colorAlpha:X2}{colorRed:X2}{colorGreen:X2}{colorBlue:X2}";
                        Console.WriteLine(hex);
                        return true;
                    }

                    var rxColor = new Regex("^#(?:[0-9a-fA-F]{3}){1,2}$");
                    var rxColor2 = new Regex("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3}|[0-9]{3}|[0-9]{6})$");
                    var rxColor3 = new Regex(@"\A\b[0-9a-fA-F]+\b\Z");
                    var rxColor4 =
                        new Regex(
                            @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"); // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"

                    if (rxColor.IsMatch(color) || rxColor2.IsMatch(color) || rxColor3.IsMatch(color) ||
                        rxColor4.IsMatch(color))
                    {
                        return true;
                    }

                    return false;
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                    return false;
                }
            }

            public static string Check_Regex(string text)
            {
                try
                {
                    var rxEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    var rxWebsite =
                        new Regex(
                            @"^(http|https|ftp|www)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*$");
                    var rxHashtag = new Regex(@"(?<=#)\w+");
                    var rxMention = new Regex("@(?<name>[^\\s]+)");
                    var rxNumber1 = new Regex(@"^\d$");
                    var rxNumber2 = new Regex("[0-9]");
                    var resultEmail = IsEmailValid(text);
                    var resultWeb = IsUrlValid(text);

                    if (rxEmail.IsMatch(text) || resultEmail)
                    {
                        return "Email";
                    }

                    if (rxWebsite.IsMatch(text) || resultWeb)
                    {
                        return "Website";
                    }

                    if (rxHashtag.IsMatch(text))
                    {
                        return "Hashtag";
                    }

                    if (rxMention.IsMatch(text))
                    {
                        //var results = Rx_Mention.Matches(text).Cast<Match>().Select(m => m.Groups["name"].Value).ToArray();

                        return "Mention";
                    }

                    if (rxNumber1.IsMatch(text) || rxNumber2.IsMatch(text))
                    {
                        return "Number";
                    }

                    return text;
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                    return text;
                }
            }
        }

        #endregion

        #region Time

        public static class Time
        {
            public static string LblJustNow = Application.Context.GetText(Resource.String.Lbl_justNow);
            public static string LblHours = Application.Context.GetText(Resource.String.Lbl_hours);
            public static string LblDays = Application.Context.GetText(Resource.String.Lbl_days);
            public static string LblMonth = Application.Context.GetText(Resource.String.Lbl_month);
            public static string LblMinutes = Application.Context.GetText(Resource.String.Lbl_minutes);
            public static string LblSeconds = Application.Context.GetText(Resource.String.Lbl_seconds);
            public static string LblYear = Application.Context.GetText(Resource.String.Lbl_year);
            public static string LblCutHours = Application.Context.GetText(Resource.String.Lbl_CutHours);
            public static string LblCutDays = Application.Context.GetText(Resource.String.Lbl_CutDays);
            public static string LblCutMonth = Application.Context.GetText(Resource.String.Lbl_CutMonth);
            public static string LblCutMinutes = Application.Context.GetText(Resource.String.Lbl_CutMinutes);
            public static string LblCutSeconds = Application.Context.GetText(Resource.String.Lbl_CutSeconds);
            public static string LblCutYear = Application.Context.GetText(Resource.String.Lbl_CutYear);
            public static string LblAboutMinute = Application.Context.GetText(Resource.String.Lbl_about_minute);
            public static string LblAboutHour = Application.Context.GetText(Resource.String.Lbl_about_hour);
            public static string LblYesterday = Application.Context.GetText(Resource.String.Lbl_yesterday);
            public static string LblAboutMonth = Application.Context.GetText(Resource.String.Lbl_about_month);
            public static string LblAboutYear = Application.Context.GetText(Resource.String.Lbl_about_year);

            //Split String Duration (00:00:00)
            public static string SplitStringDuration(string duration)
            {
                try
                {
                    string[] durationsplit = duration.Split(':');
                    switch (durationsplit.Length)
                    {
                        case 3 when durationsplit[0] == "00":
                            {
                                string newDuration = durationsplit[1] + ":" + durationsplit[2];
                                return newDuration;
                            }
                        case 3:
                            return duration;
                        default:
                            return duration;
                    }
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                    return duration;
                }
            }

            //Get TimeZone
            public static string GetTimeZone()
            {
                try
                {
                    const string dataFmt = "{0,-30}{1}";
                    const string timeFmt = "{0,-30}{1:MM-dd-yyyy HH:mm}";
                    var curTimeZone = TimeZoneInfo.GetSystemTimeZones()[0];
                    // What is TimeZone name?
                    Console.WriteLine(dataFmt, "TimeZone Name:", curTimeZone.StandardName);
                    // Is TimeZone DayLight Saving?|
                    Console.WriteLine(dataFmt, "Daylight saving time?", curTimeZone.IsDaylightSavingTime(DateTime.Now));
                    // What is GMT (also called Coordinated Universal Time (UTC)
                    var curUtc = curTimeZone.GetUtcOffset(DateTime.Now);
                    Console.WriteLine(timeFmt, "Coordinated Universal Time:", curUtc);
                    // What is GMT/UTC offset ?
                    TimeSpan currentOffset = curTimeZone.GetUtcOffset(DateTime.Now);
                    Console.WriteLine(dataFmt, "UTC offset:", currentOffset);

                    Calendar cal = Calendar.Instance;
                    var tz = cal.TimeZone;
                    Console.WriteLine("Time zone", "=" + tz.DisplayName);

                    var time = Java.Util.TimeZone.Default.DisplayName;
                    return !string.IsNullOrEmpty(time) ? time : "UTC";
                }
                catch (Exception e)
                {
                    DisplayReportResultTrack(e);
                    return "UTC";
                }
            }

            public static string TimeAgo(long time, bool withReplace)
            {
                try
                {
                    DateTime dateTime = UnixTimeStampToDateTime(time);
                    string result;
                    var timeSpan = DateTime.Now.Subtract(dateTime);

                    if (timeSpan <= TimeSpan.FromSeconds(60))
                    {
                        //result = $"{timeSpan.Seconds} " + Lbl_seconds;
                        result = LblJustNow;
                    }
                    else if (timeSpan <= TimeSpan.FromMinutes(60))
                    {
                        result = timeSpan.Minutes > 1 ? $"{timeSpan.Minutes} " + LblMinutes : LblAboutMinute;
                    }
                    else if (timeSpan <= TimeSpan.FromHours(24))
                    {
                        result = timeSpan.Hours > 1 ? $"{timeSpan.Hours} " + LblHours : LblAboutHour;
                    }
                    else if (timeSpan <= TimeSpan.FromDays(30))
                    {
                        result = timeSpan.Days > 1 ? $"{timeSpan.Days} " + LblDays : LblYesterday;
                    }
                    else if (timeSpan <= TimeSpan.FromDays(365))
                    {
                        result = timeSpan.Days > 30 ? $"{timeSpan.Days / 30} " + LblMonth : LblAboutMonth;
                    }
                    else
                    {
                        result = timeSpan.Days > 365 ? $"{timeSpan.Days / 365}" + LblYear : LblAboutYear;
                    }

                    return withReplace ? ReplaceTime(result) : result;
                }
                catch (Exception e)
                {
                    DisplayReportResultTrack(e);
                    return time.ToString();
                }
            }

            /// <summary>
            /// dataFmt = "{0,-30}{1}"
            /// timeFmt = "{0,-30}{1:MM-dd-yyyy HH:mm}"
            /// </summary>
            /// <param name="time"></param>
            /// <returns></returns>
            public static string TimeAgo(long time)
            {
                try
                {
                    DateTime dateTime = UnixTimeStampToDateTime(time);
                    string result;
                    var timeSpan = DateTime.Now.Subtract(dateTime);

                    if (timeSpan <= TimeSpan.FromSeconds(60))
                    {
                        result = dateTime.ToShortTimeString();
                    }
                    else if (timeSpan <= TimeSpan.FromMinutes(60))
                    {
                        result = dateTime.ToShortTimeString();
                    }
                    else if (timeSpan <= TimeSpan.FromHours(24))
                    {
                        result = dateTime.ToShortTimeString();
                    }
                    else if (timeSpan <= TimeSpan.FromDays(30))
                    {
                        result = dateTime.ToShortDateString();
                    }
                    else if (timeSpan <= TimeSpan.FromDays(365))
                    {
                        result = dateTime.ToShortDateString();
                    }
                    else
                    {
                        result = dateTime.ToShortDateString();
                    }

                    return result;
                }
                catch (Exception e)
                {
                    DisplayReportResultTrack(e);
                    return time.ToString();
                }
            }

            public static string TimeAgo(DateTime dateTime, bool withReplace)
            {
                try
                {
                    string result;
                    var timeSpan = DateTime.Now.Subtract(dateTime);

                    if (timeSpan <= TimeSpan.FromSeconds(60))
                    {
                        //result = $"{timeSpan.Seconds} " + Lbl_seconds;
                        result = LblJustNow;
                    }
                    else if (timeSpan <= TimeSpan.FromMinutes(60))
                    {
                        result = timeSpan.Minutes > 1 ? $"{timeSpan.Minutes} " + LblMinutes : LblAboutMinute;
                    }
                    else if (timeSpan <= TimeSpan.FromHours(24))
                    {
                        result = timeSpan.Hours > 1 ? $"{timeSpan.Hours} " + LblHours : LblAboutHour;
                    }
                    else if (timeSpan <= TimeSpan.FromDays(30))
                    {
                        result = timeSpan.Days > 1 ? $"{timeSpan.Days} " + LblDays : LblYesterday;
                    }
                    else if (timeSpan <= TimeSpan.FromDays(365))
                    {
                        result = timeSpan.Days > 30 ? $"{timeSpan.Days / 30} " + LblMonth : LblAboutMonth;
                    }
                    else
                    {
                        result = timeSpan.Days > 365 ? $"{timeSpan.Days / 365} " + LblYear : LblAboutYear;
                    }

                    return withReplace ? ReplaceTime(result) : result;
                }
                catch (Exception e)
                {
                    DisplayReportResultTrack(e);
                    return dateTime.ToShortTimeString();
                }
            }

            //Functions Replace Time
            public static string ReplaceTime(string time)
            {
                time = time.ToLower();
                if (time.Contains("hours ago") || time.Contains("hour ago"))
                {
                    time = time.Replace("hours ago", LblCutHours);
                    time = time.Replace("hour ago", LblCutHours);
                }
                else if (time.Contains("days ago") || time.Contains("day ago"))
                {
                    time = time.Replace("days ago", LblCutDays).Replace("day ago", LblCutDays);
                }
                else if (time.Contains("month ago") || time.Contains("months ago"))
                {
                    time = time.Replace("months ago", LblCutMonth);
                    time = time.Replace("month ago", LblCutMonth);
                }
                else if (time.Contains("minutes ago"))
                {
                    time = time.Replace("minutes ago", LblCutMinutes);
                }
                else if (time.Contains("seconds ago") || time.Contains("second ago"))
                {
                    time = time.Replace("seconds ago", LblCutSeconds);
                    time = time.Replace("second ago", LblCutSeconds);
                }
                else if (time.Contains("year ago") || time.Contains("years ago"))
                {
                    time = time.Replace("year ago", LblCutYear);
                    time = time.Replace("years ago", LblCutYear);
                }
                else if (time.Contains("yesterday"))
                {
                    time = time.Replace("yesterday", LblYesterday);
                }

                return time;
            }

            //convert a Unix timestamp to DateTime 
            public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
            {
                // Unix timestamp is seconds past epoch
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
                return dtDateTime;
            }


            private static readonly DateTime Jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            public static long CurrentTimeMillis()
            {
                return (long)(DateTime.UtcNow - Jan1St1970).TotalMilliseconds;
            }


            #region To days

            public static double ConvertMillisecondsToDays(double milliseconds)
            {
                return TimeSpan.FromMilliseconds(milliseconds).TotalDays;
            }

            public static double ConvertSecondsToDays(double seconds)
            {
                return TimeSpan.FromSeconds(seconds).TotalDays;
            }

            public static double ConvertMinutesToDays(double minutes)
            {
                return TimeSpan.FromMinutes(minutes).TotalDays;
            }

            public static double ConvertHoursToDays(double hours)
            {
                return TimeSpan.FromHours(hours).TotalDays;
            }

            #endregion

            #region To hours

            public static double ConvertMillisecondsToHours(double milliseconds)
            {
                return TimeSpan.FromMilliseconds(milliseconds).TotalHours;
            }

            public static double ConvertSecondsToHours(double seconds)
            {
                return TimeSpan.FromSeconds(seconds).TotalHours;
            }

            public static double ConvertMinutesToHours(double minutes)
            {
                return TimeSpan.FromMinutes(minutes).TotalHours;
            }

            public static double ConvertDaysToHours(double days)
            {
                return TimeSpan.FromHours(days).TotalHours;
            }

            #endregion

            #region To minutes

            public static double ConvertMillisecondsToMinutes(double milliseconds)
            {
                return TimeSpan.FromMilliseconds(milliseconds).Minutes;
            }

            public static double ConvertSecondsToMinutes(double seconds)
            {
                return TimeSpan.FromSeconds(seconds).TotalMinutes;
            }

            public static double ConvertHoursToMinutes(double hours)
            {
                return TimeSpan.FromHours(hours).TotalMinutes;
            }

            public static double ConvertDaysToMinutes(double days)
            {
                return TimeSpan.FromDays(days).TotalMinutes;
            }

            #endregion

            #region To seconds

            public static double ConvertMillisecondsToSeconds(double milliseconds)
            {
                return TimeSpan.FromMilliseconds(milliseconds).Seconds;
            }

            public static double ConvertMinutesToSeconds(double minutes)
            {
                return TimeSpan.FromMinutes(minutes).TotalSeconds;
            }

            public static double ConvertHoursToSeconds(double hours)
            {
                return TimeSpan.FromHours(hours).TotalSeconds;
            }

            public static double ConvertDaysToSeconds(double days)
            {
                return TimeSpan.FromDays(days).TotalSeconds;
            }

            #endregion

            #region To milliseconds

            public static double ConvertSecondsToMilliseconds(double seconds)
            {
                return TimeSpan.FromSeconds(seconds).TotalMilliseconds;
            }

            public static double ConvertMinutesToMilliseconds(double minutes)
            {
                return TimeSpan.FromMinutes(minutes).TotalMilliseconds;
            }

            public static double ConvertHoursToMilliseconds(double hours)
            {
                return TimeSpan.FromHours(hours).TotalMilliseconds;
            }

            public static double ConvertDaysToMilliseconds(double days)
            {
                return TimeSpan.FromDays(days).TotalMilliseconds;
            }

            #endregion
        }

        #endregion

        #region Dialog Popup

        public class DialogPopup
        {
            public enum MessageResult
            {
                None = 0,
                Ok = 1,
                Cancel = 2,
                Abort = 3,
                Retry = 4,
                Ignore = 5,
                Yes = 6,
                No = 7
            }

            private readonly Activity MContext;

            public DialogPopup(Activity activity)
            {
                MContext = activity;
            }

            public Task<MessageResult> ShowDialog(string title, string message, bool setCancelable = false, MessageResult positiveButton = MessageResult.Ok, MessageResult negativeButton = MessageResult.None, MessageResult neutralButton = MessageResult.None,
                int iconAttribute = Android.Resource.Attribute.AlertDialogIcon)
            {
                var tcs = new TaskCompletionSource<MessageResult>();

                var builder = new AlertDialog.Builder(MContext, Resource.Style.AlertDialogCustom);
                builder.SetIconAttribute(iconAttribute);
                builder.SetTitle(title);
                builder.SetMessage(message);
                //builder.SetInverseBackgroundForced(setInverseBackgroundForced);
                builder.SetCancelable(setCancelable);

                builder.SetPositiveButton(
                    positiveButton != MessageResult.None ? positiveButton.ToString() : string.Empty,
                    (senderAlert, args) => { tcs.SetResult(positiveButton); });
                builder.SetNegativeButton(
                    negativeButton != MessageResult.None ? negativeButton.ToString() : string.Empty,
                    delegate { tcs.SetResult(negativeButton); });
                builder.SetNeutralButton(
                    neutralButton != MessageResult.None ? neutralButton.ToString() : string.Empty,
                    delegate { tcs.SetResult(neutralButton); });

                builder.Show();

                return tcs.Task;
            }

            public static void InvokeAndShowDialog(Activity activity, string title, string message, string positiveButtonstring)
            {
                try
                {
                    if (activity?.IsDestroyed != false)
                        return;

                    activity?.RunOnUiThread(() =>
                    {
                        try
                        {
                            var dialog = new MaterialDialog.Builder(activity).Theme(AppSettings.SetTabDarkTheme ? Theme.Dark : Theme.Light);
                            dialog.Title(title).TitleColorRes(Resource.Color.primary);
                            dialog.Content(message);
                            dialog.PositiveText(positiveButtonstring).OnPositive(new WoWonderTools.MyMaterialDialog());
                            dialog.AlwaysCallSingleChoiceCallback();
                            dialog.Build().Show();
                        }
                        catch (Exception e)
                        {
                            DisplayReportResultTrack(e);
                        }
                    });
                }
                catch (Exception e)
                {
                    DisplayReportResultTrack(e);
                }
            }
        }

        #endregion

        #region IApp

        public static class App
        {
            public static void FullScreenApp(Activity context, bool setFull = false)
            {
                try
                {
                    if (AppSettings.EnableFullScreenApp || setFull)
                    {
                        switch (Build.VERSION.SdkInt)
                        {
                            case >= BuildVersionCodes.R:
                                context.Window?.SetDecorFitsSystemWindows(false);

                                context.Window?.AddFlags(WindowManagerFlags.Fullscreen);
                                //context.Window?.RequestFeature(WindowFeatures.NoTitle);
                                break;
                            case >= BuildVersionCodes.Lollipop:
                                {
                                    View mContentView = context.Window?.DecorView;

                                    if (mContentView != null)
                                    {
#pragma warning disable 618
                                        var uiOptions = (int)mContentView.SystemUiVisibility;
#pragma warning restore 618
                                        var newUiOptions = uiOptions;

                                        newUiOptions |= (int)SystemUiFlags.Fullscreen;
                                        newUiOptions |= (int)SystemUiFlags.LayoutStable;
                                        newUiOptions |= (int)SystemUiFlags.HideNavigation;
#pragma warning disable 618
                                        mContentView.SystemUiVisibility = (StatusBarVisibility)newUiOptions;
#pragma warning restore 618
                                    }

                                    context.Window?.AddFlags(WindowManagerFlags.Fullscreen);

                                    context.Window?.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                                    context.Window?.SetStatusBarColor(Color.Transparent);
                                    break;
                                }
                            case >= BuildVersionCodes.Kitkat:
                                context.Window?.AddFlags(WindowManagerFlags.TranslucentStatus);
                                break;
                        }
                    }
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }

            public static Point OverrideGetSize(Context context)
            {
                var display = context?.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
                try
                {
                    switch (Build.VERSION.SdkInt)
                    {
                        case >= BuildVersionCodes.R:
                            {
                                WindowMetrics metrics = display?.CurrentWindowMetrics;

                                // Gets all excluding insets
                                WindowInsets windowInsets = metrics?.WindowInsets;

                                Insets insets = windowInsets?.GetInsetsIgnoringVisibility(WindowInsets.Type.NavigationBars() | WindowInsets.Type.DisplayCutout());

                                int insetsWidth = insets.Right + insets.Left;
                                int insetsHeight = insets.Top + insets.Bottom;

                                // Legacy size that Display#getSize reports
                                Rect bounds = metrics.Bounds;
                                Size legacySize = new Size(bounds.Width() - insetsWidth, bounds.Height() - insetsHeight);

                                int width = legacySize.Width;
                                int height = legacySize.Height;

                                Point size = new Point(width, height);
                                return size;
                            }
                        default:
                            {
                                var point = new Point();
#pragma warning disable 618
                                display?.DefaultDisplay?.GetSize(point);
#pragma warning restore 618
                                return point;
                            }
                    }
                }
                catch (NoSuchMethodException ex)
                {
                    DisplayReportResultTrack(ex);
                    return null;
                }
            }

            public static void OpenAppByPackageName(Context context, string packageName, string type, ChatObject userChat = null)
            {
                try
                {
                    Intent intent;
                    Intent chkintent = context?.PackageManager?.GetLaunchIntentForPackage(packageName);
                    if (chkintent != null)
                    {
                        Intent launchIntent = context.PackageManager?.GetLaunchIntentForPackage(packageName);
                        if (launchIntent != null)
                        {
                            launchIntent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);

                            launchIntent.PutExtra("App", "Timeline");
                            launchIntent.PutExtra("type", type); // SendMsgProduct , OpenChat , OpenChatPage

                            if (userChat != null)
                            {
                                launchIntent?.PutExtra("UserID", userChat.UserId);

                                userChat.LastMessage = userChat.LastMessage.LastMessageClass switch
                                {
                                    null => new LastMessageUnion { LastMessageClass = new MessageData() },
                                    _ => userChat.LastMessage
                                };

                                launchIntent.PutExtra("itemObject", JsonConvert.SerializeObject(userChat));
                            }
                            launchIntent.AddFlags(ActivityFlags.SingleTop);
                            context.StartActivity(launchIntent);
                        }
                        else
                        {
                            intent = context.PackageManager?.GetLaunchIntentForPackage(packageName);
                            intent?.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                            intent?.AddFlags(ActivityFlags.SingleTop);
                            context.StartActivity(intent);
                        }
                    }
                    else
                    {
                        intent = new Intent(Intent.ActionView, Uri.Parse("market://details?id=" + packageName));
                        intent?.AddFlags(ActivityFlags.NewTask);
                        context?.StartActivity(intent);
                    }
                }
                catch (ActivityNotFoundException es)
                {
                    DisplayReportResultTrack(es);
                    var intent = new Intent(Intent.ActionView, Uri.Parse("http://play.google.com/store/apps/details?id=" + packageName));
                    intent.AddFlags(ActivityFlags.NewTask);
                    context?.StartActivity(intent);
                }
            }

            public static void ClearWebViewCache(Activity context)
            {
                try
                {
                    WebView wv = new WebView(context);
                    // wv.ClearCache(true);

                    switch (AppSettings.RenderPriorityFastPostLoad)
                    {
                        case true:
                            wv.Settings.SetRenderPriority(WebSettings.RenderPriority.High);
                            wv.Settings.SetAppCacheEnabled(true);
                            wv.Settings.EnableSmoothTransition();
                            wv.Settings.SetLayoutAlgorithm(WebSettings.LayoutAlgorithm.TextAutosizing);

                            wv.SetLayerType(Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat ? LayerType.Hardware : LayerType.Software, null);
                            break;
                    }
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }

            public static void SendSms(Context context, string phoneNumber, string textmessges)
            {
                try
                {
                    var smsUri = Uri.Parse("smsto:" + phoneNumber);
                    var smsIntent = new Intent(Intent.ActionSendto, smsUri);
                    smsIntent?.PutExtra("sms_body", textmessges);
                    smsIntent?.AddFlags(ActivityFlags.NewTask);
                    context.StartActivity(smsIntent);

                    //Or use this code
                    // Android.Telephony.SmsManager.Default.SendTextMessage(item.PhoneNumber, null, "Hello Xamarin This is My Test SMS", null, null);
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }

            public static void SaveContacts(Context context, string phonenumber, string name, string type)
            {
                try
                {
                    switch (type)
                    {
                        case "1":
                            {
                                Intent intent = new Intent(ContactsContract.Intents.Insert.Action);
                                intent.SetType(ContactsContract.RawContacts.ContentType);
                                intent.PutExtra(ContactsContract.Intents.Insert.Phone, phonenumber);
                                intent.PutExtra(ContactsContract.Intents.Insert.Name, name);
                                intent.PutExtra(ContactsContract.Intents.Insert.Email, "wael@test.com");
                                context.StartActivity(intent);
                                break;
                            }
                        default:
                            {
                                var contactUri = Uri.Parse("tel:" + phonenumber);
                                Intent intent = new Intent(ContactsContract.Intents.ShowOrCreateContact, contactUri);
                                intent.PutExtra(ContactsContract.Intents.ExtraRecipientContactName, true);
                                context.StartActivity(intent);
                                break;
                            }
                    }
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }

            public static void SendEmail(Context context, string email, string subject = "", string text = "")
            {
                try
                {
                    string mailto = "mailto:" + email + "?cc=" + email + "&subject=" + subject + "&body=" + text;
                    Intent emailIntent = new Intent(Intent.ActionSendto);
                    emailIntent.SetData(Uri.Parse(mailto));
                    context.StartActivity(Intent.CreateChooser(emailIntent, "Send Email"));
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }

            public static void SendPhoneCall(Context context, string phoneNumber)
            {
                try
                {
                    var urlNumber = Uri.Parse("tel:" + phoneNumber);
                    var callIntent = new Intent(Intent.ActionCall);
                    callIntent?.SetData(urlNumber);
                    callIntent?.AddFlags(ActivityFlags.NewTask);
                    context.StartActivity(callIntent);
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }

            public static void Restart_App(Context context, string packageName)
            {
                try
                {
                    Intent intent = Application.Context.PackageManager?.GetLaunchIntentForPackage(packageName);
                    // If not NULL run the app, if not, take the user to the app store
                    if (intent != null)
                    {
                        intent.AddFlags(ActivityFlags.NewTask);
                        context.StartActivity(intent);
                    }
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }

            public static void Close_App()
            {
                try
                {
                    Process.KillProcess(Process.MyPid());
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                    Process.KillProcess(Process.MyPid());
                }
            }


            public static string GetKeyHashesConfigured(Context applicationContext)
            {
                try
                {
                    PackageInfo info = applicationContext.PackageManager?.GetPackageInfo(applicationContext.PackageName!, PackageInfoFlags.Signatures);
#pragma warning disable 618
                    foreach (var signature in info?.Signatures!)
#pragma warning restore 618
                    {
                        MessageDigest md = MessageDigest.GetInstance("SHA");
                        md.Update(signature.ToByteArray()!);
                        string returnValue = Convert.ToBase64String(md.Digest());

                        Console.WriteLine("KeyHash: " + returnValue);
                        return returnValue;
                    }
                }
                catch (PackageManager.NameNotFoundException e)
                {
                    DisplayReportResultTrack(e);
                    return "";
                }
                catch (NoSuchAlgorithmException e)
                {
                    DisplayReportResultTrack(e);
                    return "";
                }
                catch (Exception e)
                {
                    DisplayReportResultTrack(e);
                    return "";
                }

                return "";
            }

            public static string GetValueFromManifest(Context applicationContext, string nameValue)
            {
                try
                {
                    ApplicationInfo ai = applicationContext.PackageManager?.GetApplicationInfo(applicationContext.PackageName!, PackageInfoFlags.MetaData);
                    Bundle bundle = ai?.MetaData;
                    string myApiKey = bundle?.GetString(nameValue);
                    return myApiKey;
                }
                catch (PackageManager.NameNotFoundException e)
                {
                    //string error = "Failed to load meta-data, NameNotFound: " + e.Message;
                    //Console.WriteLine(error);
                    DisplayReportResultTrack(e.InnerException);
                }
                catch (NullPointerException e)
                {
                    DisplayReportResultTrack(e.InnerException);
                    //Console.WriteLine("Failed to load meta-data, NullPointer: " + e.Message);
                }

                return "";
            }
        }

        #endregion

        #region LocalNotification

        public static class LocalNotification
        {
            private static readonly string NotificationId = "NOTIFICATION_ID";
            private static readonly string ChannelId = "Channel_2018";
            private static WebClient WebClient = new WebClient();

            public static void CreateLocalNotification(string notificationId, string title, string contentText)
            {
                try
                {
                    switch (AppSettings.ShowNotification)
                    {
                        case true:
                            {
                                // Instantiate the builder and set notification elements:
                                NotificationCompat.Builder builder = new NotificationCompat.Builder(Application.Context, notificationId)
                                    .SetContentTitle(title) //Sample Notification
                                    .SetContentText(contentText) //Hello World! This is my first notification!
                                    .SetStyle(new NotificationCompat.BigTextStyle().BigText(contentText))
                                    .SetSmallIcon(Resource.Mipmap.icon);

                                // builder.SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate);

                                // Build the notification:
                                Notification notification = builder.Build();

                                // Get the notification manager:
                                NotificationManager notificationManager =
                                    Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;

                                // Publish the notification:
                                var id = Convert.ToInt32(notificationId);

                                notificationManager?.Notify(id, notification);
                                break;
                            }
                    }
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }

            public static void Create_Progress_Notification(string notificationId, string notificationTitle)
            {
                try
                {
                    switch (AppSettings.ShowNotification)
                    {
                        case true:
                            {
                                NotificationManagerCompat notificationManager = NotificationManagerCompat.From(Application.Context);
                                var id = Convert.ToInt32(notificationId);

                                switch (Build.VERSION.SdkInt)
                                {
                                    case >= BuildVersionCodes.O:
                                        {
                                            // Create the NotificationChannel, but only on API 26+ because
                                            // the NotificationChannel class is new and not in the support library
                                            var channel = new NotificationChannel(ChannelId, "Video_Notifciation_Channel_1",
                                                    NotificationImportance.High)
                                            { Description = "" };
                                            channel.EnableVibration(true);
                                            channel.LockscreenVisibility = NotificationVisibility.Public;
                                            if (notificationManager != null)
                                            {
                                                //notificationManager.CreateNotificationChannel(channel);
                                            }

                                            break;
                                        }
                                }

                                var notificationBroadcasterAction = new Intent(Application.Context, typeof(NotificationBroadcasterCloser));
                                notificationBroadcasterAction.PutExtra(NotificationId, notificationId);
                                notificationBroadcasterAction.PutExtra("type", "dismiss");
                                //PendingIntent cancelIntent = PendingIntent?.GetBroadcast(Application.Context, id, notificationBroadcasterAction, 0);

                                NotificationCompat.Builder builder = new NotificationCompat.Builder(Application.Context, notificationId).SetContentTitle(notificationTitle).SetOngoing(true).SetProgress(100, 0, false).SetSmallIcon(Resource.Mipmap.icon);
                                builder.SetPriority(NotificationCompat.PriorityMax);
                                //.AddAction(Resource.Drawable.icon, "Dismiss", cancelIntent)

                                Notification notification = builder.Build();

                                try
                                {
                                    string url = "http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4";
                                    string filename = url.Split('/').Last();
                                    string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "mmSavedVideos");
                                    string mediaFile = filePath + "/" + filename;

                                    if (!System.IO.File.Exists(mediaFile))
                                    {
                                        WebClient = new WebClient();

                                        if (!Directory.Exists(filePath))
                                            Directory.CreateDirectory(filePath);

                                        WebClient.DownloadFileAsync(new System.Uri(url), mediaFile);

                                        WebClient.DownloadProgressChanged += (sender, ep) =>
                                        {
                                            double bytesIn = double.Parse(ep.BytesReceived.ToString());
                                            double totalBytes = double.Parse(ep.TotalBytesToReceive.ToString());
                                            double percentage = bytesIn / totalBytes * 100;
                                            var presint = Convert.ToInt32(percentage);

                                            new Thread(() =>
                                            {
                                                builder.SetProgress(100, presint, false);
                                                notificationManager?.Notify(Convert.ToInt32(id), builder.Build());
                                            }).Start();
                                        };
                                        WebClient.DownloadDataCompleted += (s, e) =>
                                        {
                                            try
                                            {
                                                builder.SetContentText("Download complete")
                                                    .SetProgress(0, 0, false);
                                                if (notificationManager != null)
                                                {
                                                    notificationManager.Notify(id, builder.Build());
                                                    notificationManager.Cancel(id);
                                                }

                                                System.IO.File.WriteAllBytes(mediaFile, e.Result);
                                            }
                                            catch (Exception exception)
                                            {
                                                DisplayReportResultTrack(exception);
                                            }
                                        };
                                    }
                                }
                                catch (Exception exception)
                                {
                                    DisplayReportResultTrack(exception);
                                }

                                notificationManager?.Notify(id, notification);
                                break;
                            }
                    }
                }
                catch (Exception exception)
                {
                    DisplayReportResultTrack(exception);
                }
            }

            [BroadcastReceiver]
            [IntentFilter(new[] { "select.notif.id" })]
            public class NotificationBroadcasterCloser : BroadcastReceiver
            {
                public override void OnReceive(Context context, Intent intent)
                {
                    try
                    {
                        int notificationId = intent.GetIntExtra("NOTIFICATION_ID", 0);

                        NotificationManager notifyMgr = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);
                        notifyMgr?.Cancel(notificationId);

                        if (intent?.GetStringExtra("action") == "dismiss")
                        {
                            WebClient.CancelAsync();
                            notifyMgr?.CancelAll();
                        }
                    }
                    catch (Exception exception)
                    {
                        DisplayReportResultTrack(exception);
                    }
                }
            }

            [Service(Exported = false)]
            public class BackgroundRunner : JobIntentService
            {
                protected override void OnHandleWork(Intent p0)
                {

                }
            }
        }

        #endregion

        #region AttachmentFiles

        public static class AttachmentFiles
        {
            /// <summary>
            /// Main feature. Return actual path for file from uri. 
            /// </summary>
            /// <param name="uri">File's uri</param>
            /// <param name="context">Current context</param>
            /// <returns>Actual path</returns>
            public static string GetActualPathFromFile(Context context, Uri uri)
            {
                try
                {
                    bool isKitKat = Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat;
                    string filePath = "";
                    switch (isKitKat)
                    {
                        // DocumentProvider
                        // MediaStore (and general)
                        case true when DocumentsContract.IsDocumentUri(context, uri):
                            {
                                // ExternalStorageProvider
                                if (IsExternalStorageDocument(uri))
                                {
                                    string docId = DocumentsContract.GetDocumentId(uri);
                                    string[] split = docId?.Split(":");
                                    string type = split[0];

                                    if ("primary".Equals(type, StringComparison.OrdinalIgnoreCase))
                                    {
                                        return Path.AndroidDcimFolder + "/" + split[1];
                                    }
                                    else
                                    {
                                        switch ((int)Build.VERSION.SdkInt)
                                        {
                                            case > 20:
                                                {
                                                    //getExternalMediaDirs() added in API 21
                                                    var extenal = context.GetExternalMediaDirs();
                                                    switch (extenal?.Length)
                                                    {
                                                        case > 1:
                                                            filePath = extenal[1].AbsolutePath;
                                                            filePath = filePath.Substring(0, filePath.IndexOf("Android")) + split[1];
                                                            break;
                                                    }

                                                    break;
                                                }
                                            default:
                                                filePath = "/storage/" + type + "/" + split[1];
                                                break;
                                        }

                                        return filePath;
                                    }
                                }

                                // DownloadsProvider 
                                if (IsDownloadsDocument(uri))
                                {
                                    string id = DocumentsContract.GetDocumentId(uri);
                                    string column = "_data";
                                    string[] projection = { column };

                                    using ICursor cursor = context.ContentResolver?.Query(uri, projection, null, null, null);
                                    if (cursor != null && cursor.MoveToFirst())
                                    {
                                        int index = cursor.GetColumnIndexOrThrow(column);
                                        filePath = cursor.GetString(index);
                                        cursor.Close();
                                        return filePath;
                                    }

                                    Uri contentUri = ContentUris.WithAppendedId(Uri.Parse("content://downloads/public_downloads"), long.Parse(id));
                                    return GetDataColumn(context, contentUri, null, null);
                                }

                                // MediaProvider 
                                if (IsMediaDocument(uri))
                                {
                                    string docId = DocumentsContract.GetDocumentId(uri);
                                    string[] split = docId.Split(":");
                                    string type = split[0];

                                    Uri contentUri = type switch
                                    {
                                        "image" => MediaStore.Images.Media.ExternalContentUri,
                                        "video" => MediaStore.Video.Media.ExternalContentUri,
                                        "audio" => MediaStore.Audio.Media.ExternalContentUri,
                                        _ => null
                                    };

                                    string selection = "_id=?";
                                    string[] selectionArgs = new string[]
                                    {
                                    split[1]
                                    };

                                    return GetDataColumn(context, contentUri, selection, selectionArgs);
                                }

                                if ("content".Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase))
                                {
                                    // Return the remote address
                                    if (IsGooglePhotosUri(uri))
                                        return uri.LastPathSegment;
                                    try
                                    {

                                        return GetDataColumn(context, uri, null, null);
                                    }
                                    catch (Exception e)
                                    {
                                        DisplayReportResultTrack(e);
                                        return null;
                                    }
                                }
                                // Other Providers
                                else
                                {
                                    try
                                    {
                                        var attachment = context.ContentResolver?.OpenInputStream(uri);
                                        if (attachment != null)
                                        {
                                            string filename = GetContentName(context.ContentResolver, uri);
                                            if (filename != null)
                                            {
                                                File file = new File(context.CacheDir, filename);
                                                FileOutputStream tmp = new FileOutputStream(file);
                                                byte[] buffer = new byte[1024];
                                                while (attachment.Read(buffer) > 0)
                                                {
                                                    tmp.Write(buffer);
                                                }
                                                tmp.Close();
                                                attachment.Close();
                                                return file.AbsolutePath;
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        DisplayReportResultTrack(e);
                                        return null;
                                    }
                                }

                                break;
                            }
                        default:
                            {
                                if ("content".Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase))
                                {
                                    // Return the remote address
                                    if (IsGooglePhotosUri(uri))
                                        return uri.LastPathSegment;

                                    var path = CopyDocumentToCache(context, uri);
                                    if (path != null)
                                    {
                                        return path;
                                    }
                                }

                                // File 
                                if ("file".Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase))
                                {
                                    return uri.Path;
                                }

                                break;
                            }
                    }
                }
                catch (Exception e)
                {
                    DisplayReportResultTrack(e);
                    return null!;
                }
                return null!;
            }

            /// <summary>
            /// Return data for specified uri
            /// </summary>
            /// <param name="context">Current context</param>
            /// <param name="uri">Current uri</param>
            /// <param name="selection">Args names</param>
            /// <param name="selectionArgs">Args values</param>
            /// <returns>Data</returns>
            private static string GetDataColumn(Context context, Uri uri, string selection, string[] selectionArgs)
            {
                ICursor cursor = null!;
                string column = "_data";
                string[] projection = { column };

                try
                {
                    cursor = context.ContentResolver?.Query(uri, projection, selection, selectionArgs, null);
                    if (cursor != null && cursor.MoveToFirst())
                    {
                        int index = cursor.GetColumnIndexOrThrow(column);
                        return cursor.GetString(index);
                    }
                }
                catch (Exception ex)
                {
                    DisplayReportResultTrack(ex);
                    return CopyDocumentToCache(context, uri);
                }
                finally
                {
                    cursor?.Close();
                }
                return null!;
            }

            //public static bool IsGoogleDrive(Uri uri)
            //{
            //    return "com.google.android.apps.docs.storage".Equals(uri.Authority);
            //}

            //Whether the Uri authority is ExternalStorageProvider.
            public static bool IsExternalStorageDocument(Uri uri)
            {
                return "com.android.externalstorage.documents".Equals(uri.Authority);
            }

            //Whether the Uri authority is DownloadsProvider.
            public static bool IsDownloadsDocument(Uri uri)
            {
                return "com.android.providers.downloads.documents".Equals(uri.Authority);
            }

            //Whether the Uri authority is MediaProvider.
            public static bool IsMediaDocument(Uri uri)
            {
                return "com.android.providers.media.documents".Equals(uri.Authority);
            }

            public static bool IsGooglePhotosUri(Uri uri)
            {
                return "com.google.android.apps.photos.content".Equals(uri.Authority);
            }

            //Functions Check File Extension */Audio, Image, Video\*
            public static string Check_FileExtension(string filename)
            {
                if (string.IsNullOrEmpty(filename))
                    return "Forbidden";

                var mime = MimeTypeMap.GetMimeType(filename.Split('.').LastOrDefault());
                if (string.IsNullOrEmpty(mime)) return "Forbidden";
                if (mime.Contains("audio"))
                {
                    return "Audio";
                }

                if (mime.Contains("video"))
                {
                    return "Video";
                }

                if (mime.Contains("image") || mime.Contains("drawing"))
                {
                    return "Image";
                }

                if (mime.Contains("application") || mime.Contains("text") || mime.Contains("x-world") ||
                    mime.Contains("message"))
                {
                    return "File";
                }

                return "Forbidden";
            }

            private static string CopyDocumentToCache(Context context, Uri uri)
            {
                ParcelFileDescriptor parcelFd = null;
                FileInputStream input = null;
                FileOutputStream output = null;
                ContentResolver contentResolver = context.ContentResolver;
                try
                {
                    string timeStamp = Time.CurrentTimeMillis().ToString();
                    parcelFd = contentResolver?.OpenFileDescriptor(uri, "r");
                    input = new FileInputStream(parcelFd?.FileDescriptor);

                    string extension = MimeTypeMap.GetExtension(contentResolver?.GetType(uri));
                    File f = new File(Path.FolderDiskMyApp, timeStamp + "_" + extension);
                    output = new FileOutputStream(f);
                    input.Channel?.TransferTo(0, input.Channel.Size(), output.Channel);
                    return f.AbsolutePath;
                }
                catch (Exception e)
                {
                    DisplayReportResultTrack(e);
                }
                finally
                {
                    try
                    {
                        parcelFd?.Close();
                        input?.Close();
                        output?.Close();
                    }
                    catch (Exception exception)
                    {
                        DisplayReportResultTrack(exception);
                    }
                }
                return null;
            }

            private static string GetContentName(ContentResolver resolver, Uri uri)
            {
                try
                {
                    ICursor cursor = resolver.Query(uri, null, null, null, null);
                    cursor?.MoveToFirst();
                    int nameIndex = cursor.GetColumnIndex(MediaStore.IMediaColumns.DisplayName);
                    switch (nameIndex)
                    {
                        case >= 0:
                            {
                                string name = cursor.GetString(nameIndex);
                                cursor.Close();
                                return name;
                            }
                        default:
                            return null;
                    }
                }
                catch (Exception e)
                {
                    DisplayReportResultTrack(e);
                    return null;
                }
            }

        }
        #endregion

        #region IPath URL


        public static class Path
        {
            private static readonly string PersonalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#pragma warning disable 618
            //public static string AndroidDcimFolder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim).AbsolutePath;
#pragma warning restore 618
            internal static string AndroidDcimFolder = GetDirectoryDcim();

            //DcimFolder 
            public static readonly string FolderDcimMyApp = AndroidDcimFolder + "/" + AppSettings.ApplicationName + "/";
            public static readonly string FolderDcimImage = FolderDcimMyApp + "/Images/";
            public static readonly string FolderDcimVideo = FolderDcimMyApp + "/Video/";
            public static readonly string FolderDcimStory = FolderDcimMyApp + "/Story/";
            public static readonly string FolderDcimSound = FolderDcimMyApp + "/Sound/";
            public static readonly string FolderDcimFile = FolderDcimMyApp + "/File/";
            public static readonly string FolderDcimGif = FolderDcimMyApp + "/Gif/";
            public static readonly string FolderDcimSticker = FolderDcimMyApp + "/Sticker/";
            //public static readonly string FolderDcimNiceArt = FolderDcimMyApp + "/Editor/";

            //Disk
            public static readonly string FolderDiskMyApp = PersonalFolder + "/" + AppSettings.ApplicationName + "/";
            public static readonly string FolderDiskImage = FolderDiskMyApp + "/Images/";
            public static readonly string FolderDiskVideo = FolderDiskMyApp + "/Video/";
            public static readonly string FolderDiskStory = FolderDiskMyApp + "/Story/";
            public static readonly string FolderDiskSound = FolderDiskMyApp + "/Sound/";
            public static readonly string FolderDiskFile = FolderDiskMyApp + "/File/";
            public static readonly string FolderDiskGif = FolderDiskMyApp + "/Gif/";
            public static readonly string FolderDiskSticker = FolderDiskMyApp + "/Sticker/";
            public static readonly string FolderDiskNiceArt = FolderDiskMyApp + "/Editor/";

            public static string GetDirectoryDcim()
            {
                try
                {
                    switch (Build.VERSION.SdkInt)
                    {
                        case > BuildVersionCodes.Q:
                            {
                                //Creates DCIM folder in the main app path in android devices
                                var directories = Application.Context.GetExternalFilesDirs(Android.OS.Environment.DirectoryDcim);
                                if (directories!.Any())
                                    return directories[0].AbsolutePath;
                                break;
                            }
                        default:
                            {
                                var directories1 = Application.Context.GetExternalFilesDir(""); //storage/emulated/0/Android/data/com.wowondermessenger.app/files
                                if (directories1 != null)
                                {
                                    var pathDefault = directories1.AbsolutePath.Split("/Android/")?.FirstOrDefault();
                                    switch (string.IsNullOrEmpty(pathDefault))
                                    {
                                        case false:
                                            return pathDefault;
                                    }
                                }

                                break;
                            }
                    }

                    var a1 = Application.Context.CacheDir?.AbsoluteFile;
                    return a1 + "/" + Android.OS.Environment.DirectoryDcim;
                }
                catch (Exception e)
                {
                    DisplayReportResultTrack(e);
                    return null!;
                }
            }

            public static void Chack_MyFolder(string id = "")
            {
                try
                {
                    if (!Directory.Exists(FolderDcimMyApp))
                        Directory.CreateDirectory(FolderDcimMyApp);

                    if (!Directory.Exists(FolderDiskMyApp))
                        Directory.CreateDirectory(FolderDiskMyApp);

                    if (!Directory.Exists(FolderDcimImage + "/" + id))
                        Directory.CreateDirectory(FolderDcimImage + "/" + id);

                    if (!Directory.Exists(FolderDcimVideo + "/" + id))
                        Directory.CreateDirectory(FolderDcimVideo + "/" + id);

                    if (!Directory.Exists(FolderDcimStory))
                        Directory.CreateDirectory(FolderDcimStory);

                    if (!Directory.Exists(FolderDcimSound + "/" + id))
                        Directory.CreateDirectory(FolderDcimSound + "/" + id);

                    if (!Directory.Exists(FolderDcimFile + "/" + id))
                        Directory.CreateDirectory(FolderDcimFile + "/" + id);

                    if (!Directory.Exists(FolderDcimGif + "/" + id))
                        Directory.CreateDirectory(FolderDcimGif + "/" + id);

                    //if (!Directory.Exists(FolderDcimPost))
                    //    Directory.CreateDirectory(FolderDcimPost);

                    //if (!Directory.Exists(FolderDcimNiceArt))
                    //    Directory.CreateDirectory(FolderDcimNiceArt);

                    //================================================

                    if (!Directory.Exists(FolderDiskImage + "/" + id))
                        Directory.CreateDirectory(FolderDiskImage + "/" + id);

                    if (!Directory.Exists(FolderDiskVideo + "/" + id))
                        Directory.CreateDirectory(FolderDiskVideo + "/" + id);

                    if (!Directory.Exists(FolderDiskStory))
                        Directory.CreateDirectory(FolderDiskStory);

                    if (!Directory.Exists(FolderDiskFile + "/" + id))
                        Directory.CreateDirectory(FolderDiskFile + "/" + id);

                    if (!Directory.Exists(FolderDiskSound + "/" + id))
                        Directory.CreateDirectory(FolderDiskSound + "/" + id);

                    if (!Directory.Exists(FolderDiskGif + "/" + id))
                        Directory.CreateDirectory(FolderDiskGif + "/" + id);

                    if (!Directory.Exists(FolderDiskSticker + "/" + id))
                        Directory.CreateDirectory(FolderDiskSticker + "/" + id);

                    if (!Directory.Exists(FolderDiskNiceArt))
                        Directory.CreateDirectory(FolderDiskNiceArt);
                }
                catch (Exception e)
                {
                    DisplayReportResultTrack(e);
                }
            }

            public static void DeleteAll_FolderUser(string id = "")
            {
                try
                {
                    if (Directory.Exists(FolderDcimImage + "/" + id))
                        Directory.Delete(FolderDcimImage + "/" + id, true);

                    if (Directory.Exists(FolderDcimVideo + "/" + id))
                        Directory.Delete(FolderDcimVideo + "/" + id, true);

                    if (Directory.Exists(FolderDcimStory + "/" + id))
                        Directory.Delete(FolderDcimStory + "/" + id, true);

                    if (Directory.Exists(FolderDcimSound + "/" + id))
                        Directory.Delete(FolderDcimSound + "/" + id, true);

                    if (Directory.Exists(FolderDcimGif + "/" + id))
                        Directory.Delete(FolderDcimGif + "/" + id, true);

                    if (Directory.Exists(FolderDcimSticker + "/" + id))
                        Directory.Delete(FolderDcimSticker + "/" + id, true);

                    //if (!Directory.Exists(FolderDcimNiceArt))
                    //    Directory.CreateDirectory(FolderDcimNiceArt);

                    //================================================

                    //if (Directory.Exists(FolderDcimMyApp))
                    //    Directory.Delete(FolderDcimMyApp,true);

                    if (Directory.Exists(FolderDiskImage + "/" + id))
                        Directory.Delete(FolderDiskImage + "/" + id, true);

                    if (Directory.Exists(FolderDiskVideo + "/" + id))
                        Directory.Delete(FolderDiskVideo + "/" + id, true);

                    if (Directory.Exists(FolderDiskStory + "/" + id))
                        Directory.Delete(FolderDiskStory + "/" + id, true);

                    if (Directory.Exists(FolderDiskSound + "/" + id))
                        Directory.Delete(FolderDiskSound + "/" + id, true);

                    if (Directory.Exists(FolderDiskGif + "/" + id))
                        Directory.Delete(FolderDiskGif + "/" + id, true);

                    if (Directory.Exists(FolderDiskSticker + "/" + id))
                        Directory.Delete(FolderDiskSticker + "/" + id, true);

                    if (!Directory.Exists(FolderDiskNiceArt + "/" + id))
                        Directory.Delete(FolderDiskNiceArt + "/" + id, true);
                }
                catch (Exception e)
                {
                    DisplayReportResultTrack(e);
                }
            }

            public static void DeleteAll_MyFolderDisk()
            {
                try
                {
                    if (Directory.Exists(FolderDiskImage))
                        Directory.Delete(FolderDiskImage, true);

                    if (Directory.Exists(FolderDiskVideo))
                        Directory.Delete(FolderDiskVideo, true);

                    if (Directory.Exists(FolderDiskStory))
                        Directory.Delete(FolderDiskStory, true);

                    if (Directory.Exists(FolderDiskSound))
                        Directory.Delete(FolderDiskSound, true);

                    if (Directory.Exists(FolderDiskGif))
                        Directory.Delete(FolderDiskGif, true);

                    if (Directory.Exists(FolderDiskSticker))
                        Directory.Delete(FolderDiskSticker, true);

                    if (!Directory.Exists(FolderDiskNiceArt))
                        Directory.Delete(FolderDiskNiceArt, true);

                    if (Directory.Exists(FolderDiskMyApp))
                        Directory.Delete(FolderDiskMyApp, true);
                }
                catch (Exception e)
                {
                    DisplayReportResultTrack(e);
                }
            }
        }

        #endregion

        #pragma warning disable 618
        public class AppLifecycleObserver : Java.Lang.Object, IGenericLifecycleObserver
        #pragma warning restore 618
        {
            //public enum AppLifeState
            //{
            //    Foreground = 1,
            //    Background = 2
            //}

            public static string AppState { set; get; }

            public void OnStateChanged(ILifecycleOwner p0, Lifecycle.Event p1)
            {
                try
                {
                    if (p1 == Lifecycle.Event.OnDestroy || p1 == Lifecycle.Event.OnStop)
                    {
                        AppState = "Background";
                    }
                    else if (p1 == Lifecycle.Event.OnStart)
                    {
                        AppState = "Foreground";
                    }
                    //AppState = p1 == Lifecycle.Event.OnDestroy || p1 == Lifecycle.Event.OnStop ? AppLifeState.Background : p1 == Lifecycle.Event.OnDestroy ? AppLifeState.Foreground : AppLifeState.Background;
                    //Toast.MakeText(Application.Context, AppState.ToString(), ToastLength.Short).Show(); 
                }
                catch (Exception e)
                {
                    DisplayReportResultTrack(e);
                }
            }
        }

    }
}