using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections;

///发布日期20190630
///本类库由云未归来完成，引用请保留作者信息
///命名空间是FIRC团队名称，修改时候请不要更改命名空间
namespace FIRC
{
    public class SerialPortManger
    {
        [DllImport("PComm.dll", EntryPoint = "sio_open")]
        public static extern int sio_open(int port);

        [DllImport("PComm.dll", EntryPoint = "sio_ioctl")]
        public static extern int sio_ioctl(int port, int baud, int mode);

        [DllImport("PComm.dll", EntryPoint = "sio_DTR")]
        public static extern int sio_DTR(int port, int mode);

        [DllImport("PComm.dll", EntryPoint = "sio_RTS")]
        public static extern int sio_RTS(int port, int mode);

        [DllImport("PComm.dll", EntryPoint = "sio_close")]
        public static extern int sio_close(int port);

        [DllImport("PComm.dll", EntryPoint = "sio_read")]
        public static extern int sio_read(int port, ref byte data, int length);


        [DllImport("PComm.dll", EntryPoint = "sio_write")]
        public static extern int sio_write(int port, string data, int length);
        [DllImport("PComm.dll", EntryPoint = "sio_write")]
        public static extern int sio_write(int port, byte[] data, int length);

        [DllImport("PComm.dll", EntryPoint = "sio_SetReadTimeouts")]
        public static extern int sio_SetReadTimeouts(int port, int TotalTimeouts, int IntervalTimeouts);

        [DllImport("PComm.dll", EntryPoint = "sio_SetWriteTimeouts")]
        public static extern int sio_SetWriteTimeouts(int port, int TotalTimeouts, int IntervalTimeouts);

        [DllImport("PComm.dll", EntryPoint = "sio_AbortRead")]
        public static extern int sio_AbortRead(int port);

        [DllImport("PComm.dll", EntryPoint = "sio_AbortWrite")]
        public static extern int sio_AbortWrite(int port);

        [DllImport("PComm.dll", EntryPoint = "sio_getmode")]
        public static extern int sio_getmode(int port);

        [DllImport("PComm.dll", EntryPoint = "sio_getbaud")]
        public static extern int sio_getbaud(int port);

        [DllImport("PComm.dll", EntryPoint = "sio_flowctrl")]
        public static extern int sio_flowctrl(int port, int mode);
        [DllImport("PComm.dll", EntryPoint = "sio_flush")]
        public static extern int sio_flush(int port, int func);
        [DllImport("PComm.dll", EntryPoint = "sio_lctrl")]
        public static extern int sio_lctrl(int port, int mode);

        [DllImport("PComm.dll", EntryPoint = "sio_baud")]
        public static extern int sio_baud(int port, long speed);

        [DllImport("PComm.dll", EntryPoint = "sio_getch")]
        public static extern int sio_getch(int port);
        [DllImport("PComm.dll", EntryPoint = "sio_linput")]
        public static extern int sio_linput(int port, byte[] data, int nLen, int term);
        [DllImport("PComm.dll", EntryPoint = "sio_putch")]
        public static extern int sio_putch(int port, int term);
        [DllImport("PComm.dll", EntryPoint = "sio_iqueue")]
        public static extern int sio_iqueue(int port);
        [DllImport("PComm.dll", EntryPoint = "sio_oqueue")]
        public static extern int sio_oqueue(int port);
        [DllImport("PComm.dll", EntryPoint = "sio_data_status")]
        public static extern int sio_data_status(int port);


        // Function return error code
        private const int SIO_OK = 0;
        private const int SIO_BADPORT = -1;
        private const int SIO_OUTCONTROL = -2;
        private const int SIO_NODATA = -4;
        private const int SIO_OPENFAIL = -5;
        private const int SIO_RTS_BY_HW = -6;
        private const int SIO_BADPARAM = -7;
        private const int SIO_WIN32FAIL = -8;
        private const int SIO_BOARDNOTSUPPORT = -9;
        private const int SIO_ABORT_WRITE = -11;
        private const int SIO_WRITETIMEOUT = -12;

        // Self Define function return error code
        private const int ERR_NOANSWER = -101;



        // Mode setting Data bits define
        private const int BIT_5 = 0x0;
        private const int BIT_6 = 0x1;
        private const int BIT_7 = 0x2;
        private const int BIT_8 = 0x3;

        // Mode setting Stop bits define
        private const int STOP_1 = 0x0;
        private const int STOP_2 = 0x4;

        // Mode setting Parity define
        private const int P_EVEN = 0x18;
        private const int P_ODD = 0x8;
        private const int P_SPC = 0x38;
        private const int P_MRK = 0x28;
        private const int P_NONE = 0x0;


        /// <summary>
        /// 获取通讯错误消息
        /// </summary>
        /// <param name="i_ErrCode">错误码</param>
        /// <returns>错误消息</returns>
        public string GetCommErrMsg(int i_ErrCode)
        {
            switch (i_ErrCode)
            {
                case SIO_OK: return "成功";
                case SIO_BADPORT: return "串口号无效,检测串口号!";
                case SIO_OUTCONTROL: return "主板不是MOXA兼容的智能主板!";
                case SIO_NODATA: return "没有可读的数据!";
                case SIO_OPENFAIL: return "打开串口失败,检查串口是否被占用!";
                case SIO_RTS_BY_HW: return "不能控制串口因为已经通过sio_flowctrl设定为自动H/W流控制";
                case SIO_BADPARAM: return "串口参数错误,检查串口参数!";
                case SIO_WIN32FAIL: return "调用Win32函数失败!";
                case SIO_BOARDNOTSUPPORT: return "串口不支持这个函数!";
                case SIO_ABORT_WRITE: return "用户终止写数据块!";
                case SIO_WRITETIMEOUT: return "写数据超时!";
                case ERR_NOANSWER: return "无应答!";
                default: return i_ErrCode.ToString();
            }
        }


        public bool IsOpen
        {
            get { return HasOpened; }
        }
        private bool HasOpened = false;

        public int PortIndex = 1;
        public delegate void dgMsg(byte[] data);
        public event dgMsg OnReceiveData;
        private bool IsListening = true;
        private void StartListening()
        {
            byte[] data = new byte[1024];
            int nByteLen = 0;
            while (IsListening)
            {
                nByteLen = sio_read(PortIndex, ref data[0], sio_iqueue(PortIndex));

                if (nByteLen > 0)
                {
                    byte[] recv = new byte[nByteLen];
                    Array.Copy(data, 0, recv, 0, nByteLen);
                    OnReceiveData(recv);
                    FlushBuffer(PortIndex, FlushTypes.Both);
                }
                Thread.Sleep(10);
            }
        }
        private void StopListening()
        {
            IsListening = false;
        }
        public bool Open(int port)
        {
            PortIndex = port;
            int nRes = sio_open(port);
            if (nRes == SIO_OK)
            {
                HasOpened = true;
                IsListening = true;
                Thread th = new Thread(StartListening);
                th.IsBackground = true;
                th.Start();
                return true;
            }

            return false;
        }
        public bool Open(string portname)
        {
            int port = Convert.ToInt32(portname.Substring(3));
            PortIndex = port;
            int nRes = sio_open(port);
            if (nRes == SIO_OK)
            {
                HasOpened = true;
                IsListening = true;
                Thread th = new Thread(StartListening);
                th.IsBackground = true;
                th.Start();
                return true;
            }
            return false;
        }
        public bool Open()
        {
            int nRes = sio_open(PortIndex);
            if (nRes == SIO_OK)
            {
                HasOpened = true;
                IsListening = true;
                Thread th = new Thread(StartListening);
                th.IsBackground = true;
                th.Start();
                return true;
            }
            return false;
        }

        public bool Close(int port)
        {
            StopListening();
            int nRes = sio_close(port);
            if (nRes == SIO_OK)
            {
                HasOpened = false;
                return true;
            }
            return false;
        }
        public bool Close(string portname)
        {
            int port = Convert.ToInt32(portname.Substring(3));
            int nRes = sio_close(port);
            if (nRes == SIO_OK)
                if (nRes == SIO_OK)
                {
                    HasOpened = false;
                    return true;
                }
            return false;
        }
        public bool Close()
        {
            int nRes = sio_close(PortIndex);
            if (nRes == SIO_OK)
                if (nRes == SIO_OK)
                {
                    HasOpened = false;
                    return true;
                }
            return false;
        }



        public void SetPort(string PortName = "COM1", int Baud = 9600, DataBits db = DataBits.Eight, StopBits sb = StopBits.One, Parity parity = Parity.None)
        {
            //端口号
            int nPort = Convert.ToInt32(PortName.Substring(3));
            //检验位
            int nParity = P_NONE;
            switch (parity)
            {
                case Parity.None:
                    nParity = P_NONE;
                    break;
                case Parity.Even:
                    nParity = P_EVEN;
                    break;
                case Parity.Mark:
                    nParity = P_MRK;
                    break;
                case Parity.Odd:
                    nParity = P_ODD;
                    break;
                case Parity.Space:
                    nParity = P_SPC;
                    break;
            }
            //停止位
            int nStopbits = STOP_1;
            switch (sb)
            {
                case StopBits.One:
                    nParity = STOP_1;
                    break;
                case StopBits.Two:
                    nParity = STOP_2;
                    break;
            }
            //数据位
            int nDatabits = BIT_8;
            switch (db)
            {
                case DataBits.Five:
                    nDatabits = BIT_5;
                    break;
                case DataBits.Six:
                    nParity = BIT_6;
                    break;
                case DataBits.Seven:
                    nParity = BIT_7;
                    break;
                case DataBits.Eight:
                    nParity = BIT_8;
                    break;
            }

            sio_ioctl(nPort, Baud, nParity | nDatabits | nStopbits);

        }
        public bool SetWriteTimeOut(int port, int totaltimeout, int intervaltimeout)
        {
            int nRes = sio_SetWriteTimeouts(port, totaltimeout, intervaltimeout);
            if (nRes == SIO_OK)
                return true;
            return false;
        }

        public bool SetWriteTimeOut(string portname, int totaltimeout, int intervaltimeout)
        {
            int port = Convert.ToInt32(portname.Substring(3));
            int nRes = sio_SetWriteTimeouts(port, totaltimeout, intervaltimeout);
            if (nRes == SIO_OK)
                return true;
            return false;
        }


        public bool SetWriteTimeOut(int totaltimeout, int intervaltimeout)
        {

            int nRes = sio_SetWriteTimeouts(PortIndex, totaltimeout, intervaltimeout);
            if (nRes == SIO_OK)
                return true;
            return false;
        }


        public bool SetReadTimeOut(int port, int totaltimeout, int intervaltimeout)
        {
            int nRes = sio_SetReadTimeouts(port, totaltimeout, intervaltimeout);
            if (nRes == SIO_OK)
                return true;
            return false;
        }

        public bool SetReadTimeOut(string portname, int totaltimeout, int intervaltimeout)
        {
            int port = Convert.ToInt32(portname.Substring(3));
            int nRes = sio_SetReadTimeouts(port, totaltimeout, intervaltimeout);
            if (nRes == SIO_OK)
                return true;
            return false;
        }


        public bool SetReadTimeOut(int totaltimeout, int intervaltimeout)
        {

            int nRes = sio_SetReadTimeouts(PortIndex, totaltimeout, intervaltimeout);
            if (nRes == SIO_OK)
                return true;
            return false;
        }

        public bool FlushBuffer(int port, FlushTypes ft)
        {
            int func = 2;
            switch (ft)
            {
                case FlushTypes.In:
                    func = 0;
                    break;
                case FlushTypes.Out:
                    func = 1;
                    break;
                case FlushTypes.Both:
                    func = 2;
                    break;

            }
            int nRes = sio_flush(port, func);
            if (nRes == SIO_OK)
                return true;
            return false;
        }

        public bool FlushBuffer(string portname, FlushTypes ft)
        {
            int port = Convert.ToInt32(portname.Substring(3));
            int func = 2;
            switch (ft)
            {
                case FlushTypes.In:
                    func = 0;
                    break;
                case FlushTypes.Out:
                    func = 1;
                    break;
                case FlushTypes.Both:
                    func = 2;
                    break;

            }
            int nRes = sio_flush(port, func);
            if (nRes == SIO_OK)
                return true;
            return false;
        }
        public bool FlushBuffer(FlushTypes ft)
        {

            int func = 2;
            switch (ft)
            {
                case FlushTypes.In:
                    func = 0;
                    break;
                case FlushTypes.Out:
                    func = 1;
                    break;
                case FlushTypes.Both:
                    func = 2;
                    break;

            }
            int nRes = sio_flush(PortIndex, func);
            if (nRes == SIO_OK)
                return true;
            return false;
        }


        public int Write(int port, string data)
        {
            return sio_write(port, data, data.Length);
        }
        public int Write(string portname, string data)
        {
            int port = Convert.ToInt32(portname.Substring(3));
            return sio_write(port, data, data.Length);
        }


        public int Write(string data)
        {

            return sio_write(PortIndex, data, data.Length);
        }

        public int WriteLine(int port, string data)
        {
            return sio_write(port, data + "\r\n", data.Length + 2);
        }
        public int WriteLine(string portname, string data)
        {
            int port = Convert.ToInt32(portname.Substring(3));
            return sio_write(port, data + "\r\n", data.Length + 2);
        }


        public int WriteLine(string data)
        {
            return sio_write(PortIndex, data + "\r\n", data.Length + 2);
        }




        public int Write(int port, byte[] data)
        {
            return sio_write(port, data, data.Length);
        }
        public int Write(string portname, byte[] data)
        {
            int port = Convert.ToInt32(portname.Substring(3));
            return sio_write(port, data, data.Length);
        }


        public int Write(byte[] data)
        {
            return sio_write(PortIndex, data, data.Length);
        }

        public int WriteLine(int port, byte[] data)
        {
            byte[] data1 = new byte[data.Length + 2];
            Array.Copy(data1, 0, data, 0, data.Length);
            data1[data1.Length - 1] = 10;
            data1[data1.Length - 2] = 13;
            return sio_write(port, data1, data1.Length);
        }
        public int WriteLine(string portname, byte[] data)
        {
            int port = Convert.ToInt32(portname.Substring(3));
            byte[] data1 = new byte[data.Length + 2];
            Array.Copy(data1, 0, data, 0, data.Length);
            data1[data1.Length - 1] = 10;
            data1[data1.Length - 2] = 13;
            return sio_write(port, data1, data1.Length);
        }


        public int WriteLine(byte[] data)
        {

            byte[] data1 = new byte[data.Length + 2];
            Array.Copy(data1, 0, data, 0, data.Length);
            data1[data1.Length - 1] = 10;
            data1[data1.Length - 2] = 13;
            return sio_write(PortIndex, data1, data1.Length);
        }
        public void SetPort(int PortIndex = 1, int Baud = 9600, DataBits db = DataBits.Eight, StopBits sb = StopBits.One, Parity parity = Parity.None)
        {

            //检验位
            int nParity = P_NONE;
            switch (parity)
            {
                case Parity.None:
                    nParity = P_NONE;
                    break;
                case Parity.Even:
                    nParity = P_EVEN;
                    break;
                case Parity.Mark:
                    nParity = P_MRK;
                    break;
                case Parity.Odd:
                    nParity = P_ODD;
                    break;
                case Parity.Space:
                    nParity = P_SPC;
                    break;
            }
            //停止位
            int nStopbits = STOP_1;
            switch (sb)
            {
                case StopBits.One:
                    nParity = STOP_1;
                    break;
                case StopBits.Two:
                    nParity = STOP_2;
                    break;
            }
            //数据位
            int nDatabits = BIT_8;
            switch (db)
            {
                case DataBits.Five:
                    nDatabits = BIT_5;
                    break;
                case DataBits.Six:
                    nParity = BIT_6;
                    break;
                case DataBits.Seven:
                    nParity = BIT_7;
                    break;
                case DataBits.Eight:
                    nParity = BIT_8;
                    break;
            }

            sio_ioctl(PortIndex, (int)((BaudRate)Enum.Parse(typeof(BaudRate), "B" + Baud)), nParity | nDatabits | nStopbits);

        }
    }


    public enum Parity
    {
        None,
        Even,
        Odd,
        Mark,
        Space
    }

    public enum FlushTypes
    {
        In,
        Out,
        Both
    }

    public enum StopBits
    {
        One,
        Two
    }
    public enum DataBits
    {
        Five,
        Six,
        Seven,
        Eight
    }

    public enum BaudRate
    {
        B50,
        B75,
        B110,
        B134,
        B150,
        B300,
        B600,
        B1200,
        B1800,
        B2400,
        B4800,
        B7200,
        B9600,
        B19200,
        B38400,
        B57600,
        B115200,
        B23040,
        B460800,
        B921600
    }




}

