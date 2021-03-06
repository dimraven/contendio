﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using Contendio.Model;
using System.Data.Linq.Mapping;
using Contendio.Sql.Mapping;
using Contendio.Sql.Provider;
using Contendio.Sql.Model;
using Contendio.Sql.Entity;

namespace Contendio.Sql
{
    public class SqlContentRepository : IContentRepository
    {
        public string Workspace { get; private set; }
        public SqlObserverManager ObserverManager { get { return _observerManager; } }
        public DataContext DataContext { get; private set; }
        public IQueryManager QueryManager { get { return _queryManager; } }
        
        #region Private members

        private readonly SqlObserverManager _observerManager;
        private readonly SqlQueryManager _queryManager;

        #endregion

        public INode RootNode
        {
            get
            {
                var node = from n in _queryManager.NodeQueryable where n.NodeId == null select n;
                return new SqlNode(node.FirstOrDefault(), this);
            }
        }

        public SqlContentRepository(string workspace, string connectionString)
        {            
            this.Workspace = workspace;
            this._observerManager = new SqlObserverManager();

            DataContext = new DataContext(connectionString, new RepositoryMappingSource(Workspace));
            this._queryManager = new SqlQueryManager(workspace, DataContext);
        }
    }
}
