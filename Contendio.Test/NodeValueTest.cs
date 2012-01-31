﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contendio.Sql;
using System.Data.Linq;
using System.Transactions;

namespace Contendio.Test
{
    [TestClass]
    public class NodeValueTest
    {
        private const string DatabaseSchema = "contendio";
        private const string ConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=" + DatabaseSchema + ";Integrated Security=True";

        private IRepositorySetup ContentInstall
        {
            get
            {
                return new SqlRepositorySetup("test", ConnectionString);
            }
        }

        private IContentRepository ContentRepository
        {
            get
            {
                SqlContentRepository contentRepository = new SqlContentRepository("test", ConnectionString);
                return contentRepository;
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            ContentInstall.Install();
        }

        [TestCleanup]
        public void Cleanup()
        {
            ContentInstall.Uninstall();
        }

        [TestMethod]
        public void TestStringValue()
        {
            IContentRepository contentRepository = ContentRepository;

            var rootNode = contentRepository.RootNode;
            Assert.IsNotNull(rootNode);

            rootNode.AddValue("mystringvalue", "hello world!!!");
            rootNode.AddValue("mystringvalue2", "goodbye world!!!");
            var values = rootNode.Values;

            Assert.AreEqual(2, values.Count);
            Assert.AreEqual("hello world!!!", values[0].ValueAsString());
            Assert.AreEqual("goodbye world!!!", values[1].ValueAsString());
        }

        [TestMethod]
        public void TestStringReplaceValue()
        {
            IContentRepository contentRepository = ContentRepository;

            var rootNode = contentRepository.RootNode;
            Assert.IsNotNull(rootNode);

            rootNode.AddValue("mystringvalue", "hello world!!!");
            rootNode.AddValue("mystringvalue2", "goodbye world!!!");
            rootNode.AddValue("mystringvalue", "replacedValue!!!");
            var values = rootNode.Values;

            Assert.AreEqual(2, values.Count);
            Assert.AreEqual("replacedValue!!!", values[0].ValueAsString());
            Assert.AreEqual("goodbye world!!!", values[1].ValueAsString());
        }
    }
}