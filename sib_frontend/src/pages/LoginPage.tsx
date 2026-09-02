import { useState, type FormEvent } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import logoSb from '../assets/logo-sb.png'

function LoginPage() {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [cargando, setCargando] = useState(false)

  const { login } = useAuth()
  const navigate = useNavigate()

  async function manejarEnvio(evento: FormEvent) {
    evento.preventDefault()
    setError(null)
    setCargando(true)

    try {
      await login(username, password)
      navigate('/')
    } catch {
      setError('Usuario o contraseña incorrectos.')
    } finally {
      setCargando(false)
    }
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-[rgba(237,240,247,1)]">
      <form
        onSubmit={manejarEnvio}
        className="bg-white rounded-2xl shadow-sm p-8 w-full max-w-sm"
      >
        <img src={logoSb} alt="Superintendencia de Bancos" className="w-28 mx-auto mb-6" />

        <label className="block text-sm font-medium text-gray-700 mb-1">Usuario</label>
        <input
          type="text"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          className="w-full border border-gray-300 rounded-md px-3 py-2 mb-4 text-sm"
          required
        />

        <label className="block text-sm font-medium text-gray-700 mb-1">Contraseña</label>
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          className="w-full border border-gray-300 rounded-md px-3 py-2 mb-4 text-sm"
          required
        />

        {error && <p className="text-red-600 text-sm mb-4">{error}</p>}

        <button
          type="submit"
          disabled={cargando}
          className="w-full bg-[rgba(13,48,72,0.9)] text-white rounded-md py-2 text-sm font-medium disabled:opacity-50"
        >
          {cargando ? 'Ingresando...' : 'Ingresar'}
        </button>
      </form>
    </div>
  )
}

export default LoginPage