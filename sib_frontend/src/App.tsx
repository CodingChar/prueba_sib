import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import Layout from './components/Layout'

function InicioPage() {
  return (
    <Layout titulo="Inicio">
      <p className="text-gray-600">Bienvenido al sistema de Gestión de Pagos.</p>
    </Layout>
  )
}

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<InicioPage />} />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  )
}

export default App