using UnityEngine;
using System.IO.Ports;
using System.Collections;
using System.Text;
using System;

public class TouchSerial : MonoBehaviour
{
    private SerialPort Serial1P;
    private bool CanRead = false;
    private byte[] ReadBuf = new byte[6] { 0x7B, 0x00, 0x00, 0x00, 0x00, 0x7D };
    private int ReadCursor = 0;
    private byte[] WriteBuf = new byte[6] { 0x28, 0x00, 0x00, 0x00, 0x00, 0x29 };
    private byte[] TouchPacket = new byte[9] { 0x28, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x29 };

    private bool IsConditioningMode = false;

    public string COM;

    private void Start()
    {
        Debug.Log("Start Serial");
        Serial1P = new SerialPort(COM, 9600);
        Serial1P.Open();
        Debug.Log("Serial Started");
    }

    private void Update()
    {
        ReadData();
        SendData();
    }

    private void ReadData()
    {
        int len = Serial1P.BytesToRead;
        if (len > 0)
        {
            var b = new byte[len];
            Serial1P.Read(b, 0, len);
            foreach (var i in b)
            {
                if (i == 0x7B) // 命令开始
                {
                    CanRead = true;
                    ReadCursor = 1;
                }
                else if (i == 0x7D) // 命令结束
                {
                    CanRead = false;
                    // 处理命令包
                    PkgHandle();
                } 
                else if (CanRead && ReadCursor <= 4) // 命令正文
                {
                    ReadBuf[ReadCursor] = i;
                    ReadCursor++;
                }
            }
        }
    }

    private void PkgHandle()
    {
        string command = Encoding.ASCII.GetString(ReadBuf);

        Debug.Log("收到指令: " + command);

        switch (command)
        {
            case "{RSET}":
                break;
            case "{HALT}":
                IsConditioningMode = true;
                break;
            case "{STAT}":
                IsConditioningMode = false;
                break;
            default:
                if (IsConditioningMode)
                {
                    Array.Copy(ReadBuf, 1, WriteBuf, 1, 4);
                    Serial1P.Write(WriteBuf, 0, 6);
                }
                break;
        }
    }

    private void SendData()
    {
        if (!IsConditioningMode)
            Serial1P.Write(TouchPacket, 0, 9);
    }

    public void ChangeTouch(int Area, bool State)
    {
        ByteArrayExt.SetBit(TouchPacket, Area + 8, State);
    }
}

public static class ByteArrayExt
{
    public static byte[] SetBit(this byte[] self, int index, bool value)
    {
        var bitArray = new BitArray(self);
        bitArray.Set(index, value);
        bitArray.CopyTo(self, 0);
        return self;
    }
}