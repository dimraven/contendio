# What is a content repository?

A content repository is an abstraction layer between your application source code and the database. It's purpose is to "simulate" that your
database is a document database.

A Content repository saves data in a structured manner and is often used as an application layer for 
both CMS- (Content management systems) and CRM systems.

**Why use a content repository?**

**What is contendio?**

Contendio is a content repository implementation for .NET framework. I've worked with Apache Jackrabbit before and felt that this type of
feature is missing in the .NET community.

The base class for all contendios functionality is the IContentRepository (or SqlContentRepository). When you create a content repository instance
you specify a workspace name. If the neccessary database tables are missing then Contendio creates them automatically for you. Note that the root
node is always added for you when the database is created.

The content repository also support a complete customizable node type system. You can see them as tags or keywords for nodes and node values.

```c#
var contentRepository = new SqlContentRepository("myworkspace", connectionString);
```

The above example creates the tables:
* myworkspace_Node
* myworkspace_NodeType
* myworkspace_NodeValue
* myworkspace_StringValue
* myworkspace_BinaryValue
* myworkspace_DateValue

When creating a content management system then the workspaces usually needed are:
* website
* config
* media

Each workspace is an isolated island. That means that you can't create hard-links between Nodes in different workspaces. You can however create
"soft" links. Example:

/ (type = "node:root")
/index (type = "customtypes:webpage")
/index/linkComponent (type = "customtypes:component")
       |- imageAlt = "alt text used on the web page"
	      workspace = "media"
		  id = 10

And in the code you do something like this:

```c#
webSiteRoot = contentRepositoryManager.GetContentRepository("website");
var indexPage = webSiteRoot.GetNode("index");
var component1 = indexPage.GetNode("linkComponent");

mediaContentRepo = contentRepositoryManager.GetContentRepository(component1.GetValue("workspace"));
var mediaItem = mediaContentRepo.GetNodeById(component1.GetValue("id"));
```

Contendio also supports LINQ. Example:

```c#
from node in RootNode where node.NodeType.Equals("customtypes:webpage") select node;
from node in RootNode where node.NodeType.Equals("customtypes:webpage") select new {
	Name = node.Name,
	Path = node.Path
};

from nodeValue in RootNode.Values where !nodeValue.NodeType.Equals("value:hidden") select nodeValue;
```

All node fetches are lazy. That means that no node data below the current node-level are retrieved from the database.

**Requirements**

**Future**
