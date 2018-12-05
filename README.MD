<h3>What will you get with this module?</h3>

this module will make XAF search case insensitive no matter with collation, case or accent sensitivity you have setup in your database
 

<p align="center">
  <img width="460" height="300" src="https://www.jocheojeda.com/wp-content/uploads/2018/11/Search-Case-Insensitive.gif">
</p>

 
<h3>Why sometimes search functionaly in XAF is  case and accent sensitive?</h3>

Depending on your dabase server and how the server was setup you search might be case and accent sensitive, in most cases
after the creation of the database it is impossible to modify these settings.

 
<h3>Module requirements</h3>

this module is require official XAF Nuget packages, you can read more about this here https://community.devexpress.com/blogs/eaf/archive/2018/10/29/xaf-nuget-packages-v18-2.aspx

<h3>How to setup this module</h3>

1) Download the NuGet BIT.XAF.FilterCaseInsensitive.Module
2) Include BIT.XAF.FilterCaseInsensitive.Module on your required modules using the module designer or by adding this line to your Module.designer.cs or Module.designer.vb
on the InitializeComponent method 

this.RequiredModuleTypes.Add(typeof(BIT.XAF.FilterCaseInsensitive.Module.FilterCaseInsensitiveModule))
 
<h3>Would you like to add new features to this module?</h3>

I'm glad to help just write me at joche.ojeda@bitframeworks.com
 

<h3>Database server documentation</h3>

Most of database servers provide options to configure the case and accent sensitivity globally or for specific tables.
Refer to the documentation of your SQL server for additional details.
 
PostgreSQL
Collation Support https://www.postgresql.org/docs/9.1/static/collation.html
 
Oracle
Linguistic Sorting and Matching http://docs.oracle.com/database/121/NLSPG/ch5lingsort.htm#NLSPG005
 
MS SQL Server
SQL Server Collation Fundamentals https://technet.microsoft.com/en-us/library/aa174903%28v=sql.80%29.aspx
Specifying Collations https://technet.microsoft.com/en-us/library/aa214328%28v=sql.80%29.aspx
 
 
MySQL
Character Sets and Collations in General http://dev.mysql.com/doc/refman/5.7/en/charset-general.html
Server Character Set and Collation http://dev.mysql.com/doc/refman/5.7/en/charset-server.html

