using Godot;
using LiteDB;

public partial class Main : ColorRect
{
	private static readonly string DbDir = "user://Data";
	private static readonly string DbFile = $"{DbDir}/test.db";

	private string _dbPath;
	private LineEdit _nameLine;
	private LineEdit _noteLine;

	public class Person
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Note { get; set; }
	}

	public override void _Ready()
	{
		_nameLine = GetNode<LineEdit>("HBoxContainer/NameLineEdit");
		_noteLine = GetNode<LineEdit>("HBoxContainer/NoteLineEdit");

		// create the data directory if it doesn't exist
		if (!DirAccess.DirExistsAbsolute(DbDir))
		{
			DirAccess.MakeDirAbsolute(DbDir);
		}

		// check if the LiteDB file exists
		bool createTable = false;
		if (!FileAccess.FileExists(DbFile))
		{
			// create an empty file for LiteDB
			var file = FileAccess.Open(DbFile, FileAccess.ModeFlags.Write);
			file.Close();
			// toggle flag to run minimal DB set up
			createTable = true;
		}

		// get the absolute file path for LiteDB to use
		// occurs after file exists confirmed, otherwise FileAccess throws a null reference exception
		_dbPath = FileAccess.Open(DbFile, FileAccess.ModeFlags.Read).GetPathAbsolute();

		if (createTable)
		{
			PopulateTable();
		}
	}

	public void OnGetButtonPress()
	{
		var person = GetPersonById(1);

		_nameLine.Text = person.Name;
		_noteLine.Text = person.Note;
	}

	public void OnUpdateButtonPress()
	{
		var person = GetPersonById(1);

		person.Name = _nameLine.Text;
		person.Note = _noteLine.Text;

		UpdatePerson(person);
	}

	public void OnInsertButtonPress()
	{
		if (_nameLine.Text != "" && _noteLine.Text != "")
		{
			InsertPerson(_nameLine.Text, _noteLine.Text);
		}
	}

	// UI is constrained to showing only row id 1
	// pull all rows and write out the data to the debug console
	// to confirm `InsertPerson()` is working appropriately
	public void OnQueryButtonPress()
	{
		using var cn = new LiteDatabase(_dbPath);
		var col = cn.GetCollection<Person>("person");

		foreach (Person row in col.FindAll())
		{
			GD.Print($"{row.Id}: {row.Name} - {row.Note}");
		}
	}

	public void OnClearButtonPress()
	{
		_nameLine.Clear();
		_noteLine.Clear();
	}


	private void PopulateTable()
	{
		InsertPerson("Horse Renoir", "Bounty Hunter");
	}

	private void InsertPerson(string name, string note)
	{
		// connect to file
		using var cn = new LiteDatabase(_dbPath);
		// get collection of Person from person table
		// automatically creates, if table doesn't exist
		var col = cn.GetCollection<Person>("person");

		// define a new person
		var person = new Person
		{
			Name = name,
			Note = note,
		};

		// insert new person into collection
		col.Insert(person);
	}

	private void UpdatePerson(Person person)
	{
		using var cn = new LiteDatabase(_dbPath);
		var col = cn.GetCollection<Person>("person");

		col.Update(person);
	}

	private Person GetPersonById(int id)
	{
		using var cn = new LiteDatabase(_dbPath);
		var col = cn.GetCollection<Person>("person");

		return col.FindOne(x => x.Id == id);
	}
}