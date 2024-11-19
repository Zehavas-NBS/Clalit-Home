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
      const confirmDelete = window.confirm("Are you sure you want to delete this employee?");
    if (!confirmDelete) {
      return; // Exit if user cancels the confirmation dialog
    }
      setSelectedEmployee(employee);
      onDelete(employee.id);
    }
const columns = [
  { field: 'fullName', headerName: 'Name', width: 200 },
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


  return (
    <div className="employee-list">
      <DataGrid rows={employees} 
      columns={columns} 
      onRowSelectionModelChange={handleRowSelection}

      />
      {   
      <EmployeeForm
          employee={selectedEmployee}
          readOnly={true}
        /> 
      }
    </div>
  );
};

export default EmployeeList;
