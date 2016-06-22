﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cindalnet.SQLBot.Database
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="BazaRelacyjna")]
	public partial class BazaRelacyjnaDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertSQLBot_FieldType(SQLBot_FieldType instance);
    partial void UpdateSQLBot_FieldType(SQLBot_FieldType instance);
    partial void DeleteSQLBot_FieldType(SQLBot_FieldType instance);
    partial void InsertSQLBot_Table(SQLBot_Table instance);
    partial void UpdateSQLBot_Table(SQLBot_Table instance);
    partial void DeleteSQLBot_Table(SQLBot_Table instance);
    partial void InsertSQLBot_TableJoin(SQLBot_TableJoin instance);
    partial void UpdateSQLBot_TableJoin(SQLBot_TableJoin instance);
    partial void DeleteSQLBot_TableJoin(SQLBot_TableJoin instance);
    partial void InsertSQLBot_Field(SQLBot_Field instance);
    partial void UpdateSQLBot_Field(SQLBot_Field instance);
    partial void DeleteSQLBot_Field(SQLBot_Field instance);
    partial void InsertSQLBot_TableDefault(SQLBot_TableDefault instance);
    partial void UpdateSQLBot_TableDefault(SQLBot_TableDefault instance);
    partial void DeleteSQLBot_TableDefault(SQLBot_TableDefault instance);
    #endregion
		
		public BazaRelacyjnaDataContext() : 
				base(global::Cindalnet.SQLBot.Properties.Settings.Default.BazaRelacyjnaConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public BazaRelacyjnaDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public BazaRelacyjnaDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public BazaRelacyjnaDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public BazaRelacyjnaDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<SQLBot_FieldType> SQLBot_FieldType
		{
			get
			{
				return this.GetTable<SQLBot_FieldType>();
			}
		}
		
		public System.Data.Linq.Table<SQLBot_Table> SQLBot_Table
		{
			get
			{
				return this.GetTable<SQLBot_Table>();
			}
		}
		
		public System.Data.Linq.Table<SQLBot_TableJoin> SQLBot_TableJoin
		{
			get
			{
				return this.GetTable<SQLBot_TableJoin>();
			}
		}
		
		public System.Data.Linq.Table<vSQLBotTableJoins> vSQLBotTableJoins
		{
			get
			{
				return this.GetTable<vSQLBotTableJoins>();
			}
		}
		
		public System.Data.Linq.Table<SQLBot_Field> SQLBot_Field
		{
			get
			{
				return this.GetTable<SQLBot_Field>();
			}
		}
		
		public System.Data.Linq.Table<SQLBot_TableDefault> SQLBot_TableDefault
		{
			get
			{
				return this.GetTable<SQLBot_TableDefault>();
			}
		}
		
		public System.Data.Linq.Table<vSQLBotFields> vSQLBotFields
		{
			get
			{
				return this.GetTable<vSQLBotFields>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.SQLBot_FieldType")]
	public partial class SQLBot_FieldType : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _sqlft_ID;
		
		private string _sqlft_Name;
		
		private EntitySet<SQLBot_Field> _SQLBot_Field;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void Onsqlft_IDChanging(int value);
    partial void Onsqlft_IDChanged();
    partial void Onsqlft_NameChanging(string value);
    partial void Onsqlft_NameChanged();
    #endregion
		
		public SQLBot_FieldType()
		{
			this._SQLBot_Field = new EntitySet<SQLBot_Field>(new Action<SQLBot_Field>(this.attach_SQLBot_Field), new Action<SQLBot_Field>(this.detach_SQLBot_Field));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sqlft_ID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int sqlft_ID
		{
			get
			{
				return this._sqlft_ID;
			}
			set
			{
				if ((this._sqlft_ID != value))
				{
					this.Onsqlft_IDChanging(value);
					this.SendPropertyChanging();
					this._sqlft_ID = value;
					this.SendPropertyChanged("sqlft_ID");
					this.Onsqlft_IDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sqlft_Name", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string sqlft_Name
		{
			get
			{
				return this._sqlft_Name;
			}
			set
			{
				if ((this._sqlft_Name != value))
				{
					this.Onsqlft_NameChanging(value);
					this.SendPropertyChanging();
					this._sqlft_Name = value;
					this.SendPropertyChanged("sqlft_Name");
					this.Onsqlft_NameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="SQLBot_FieldType_SQLBot_Field", Storage="_SQLBot_Field", ThisKey="sqlft_ID", OtherKey="sqlf_Type")]
		public EntitySet<SQLBot_Field> SQLBot_Field
		{
			get
			{
				return this._SQLBot_Field;
			}
			set
			{
				this._SQLBot_Field.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_SQLBot_Field(SQLBot_Field entity)
		{
			this.SendPropertyChanging();
			entity.SQLBot_FieldType = this;
		}
		
		private void detach_SQLBot_Field(SQLBot_Field entity)
		{
			this.SendPropertyChanging();
			entity.SQLBot_FieldType = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.SQLBot_Table")]
	public partial class SQLBot_Table : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _sqlt_ID;
		
		private string _sqlt_SQLName;
		
		private string _sqlt_Name;
		
		private string _sqlt_Description;
		
		private EntitySet<SQLBot_TableJoin> _SQLBot_TableJoin;
		
		private EntitySet<SQLBot_TableJoin> _SQLBot_TableJoin1;
		
		private EntitySet<SQLBot_Field> _SQLBot_Field;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void Onsqlt_IDChanging(int value);
    partial void Onsqlt_IDChanged();
    partial void Onsqlt_SQLNameChanging(string value);
    partial void Onsqlt_SQLNameChanged();
    partial void Onsqlt_NameChanging(string value);
    partial void Onsqlt_NameChanged();
    partial void Onsqlt_DescriptionChanging(string value);
    partial void Onsqlt_DescriptionChanged();
    #endregion
		
		public SQLBot_Table()
		{
			this._SQLBot_TableJoin = new EntitySet<SQLBot_TableJoin>(new Action<SQLBot_TableJoin>(this.attach_SQLBot_TableJoin), new Action<SQLBot_TableJoin>(this.detach_SQLBot_TableJoin));
			this._SQLBot_TableJoin1 = new EntitySet<SQLBot_TableJoin>(new Action<SQLBot_TableJoin>(this.attach_SQLBot_TableJoin1), new Action<SQLBot_TableJoin>(this.detach_SQLBot_TableJoin1));
			this._SQLBot_Field = new EntitySet<SQLBot_Field>(new Action<SQLBot_Field>(this.attach_SQLBot_Field), new Action<SQLBot_Field>(this.detach_SQLBot_Field));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sqlt_ID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int sqlt_ID
		{
			get
			{
				return this._sqlt_ID;
			}
			set
			{
				if ((this._sqlt_ID != value))
				{
					this.Onsqlt_IDChanging(value);
					this.SendPropertyChanging();
					this._sqlt_ID = value;
					this.SendPropertyChanged("sqlt_ID");
					this.Onsqlt_IDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sqlt_SQLName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string sqlt_SQLName
		{
			get
			{
				return this._sqlt_SQLName;
			}
			set
			{
				if ((this._sqlt_SQLName != value))
				{
					this.Onsqlt_SQLNameChanging(value);
					this.SendPropertyChanging();
					this._sqlt_SQLName = value;
					this.SendPropertyChanged("sqlt_SQLName");
					this.Onsqlt_SQLNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sqlt_Name", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string sqlt_Name
		{
			get
			{
				return this._sqlt_Name;
			}
			set
			{
				if ((this._sqlt_Name != value))
				{
					this.Onsqlt_NameChanging(value);
					this.SendPropertyChanging();
					this._sqlt_Name = value;
					this.SendPropertyChanged("sqlt_Name");
					this.Onsqlt_NameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sqlt_Description", DbType="VarChar(200)")]
		public string sqlt_Description
		{
			get
			{
				return this._sqlt_Description;
			}
			set
			{
				if ((this._sqlt_Description != value))
				{
					this.Onsqlt_DescriptionChanging(value);
					this.SendPropertyChanging();
					this._sqlt_Description = value;
					this.SendPropertyChanged("sqlt_Description");
					this.Onsqlt_DescriptionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="SQLBot_Table_SQLBot_TableJoin", Storage="_SQLBot_TableJoin", ThisKey="sqlt_ID", OtherKey="sqltj_Table1")]
		public EntitySet<SQLBot_TableJoin> SQLBot_TableJoin
		{
			get
			{
				return this._SQLBot_TableJoin;
			}
			set
			{
				this._SQLBot_TableJoin.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="SQLBot_Table_SQLBot_TableJoin1", Storage="_SQLBot_TableJoin1", ThisKey="sqlt_ID", OtherKey="sqltj_Table2")]
		public EntitySet<SQLBot_TableJoin> SQLBot_TableJoin1
		{
			get
			{
				return this._SQLBot_TableJoin1;
			}
			set
			{
				this._SQLBot_TableJoin1.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="SQLBot_Table_SQLBot_Field", Storage="_SQLBot_Field", ThisKey="sqlt_ID", OtherKey="sqlf_Table")]
		public EntitySet<SQLBot_Field> SQLBot_Field
		{
			get
			{
				return this._SQLBot_Field;
			}
			set
			{
				this._SQLBot_Field.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_SQLBot_TableJoin(SQLBot_TableJoin entity)
		{
			this.SendPropertyChanging();
			entity.SQLBot_Table = this;
		}
		
		private void detach_SQLBot_TableJoin(SQLBot_TableJoin entity)
		{
			this.SendPropertyChanging();
			entity.SQLBot_Table = null;
		}
		
		private void attach_SQLBot_TableJoin1(SQLBot_TableJoin entity)
		{
			this.SendPropertyChanging();
			entity.SQLBot_Table1 = this;
		}
		
		private void detach_SQLBot_TableJoin1(SQLBot_TableJoin entity)
		{
			this.SendPropertyChanging();
			entity.SQLBot_Table1 = null;
		}
		
		private void attach_SQLBot_Field(SQLBot_Field entity)
		{
			this.SendPropertyChanging();
			entity.SQLBot_Table = this;
		}
		
		private void detach_SQLBot_Field(SQLBot_Field entity)
		{
			this.SendPropertyChanging();
			entity.SQLBot_Table = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.SQLBot_TableJoin")]
	public partial class SQLBot_TableJoin : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _sqltj_ID;
		
		private int _sqltj_Table1;
		
		private int _sqltj_Table2;
		
		private string _sqltj_Join;
		
		private EntityRef<SQLBot_Table> _SQLBot_Table;
		
		private EntityRef<SQLBot_Table> _SQLBot_Table1;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void Onsqltj_IDChanging(int value);
    partial void Onsqltj_IDChanged();
    partial void Onsqltj_Table1Changing(int value);
    partial void Onsqltj_Table1Changed();
    partial void Onsqltj_Table2Changing(int value);
    partial void Onsqltj_Table2Changed();
    partial void Onsqltj_JoinChanging(string value);
    partial void Onsqltj_JoinChanged();
    #endregion
		
		public SQLBot_TableJoin()
		{
			this._SQLBot_Table = default(EntityRef<SQLBot_Table>);
			this._SQLBot_Table1 = default(EntityRef<SQLBot_Table>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sqltj_ID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int sqltj_ID
		{
			get
			{
				return this._sqltj_ID;
			}
			set
			{
				if ((this._sqltj_ID != value))
				{
					this.Onsqltj_IDChanging(value);
					this.SendPropertyChanging();
					this._sqltj_ID = value;
					this.SendPropertyChanged("sqltj_ID");
					this.Onsqltj_IDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sqltj_Table1", DbType="Int NOT NULL")]
		public int sqltj_Table1
		{
			get
			{
				return this._sqltj_Table1;
			}
			set
			{
				if ((this._sqltj_Table1 != value))
				{
					if (this._SQLBot_Table.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.Onsqltj_Table1Changing(value);
					this.SendPropertyChanging();
					this._sqltj_Table1 = value;
					this.SendPropertyChanged("sqltj_Table1");
					this.Onsqltj_Table1Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sqltj_Table2", DbType="Int NOT NULL")]
		public int sqltj_Table2
		{
			get
			{
				return this._sqltj_Table2;
			}
			set
			{
				if ((this._sqltj_Table2 != value))
				{
					if (this._SQLBot_Table1.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.Onsqltj_Table2Changing(value);
					this.SendPropertyChanging();
					this._sqltj_Table2 = value;
					this.SendPropertyChanged("sqltj_Table2");
					this.Onsqltj_Table2Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sqltj_Join", DbType="VarChar(100) NOT NULL", CanBeNull=false)]
		public string sqltj_Join
		{
			get
			{
				return this._sqltj_Join;
			}
			set
			{
				if ((this._sqltj_Join != value))
				{
					this.Onsqltj_JoinChanging(value);
					this.SendPropertyChanging();
					this._sqltj_Join = value;
					this.SendPropertyChanged("sqltj_Join");
					this.Onsqltj_JoinChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="SQLBot_Table_SQLBot_TableJoin", Storage="_SQLBot_Table", ThisKey="sqltj_Table1", OtherKey="sqlt_ID", IsForeignKey=true)]
		public SQLBot_Table SQLBot_Table
		{
			get
			{
				return this._SQLBot_Table.Entity;
			}
			set
			{
				SQLBot_Table previousValue = this._SQLBot_Table.Entity;
				if (((previousValue != value) 
							|| (this._SQLBot_Table.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._SQLBot_Table.Entity = null;
						previousValue.SQLBot_TableJoin.Remove(this);
					}
					this._SQLBot_Table.Entity = value;
					if ((value != null))
					{
						value.SQLBot_TableJoin.Add(this);
						this._sqltj_Table1 = value.sqlt_ID;
					}
					else
					{
						this._sqltj_Table1 = default(int);
					}
					this.SendPropertyChanged("SQLBot_Table");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="SQLBot_Table_SQLBot_TableJoin1", Storage="_SQLBot_Table1", ThisKey="sqltj_Table2", OtherKey="sqlt_ID", IsForeignKey=true)]
		public SQLBot_Table SQLBot_Table1
		{
			get
			{
				return this._SQLBot_Table1.Entity;
			}
			set
			{
				SQLBot_Table previousValue = this._SQLBot_Table1.Entity;
				if (((previousValue != value) 
							|| (this._SQLBot_Table1.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._SQLBot_Table1.Entity = null;
						previousValue.SQLBot_TableJoin1.Remove(this);
					}
					this._SQLBot_Table1.Entity = value;
					if ((value != null))
					{
						value.SQLBot_TableJoin1.Add(this);
						this._sqltj_Table2 = value.sqlt_ID;
					}
					else
					{
						this._sqltj_Table2 = default(int);
					}
					this.SendPropertyChanged("SQLBot_Table1");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.vSQLBotTableJoins")]
	public partial class vSQLBotTableJoins
	{
		
		private string _Table1NameSQL;
		
		private string _Table1Name;
		
		private string _Table2NameSQL;
		
		private string _Table2Name;
		
		private string _TableJoin;
		
		public vSQLBotTableJoins()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Table1NameSQL", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Table1NameSQL
		{
			get
			{
				return this._Table1NameSQL;
			}
			set
			{
				if ((this._Table1NameSQL != value))
				{
					this._Table1NameSQL = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Table1Name", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Table1Name
		{
			get
			{
				return this._Table1Name;
			}
			set
			{
				if ((this._Table1Name != value))
				{
					this._Table1Name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Table2NameSQL", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Table2NameSQL
		{
			get
			{
				return this._Table2NameSQL;
			}
			set
			{
				if ((this._Table2NameSQL != value))
				{
					this._Table2NameSQL = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Table2Name", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Table2Name
		{
			get
			{
				return this._Table2Name;
			}
			set
			{
				if ((this._Table2Name != value))
				{
					this._Table2Name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TableJoin", DbType="VarChar(100) NOT NULL", CanBeNull=false)]
		public string TableJoin
		{
			get
			{
				return this._TableJoin;
			}
			set
			{
				if ((this._TableJoin != value))
				{
					this._TableJoin = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.SQLBot_Field")]
	public partial class SQLBot_Field : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _sqlf_ID;
		
		private int _sqlf_Table;
		
		private string _sqlf_SQLColumnName;
		
		private string _sqlf_ColumnName;
		
		private int _sqlf_Type;
		
		private string _sqlf_Description;
		
		private EntitySet<SQLBot_TableDefault> _SQLBot_TableDefault;
		
		private EntityRef<SQLBot_FieldType> _SQLBot_FieldType;
		
		private EntityRef<SQLBot_Table> _SQLBot_Table;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void Onsqlf_IDChanging(int value);
    partial void Onsqlf_IDChanged();
    partial void Onsqlf_TableChanging(int value);
    partial void Onsqlf_TableChanged();
    partial void Onsqlf_SQLColumnNameChanging(string value);
    partial void Onsqlf_SQLColumnNameChanged();
    partial void Onsqlf_ColumnNameChanging(string value);
    partial void Onsqlf_ColumnNameChanged();
    partial void Onsqlf_TypeChanging(int value);
    partial void Onsqlf_TypeChanged();
    partial void Onsqlf_DescriptionChanging(string value);
    partial void Onsqlf_DescriptionChanged();
    #endregion
		
		public SQLBot_Field()
		{
			this._SQLBot_TableDefault = new EntitySet<SQLBot_TableDefault>(new Action<SQLBot_TableDefault>(this.attach_SQLBot_TableDefault), new Action<SQLBot_TableDefault>(this.detach_SQLBot_TableDefault));
			this._SQLBot_FieldType = default(EntityRef<SQLBot_FieldType>);
			this._SQLBot_Table = default(EntityRef<SQLBot_Table>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sqlf_ID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int sqlf_ID
		{
			get
			{
				return this._sqlf_ID;
			}
			set
			{
				if ((this._sqlf_ID != value))
				{
					this.Onsqlf_IDChanging(value);
					this.SendPropertyChanging();
					this._sqlf_ID = value;
					this.SendPropertyChanged("sqlf_ID");
					this.Onsqlf_IDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sqlf_Table", DbType="Int NOT NULL")]
		public int sqlf_Table
		{
			get
			{
				return this._sqlf_Table;
			}
			set
			{
				if ((this._sqlf_Table != value))
				{
					if (this._SQLBot_Table.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.Onsqlf_TableChanging(value);
					this.SendPropertyChanging();
					this._sqlf_Table = value;
					this.SendPropertyChanged("sqlf_Table");
					this.Onsqlf_TableChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sqlf_SQLColumnName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string sqlf_SQLColumnName
		{
			get
			{
				return this._sqlf_SQLColumnName;
			}
			set
			{
				if ((this._sqlf_SQLColumnName != value))
				{
					this.Onsqlf_SQLColumnNameChanging(value);
					this.SendPropertyChanging();
					this._sqlf_SQLColumnName = value;
					this.SendPropertyChanged("sqlf_SQLColumnName");
					this.Onsqlf_SQLColumnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sqlf_ColumnName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string sqlf_ColumnName
		{
			get
			{
				return this._sqlf_ColumnName;
			}
			set
			{
				if ((this._sqlf_ColumnName != value))
				{
					this.Onsqlf_ColumnNameChanging(value);
					this.SendPropertyChanging();
					this._sqlf_ColumnName = value;
					this.SendPropertyChanged("sqlf_ColumnName");
					this.Onsqlf_ColumnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sqlf_Type", DbType="Int NOT NULL")]
		public int sqlf_Type
		{
			get
			{
				return this._sqlf_Type;
			}
			set
			{
				if ((this._sqlf_Type != value))
				{
					if (this._SQLBot_FieldType.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.Onsqlf_TypeChanging(value);
					this.SendPropertyChanging();
					this._sqlf_Type = value;
					this.SendPropertyChanged("sqlf_Type");
					this.Onsqlf_TypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sqlf_Description", DbType="VarChar(200)")]
		public string sqlf_Description
		{
			get
			{
				return this._sqlf_Description;
			}
			set
			{
				if ((this._sqlf_Description != value))
				{
					this.Onsqlf_DescriptionChanging(value);
					this.SendPropertyChanging();
					this._sqlf_Description = value;
					this.SendPropertyChanged("sqlf_Description");
					this.Onsqlf_DescriptionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="SQLBot_Field_SQLBot_TableDefault", Storage="_SQLBot_TableDefault", ThisKey="sqlf_ID", OtherKey="sqld_SQLColumn")]
		public EntitySet<SQLBot_TableDefault> SQLBot_TableDefault
		{
			get
			{
				return this._SQLBot_TableDefault;
			}
			set
			{
				this._SQLBot_TableDefault.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="SQLBot_FieldType_SQLBot_Field", Storage="_SQLBot_FieldType", ThisKey="sqlf_Type", OtherKey="sqlft_ID", IsForeignKey=true)]
		public SQLBot_FieldType SQLBot_FieldType
		{
			get
			{
				return this._SQLBot_FieldType.Entity;
			}
			set
			{
				SQLBot_FieldType previousValue = this._SQLBot_FieldType.Entity;
				if (((previousValue != value) 
							|| (this._SQLBot_FieldType.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._SQLBot_FieldType.Entity = null;
						previousValue.SQLBot_Field.Remove(this);
					}
					this._SQLBot_FieldType.Entity = value;
					if ((value != null))
					{
						value.SQLBot_Field.Add(this);
						this._sqlf_Type = value.sqlft_ID;
					}
					else
					{
						this._sqlf_Type = default(int);
					}
					this.SendPropertyChanged("SQLBot_FieldType");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="SQLBot_Table_SQLBot_Field", Storage="_SQLBot_Table", ThisKey="sqlf_Table", OtherKey="sqlt_ID", IsForeignKey=true)]
		public SQLBot_Table SQLBot_Table
		{
			get
			{
				return this._SQLBot_Table.Entity;
			}
			set
			{
				SQLBot_Table previousValue = this._SQLBot_Table.Entity;
				if (((previousValue != value) 
							|| (this._SQLBot_Table.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._SQLBot_Table.Entity = null;
						previousValue.SQLBot_Field.Remove(this);
					}
					this._SQLBot_Table.Entity = value;
					if ((value != null))
					{
						value.SQLBot_Field.Add(this);
						this._sqlf_Table = value.sqlt_ID;
					}
					else
					{
						this._sqlf_Table = default(int);
					}
					this.SendPropertyChanged("SQLBot_Table");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_SQLBot_TableDefault(SQLBot_TableDefault entity)
		{
			this.SendPropertyChanging();
			entity.SQLBot_Field = this;
		}
		
		private void detach_SQLBot_TableDefault(SQLBot_TableDefault entity)
		{
			this.SendPropertyChanging();
			entity.SQLBot_Field = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.SQLBot_TableDefault")]
	public partial class SQLBot_TableDefault : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _sqld_ID;
		
		private int _sqld_SQLColumn;
		
		private EntityRef<SQLBot_Field> _SQLBot_Field;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void Onsqld_IDChanging(int value);
    partial void Onsqld_IDChanged();
    partial void Onsqld_SQLColumnChanging(int value);
    partial void Onsqld_SQLColumnChanged();
    #endregion
		
		public SQLBot_TableDefault()
		{
			this._SQLBot_Field = default(EntityRef<SQLBot_Field>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sqld_ID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int sqld_ID
		{
			get
			{
				return this._sqld_ID;
			}
			set
			{
				if ((this._sqld_ID != value))
				{
					this.Onsqld_IDChanging(value);
					this.SendPropertyChanging();
					this._sqld_ID = value;
					this.SendPropertyChanged("sqld_ID");
					this.Onsqld_IDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sqld_SQLColumn", DbType="Int NOT NULL")]
		public int sqld_SQLColumn
		{
			get
			{
				return this._sqld_SQLColumn;
			}
			set
			{
				if ((this._sqld_SQLColumn != value))
				{
					if (this._SQLBot_Field.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.Onsqld_SQLColumnChanging(value);
					this.SendPropertyChanging();
					this._sqld_SQLColumn = value;
					this.SendPropertyChanged("sqld_SQLColumn");
					this.Onsqld_SQLColumnChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="SQLBot_Field_SQLBot_TableDefault", Storage="_SQLBot_Field", ThisKey="sqld_SQLColumn", OtherKey="sqlf_ID", IsForeignKey=true)]
		public SQLBot_Field SQLBot_Field
		{
			get
			{
				return this._SQLBot_Field.Entity;
			}
			set
			{
				SQLBot_Field previousValue = this._SQLBot_Field.Entity;
				if (((previousValue != value) 
							|| (this._SQLBot_Field.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._SQLBot_Field.Entity = null;
						previousValue.SQLBot_TableDefault.Remove(this);
					}
					this._SQLBot_Field.Entity = value;
					if ((value != null))
					{
						value.SQLBot_TableDefault.Add(this);
						this._sqld_SQLColumn = value.sqlf_ID;
					}
					else
					{
						this._sqld_SQLColumn = default(int);
					}
					this.SendPropertyChanged("SQLBot_Field");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.vSQLBotFields")]
	public partial class vSQLBotFields
	{
		
		private string _TableNameSQL;
		
		private string _TableName;
		
		private string _TableDescription;
		
		private string _FieldNameSQL;
		
		private string _FieldName;
		
		private string _FieldType;
		
		private string _FieldDescription;
		
		public vSQLBotFields()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TableNameSQL", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string TableNameSQL
		{
			get
			{
				return this._TableNameSQL;
			}
			set
			{
				if ((this._TableNameSQL != value))
				{
					this._TableNameSQL = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TableName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string TableName
		{
			get
			{
				return this._TableName;
			}
			set
			{
				if ((this._TableName != value))
				{
					this._TableName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TableDescription", DbType="VarChar(200)")]
		public string TableDescription
		{
			get
			{
				return this._TableDescription;
			}
			set
			{
				if ((this._TableDescription != value))
				{
					this._TableDescription = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FieldNameSQL", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string FieldNameSQL
		{
			get
			{
				return this._FieldNameSQL;
			}
			set
			{
				if ((this._FieldNameSQL != value))
				{
					this._FieldNameSQL = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FieldName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string FieldName
		{
			get
			{
				return this._FieldName;
			}
			set
			{
				if ((this._FieldName != value))
				{
					this._FieldName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FieldType", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string FieldType
		{
			get
			{
				return this._FieldType;
			}
			set
			{
				if ((this._FieldType != value))
				{
					this._FieldType = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FieldDescription", DbType="VarChar(200)")]
		public string FieldDescription
		{
			get
			{
				return this._FieldDescription;
			}
			set
			{
				if ((this._FieldDescription != value))
				{
					this._FieldDescription = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
