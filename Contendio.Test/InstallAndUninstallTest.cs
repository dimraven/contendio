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
    public class InstallAndUninstallTest
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
        public void TestRepositoryInstallation()
        {
            IContentRepository contentRepository = ContentRepository;

            var rootNode = contentRepository.RootNode;
            Assert.IsNotNull(rootNode);
            Assert.AreEqual("/", rootNode.Path);
            Assert.IsNotNull(rootNode.NodeType);
            Assert.AreEqual("node:root", rootNode.NodeType.Name);
        }
    }
}