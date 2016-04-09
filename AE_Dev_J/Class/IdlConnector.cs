using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AE_Dev_J
{
    /// <summary>
    /// 这个类专门用来与IDL算法做交互
    /// </summary>
    class IdlConnector
    {
        private string m_profilename = null; // *.pro文件完整路径
        private string m_functionName = null; // *.pro文件中的函数名称，通常是*.pro文件去掉后缀的名称
        private string m_runStr = null; // 运行命令字符串

        private COM_IDL_connectLib.COM_IDL_connect idlCon;

        public string Profilename
        {
            get { return m_profilename; }
            set { m_profilename = value; }
        }
        public string RunStr
        {
            get { return m_runStr; }
            set { m_runStr = value; }
        }
        public string FunctionName
        {
            get { return m_functionName; }
            set { m_functionName = value; }
        }

        /// <summary>
        /// 构造函数，需要*.pro文件路径做参数
        /// </summary>
        /// <param name="profilename">*.pro文件路径</param>
        public IdlConnector(string profilename)
        {
            if (profilename == null) { return; }
            FileInfo finfor = new FileInfo(profilename);

            if (!finfor.Exists) { return; }
            idlCon = new COM_IDL_connectLib.COM_IDL_connect();
            idlCon.CreateObject(0, 0, 0);

            m_profilename = profilename;
            m_functionName = Path.GetFileNameWithoutExtension(finfor.FullName);
            try
            {
                idlCon.ExecuteString(".RESET_SESSION");
                idlCon.ExecuteString(@".compile '" + m_profilename + "'");
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 传入调用IDL命令的参数
        /// 参数顺序必须正确
        /// </summary>
        /// <param name="param"></param>
        public void setParameters(string[] param)
        {
            if (param.Length == 0) return;

            m_runStr = m_functionName + ", ";
            for (int i = 0; i < param.Length - 1; i++)
            {
                string s = param[i];
                m_runStr += "'" + s + "',";
            }
            m_runStr += "'" + param[param.Length - 1] + "'";
        }

        /// <summary>
        /// 执行IDL算法
        /// </summary>
        public void run()
        {
            if (m_runStr == null) return;
            try { idlCon.ExecuteString(m_runStr); }
            catch (Exception ex) { throw ex; }
        }

        ~IdlConnector()
        {
            idlCon.DestroyObject();
        }
    }
}
