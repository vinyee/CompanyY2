using System;
namespace CompanyYV2.Classes.Jobs
{
    public class JobsData
    {
        private int _id;
        private string _title;
        private int _stage;
        private int _points;
        private int _time;
        private int _userid;
        private int _vip;

        public int Points
        {
            get { return _points; }
            set { _points = value; }
        }

		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public int UserId
		{
            get { return _userid; }
            set { _userid = value; }
		}

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public int Stage
        {
            get { return _stage; }
            set { _stage = value; }
        }

        public int Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public int Vip
        {
            get { return _vip; }
            set { _vip = value; }
        }

    }
}
