import { useEffect, useState } from "react";
import Login from "./components/Login";
import Register from "./components/Register";
import Dashboard from "./components/Dashboard";
import { apiFetch } from "./services/api";
import "./App.css";

function App() {
    const [user, setUser] = useState(null);
    const [showRegister, setShowRegister] = useState(false);
    const [activeTab, setActiveTab] = useState("overview");
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const token = sessionStorage.getItem("token");

        if (!token) {
            setLoading(false);
            return;
        }

        const restoreSession = async () => {
            try {
                const currentUser =
                    await apiFetch("/Auth/me");

                setUser({
                    userId: Number(currentUser.userId),
                    userName: currentUser.userName,
                    email: currentUser.email,
                    role: currentUser.role
                });
            } catch {
                sessionStorage.removeItem("token");
                setUser(null);
            } finally {
                setLoading(false);
            }
        };

        restoreSession();
    }, []);

    const handleLoginSuccess = (data) => {
        setUser(data);
        setActiveTab("overview");
    };

    const handleLogout = () => {
        sessionStorage.removeItem("token");
        setUser(null);
        setActiveTab("overview");
    };

    if (loading) {
        return (
            <div className="loading-screen">
                Loading application...
            </div>
        );
    }

    if (!user) {
        return (
            <div className="auth-page">
                {showRegister ? (
                    <Register
                        onRegistered={() =>
                            setShowRegister(false)
                        }
                        onSwitchLogin={() =>
                            setShowRegister(false)
                        }
                    />
                ) : (
                    <Login
                        onLoginSuccess={
                            handleLoginSuccess
                        }
                        onSwitchRegister={() =>
                            setShowRegister(true)
                        }
                    />
                )}
            </div>
        );
    }

    return (
        <Dashboard
            user={user}
            activeTab={activeTab}
            setActiveTab={setActiveTab}
            onLogout={handleLogout}
        />
    );
}

export default App;