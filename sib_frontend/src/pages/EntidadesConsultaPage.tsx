import { useEffect, useMemo, useState } from 'react'
import Layout from '../components/Layout'
import api from '../services/api'
import type { EntidadGubernamental } from '../types/entidadGubernamental'

const FILAS_POR_PAGINA = 20

function EntidadesConsultaPage() {
  const [entidades, setEntidades] = useState<EntidadGubernamental[]>([])
  const [busqueda, setBusqueda] = useState('')
  const [pagina, setPagina] = useState(1)
  const [cargando, setCargando] = useState(true)
  const [errorCarga, setErrorCarga] = useState<string | null>(null)

  useEffect(() => {
    api.get<EntidadGubernamental[]>('/entidadesgubernamentales')
      .then((respuesta) => setEntidades(respuesta.data))
      .catch((error) => {
        console.error(error)
        setErrorCarga('No se pudieron cargar las entidades gubernamentales.')
      })
      .finally(() => setCargando(false))
  }, [])

  const filtradas = useMemo(() => {
    if (!busqueda.trim()) return entidades
    const termino = busqueda.trim().toLowerCase()
    return entidades.filter((e) => e.nombre.toLowerCase().includes(termino))
  }, [entidades, busqueda])

  const totalPaginas = Math.max(1, Math.ceil(filtradas.length / FILAS_POR_PAGINA))
  const paginaActual = Math.min(pagina, totalPaginas)
  const inicio = (paginaActual - 1) * FILAS_POR_PAGINA
  const filasVisibles = filtradas.slice(inicio, inicio + FILAS_POR_PAGINA)

  function cambiarBusqueda(valor: string) {
    setBusqueda(valor)
    setPagina(1)
  }

  async function eliminar(id: number) {
    if (!confirm('¿Eliminar esta entidad gubernamental?')) return
    await api.delete(`/entidadesgubernamentales/${id}`)
    setEntidades((anteriores) => anteriores.filter((e) => e.id !== id))
  }

  return (
    <Layout titulo="Consulta de Entidades Gubernamentales">
      <div className="flex items-center justify-between mb-6">
        <input
          type="text"
          placeholder="Buscar por nombre..."
          value={busqueda}
          onChange={(e) => cambiarBusqueda(e.target.value)}
          className="border border-gray-300 rounded-md px-3 py-2 text-sm w-full max-w-sm"
        />
        <span className="text-sm text-gray-500 ml-4 whitespace-nowrap">
          {filtradas.length} resultado{filtradas.length !== 1 ? 's' : ''}
        </span>
      </div>

      {errorCarga && <p className="text-red-600 text-sm mb-4">{errorCarga}</p>}

      {cargando ? (
        <p className="text-gray-500 text-sm">Cargando...</p>
      ) : (
        <>
          <div className="overflow-x-auto">
            <table className="w-full text-sm text-left">
              <thead>
                <tr className="border-b border-gray-200 text-gray-500">
                  <th className="py-2 pr-4">Nombre</th>
                  <th className="py-2 pr-4">Categoría</th>
                  <th className="py-2 pr-4">Poder del Estado</th>
                  <th className="py-2 pr-4">Sector</th>
                  <th className="py-2 pr-4"></th>
                </tr>
              </thead>
              <tbody>
                {filasVisibles.map((entidad) => (
                  <tr key={entidad.id} className="border-b border-gray-100">
                    <td className="py-2 pr-4">{entidad.nombre}</td>
                    <td className="py-2 pr-4">{entidad.categoria}</td>
                    <td className="py-2 pr-4">{entidad.poderDelEstado}</td>
                    <td className="py-2 pr-4">{entidad.sector}</td>
                    <td className="py-2 pr-4 text-right">
                      <button
                        onClick={() => eliminar(entidad.id)}
                        className="text-red-600 text-xs font-medium hover:underline"
                      >
                        Eliminar
                      </button>
                    </td>
                  </tr>
                ))}
                {filasVisibles.length === 0 && (
                  <tr>
                    <td colSpan={5} className="py-6 text-center text-gray-400">
                      No se encontraron entidades.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>

          <div className="flex items-center justify-between mt-4">
            <button
              onClick={() => setPagina((p) => Math.max(1, p - 1))}
              disabled={paginaActual === 1}
              className="text-sm px-3 py-1.5 rounded-md border border-gray-300 disabled:opacity-40"
            >
              Anterior
            </button>
            <span className="text-sm text-gray-500">
              Página {paginaActual} de {totalPaginas}
            </span>
            <button
              onClick={() => setPagina((p) => Math.min(totalPaginas, p + 1))}
              disabled={paginaActual === totalPaginas}
              className="text-sm px-3 py-1.5 rounded-md border border-gray-300 disabled:opacity-40"
            >
              Siguiente
            </button>
          </div>
        </>
      )}
    </Layout>
  )
}

export default EntidadesConsultaPage
