using System;
using System.Collections;
using System.Xml;
using System.Configuration;
using System.Net;


namespace DIClassLib.Membership
{
    public class AutoLoginByIPSettings : IConfigurationSectionHandler
    {

        const string _targetLocationsSectionName = "diselocation";
        const string _autoLoginIPsSectionName = "iprange";

        private Hashtable m_htTargetLocations = new Hashtable();
        private bool m_bCreated = false;

        private Hashtable TargetLocations
        {
            get { return m_htTargetLocations; }
        }

        public object Create(object parent, object configContext, XmlNode section)
        {

            if (!m_bCreated)
            {
                ReadSectionSettings(section);
                m_bCreated = true;
            }

            return this;
        }

        public bool IsIPAddressAcceptable(string locationAbs, string ipUser)
        {
            bool bRes = false;
            Hashtable ht = null;
            string location = "";

            if (locationAbs.StartsWith("/"))
            {
                location = locationAbs.Substring(1);
            }
            else
            {
                location = locationAbs;
            }

            IPAddress userAddr = IPAddress.Parse(ipUser);
            string sLocLower = location.ToLower();
            foreach (string sTargetLocation in this.TargetLocations.Keys)
            {
                if (sLocLower.StartsWith(sTargetLocation.ToLower()))
                {
                    ht = (Hashtable)this.TargetLocations[sTargetLocation];
                    bRes = IsIPAddressInTrustedRanges(ht, userAddr);
                    if (bRes)
                    {
                        break;
                    }
                }
            }

            return bRes;
        }

        private bool IsIPAddressInTrustedRanges(Hashtable ht, IPAddress addr)
        {
            bool bRes = false;
            foreach (IPAddress[] aRange in ht.Values)
            {
                bRes = IsIPAddressInRange(addr, aRange);
                if (bRes)
                {
                    break;
                }
            }
            return bRes;
        }

        private void ReadSectionSettings(XmlNode section)
        {
            //XmlElement xeLocation = default(XmlElement);
            //XmlElement xeIPRange = default(XmlElement);

            XmlNodeList xnlLocations = section.SelectNodes(_targetLocationsSectionName);
            foreach (XmlElement xeLocation in xnlLocations)
            {
                string sPath = xeLocation.GetAttribute("path");
                if ((sPath != null))
                {
                    Hashtable htIPRanges = new Hashtable();
                    this.TargetLocations[sPath] = htIPRanges;

                    XmlNodeList xnlIPRanges = xeLocation.SelectNodes(_autoLoginIPsSectionName);
                    foreach (XmlElement xeIPRange in xnlIPRanges)
                    {
                        string sKey = xeIPRange.GetAttribute("key");
                        string sValue = xeIPRange.GetAttribute("value");
                        htIPRanges[sKey] = GetIPAddressRange(sValue);
                    }
                }
            }
        }

        private static IPAddress[] GetIPAddressRange(string sAddressRange)
        {
            IPAddress[] aRes = new IPAddress[2];

            string[] asAddresses = sAddressRange.Split(new char[] { '-' });

            if (asAddresses.Length > 0 & IPAddress.TryParse(asAddresses[0], out aRes[0]))
            {
                if (asAddresses.Length > 1)
                {
                    if (!IPAddress.TryParse(asAddresses[1], out aRes[1]))
                    {
                        aRes[1] = aRes[0];
                    }
                }
                else
                {
                    aRes[1] = aRes[0];
                }
            }
            else
            {
                aRes = null;
            }
            return aRes;
        }

        private static long ConvertIPAddressToLong(IPAddress addr)
        {
            byte[] aBytes = addr.GetAddressBytes();
            long lAddr = 0;
            foreach (byte bt in aBytes)
            {
                lAddr = lAddr * 256 + bt;
            }
            return lAddr;
        }

        private static bool IsIPAddressInRange(IPAddress addr, IPAddress[] aRange)
        {
            bool bRes = false;
            long lAddr = ConvertIPAddressToLong(addr);
            long lAddrMin = ConvertIPAddressToLong(aRange[0]);
            long lAddrMax = ConvertIPAddressToLong(aRange[1]);
            bRes = (lAddrMin <= lAddr & lAddr <= lAddrMax);
            return bRes;
        }

    }
}