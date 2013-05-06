using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlUnit.Xunit;

namespace Mediane.Tests.Utils
{
    public static class XmlAssert
    {
        public static void IsEqual(string expect, string actual)
        {
            XmlAssertion.AssertXmlEquals(
                new XmlDiff(
                    new XmlInput("<testroot>" + expect + "</testroot>"),
                    new XmlInput("<testroot>" + actual + "</testroot>"),
                    new DiffConfiguration("XmlUnit", System.Xml.WhitespaceHandling.None)
                    ));
        }
    }
}
