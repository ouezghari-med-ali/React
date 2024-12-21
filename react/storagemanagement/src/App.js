import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Login from './Login';
import ItemsTable from './ItemsTable';
import ItemForm from './ItemForm';
import { AuthProvider } from './AuthProvider';
import ProtectedRoute from './ProtectedRoute';
import './GlobalStyles.css';

function App() {
    return (
        <AuthProvider>
            <Router>
                <Routes>
                    <Route path="/" element={<Login />} />
                    <Route
                        path="/items-table"
                        element={
                            <ProtectedRoute>
                                <ItemsTable />
                            </ProtectedRoute>
                        }
                    />
                    <Route
                        path="/item-form"
                        element={
                            <ProtectedRoute>
                                <ItemForm />
                            </ProtectedRoute>
                        }
                    />
                </Routes>
            </Router>
        </AuthProvider>
    );
}

export default App;
