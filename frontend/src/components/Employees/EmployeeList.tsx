import React, { useState } from "react";
import { Employee } from "../../types";
import "./EmployeeList.css";
import { DataGrid, GridCallbackDetails, GridRowSelectionModel } from '@mui/x-data-grid';  // יבוא של רכיב ה-Grid
import { Button } from "@mui/material";
import EmployeeForm from "./EmployeeForm";


interface EmployeeListProps {
  employees: Employee[];
  onSelect: (employee: Employee) => void;
  onDelete: (id: string) => void;
}

const EmployeeList = ({ employees, onSelect, onDelete } : EmployeeListProps) => {
  const [selectedEmployee, setSelectedEmployee] = useState<Employee | null>(null);

  const onSelected = (employee: Employee) =>
  {
    setSelectedEmployee(employee);
    onSelect(employee);
  }
  const onDeleted = (employee : Employee) =>
    {
      setSelectedEmployee(employee);
      onDelete(employee.id);
    }
// // דוגמת נתונים (למשל רשימת עובדים)
// const rows = [
//   { id: 1, name: 'John Doe', email: 'john@example.com' },
//   { id: 2, name: 'Jane Smith', email: 'jane@example.com' },
//   { id: 3, name: 'Bob Johnson', email: 'bob@example.com' },
// ];
    // העמודות שיכללו את כפתורי העריכה והמחיקה
const columns = [
  { field: 'id', headerName: 'ID', width: 150 },
  { field: 'name', headerName: 'Name', width: 200 },
  { field: 'email', headerName: 'Email', width: 250 },
  {
    field: 'actions',
    headerName: 'Actions',
    width: 250,
    renderCell: (params: any) => (
      <>
        <Button
          variant="outlined"
          color="primary"
          onClick={() => onSelected(params.row)}
        >
          Edit
        </Button>
        <Button
          variant="outlined"
          color="secondary"
          onClick={() => onDeleted(params.row)}
        >
          Delete
        </Button>
      </>
    ),
  },
];

const handleRowSelection = (rowSelectionModel: GridRowSelectionModel, details: GridCallbackDetails<any>) => {
  // שמירת המזהה של השורה שנבחרה
  if (rowSelectionModel.length > 0) {
    const selectedId = rowSelectionModel[0]; // בוחרת את השורה הראשונה
    const rowData = employees.find((row) => row.id === selectedId);
    setSelectedEmployee(rowData || null);
  } else {
    setSelectedEmployee(null); // במידה ולא נבחרה שורה
  }
};

// // פונקציות לטיפול בכפתורים
// const handleEdit = (emp: Employee) => {
//   select(emp);
//   // כאן תוכל להוסיף לוגיקה לפתיחת עורך עבור אותו עובד
// };

// const handleDelete = (id: string) => {
//   onDeleted(id);
//   // כאן תוכל להוסיף לוגיקה למחיקת העובד
// };

  return (
    <div className="employee-list">
      <DataGrid rows={employees} 
      columns={columns} 
      onRowSelectionModelChange={handleRowSelection} // אירוע מעקב אחרי בחירה

      />
      {selectedEmployee ?   
      <EmployeeForm
          employee={selectedEmployee}
          readOnly={true}
        /> : null
      }
      {/* <ul >
        {employees.map((employee) => (
          <li  key={employee.id}   className={`employee-item ${
            employee.id === selectedEmployeeId ? "selected" : ""
          }`}
        >
            <span>
              {employee.name} ({employee.email})
            </span>
            <div className="actions">
            <button onClick={() => select(employee)}>Edit</button>
            <button onClick={() => onDeleted(employee.id)}>Delete</button>
            </div>
          </li>
        ))}
      </ul> */}
    </div>
  );
};

export default EmployeeList;
