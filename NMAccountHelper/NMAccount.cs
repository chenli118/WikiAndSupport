using System;
using System.Collections.Generic;
using System.Text;

namespace NMAccountHelper
{
    public static class NMAccount
    {
        public static bool Login(string conStr, string userName, string password)
        {
            if (String.IsNullOrWhiteSpace(conStr) || String.IsNullOrWhiteSpace(conStr)) return false;
            BaseDataAccess bda = new BaseDataAccess(conStr);
            string sql = "select SSUSER_PWD from  neweasv5..V_NM_UserInfo where SSUSER_CODE='" + userName + "'";
            try
            {
                var val = bda.ExecScalar(sql);
                if (val == DBNull.Value || val == null)
                {
                    return false;
                }
                string pwd = NMAccountHelper.DESEncryptProvider.Create().NMEncrypt(password);
                if (pwd == val.ToString())
                    return true;
                return false;

            }
            catch
            {
                return false;
            }

        }
    }
}
