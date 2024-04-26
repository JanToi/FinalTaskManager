interface Task {
  Id: number;
  Name: string;
  Description: string;
  StartDate: Date;
  EndDate: Date;
  StatusName: string;
  StatusTheme: string;
  ActivityTypeName: string;
}

export default Task;
