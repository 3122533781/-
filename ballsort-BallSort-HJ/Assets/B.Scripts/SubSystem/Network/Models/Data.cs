using System;

namespace Models
{
    /**
     * 加密数据类
     */
    [Serializable]
    public class Data
    {
		public bool success;
        public string data;
    }
    
    [Serializable]
    public class ErrorData
    {
        public string error_code;
        public string error;
    }
}

