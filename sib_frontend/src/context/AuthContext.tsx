import { createContext, useContext, useState, type ReactNode } from 'react'
import api from '../services/api'

type AuthContextType = {
  token: string | null
  login: (username: string, password: string) => Promise<void>
  logout: () => void
}

const AuthContext = createContext<AuthContextType | undefined>(undefined)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [token, setToken] = useState<string | null>(localStorage.getItem('token'))

  async function login(username: string, password: string) {
    const respuesta = await api.post('/auth/login', { username, password })
    const nuevoToken = respuesta.data.token as string
    localStorage.setItem('token', nuevoToken)
    setToken(nuevoToken)
  }

  function logout() {
    localStorage.removeItem('token')
    setToken(null)
  }

  return (
    <AuthContext.Provider value={{ token, login, logout }}>
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  const contexto = useContext(AuthContext)
  if (!contexto) {
    throw new Error('useAuth debe usarse dentro de AuthProvider')
  }
  return contexto
}