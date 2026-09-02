import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { AuthProvider, useAuth } from "./context/AuthContext";
import Layout from "./components/Layout";
import LoginPage from "./pages/LoginPage";
import type { ReactNode } from "react";
import EntidadesConsultaPage from './pages/EntidadesConsultaPage'
import EntidadesCrearPage from './pages/EntidadesCrearPage'

import EmpleadosConsultaPage from "./pages/EmpleadosConsultaPage";
import EmpleadosCrearPage from "./pages/EmpleadosCrearPage";

function RutaProtegida({ children }: { children: ReactNode }) {
  const { token } = useAuth();
  if (!token) {
    return <Navigate to="/login" replace />;
  }
  return <>{children}</>;
}

function InicioPage() {
  return (
    <Layout titulo="Inicio">
      <p className="text-gray-600">
        Bienvenido al sistema de Gestión de Pagos.
      </p>
    </Layout>
  );
}

function AppRoutes() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route
        path="/"
        element={
          <RutaProtegida>
            <InicioPage />
          </RutaProtegida>
        }
      />
      <Route path="*" element={<Navigate to="/" replace />} />
      <Route
        path="/empleados"
        element={
          <RutaProtegida>
            <EmpleadosConsultaPage />
          </RutaProtegida>
        }
      />
      <Route
        path="/empleados/nuevo"
        element={
          <RutaProtegida>
            <EmpleadosCrearPage />
          </RutaProtegida>
        }
      />
      <Route
  path="/entidades-gubernamentales"
  element={
    <RutaProtegida>
      <EntidadesConsultaPage />
    </RutaProtegida>
  }
/>
<Route
  path="/entidades-gubernamentales/nueva"
  element={
    <RutaProtegida>
      <EntidadesCrearPage />
    </RutaProtegida>
  }
/>
    </Routes>
  );
}

function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <AppRoutes />
      </AuthProvider>
    </BrowserRouter>
  );
}

export default App;
