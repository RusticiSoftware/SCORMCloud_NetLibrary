using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RusticiSoftware.HostedEngine.Client
{
  public class Rsp
  {
    public Rsp(XmlElement rspElement)
    {
      this.Status = rspElement.GetAttribute("stat");
      this.IsSuccess = rspElement.GetElementsByTagName("success").Count > 0;
    }
    private bool _isSuccess;
    private string _status;
    public string Status
    {
      get { return _status; }
      private set { _status = value; }
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
      private set
      {
        _isSuccess = value;
      }
    }
  }
}
