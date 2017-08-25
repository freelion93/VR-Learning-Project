using UnityEngine;
using System;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;


public class VRVUPoseServer : MonoBehaviour {

    public int portNo = 1234;
	public Vector3 position;
	public Quaternion rotation;

	private static bool serverActive = false;
	private static Thread senderThread;
	private static VRVUServer senderObj;
	private PacketHeader curr_pose = new PacketHeader();

	// Use this for initialization
	void Start ()
	{
		startSender();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(senderObj != null)
		{
			serverActive = senderObj.getRunning();
		}
		else
		{
			serverActive = false;
		}
		if(serverActive)
		{
			curr_pose.t_x = position.x;
			curr_pose.t_y = position.y;
			curr_pose.t_z = position.z;

			curr_pose.q_x = rotation.x;
			curr_pose.q_y = rotation.y;
			curr_pose.q_z = rotation.z;
			curr_pose.q_w = rotation.w;

			senderObj.sendPose(curr_pose);
		}
        rotkeys();
        rotkeys2();

        if (Input.GetKey(KeyCode.K))
        {
            curr_pose.t_x += 0.1f;
            position.x += 0.1f;
        }
        if (Input.GetKey(KeyCode.J))
        {
            curr_pose.t_y += 0.05f;
            position.y += 0.05f;
        }
        if (Input.GetKey(KeyCode.N))
        {
            curr_pose.t_x -= 0.05f;
            position.x -= 0.05f;
        }
        if (Input.GetKey(KeyCode.M))
        {
            curr_pose.t_y -= 0.05f;
            position.y -= 0.05f;
        }

    }
    private void rotkeys()
    {
        if (Input.GetKey(KeyCode.R))
        {
            curr_pose.q_y += 0.05f;
            rotation.y += 0.05f;
        }

        if (Input.GetKey(KeyCode.T))
        {
            curr_pose.q_z += 0.05f;
            rotation.z += 0.05f;
        }

        if (Input.GetKey(KeyCode.F))
        {
            curr_pose.q_y -= 0.05f;
            rotation.y -= 0.05f;
        }

        if (Input.GetKey(KeyCode.G))
        {
            curr_pose.q_z -= 0.05f;
            rotation.z -= 0.05f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            curr_pose.q_y = 0;
            rotation.y = 0;
            curr_pose.q_z = 0;
            rotation.z = 0;
            curr_pose.q_x = 0;
            rotation.x = 0;
            curr_pose.q_w = 0;
            rotation.w = 0;
        }
    }
    private void rotkeys2()
    {
        
        if (Input.GetKey(KeyCode.Keypad9))
        {
            curr_pose.q_x = 0.924f;
            rotation.x = 0.924f;
            curr_pose.q_w = 0.383f;
            rotation.w = 0.383f;
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            curr_pose.q_x = 0.707f;
            rotation.x = 0.707f;
            curr_pose.q_w = 0.707f;
            rotation.w = 0.707f;
        }
        if (Input.GetKey(KeyCode.Keypad3))
        {
            curr_pose.q_x = 0.383f;
            rotation.x = 0.383f;
            curr_pose.q_w = 0.924f;
            rotation.w = 0.924f;
        }

        if (Input.GetKey(KeyCode.Keypad2))
        {
            curr_pose.q_x = 0;
            rotation.x = 0f;
            curr_pose.q_w = 1f;
            rotation.w = 1f;
        }
        if (Input.GetKey(KeyCode.Keypad1))
        {
            curr_pose.q_x = -0.383f;
            rotation.x = -0.383f;
            curr_pose.q_w = 0.924f;
            rotation.w = 0.924f;
        }
        if (Input.GetKey(KeyCode.Keypad4))
        {
            curr_pose.q_x = -0.707f;
            rotation.x = -0.707f;
            curr_pose.q_w = 0.707f;
            rotation.w = 0.707f;
        }
        if (Input.GetKey(KeyCode.Keypad7))
        {
            curr_pose.q_x = -0.924f;
            rotation.x = -0.924f;
            curr_pose.q_w = 0.383f;
            rotation.w = 0.383f;
        }
        if (Input.GetKey(KeyCode.Keypad5))
        {
            curr_pose.q_x += 1f;
            rotation.x += 1f;
            curr_pose.q_w += 0.009f;
            rotation.w += 0.009f;
        }
        //rotate 120 deegrees right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            curr_pose.q_x = 0.500f;
            rotation.x = 0.500f;
            curr_pose.q_w = 0.866f;
            rotation.w = 0.866f;
        }
        //rotate 120 deegrees left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            curr_pose.q_x = 0.500f;
            rotation.x = 0.500f;
            curr_pose.q_w = -0.866f;
            rotation.w = -0.866f;
        }
    }

    public void startSender()
	{
		if(!serverActive)
		{
			if(senderObj == null)
			{
				senderObj = new VRVUServer();
				senderObj.init(portNo);
			}
			senderThread = new Thread(senderObj.DoWork);
			senderThread.Start();
			serverActive = true;
			Debug.Log ("PoseServer: sender started");
		}
	}
	
	public void stopSender()
	{
		if(serverActive)
		{
			serverActive = false;
			senderObj.RequestStop();
			senderThread.Join();
		}
	}
	
	public void OnApplicationQuit()
	{
		if(serverActive)
		{
			stopSender();
		}
	}
}

public class VRVUServer
{
	private PacketHeader pose_sent = new PacketHeader();
	private byte[] receivedData;
	private UdpClient server;
	private IPEndPoint ep;

	volatile private bool handshake = false;
	volatile private bool running = false; 

	public void init(int port_no)
	{
		Debug.Log("VRVU DUMMY Server - Sending 6DOF Pose Data (2017)");
		Debug.Log("==========================================================");

		IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port_no);
		server = new UdpClient(ipep);

		ep = new IPEndPoint(IPAddress.Any, 0);

		running = true;
	}

	public void sendPose(PacketHeader pose)
	{
		pose_sent = pose;
	}

	public bool getRunning()
	{
		return running;
	}

	public void RequestStop()
	{
		running = false;
	}

	private void receiveCallback(IAsyncResult res)
	{
		receivedData = server.EndReceive(res, ref ep);

		if (receivedData.Length == 28)
		{
			bool valid = true;

			for (int i = 0; i < 28; i++)
			{
				if (receivedData[i] != (i + 1))
				{
					valid = false;
					break;
				}
			}

			if (valid)
			{
				handshake = true;
				Debug.Log("Server handshake");
				return;
			}
		}
	}

	public void DoWork()
	{
		IAsyncResult res = server.BeginReceive(new AsyncCallback(receiveCallback), null);
		while (running && !handshake)
		{
			Thread.Sleep(1);
		}
		
		while (running)
		{
			byte[] packet_header = new byte[28];

			WritePacketHeader(ref packet_header);

			server.Send(packet_header, 28, ep);

			Thread.Sleep(10);
		}
		server.Close();
	}

	public void WritePacketHeader(ref byte[] byte_array)
	{
		MemoryStream stream = new MemoryStream(byte_array);
		BinaryWriter writer = new BinaryWriter(stream);

		writer.Write(pose_sent.t_x);
		writer.Write(pose_sent.t_y);
		writer.Write(pose_sent.t_z);

		writer.Write(pose_sent.q_x);
		writer.Write(pose_sent.q_y);
		writer.Write(pose_sent.q_z);
		writer.Write(pose_sent.q_w);
	}
}