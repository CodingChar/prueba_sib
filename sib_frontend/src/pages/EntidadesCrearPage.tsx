import { useState, type FormEvent } from 'react'
import { useNavigate } from 'react-router-dom'
import Layout from '../components/Layout'
import api from '../services/api'

function EntidadesCrearPage() {
  const [nombre, setNombre] = useState('')
  const [categoria, setCategoria] = useState('')
  const [poderDelEstado, setPoderDelEstado] = useState('')
  const [sector, setSector] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [enviando, setEnviando] = useState(false)
  const navigate = useNavigate()

  async function manejarEnvio(evento: FormEvent) {
    evento.preventDefault()
    setError(null)
    setEnviando(true)

    try {
      await api.post('/entidadesgubernamentales', {
        nombre,
        categoria,
        poderDelEstado,
        sector,
      })
      navigate('/entidades-gubernamentales')
    } catch {
      setError('No se pudo crear la entidad gubernamental.')
    } finally {
      setEnviando(false)
    }
  }

  return (
    <Layout titulo="Crear entidad gubernamental">
      <form onSubmit={manejarEnvio} className="max-w-lg">
        <label className="block text-sm font-medium text-gray-700 mb-1">Nombre</label>
        <input
          type="text"
          value={nombre}
          onChange={(e) => setNombre(e.target.value)}
          required
          className="w-full border border-gray-300 rounded-md px-3 py-2 mb-4 text-sm"
        />

        <label className="block text-sm font-medium text-gray-700 mb-1">Categoría</label>
        <input
          type="text"
          value={categoria}
          onChange={(e) => setCategoria(e.target.value)}
          required
          className="w-full border border-gray-300 rounded-md px-3 py-2 mb-4 text-sm"
        />

        <label className="block text-sm font-medium text-gray-700 mb-1">Poder del Estado</label>
        <input
          type="text"
          value={poderDelEstado}
          onChange={(e) => setPoderDelEstado(e.target.value)}
          required
          className="w-full border border-gray-300 rounded-md px-3 py-2 mb-4 text-sm"
        />

        <label className="block text-sm font-medium text-gray-700 mb-1">Sector</label>
        <input
          type="text"
          value={sector}
          onChange={(e) => setSector(e.target.value)}
          required
          className="w-full border border-gray-300 rounded-md px-3 py-2 mb-4 text-sm"
        />

        {error && <p className="text-red-600 text-sm mb-4">{error}</p>}

        <button
          type="submit"
          disabled={enviando}
          className="bg-[rgba(13,48,72,0.9)] text-white rounded-md px-5 py-2 text-sm font-medium disabled:opacity-50"
        >
          {enviando ? 'Guardando...' : 'Guardar entidad'}
        </button>
      </form>
    </Layout>
  )
}

export default EntidadesCrearPage