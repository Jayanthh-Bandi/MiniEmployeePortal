import { useEffect, useState } from "react";
import { apiFetch } from "../services/api";

function DepartmentManager({ isAdmin }) {
    const [departments, setDepartments] = useState([]);
    const [departmentId, setDepartmentId] = useState("");
    const [departmentName, setDepartmentName] = useState("");
    const [editingId, setEditingId] = useState(null);

    const [message, setMessage] = useState("");
    const [error, setError] = useState("");

    const loadDepartments = async () => {
        setError("");

        try {
            const data =
                await apiFetch("/Departments");

            setDepartments(data || []);
        } catch (error) {
            setError(error.message);
        }
    };

    useEffect(() => {
        loadDepartments();
    }, []);

    const findDepartment = async () => {
        if (!departmentId) {
            setError("Enter a department ID.");
            return;
        }

        try {
            const data =
                await apiFetch(
                    `/Departments/${departmentId}`
                );

            setEditingId(data.departmentId);
            setDepartmentName(
                data.departmentName
            );

            setMessage("Department loaded.");
        } catch (error) {
            setError(error.message);
        }
    };

    const saveDepartment = async (event) => {
        event.preventDefault();

        setMessage("");
        setError("");

        try {
            const payload = {
                departmentName
            };

            if (editingId !== null) {
                await apiFetch(
                    `/Departments/${editingId}`,
                    {
                        method: "PUT",
                        body: JSON.stringify(payload)
                    }
                );

                setMessage(
                    "Department updated successfully."
                );
            } else {
                await apiFetch(
                    "/Departments",
                    {
                        method: "POST",
                        body: JSON.stringify(payload)
                    }
                );

                setMessage(
                    "Department created successfully."
                );
            }

            setEditingId(null);
            setDepartmentName("");
            await loadDepartments();
        } catch (error) {
            setError(error.message);
        }
    };

    return (
        <div className="content-card">
            <div className="section-header">
                <div>
                    <h2>Departments</h2>
                    <p>
                        Organization departments.
                    </p>
                </div>

                <button
                    className="secondary-btn"
                    onClick={loadDepartments}
                >
                    Refresh
                </button>
            </div>

            {error && (
                <div className="error-message">
                    {error}
                </div>
            )}

            {message && (
                <div className="success-message">
                    {message}
                </div>
            )}

            <div className="toolbar">
                <input
                    type="number"
                    placeholder="Department ID"
                    value={departmentId}
                    onChange={(e) =>
                        setDepartmentId(e.target.value)
                    }
                />

                <button
                    className="secondary-btn"
                    onClick={findDepartment}
                >
                    Find
                </button>
            </div>

            {isAdmin && (
                <form
                    className="data-form"
                    onSubmit={saveDepartment}
                >
                    <h3>
                        {editingId !== null
                            ? "Update Department"
                            : "Add Department"}
                    </h3>

                    <input
                        value={departmentName}
                        onChange={(e) =>
                            setDepartmentName(
                                e.target.value
                            )
                        }
                        placeholder="Department name"
                        required
                    />

                    <div className="button-row">
                        <button className="primary-btn">
                            {editingId !== null
                                ? "Update Department"
                                : "Create Department"}
                        </button>

                        {editingId !== null && (
                            <button
                                type="button"
                                className="secondary-btn"
                                onClick={() => {
                                    setEditingId(null);
                                    setDepartmentName("");
                                }}
                            >
                                Cancel
                            </button>
                        )}
                    </div>
                </form>
            )}

            <div className="department-grid">
                {departments.length === 0 ? (
                    <p>No departments found.</p>
                ) : (
                    departments.map(
                        (department) => (
                            <div
                                className="department-card"
                                key={
                                    department.Id
                                }
                            >
                                <span>
                                    #
                                    {
                                        department.Id
                                    }
                                </span>

                                <h3>
                                    {
                                        department.departmentName
                                    }
                                </h3>
                            </div>
                        )
                    )
                )}
            </div>
        </div>
    );
}
const deleteDepartment = async (id) => {
    if (!window.confirm(
        "Delete this department?"
    )) {
        return;
    }

    try {
        await apiFetch(
            `/Departments/${id}`,
            {
                method: "DELETE"
            }
        );

        setMessage(
            "Department deleted successfully."
        );

        await loadDepartments();
    } catch (error) {
        setError(error.message);
    }
};

export default DepartmentManager;