import React, { useState, useEffect } from "react";
import { Employee } from "../../types";
import "./EmployeeForm.css";


// interface EmployeeFormProps {
//   employee?: Employee;
//   readOnly: boolean;
//   onSave: (employee: Employee, isNew : boolean) => void;
//   onCancel: () => void;
// }

const EmployeeForm=  ({ employee, onSave, onCancel , readOnly} : any) => {
  const [fullName, setFullName] = useState<string>(employee?.fullName || "");
  const [email, setEmail] = useState<string>(employee?.email || "");
  const [isNew] = useState<boolean>(employee?.id === undefined);

  useEffect(() => {
    if (employee) {
      setFullName(employee.fullName ?? "");
      setEmail(employee.email ?? "");
    }
  }, [employee]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const a = {...employee, fullName, email, id: employee?.id || ""};
    console.log(a);
    console.log(employee);
    onSave({ ...employee, fullName, email, id: employee?.id || "" }, isNew);
  };

  return (
    <form className="employee-form" onSubmit={handleSubmit} >
      {
      !readOnly?  <h2>{employee ? "Edit Employee" : "Add New Employee"}</h2> : <h2>{"Employee Details"}</h2>
      }
      <label>
        Name:
        <input
          type="text"
          
          value={fullName}
          onChange={(e) => setFullName(e.target.value)}
          readOnly={readOnly} // במצב קריאה בלבד, השדה לא ניתן לעריכה
          required={!readOnly}
        />
      </label>
      <label>
        Email:
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          readOnly={readOnly} // במצב קריאה בלבד, השדה לא ניתן לעריכה
          required={!readOnly}
        />
      </label>
      {!readOnly && <div className="form-buttons">

        <button type="submit">Save</button>
        <button type="button" onClick={onCancel}>
          Cancel
        </button>
      </div>
  }
    </form>
  );
};

export default EmployeeForm;
