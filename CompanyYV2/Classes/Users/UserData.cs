using System;
using System.Collections.Generic;
using CompanyYV2.Classes.Jobs;

namespace CompanyYV2.Classes.Users
{
	public class UserData
	{
		private string _name;
		private string _lastname;

        private int _id;
        private int _yearofbirth;
		
        private int _rank;

        public List<JobsData> _jobs;

        public UserData()
        {
			 _jobs = new List<JobsData>();
        }

        public List<JobsData> MyJobs
        {
            get { return _jobs; }
            set { _jobs = value; }
        }

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public string Lastname
		{
			get { return _lastname; }
			set { _lastname = value; }
		}

		public int YearofBirth
		{
			get { return _yearofbirth; }
			set { _yearofbirth = value; }
		}

		public int Rank
		{
            get { return _rank; }
			set { _rank = value; }
		}

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
	}
}
