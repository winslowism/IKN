using System;
using System.IO;
using System.Text;
using Transportlaget;
using Library;

namespace Application
{
	class file_server
	{
		/// <summary>
		/// The BUFSIZE
		/// </summary>
		private const int BUFSIZE = 1000;

		/// <summary>
		/// Initializes a new instance of the <see cref="file_server"/> class.
		/// </summary>
		private file_server ()
		{
			Transport transport = new Transport (BUFSIZE);


			Console.WriteLine("Receiving filename");


			byte[] receivedFilename = new byte[1000];

			int buffSize = transport.receive (ref receivedFilename);

			String filename = Encoding.ASCII.GetString (receivedFilename, 0, buffSize);

			Console.WriteLine ("Received filename: " + filename);

			long filesize = LIB.check_File_Exists (filename);

			byte[] bytefilesize = Encoding.ASCII.GetBytes (filesize.ToString());


			Console.WriteLine ("Sending filesize");
			transport.send (bytefilesize, bytefilesize.Length);
			Console.WriteLine ("Filesize sent: " + filesize);

			if (filesize > 0) 
			{
				sendFile (filename, filesize, transport);
			}

		}

		/// <summary>
		/// Sends the file.
		/// </summary>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		/// <param name='fileSize'>
		/// File size.
		/// </param>
		/// <param name='tl'>
		/// Tl.
		/// </param>
		private void sendFile(String fileName, long fileSize, Transport transport)
		{
			Console.WriteLine ("Sending file");
			byte[] readBytesFromFile = new byte[BUFSIZE];

			FileStream filestream = File.OpenRead(fileName);

			int actualReadBytes = 0;
			while ( (actualReadBytes = filestream.Read (readBytesFromFile, 0, BUFSIZE)) > 0)
			{
				transport.send (readBytesFromFile, actualReadBytes);
			}
		}

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main (string[] args)
		{
			new file_server();
		}
	}
}