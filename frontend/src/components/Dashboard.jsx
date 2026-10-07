import { useEffect, useState } from "react";
import EmployeeManager from "./EmployeeManager";
import DepartmentManager from "./DepartmentManager";
import UserManager from "./UserManager";
import { apiFetch } from "../services/api";

function Dashboard({
    user,
    activeTab,
    setActiveTab,
    onLogout
}) {
    const isAdmin =
        user.role?.toLowerCase() === "admin";

    return (
        <div className="dashboard">
            <aside className="sidebar">
                <div className="sidebar-brand">
                    MEP
                    <span>Employee Portal</span>
                </div>

                <div className="user-mini">
                    <div className="avatar">
                        {user.userName
                            ?.charAt(0)
                            ?.toUpperCase()}
                    </div>

                    <div>
                        <strong>{user.userName}</strong>
                        <small>{user.role}</small>
                    </div>
                </div>

                <nav>
                    <button
                        className={
                            activeTab === "overview"
                                ? "nav-item active"
                                : "nav-item"
                        }
                        onClick={() =>
                            setActiveTab("overview")
                        }
                    >
                        Dashboard
                    </button>

                    <button
                        className={
                            activeTab === "employees"
                                ? "nav-item active"
                                : "nav-item"
                        }
                        onClick={() =>
                            setActiveTab("employees")
                        }
                    >
                        Employees
                    </button>

                    <button
                        className={
                            activeTab === "departments"
                                ? "nav-item active"
                                : "nav-item"
                        }
                        onClick={() =>
                            setActiveTab("departments")
                        }
                    >
                        Departments
                    </button>

                    {isAdmin && (
                        <button
                            className={
                                activeTab === "users"
                                    ? "nav-item active"
                                    : "nav-item"
                            }
                            onClick={() =>
                                setActiveTab("users")
                            }
                        >
                            User Management
                        </button>
                    )}
                </nav>

                <button
                    className="logout-btn"
                    onClick={onLogout}
                >
                    Logout
                </button>
            </aside>

            <main className="main-content">
                <header className="topbar">
                    <div>
                        <h1>
                            {activeTab === "overview"
                                ? "Dashboard"
                                : activeTab === "users"
                                ? "User Management"
                                : activeTab}
                        </h1>

                        <p>
                            Welcome back, {user.userName}
                        </p>
                    </div>

                    <span
                        className={
                            isAdmin
                                ? "role-badge admin"
                                : "role-badge employee"
                        }
                    >
                        {user.role}
                    </span>
                </header>

                {activeTab === "overview" && (
                    <Overview user={user} />
                )}

                {activeTab === "employees" && (
                    <EmployeeManager
                        isAdmin={isAdmin}
                    />
                )}

                {activeTab === "departments" && (
                    <DepartmentManager
                        isAdmin={isAdmin}
                    />
                )}

                {activeTab === "users" && isAdmin && (
                    <UserManager />
                )}
            </main>
        </div>
    );
}

function Overview({ user }) {
    const [employee, setEmployee] = useState(null);
    const [error, setError] = useState("");

    useEffect(() => {
        const loadProfile = async () => {
            try {
                const data =
                    await apiFetch("/Employees/me");

                setEmployee(data);
            } catch (error) {
                setError(error.message);
            }
        };

        loadProfile();
    }, []);

    return (
        <>
            <div className="welcome-panel">
                <p className="eyebrow">
                    AUTHENTICATED SESSION
                </p>

                <h2>
                    Welcome, {user.userName}
                </h2>

                <p>
                    You are logged in as{" "}
                    <strong>{user.role}</strong>.
                </p>
            </div>

            <div className="stats-grid">
                <div className="stat-card">
                    <span>USER ID</span>
                    <strong>{user.userId}</strong>
                </div>

                <div className="stat-card">
                    <span>EMAIL</span>
                    <strong>{user.email}</strong>
                </div>

                <div className="stat-card">
                    <span>ROLE</span>
                    <strong>{user.role}</strong>
                </div>
            </div>

            {employee && (
                <div className="content-card profile-card">
                    <div className="section-header">
                        <div>
                            <h2>My Employee Profile</h2>
                            <p>
                                Employee information linked
                                to your authenticated account.
                            </p>
                        </div>
                    </div>

                    <div className="stats-grid">
                        <div className="stat-card">
                            <span>EMPLOYEE ID</span>
                            <strong>
                                {employee.employeeId}
                            </strong>
                        </div>

                        <div className="stat-card">
                            <span>NAME</span>
                            <strong>
                                {employee.name}
                            </strong>
                        </div>

                        <div className="stat-card">
                            <span>DEPARTMENT</span>
                            <strong>
                                {employee.departmentName}
                            </strong>
                        </div>

                        <div className="stat-card">
                            <span>SALARY</span>
                            <strong>
                                ₹ {employee.salary}
                            </strong>
                        </div>
                    </div>
                </div>
            )}

            {error && (
                <div className="error-message">
                    {error}
                </div>
            )}
        </>
    );
}

export default Dashboard;